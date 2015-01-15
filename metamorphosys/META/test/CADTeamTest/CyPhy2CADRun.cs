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
using System.IO;
using GME.MGA;
using Xunit;

namespace CADTeamTest
{
    public static class CyPhy2CADRun
    {
        private static void CopyDirectory(string strSource, string strDestination)
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(strSource);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name));
            }
            DirectoryInfo[] dirctororys = dirInfo.GetDirectories();
            foreach (DirectoryInfo tempdir in dirctororys)
            {
                CopyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name));
            }

        }

        public static string GetProjectDir(MgaProject project)
        {
            string workingDir = Path.GetTempPath();
            if (project.ProjectConnStr.StartsWith("MGA="))
            {
                workingDir = Path.GetDirectoryName(project.ProjectConnStr.Substring("MGA=".Length));
            }
            return workingDir;
        }

        public static bool Run(string outputdirname, MgaProject project, MgaFCO testObj, bool copycomponents)
        {
            bool status = true;
            try
            {

                if (copycomponents)
                {
                    CopyDirectory(Path.Combine(GetProjectDir(project),"components"), Path.Combine(outputdirname, "components"));
                }

                var interpreter = new CyPhy2CAD_CSharp.CyPhy2CAD_CSharpInterpreter();
                interpreter.Initialize(project);

                var mainParameters = new CyPhyGUIs.InterpreterMainParameters();
                var cadSettings = new CyPhy2CAD_CSharp.CyPhy2CADSettings();
                cadSettings.OutputDirectory = outputdirname;
                cadSettings.AuxiliaryDirectory = "";
                mainParameters.config = cadSettings;
                mainParameters.Project = project;
                mainParameters.CurrentFCO = testObj;
                mainParameters.SelectedFCOs = (MgaFCOs)Activator.CreateInstance(Type.GetTypeFromProgID("Mga.MgaFCOs"));
                mainParameters.StartModeParam = 128;
                mainParameters.ConsoleMessages = false;
                mainParameters.ProjectDirectory = Path.GetDirectoryName(GetProjectDir(project));
                mainParameters.OutputDirectory = outputdirname;

                interpreter.Main(mainParameters);
            }
            catch (Exception)
            {
                status = false;
            }
            finally
            {
                project.Close();
            }

            return status;

        }

        public static bool Run(string outputdirname, string xmePath, string absPath, bool copycomponents = false, bool deletedir = true)
        {
            bool status = true;
            string ProjectConnStr;
            if (deletedir && Directory.Exists(outputdirname))
            {
                Directory.Delete(outputdirname, true);
            }
            Directory.CreateDirectory(outputdirname);

            MgaUtils.ImportXMEForTest(xmePath, Path.Combine(outputdirname, Path.GetFileNameWithoutExtension(xmePath) + "_CADtest.mga"), out ProjectConnStr);

            MgaProject project = new MgaProject();
            bool ro_mode;
            project.Open(ProjectConnStr, out ro_mode);

            try
            {
                var terr = project.BeginTransactionInNewTerr();
                var testObj = project.ObjectByPath[absPath] as MgaFCO;
                project.AbortTransaction();

                return Run(outputdirname, project, testObj, copycomponents);
            }
            catch(Exception)
            {
                status = false;
            }
            finally
            {
                project.Close();
            }

            return status;
        }
    }
}
