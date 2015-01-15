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

namespace CyPhyMasterInterpreter.Rules
{
    public class ParametricExplorationChecker : ContextChecker
    {
        private CyPhy.ParametricExploration parametricExploration { get; set; }

        public ParametricExplorationChecker(CyPhy.ParametricExploration parametricExploration)
        {
            this.parametricExploration = parametricExploration;
        }

        public override GME.MGA.IMgaModel GetContext()
        {
            return this.parametricExploration.Impl as GME.MGA.IMgaModel;
        }

        public override void CheckNoThrow()
        {
            base.CheckNoThrow();

            this.m_details.AddRange(this.TestBenchReferences());
            this.m_details.AddRange(this.ExactlyOneDriver());
        }

        protected IEnumerable<ContextCheckerResult> TestBenchReferences()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();

            var testBenchRefCount = this.parametricExploration.Children.TestBenchRefCollection.Count();

            if (testBenchRefCount == 0)
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.parametricExploration.Impl,
                    Message = "Parametric exploration no test bench reference. It must have exactly one test bench reference."
                };

                results.Add(feedback);
            }
            else if (testBenchRefCount == 1)
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = true,
                    Subject = this.parametricExploration.Children.TestBenchRefCollection.FirstOrDefault().Impl,
                    Message = "One test bench reference found."
                };

                results.Add(feedback);
            }
            else if (testBenchRefCount > 1)
            {
                foreach (var testBenchRef in this.parametricExploration.Children.TestBenchRefCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = testBenchRef.Impl,
                        Message = "Only one test bench reference is allowed."
                    };

                    results.Add(feedback);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            foreach (var testBenchRef in this.parametricExploration.Children.TestBenchRefCollection)
            {
                // check test benches
                if (testBenchRef.Referred.TestBenchType == null)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = testBenchRef.Impl,
                        Message = "Test bench reference cannot be null."
                    };

                    results.Add(feedback);

                    continue;
                }
                else
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = testBenchRef.Impl,
                        Message = "Test bench reference is not null."
                    };

                    results.Add(feedback);
                }

                var testBench = testBenchRef.Referred.TestBenchType;

                // testbench ref is NOT null at this point
                ContextChecker testBenchChecker = ContextChecker.GetContextChecker(testBench.Impl as GME.MGA.MgaModel);

                testBenchChecker.TryCheck();

                results.AddRange(testBenchChecker.Details);
            }

            return results;
        }

        protected IEnumerable<ContextCheckerResult> ExactlyOneDriver()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();

            var pccDriverCount = this.parametricExploration.Children.PCCDriverCollection.Count();
            var optimizerCount = this.parametricExploration.Children.OptimizerCollection.Count();
            var parameterStudyCount = this.parametricExploration.Children.ParameterStudyCollection.Count();


            if (pccDriverCount + optimizerCount + parameterStudyCount == 0)
            {
                string errorMessage = "Parametric Exploration model has no driver. It must have exactly one driver PCCDriver OR Optimizer OR ParameterStudy.";

                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.parametricExploration.Impl,
                    Message = errorMessage
                };

                results.Add(feedback);
            }
            else if (pccDriverCount + optimizerCount + parameterStudyCount == 1)
            {
                foreach (var pccDriver in this.parametricExploration.Children.PCCDriverCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = pccDriver.Impl,
                        Message = string.Format("Test bench has exectly driver: {0}.", pccDriver.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var optimizer in this.parametricExploration.Children.OptimizerCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = optimizer.Impl,
                        Message = string.Format("Test bench has exectly driver: {0}.", optimizer.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var parameterStudy in this.parametricExploration.Children.ParameterStudyCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = parameterStudy.Impl,
                        Message = string.Format("Test bench has exectly driver: {0}.", parameterStudy.Kind)
                    };

                    results.Add(feedback);
                }
            }
            else if (pccDriverCount + optimizerCount + parameterStudyCount > 1)
            {
                string errorMessage = "Parametric Exploration model must have exactly one driver PCCDriver OR Optimizer OR ParameterStudy.";

                foreach (var pccDriver in this.parametricExploration.Children.PCCDriverCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = pccDriver.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }

                foreach (var optimizer in this.parametricExploration.Children.OptimizerCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = optimizer.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }

                foreach (var parameterStudy in this.parametricExploration.Children.ParameterStudyCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = parameterStudy.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }
            }
            else
            {
                throw new NotImplementedException();
            }


            return results;
        }

    }
}
