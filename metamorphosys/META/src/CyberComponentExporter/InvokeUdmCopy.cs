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
using System.Runtime.InteropServices;
using GME.MGA;
using GME;
using System.IO;
using System.Diagnostics;

// Modified from Run_ESMoL_Toolchain by Kevin Smyth

namespace Cyber2AVM
{

    public class InvokeUDMCopy
    {
        private string getPath(IMgaFCO fco)
        {
            string path = fco.Name;
            while (fco.ParentModel != null)
            {
                path = fco.ParentModel.Name + "/" + path;
                fco = fco.ParentModel;
            }
            IMgaFolder folder = fco.ParentFolder;
            while (folder != null)
            {
                path = folder.Name + "/" + path;
                folder = folder.ParentFolder;
            }
            return "/" + path;
        }

        string UDM_PATH = "";
        public bool GenerateXML(MgaProject project, string input_filename)
        {
            GME.CSharp.GMEConsole console = GME.CSharp.GMEConsole.CreateFromProject(project);

            string[] filename_parts = input_filename.Split('.');
            string output_filename = filename_parts[0] + ".xml";
            int pathPosition = output_filename.LastIndexOf("\\");
            string output_path = output_filename.Substring(0, pathPosition + 1) + "\\Cyber\\";
            string filename_base = output_filename.Substring(pathPosition + 1);

            if (!Directory.Exists(output_path))
            {
                Directory.CreateDirectory(output_path);
            }

            output_filename = output_path + filename_base;

            UDM_PATH = Environment.GetEnvironmentVariable("UDM_PATH");
            if (UDM_PATH == null || UDM_PATH == "")
            {
                console.Out.WriteLine("Udm not detected. Model will not be translated to XML.");
                return false;
            }

            string keyName = @"HKEY_LOCAL_MACHINE\Software\META";
            string value = @"META_PATH";

            string META_PATH = (string)Microsoft.Win32.Registry.GetValue(
                keyName,
                value,
                "ERROR: " + keyName + value + " does not exist!");


            string cybercomp_paradigm_filename = META_PATH + "\\meta\\CyberComposition_udm.xml";
            string cybercomp_xsd_path = META_PATH + "\\meta\\CyberComposition";

            console.Out.WriteLine("Translating model to XML...");
            console.Out.WriteLine("Output file is " + output_filename);

            runProgram("UdmCopy.exe", new string[] { input_filename, output_filename, cybercomp_paradigm_filename, cybercomp_xsd_path });

            return true;

        }

        private string escapeArguments(string[] arguments)
        {
            string ret = "";
            foreach (string arg in arguments)
            {
                ret += "\"" + arg + "\" ";
            }
            return ret;
        }
        private void runProgram(string program, string[] arguments)
        {
            runProgram(program, escapeArguments(arguments));
        }

        private void runProgram(string program, string arguments)
        {
            program = UDM_PATH + "\\bin\\" + program;
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.FileName = program;
            p.StartInfo.Arguments = arguments;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = " /k \"\"" + program + "\" " + arguments + "\"";
                p.Start();

                throw new ApplicationException("Running " + program + " " + arguments + " failed: " + output);
            }
        }

        private delegate void voidDelegate();
    }

}