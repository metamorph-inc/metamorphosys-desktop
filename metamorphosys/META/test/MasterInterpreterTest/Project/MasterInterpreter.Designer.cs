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
using GME.MGA;

namespace MasterInterpreterTest.Projects
{
    public class MasterInterpreterFixture
    {
        internal string mgaFile = null;

        public MasterInterpreterFixture()
        {
            this.mgaFile = Test.ImportXME2Mga("MasterInterpreter", "MasterInterpreter.xme");
        }
    }

    public class MasterInterpreterModelImport
    {
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("ProjectImport/Open", "MasterInterpreter")]
        public void ProjectXmeImport()
        {
            Assert.DoesNotThrow(() => { new MasterInterpreterFixture(); });
        }
    }

    public partial class MasterInterpreter : IUseFixture<MasterInterpreterFixture>
    {
        internal string mgaFile { get { return this.fixture.mgaFile; } }
        private MasterInterpreterFixture fixture { get; set; }

        public void SetFixture(MasterInterpreterFixture data)
        {
            this.fixture = data;
        }

        //[Fact]
        //[Trait("Model", "MasterInterpreter")]
        //[Trait("ProjectImport/Open", "MasterInterpreter")]
        //public void ProjectXmeImport()
        //{
        //    Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");
        //}

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("ProjectImport/Open", "MasterInterpreter")]
        public void ProjectMgaOpen()
        {
            var mgaReference = "MGA=" + mgaFile;

            MgaProject project = new MgaProject();
            project.OpenEx(mgaReference, "CyPhyML", null);
            MgaHelper.CheckParadigmVersionUpgrade(project);
            project.Close(true);
            Assert.True(File.Exists(mgaReference.Substring("MGA=".Length)));
        }



        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_BallisticHasTestInjectionPoint_Invalid()
        {
            string outputDir = "BallisticTestBench_BallisticHasTestInjectionPoint_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@BallisticHasTestInjectionPoint|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_BallisticTargetNotDescendantOfSystemUnderTest_Invalid()
        {
            string outputDir = "BallisticTestBench_BallisticTargetNotDescendantOfSystemUnderTest_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@BallisticTargetNotDescendantOfSystemUnderTest|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_BallisticTargetNullRef_Invalid()
        {
            string outputDir = "BallisticTestBench_BallisticTargetNullRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@BallisticTargetNullRef|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_CriticalComponentNotDescendantOfSystemUnderTest_Invalid()
        {
            string outputDir = "BallisticTestBench_CriticalComponentNotDescendantOfSystemUnderTest_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CriticalComponentNotDescendantOfSystemUnderTest|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_CriticalComponentNullReference_Invalid()
        {
            string outputDir = "BallisticTestBench_CriticalComponentNullReference_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CriticalComponentNullReference|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_MoreThanOnePredefinedBallisticSuite_Invalid()
        {
            string outputDir = "BallisticTestBench_MoreThanOnePredefinedBallisticSuite_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOnePredefinedBallisticSuite|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_MoreThanOneReferencePlane_Invalid()
        {
            string outputDir = "BallisticTestBench_MoreThanOneReferencePlane_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneReferencePlane|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_NoPredefinedBallisticSuite_Invalid()
        {
            string outputDir = "BallisticTestBench_NoPredefinedBallisticSuite_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoPredefinedBallisticSuite|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_NoReferencePlane_Invalid()
        {
            string outputDir = "BallisticTestBench_NoReferencePlane_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoReferencePlane|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_NoShotLineModel_Invalid()
        {
            string outputDir = "BallisticTestBench_NoShotLineModel_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoShotLineModel|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_OneShotLineModelAndOnePredefinedBallisticSuiteWithBallisticTarget_Invalid()
        {
            string outputDir = "BallisticTestBench_OneShotLineModelAndOnePredefinedBallisticSuiteWithBallisticTarget_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@OneShotLineModelAndOnePredefinedBallisticSuiteWithBallisticTarget|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BallisticTestBench_OneShotLineModelAndOnePredefinedBallisticSuiteWithCriticalComponent_Invalid()
        {
            string outputDir = "BallisticTestBench_OneShotLineModelAndOnePredefinedBallisticSuiteWithCriticalComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@OneShotLineModelAndOnePredefinedBallisticSuiteWithCriticalComponent|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_CriticalComponentMultipleReferredCA_Valid()
        {
            string outputDir = "BallisticTestBench_CriticalComponentMultipleReferredCA_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@CriticalComponentMultipleReferredCA|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_CriticalComponentSingleReferredCA_Valid()
        {
            string outputDir = "BallisticTestBench_CriticalComponentSingleReferredCA_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@CriticalComponentSingleReferredCA|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_CustomOneBallisticTarget_Valid()
        {
            string outputDir = "BallisticTestBench_CustomOneBallisticTarget_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@CustomOneBallisticTarget|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_CustomTwoBallisticTargets_Valid()
        {
            string outputDir = "BallisticTestBench_CustomTwoBallisticTargets_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@CustomTwoBallisticTargets|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_MoreThanOneShotLineModel_Valid()
        {
            string outputDir = "BallisticTestBench_MoreThanOneShotLineModel_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@MoreThanOneShotLineModel|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_NoCriticalComponent_Valid()
        {
            string outputDir = "BallisticTestBench_NoCriticalComponent_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@NoCriticalComponent|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_PredefinedOneCriticalComponent_Valid()
        {
            string outputDir = "BallisticTestBench_PredefinedOneCriticalComponent_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@PredefinedOneCriticalComponent|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BallisticTestBench_PredefinedTwoCriticalComponents_Valid()
        {
            string outputDir = "BallisticTestBench_PredefinedTwoCriticalComponents_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BallisticTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@PredefinedTwoCriticalComponents|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BlastTestBench_BlastHasTestInjectionPoint_Invalid()
        {
            string outputDir = "BlastTestBench_BlastHasTestInjectionPoint_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@BlastHasTestInjectionPoint|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BlastTestBench_HasNoBlastModelNorPredefinedBlastSuite_Invalid()
        {
            string outputDir = "BlastTestBench_HasNoBlastModelNorPredefinedBlastSuite_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@HasNoBlastModelNorPredefinedBlastSuite|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BlastTestBench_MoreThanOneBlastModel_Invalid()
        {
            string outputDir = "BlastTestBench_MoreThanOneBlastModel_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneBlastModel|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BlastTestBench_MoreThanOnePredefinedBlastSuite_Invalid()
        {
            string outputDir = "BlastTestBench_MoreThanOnePredefinedBlastSuite_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOnePredefinedBlastSuite|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BlastTestBench_MoreThanOneReferencePlane_Invalid()
        {
            string outputDir = "BlastTestBench_MoreThanOneReferencePlane_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneReferencePlane|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void BlastTestBench_NoReferencePlane_Invalid()
        {
            string outputDir = "BlastTestBench_NoReferencePlane_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoReferencePlane|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BlastTestBench_HasBlastModel_Valid()
        {
            string outputDir = "BlastTestBench_HasBlastModel_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@HasBlastModel|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void BlastTestBench_HasPredefinedBlastSuite_Valid()
        {
            string outputDir = "BlastTestBench_HasPredefinedBlastSuite_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@BlastTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@HasPredefinedBlastSuite|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToNull_Invalid()
        {
            string outputDir = "CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToNull_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@ComponentAssemblyTipPointsToComponentRefPointsToNull|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToTestComponent_Invalid()
        {
            string outputDir = "CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToTestComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@ComponentAssemblyTipPointsToComponentRefPointsToTestComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_DesignSpaceTipPointsToComponentRefPointsToNull_Invalid()
        {
            string outputDir = "CADTestBench_DesignSpaceTipPointsToComponentRefPointsToNull_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@DesignSpaceTipPointsToComponentRefPointsToNull|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_DesignSpaceTipPointsToComponentRefPointsToTestComponent_Invalid()
        {
            string outputDir = "CADTestBench_DesignSpaceTipPointsToComponentRefPointsToTestComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@DesignSpaceTipPointsToComponentRefPointsToTestComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_FaceObjectInSolverTypeABAQUSDeckBasedTestBench_Invalid()
        {
            string outputDir = "CADTestBench_FaceObjectInSolverTypeABAQUSDeckBasedTestBench_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@FaceObjectInSolverTypeABAQUSDeckBasedTestBench|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_FaceObjectInSolverTypeNASTRANTestBench_Invalid()
        {
            string outputDir = "CADTestBench_FaceObjectInSolverTypeNASTRANTestBench_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@FaceObjectInSolverTypeNASTRANTestBench|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_NoTestInjectionPoint_Invalid()
        {
            string outputDir = "CADTestBench_NoTestInjectionPoint_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoTestInjectionPoint|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_TestInjectionPointNotADescendantOfSystemUnderTest_Invalid()
        {
            string outputDir = "CADTestBench_TestInjectionPointNotADescendantOfSystemUnderTest_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@TestInjectionPointNotADescendantOfSystemUnderTest|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_TestInjectionPointNullRef_Invalid()
        {
            string outputDir = "CADTestBench_TestInjectionPointNullRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@TestInjectionPointNullRef|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_TestInjectionPointPointsToComponent_Invalid()
        {
            string outputDir = "CADTestBench_TestInjectionPointPointsToComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@TestInjectionPointPointsToComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_TestInjectionPointPointsToTestComponent_Invalid()
        {
            string outputDir = "CADTestBench_TestInjectionPointPointsToTestComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@TestInjectionPointPointsToTestComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToComponent_Valid()
        {
            string outputDir = "CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToComponent_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@ComponentAssemblyTipPointsToComponentRefPointsToComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToComponentAssembly_Valid()
        {
            string outputDir = "CADTestBench_ComponentAssemblyTipPointsToComponentRefPointsToComponentAssembly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@ComponentAssemblyTipPointsToComponentRefPointsToComponentAssembly|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_DesignSpaceTipPointsToComponentRefPointsToComponent_Valid()
        {
            string outputDir = "CADTestBench_DesignSpaceTipPointsToComponentRefPointsToComponent_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@DesignSpaceTipPointsToComponentRefPointsToComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_DesignSpaceTipPointsToComponentRefPointsToComponentAssembly_Valid()
        {
            string outputDir = "CADTestBench_DesignSpaceTipPointsToComponentRefPointsToComponentAssembly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@DesignSpaceTipPointsToComponentRefPointsToComponentAssembly|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_FaceObjectInSolverTypeABAQUSModelBasedTestBench_Valid()
        {
            string outputDir = "CADTestBench_FaceObjectInSolverTypeABAQUSModelBasedTestBench_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@FaceObjectInSolverTypeABAQUSModelBasedTestBench|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_HasOneTestInjectionPoint_Valid()
        {
            string outputDir = "CADTestBench_HasOneTestInjectionPoint_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@HasOneTestInjectionPoint|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_MoreThanOneTestInjectionPoint_Valid()
        {
            string outputDir = "CADTestBench_MoreThanOneTestInjectionPoint_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@MoreThanOneTestInjectionPoint|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_MoreThanTestInjectionPointPointsToTheSameComponent_Valid()
        {
            string outputDir = "CADTestBench_MoreThanTestInjectionPointPointsToTheSameComponent_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@MoreThanTestInjectionPointPointsToTheSameComponent|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_TestInjectionPointPointsToAlternativeDesignContainer_Valid()
        {
            string outputDir = "CADTestBench_TestInjectionPointPointsToAlternativeDesignContainer_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@TestInjectionPointPointsToAlternativeDesignContainer|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_TestInjectionPointPointsToComponentAssembly_Valid()
        {
            string outputDir = "CADTestBench_TestInjectionPointPointsToComponentAssembly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@TestInjectionPointPointsToComponentAssembly|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_TestInjectionPointPointsToCompoundDesignContainer_Valid()
        {
            string outputDir = "CADTestBench_TestInjectionPointPointsToCompoundDesignContainer_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@TestInjectionPointPointsToCompoundDesignContainer|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CADTestBench_TestInjectionPointPointsToOptionalDesignContainer_Valid()
        {
            string outputDir = "CADTestBench_TestInjectionPointPointsToOptionalDesignContainer_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@StructuralFEATestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@TestInjectionPointPointsToOptionalDesignContainer|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CADTestBench_StructuralFEATestBench_TIP_Invalid_Invalid()
        {
            string outputDir = "CADTestBench_StructuralFEATestBench_TIP_Invalid_Invalid";
            string objectAbsPath = "/@011_TestingContextCheckerUseCases|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@StructuralFEATestBench_TIP_Invalid|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CalmWaterAndCorrelation_Invalid()
        {
            string outputDir = "CFDTestBench_CalmWaterAndCorrelation_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CalmWaterAndCorrelation|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CalmWaterAndCorrelationAndHydrostatics_Invalid()
        {
            string outputDir = "CFDTestBench_CalmWaterAndCorrelationAndHydrostatics_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CalmWaterAndCorrelationAndHydrostatics|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CalmWaterAndWaveResistance_Invalid()
        {
            string outputDir = "CFDTestBench_CalmWaterAndWaveResistance_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CalmWaterAndWaveResistance|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CalmWaterAndWaveResistanceAndCorrelation_Invalid()
        {
            string outputDir = "CFDTestBench_CalmWaterAndWaveResistanceAndCorrelation_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CalmWaterAndWaveResistanceAndCorrelation|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CalmWaterAndWaveResistanceAndCorrelationAndHydrostatics_Invalid()
        {
            string outputDir = "CFDTestBench_CalmWaterAndWaveResistanceAndCorrelationAndHydrostatics_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CalmWaterAndWaveResistanceAndCorrelationAndHydrostatics|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CalmWaterAndWaveResistanceAndHydrostatics_Invalid()
        {
            string outputDir = "CFDTestBench_CalmWaterAndWaveResistanceAndHydrostatics_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CalmWaterAndWaveResistanceAndHydrostatics|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_HasTestInjectionPoint_Invalid()
        {
            string outputDir = "CFDTestBench_HasTestInjectionPoint_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@HasTestInjectionPoint|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_MoreThanOneCalmWater_Invalid()
        {
            string outputDir = "CFDTestBench_MoreThanOneCalmWater_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneCalmWater|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_MoreThanOneCorrelation_Invalid()
        {
            string outputDir = "CFDTestBench_MoreThanOneCorrelation_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneCorrelation|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_MoreThanOneHydrostatics_Invalid()
        {
            string outputDir = "CFDTestBench_MoreThanOneHydrostatics_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneHydrostatics|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_MoreThanWaveResistance_Invalid()
        {
            string outputDir = "CFDTestBench_MoreThanWaveResistance_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanWaveResistance|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_NoCalmWaterNorWaveResistanceNorCorrelationNorHydrostatics_Invalid()
        {
            string outputDir = "CFDTestBench_NoCalmWaterNorWaveResistanceNorCorrelationNorHydrostatics_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoCalmWaterNorWaveResistanceNorCorrelationNorHydrostatics|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_WaveResistanceAndCorrelation_Invalid()
        {
            string outputDir = "CFDTestBench_WaveResistanceAndCorrelation_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@WaveResistanceAndCorrelation|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_WaveResistanceAndCorrelationAndHydrostatics_Invalid()
        {
            string outputDir = "CFDTestBench_WaveResistanceAndCorrelationAndHydrostatics_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@WaveResistanceAndCorrelationAndHydrostatics|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_HydrostaticsAndCalmWater_Valid()
        {
            string outputDir = "CFDTestBench_HydrostaticsAndCalmWater_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@HydrostaticsAndCalmWater|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_HydrostaticsAndCorrelation_Valid()
        {
            string outputDir = "CFDTestBench_HydrostaticsAndCorrelation_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@HydrostaticsAndCorrelation|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_HydrostaticsAndWaveResistance_Valid()
        {
            string outputDir = "CFDTestBench_HydrostaticsAndWaveResistance_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@HydrostaticsAndWaveResistance|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_OneCalmWaterOnly_Valid()
        {
            string outputDir = "CFDTestBench_OneCalmWaterOnly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@OneCalmWaterOnly|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_OneCorrelationOnly_Valid()
        {
            string outputDir = "CFDTestBench_OneCorrelationOnly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@OneCorrelationOnly|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_OneHydrostaticsOnly_Valid()
        {
            string outputDir = "CFDTestBench_OneHydrostaticsOnly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@OneHydrostaticsOnly|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void CFDTestBench_OneWaveResistanceOnly_Valid()
        {
            string outputDir = "CFDTestBench_OneWaveResistanceOnly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@CFDTestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@OneWaveResistanceOnly|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CFDTestBench_DS_Invalid_Invalid()
        {
            string outputDir = "CFDTestBench_CFDTestBench_DS_Invalid_Invalid";
            string objectAbsPath = "/@011_TestingContextCheckerUseCases|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CFDTestBench_DS_Invalid|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void CFDTestBench_CFDTestBench_Invalid_Invalid()
        {
            string outputDir = "CFDTestBench_CFDTestBench_Invalid_Invalid";
            string objectAbsPath = "/@011_TestingContextCheckerUseCases|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@CFDTestBench_Invalid|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_Empty_Invalid()
        {
            string outputDir = "ParametricExploration_Empty_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@Empty|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_HasNoDriver_Invalid()
        {
            string outputDir = "ParametricExploration_HasNoDriver_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@HasNoDriver|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_MoreThanOnePCCDriver_Invalid()
        {
            string outputDir = "ParametricExploration_MoreThanOnePCCDriver_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@MoreThanOnePCCDriver|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_MoreThanOneTestBenchRef_Invalid()
        {
            string outputDir = "ParametricExploration_MoreThanOneTestBenchRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@MoreThanOneTestBenchRef|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_NoTestBenchRef_Invalid()
        {
            string outputDir = "ParametricExploration_NoTestBenchRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@NoTestBenchRef|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_OptimizerAndParameterStudy_Invalid()
        {
            string outputDir = "ParametricExploration_OptimizerAndParameterStudy_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@OptimizerAndParameterStudy|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_PCCDriverAndOptimizer_Invalid()
        {
            string outputDir = "ParametricExploration_PCCDriverAndOptimizer_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@PCCDriverAndOptimizer|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_PCCDriverAndOptimizerAndParameterStudy_Invalid()
        {
            string outputDir = "ParametricExploration_PCCDriverAndOptimizerAndParameterStudy_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@PCCDriverAndOptimizerAndParameterStudy|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_PCCDriverAndParameterStudy_Invalid()
        {
            string outputDir = "ParametricExploration_PCCDriverAndParameterStudy_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@PCCDriverAndParameterStudy|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_PCCDriverOneTestBenchRefInstance_Invalid()
        {
            string outputDir = "ParametricExploration_PCCDriverOneTestBenchRefInstance_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@PCCDriverOneTestBenchRefInstance|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_PCCDriverOneTestBenchRefSubtype_Invalid()
        {
            string outputDir = "ParametricExploration_PCCDriverOneTestBenchRefSubtype_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@PCCDriverOneTestBenchRefSubtype|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void ParametricExploration_TestBenchRefNull_Invalid()
        {
            string outputDir = "ParametricExploration_TestBenchRefNull_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Invalid|kind=ParametricExplorationFolder|relpos=0/@TestBenchRefNull|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void ParametricExploration_OptimizerOneTestBenchRef_Valid()
        {
            string outputDir = "ParametricExploration_OptimizerOneTestBenchRef_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Valid|kind=ParametricExplorationFolder|relpos=0/@OptimizerOneTestBenchRef|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void ParametricExploration_ParameterStudyOneTestBenchRef_Valid()
        {
            string outputDir = "ParametricExploration_ParameterStudyOneTestBenchRef_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Valid|kind=ParametricExplorationFolder|relpos=0/@ParameterStudyOneTestBenchRef|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void ParametricExploration_PCCDriverOneTestBenchRef_Valid()
        {
            string outputDir = "ParametricExploration_PCCDriverOneTestBenchRef_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@Valid|kind=ParametricExplorationFolder|relpos=0/@PCCDriverOneTestBenchRef|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_ComponentRefTopLevelSystemUnderTestPointsToComponent_Invalid()
        {
            string outputDir = "TestBench_ComponentRefTopLevelSystemUnderTestPointsToComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@ComponentRefTopLevelSystemUnderTestPointsToComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_ComponentRefTopLevelSystemUnderTestPointsToComponentAssembly_Invalid()
        {
            string outputDir = "TestBench_ComponentRefTopLevelSystemUnderTestPointsToComponentAssembly_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@ComponentRefTopLevelSystemUnderTestPointsToComponentAssembly|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_ComponentRefTopLevelSystemUnderTestPointsToTestComponent_Invalid()
        {
            string outputDir = "TestBench_ComponentRefTopLevelSystemUnderTestPointsToTestComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@ComponentRefTopLevelSystemUnderTestPointsToTestComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_HasComponent_Invalid()
        {
            string outputDir = "TestBench_HasComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@HasComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_HasComponentRefTopLevelSystemUnderTestPointsToComponent_Invalid()
        {
            string outputDir = "TestBench_HasComponentRefTopLevelSystemUnderTestPointsToComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@HasComponentRefTopLevelSystemUnderTestPointsToComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_HasTestInjectionPoint_Invalid()
        {
            string outputDir = "TestBench_HasTestInjectionPoint_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@HasTestInjectionPoint|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_HasTestInjectionPointNullReference_Invalid()
        {
            string outputDir = "TestBench_HasTestInjectionPointNullReference_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@HasTestInjectionPointNullReference|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_MoreThanOneSystemUnderTest_Invalid()
        {
            string outputDir = "TestBench_MoreThanOneSystemUnderTest_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@MoreThanOneSystemUnderTest|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_NoSystemUnderTest_Invalid()
        {
            string outputDir = "TestBench_NoSystemUnderTest_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@NoSystemUnderTest|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestNullReference_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestNullReference_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestNullReference|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToComponent_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToComponentAssemblyRef_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToComponentAssemblyRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentAssemblyRef|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToComponentRef_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToComponentRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentRef|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToComponentRefPointsToTestComponent_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToComponentRefPointsToTestComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentRefPointsToTestComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToCompoundDesignContainerInstance_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToCompoundDesignContainerInstance_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToCompoundDesignContainerInstance|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToCompoundDesignContainerSubtype_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToCompoundDesignContainerSubtype_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToCompoundDesignContainerSubtype|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToNonRootDesignContainer_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToNonRootDesignContainer_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToNonRootDesignContainer|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBench_SystemUnderTestPointsToTestComponent_Invalid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToTestComponent_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Invalid|kind=Testing|relpos=0/@SystemUnderTestPointsToTestComponent|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBench_NoTestInjectionPoints_Valid()
        {
            string outputDir = "TestBench_NoTestInjectionPoints_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@NoTestInjectionPoints|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBench_SystemUnderTestPointsToAlternativeDesignContainer_Valid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToAlternativeDesignContainer_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@SystemUnderTestPointsToAlternativeDesignContainer|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBench_SystemUnderTestPointsToComponentAssembly_Valid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToComponentAssembly_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentAssembly|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBench_SystemUnderTestPointsToComponentAssemblyForSot_Valid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToComponentAssemblyForSot_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentAssemblyForSot|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBench_SystemUnderTestPointsToCompoundDesignContainer_Valid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToCompoundDesignContainer_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@SystemUnderTestPointsToCompoundDesignContainer|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBench_SystemUnderTestPointsToOptionalDesignContainer_Valid()
        {
            string outputDir = "TestBench_SystemUnderTestPointsToOptionalDesignContainer_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@SystemUnderTestPointsToOptionalDesignContainer|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBenchSuite_NonUniqueTestBenchReferences_Invalid()
        {
            string outputDir = "TestBenchSuite_NonUniqueTestBenchReferences_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Invalid|kind=TestBenchSuiteFolder|relpos=0/@NonUniqueTestBenchReferences|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBenchSuite_NoTestBenchRef_Invalid()
        {
            string outputDir = "TestBenchSuite_NoTestBenchRef_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Invalid|kind=TestBenchSuiteFolder|relpos=0/@NoTestBenchRef|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBenchSuite_OneTestBenchRefInstance_Invalid()
        {
            string outputDir = "TestBenchSuite_OneTestBenchRefInstance_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Invalid|kind=TestBenchSuiteFolder|relpos=0/@OneTestBenchRefInstance|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBenchSuite_OneTestBenchRefSubtype_Invalid()
        {
            string outputDir = "TestBenchSuite_OneTestBenchRefSubtype_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Invalid|kind=TestBenchSuiteFolder|relpos=0/@OneTestBenchRefSubtype|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBenchSuite_TestBenchRefNull_Invalid()
        {
            string outputDir = "TestBenchSuite_TestBenchRefNull_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Invalid|kind=TestBenchSuiteFolder|relpos=0/@TestBenchRefNull|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerFail")]
        public void TestBenchSuite_TestBenchRefPointsToDifferentDesigns_Invalid()
        {
            string outputDir = "TestBenchSuite_TestBenchRefPointsToDifferentDesigns_Invalid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Invalid|kind=TestBenchSuiteFolder|relpos=0/@TestBenchRefPointsToDifferentDesigns|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter context checker should have failed, but did not.");
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBenchSuite_MoreThanOneTestBenchReferencesPointToSameDesign_Valid()
        {
            string outputDir = "TestBenchSuite_MoreThanOneTestBenchReferencesPointToSameDesign_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Valid|kind=TestBenchSuiteFolder|relpos=0/@MoreThanOneTestBenchReferencesPointToSameDesign|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "ContextCheckerSuccess")]
        public void TestBenchSuite_OneTestBenchRef_Valid()
        {
            string outputDir = "TestBenchSuite_OneTestBenchRef_Valid";
            string objectAbsPath = "/@010_TestingContextChecker|kind=Testing|relpos=0/@TestBenchSuite|kind=TestBenchSuiteFolder|relpos=0/@Valid|kind=TestBenchSuiteFolder|relpos=0/@OneTestBenchRef|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            // check context
            var success = CyPhyMasterInterpreterRunner.RunContextCheck(mgaFile, objectAbsPath);

            Assert.True(success, "CyPhyMasterInterpreter context checker should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_ParametricExploration_PETAlternativeDesignContainer_ParametricExploration()
        {
            string outputDir = "GetConfigurations_ParametricExploration_PETAlternativeDesignContainer_ParametricExploration";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@PETAlternativeDesignContainer|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_ParametricExploration_PETComponentAssembly_ParametricExploration()
        {
            string outputDir = "GetConfigurations_ParametricExploration_PETComponentAssembly_ParametricExploration";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@PETComponentAssembly|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_ParametricExploration_PETCompoundDesignContainer_ParametricExploration()
        {
            string outputDir = "GetConfigurations_ParametricExploration_PETCompoundDesignContainer_ParametricExploration";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@ParametricExploration|kind=ParametricExplorationFolder|relpos=0/@PETCompoundDesignContainer|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasAlternative_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasAlternative_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasAlternative|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasComponentAssemblyReference_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasComponentAssemblyReference_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasComponentAssemblyReference|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasComponentNullReference_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasComponentNullReference_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasComponentNullReference|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasComponentReference_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasComponentReference_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasComponentReference|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasCompound_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasCompound_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasCompound|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasOptionalCAExporterGeneratesNullComponentAssemblyReferences_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasOptionalCAExporterGeneratesNullComponentAssemblyReferences_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasOptionalCAExporterGeneratesNullComponentAssemblyReferences|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_HasTestComponentReference_ContainmentVariations()
        {
            string outputDir = "GetConfigurations_TestBench_HasTestComponentReference_ContainmentVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ContainmentVariations|kind=Testing|relpos=0/@HasTestComponentReference|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_GeneratedAndNotExportedConfigurations_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_GeneratedAndNotExportedConfigurations_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@GeneratedAndNotExportedConfigurations|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_GeneratedMultipleExportedConfigurations_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_GeneratedMultipleExportedConfigurations_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@GeneratedMultipleExportedConfigurations|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_MassSpringDamperCustomConfigurationNames_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_MassSpringDamperCustomConfigurationNames_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@MassSpringDamperCustomConfigurationNames|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_MultipleExportedAndGeneratedConfigurations_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_MultipleExportedAndGeneratedConfigurations_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@MultipleExportedAndGeneratedConfigurations|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_MultipleExportedAndMultipleGeneratedConfigurations_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_MultipleExportedAndMultipleGeneratedConfigurations_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@MultipleExportedAndMultipleGeneratedConfigurations|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_MultipleExportedConfigurations_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_MultipleExportedConfigurations_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@MultipleExportedConfigurations|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_NoGeneratedConfigurations_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_NoGeneratedConfigurations_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@NoGeneratedConfigurations|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_NullComponentAssemblyRefFromExportedConfiguration_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_NullComponentAssemblyRefFromExportedConfiguration_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@NullComponentAssemblyRefFromExportedConfiguration|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_NullComponentRefsInsideExportedConfiguration_ExportedConfigurationVariations()
        {
            string outputDir = "GetConfigurations_TestBench_NullComponentRefsInsideExportedConfiguration_ExportedConfigurationVariations";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@ExportedConfigurationVariations|kind=Testing|relpos=0/@NullComponentRefsInsideExportedConfiguration|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_MSDAlternativeRoot_MSDVariants()
        {
            string outputDir = "GetConfigurations_TestBench_MSDAlternativeRoot_MSDVariants";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@MSDVariants|kind=Testing|relpos=0/@MSDAlternativeRoot|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_MSDCompoundRoot_MSDVariants()
        {
            string outputDir = "GetConfigurations_TestBench_MSDCompoundRoot_MSDVariants";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@MSDVariants|kind=Testing|relpos=0/@MSDCompoundRoot|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_SystemUnderTestPointsToAlternativeDesignContainer_TestBench()
        {
            string outputDir = "GetConfigurations_TestBench_SystemUnderTestPointsToAlternativeDesignContainer_TestBench";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@SystemUnderTestPointsToAlternativeDesignContainer|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_SystemUnderTestPointsToAlternativeDesignContainerForSot_TestBench()
        {
            string outputDir = "GetConfigurations_TestBench_SystemUnderTestPointsToAlternativeDesignContainerForSot_TestBench";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@SystemUnderTestPointsToAlternativeDesignContainerForSot|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_SystemUnderTestPointsToComponentAssembly_TestBench()
        {
            string outputDir = "GetConfigurations_TestBench_SystemUnderTestPointsToComponentAssembly_TestBench";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentAssembly|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_SystemUnderTestPointsToComponentAssemblyForSot_TestBench()
        {
            string outputDir = "GetConfigurations_TestBench_SystemUnderTestPointsToComponentAssemblyForSot_TestBench";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@SystemUnderTestPointsToComponentAssemblyForSot|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_SystemUnderTestPointsToCompoundDesignContainer_TestBench()
        {
            string outputDir = "GetConfigurations_TestBench_SystemUnderTestPointsToCompoundDesignContainer_TestBench";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@SystemUnderTestPointsToCompoundDesignContainer|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBench_SystemUnderTestPointsToCompoundDesignContainerForSot_TestBench()
        {
            string outputDir = "GetConfigurations_TestBench_SystemUnderTestPointsToCompoundDesignContainerForSot_TestBench";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBench|kind=Testing|relpos=0/@SystemUnderTestPointsToCompoundDesignContainerForSot|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBenchSuite_SoTAlternativeDesignContainer_TestBenchSuites()
        {
            string outputDir = "GetConfigurations_TestBenchSuite_SoTAlternativeDesignContainer_TestBenchSuites";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBenchSuites|kind=TestBenchSuiteFolder|relpos=0/@SoTAlternativeDesignContainer|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBenchSuite_SoTComponentAssembly_TestBenchSuites()
        {
            string outputDir = "GetConfigurations_TestBenchSuite_SoTComponentAssembly_TestBenchSuites";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBenchSuites|kind=TestBenchSuiteFolder|relpos=0/@SoTComponentAssembly|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "GetConfiguration")]
        public void GetConfigurations_TestBenchSuite_SoTCompoundDesignContainer_TestBenchSuites()
        {
            string outputDir = "GetConfigurations_TestBenchSuite_SoTCompoundDesignContainer_TestBenchSuites";
            string objectAbsPath = "/@02_TestingGetConfigurations|kind=Testing|relpos=0/@TestBenchSuites|kind=TestBenchSuiteFolder|relpos=0/@SoTCompoundDesignContainer|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.GetConfigurations(mgaFile, objectAbsPath);

            Assert.False(success, "CyPhyMasterInterpreter getting configurations for context failed.");
        }


        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BallisticTestBench_BallisticTestBenchTier1TestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BallisticTestBench_BallisticTestBenchTier1TestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BallisticTestBenchTier1TestBenchTypeProcessor|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BallisticTestBench_BallisticTestBenchTier2TestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BallisticTestBench_BallisticTestBenchTier2TestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BallisticTestBenchTier2TestBenchTypeProcessor|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BallisticTestBench_BallisticTestBenchTier3TestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BallisticTestBench_BallisticTestBenchTier3TestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BallisticTestBenchTier3TestBenchTypeProcessor|kind=BallisticTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier1TestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier1TestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BlastTestBenchTier1TestBenchTypeProcessor|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier2TestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier2TestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BlastTestBenchTier2TestBenchTypeProcessor|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier3MultiJobRunProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier3MultiJobRunProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BlastTestBenchTier3MultiJobRunProcessor|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier4MultiJobRunProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier4MultiJobRunProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BlastTestBenchTier4MultiJobRunProcessor|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier5MultiJobRunProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_BlastTestBench_BlastTestBenchTier5MultiJobRunProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@BlastTestBenchTier5MultiJobRunProcessor|kind=BlastTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_CADTestBench_StructuralFEATestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_CADTestBench_StructuralFEATestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@StructuralFEATestBenchTypeProcessor|kind=CADTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_CFDTestBench_CFDTestBenchMultiJobRunProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_CFDTestBench_CFDTestBenchMultiJobRunProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@CFDTestBenchMultiJobRunProcessor|kind=CFDTestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_ParametricExploration_ParametricExplorationProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_ParametricExploration_ParametricExplorationProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=ParametricExplorationFolder|relpos=0/@ParametricExplorationProcessor|kind=ParametricExploration|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_TestBench_TestBenchTypeProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_TestBench_TestBenchTypeProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=Testing|relpos=0/@TestBenchTypeProcessor|kind=TestBench|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterSupported")]
        public void AnalysisModelProcessor_TestBenchSuite_TestBenchSuiteProcessor_Valid()
        {
            string outputDir = "AnalysisModelProcessor_TestBenchSuite_TestBenchSuiteProcessor_Valid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Valid|kind=TestBenchSuiteFolder|relpos=0/@TestBenchSuiteProcessor|kind=TestBenchSuite|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelSupported(mgaFile, objectAbsPath);
        }
        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterNotSupported")]
        public void AnalysisModelProcessor_TestComponent_TestComponent_Invalid()
        {
            string outputDir = "AnalysisModelProcessor_TestComponent_TestComponent_Invalid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Invalid|kind=TestComponents|relpos=0/@TestComponent|kind=TestComponent|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelNotSupported(mgaFile, objectAbsPath);
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "AnalysisInterpreterNotSupported")]
        public void AnalysisModelProcessor_Workflow_Workflow_Invalid()
        {
            string outputDir = "AnalysisModelProcessor_Workflow_Workflow_Invalid";
            string objectAbsPath = "/@03_TestingAnalysisModelProcessors|kind=Testing|relpos=0/@01_ProcessorTypesForContexts|kind=Testing|relpos=0/@Invalid|kind=WorkflowDefinitionFolder|relpos=0/@Workflow|kind=Workflow|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            CyPhyMasterInterpreterRunner.AnalysisModelNotSupported(mgaFile, objectAbsPath);
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_CAD_Valid_MSD_ComponentAssembly_Island()
        {
            string outputDir = "MI_10_TestBench_MSD_CAD_Valid_MSD_ComponentAssembly_Island";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@CAD|kind=Testing|relpos=0/@MSD_CAD_Valid|kind=TestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly_Island|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        //[Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_om_CA2_MSD_ComponentAssembly_Island()
        {
            string outputDir = "MI_10_TestBench_MSD_om_CA2_MSD_ComponentAssembly_Island";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MSD_om_CA2|kind=TestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly_Island|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        //[Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_om_CA_MSD_ComponentAssembly_Island()
        {
            string outputDir = "MI_10_TestBench_MSD_om_CA_MSD_ComponentAssembly_Island";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MSD_om_CA|kind=TestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly_Island|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_ComponentRef_Config_3Pt_Polygon_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_ComponentRef_Config_3Pt_Polygon_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@FEA_ComponentRef_Config_3Pt_Polygon|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_AlternativeTIP_Config_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_AlternativeTIP_Config_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@FEA_AlternativeTIP_Config|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_PressureLoad_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_PressureLoad_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_PressureLoad|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_StructuralFEATestBench_Valid_MSD_ComponentAssembly_Island()
        {
            string outputDir = "MI_10_CADTestBench_StructuralFEATestBench_Valid_MSD_ComponentAssembly_Island";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@StructuralFEATestBench_Valid|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly_Island|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_ComponentRef_Config_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_ComponentRef_Config_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_ComponentRef_Config|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_BALL_2_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_BALL_2_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_BALL_2|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_ComponentRef_Config_2_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_ComponentRef_Config_2_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_ComponentRef_Config_2|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_PRESS_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_PRESS_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_PRESS|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_FORCE_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_FORCE_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_FORCE|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_BALL_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_BALL_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_BALL|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_PIN_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_PIN_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_PIN|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_CompoundTIP_Config_DISPLACEMENT_DesignContainer_cfg1_Valid()
        {
            string outputDir = "MI_10_CADTestBench_FEA_CompoundTIP_Config_DISPLACEMENT_DesignContainer_cfg1_Valid";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@MoreTesting|kind=Testing|relpos=0/@FEA_CompoundTIP_Config_DISPLACEMENT|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Valid|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CFDTestBench_CFDTestBench_Valid_MSD_ComponentAssembly_Island()
        {
            string outputDir = "MI_10_CFDTestBench_CFDTestBench_Valid_MSD_ComponentAssembly_Island";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@CFD|kind=Testing|relpos=0/@CFDTestBench_Valid|kind=CFDTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly_Island|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BlastTestBench_BlastTestBench_Custom_Valid_MSD_ComponentAssembly()
        {
            string outputDir = "MI_10_BlastTestBench_BlastTestBench_Custom_Valid_MSD_ComponentAssembly";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Blast|kind=Testing|relpos=0/@BlastTestBench_Custom_Valid|kind=BlastTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BlastTestBench_BlastTestBench_Predefined_Valid_MSD_ComponentAssembly()
        {
            string outputDir = "MI_10_BlastTestBench_BlastTestBench_Predefined_Valid_MSD_ComponentAssembly";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Blast|kind=Testing|relpos=0/@BlastTestBench_Predefined_Valid|kind=BlastTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterFail")]
        public void MI_10_BallisticTestBench_BallisticTestBench_BallisticTarget_Invalid_MSD_ComponentAssembly()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_BallisticTarget_Invalid_MSD_ComponentAssembly";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_BallisticTarget_Invalid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.False(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterFail")]
        public void MI_10_BallisticTestBench_BallisticTestBench_CriticalComponent_Invalid_MSD_ComponentAssembly()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_CriticalComponent_Invalid_MSD_ComponentAssembly";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_CriticalComponent_Invalid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.False(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BallisticTestBench_BallisticTestBench_Custom_Valid_MSD_ComponentAssembly()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_Custom_Valid_MSD_ComponentAssembly";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_Custom_Valid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BallisticTestBench_BallisticTestBench_Predef_Valid_MSD_ComponentAssembly()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_Predef_Valid_MSD_ComponentAssembly";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@ComponentAssembly|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_Predef_Valid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@31_MSDAssembly|kind=ComponentAssemblies|relpos=0/@MSD_ComponentAssembly|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BallisticTestBench_BallisticTestBench_Predef_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_Predef_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_Predef_DS_Valid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BallisticTestBench_BallisticTestBench_Predef_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_Predef_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_Predef_DS_Valid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BallisticTestBench_BallisticTestBench_Custom_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_Custom_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_Custom_DS_Valid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BallisticTestBench_BallisticTestBench_Custom_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_BallisticTestBench_BallisticTestBench_Custom_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Ballistics|kind=Testing|relpos=0/@BallisticTestBench_Custom_DS_Valid|kind=BallisticTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_CAD_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_TestBench_MSD_CAD_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@CAD|kind=Testing|relpos=0/@MSD_CAD_DS_Valid|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_CAD_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_TestBench_MSD_CAD_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@CAD|kind=Testing|relpos=0/@MSD_CAD_DS_Valid|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BlastTestBench_BlastTestBench_Custom_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_BlastTestBench_BlastTestBench_Custom_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Blast|kind=Testing|relpos=0/@BlastTestBench_Custom_DS_Valid|kind=BlastTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BlastTestBench_BlastTestBench_Custom_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_BlastTestBench_BlastTestBench_Custom_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Blast|kind=Testing|relpos=0/@BlastTestBench_Custom_DS_Valid|kind=BlastTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BlastTestBench_BlastTestBench_Predef_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_BlastTestBench_BlastTestBench_Predef_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Blast|kind=Testing|relpos=0/@BlastTestBench_Predef_DS_Valid|kind=BlastTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_BlastTestBench_BlastTestBench_Predef_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_BlastTestBench_BlastTestBench_Predef_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Blast|kind=Testing|relpos=0/@BlastTestBench_Predef_DS_Valid|kind=BlastTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_ComponentRef_DS_cfg2()
        {
            string outputDir = "MI_10_CADTestBench_FEA_ComponentRef_DS_cfg2";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@FEA_ComponentRef_DS|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@DesignSpaceSrtucturalFEA|kind=DesignSpace|relpos=0/@DesignContainerStructuralFEA|kind=DesignContainer|relpos=0/@Exported-Configurations-at--11-15--11-59-11|kind=Configurations|relpos=0/@cfg2|kind=CWC|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_FEA_ComponentRef_DS_DesignContainer_cfg1_Invalid_Generated()
        {
            string outputDir = "MI_10_CADTestBench_FEA_ComponentRef_DS_DesignContainer_cfg1_Invalid_Generated";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@FEA_ComponentRef_DS|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations Structural FEA|kind=ComponentAssemblies|relpos=0/@DesignContainer|kind=ComponentAssemblies|relpos=0/@Configurations (11-15-2013  11:59:32)|kind=ComponentAssemblies|relpos=0/@DesignContainer_cfg1_Invalid_Generated|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_StructuralFEATestBench_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_CADTestBench_StructuralFEATestBench_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@StructuralFEATestBench_DS_Valid|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CADTestBench_StructuralFEATestBench_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_CADTestBench_StructuralFEATestBench_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@StructuralFEA|kind=Testing|relpos=0/@StructuralFEATestBench_DS_Valid|kind=CADTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CFDTestBench_CFDTestBench_DS_Valid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_CFDTestBench_CFDTestBench_DS_Valid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@CFD|kind=Testing|relpos=0/@CFDTestBench_DS_Valid|kind=CFDTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        //[Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_CFDTestBench_CFDTestBench_DS_Valid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_CFDTestBench_CFDTestBench_DS_Valid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@CFD|kind=Testing|relpos=0/@CFDTestBench_DS_Valid|kind=CFDTestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        //[Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_om_DS2_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_TestBench_MSD_om_DS2_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MSD_om_DS2|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        //[Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_om_DS2_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_TestBench_MSD_om_DS2_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MSD_om_DS2|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        //[Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_om_DS_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_TestBench_MSD_om_DS_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MSD_om_DS|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterSuccess")]
        public void MI_10_TestBench_MSD_om_DS_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_TestBench_MSD_om_DS_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MSD_om_DS|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.True(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterFail")]
        public void MI_10_TestBench_MSD_om_DS_postprocessing_Invalid_MassSpringDamper_cfg1()
        {
            string outputDir = "MI_10_TestBench_MSD_om_DS_postprocessing_Invalid_MassSpringDamper_cfg1";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MustFail|kind=Testing|relpos=0/@MSD_om_DS_postprocessing_Invalid|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg1|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.False(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }

        [Fact]
        [Trait("Model", "MasterInterpreter")]
        [Trait("MasterInterpreter", "RunInterpreterFail")]
        public void MI_10_TestBench_MSD_om_DS_postprocessing_Invalid_MassSpringDamper_cfg4()
        {
            string outputDir = "MI_10_TestBench_MSD_om_DS_postprocessing_Invalid_MassSpringDamper_cfg4";
            string objectAbsPath = "/@TestBenches|kind=Testing|relpos=0/@DesignSpace|kind=Testing|relpos=0/@Dynamics|kind=Testing|relpos=0/@MustFail|kind=Testing|relpos=0/@MSD_om_DS_postprocessing_Invalid|kind=TestBench|relpos=0";
            string configAbsPath = "/@Generated configurations|kind=ComponentAssemblies|relpos=0/@MassSpringDamper|kind=ComponentAssemblies|relpos=0/@Configurations (11-16-2013  14:05:10)|kind=ComponentAssemblies|relpos=0/@MassSpringDamper_cfg4|kind=ComponentAssembly|relpos=0";

            Assert.True(File.Exists(mgaFile), "Failed to generate the mga.");

            var success = CyPhyMasterInterpreterRunner.RunMasterInterpreter(
                projectPath: mgaFile,
                absPath: objectAbsPath,
                configPath: configAbsPath,
                postToJobManager: false,
                keepTempModels: false);

            Assert.False(success, "CyPhyMasterInterpreter run should have succeeded, but did not.");
        }



    }

}
