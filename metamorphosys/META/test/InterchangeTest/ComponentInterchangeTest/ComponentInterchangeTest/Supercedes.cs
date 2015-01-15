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
    public class Supercedes
    {
        #region Path Variables
        private static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "Supercedes"
            );

        private static readonly string xmePath_DesiredResult = Path.Combine(
            testPath,
            "DesiredResult.xme"
            );

        private static readonly string xmePath_InputModel = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "BlankInputModel",
            "InputModel.xme"
            );

        private string mgaPath_DesiredResult;
        private string mgaPath_InputModel = Path.Combine(
            testPath,
            "InputModel.mga");
        #endregion

        private bool couldNotDeleteComponents = false;
        private string couldNotDeleteComponents_Reason = "";
        private int exporterReturnCode;
        private int importerReturnCode;

        #region Initialization and Dispose
        private bool bInitialized = false;
        private void Initialize()
        {
            if (bInitialized)
                return;

            // Clear the Components folder
            try
            {
                Directory.Delete(Path.Combine(testPath, "Components"), true);
            }
            catch (DirectoryNotFoundException)
            {
                // It's okay if it didn't exist.
            }
            catch (Exception ex)
            {
                // Results will be unreliable. Might as well quit now.
                couldNotDeleteComponents = true;
                couldNotDeleteComponents_Reason = String.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                return;
            }

            // Import the model.
            mgaPath_DesiredResult = CommonFunctions.unpackXme(xmePath_DesiredResult);

            // Export the components
            exporterReturnCode = CommonFunctions.runCyPhyComponentExporterCL(mgaPath_DesiredResult);

            // Import the blank model
            GME.MGA.MgaUtils.ImportXME(xmePath_InputModel, mgaPath_InputModel);

            // Import the components to the blank model
            importerReturnCode = CommonFunctions.runCyPhyComponentImporterCLRecursively(mgaPath_InputModel, Path.Combine(testPath, "Components"));
            
            bInitialized = true;
        }
        #endregion
        
        private void CheckPreConditions()
        {
            Initialize();
            try
            {
                Assert.True(exporterReturnCode == 0, "Exporter encountered an exception.");
                Assert.False(couldNotDeleteComponents, String.Format("Couldn't delete exported components folder ({0})", couldNotDeleteComponents_Reason));
                Assert.True(File.Exists(mgaPath_DesiredResult), "Desired Result MGA file not found. Model import may have failed.");
                Assert.True(File.Exists(mgaPath_InputModel), "Input Model MGA file not found. Model import may have failed.");
                Assert.True(importerReturnCode == 0, "Importer encountered an exception.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Fact]
        public void SupercedesRoundTrip()
        {
            CheckPreConditions();

            var result = CommonFunctions.RunCyPhyMLComparator(mgaPath_DesiredResult, mgaPath_InputModel);
            Assert.True(result == 0, "Resulting model didn't match expected.");
        }
    }
}
