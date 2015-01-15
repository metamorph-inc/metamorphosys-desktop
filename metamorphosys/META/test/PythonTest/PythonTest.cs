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
using System.Reflection;
using Xunit;
using META;
using System.Diagnostics;

namespace PythonTest
{
    public class PythonTest
    {
        [STAThread]
        public static int Main(string[] args)
        {
            int ret = Xunit.ConsoleClient.Program.Main(new string[] {
                Assembly.GetAssembly(typeof(PythonTest)).CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
            return ret;
        }

        private Exception ImportModule(string moduleName)
        {
            try
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    FileName = VersionInfo.PythonVEnvExe,
                    Arguments = String.Format("-c \"import {0}\"", moduleName),
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                p.Start();
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    throw new Exception("python.exe exited with non-zero code " + p.ExitCode);
                }
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        [Fact]
        public void TestImports()
        {
            var module_names = new string[] {
                    //"isis_meta", not used anywhere
                    "MaterialLibraryInterface",
                    "meta_nrmm",
                    "PCC",
                    "py_modelica",
                    "py_modelica_exporter",

                    "SpiceVisualizer",
                    "SpiceVisualizer.post_process",
                    "SpiceVisualizer.process_spice",
                    "SpiceVisualizer.spicedatareader",
                    "chipfit_display",
            };

            foreach (var test in module_names.Select(moduleName =>
                {
                    Func<Exception> m = () => ImportModule(moduleName);
                    var asyncResult = m.BeginInvoke(null, null);
                    return new Tuple<string, IAsyncResult, Func<Exception>>(moduleName, asyncResult, m);
                }).ToList())
            {
                Exception e = test.Item3.EndInvoke(test.Item2);
                if (e != null)
                {
                    Assert.True(e == null, "Importing " + test.Item1 + " failed: " + e.ToString());
                }
            }
        }

        [Fact]
        public void TestPythonExitCode()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                FileName = VersionInfo.PythonVEnvExe,
                Arguments = String.Format("-c \"import py_modelica; import scipy.io; import sys; sys.exit(42);\""),
                CreateNoWindow = true,
                UseShellExecute = false
            };
            p.Start();
            p.WaitForExit();
            Assert.Equal(42, p.ExitCode);
        }
    }
}
