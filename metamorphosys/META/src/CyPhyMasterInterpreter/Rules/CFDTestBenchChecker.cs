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
    public class CFDTestBenchChecker : TestBenchTypeChecker<CyPhy.CFDTestBench>
    {
        // CFD
        public CFDTestBenchChecker(CyPhy.CFDTestBench testBench)
            : base(testBench)
        {
        }

        public override void CheckNoThrow()
        {
            base.CheckNoThrow();

            this.m_details.AddRange(this.NoTestInjectionPoints());
            this.m_details.AddRange(this.ExactlyOneSolverSettings());
        }

        protected IEnumerable<ContextCheckerResult> ExactlyOneSolverSettings()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();

            var calmWaterSolverCount = this.testBench.Children.CalmWaterSolverSettingsCollection.Count();
            var waveResistanceSolverCount = this.testBench.Children.WaveResistanceSolverSettingsCollection.Count();
            var correlationSettingsCount = this.testBench.Children.CorrelationSettingsCollection.Count();
            var hydrostaticsSolverCount = this.testBench.Children.HydrostaticsSolverSettingsCollection.Count();

            if (calmWaterSolverCount + waveResistanceSolverCount + correlationSettingsCount + hydrostaticsSolverCount == 0)
            {
                string errorMessage = "Test bench has no solver settings. It must have of one solver settings CalmWaterSolverSettings, " +
                    "WaveResistanceSolverSettings, CorrelationSettings or HydrostaticsSolverSettings. A HydrostaticsSolverSettings can " +
                    "further be combined with one of the other types.";

                var feedback = new ContextCheckerResult()
                {
                    Success = false,
                    Subject = this.testBench.Impl,
                    Message = errorMessage
                };

                results.Add(feedback);
            }
            else if (calmWaterSolverCount + waveResistanceSolverCount + correlationSettingsCount + hydrostaticsSolverCount == 1)
            {
                foreach (var calmWaterSolver in this.testBench.Children.CalmWaterSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = calmWaterSolver.Impl,
                        Message = string.Format("Test bench has exectly one solver settings: {0}.", calmWaterSolver.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var waveResistanceSolver in this.testBench.Children.WaveResistanceSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = waveResistanceSolver.Impl,
                        Message = string.Format("Test bench has exectly one solver settings: {0}.", waveResistanceSolver.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var correlationSetting in this.testBench.Children.CorrelationSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = correlationSetting.Impl,
                        Message = string.Format("Test bench has exectly one solver settings: {0}.", correlationSetting.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var hydrostaticsSolver in this.testBench.Children.HydrostaticsSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = hydrostaticsSolver.Impl,
                        Message = string.Format("Test bench has exectly one solver settings: {0}.", hydrostaticsSolver.Kind)
                    };

                    results.Add(feedback);
                }
            }
            else if (calmWaterSolverCount + waveResistanceSolverCount + correlationSettingsCount > 1)
            {
                string errorMessage = "Test bench can only have one solver setting of type CalmWaterSolverSettings, WaveResistanceSolverSettings and CorrelationSettings.";

                foreach (var calmWaterSolver in this.testBench.Children.CalmWaterSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = calmWaterSolver.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }

                foreach (var waveResistanceSolver in this.testBench.Children.WaveResistanceSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = waveResistanceSolver.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }

                foreach (var correlationSetting in this.testBench.Children.CorrelationSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = correlationSetting.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }
            }
            else if (hydrostaticsSolverCount > 1)
            {
                string errorMessage = "Test bench can only have one solver setting of type HydrostaticsSolverSettings.";

                foreach (var hydrostaticsSolver in this.testBench.Children.HydrostaticsSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = false,
                        Subject = hydrostaticsSolver.Impl,
                        Message = errorMessage
                    };

                    results.Add(feedback);
                }
            }
            else if ((calmWaterSolverCount + waveResistanceSolverCount + correlationSettingsCount) == 1 && 
                      hydrostaticsSolverCount == 1)
            {
                foreach (var hydrostaticsSolver in this.testBench.Children.HydrostaticsSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = hydrostaticsSolver.Impl,
                        Message = string.Format("Test bench has one {0} and other solver-settings.", hydrostaticsSolver.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var calmWaterSolver in this.testBench.Children.CalmWaterSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = calmWaterSolver.Impl,
                        Message = string.Format("Test bench has one HydrostaticsSolverSettings and a {0}.", calmWaterSolver.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var waveResistanceSolver in this.testBench.Children.WaveResistanceSolverSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = waveResistanceSolver.Impl,
                        Message = string.Format("Test bench has one HydrostaticsSolverSettings and a {0}.", waveResistanceSolver.Kind)
                    };

                    results.Add(feedback);
                }

                foreach (var correlationSetting in this.testBench.Children.CorrelationSettingsCollection)
                {
                    var feedback = new ContextCheckerResult()
                    {
                        Success = true,
                        Subject = correlationSetting.Impl,
                        Message = string.Format("Test bench has one HydrostaticsSolverSettings and a {0}.", correlationSetting.Kind)
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
