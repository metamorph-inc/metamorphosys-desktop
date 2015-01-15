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
    public class MSD_CADFixture : XmeImportFixture
    {
        protected override string xmeFilename
        {
            get { return Path.Combine("MSD_CAD", "MSD_CAD.xme"); }
        }
    }

    public partial class MSD_CAD : IUseFixture<MSD_CADFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private MSD_CADFixture fixture { get; set; }

        public void SetFixture(MSD_CADFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "MSD_CAD")]
        //[Trait("ProjectImport/Open", "MSD_CAD")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("ProjectImport/Open", "MSD_CAD")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_extra_mass_with_no_modelica_model()
        {
            string outputDir = "tbs_MSD_extra_mass_with_no_modelica_model";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_extra_mass_with_no_modelica_model|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_extra_mass_with_unconnected_modelica_model()
        {
            string outputDir = "tbs_MSD_extra_mass_with_unconnected_modelica_model";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_extra_mass_with_unconnected_modelica_model|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_system_without_dynamic_models()
        {
            string outputDir = "tbs_MSD_system_without_dynamic_models";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_system_without_dynamic_models|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_system_where_inner_system_only_has_dynamic_models()
        {
            string outputDir = "tbs_MSD_system_where_inner_system_only_has_dynamic_models";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_system_where_inner_system_only_has_dynamic_models|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_system_without_dynamic_models2()
        {
            string outputDir = "tbs_MSD_system_without_dynamic_models2";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_system_without_dynamic_models2|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_two_subsystems()
        {
            string outputDir = "tbs_MSD_two_subsystems";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_two_subsystems|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_two_subsystems_passing_parameter()
        {
            string outputDir = "tbs_MSD_two_subsystems_passing_parameter";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_two_subsystems_passing_parameter|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_two_subsystems_passing_parameter2()
        {
            string outputDir = "tbs_MSD_two_subsystems_passing_parameter2";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_two_subsystems_passing_parameter2|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_passing_value_between_components()
        {
            string outputDir = "tbs_MSD_passing_value_between_components";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_passing_value_between_components|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_passing_value_between_components2()
        {
            string outputDir = "tbs_MSD_passing_value_between_components2";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_passing_value_between_components2|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_two_subsystems_passing_parameter3()
        {
            string outputDir = "tbs_MSD_two_subsystems_passing_parameter3";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_two_subsystems_passing_parameter3|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_passing_value_nested_systems()
        {
            string outputDir = "tbs_MSD_passing_value_nested_systems";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_passing_value_nested_systems|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void tbs_MSD_one_dynamic_one_pure_cad()
        {
            string outputDir = "tbs_MSD_one_dynamic_one_pure_cad";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@tbs|kind=Testing|relpos=0/@MSD_one_dynamic_one_pure_cad|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }

        [Fact]
        [Trait("Model", "MSD_CAD")]
        [Trait("CyPhy2Modelica", "MSD_CAD")]
        public void Dynamics_MSD()
        {
            string outputDir = "Dynamics_MSD";
            string testBenchPath = "/@Dynamics|kind=Testing|relpos=0/@MSD|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
            bool result = CyPhy2ModelicaRunner.Run(outputDir, mgaFile, testBenchPath);

            Assert.True(result, "CyPhy2Modelica_v2 failed.");
        }


    }
}

