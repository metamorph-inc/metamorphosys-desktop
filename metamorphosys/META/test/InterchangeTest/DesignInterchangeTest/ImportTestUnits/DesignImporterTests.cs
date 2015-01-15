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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xunit;
using System.IO;
using GME.MGA;
using Xunit.Sdk;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;

namespace DesignImporterTests
{
    public abstract class DesignImporterTestFixtureBase : IDisposable
    {
        public static String PathTest = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring("file:///".Length)),
            @"..\..\");

        public String AdmPath
        {
            get { return Path.GetDirectoryName(proj.ProjectConnStr.Substring(("MGA=".Length))); }
        }

        public DesignImporterTestFixtureBase()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(Path.Combine(PathTest, pathXME), out mgaConnectionString);

            proj = new MgaProject();
            bool ro_mode;
            proj.Open(mgaConnectionString, out ro_mode);
            proj.EnableAutoAddOns(true);
        }

        public void Dispose()
        {
            proj.Save();
            proj.Close();
        }

        public MgaProject proj { get; private set; }

        public abstract String pathXME { get; }
    }


    public class RollingWheelFixture : DesignImporterTestFixtureBase
    {
        public override String pathXME
        {
            get { return "RollingWheel.xme"; }
        }
    }


    public class ImportWebGMEDesigns : IUseFixture<RollingWheelFixture>
    {
        [STAThread]
        public static int Main(string[] args)
        {
            int ret = Xunit.ConsoleClient.Program.Main(new string[]
            {
                Assembly.GetExecutingAssembly().CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
            return ret;
        }

        //[Fact]
        public void TestRollingWheelWithFormula()
        {
            var admFilename = "WheelWithFormula.adm";
            avm.Design design;
            using (StreamReader streamReader = new StreamReader(Path.Combine(fixture.AdmPath, admFilename)))
                design = CyPhyDesignImporter.CyPhyDesignImporterInterpreter.DeserializeAvmDesignXml(streamReader);

            Assert.True(0 < design.RootContainer.Property.Count);

            proj.BeginTransactionInNewTerr();
            try
            {
                var importer = new CyPhyDesignImporter.AVMDesignImporter(null, proj);
                var ret = (CyPhy.DesignContainer)importer.ImportDesign(design, CyPhyDesignImporter.AVMDesignImporter.DesignImportMode.CREATE_DS);

                Xunit.Assert.Equal(3, ret.Children.PropertyCollection.Count());
            }
            finally
            {
                proj.CommitTransaction();
                if (Debugger.IsAttached)
                {
                    proj.Save(proj.ProjectConnStr + admFilename + ".mga", true);
                }
            }
        }

        private IMgaProject proj { get { return fixture.proj; } }

        private RollingWheelFixture fixture;
        public void SetFixture(RollingWheelFixture data)
        {
            fixture = data;
        }
    }

}
