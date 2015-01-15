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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class StructuralFEATestBenchChecker : TestBenchTypeChecker<CyPhy.CADTestBench>
    {
        // CAD = Structural FEA
        public StructuralFEATestBenchChecker(CyPhy.CADTestBench testBench)
            : base(testBench)
        {
        }

        public override void CheckNoThrow()
        {
            base.CheckNoThrow();

            this.m_details.AddRange(this.AtLeastOneTestInjectionPoints());
            this.m_details.AddRange(this.FaceConstructMustBeAbaqusModelBased());
        }

        protected IEnumerable<ContextCheckerResult> FaceConstructMustBeAbaqusModelBased()
        {
            List<ContextCheckerResult> results = new List<ContextCheckerResult>();
            if (this.testBench.Children.FaceCollection.Any())
            {
                switch (this.testBench.Attributes.SolverType)
                {
                    case CyPhyClasses.CADTestBench.AttributesClass.SolverType_enum.ABAQUS_Model_Based:
                        foreach (var face in this.testBench.Children.FaceCollection)
                        {
                            var feedback = new ContextCheckerResult()
                            {
                                Success = true,
                                Subject = face.Impl,
                                Message = string.Format("Face construct allowed for solver type ABAQUS model based.")
                            };

                            results.Add(feedback);
                        }

                        break;
                    case CyPhyClasses.CADTestBench.AttributesClass.SolverType_enum.ABAQUS_Deck_Based:
                        foreach (var face in this.testBench.Children.FaceCollection)
                        {
                            var feedback = new ContextCheckerResult()
                            {
                                Success = false,
                                Subject = face.Impl,
                                Message = string.Format("Face construct not allowed for solver type ABAQUS deck based.")
                            };

                            results.Add(feedback);
                        }

                        break;
                    case CyPhyClasses.CADTestBench.AttributesClass.SolverType_enum.NASTRAN:
                        foreach (var face in this.testBench.Children.FaceCollection)
                        {
                            var feedback = new ContextCheckerResult()
                            {
                                Success = false,
                                Subject = face.Impl,
                                Message = string.Format("Face construct not allowed for solver type NASTRAN.")
                            };

                            results.Add(feedback);
                        }

                        break;
                }
            }

            return results;
        }
    }
}
