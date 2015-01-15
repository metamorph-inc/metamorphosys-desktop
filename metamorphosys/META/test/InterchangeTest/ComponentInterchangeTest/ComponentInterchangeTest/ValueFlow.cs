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
    public class ValueFlow : IUseFixture<ValueFlowFixture>
    {
        #region Path Variables
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "ValueFlow"
            );

        public static readonly string xmePath = Path.Combine(
            testPath,
            "ValueFlow.xme"
            );
        public static readonly string mgaPath = Path.Combine(
            testPath,
            "ValueFlow.mga"
            );
        #endregion

        #region Fixture
        ValueFlowFixture fixture;
        public void SetFixture(ValueFlowFixture data)
        {
            fixture = data;
        }
        #endregion
        
        [Fact]
        [Trait("Interchange","ValueFlow")]
        [Trait("Interchange","Component Export")]
        public void AllComponentsExported()
        {
            var exportedACMRoot = Path.Combine(testPath, "Exported");
            var acmFiles = Directory.GetFiles(exportedACMRoot, "*.acm", SearchOption.AllDirectories);
            Assert.Equal(5, acmFiles.Length);
        }
        
        [Fact]
        public void ImportAll()
        {
            var importXmePath = Path.Combine(testPath,"InputModel.xme");
            var importMgaPath = CommonFunctions.unpackXme(importXmePath);
            Assert.True(File.Exists(importMgaPath),"MGA file not found. Model import may have failed.");

            var compFolderRoot = Path.Combine(testPath, "Exported");
            int rtnCode = CommonFunctions.runCyPhyComponentImporterCLRecursively(importMgaPath, compFolderRoot);
            Assert.True(rtnCode == 0, String.Format("Importer failed on one or more components"));

            Assert.True(0 == CommonFunctions.RunCyPhyMLComparator(mgaPath, importMgaPath), String.Format("Imported model {0} doesn't match expected {1}.", importMgaPath, mgaPath));
        }
    }
}
