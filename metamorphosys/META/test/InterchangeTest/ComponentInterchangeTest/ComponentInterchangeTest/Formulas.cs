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
    public class Formulas : IUseFixture<FormulasFixture>
    {
        #region Path Variables
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "Formulas"
            );
        public static readonly string xmePath = Path.Combine(
            testPath,
            "Formulas.xme"
            );
        public static readonly string mgaPath = Path.Combine(
            testPath,
            "Formulas.mga"
            );
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
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "Formulas",
            "InputModel.mga"
            );
        #endregion

        #region Fixture
        FormulasFixture fixture;
        public void SetFixture(FormulasFixture data)
        {
            fixture = data;
        }
        #endregion
        
        [Fact]
        public void ModelImported()
        {
            // Fixture does the heavy lifting
        }
        
        [Fact]
        public void RoundTripTest()
        {
            Assert.True(0 == CommonFunctions.RunCyPhyMLComparator(mgaPath, mgaPath_InputModel), "Imported models don't match expected.");
        }
    }

    public class FormulasFixture : IDisposable
    {
        public FormulasFixture()
        {
            var compDir = Path.Combine(Formulas.testPath, "Components");

            // Clear the Components folder
            try
            {
                if (Directory.Exists(compDir))
                    Directory.Delete(compDir, true);
            }
            catch (Exception ex)
            {
                // Results will be unreliable. Might as well quit now.
                throw ex;
            }

            // Import the model.
            File.Delete(Formulas.mgaPath);
            GME.MGA.MgaUtils.ImportXME(Formulas.xmePath, Formulas.mgaPath);
            Assert.True(File.Exists(Formulas.mgaPath), String.Format("{0} not found. Model import may have failed.", Formulas.mgaPath));

            // Export the components
            var returnCode = CommonFunctions.runCyPhyComponentExporterCL(Formulas.mgaPath);
            Assert.True(0 == returnCode, "Exporter had non-zero return code of " + returnCode);

            // Import the blank model.
            File.Delete(Formulas.mgaPath_InputModel);
            GME.MGA.MgaUtils.ImportXME(Formulas.xmePath_InputModel, Formulas.mgaPath_InputModel);
            Assert.True(File.Exists(Formulas.mgaPath_InputModel), String.Format("{0} not found. Model import may have failed.", Formulas.mgaPath_InputModel));

            // Import the components to the blank model
            Assert.True(0 == CommonFunctions.runCyPhyComponentImporterCLRecursively(Formulas.mgaPath_InputModel, compDir), "Component Importer had non-zero return code.");            
        }

        public void Dispose()
        {
            // No state, so nothing to do here
        }
    }
}
