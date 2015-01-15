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
using GME;
using System.IO;
using System.Reflection;
using GME.CSharp;

namespace CyPhyGUIs
{
    public class GMELogger : SmartLogger
    {
        private const string BADGE_COMMON_STYLE = "block:inline;display: inline;padding-left: 6px;margin-left: 6px;padding-right: 6px; margin-right: 6px; width: 60px; text-align: center;";
        public const string ERROR_BADGE = "<div style=\"" + BADGE_COMMON_STYLE +
            " background-color: rgb(255, 145, 145);color: rgb(255, 0, 0);\">Error</div>";

        public const string WARNING_BADGE = "<div style=\"" + BADGE_COMMON_STYLE +
            " background-color: rgb(255, 255, 79);color: rgb(255, 112, 0);\">Warning</div>";

        public const string INFO_BADGE = "<div style=\"" + BADGE_COMMON_STYLE +
            " background-color: rgb(205, 227, 255);color: rgb(0, 51, 255);\">Info</div>";

        public const string DEBUG_BADGE = "<div style=\"" + BADGE_COMMON_STYLE +
            " background-color: rgb(230, 230, 230);color: rgb(0, 0, 0);\">Debug</div>";

        public const string FAILED_BADGE = "<div style=\"" + BADGE_COMMON_STYLE +
            " background-color: rgb(255, 203, 203);color: rgb(151, 19, 19);\">Failed</div>";

        public const string SUCCESS_BADGE = "<div style=\"" + BADGE_COMMON_STYLE +
            "background-color: rgb(212, 255, 203);color: rgb(31, 129, 12);\">Success</div>";

        private Dictionary<MessageType_enum, string> messageTypeToHtmlBadge =
            new Dictionary<MessageType_enum, string>()
            {
                {MessageType_enum.Debug, DEBUG_BADGE},
                {MessageType_enum.Success, SUCCESS_BADGE},
                {MessageType_enum.Info, INFO_BADGE},
                {MessageType_enum.Warning, WARNING_BADGE},
                {MessageType_enum.Failed, FAILED_BADGE},
                {MessageType_enum.Error, ERROR_BADGE},
                
            };

        /// <summary>
        /// Only for the GME console, not for the log files!
        /// </summary>
        public MessageType_enum GMEConsoleLoggingLevel { get; set; }

        public CyPhyCOMInterfaces.IMgaTraceability Traceability { get; set; }

        private MgaProject m_project { get; set; }

        public GMELogger(MgaProject project, string interpreterName = null)
        {
            this.m_project = project;
            this.GMEConsoleLoggingLevel = MessageType_enum.Success;
            this.Traceability = new META.MgaTraceability();

            var gme_console = GMEConsole.CreateFromProject(project);
            this.AddWriter(gme_console.Out);

            if (string.IsNullOrWhiteSpace(interpreterName) == false)
            {
                var logFilePath = Path.Combine(Path.GetDirectoryName(project.ProjectConnStr.Substring("MGA=".Length)), "log");

                Directory.CreateDirectory(logFilePath);

                string logFileName = string.Format("{0}.{1}.log",
                    interpreterName,
                    System.Diagnostics.Process.GetCurrentProcess().Id);

                this.AddWriter(Path.Combine(logFilePath, logFileName));

                // TODO: would be nice to log as html file too.
            }
        }

        private IAsyncResult LoggingVersionInfoAsyncResult = null;

        public Func<MgaProject, string> LoggingVersionInfo = new Func<MgaProject, string>((project) =>
        {
            return META.Logger.Header(project);
        });

        private void MakeVersionInfoHeaderAsync()
        {
            // generate header in an async mode i.e. this will not block any UI activity.
            // but there is no guarantee that the header will be the first entry in the log.
            this.LoggingVersionInfoAsyncResult = this.LoggingVersionInfo.BeginInvoke(this.m_project, (result) =>
            {
                if (result.IsCompleted == false)
                {
                    var header = this.LoggingVersionInfo.EndInvoke(result);
                    this.WriteDebug(header);
                }
            }, null);
        }

        public void MakeVersionInfoHeader()
        {
            string header = this.LoggingVersionInfo.Invoke(this.m_project);
            this.WriteDebug(header);
        }

        public override void Dispose()
        {
            if (this.LoggingVersionInfoAsyncResult != null &&
                this.LoggingVersionInfoAsyncResult.IsCompleted == false)
            {
                var header = this.LoggingVersionInfo.EndInvoke(this.LoggingVersionInfoAsyncResult);
                this.WriteDebug(header);
            }
            base.Dispose();
        }

        protected override void WriteAll(MessageType_enum type, string message)
        {
            foreach (var textwriter in this.m_textWriters)
            {
                if (textwriter is GMETextWriter)
                {
                    if (type >= this.GMEConsoleLoggingLevel)
                    {
                        textwriter.WriteLine("{0} {1}", messageTypeToHtmlBadge[type], message);
                    }
                }
                else if (textwriter is ConsoleTextWriter)
                {
                    var consoleTextWriter = textwriter as ConsoleTextWriter;
                    consoleTextWriter.WriteLine(type, message);
                }
                else
                {
                    base.Write(textwriter, type, message);
                }
            }
        }

        public void WriteCheckPassed(MgaFCO subject, string message)
        {
            this.WriteAll(MessageType_enum.Success, string.Format("{0} {1}", subject.ToMgaHyperLink(this.Traceability), message));
        }

        public void WriteCheckPassed(string message)
        {
            this.WriteAll(MessageType_enum.Success, message);
        }

        public void WriteCheckPassed(string format, params object[] args)
        {
            base.WriteAll(MessageType_enum.Success, format, args);
        }

        public void WriteCheckFailed(MgaFCO subject, string message)
        {
            this.WriteAll(MessageType_enum.Success, string.Format("{0} {1}", subject.ToMgaHyperLink(this.Traceability), message));
        }

        public void WriteCheckFailed(string message)
        {
            this.WriteAll(MessageType_enum.Failed, message);
        }

        public void WriteCheckFailed(string format, params object[] args)
        {
            base.WriteAll(MessageType_enum.Failed, format, args);
        }
    }
}
