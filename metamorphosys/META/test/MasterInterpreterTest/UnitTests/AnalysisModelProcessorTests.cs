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

namespace MasterInterpreterTest.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CyPhyMasterInterpreter;
    using Xunit;
    using MasterInterpreterTest.Projects;
    using GME.MGA;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnalysisModelProcessorTests : IUseFixture<MasterInterpreterFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private MasterInterpreterFixture fixture { get; set; }

        public void SetFixture(MasterInterpreterFixture data)
        {
            this.fixture = data;
        }

        [Fact]
        [Trait("MasterInterpreter", "AnalysisModelProcessor")]
        public void ShouldThrowExceptions()
        {
            // null context
            Assert.Throws<ArgumentNullException>(() => { AnalysisModelProcessor.GetAnalysisModelProcessor(null); });

            Assert.True(File.Exists(this.mgaFile), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + Path.GetFullPath(this.mgaFile);

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var componentAssembly = project.RootFolder.GetDescendantFCOs(project.CreateFilter()).OfType<MgaModel>().FirstOrDefault(x => x.MetaBase.Name == "ComponentAssembly");

                Assert.True(componentAssembly != null, string.Format("{0} project must contain one component assembly.", Path.GetFullPath(this.mgaFile)));

                // invalid context
                Assert.Throws<AnalysisModelContextNotSupportedException>(() => { AnalysisModelProcessor.GetAnalysisModelProcessor(componentAssembly); });
            }
            finally
            {
                project.AbortTransaction();
                project.Close(true);
            }

        }
    }
}
