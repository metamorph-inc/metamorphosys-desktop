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


namespace CyPhyMasterInterpreter.Rules
{
    public class BlastTestBenchChecker : TestBenchTypeChecker<CyPhy.BlastTestBench>
    {
        // Blast
        public BlastTestBenchChecker(CyPhy.BlastTestBench testBench)
            : base(testBench)
        {
        }

        public override void CheckNoThrow()
        {
            base.CheckNoThrow();

            this.m_details.AddRange(this.NoTestInjectionPoints());
            this.m_details.AddRange(this.ExactlyOneReferencePlane());
            this.m_details.AddRange(this.ExactlyOnePredefinedBlastSuiteOrOneBlastModel());
        }

        protected IEnumerable<ContextCheckerResult> ExactlyOneReferencePlane()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();

            var referencePlaneCount = this.testBench.Children.ReferencePlaneCollection.Count();

            if (referencePlaneCount == 0)
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.testBench.Impl,
                    Message = "Test bench must have one reference plane."
                };

                results.Add(feedback);
            }
            else if (referencePlaneCount == 1)
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = true,
                    Subject = this.testBench.Children.ReferencePlaneCollection.FirstOrDefault().Impl,
                    Message = "One reference plane found."
                };

                results.Add(feedback);
            }
            else if (referencePlaneCount > 1)
            {
                foreach (var referencePlane in this.testBench.Children.ReferencePlaneCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = referencePlane.Impl,
                        Message = "Only one ReferencePlace object is allowed."
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

        protected IEnumerable<ContextCheckerResult> ExactlyOnePredefinedBlastSuiteOrOneBlastModel()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();

            var predefinedBlastSuiteCount = this.testBench.Children.PredefinedBlastSuiteCollection.Count();
            var blastModelCount = this.testBench.Children.BlastModelCollection.Count();


            if (predefinedBlastSuiteCount + blastModelCount == 0)
            {
                string errorMessage = "Test bench has no PredefinedBlastSuite OR BlastModel. Test bench must have either one PredefinedBlastSuite or one BlastModel.";

                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.testBench.Impl,
                    Message = errorMessage
                };

                results.Add(feedback);

                feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.testBench.Impl,
                    Message = errorMessage
                };

                results.Add(feedback);

            }
            else if (predefinedBlastSuiteCount + blastModelCount == 1)
            {
                foreach (var predefinedBlastSuite in this.testBench.Children.PredefinedBlastSuiteCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = predefinedBlastSuite.Impl,
                        Message = string.Format("Test bench has exectly one solver settings: {0}.", predefinedBlastSuite.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var blastModel in this.testBench.Children.BlastModelCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = blastModel.Impl,
                        Message = string.Format("Test bench has exectly one solver settings: {0}.", blastModel.Kind)
                    };

                    results.Add(feedback);
                }

            }
            else if (predefinedBlastSuiteCount + blastModelCount > 1)
            {
                string errorMessage = "Test bench has more than one PredefinedBlastSuite/BlastModel. Test bench must have either one PredefinedBlastSuite or one BlastModel.";

                foreach (var predefinedBlastSuite in this.testBench.Children.PredefinedBlastSuiteCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = predefinedBlastSuite.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }

                foreach (var blastModel in this.testBench.Children.BlastModelCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = blastModel.Impl,
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
