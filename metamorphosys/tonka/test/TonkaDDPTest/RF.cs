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
using GME.CSharp;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ComponentImporterUnitTests;

namespace TonkaACMTest
{
    public class RFFixture
    {
        public RFFixture()
        {
            // First, copy BlankInputModel/InputModel.xme into the test folder
            File.Delete(RF.inputMgaPath);
            GME.MGA.MgaUtils.ImportXME(RF.blankXMEPath, RF.inputMgaPath);
            Assert.True(File.Exists(RF.inputMgaPath), "InputModel.mga not found; import may have failed.");

            // Next, import the content model
            File.Delete(RF.inputMgaPath);
            GME.MGA.MgaUtils.ImportXME(RF.rfXMEPath, RF.rfMgaPath);
            Assert.True(File.Exists(RF.rfMgaPath),
                        String.Format("{0} not found; import may have failed.",
                                      Path.GetFileName(RF.inputMgaPath)
                                     )
                        );

            // Next, export all component models from the content model
            var args = String.Format("{0} -f {1}", RF.rfMgaPath, RF.modelOutputPath).Split();
            CyPhyComponentExporterCL.CyPhyComponentExporterCL.Main(args);

            Assert.True(Directory.Exists(RF.modelOutputPath), "Model output path doesn't exist; Exporter may have failed.");
        }
    }

    public class RF : IUseFixture<RFFixture>
    {
        #region Paths
        public static readonly string tonkaPath = Path.GetFullPath(
                                                      Path.Combine(META.VersionInfo.MetaPath,
                                                                   "..",
                                                                   "tonka")
                                                                   );
        public static readonly string testPath = Path.Combine(tonkaPath,
                                                              "models",
                                                              "ACMTestModels",
                                                              "RF");
        public static readonly string blankXMEPath = Path.Combine(META.VersionInfo.MetaPath,
                                                                  "test",
                                                                  "InterchangeTest",
                                                                  "ComponentInterchangeTest",
                                                                  "SharedModels",
                                                                  "BlankInputModel",
                                                                  "InputModel.xme");
        public static readonly string inputMgaPath = Path.Combine(testPath,
                                                                  "InputModel.mga");
        public static readonly string rfXMEPath = Path.Combine(testPath,
                                                                      "RFModel.xme");
        public static readonly string rfMgaPath = Path.Combine(testPath,
                                                                      "RFModel.mga");
        public static readonly string modelOutputPath = Path.Combine(testPath,
                                                                     "acm");
        #endregion

        #region Fixture
        RFFixture fixture;
        public void SetFixture(RFFixture data)
        {
            fixture = data;
        }
        #endregion

        // The round-trip test will catch any inconsistencies.
        // The ACM export comparison succeeds/fails nondeterministically.
        // Let's skip it for now.
        //[Fact]
        public void OutputTest()
        {
            var list_Comparisons = new List<String>()
            {
                "BasicAttributes",
                "Ports",
                "Mappings"
            };

            var list_NotGenerated = new List<String>();
            var list_DidNotMatch = new List<String>();

            foreach (var name in list_Comparisons)
            {
                var path_Expected = Path.Combine(testPath, name + ".expected.acm");
                var path_Generated = Path.Combine(modelOutputPath, name + ".component.acm");

                if (false == File.Exists(path_Generated))
                    list_NotGenerated.Add(path_Generated);

                if (0 != Common.RunXmlComparator(path_Generated, path_Expected))
                    list_DidNotMatch.Add(path_Generated);
            }

            if (list_NotGenerated.Any())
            {
                String failed = "";
                list_NotGenerated.ForEach(x => failed += "\n" + x);
                Assert.True(false, "These expected files weren't generated: " + failed);
            }
            if (list_DidNotMatch.Any())
            {
                String failed = "";
                list_DidNotMatch.ForEach(x => failed += "\n" + x);
                Assert.True(false, "These generated files didn't match expected: " + failed);
            }
        }

        [Fact]
        public void RoundTripTest()
        {
            // Run Importer
            var args = String.Format("-r {0} {1}", modelOutputPath, inputMgaPath).Split();
            var importer_result = CyPhyComponentImporterCL.CyPhyComponentImporterCL.Main(args);
            Assert.True(0 == importer_result, "Importer had non-zero return code.");

            // Compare
            var comp_result = Common.RunCyPhyMLComparator(rfMgaPath, inputMgaPath);
            Assert.True(comp_result == 0, "Imported model doesn't match expected.");
        }

        [Fact]
        public void PythonLibraryTest()
        {
            // Find all exported ACM files
            var acms = Directory.EnumerateFiles(modelOutputPath, "*.acm", SearchOption.AllDirectories);
            ConcurrentBag<String> cb_Failures = new ConcurrentBag<String>();
            Parallel.ForEach(acms, pathACM =>
            {
                var absPathACM = Path.Combine(modelOutputPath, pathACM);
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
