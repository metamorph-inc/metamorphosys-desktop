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
    public class WaterPumpFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("WaterPump", "WaterPump.xme"); }
        }
    }

    public partial class WaterPump : IUseFixture<WaterPumpFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private WaterPumpFixture fixture { get; set; }

        public void SetFixture(WaterPumpFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "WaterPump")]
        //[Trait("ProjectImport/Open", "WaterPump")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "WaterPump")]
        [Trait("ProjectImport/Open", "WaterPump")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }

        [Fact]
        [Trait("Model", "WaterPump")]
        [Trait("CyPhy2Modelica", "WaterPump")]
        public void TestBenches_Test_Delivered_Electric_Pump_TransportedFluid()
        {
            string outputDir = "TestBenches_Test_Delivered_Electric_Pump_TransportedFluid";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@Test_Delivered_Electric_Pump_TransportedFluid|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "WaterPump")]
        [Trait("CheckerShouldFail", "WaterPump")]
        public void Fail_MediaFails_DifferentMediumInTestComponent()
        {
            string outputDir = "MediaFails_DifferentMediumInTestComponent";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MediaFails|kind=Testing|relpos=0/@DifferentMediumInTestComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "WaterPump")]
        [Trait("CheckerShouldFail", "WaterPump")]
        public void Fail_MediaFails_PartialMediumTranportedFluidNotDefined()
        {
            string outputDir = "MediaFails_PartialMediumTranportedFluidNotDefined";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MediaFails|kind=Testing|relpos=0/@PartialMediumTranportedFluidNotDefined|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "WaterPump")]
        [Trait("CheckerShouldFail", "WaterPump")]
        public void Fail_MediaFails_PartialMediumInComponent()
        {
            string outputDir = "MediaFails_PartialMediumInComponent";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MediaFails|kind=Testing|relpos=0/@PartialMediumInComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.False(result, "CyPhy2Modelica_v2 should have failed, but did not.");
        }

        [Fact]
        [Trait("Model", "WaterPump")]
        [Trait("CyPhy2Modelica", "WaterPump")]
        public void TestBenches_Test_Delivered_Electric_Pump()
        {
            string outputDir = "TestBenches_Test_Delivered_Electric_Pump";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@Test_Delivered_Electric_Pump|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }


    }
}

