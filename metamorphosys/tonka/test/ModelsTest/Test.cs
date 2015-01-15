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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xunit;
using GME.MGA;

namespace ModelsTest
{
    public class ModelsTest
    {
        private static void ImportTest(String p_test, String p_xme, out String p_mga)
        {
            try
            {
                String xmeName = Path.GetFileName(p_xme);
                Assert.True(File.Exists(p_xme), String.Format("{0} is missing", xmeName));

                p_mga = p_xme.Replace(".xme", "_test.mga");
                MgaUtils.ImportXME(p_xme, p_mga);

                // Assert that MGA file exists, and is relatively new
                String mgaName = Path.GetFileName(p_mga);
                Assert.True(File.Exists(p_mga), String.Format("{0} does not exist. It probably didn't import successfully.", mgaName));

                DateTime dt_mgaLastWrite = File.GetLastWriteTime(p_mga);
                var threshold = DateTime.Now.AddSeconds(-10.0);
                Assert.True(dt_mgaLastWrite > threshold, String.Format("{0} is older than 10 seconds. It probably didn't import successfully, and this is an old copy.", mgaName));

                // Delete temp file
                File.Delete(p_mga);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        [Fact]
        public static void PulseOxyManual_LoadTest()
        {
            String p_test = "../../../../models/PulseOxy";
            String p_xme = Path.Combine(p_test, "PulseOxy.xme");
            String p_mga;
            ImportTest(p_test, p_xme, out p_mga);
        }

    }
}
