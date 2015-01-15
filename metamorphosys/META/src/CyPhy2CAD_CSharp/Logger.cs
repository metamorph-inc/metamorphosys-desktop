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

namespace CyPhy2CAD_CSharp
{
    public enum Severity
    {
        Normal,
        Info,
        Warning,
        Error
    }

    public struct Message
    {
        public string Content;
        public Severity Level;

        public Message (string content, Severity level)
        {
            Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + content;
            Level = level;
        }
    }
    
    public sealed class Logger : TextWriter
    {
        private static readonly Logger instance = new Logger();
        public static readonly string LogFileName = "CyPhy2CAD.log";
        public List<Message> consoleMessages;
        public List<string> fileMessages;
        public Severity consoleLevel { get; set; }
        public Severity fileLevel { get; set; }

        public int ErrorCnt { get; set; }
        public int WarningCnt { get; set; }

        private Logger()
        {
            Reset();
        }

        public void Reset()
        {
            consoleMessages = new List<Message>();
            fileMessages = new List<string>();
            consoleLevel = Severity.Warning;
            fileLevel = Severity.Normal;
            ErrorCnt = 0;
            WarningCnt = 0;
        }

        public static Logger Instance
        {
            get 
            {
                return instance; 
            }
        }

        public void AddLogMessage(string content, Severity level)
        {
            if (level == Severity.Error)
                ErrorCnt++;
            else if (level == Severity.Warning)
                WarningCnt++;


            if (level >= consoleLevel)
            {
                Message message = new Message(content, level);
                consoleMessages.Add(message);
            }

            if (level >= fileLevel)
                fileMessages.Add(GetSeverityString(level) + ": " + content);
        }

        public void DumpLog(GME.CSharp.GMEConsole console, string path, Severity filelevel = Severity.Normal, Severity consolelevel = Severity.Normal)
        {
            if (fileMessages.Count > 0)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(path, LogFileName)))
                {
                    foreach (string item in fileMessages)
                        file.WriteLine(item);
                }
            }
            
            foreach (Message item in consoleMessages)
            {
                Severity level = item.Level;
                string msg = item.Content;
                if (level == Severity.Error)
                {
                    console.Error.WriteLine(msg);
                }
                else if (level == Severity.Info)
                {
                    console.Info.WriteLine(msg);
                }
                else if (level == Severity.Warning)
                {
                    console.Warning.WriteLine(msg);
                }
                else
                {
                    console.Out.WriteLine(msg);
                }       
            }

            fileMessages.RemoveRange(0, fileMessages.Count());
            consoleMessages.RemoveRange(0, consoleMessages.Count());
            ErrorCnt = 0;
            WarningCnt = 0;
        }

        private string GetSeverityString(Severity level)
        {
            switch (level)
            {
                case Severity.Normal:
                    return "NORMAL";
                case Severity.Info:
                    return "INFO";
                case Severity.Warning:
                    return "WARNING";
                case Severity.Error:
                    return "ERROR";
                default:
                    return "NORMAL";
            }
        }

        override public Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        override public void WriteLine(string str)
        {
            fileMessages.Add(str);
        }

        override public void Write(string str)
        {
            fileMessages.Add(str);
        }
    }
}
