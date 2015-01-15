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
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Xunit;

using GME.MGA;
using GME.CSharp;

namespace GenericSubtreeSwapTest
{

    public class Tests
    {
        private static string gmepyPath = Path.Combine(META.VersionInfo.MetaPath, "externals", "common-scripts", "gmepy.exe");

        private int processCommon(Process process)
        {
            process.StartInfo.UseShellExecute = false;

            process.Start();
            process.WaitForExit();

            return process.ExitCode;
        }

        private void unpackXmes(string testName) {
            unpackXme(Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest", "Models", testName, "Master.xme"));
            unpackXme(Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest", "Models", testName, "Secondary.xme"));
            unpackXme(Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest", "Models", testName, "Final_(expected).xme"));
        }

        private void unpackXme(string xmeFilename)
        {
            if (!File.Exists(xmeFilename)) return;
            string mgaFilename = Path.ChangeExtension(xmeFilename, "mga");
            GME.MGA.MgaUtils.ImportXME(xmeFilename, mgaFilename);
        }

        private int runSubTreeMerge(string testName) {
            var process = new Process();
            process.StartInfo.FileName = Path.Combine(META.VersionInfo.MetaPath, "src", "SubTreeMergeCL", "bin", "Release", "SubTreeMergeCL.exe");
            process.StartInfo.WorkingDirectory = Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest");

            process.StartInfo.Arguments += "Models/" + testName + "/Master.mga";
            process.StartInfo.Arguments += " \"Models/" + testName + "/Secondary.mga/DesignSpace ricardoSystemAssembly/ricardoSystemAssembly/SubsystemToReplace\"";

            return processCommon(process);
        }

        private int runCyPhyMLComparator(string testName)
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(META.VersionInfo.MetaPath, "src", "bin", "CyPhyMLComparator.exe");
            process.StartInfo.WorkingDirectory = Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest");

            process.StartInfo.Arguments += "Models/" + testName + "/Master.mga";
            process.StartInfo.Arguments += " \"Models/" + testName + "/Final_(expected).mga\"";

            return processCommon(process);
        }

        [Fact]
        public void MergeTest()
        {
            string testName = "GenericSubtreeSwapTest";
            unpackXmes(testName);
            Assert.Equal(runSubTreeMerge(testName), 0);
            Assert.Equal(runCyPhyMLComparator(testName), 0);
        }


        private static MgaProject GetProject(String filename) {
            MgaProject result = null;

            if (filename != null && filename != "") {
                if (Path.GetExtension(filename) == ".mga") {
                    result = new MgaProject();
                    if (System.IO.File.Exists(filename)) {
                        Console.Out.Write("Opening {0} ... ", filename);
                        bool ro_mode;
                        result.Open("MGA=" + filename, out ro_mode);
                    } else {
                        Console.Out.Write("Creating {0} ... ", filename);
                        result.Create("MGA=" + filename, "CyPhyML");
                    }
                    Console.Out.WriteLine("Done.");
                } else {
                    Console.Error.WriteLine("{0} file must be an mga project.", filename);
                }
            } else {
                Console.Error.WriteLine("Please specify an Mga project.");
            }

            return result;
        }

        public bool checkForErasure() {

            bool retval = false;

            MgaProject mainMgaProject = GetProject(Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest", "Models", "MergeOverwriteTest", "Master.mga"));
            try
            {

                if (mainMgaProject == null)
                {
                    return false;
                }

                MgaGateway mgaGateway = new MgaGateway(mainMgaProject);

                mainMgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
                mgaGateway.PerformInTransaction(delegate
                {
                    MgaFCO currentObject = mainMgaProject.get_ObjectByPath("/DesignSpace ricardoSystemAssembly/ricardoSystemAssembly/SubsystemToReplace/ShouldBeErased") as MgaFCO;
                    retval = currentObject == null;
                });
            }
            finally
            {
                mainMgaProject.Close();
            }

            return retval;
        }


        [Fact]
        public void MergeOverwriteTest() {
            string testName = "MergeOverwriteTest";
            unpackXmes(testName);
            Assert.Equal(runSubTreeMerge(testName), 0);
            Assert.Equal(runCyPhyMLComparator(testName), 0);
            Assert.Equal(checkForErasure(), true);
        }

        public bool checkForPortErasure() {

            bool retval = false;

            MgaProject mainMgaProject = GetProject(Path.Combine(META.VersionInfo.MetaPath, "test", "GenericSubtreeSwapTest", "Models", "TopLevelPortDeleteTest", "Master.mga"));

            if (mainMgaProject == null) {
                return false;
            }

            MgaGateway mgaGateway = new MgaGateway(mainMgaProject);

            mainMgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(delegate {
                MgaFCO currentObject = mainMgaProject.get_ObjectByPath("/DesignSpace ricardoSystemAssembly/ricardoSystemAssembly/SubsystemToReplace/pin_n_alternator") as MgaFCO;
                retval = currentObject == null;
            });

            return retval;
        }

        [Fact]
        public void TopLevelPortDeleteTest() {
            string testName = "TopLevelPortDeleteTest";
            unpackXmes(testName);
            Assert.Equal(runSubTreeMerge(testName), 0);
            Assert.Equal(runCyPhyMLComparator(testName), 0);
            Assert.Equal(checkForPortErasure(), true);
        }

        [Fact]
        public void SubtreeDisplacedTest() {
            string testName = "SubtreeDisplacedTest";
            unpackXmes(testName);
            Assert.Equal(runSubTreeMerge(testName), 4);
        }

        [Fact]
        public void ChangedReferenceTest() {
            string testName = "ChangedReferenceTest";
            unpackXmes(testName);
            Assert.Equal(runSubTreeMerge(testName), 4);
            Assert.Equal(runCyPhyMLComparator(testName), 0);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Tests tests = new Tests();
            tests.MergeTest();
        }
    }
}
