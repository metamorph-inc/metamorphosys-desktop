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

namespace DynamicsTeamTest.Projects
{
    public class RICheckerTestModelFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("RICheckerTestModel", "RICheckerTestModel.xme"); }
        }
    }

    public partial class RICheckerTestModel : IUseFixture<RICheckerTestModelFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private RICheckerTestModelFixture fixture { get; set; }

        public void SetFixture(RICheckerTestModelFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "RICheckerTestModel")]
        //[Trait("ProjectImport/Open", "RICheckerTestModel")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("ProjectImport/Open", "RICheckerTestModel")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_Missing_Uri_In_Component()
        {
            string outputDir = "CheckerTests_CA_RICircuit_Missing_Uri_In_Component";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_Missing_Uri_In_Component|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_Missing_Uri_In_TestComponent()
        {
            string outputDir = "CheckerTests_CA_RICircuit_Missing_Uri_In_TestComponent";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_Missing_Uri_In_TestComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        /* MOT-171: Case can no longer occur -- Connectors are now "unrolled"
        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_ModelicaConnector_in_CA_connected_to_Connector()
        {
            string outputDir = "CheckerTests_CA_RICircuit_ModelicaConnector_in_CA_connected_to_Connector";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_ModelicaConnector_in_CA_connected_to_Connector|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }
        */

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_CA_RegularModelicaConnectorsNotConnectedInComponent()
        {
            string outputDir = "CheckerTests_CA_RICircuit_CA_RegularModelicaConnectorsNotConnectedInComponent";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_CA_RegularModelicaConnectorsNotConnectedInComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        /* MOT-171 obsolete test
        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CyPhy2Modelica", "RICheckerTestModel")]
        public void CheckerTests_CA_RICircuit_ConnectorNotConnectedInComponent()
        {
            string outputDir = "CheckerTests_CA_RICircuit_ConnectorNotConnectedInComponent";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_ConnectorNotConnectedInComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }
        */

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_With_Space_in_Name()
        {
            string outputDir = "CheckerTests_CA_RICircuit With Space in Name";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit With Space in Name|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_()
        {
            string outputDir = "CheckerTests_CA_";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_1TB_Starts_with_character()
        {
            string outputDir = "CheckerTests_CA_1TB_Starts_with_character";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@1TB_Starts_with_character|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_for()
        {
            string outputDir = "CheckerTests_CA_for";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@for|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_CA_SolverSettingNegativeIntervalLength()
        {
            string outputDir = "CheckerTests_CA_RICircuit_CA_SolverSettingNegativeIntervalLength";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_CA_SolverSettingNegativeIntervalLength|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_CA_SolverSettingNegativeTolerance()
        {
            string outputDir = "CheckerTests_CA_RICircuit_CA_SolverSettingNegativeTolerance";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_CA_SolverSettingNegativeTolerance|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CheckerShouldFail", "RICheckerTestModel")]
        public void Fail_CheckerTests_CA_RICircuit_CA_SolverSettingNegativeNbrOfIntervals()
        {
            string outputDir = "CheckerTests_CA_RICircuit_CA_SolverSettingNegativeNbrOfIntervals";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@CheckerTests_CA|kind=Testing|relpos=0/@RICircuit_CA_SolverSettingNegativeNbrOfIntervals|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "RICheckerTestModel")]
        [Trait("CyPhy2Modelica", "RICheckerTestModel")]
        public void Tests_RICircuit_CA()
        {
            string outputDir = "Tests_RICircuit_CA";
            string testBenchPath = "/@Tests|kind=Testing|relpos=0/@RICircuit_CA|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }


    }
}

