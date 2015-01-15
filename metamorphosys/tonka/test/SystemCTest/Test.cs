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
using Xunit;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GME.MGA;

namespace SystemCTest
{
    class SystemCExecutable
    {
        public String executable;
        public String outputFile;
        public int rtnCode;

        public bool OutputFileExists(string testFolder)
        {
            return File.Exists(Path.Combine(testFolder, outputFile));
        }
    };
    
    public class Test
    {
        [Fact(Skip="Known Broken - MOT-449")]
        public void TestsRanSuccessfully()
        {
            string simModelPath = Path.GetFullPath(Path.Combine(@"..\..\..\..\",
                "models",
                "SystemC",
                "Release"));

            var list_Executables = new List<SystemCExecutable>();

            foreach (var exec in Directory.GetFiles(simModelPath, "*.exe")
                                          .Where(f => false == 
                                                      (  f.ToLower().Contains("test_scbus.exe")
                                                      || f.ToLower().Contains("test_scbusapp.exe"))))
            {
                SystemCExecutable sce = new SystemCExecutable()
                {
                    executable = exec,
                    outputFile = Path.GetFileNameWithoutExtension(exec) + ".vcd"
                };
                list_Executables.Add(sce);
            }

            Parallel.ForEach(list_Executables, exec =>
            {
                // Run and capture return code
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = Path.Combine(simModelPath, exec.executable),
                        WorkingDirectory = simModelPath,
                        CreateNoWindow = true
                    }
                };
                exec.rtnCode = processCommon(process);
            });

            int numFailures = 0;
            String msg = "";
            foreach (var exec in list_Executables)
            {
                if (exec.rtnCode != 0)
                {
                    numFailures++;
                    msg += String.Format("{0} had non-zero return code of {1}" + Environment.NewLine, exec.executable, exec.rtnCode);
                }
                else if (!exec.OutputFileExists(simModelPath))
                {
                    numFailures++;
                    msg += String.Format("{0} did not produce a VCD output file ({1} expected)" + Environment.NewLine, exec.executable, exec.outputFile);
                }
            }
            Assert.True(numFailures == 0, msg);
        }
        
        private int processCommon(Process process)
        {
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            if (process.WaitForExit(1000 * 60 * 5) == false)
            {
                process.Kill();
                throw new TimeoutException(process.StartInfo.FileName + " did not complete in 5 minutes");
            }

            return process.ExitCode;
        }
    }
}
