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

namespace ComponentInterchangeTest
{
    public class GuideDatumFixture
    {
        public GuideDatumFixture()
        {
            // Import the GME Models
            File.Delete(GuideDatum.mgaPath_InputModel);
            GME.MGA.MgaUtils.ImportXME(GuideDatum.xmePath_InputModel, GuideDatum.mgaPath_InputModel);

            File.Delete(GuideDatum.mgaPath_DesiredResult);
            GME.MGA.MgaUtils.ImportXME(GuideDatum.xmePath_DesiredResult, GuideDatum.mgaPath_DesiredResult);

            Assert.True(File.Exists(GuideDatum.mgaPath_InputModel), String.Format("{0} not found. Model import may have failed.", GuideDatum.mgaPath_InputModel));
            Assert.True(File.Exists(GuideDatum.mgaPath_DesiredResult), String.Format("{0} not found. Model import may have failed.", GuideDatum.mgaPath_DesiredResult));

            // Clear exported components folder
            if (File.Exists(GuideDatum.path_ExportedACM))
            {
                File.Delete(GuideDatum.path_ExportedACM);
            }

            // Export the Manuf model.
            var rtnCode_Exporter = CommonFunctions.runCyPhyComponentExporterCL(GuideDatum.mgaPath_DesiredResult);
            Assert.True(0 == rtnCode_Exporter, "Component Exporter had a non-zero return code of " + rtnCode_Exporter);

            // Import to the blank model
            var rtnCode_Importer = CommonFunctions.runCyPhyComponentImporterCL(GuideDatum.mgaPath_InputModel, GuideDatum.path_ExportedACM);
            Assert.True(0 == rtnCode_Importer, "Component Importer had a non-zero return code of " + rtnCode_Importer);
        }
    }

    public class GuideDatumModelImport
    {
        [Fact]
        [Trait("ProjectImport/Open", "GuideDatum")]
        public void ProjectXmeImport()
        {
            Assert.DoesNotThrow(() => { new GuideDatumFixture(); });
        }
    }

    public class GuideDatum : IUseFixture<GuideDatumFixture>
    {
        #region Path Variables
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "GuideDatum"
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

        public static readonly string path_ExportedACM = Path.Combine(testPath,
                                       "Components",
                                       "HasGuideDatums",
                                       "HasGuideDatums.component.acm");
        #endregion

        #region Fixture
        GuideDatumFixture fixture;
        public void SetFixture(GuideDatumFixture data)
        {
            fixture = data;
        }
        #endregion

        [Fact]
        [Trait("Interchange", "GuideDatum")]
        public void RoundTrip()
        {
            Assert.True(0 == CommonFunctions.RunCyPhyMLComparator(mgaPath_DesiredResult, mgaPath_InputModel), "Imported model doesn't match expected.");
        }

        [Fact]
        [Trait("Interchange", "GuideDatum")]
        public void CheckExportedACM()
        {
            avm.Component acm = null;
            using (var reader = new StreamReader(path_ExportedACM))
            {
                acm = CyPhyComponentImporter.CyPhyComponentImporterInterpreter.DeserializeAvmComponentXml(reader);
            }

            Assert.Equal(1, acm.Connector.Count);
            var connector = acm.Connector.FirstOrDefault();

            Assert.Equal(8, connector.Role.Count);
            var datums = connector.Role.OfType<avm.cad.Datum>();
            Assert.Equal(8, datums.Count());

            var guideDatums = connector.ConnectorFeature.OfType<avm.cad.GuideDatum>();
            Assert.Equal(4, guideDatums.Count());

            foreach (var guideDatum in guideDatums)
            {
                var linkedDatum = datums.FirstOrDefault(x => x.ID == guideDatum.Datum);
                Assert.NotNull(linkedDatum);
                Assert.True(linkedDatum.Name.StartsWith("GD_"));
            }
        }
    }
}
