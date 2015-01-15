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

// -----------------------------------------------------------------------
// <copyright file="AnalysisTool.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace META
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Win32;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnalysisTool
    {
        public const string ParameterNameInWorkflow = "AnalysisTool";

        public static List<AnalysisTool> GetFromProgId(string progid)
        {
            if (_byProgId == null)
            {
                CacheTools();
            }
            List<AnalysisTool> tools = new List<AnalysisTool>();
            if (progid != null)
            {
                _byProgId.TryGetValue(progid, out tools);
                if (tools == null)
                {
                    tools = new List<AnalysisTool>();
                }
            }
            return tools;
        }

        public static AnalysisTool GetByName(string analysisToolName)
        {
            if (_byName == null)
            {
                CacheTools();
            }
            AnalysisTool result = null;
            if (analysisToolName != null)
            {
                _byName.TryGetValue(analysisToolName, out result);
            }
            return result;
        }

        private static SortedDictionary<string, List<AnalysisTool>> _byProgId;
        private static SortedDictionary<string, AnalysisTool> _byName;

        private static void CacheTools()
        {
            _byName = new SortedDictionary<string, AnalysisTool>();
            _byProgId = new SortedDictionary<string, List<AnalysisTool>>();

            var analysisToolKey = Registry.LocalMachine.OpenSubKey(@"Software\META\AnalysisTools");
            if (analysisToolKey != null)
            {
                foreach (string subkeyname in analysisToolKey.GetSubKeyNames())
                {
                    AnalysisTool analysistool = new AnalysisTool()
                        {
                            Name = subkeyname
                        };

                    var subkey = analysisToolKey.OpenSubKey(subkeyname);
                    string value;
                    value = subkey.GetValue("InstallLocation") as string;
                    if (value != null)
                    {
                        analysistool.InstallLocation = value;
                    }

                    value = subkey.GetValue("Version") as string;
                    if (value != null)
                    {
                        analysistool.Version = value;
                    }

                    value = subkey.GetValue("OutputDirectory") as string;
                    if (value != null)
                    {
                        analysistool.OutputDirectory = value;
                    }

                    value = subkey.GetValue("RunCommand") as string;
                    if (value != null)
                    {
                        analysistool.RunCommand = value;
                    }

                    value = subkey.GetValue("RequiredInterpreter") as string;
                    if (value != null)
                    {
                        analysistool.RequiredInterpreter = value;
                    }

                    List<AnalysisTool> tools = new List<AnalysisTool>();
                    _byProgId.TryGetValue(analysistool.RequiredInterpreter, out tools);
                    if (tools == null)
                    {
                        tools = new List<AnalysisTool>();
                    }
                    tools.Add(analysistool);
                    _byProgId[analysistool.RequiredInterpreter] = tools;
                    _byName[analysistool.Name] = analysistool;
                }
            }
        }

        public static void ApplyToolSelection(
            string progid,
            Dictionary<string, string> workflowParameters,
            CyPhyGUIs.InterpreterResult interpreterResult,
            CyPhyGUIs.IInterpreterMainParameters interpreterMainParameters,
            bool modifyLabels=true)
        {
            string toolName;
            workflowParameters.TryGetValue(AnalysisTool.ParameterNameInWorkflow, out toolName);
            if (string.IsNullOrEmpty(toolName) ||
                toolName == "Default")
            {
                return;
            }

            var tool = GetByName(toolName);

            if (tool == null)
            {
                return;
            }

            // copy tool.OutputDirectory to generated directory
            MethodDelegateCopy toolOutputDirCopy = new MethodDelegateCopy(DirectoryCopy);
            IAsyncResult ar = toolOutputDirCopy.BeginInvoke(Path.Combine(tool.InstallLocation, tool.OutputDirectory), interpreterMainParameters.OutputDirectory, true, null, null);
            toolOutputDirCopy.EndInvoke(ar);

            interpreterResult.RunCommand = tool.RunCommand;
            if (modifyLabels)
            {
                interpreterResult.Labels += " && " + tool.Name;
            }
        }


        public string Name { get; private set; }
        public string InstallLocation { get; private set; }
        public string Version { get; private set; }
        public string OutputDirectory { get; private set; }
        public string RunCommand { get; private set; }
        public string RequiredInterpreter { get; private set; }


        private delegate void MethodDelegateCopy(
            string sourceDirName,
            string destDirName,
            bool copySubDirs);

        private static void DirectoryCopy(
            string sourceDirName,
            string destDirName,
            bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
