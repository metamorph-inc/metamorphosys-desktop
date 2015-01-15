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

using DesignConsistencyChecker.DesignRule.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GME.MGA;
using CyPhyCOMInterfaces;

namespace DesignConsistencyChecker.DesignRule.TestBench
{
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ModelicaTestBench : RuleBase
    {

        //[CheckerRule("Modelica TB", Description = "Description is not mandatory.")]
        //[Tags("Modelica","Modelica 2")]
        //[ValidContext("AggregatePort")]
        public static IEnumerable<RuleFeedbackBase> CheckRule1(MgaFCO context)
        {
            return new List<RuleFeedbackBase>
                      {
                          new GenericRuleFeedback{FeedbackType = FeedbackTypes.Error, Message = "Error1"},
                          new GenericRuleFeedback{FeedbackType = FeedbackTypes.Error, Message = "Error2"},
                          new GenericRuleFeedback{FeedbackType = FeedbackTypes.Error, Message = "Error3"},
                      };
        }

        //[CheckerRule("Modelica TB 2", Description = "Description!")]
        //[Tags("Modelica", "Modelica 2")]
        //[ValidContext("Component")]
        public static IEnumerable<RuleFeedbackBase> CheckRule2(MgaFCO context)
        {
            return new List<RuleFeedbackBase>
                      {
                          new GenericRuleFeedback{FeedbackType = FeedbackTypes.Warning, Message = "Warning1"},
                      };
        }


        [CheckerRule("TLSUT", Description = "Top level system under test rule.")]
        [Tags("Modelica", "CAD", "FormulaEvaluator", "CyPython")]
        [ValidContext("TestBench")]
        public static IEnumerable<RuleFeedbackBase> CheckTLSUT(MgaFCO context)
        {
            var result = new List<RuleFeedbackBase>();

            var tsults = context.ChildObjects.OfType<MgaReference>().Where(x => x.Meta.Name == "TopLevelSystemUnderTest");

            var count = tsults.Count();

            if (count == 0)
            {
                var feedback = new GenericRuleFeedback()
                {
                    FeedbackType = FeedbackTypes.Error,
                    Message = "There is no top level system under test object."
                };

                result.Add(feedback);
            }
            else if (count == 1)
            {
                var referred = tsults.FirstOrDefault().Referred;
                if (referred == null)
                {
                    var feedback = new GenericRuleFeedback()
                    {
                        FeedbackType = FeedbackTypes.Error,
                        Message = "Top level system under test reference cannot be null."
                    };

                    result.Add(feedback);
                }
                else if (referred.Meta.Name != "DesignContainer")
                {
                    var feedback = new GenericRuleFeedback
                                       {
                        FeedbackType = FeedbackTypes.Warning,
                        Message = "Top level system under test reference MUST point to a Design Container."
                    };

                    result.Add(feedback);
                }
            }
            else
            {
                var feedback = new GenericRuleFeedback
                                   {
                    FeedbackType = FeedbackTypes.Error,
                    Message = "There is more than one top level system under test object."
                };

                result.Add(feedback);
            }

            return result;
        }



        // HyperLink
        // sb.AppendFormat("<a href=\"mga:{0}\">{1} (not mapped)</a>", subject.ID, subject.Name);
       
    }
}
