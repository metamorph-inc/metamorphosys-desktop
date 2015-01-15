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

namespace ComponentExporterUnitTests
{
    public class ModelicaStructuresFixture : IDisposable
    {
        public ModelicaStructuresFixture()
        {
            // Clear the Components folder
            var compFolder = Path.Combine(ModelicaStructures.testPath, "Components");
            try
            {
                if (Directory.Exists(compFolder))
                    Directory.Delete(compFolder, true);
            }
            catch (IOException ex)
            {
                // Results will be unreliable. Might as well quit now.
                throw ex;
            }

            // Import the model.
            GME.MGA.MgaUtils.ImportXME(ModelicaStructures.xmePath, ModelicaStructures.mgaPath);
            Assert.True(File.Exists(ModelicaStructures.mgaPath), "MGA file not found; Import may have failed.");

            // Export the components
            Assert.True(0 == Common.runCyPhyComponentExporterCL(ModelicaStructures.mgaPath), "Component Exporter had non-zero return code");
        }
        
        public void Dispose()
        {
            // Nothing to do
        }
    }

    public class ModelicaStructures : IUseFixture<ModelicaStructuresFixture>
    {
        #region Path Variables
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath, 
            "test", 
            "InterchangeTest", 
            "ComponentInterchangeTest", 
            "SharedModels", 
            "Modelica");

        public static readonly string xmePath = Path.Combine(
            testPath,
            "Modelica.xme");

        public static readonly string mgaPath = Path.Combine(
            testPath,
            "Modelica.mga");
        #endregion
        
        #region Fixture
        ModelicaStructuresFixture fixture;
        public void SetFixture(ModelicaStructuresFixture data)
        {
            fixture = data;
        }
        #endregion
        
        [Fact]
        [Trait("Interchange", "Modelica")]
        [Trait("Interchange", "Component Export")]
        public void ModelRedeclareExport()
        {
            var generatedACM = Path.Combine(testPath, 
                                            "Components",
                                            "Some_Class",
                                            "Comp_ModelRedeclare", 
                                            "Comp_ModelRedeclare.component.acm");
            var expectedACM = Path.Combine(testPath, "Comp_ModelRedeclare.expected.acm");

            Assert.True(File.Exists(generatedACM), "Exported ACM file not found.");
            Assert.True(0 == Common.RunXmlComparator(generatedACM, expectedACM), "Generated ACM file doesn't match expected output.");
        }

        [Fact]
        [Trait("Interchange", "Modelica")]
        [Trait("Interchange", "Component Export")]
        public void PortRedeclareExport()
        {
            var generatedACM = Path.Combine(testPath, 
                                            "Components", 
                                            "Comp_PortRedeclare", 
                                            "Comp_PortRedeclare.component.acm");
            var expectedACM = Path.Combine(testPath, "Comp_PortRedeclare.expected.acm");

            Assert.True(File.Exists(generatedACM), "Exported ACM file not found.");
            Assert.True(0 == Common.RunXmlComparator(generatedACM, expectedACM), "Generated ACM file doesn't match expected output.");
        }

        [Fact]
        [Trait("Interchange", "Modelica")]
        [Trait("Interchange", "Component Export")]
        public void PortRedeclareAtCompLevelExport()
        {
            var generatedACM = Path.Combine(testPath, 
                                            "Components", 
                                            "Engine",
                                            "Comp_PortRedeclareAtCompLvl", 
                                            "Comp_PortRedeclareAtCompLvl.component.acm");
            var expectedACM = Path.Combine(testPath, "Comp_PortRedeclareAtCompLvl.expected.acm");

            Assert.True(File.Exists(generatedACM), "Exported ACM file not found.");
            Assert.True(0 == Common.RunXmlComparator(generatedACM, expectedACM), "Generated ACM file doesn't match expected output.");
        }
    }
}
