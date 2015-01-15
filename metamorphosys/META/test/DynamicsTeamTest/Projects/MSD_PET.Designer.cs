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
    public class MSD_PETFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("MSD_PET", "MSD_PET.xme"); }
        }
    }

    public partial class MSD_PET : IUseFixture<MSD_PETFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private MSD_PETFixture fixture { get; set; }

        public void SetFixture(MSD_PETFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "MSD_PET")]
        //[Trait("ProjectImport/Open", "MSD_PET")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("ProjectImport/Open", "MSD_PET")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("CyPhy2Modelica", "MSD_PET")]
        public void TestBenches_MassSpringDamperTest()
        {
            string outputDir = "Test Benches_MassSpringDamperTest";
            string testBenchPath = "/@Test Benches|kind=Testing|relpos=0/@MassSpringDamperTest|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhyPET failed.");
        }


        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void ResponseSurfaceExample()
        {
            string outputDir = "ResponseSurfaceExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@SurrogateModeling|kind=ParametricExplorationFolder|relpos=0/@ResponseSurfaceExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void LogisticRegressionExample()
        {
            string outputDir = "LogisticRegressionExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@SurrogateModeling|kind=ParametricExplorationFolder|relpos=0/@LogisticRegressionExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void NeuralNetExample()
        {
            string outputDir = "NeuralNetExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@SurrogateModeling|kind=ParametricExplorationFolder|relpos=0/@NeuralNetExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void KrigingExample()
        {
            string outputDir = "KrigingExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@SurrogateModeling|kind=ParametricExplorationFolder|relpos=0/@KrigingExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void CentralCompositeExample()
        {
            string outputDir = "CentralCompositeExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@DOE|kind=ParametricExplorationFolder|relpos=0/@CentralCompositeExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void FullFactorialExample()
        {
            string outputDir = "FullFactorialExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@DOE|kind=ParametricExplorationFolder|relpos=0/@FullFactorialExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void LatinHypercubeExample()
        {
            string outputDir = "LatinHypercubeExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@DOE|kind=ParametricExplorationFolder|relpos=0/@LatinHypercubeExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void UniformExample()
        {
            string outputDir = "UniformExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@DOE|kind=ParametricExplorationFolder|relpos=0/@UniformExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void CONMINExample()
        {
            string outputDir = "CONMINExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@Optimization|kind=ParametricExplorationFolder|relpos=0/@CONMINExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void COBYLAExample()
        {
            string outputDir = "COBYLAExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@Optimization|kind=ParametricExplorationFolder|relpos=0/@COBYLAExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

        [Fact]
        [Trait("Model", "MSD_PET")]
        [Trait("PET", "MSD_PET")]
        public void NEWSUMTExample()
        {
            string outputDir = "NEWSUMTExample";
            string petExperimentPath = "/@Examples|kind=Testing|relpos=0/@Optimization|kind=ParametricExplorationFolder|relpos=0/@NEWSUMTExample|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhyPETRunner.Run(outputDir, mgaFile, petExperimentPath);

            Assert.True(result, "CyPhyPET failed.");
        }

    }
}

