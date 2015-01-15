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
using GME.MGA;
using System.Diagnostics;
using System.IO;

namespace DesignImporterTests
{
    public class TonkaPortsFixture : DesignImporterTestFixtureBase
    {
        public override string pathXME
        {
            get { return "Ports\\TonkaPorts.xme"; }
        }
    }

    public class TonkaPortsRoundTrip : PortsRoundTripBase<TonkaPortsFixture>, IUseFixture<TonkaPortsFixture>
    {
        [Fact]
        public void ComponentAssembly_Tonka()
        {
            string asmName = "TonkaComponentAssembly";
            RunRoundTrip(asmName);
        }
    }

    public class PortsFixture : DesignImporterTestFixtureBase
    {
        public override String pathXME
        {
            get { return "Ports\\Ports.xme"; }
        }
    }

    public class PortsRoundTrip : PortsRoundTripBase<PortsFixture>, IUseFixture<PortsFixture>
    {
        [Fact]
        public void ComponentAssembly()
        {
            string asmName = "ComponentAssembly";
            RunRoundTrip(asmName);
        }

        [Fact]
        public void ComponentAssemblyWithCAD()
        {
            string asmName = "ComponentAssemblyWithCAD";
            RunRoundTrip(asmName);
        }
    }

    public abstract class PortsRoundTripBase<T> where T : DesignImporterTestFixtureBase
    {
        protected void RunRoundTrip(string asmName)
        {
            File.Delete(Path.Combine(fixture.AdmPath, asmName + ".adm"));
            RunDesignExporter(asmName);
            CopyMgaAndRunDesignImporter(asmName);
            ComponentExporterUnitTests.Tests.runCyPhyMLComparator(proj.ProjectConnStr.Substring("MGA=".Length),
                (proj.ProjectConnStr + asmName + ".mga").Substring("MGA=".Length), Environment.CurrentDirectory);
        }

        protected void CopyMgaAndRunDesignImporter(string asmName)
        {
            //proj.Save(proj.ProjectConnStr + asmName + ".mga", true);
            File.Copy(proj.ProjectConnStr.Substring("MGA=".Length), (proj.ProjectConnStr + asmName + ".mga").Substring("MGA=".Length), true);

            MgaProject proj2 = (MgaProject)Activator.CreateInstance(Type.GetTypeFromProgID("Mga.MgaProject"));
            proj2.OpenEx(proj.ProjectConnStr + asmName + ".mga", "CyPhyML", null);
            proj2.BeginTransactionInNewTerr();
            try
            {
                MgaFCO componentAssembly = (MgaFCO)proj2.RootFolder.GetObjectByPathDisp("/@ComponentAssemblies/@" + asmName);
                Assert.NotNull(componentAssembly);
                componentAssembly.DestroyObject();
                var importer = new CyPhyDesignImporter.AVMDesignImporter(null, proj2);
                avm.Design design;
                using (StreamReader streamReader = new StreamReader(Path.Combine(fixture.AdmPath, asmName + ".adm")))
                    design = CyPhyDesignImporter.CyPhyDesignImporterInterpreter.DeserializeAvmDesignXml(streamReader);
                var ret = (ISIS.GME.Dsml.CyPhyML.Interfaces.ComponentAssembly)importer.ImportDesign(design, CyPhyDesignImporter.AVMDesignImporter.DesignImportMode.CREATE_CAS);
            }
            finally
            {
                proj2.CommitTransaction();
                if (Debugger.IsAttached)
                {
                    proj2.Save(null, true);
                }
                proj2.Close(true);
            }
        }

        protected MgaFCO RunDesignExporter(string asmName)
        {
            MgaFCO componentAssembly;
            proj.BeginTransactionInNewTerr();
            try
            {
                componentAssembly = (MgaFCO)proj.RootFolder.GetObjectByPathDisp("/@ComponentAssemblies/@" + asmName);
                Assert.NotNull(componentAssembly);
                var designExporter = new CyPhyDesignExporter.CyPhyDesignExporterInterpreter();
                designExporter.Initialize(proj);

                var parameters = new CyPhyGUIs.InterpreterMainParameters()
                {
                    CurrentFCO = componentAssembly,
                    Project = proj,
                    OutputDirectory = fixture.AdmPath
                };

                designExporter.MainInTransaction(parameters);

            }
            finally
            {
                proj.AbortTransaction();
            }

            Assert.True(File.Exists(Path.Combine(fixture.AdmPath, asmName + ".adm")));
            return componentAssembly;
        }

        MgaProject proj { get { return fixture.proj; } }
        T fixture;
        public void SetFixture(T data)
        {
            fixture = data;
        }
    }
}
