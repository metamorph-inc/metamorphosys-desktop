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
using System.IO;
using Xunit;

namespace ComponentInterchangeTest
{
    public class ManufacturingFixture : IDisposable
    {
        public ManufacturingFixture()
        {
            // Import the GME Models
            File.Delete(Manufacturing.mgaPath_InputModel);
            GME.MGA.MgaUtils.ImportXME(Manufacturing.xmePath_InputModel, Manufacturing.mgaPath_InputModel);
            
            File.Delete(Manufacturing.mgaPath_DesiredResult);
            GME.MGA.MgaUtils.ImportXME(Manufacturing.xmePath_DesiredResult, Manufacturing.mgaPath_DesiredResult);
            
            Assert.True(File.Exists(Manufacturing.mgaPath_InputModel), String.Format("{0} not found. Model import may have failed.", Manufacturing.mgaPath_InputModel));
            Assert.True(File.Exists(Manufacturing.mgaPath_DesiredResult), String.Format("{0} not found. Model import may have failed.", Manufacturing.mgaPath_DesiredResult));

            // Clear exported components folder
            if (Directory.Exists(Manufacturing.path_ExportedComponents))
                Directory.Delete(Manufacturing.path_ExportedComponents, true);

            // Export the Manuf model.
            var rtnCode_Exporter = CommonFunctions.runCyPhyComponentExporterCL(Manufacturing.mgaPath_DesiredResult);
            Assert.True(0 == rtnCode_Exporter, "Component Exporter had a non-zero return code of " + rtnCode_Exporter);

            // Import to the blank model
            var rtnCode_Importer = CommonFunctions.runCyPhyComponentImporterCLRecursively(Manufacturing.mgaPath_InputModel, Manufacturing.path_ExportedComponents);
            Assert.True(0 == rtnCode_Importer, "Component Importer had a non-zero return code of " + rtnCode_Importer);
        }

        public void Dispose()
        {
            // No state, so nothing to do here.
        }
    }

    public class Manufacturing : IUseFixture<ManufacturingFixture>
    {
        #region Path Variables
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "Manufacturing"
            );

        public static readonly string xmePath_DesiredResult = Path.Combine(
            testPath,
            "DesiredResult.xme"
            );

        public static readonly string mgaPath_DesiredResult = xmePath_DesiredResult.Replace(".xme", ".mga");
        
        public static readonly string xmePath_InputModel = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "BlankInputModel",
            "InputModel.xme"
            );

        public static readonly string mgaPath_InputModel = Path.Combine(
            testPath,
            "InputModel.mga"
            );

        public static readonly string path_ExportedComponents = Path.Combine(
            testPath,
            "Components"
            );
        #endregion

        #region Fixture
        ManufacturingFixture fixture;
        public void SetFixture(ManufacturingFixture data)
        {
            fixture = data;
        }
        #endregion

        [Fact]
        [Trait("Interchange", "Manufacturing")]
        public void ImportAll()
        {
            Assert.True(0 == CommonFunctions.RunCyPhyMLComparator(mgaPath_DesiredResult, mgaPath_InputModel), "Imported model doesn't match expected.");
        }
    }
}
