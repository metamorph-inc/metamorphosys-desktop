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
using GME.MGA;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using META;

namespace CyPhyMasterInterpreter
{
    public class TestBenchSuiteProcessor : AnalysisModelProcessor
    {
        public CyPhy.TestBenchSuite testBenchSuite { get; set; }
        public CyPhy.TestBenchSuite expandedTestBenchSuite { get; set; }

        public TestBenchSuiteProcessor(CyPhy.TestBenchSuite testBenchSuite)
        {
            this.testBenchSuite = testBenchSuite;

            this.OriginalSystemUnderTest = testBenchSuite
                .Children
                .TestBenchRefCollection
                .FirstOrDefault()
                .Referred
                .TestBenchType
                .Children
                .TopLevelSystemUnderTestCollection
                .FirstOrDefault();
        }

        public override MgaModel GetInvokedObject()
        {
            return this.testBenchSuite.Impl as MgaModel;
        }

        public override MgaModel GetExpandedObject()
        {
            return this.expandedTestBenchSuite.Impl as MgaModel;
        }

        public override void Expand(CyPhy.ComponentAssembly configuration)
        {
            this.Configuration = configuration;

            if (this.OriginalSystemUnderTest.Referred.DesignEntity.ID == configuration.ID)
            {
                this.expandedTestBenchSuite = this.testBenchSuite;
            }
            else
            {
                // create temp folder for test bench suite
                CyPhy.TestBenchSuiteFolder testing = CyPhyClasses.TestBenchSuiteFolder.Cast(this.testBenchSuite.ParentContainer.Impl);

                var tempFolderName = AnalysisModelProcessor.GetTemporaryFolderName(this.testBenchSuite.Impl);

                CyPhy.TestBenchSuiteFolder tempFolder = testing.Children.TestBenchSuiteFolderCollection.FirstOrDefault(x => x.Name == tempFolderName);
                if (tempFolder == null)
                {
                    tempFolder = CyPhyClasses.TestBenchSuiteFolder.Create(testing);
                    tempFolder.Name = tempFolderName;

                    this.AddToTraceabilityAndTemporary(tempFolder.Impl, testing.Impl, recursive: false);
                }

                // copy test bench suite
                var tempCopy = (tempFolder.Impl as MgaFolder).CopyFCODisp(this.testBenchSuite.Impl as MgaFCO);
                // fix name
                tempCopy.Name = AnalysisModelProcessor.GetTemporaryObjectName(this.testBenchSuite.Impl, configuration.Impl);

                this.AddToTraceabilityAndTemporary(tempCopy, this.testBenchSuite.Impl);

                // set expanded property to the expanded element
                this.expandedTestBenchSuite = CyPhyClasses.TestBenchSuite.Cast(tempCopy);
            }

            // expand all test benches
            foreach (var testBenchRef in this.expandedTestBenchSuite.Children.TestBenchRefCollection)
            {
                var testBenchTypeExpander = new TestBenchTypeProcessor(testBenchRef.Referred.TestBenchType);
                testBenchTypeExpander.Expand(configuration);

                // switch references
                var switcher = new ReferenceSwitcher.ReferenceSwitcherInterpreter();
                // TODO: handle failures for this
                switcher.SwitchReference(testBenchTypeExpander.expandedTestBenchType.Impl as MgaFCO, testBenchRef.Impl as IMgaReference);

                this.InnerExpanders.Push(testBenchTypeExpander);
            }
        }

        public override Queue<ComComponent> GetWorkflow()
        {
            Queue<ComComponent> workflow = new Queue<ComComponent>();
            this.ExecuteInTransaction(() =>
            {
                workflow.Enqueue(new ComComponent("MGA.Interpreter.CyPhySoT", true));
            });
            return workflow;
        }

        public override bool PostToJobManager(JobManagerDispatch manager = null)
        {
            if (this.Interpreters == null)
            {
                throw new InvalidOperationException("Call RunInterpreters method first.");
            }

            if (manager == null)
            {
                manager = new JobManagerDispatch();
            }

            bool success = true;

            foreach (var interpreter in this.Interpreters)
            {
                // TODO: what if some of them failed ???
                // TODO: can be more than one ???
                if (interpreter.result.Success)
                {
                    string workingDirectory = interpreter.MainParameters.OutputDirectory;

                    success = success && manager.EnqueueSoT(workingDirectory);
                }
            }

            // TODO: should this be inside the Dispatch and triggred by a timer ???
            manager.AddSoTs();


            if (this.Interpreters.Count == 0)
            {
                success = false;
            }

            return success;
        }

        public override bool SaveTestBenchManifest(AVM.DDP.MetaAvmProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            // TODO: implement this method!

            // FIXME: return fake true for now.
            return true;
        }

        public override bool SaveTestBench(AVM.DDP.MetaAvmProject project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            this.ThrowIfNotExpanded();

            this.EnsureOutputDirectory();

            var success = true;

            // NOTE: saving the original design space test benches
            foreach (var testbenchRef in this.testBenchSuite.Children.TestBenchRefCollection)
            {
                var thisSuccess = project.SaveTestBench(testbenchRef.Referred.TestBenchType);
                success = success && thisSuccess;
            }

            return success;
        }

        public override bool UpdateTestBenchManifestExecutionSteps(AVM.DDP.MetaTBManifest manifest)
        {
            // FIXME: return fake true for now.
            return true;
        }
    }

}
