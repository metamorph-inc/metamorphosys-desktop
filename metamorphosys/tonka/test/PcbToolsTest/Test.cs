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

namespace PcbToolsTest
{
    public class Test
    {
        [Fact]
        public void PreRoutedNominal()
        {
            //////// Layout Solver section
            var pathLayoutJson = Path.Combine(pathTest, "layout.json");
            if (File.Exists(pathLayoutJson))
            {
                File.Delete(pathLayoutJson);
            }

            using (var proc = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        FileName = pathLayoutSolver,
                        Arguments = "layout-input.json layout.json",
                        WorkingDirectory = pathTest,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                })
            {
                proc.Start();
                Assert.True(proc.WaitForExit(5000));
                Assert.True(0 == proc.ExitCode, "Non-zero exit code " + proc.ExitCode.ToString() + ": " + proc.StandardError.ReadToEnd());
            }
            
            // Check that output file is different than input.
            // It's not a great test, but it's a start.
            Assert.True(File.Exists(pathLayoutJson));
            var jsonLayoutJson = File.ReadAllText(pathLayoutJson);
            var jsonLayoutInputJson = File.ReadAllText(Path.Combine(pathTest, "layout-input.json"));
            Assert.NotEqual(jsonLayoutInputJson, jsonLayoutJson);


            //////// Board Synthesis section
            var pathSchemaSch = Path.Combine(pathTest, "schema.sch");
            var orgContentsSchemaSch = File.ReadAllText(pathSchemaSch);

            var pathSchemaBrd = Path.Combine(pathTest, "schema.brd");
            if (File.Exists(pathSchemaBrd))
            {
                File.Delete(pathSchemaBrd);
            }

            using (var proc = new System.Diagnostics.Process()
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        FileName = pathBoardSynthesis,
                        Arguments = "schema.sch layout.json",
                        WorkingDirectory = pathTest,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                })
            {
                proc.Start();
                Assert.True(proc.WaitForExit(5000));
                Assert.Equal(0, proc.ExitCode);
                Assert.True(0 == proc.ExitCode, proc.StandardError.ToString());
            }

            // Ensure schema.sch hasn't changed
            Assert.Equal(orgContentsSchemaSch, File.ReadAllText(pathSchemaSch));

            // Ensure board file was generated
            Assert.True(File.Exists(pathSchemaBrd));
        }
        

        private static String pathLayoutSolver = Path.Combine(META.VersionInfo.MetaPath,
                                                              "bin",
                                                              "LayoutSolver.exe");

        private static String pathBoardSynthesis = Path.Combine(META.VersionInfo.MetaPath,
                                                                "..",
                                                                "tonka",
                                                                "src",
                                                                "BoardSynthesis",
                                                                "bin",
                                                                "Release",
                                                                "BoardSynthesis.exe");

        private static String pathTest = Path.Combine(META.VersionInfo.MetaPath,
                                                      "..",
                                                      "tonka",
                                                      "test",
                                                      "PcbToolsTest",
                                                      "PreRouted");
    }
}
