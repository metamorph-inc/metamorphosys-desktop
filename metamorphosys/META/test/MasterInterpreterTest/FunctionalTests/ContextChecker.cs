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
using GME.MGA;
using System.IO;
using Xunit;

namespace MasterInterpreterTest.FunctionalTests
{

    public class ContextChecker
    {

        //[Fact]
        public void TestCodeSample()
        {
            string ProjectConnStr;
            MgaUtils.ImportXMEForTest(Path.GetFullPath(@"..\..\..\..\models\DynamicsTeam\MasterInterpreter\MasterInterpreter.xme"), out ProjectConnStr);

            MgaProject project = new MgaProject();
            bool ro_mode;
            project.Open(ProjectConnStr, out ro_mode);

            MgaHelper.CheckParadigmVersionUpgrade(project);

            try
            {
                List<IMgaFCO> objectsToCheck = null;

                project.BeginTransactionInNewTerr();
                try
                {
                    // discover objects

                    var allObjects = project
                        .RootFolder
                        .ChildFolders
                        .Cast<MgaFolder>()
                        .Where(x => x.Name.StartsWith("0"))
                        .SelectMany(x => x.GetDescendantFCOs(project.CreateFilter()).Cast<IMgaFCO>())
                        .Where(x => x.RootFCO == x);

                    // get all objects from folders starts with 0 within the root folder.
                    objectsToCheck = allObjects.Where(x => x.AbsPath.Contains("TestingContextChecker") && x.Name.Contains("ReadOnly") == false).ToList();

                    objectsToCheck.Sort((x, y) =>
                    {
                        return x.Meta.Name.CompareTo(y.Meta.Name) != 0 ?
                            x.Meta.Name.CompareTo(y.Meta.Name) :
                            x.AbsPath.CompareTo(y.AbsPath);
                    });
                }
                finally
                {
                    project.AbortTransaction();
                }

                Assert.True(objectsToCheck != null, "There are no object in the project that has to be checked.");

                int numContexts = objectsToCheck.Count;
                int numSuccess = 0;
                int numFailures = 0;

                bool success = true;

                foreach (var subject in objectsToCheck)
                {
                    // single test
                    using (var masterInterpreter = new CyPhyMasterInterpreter.CyPhyMasterInterpreterAPI(project))
                    {
                        CyPhyMasterInterpreter.Rules.ContextCheckerResult[] contextCheckerResults = null;

                        // check context
                        var checkerSuccess = masterInterpreter.TryCheckContext(subject as MgaModel, out contextCheckerResults);

                        List<CyPhyMasterInterpreter.Rules.ContextCheckerResult> sortedResults = contextCheckerResults.ToList();

                        // sort results Passed, Failed, then alphabetically based on message.
                        sortedResults.Sort((x, y) => { return x.Success == y.Success ? x.Message.CompareTo(y.Message) : y.Success.CompareTo(x.Success); });

                        project.BeginTransactionInNewTerr();
                        try
                        {
                            MgaObject parent = null;
                            GME.MGA.Meta.objtype_enum type;
                            subject.GetParent(out parent, out type);

                            var successExpected = parent.Name.ToLowerInvariant() == "invalid" ? false : true;

                            if (successExpected == checkerSuccess)
                            {
                                numSuccess++;
                                //GMEConsole.Info.WriteLine("OK");
                            }
                            else
                            {
                                success = false;

                                foreach (var result in sortedResults)
                                {
                                    TextWriter tw = null;
                                    StringBuilder sb = new StringBuilder();
                                    if (result.Success)
                                    {
                                        sb.Append("[Passed]");
                                        tw = Console.Out;
                                    }
                                    else
                                    {
                                        sb.Append("[Failed]");
                                        tw = Console.Error;
                                    }

                                    sb.AppendFormat(" {0} - {1} ", result.Subject.Name, result.Subject.AbsPath);

                                    sb.Append(result.Message);

                                    tw.WriteLine(sb);
                                }

                                numFailures++;
                                Console.Error.WriteLine("========= FAILED ==========");
                                Console.Error.WriteLine("= {0}", subject.Name);
                                Console.Error.WriteLine("= {0}", subject.AbsPath);
                                Console.Error.WriteLine("===========================");
                            }

                        }
                        finally
                        {
                            project.AbortTransaction();
                        }
                    }
                }

                if (success)
                {
                    Console.Out.WriteLine("[OK] Checked contexts: {0}, Success: {1}, Failed: {2}", numContexts, numSuccess, numFailures);
                }
                else
                {
                    Console.Error.WriteLine("[FAILED] Checked contexts: {0}, Success: {1}, Failed: {2}", numContexts, numSuccess, numFailures);
                }

                Assert.True(success, "At least one context was failed to check against the expected results.");

            }
            finally
            {
                project.Close(true);
            }



        }


    }
}
