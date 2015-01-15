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


    public class BallisticTestBenchChecker : TestBenchTypeChecker<CyPhy.BallisticTestBench>
    {
        // Ballistic
        public BallisticTestBenchChecker(CyPhy.BallisticTestBench testBench)
            : base(testBench)
        {
        }

        public override void CheckNoThrow()
        {
            base.CheckNoThrow();

            this.m_details.AddRange(this.NoTestInjectionPoints());
            this.m_details.AddRange(this.ValidateBallisticTargets());
            this.m_details.AddRange(this.ValidateCriticalComponents());
            this.m_details.AddRange(this.ExactlyOneReferencePlane());

            this.m_details.AddRange(this.AtLeastOneShotlineModelOrAPredefined());

            // case 1
            this.m_details.AddRange(this.HasShotLinesAndAtLeastOneBallisticTarget());
            
            // case 2
            this.m_details.AddRange(this.HasAPredefinedAndNoBallisticTarget());
        }

        private IEnumerable<ContextCheckerResult> AtLeastOneShotlineModelOrAPredefined()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();
            
            var hasShotline = this.testBench.Children.ShotlineModelCollection.Any();
            var hasPredefined = this.testBench.Children.PredefinedBallisticSuiteCollection.Any();

            if (hasShotline || hasPredefined)
            {
                if (hasShotline && hasPredefined)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = this.testBench.Impl,
                        Message = "Test bench cannot have both ShotlineModel or PredefinedBallisticSuite."
                    };

                    results.Add(feedback);
                }
                else
                {
                    if (hasShotline)
                    {
                        var feedback = new ContextCheckerResult()
                        {
                            Success = true,
                            Subject = this.testBench.Impl,
                            Message = "Test bench has at least one ShotlineModel."
                        };

                        results.Add(feedback);
                    }

                    if (hasPredefined)
                    {
                        var feedback = new ContextCheckerResult()
                        {
                            Success = true,
                            Subject = this.testBench.Impl,
                            Message = "Test bench has PredefinedBallisticSuite."
                        };

                        results.Add(feedback);
                    }
                }
                
            }
            else
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.testBench.Impl,
                    Message = "Test bench does not have any ShotlineModel or PredefinedBallisticSuite."
                };

                results.Add(feedback);
            }

            return results;
        }

        protected IEnumerable<ContextCheckerResult> ValidateBallisticTargets()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();
            foreach (var ballisticTarget in this.testBench.Children.BallisticTargetCollection)
            {
               results.AddRange(this.ValidateTestInjectionPoint(ballisticTarget.Impl as MgaReference));
            }
            return results;
        }

        protected IEnumerable<ContextCheckerResult> ValidateCriticalComponents()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();
            foreach (var criticalComponent in this.testBench.Children.CriticalComponentCollection)
            {
                results.AddRange(this.ValidateTestInjectionPoint(criticalComponent.Impl as MgaReference));
            }
            return results;
        }

        protected IEnumerable<ContextCheckerResult> HasShotLinesAndAtLeastOneBallisticTarget()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();

            if (this.testBench.Children.ShotlineModelCollection.Any())
            {
                if (this.testBench.Children.BallisticTargetCollection.Any())
                {
                    // ok
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = this.testBench.Impl,
                        Message = "Test bench has at least one BallisticTarget component and has at least one shotline models."
                    };

                    results.Add(feedback);
                }
                else
                {
                    // at least one ballistic target is required
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = this.testBench.Impl,
                        Message = "At least one BallisticTarget component is required, when test bench has shotline models."
                    };

                    results.Add(feedback);
                }
            }

            return results;
        }


        protected IEnumerable<ContextCheckerResult> HasAPredefinedAndNoBallisticTarget()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();
            var predefinedCount = this.testBench.Children.PredefinedBallisticSuiteCollection.Count();
            if (predefinedCount == 1)
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = true,
                    Subject = this.testBench.Children.PredefinedBallisticSuiteCollection.FirstOrDefault().Impl,
                    Message = "Test bench has exactly one PredefinedBallisticSuite."
                };

                results.Add(feedback);

                if (this.testBench.Children.BallisticTargetCollection.Any())
                {
                    feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = this.testBench.Impl,
                        Message = "If test bench has PredefinedBallisticSuite, it cannot have BallisticTarget."
                    };

                    results.Add(feedback);
                }
                else
                {
                    feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = this.testBench.Impl,
                        Message = "Test bench has PredefinedBallisticSuite and does not have BallisticTarget."
                    };

                    results.Add(feedback);
                }
            }
            else if (predefinedCount > 1)
            {
                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.testBench.Impl,
                    Message = "Test bench cannot have more than one PredefinedBallisticSuite."
                };

                results.Add(feedback);
            }

            return results;
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


    }
}
