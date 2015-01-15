/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.IO;
using GME.MGA;

namespace SubTreeMergeTest
{
    public class SubTreeMergeModelFixture
    {
        internal string mgaFile = null;

        public SubTreeMergeModelFixture()
        {
            this.mgaFile = ImportXME2Mga();
        }

        public static string ImportXME2Mga()
        {
            var xmePath = Path.Combine("..", "..", "SubTreeMergeModel.xme");
            string projectConnStr;
            MgaUtils.ImportXMEForTest(xmePath, out projectConnStr);
            return projectConnStr.Substring("MGA=".Length);
        }
    }

    public class SubTreeMergeTest : IUseFixture<SubTreeMergeModelFixture>
    {
        [STAThread]
        static void Main(string[] args)
        {
            int ret = Xunit.ConsoleClient.Program.Main(new string[] {
                System.Reflection.Assembly.GetAssembly(typeof(SubTreeMergeTest)).CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
        }

        private string mgaFile { get { return this.fixture.mgaFile; } }
        private SubTreeMergeModelFixture fixture;

        public void SetFixture(SubTreeMergeModelFixture data)
        {
            this.fixture = data;
        }

        [Fact]
        void TestSubTreeMergeModel()
        {
            string mgaFileCopy = mgaFile + "copy.mga";
            File.Copy(mgaFile, mgaFile + "copy.mga", true);
            MgaProject project = new MgaProject();
            project.OpenEx("MGA=" + this.mgaFile, "CyPhyML", null);
            try
            {
                project.BeginTransactionInNewTerr(transactiontype_enum.TRANSACTION_NON_NESTED);
                try
                {
                    string mergeMeModelPath = "/@ComponentAssemblies/@MergeMe";
                    MgaFCO mergeMe = (MgaFCO)project.ObjectByPath[mergeMeModelPath];
                    int startingDescendantFCOs = ((MgaModel)mergeMe).GetDescendantFCOs(mergeMe.Project.CreateFilter()).Count;

                    var fcos = ((MgaModel)mergeMe).GetDescendantFCOs(project.CreateFilter()).Cast<MgaFCO>().Select(
                        x => x.AbsPath).ToList();

                    foreach (MgaObject obj in mergeMe.ChildObjects)
                    {
                        if (obj.MetaBase.Name != "Connector" && obj.MetaBase.Name != "Axis" /* keep these, since they have refport connections*/
                            && obj.Status == (int)objectstatus_enum.OBJECT_EXISTS /* connections may be deleted when their endpoints are */)
                        {
                            obj.DestroyObject();
                        }
                    }

                    var subTreeMerge = new SubTreeMerge.SubTreeMerge();
                    subTreeMerge.merge(mergeMe, mgaFileCopy);
                    Assert.Equal(SubTreeMerge.SubTreeMerge.Errors.NoError, subTreeMerge.exitStatus);
                    MgaFCO newMergeMe = (MgaFCO)project.ObjectByPath[mergeMeModelPath];

                    Assert.Equal(newMergeMe.ID, ((MgaReference)project.ObjectByPath["/@ComponentAssemblies/@AsmWithRef/@MergeMe"]).Referred.ID);
                    Assert.Equal(newMergeMe.ID, ((MgaReference)project.ObjectByPath["/@Testing/@TestBench/@MergeMe"]).Referred.ID);
                    Assert.Equal(2, ((MgaReference)project.ObjectByPath["/@ComponentAssemblies/@AsmWithRef/@MergeMe"]).UsedByConns.Count);
                    Assert.Equal(2, ((MgaReference)project.ObjectByPath["/@Testing/@TestBench/@MergeMe"]).UsedByConns.Count);
                    //Debugging:
                    //var fcos2 = ((MgaModel)newMergeMe).GetDescendantFCOs(project.CreateFilter()).Cast<MgaFCO>().Select(x => x.AbsPath)
                    //    .Where(x => fcos.Contains(x) == false);
                    //HashSet<string> originalObjects = new HashSet<string>();
                    //foreach (string abspath in fcos)
                    //    originalObjects.Add(abspath);
                    //foreach (string abspath in ((MgaModel)newMergeMe).GetDescendantFCOs(project.CreateFilter()).Cast<IMgaFCO>().Select(x => x.AbsPath))
                    //    originalObjects.Remove(abspath);
                    // Console.Out.WriteLine(string.Join("\n", originalObjects));
                    Assert.Equal(startingDescendantFCOs, ((MgaModel)newMergeMe).GetDescendantFCOs(project.CreateFilter()).Count);
                }
                finally
                {
                    project.CommitTransaction();
                }
            }
            finally
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    project.Save(project.ProjectConnStr + "_testoutput.mga", true);
                }
                project.Close(true);
            }
        }
    }
}
