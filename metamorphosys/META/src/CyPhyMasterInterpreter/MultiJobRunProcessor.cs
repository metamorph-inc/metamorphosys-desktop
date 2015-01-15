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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using META;

namespace CyPhyMasterInterpreter
{
    public class MultiJobRunProcessor : TestBenchTypeProcessor
    {
        public MultiJobRunProcessor(CyPhy.TestBenchType testBenchType)
            : base(testBenchType)
        {
        }

        Queue<ComComponent> _interpretersToConfigure;
        public override System.Collections.Generic.Queue<META.ComComponent> InterpretersToConfiguration
        {
            get
            {
                if (_interpretersToConfigure == null)
                {
                    _interpretersToConfigure = new Queue<ComComponent>();
                    foreach (var interpreter in base.GetWorkflow())
                    {
                        _interpretersToConfigure.Enqueue(interpreter);
                    }
                }
                return _interpretersToConfigure;
            }
        }

        public override Queue<META.ComComponent> GetWorkflow()
        {
            Queue<ComComponent> workflow = new Queue<ComComponent>();

            this.ExecuteInTransaction(() =>
            {
                workflow.Enqueue(new ComComponent("MGA.Interpreter.CyPhyMultiJobRun", true));
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
    }
}
