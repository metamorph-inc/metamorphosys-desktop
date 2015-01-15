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
using Newtonsoft.Json;
using System.Reflection;

namespace BinPackTest
{
    public class TBManifestFixture
    {
        public TBManifestFixture()
        {
            Assert.True(File.Exists(TBManifest.pathExecutable),
                        String.Format("MaxRectsBinPack executable could not be found at {0}",
                                      TBManifest.pathExecutable));
        }
    }

    public class TBManifest : IUseFixture<TBManifestFixture>
    {
        #region Fixture
        TBManifestFixture fixture;
        public void SetFixture(TBManifestFixture data)
        {
            fixture = data;
        }
        #endregion

        String pathTest = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring("file:///".Length)),
                                "..", "..", "testfiles");
        public static String pathExecutable = Path.Combine(META.VersionInfo.MetaPath,
                                                           "bin",
                                                           "MaxRectsBinPack.exe");

        private void runCommand(String nameManifest)
        {
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = pathExecutable,
                    Arguments = "layout-input.json " + nameManifest,
                    WorkingDirectory = pathTest,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Give it 30 seconds to execute.
            Assert.True(process.WaitForExit(30*1000),
                        "MaxRectsBinPack timed out (over 30 seconds)");

            Assert.Equal(0, process.ExitCode);
        }

        [Fact]
        public void RunsWithoutManifestMod()
        {
            runCommand("");
        }

        [Fact]
        public void NoMetrics()
        {
            String pathOriginalManifest = Path.Combine(pathTest,
                                                       "org_tb_manifest.no_metrics.json");
            String pathManifestCopy = Path.Combine(pathTest,
                                                   "tb_manifest.no_metrics.json");
            File.Copy(pathOriginalManifest, pathManifestCopy, true);
            Assert.True(File.Exists(pathManifestCopy));

            runCommand("tb_manifest.no_metrics.json");

            String json = File.ReadAllText(pathManifestCopy);
            var manifest = JsonConvert.DeserializeObject<AVM.DDP.MetaTBManifest>(json);

            Assert.Equal(2, manifest.Metrics.Count);

            var fits = manifest.Metrics.First(m => m.Name.StartsWith("fits_"));
            Assert.NotNull(fits);
            Assert.Equal("true", fits.Value);

            var occupied = manifest.Metrics.First(m => m.Name.StartsWith("pct_occupied_"));
            Assert.NotNull(occupied);
            Assert.DoesNotThrow(() => float.Parse(occupied.Value));
        }

        [Fact]
        public void OldValues()
        {
            String pathOriginalManifest = Path.Combine(pathTest,
                                                       "org_tb_manifest.old_values.json");
            String pathManifestCopy = Path.Combine(pathTest,
                                                   "tb_manifest.old_values.json");
            File.Copy(pathOriginalManifest, pathManifestCopy, true);
            Assert.True(File.Exists(pathManifestCopy));

            runCommand("tb_manifest.old_values.json");

            String json = File.ReadAllText(pathManifestCopy);
            var manifest = JsonConvert.DeserializeObject<AVM.DDP.MetaTBManifest>(json);

            Assert.Equal(2, manifest.Metrics.Count);

            var fits = manifest.Metrics.First(m => m.Name.StartsWith("fits_"));
            Assert.NotNull(fits);
            Assert.Equal("true", fits.Value);

            var occupied = manifest.Metrics.First(m => m.Name.StartsWith("pct_occupied_"));
            Assert.NotNull(occupied);
            Assert.DoesNotThrow(() => float.Parse(occupied.Value));
        }

        [Fact]
        public void ManifestDoesNotExist()
        {
            String pathManifest = Path.Combine(pathTest,
                                               "manifest_does_not_exists.json");

            runCommand("tb_manifest.old_values.json");
        }
    }
}
