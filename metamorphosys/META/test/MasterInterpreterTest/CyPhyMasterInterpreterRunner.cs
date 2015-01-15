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
using Xunit;
using System.IO;

namespace MasterInterpreterTest
{
    public class CyPhyMasterInterpreterRunner
    {

        public static bool RunContextCheck(string projectPath, string absPath)
        {
            bool result = false;
            Assert.True(File.Exists(projectPath), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + Path.GetFullPath(projectPath);

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var testObj = project.ObjectByPath[absPath] as MgaFCO;
                project.AbortTransaction();

                using (var masterInterpreter = new CyPhyMasterInterpreter.CyPhyMasterInterpreterAPI(project))
                {
                    CyPhyMasterInterpreter.Rules.ContextCheckerResult[] contextCheckerResults = null;

                    // check context
                    result = masterInterpreter.TryCheckContext(testObj as MgaModel, out contextCheckerResults);
                }
            }
            finally
            {
                project.Close(true);
            }

            return result;
        }


        public static bool GetConfigurations(string projectPath, string absPath)
        {

            bool result = false;
            Assert.True(File.Exists(projectPath), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + Path.GetFullPath(projectPath);

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var testObj = project.ObjectByPath[absPath] as MgaFCO;
                project.AbortTransaction();

                using (var masterInterpreter = new CyPhyMasterInterpreter.CyPhyMasterInterpreterAPI(project))
                {
                    IMgaFCOs configurations = null;

                    Assert.ThrowsDelegate d = () =>
                    {
                        configurations = masterInterpreter.GetConfigurations(testObj as MgaModel);
                    };

                    Assert.DoesNotThrow(d);
                }
            }
            finally
            {
                project.Close(true);
            }

            return result;
        }

        public static bool AnalysisModelSupported(string projectPath, string absPath)
        {

            bool result = false;
            Assert.True(File.Exists(projectPath), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + Path.GetFullPath(projectPath);

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var testObj = project.ObjectByPath[absPath] as MgaFCO;
                project.AbortTransaction();

                CyPhyMasterInterpreter.AnalysisModelProcessor analysisModelProcessor = null;

                 project.BeginTransactionInNewTerr();
                    try
                    {
                        Assert.ThrowsDelegate d = () =>
                        {
                            analysisModelProcessor = CyPhyMasterInterpreter.AnalysisModelProcessor.GetAnalysisModelProcessor(testObj as MgaModel);
                        };
                        Assert.DoesNotThrow(d);
                            Assert.True(analysisModelProcessor != null, string.Format("Analysis model processor was not able to create the model processor for {0} {1}.", testObj.Name, testObj.Meta.Name));
                    }
                    finally
                    {
                        project.AbortTransaction();
                    }

            }
            finally
            {
                project.Close(true);
            }

            return result;
        }

        public static void AnalysisModelNotSupported(string projectPath, string absPath)
        {
            Assert.True(File.Exists(projectPath), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + Path.GetFullPath(projectPath);

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var testObj = project.ObjectByPath[absPath] as MgaFCO;
                project.AbortTransaction();

                CyPhyMasterInterpreter.AnalysisModelProcessor analysisModelProcessor = null;

                project.BeginTransactionInNewTerr();
                try
                {
                    Assert.ThrowsDelegate d = () =>
                    {
                        analysisModelProcessor = CyPhyMasterInterpreter.AnalysisModelProcessor.GetAnalysisModelProcessor(testObj as MgaModel);
                    };

                    Assert.False(analysisModelProcessor != null, string.Format("Analysis model processor was not able to create the model processor for {0} {1}.", testObj.Name, testObj.Meta.Name));

                    Assert.Throws<CyPhyMasterInterpreter.AnalysisModelContextNotSupportedException>(d);
                }
                finally
                {
                    project.AbortTransaction();
                }
            }
            finally
            {
                project.Close(true);
            }
        }

        public static bool RunMasterInterpreter(
            string projectPath,
            string absPath,
            string configPath,
            bool postToJobManager = false,
            bool keepTempModels = false)
        {
            bool result = false;
            Assert.True(File.Exists(projectPath), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + Path.GetFullPath(projectPath);

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var testObj = project.ObjectByPath[absPath] as MgaFCO;
                var configObj = project.ObjectByPath[configPath] as MgaFCO;
                project.AbortTransaction();

                using (var masterInterpreter = new CyPhyMasterInterpreter.CyPhyMasterInterpreterAPI(project))
                {
                    masterInterpreter.Logger.GMEConsoleLoggingLevel = CyPhyGUIs.SmartLogger.MessageType_enum.Debug;

                    var miResults = masterInterpreter.RunInTransactionOnOneConfig(testObj as MgaModel, configObj, postToJobManager, keepTempModels);
                    result = miResults.Any(x => x.Success == false) ? false: true;
                }
            }
            finally
            {
                project.Close(true);
            }

            return result;
        }

    }
}
