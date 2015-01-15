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
using DesignConsistencyChecker;
using DesignConsistencyChecker.DesignRule;
using CyPhyCOMInterfaces;

namespace CyPhySoT.Rules
{
    public class Checker
    {
        private CyPhyGUIs.IInterpreterMainParameters mainParameters { get; set; }

        public Dictionary<RuleDescriptor, List<RuleFeedbackBase>> Results { get; set; }
        //public static GME.CSharp.GMEConsole GMEConsole { get; set; }
        public CyPhyGUIs.GMELogger Logger { get; set; }
        public Checker(CyPhyGUIs.IInterpreterMainParameters parameters, CyPhyGUIs.GMELogger logger)
        {
            this.Logger = logger;
            this.mainParameters = parameters;
            this.Results = new Dictionary<RuleDescriptor, List<RuleFeedbackBase>>();
        }

        int NbrOfErrors { get; set; }
        int NbrOfWarnings { get; set; }
        /// <summary>
        /// Checks all rules from this dll.
        /// </summary>
        /// <returns>True if there are no errors, otherwise false</returns>
        public bool Check(IMgaTraceability traceability)
        {
            bool result = true;

            // check current context
            if (this.mainParameters.CurrentFCO == null ||
                this.mainParameters.CurrentFCO.Meta.Name != typeof(ISIS.GME.Dsml.CyPhyML.Interfaces.TestBenchSuite).Name)
            {
                this.Logger.WriteError("A TestBenchSuite must be opened.");
                result = false;
                NbrOfErrors = 1;
                return result;
            }

            // reset the results dictionary
            this.Results = new Dictionary<RuleDescriptor, List<RuleFeedbackBase>>();

            // Create a checker object
            DesignConsistencyChecker.Framework.Checker dccChecker =
                new DesignConsistencyChecker.Framework.Checker(this.mainParameters.CurrentFCO, this.mainParameters.Project, traceability, this.Logger);

            dccChecker.RegisterRulesForTypes(new Type[] { typeof(CyPhySoT.Rules.Global) }, "CyPhySoT");

            NbrOfErrors = 0;
            NbrOfWarnings = 0;
            // get all rules and check them

            List<RuleFeedbackBase> ruleFeedbacks;
            List<CheckerFeedback> checkerFeedbacks;
            dccChecker.CheckRules(dccChecker.GetRegisteredRules, out ruleFeedbacks, out checkerFeedbacks);
            if (ruleFeedbacks.Any(x => x.FeedbackType == FeedbackTypes.Error))
            {
                result = false;
            }
            NbrOfErrors += ruleFeedbacks.Where(x => x.FeedbackType == FeedbackTypes.Error).Count();
            NbrOfWarnings += ruleFeedbacks.Where(x => x.FeedbackType == FeedbackTypes.Warning).Count();

            return result;
        }

        public void PrintDetails()
        {
            if (NbrOfErrors > 0)
            {
                this.Logger.WriteError("Errors: {0}, Warnings: {1}", NbrOfErrors, NbrOfWarnings);
            }
            else if (NbrOfWarnings > 0)
            {
                this.Logger.WriteWarning("Errors: {0}, Warnings: {1}", NbrOfErrors, NbrOfWarnings);
            }
            else
            {
                this.Logger.WriteDebug("Errors: {0}, Warnings: {1}", NbrOfErrors, NbrOfWarnings);
            }
        }
    }
}
