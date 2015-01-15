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
// <copyright file="Logger.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace META
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Logger
    {
        private static Dictionary<string, TraceListener> LoggingFiles = new Dictionary<string, TraceListener>();

        public static string Header(GME.MGA.MgaProject project = null)
        {
            StringBuilder logFileHeader = new StringBuilder();

            logFileHeader.AppendLine("=== HEADER ===================================================");
            logFileHeader.AppendLine(string.Format("Date/Time:       {0}", DateTime.Now));
            logFileHeader.AppendLine(string.Format("Windows Version: {0}", System.Environment.OSVersion.VersionString));
            logFileHeader.AppendLine(string.Format("x64 bit OS:      {0}", System.Environment.Is64BitOperatingSystem));
            logFileHeader.AppendLine(string.Format("GME version:     {0}", META.VersionInfo.GmeVersion));
            logFileHeader.AppendLine(string.Format("META version:    {0}", META.VersionInfo.MetaVersion));
            logFileHeader.AppendLine(string.Format("META install:    {0}", META.VersionInfo.MetaPath));
            logFileHeader.AppendLine(string.Format("CyPhyML version: {0}", META.VersionInfo.CyPhyML));
            logFileHeader.AppendLine(string.Format("CyPhyML GUID:    {0}", META.VersionInfo.CyPhyMLGuid));

            logFileHeader.AppendLine(string.Format("Python Dll:      {0}", META.VersionInfo.PythonVersion));
            logFileHeader.AppendLine(string.Format("Python Exe:      {0}", META.VersionInfo.PythonExe));
            logFileHeader.AppendLine(string.Format("Python27 VE exe: {0}", META.VersionInfo.PythonVEnvExe));

            logFileHeader.AppendLine(string.Format("PROE ISIS Ext. : {0}", META.VersionInfo.ProeISISExtPath));
            logFileHeader.AppendLine(string.Format("PROE ISIS ver. : {0}", META.VersionInfo.ProeISISExtVer));

            logFileHeader.AppendLine(string.Format("Username:        {0}", Environment.UserName));

            logFileHeader.AppendLine("Currently opened paradigm information");

            if (project != null)
            {
                bool transactionAlreadyOpen = (project.ProjectStatus & 8) == 0;
                try
                {
                    if (transactionAlreadyOpen)
                    {
                        project.BeginTransactionInNewTerr();
                    }

                    logFileHeader.AppendLine(string.Format("  MetaName    : {0}", project.MetaName));
                    logFileHeader.AppendLine(string.Format("  MetaVersion : {0}", project.MetaVersion));

                    //logFileHeader.AppendLine(string.Format("  MetaGuid : {0}", ByteArrayToString((project.MetaGUID as IEnumerable<byte>).ToList())));
                }
                finally
                {
                    if (transactionAlreadyOpen)
                    {
                        project.AbortTransaction();
                    }
                }
            }
            else
            {
                logFileHeader.AppendLine(string.Format("  Not available."));
            }

            return logFileHeader.ToString();
        }

        public static void AddFileListener(string loggingFileName, string name, GME.MGA.MgaProject project = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            }

            if (string.IsNullOrWhiteSpace(loggingFileName))
            {
                // get temp path and temp file name
                loggingFileName = System.IO.Path.GetTempPath() +
                    System.Reflection.Assembly.GetExecutingAssembly().FullName +
                    ".trace.txt";
            }

            loggingFileName = Path.GetFullPath(loggingFileName);

            var dirName = Path.GetDirectoryName(loggingFileName);
            if (Directory.Exists(dirName) == false)
            {
                Directory.CreateDirectory(dirName);
            }

            if (LoggingFiles.Keys.Contains(loggingFileName) == false)
            {
                // set up tracing

                // print header and version info
                var fs = new FileStream(loggingFileName, FileMode.Create);
                TraceListener fileTL = new TextWriterTraceListener(fs)
                {
                    //TraceOutputOptions = TraceOptions.DateTime,
                    Name = name,
                };
                // use TraceXXX to get timestamp per http://stackoverflow.com/questions/863394/add-timestamp-to-trace-writeline

                Trace.AutoFlush = true;
                Trace.Listeners.Add(fileTL);

                LoggingFiles.Add(loggingFileName, fileTL);

                Trace.TraceInformation("{0} trace file listener was created.", loggingFileName);
                Trace.TraceInformation("{0}", Header(project));
            }
            else
            {
                Console.WriteLine("{0} already exists for logging.", loggingFileName);
            }
        }

        public static void RemoveFileListener(string name)
        {
            TraceListener tl = null;
            if (LoggingFiles.TryGetValue(name, out tl))
            {
                Trace.Listeners.Remove(tl);
                Trace.TraceInformation("{0} was removed.", name);
                LoggingFiles.Remove(name);
                tl.Close();
            }
        }
    }
}
