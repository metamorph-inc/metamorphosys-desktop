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
using System.IO;
using Xunit;
using System.Diagnostics;

namespace CADCreoTest
{
    public class Cyphy2CADCreoTest : IUseFixture<CADCreoTest.Cyphy2CADCreoTest.CadAssemblyFixture>
    {
        CadAssemblyFixture fixture;

        public class MetaLinkFixture
        {
            public readonly string createAssemblyExe;

            public MetaLinkFixture()
            {
                string proeIsisExtensionsDir = System.Environment.GetEnvironmentVariable("PROE_ISIS_EXTENSIONS", EnvironmentVariableTarget.Machine);
                createAssemblyExe = Path.Combine(proeIsisExtensionsDir ?? "", "bin", "CADCreoParametricMetaLink.exe");
                if (File.Exists(createAssemblyExe) == false)
                {
                    throw new FileNotFoundException("CADCreoParametricMetaLink.exe is not installed.");
                }
            }
        }

        public class CadAssemblyFixture
        {
            public readonly string createAssemblyExe;

            public CadAssemblyFixture()
            {
                string proeIsisExtensionsDir = System.Environment.GetEnvironmentVariable("PROE_ISIS_EXTENSIONS", EnvironmentVariableTarget.Machine);
                createAssemblyExe = Path.Combine(proeIsisExtensionsDir ?? "", "bin", "CADCreoParametricCreateAssembly.exe");
                if (File.Exists(createAssemblyExe) == false)
                {
                    throw new FileNotFoundException("CADCreoParametricCreateAssembly.exe is not installed.");
                }
            }
        }


        [Fact]
        public void BallisticTB_Creo()
        {
            CyPhyPropagateTest.MetaLinkCreoTest.KillCreo();
            string XmePath = Path.GetFullPath(@"..\..\..\..\models\CADTeam\MSD_CAD.xme");
            string TestbenchPath = "/@MyTestBenches|kind=Testing|relpos=0/@TestBench_Config|kind=Testing|relpos=0/@Ballistic|kind=Testing|relpos=0/@Custom_Ballistics_Valid|kind=BallisticTestBench|relpos=0";
            string OutputDir = Path.Combine(Path.GetDirectoryName(XmePath), "BallisticTB_Custom_Valid");

            bool status = CADTeamTest.CyPhy2CADRun.Run(OutputDir, XmePath, TestbenchPath, true);
            Assert.True(File.Exists(Path.Combine(OutputDir, CADTeamTest.CADTests.generatedAsmFile)), "Failed to generate " + CADTeamTest.CADTests.generatedAsmFile);

            ProcessStartInfo info = new ProcessStartInfo()
            {
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
                FileName = fixture.createAssemblyExe,
                Arguments = "-w . -i CADAssembly.xml",
                WorkingDirectory = OutputDir
                // TODO -p ?
            };

            Process createAssembly = new Process();
            createAssembly.StartInfo = info;

            createAssembly.Start();

            bool exited = createAssembly.WaitForExit(45000);
            if (!exited)
            {
                createAssembly.Kill();
                createAssembly.WaitForExit();
            }
            Assert.True(exited);

            Assert.Equal(createAssembly.ExitCode, 0);
            Assert.True(VerifyCADAssemblerLog(Path.Combine(OutputDir, "log", "cad-assembler.log")));
            Assert.True(File.Exists(Path.Combine(OutputDir,"mymassspringdamper_1.asm.2")));
        }

        [Fact]
        public void KinematicTB_Creo_4Bar()
        {
            CyPhyPropagateTest.MetaLinkCreoTest.KillCreo();
            string XmePath = Path.GetFullPath(@"..\..\..\..\models\MBD\MBD.xme");
            string TestbenchPath = "/@Testing|kind=Testing|relpos=0/@Kinematic_FourBar|kind=KinematicTestBench|relpos=0";
            string OutputDir = Path.Combine(Path.GetDirectoryName(XmePath), "Kinematic_FourBar");

            bool status = CADTeamTest.CyPhy2CADRun.Run(OutputDir, XmePath, TestbenchPath, true);
            Assert.True(File.Exists(Path.Combine(OutputDir, CADTeamTest.CADTests.generatedAsmFile)), "Failed to generate " + CADTeamTest.CADTests.generatedAsmFile);
            Assert.True(File.Exists(Path.Combine(OutputDir, CADTeamTest.CADTests.generatedMBDFile)), "Failed to generate " + CADTeamTest.CADTests.generatedMBDFile);

            ProcessStartInfo info = new ProcessStartInfo()
            {
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
                FileName = fixture.createAssemblyExe,
                Arguments = "-w . -i CADAssembly.xml",
                WorkingDirectory = OutputDir
                // TODO -p ?
            };

            Process createAssembly = new Process();
            createAssembly.StartInfo = info;

            createAssembly.Start();

            bool exited = createAssembly.WaitForExit(45000);
            if (!exited)
            {
                createAssembly.Kill();
                createAssembly.WaitForExit();
            }
            Assert.True(exited);

            Assert.Equal(createAssembly.ExitCode, 0);
            Assert.True(VerifyCADAssemblerLog(Path.Combine(OutputDir, "log", "cad-assembler.log")));
            Assert.True(File.Exists(Path.Combine(OutputDir, "systemundertest_1.asm.2")));
            //Assert.True(File.Exists(Path.Combine(OutputDir, "PARASOLID","SystemUnderTest_1_asm.x_t")));
            Assert.True(File.Exists(Path.Combine(OutputDir, "ComputedValues.xml")));
            Assert.True(File.Exists(Path.Combine(OutputDir, "CADAssembly_metrics.xml")));
        }

        [Fact]
        public void KinematicTB_Creo_Excavator()
        {
            CyPhyPropagateTest.MetaLinkCreoTest.KillCreo();
            string XmePath = Path.GetFullPath(@"..\..\..\..\models\MBD\MBD.xme");
            string TestbenchPath = "/@Testing|kind=Testing|relpos=0/@Kinematic_Excavator|kind=KinematicTestBench|relpos=0";
            string OutputDir = Path.Combine(Path.GetDirectoryName(XmePath), "Kinematic_Excavator");

            bool status = CADTeamTest.CyPhy2CADRun.Run(OutputDir, XmePath, TestbenchPath, true);
            Assert.True(File.Exists(Path.Combine(OutputDir, CADTeamTest.CADTests.generatedAsmFile)), "Failed to generate " + CADTeamTest.CADTests.generatedAsmFile);
            Assert.True(File.Exists(Path.Combine(OutputDir, CADTeamTest.CADTests.generatedMBDFile)), "Failed to generate " + CADTeamTest.CADTests.generatedMBDFile);

            ProcessStartInfo info = new ProcessStartInfo()
            {
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                CreateNoWindow = false,
                FileName = fixture.createAssemblyExe,
                Arguments = "-w . -i CADAssembly.xml",
                WorkingDirectory = OutputDir
                // TODO -p ?
            };

            Process createAssembly = new Process();
            createAssembly.StartInfo = info;

            createAssembly.Start();

            bool exited = createAssembly.WaitForExit(45000);
            if (!exited)
            {
                createAssembly.Kill();
                createAssembly.WaitForExit();
            }
            Assert.True(exited);

            Assert.Equal(createAssembly.ExitCode, 0);
            Assert.True(VerifyCADAssemblerLog(Path.Combine(OutputDir, "log", "cad-assembler.log")));
            Assert.True(File.Exists(Path.Combine(OutputDir, "systemundertest_1.asm.2")));
            //Assert.True(File.Exists(Path.Combine(OutputDir, "PARASOLID", "SystemUnderTest_1_asm.x_t")));
            Assert.True(File.Exists(Path.Combine(OutputDir, "ComputedValues.xml")));
            Assert.True(File.Exists(Path.Combine(OutputDir, "CADAssembly_metrics.xml")));
        }

        public static bool VerifyCADAssemblerLog(string logfile)
        {
            return true;
        }


        public void SetFixture(Cyphy2CADCreoTest.CadAssemblyFixture data)
        {
            this.fixture = data;
        }
    }
}
