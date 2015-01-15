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
using System.Diagnostics;

namespace TonkaACMTest
{
    class Common
    {
        public static int RunCyPhyMLComparator(string desired, string imported)
        {
            var path = Path.GetDirectoryName(desired);
            var process = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(META.VersionInfo.MetaPath, "src", "bin", "CyPhyMLComparator.exe")
                }
            };

            process.StartInfo.Arguments += desired;
            process.StartInfo.Arguments += " " + imported;

            return processCommon(process, true);
        }

        public static int processCommon(Process process, bool redirect = false)
        {
            using (process)
            {
                process.StartInfo.UseShellExecute = false;

                if (redirect)
                {
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                }

                process.StartInfo.CreateNoWindow = true;
                process.Start();
                if (redirect)
                {
                    char[] buffer = new char[4096];
                    while (true)
                    {
                        int read = process.StandardError.Read(buffer, 0, 4096);
                        if (read == 0)
                        {
                            break;
                        }
                        Console.Error.Write(buffer, 0, read);
                    }

                    buffer = new char[4096];
                    while (true)
                    {
                        int read = process.StandardOutput.Read(buffer, 0, 4096);
                        if (read == 0)
                        {
                            break;
                        }
                        Console.Out.Write(buffer, 0, read);
                    }
                }
                process.WaitForExit();

                return process.ExitCode;
            }
        }

        public static int RunXmlComparator(string exported, string desired)
        {
            string xmlComparatorPath = Path.Combine(
                META.VersionInfo.MetaPath,
                "test",
                "InterchangeTest",
                "InterchangeXmlComparator",
                "bin",
                "Release",
                "InterchangeXmlComparator.exe"
                );

            var process = new Process
            {
                StartInfo =
                {
                    FileName = xmlComparatorPath
                }
            };

            process.StartInfo.Arguments += String.Format(" -e {0} -d {1} -m Component", exported, desired);
            return processCommon(process, true);
        }
    }
}
