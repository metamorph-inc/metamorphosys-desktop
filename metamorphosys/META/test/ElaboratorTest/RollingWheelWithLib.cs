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
using System.Reflection;
using Xunit;
using System.IO;
using System.Runtime.InteropServices;

namespace ElaboratorTest
{
    public class RollingWheelWithLibFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("..", "..", "..", "..", "models", "DynamicsTeam", "RollingWheelWithLib", "RollingWheelWithLib.xme"); }
        }
    }

    public class RollingWheelWithLibTest : IUseFixture<RollingWheelWithLibFixture>
    {
        //[STAThread]
        //static int Main(string[] args)
        //{
        //    int ret = Xunit.ConsoleClient.Program.Main(new string[] {
        //        Assembly.GetAssembly(typeof(Test)).CodeBase.Substring("file:///".Length),
        //        //"/noshadow",
        //    });
        //    Console.In.ReadLine();
        //    return ret;
        //}

        private string mgaFile { get { return this.fixture.mgaFile; } }
        private RollingWheelWithLibFixture fixture;

        public void SetFixture(RollingWheelWithLibFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "RollingWheelWithLib")]
        //[Trait("ProjectImport/Open", "RollingWheelWithLib")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        [Trait("ProjectImport/Open", "RollingWheelWithLib")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + this.mgaFile;
             
            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            MgaHelper.CheckParadigmVersionUpgrade(project);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }


        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void ComponentAssembly()
        {
            string objectAbsPath = "@CAWheel/@Wheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void TestBench()
        {
            string objectAbsPath = "/@TestBenches/@RollingWheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void LibraryComponentAssembly()
        {
            string objectAbsPath = "/@_Lib_/@CAWheel/@Wheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void LibraryTestBench()
        {
            string objectAbsPath = "/@_Lib_/@TestBenches/@RollingWheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void RefersToSameComponentRefTwice_Fail()
        {
            string objectAbsPath = "/@TestBenches/@BallisticCriticalComponent/@RefersToSameComponentRefTwice_Fail";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyElaborator should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void ReferencedByRefersTo()
        {
            string objectAbsPath = "/@TestBenches/@BallisticCriticalComponent/@ReferencedByRefersTo";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void RefersToMultiple()
        {
            string objectAbsPath = "/@TestBenches/@BallisticCriticalComponent/@RefersToMultiple";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void RefersToSameComponentTwice()
        {
            string objectAbsPath = "/@TestBenches/@BallisticCriticalComponent/@RefersToSameComponentTwice";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void RefersToSingle()
        {
            string objectAbsPath = "/@TestBenches/@BallisticCriticalComponent/@RefersToSingle";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");

            var success = ElaboratorRunner.RunElaborator(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyElaborator should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryComponent()
        {
            string objectAbsPath = "/@_Lib_/@ModelicaImports/@Damper";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            try
            {
                FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, true);

            }
            catch (COMException err)
            {
                Assert.True(err.Message.Contains("Cannot run FormulaEvaluator on Library Objects."));
            }
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryCA()
        {
            string objectAbsPath = "/@_Lib_/@CAWheel/@Wheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            try
            {
                FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, true);
            }
            catch (COMException err)
            {
                Assert.True(err.Message.Contains("Cannot run FormulaEvaluator on Library Objects."));
            }
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryTestComponent()
        {
            string objectAbsPath = "/@_Lib_/@ModelicaImports/@ModelicaImports/@Sine";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            try
            {
                FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, true);
            }
            catch (COMException err)
            {
                Assert.True(err.Message.Contains("Cannot run FormulaEvaluator on Library Objects."));
            }
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryTestBench()
        {
            string objectAbsPath = "/@_Lib_/@TestBenches/@RollingWheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            try
            {
                FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, true);
            }
            catch (COMException err)
            {
                Assert.True(err.Message.Contains("Cannot run FormulaEvaluator on Library Objects."));
            }
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryComponentAsUser()
        {
            string objectAbsPath = "/@_Lib_/@ModelicaImports/@Damper";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            // Set automation to false and expect no exception
            FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, false);
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryCAAsUser()
        {
            string objectAbsPath = "/@_Lib_/@CAWheel/@Wheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            // Set automation to false and expect no exception
            FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, false);
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryTestComponentAsUser()
        {
            string objectAbsPath = "/@_Lib_/@ModelicaImports/@ModelicaImports/@Sine";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            // Set automation to false and expect no exception
            FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, false);
        }

        [Fact]
        [Trait("Model", "RollingWheelWithLib")]
        public void FormulaEvaluatorLibraryTestBenchAsUser()
        {
            string objectAbsPath = "/@_Lib_/@TestBenches/@RollingWheel";

            Assert.True(File.Exists(this.mgaFile), "Failed to generate the mga.");
            // Set automation to false and expect no exception
            FormulaEvaluateRunner.RunFormulaEvaluate(mgaFile, objectAbsPath, false);
        }
    }
}