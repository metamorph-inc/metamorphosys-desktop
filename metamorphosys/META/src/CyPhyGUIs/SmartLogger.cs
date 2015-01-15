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

namespace CyPhyGUIs
{
    public class ConsoleTextWriter : System.IO.TextWriter
    {
        override public void WriteLine(string str)
        {
            Write(str + Environment.NewLine);
        }

        override public void Write(string str)
        {
            this.Write(SmartLogger.MessageType_enum.Info, str);
        }

        public void WriteLine(SmartLogger.MessageType_enum type, string msg)
        {
            this.Write(type, msg + Environment.NewLine);
        }

        public void Write(SmartLogger.MessageType_enum type, string msg)
        {
            var originalColor = Console.ForegroundColor;

            switch (type)
            {
                case SmartLogger.MessageType_enum.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Out.Write(string.Format("{0,-8} - {1}", type, msg));
                    break;
                case SmartLogger.MessageType_enum.Error:
                case SmartLogger.MessageType_enum.Failed:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.Write(string.Format("{0,-8} - {1}", type, msg));
                    Console.Error.Flush();
                    break;
                case SmartLogger.MessageType_enum.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Out.Write(string.Format("{0,-8} - ", type));
                    Console.ForegroundColor = originalColor;
                    Console.Out.Write(msg);
                    break;
                case SmartLogger.MessageType_enum.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.Write(string.Format("{0,-8} - ", type));
                    Console.ForegroundColor = originalColor;
                    Console.Out.Write(msg);
                    break;
                case SmartLogger.MessageType_enum.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Out.Write(string.Format("{0,-8} - {1}", type, msg));
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.ForegroundColor = originalColor;
        }

        override public Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }
    }

    public class SmartLogger : IDisposable
    {
        public enum MessageType_enum
        {
            Debug,
            Success,
            Info,
            Warning,
            Failed,
            Error
        }

        protected List<TextWriter> m_textWriters { get; set; }

        public MessageType_enum LoggingLevel { get; set; }

        private List<string> m_filenames { get; set; }

        public List<string> LogFilenames
        {
            get
            {
                return this.m_filenames;
            }
        }

        public SmartLogger()
        {
            this.LoggingLevel = MessageType_enum.Debug;
            this.m_textWriters = new List<TextWriter>();
            this.m_filenames = new List<string>();
        }

        public void AddWriter(TextWriter writer, bool useHtmlTags = false)
        {
            if (this.m_textWriters.Contains(writer) == false)
            {
                this.m_textWriters.Add(writer);
            }
        }

        public void AddWriter(string filename)
        {
            // TODO: what if this throughs an exception?

            TextWriter tw = File.AppendText(filename);
            m_textWriters.Add(tw);
            m_filenames.Add(filename);
        }

        protected virtual void Write(TextWriter textWriter, MessageType_enum type, string message)
        {
            if (type >= this.LoggingLevel)
            {
                textWriter.WriteLine("{0} - [{1}] - {2}", DateTime.Now.ToString("s"), type, message);
                textWriter.Flush();
            }
        }

        protected virtual void WriteAll(MessageType_enum type, string message)
        {
            foreach (var textWriter in this.m_textWriters)
            {
                this.Write(textWriter, type, message);
            }
        }

        protected void WriteAll(MessageType_enum type, string format, params object[] args)
        {
            this.WriteAll(type, string.Format(format, args));
        }

        public void WriteError(string message)
        {
            this.WriteAll(MessageType_enum.Error, message);
        }

        public void WriteError(string format, params object[] args)
        {
            this.WriteAll(MessageType_enum.Error, format, args);
        }

        public void WriteWarning(string message)
        {
            this.WriteAll(MessageType_enum.Warning, message);
        }

        public void WriteWarning(string format, params object[] args)
        {
            this.WriteAll(MessageType_enum.Warning, format, args);
        }

        public void WriteInfo(string message)
        {
            this.WriteAll(MessageType_enum.Info, message);
        }

        public void WriteInfo(string format, params object[] args)
        {
            this.WriteAll(MessageType_enum.Info, format, args);
        }

        public void WriteDebug(string message)
        {
            this.WriteAll(MessageType_enum.Debug, message);
        }

        public void WriteDebug(string format, params object[] args)
        {
            this.WriteAll(MessageType_enum.Debug, format, args);
        }


        public void WriteFailed(string message)
        {
            this.WriteAll(MessageType_enum.Failed, message);
        }

        public void WriteFailed(string format, params object[] args)
        {
            this.WriteAll(MessageType_enum.Failed, format, args);
        }

        public void WriteSuccess(string message)
        {
            this.WriteAll(MessageType_enum.Success, message);
        }

        public void WriteSuccess(string format, params object[] args)
        {
            this.WriteAll(MessageType_enum.Success, format, args);
        }


        public virtual void Dispose()
        {
            foreach (var textWriter in this.m_textWriters)
            {
                textWriter.Dispose();
            }
        }
    }
}
