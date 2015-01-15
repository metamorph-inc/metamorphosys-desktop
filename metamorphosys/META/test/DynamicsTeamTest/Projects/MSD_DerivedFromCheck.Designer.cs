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
    public class MSD_DerivedFromCheckFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("MSD_DerivedFromCheck", "MSD_DerivedFromCheck.xme"); }
        }
    }

    public partial class MSD_DerivedFromCheck : IUseFixture<MSD_DerivedFromCheckFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private MSD_DerivedFromCheckFixture fixture { get; set; }

        public void SetFixture(MSD_DerivedFromCheckFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "MSD_DerivedFromCheck")]
        //[Trait("ProjectImport/Open", "MSD_DerivedFromCheck")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("ProjectImport/Open", "MSD_DerivedFromCheck")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("CyPhy2Modelica", "MSD_DerivedFromCheck")]
        public void TestBenches_MSD()
        {
            string outputDir = "TestBenches_MSD";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MSD|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("CyPhy2Modelica", "MSD_DerivedFromCheck")]
        public void TestBenches_MSD_Damper_Formula_Shared_Parameter()
        {
            string outputDir = "TestBenches_MSD_Damper_Formula_Shared_Parameter";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MSD_Damper_Formula_Shared_Parameter|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("CyPhy2Modelica", "MSD_DerivedFromCheck")]
        public void TestBenches_MSD_Damper_External_Property()
        {
            string outputDir = "TestBenches_MSD_Damper_External_Property";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MSD_Damper_External_Property|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("CyPhy2Modelica", "MSD_DerivedFromCheck")]
        public void TestBenches_MSD_Damper_External_Parameter()
        {
            string outputDir = "TestBenches_MSD_Damper_External_Parameter";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MSD_Damper_External_Parameter|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("CyPhy2Modelica", "MSD_DerivedFromCheck")]
        public void TestBenches_MSD_Damper_Formula_Parameter()
        {
            string outputDir = "TestBenches_MSD_Damper_Formula_Parameter";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MSD_Damper_Formula_Parameter|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_DerivedFromCheck")]
        [Trait("CyPhy2Modelica", "MSD_DerivedFromCheck")]
        public void TestBenches_MSD_Damper_Formula_Property()
        {
            string outputDir = "TestBenches_MSD_Damper_Formula_Property";
            string testBenchPath = "/@TestBenches|kind=Testing|relpos=0/@MSD_Damper_Formula_Property|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }


    }
}

