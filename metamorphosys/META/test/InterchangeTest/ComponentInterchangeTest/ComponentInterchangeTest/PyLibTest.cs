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
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ComponentImporterUnitTests
{
    public class PyLibTestFixture : IDisposable
    {
        public PyLibTestFixture()
        {
            // Import Driveline XME file
            if (Directory.Exists(PyLibTest.acmFilesPath))
                Directory.Delete(PyLibTest.acmFilesPath, true);
            Directory.CreateDirectory(PyLibTest.acmFilesPath);

            File.Delete(PyLibTest.mgaPath);
            GME.MGA.MgaUtils.ImportXME(PyLibTest.xmePath, PyLibTest.mgaPath);
            Assert.True(File.Exists(PyLibTest.mgaPath));

            var args = (PyLibTest.mgaPath + " -f " + PyLibTest.acmFilesPath).Split();
            var rtnCode = CyPhyComponentExporterCL.CyPhyComponentExporterCL.Main(args);
            Assert.True(rtnCode == 0, "Component Exporter had non-zero return code.");
        }

        public void Dispose()
        {
            // No state, so nothing to do here.
        }
    }

    public class PyLibTest : IUseFixture<PyLibTestFixture>
    {
        #region Path Variables
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "models",
            "DynamicsTeam",
            "DriveLine_v3"
            );
        public static readonly string xmePath = Path.Combine(
            testPath,
            "DriveLine_v3.xme"
            );
        public static readonly string mgaPath = Path.Combine(
            testPath,
            "DriveLine_v3_PyImportTest.mga"
            );
        public static readonly string acmFilesPath = Path.Combine(
            testPath,
            "ExportedComponents"
            );
        #endregion

        #region Fixture
        PyLibTestFixture fixture;
        public void SetFixture(PyLibTestFixture data)
        {
            fixture = data;
        }
        #endregion
        
        [Fact]
        public void PythonLibraryTest()
        {
            // Find all exported ACM files
            var acms = Directory.EnumerateFiles(acmFilesPath, "*.acm", SearchOption.AllDirectories);
            ConcurrentBag<String> cb_Failures = new ConcurrentBag<String>();
            Parallel.ForEach(acms, pathACM =>
            {
                var absPathACM = Path.Combine(acmFilesPath, pathACM);
                String output;
                int rtnCode = PyLibUtils.TryImportUsingPyLib(absPathACM, out output);

                if (rtnCode != 0)
                    cb_Failures.Add(pathACM);
            });

            if (cb_Failures.Any())
            {
                var msg = "AVM PyLib failed to parse:" + Environment.NewLine;
                foreach (var acmPath in cb_Failures)
                {
                    msg += String.Format("{0}" + Environment.NewLine, acmPath);
                }
                Assert.False(true, msg);
            }
        }
    }
}
