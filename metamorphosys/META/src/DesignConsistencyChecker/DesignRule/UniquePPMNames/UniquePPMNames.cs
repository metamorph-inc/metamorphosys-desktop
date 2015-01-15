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

using System.ComponentModel;
using GME.MGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignConsistencyChecker.DesignRule.Attributes;

using CyPhyML = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyMLClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using CyPhyCOMInterfaces;

namespace DesignConsistencyChecker.DesignRule.UniquePPMNames
{
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UniquePPMNames : RuleBase
    {
        public UniquePPMNames()
            : base()
        {
            // TODO: initialize valid context
        }

        [CheckerRule("UniquePPMNames")]
        [Tags("Modelica")]
        [ValidContext("Component")]
        public static IEnumerable<RuleFeedbackBase> CheckRule(MgaFCO context)
        {
            var result = new List<RuleFeedbackBase>();

            CyPhyML.Component cyPhyMLComponent = ISIS.GME.Common.Utils.CreateObject<CyPhyMLClasses.Component>(context as MgaObject);

            Dictionary<string, List<CyPhyML.HasDescriptionAndGUID>> namePPMListMap = new Dictionary<string, List<CyPhyML.HasDescriptionAndGUID> >();

            List<object> objectList = cyPhyMLComponent.AllChildren.ToList<object>();
            foreach( CyPhyML.HasDescriptionAndGUID cyPhyMLHasDescriptionAndGUID in cyPhyMLComponent.AllChildren.
             Where(x => x.GetType().UnderlyingSystemType == typeof(CyPhyMLClasses.Parameter) || x.GetType().UnderlyingSystemType == typeof(CyPhyMLClasses.Property) || x.GetType().UnderlyingSystemType == typeof(CyPhyMLClasses.Metric))
            ) {
                Type type = cyPhyMLHasDescriptionAndGUID.GetType();
                string name = cyPhyMLHasDescriptionAndGUID.Name;
                if (!namePPMListMap.ContainsKey(name)) {
                    namePPMListMap[name] = new List<CyPhyML.HasDescriptionAndGUID>();
                }
                namePPMListMap[name].Add(cyPhyMLHasDescriptionAndGUID);
            }

            foreach (string name in namePPMListMap.Keys) {
                List<CyPhyML.HasDescriptionAndGUID> ppmList = namePPMListMap[name];
                if (ppmList.Count > 1) {
                    var genericRuleFeedback = new GenericRuleFeedback() {
                        FeedbackType = FeedbackTypes.Error,
                        Message = "Name \"" + name + "\" not unique between Parameter, Property, and Metric children of Component \"" + cyPhyMLComponent.Name + "\""
                    };
                    result.Add(genericRuleFeedback);
                }
            }
            return result;
        }
    }
}
