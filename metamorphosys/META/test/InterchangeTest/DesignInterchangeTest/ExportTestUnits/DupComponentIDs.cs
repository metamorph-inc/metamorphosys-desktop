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
using GME.CSharp;
using GME.MGA;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using CyPhyGUIs;

namespace DesignExporterUnitTests
{
    public class DupComponentIDsFixture : IDisposable
    {
        public static String PathTest = Path.Combine(META.VersionInfo.MetaPath,
                                                     "test",
                                                     "InterchangeTest",
                                                     "DesignInterchangeTest",
                                                     "ExportTestModels",
                                                     "DupComponentIDs");

        public String pathXME = Path.Combine(PathTest, "DupComponentIDs.xme");

        public DupComponentIDsFixture()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(pathXME, out mgaConnectionString);

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
    }

    public class DupComponentIDs : IUseFixture<DupComponentIDsFixture>
    {
        #region Fixture
        DupComponentIDsFixture fixture;
        public void SetFixture(DupComponentIDsFixture data)
        {
            fixture = data;
        }
        #endregion

        private MgaProject proj { get { return fixture.proj; } }

        [Fact]
        public void DupConnectorIds()
        {
            String pathCA = "ComponentAssemblies/DupConnectorIds";

            Xunit.Assert.Throws(typeof(ApplicationException), () =>
            {
                avm.Design design = Convert(pathCA);
            });
        }

        [Fact]
        public void DupPortIds()
        {
            String pathCA = "ComponentAssemblies/DupPortIds";

            Xunit.Assert.Throws(typeof(ApplicationException), () =>
            {
                avm.Design design = Convert(pathCA);
            });
        }

        private avm.Design Convert(String pathDE)
        {
            MgaObject objDE = null;
            proj.PerformInTransaction(delegate
            {
                objDE = proj.get_ObjectByPath(pathDE);
            });
            Assert.NotNull(objDE);

            var interp = new CyPhyDesignExporter.CyPhyDesignExporterInterpreter();
            interp.Initialize(proj);
            InterpreterMainParameters param = new InterpreterMainParameters()
            {
                OutputDirectory = DupComponentIDsFixture.PathTest,
                CurrentFCO = objDE as MgaFCO,
                Project = proj
            };
            var result = interp.Main(param);
            Assert.True(result.Success);

            // Load the new .adm file
            var pathAdm = Path.Combine(DupComponentIDsFixture.PathTest,
                                       pathDE.Split('/').Last() + ".adm");
            var xml = File.ReadAllText(pathAdm);
            var design = XSD2CSharp.AvmXmlSerializer.Deserialize<avm.Design>(xml);
            Assert.NotNull(design);

            return design;
        }
    }
}
