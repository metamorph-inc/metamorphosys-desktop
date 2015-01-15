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
using GME.CSharp;
using System.Text.RegularExpressions;
using GME.MGA;

namespace ComponentImporterUnitTests
{
    public class ImportZip : IUseFixture<ImportZipFixture>
    {
        private static string testPath = Path.Combine(Common._importModelDirectory, "ImportZip");
        public static string mgaPath = Path.Combine(testPath, "InputModel.mga");
        private static string manifestFilePath = Path.Combine(testPath, "manifest.project.json");

        ImportZipFixture fixture;
        public void SetFixture(ImportZipFixture data)
        {
            fixture = data;
        }

        [Fact]
        public void ImportZIP()
        {
            var zipPath = Path.Combine(testPath, "TestModel.zip");

            // Import the ZIP file.
            // Check that we run without exception, have a manifest file,
            // have a folder for the component, and have 2 files in there.
            
            // Delete manifest and any subfolders
            File.Delete(manifestFilePath);
            if (Directory.Exists(Path.Combine(testPath, "components")))
                Directory.Delete(Path.Combine(testPath, "components"), true);
            
            var mgaProject = Common.GetProject(mgaPath);
            Assert.True(mgaProject != null, "Could not load MGA project.");

            bool resultIsNull = false;
            var mgaGateway = new MgaGateway(mgaProject);
            mgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(delegate
            {
                var importer = new CyPhyComponentImporter.CyPhyComponentImporterInterpreter();
                importer.Initialize(mgaProject);

                try
                {
                    var result = importer.ImportFile(mgaProject, testPath, zipPath);
                    if (result == null)
                        resultIsNull = true;
                }
                finally
                {
                    importer.DisposeLogger();
                }

            });
            Assert.False(resultIsNull, "Exception occurred during import.");
            Assert.False(File.Exists(manifestFilePath), "Manifest erroneously generated");
        }

        [Fact]
        public void ZipIsMissingResource()
        {
            var zipPath = Path.Combine(testPath, "TestModel_missingResource.zip");

            var mgaProject = Common.GetProject(mgaPath);
            Assert.False(mgaProject == null, "Could not load MGA project.");

            var mgaGateway = new MgaGateway(mgaProject);
            mgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(delegate
            {
                var importer = new CyPhyComponentImporter.CyPhyComponentImporterInterpreter();
                importer.Initialize(mgaProject);

                try
                {
                    importer.ImportFile(mgaProject, testPath, zipPath);
                }
                finally
                {
                    importer.DisposeLogger();
                }
            });
        }

        [Fact]
        public void ZipIsMissingACM()
        {
            var zipPath = Path.Combine(testPath, "TestModel_noACM.zip");

            var mgaProject = Common.GetProject(mgaPath);
            Assert.False(mgaProject == null, "Could not load MGA project.");

            var mgaGateway = new MgaGateway(mgaProject);
            mgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(delegate
            {
                var importer = new CyPhyComponentImporter.CyPhyComponentImporterInterpreter();
                importer.Initialize(mgaProject);

                try
                {

                    var result = importer.ImportFile(mgaProject, testPath, zipPath);
                }
                finally
                {
                    importer.DisposeLogger();
                }
            });
        }

        [Fact]
        public void ImportCADZIP_Spring_Tungsten()
        {
            var zipPath = Path.Combine(testPath, "Spring_Tungsten.zip");

            // Import the ZIP file.
            // Check that we run without exception, have a manifest file,
            // have a folder for the component, and have 2 files in there.

            // Delete manifest and any subfolders
            File.Delete(manifestFilePath);
            if (Directory.Exists(Path.Combine(testPath, "components")))
                Directory.Delete(Path.Combine(testPath, "components"), true);

            var mgaProject = Common.GetProject(mgaPath);
            Assert.True(mgaProject != null, "Could not load MGA project.");

            var mgaGateway = new MgaGateway(mgaProject);
            mgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(delegate
            {
                var importer = new CyPhyComponentImporter.CyPhyComponentImporterInterpreter();
                importer.Initialize(mgaProject);

                try
                {
                    var result = importer.ImportFile(mgaProject, testPath, zipPath);
                    var tungsten = result.ChildObjects.Cast<IMgaFCO>().Where(x => x.Meta.Name == "Resource" && x.Name == "TUNGSTEN_SPRING.PRT").FirstOrDefault();
                    Assert.Equal("CAD\\TUNGSTEN_SPRING.PRT", tungsten.StrAttrByName["Path"]);
                }
                finally
                {
                    importer.DisposeLogger();
                }
            });
        }

    }
}
