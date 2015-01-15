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
using GME;

namespace GME.CSharp
{
    /// <summary>
    /// Automatically redirects console messages to the GME console output, if GME is available.
    /// Otherwise prints the output to System console.
    /// </summary>
    public class GMEConsole
    {

        /// <summary>
        /// The GME application variable
        /// </summary>
        public IGMEOLEApp gme = null;
        private GMETextWriter error;
        private GMETextWriter warning;
        private GMETextWriter info;
        private GMETextWriter normal;

        public GMEConsole()
        {
            error = new GMETextWriter(msgtype_enum.MSG_ERROR, this);
            warning = new GMETextWriter(msgtype_enum.MSG_WARNING, this);
            info = new GMETextWriter(msgtype_enum.MSG_INFO, this);
            normal = new GMETextWriter(msgtype_enum.MSG_NORMAL, this);
        }

        /// <summary>
        /// Handles error messages
        /// The message to be written. GME Console does not handle special characters and trims white-spaces.
        /// Example: GMEConsole.Error.Write("RootFolder name error: {0}.", rf.Name);
        /// If console is initialized, the message appears in GME console, if not, then in standard error.
        /// If DEBUG is defined, it also appears in VS output window.
        /// </summary>
        public TextWriter Error
        {
            get { return error; }
        }

        /// <summary>
        /// Prints messages.
        /// The message to be written. GME Console does not handle special characters and trims white-spaces.
        /// Example: GMEConsole.Out.Write("RootFolder name : {0}.", rf.Name);
        /// </summary>
        public TextWriter Out
        {
            get { return normal; }
        }


        /// <summary>
        /// Prints warning messages.
        /// The message to be written. GME Console does not handle special characters and trims white-spaces.
        /// Example: GMEConsole.Warning.Write("RootFolder name is not changed : {0}.", rf.Name);
        /// </summary>
        public TextWriter Warning
        {
            get { return warning; }
        }


        /// <summary>
        /// Proints info messages.
        /// The message to be written. GME Console does not handle special characters and trims white-spaces.
        /// Example: GMEConsole.Info.Write("RootFolder name is changed : {0}.", rf.Name);
        /// </summary>
        public TextWriter Info
        {
            get { return info; }
        }

        /// <summary>
        /// Clear the console
        /// </summary>
        public void Clear()
        {
            if (gme != null)
                gme.ConsoleClear();
            else
                try
                {
                    System.Console.Clear();
                }
                catch (IOException) // fails if the console is redirected to a file
                {
                }
        }


        public static GMEConsole CreateFromProject(GME.MGA.MgaProject project)
        {
            GMEConsole console = new GMEConsole();
            try
            {
                // Initializing console               
                console.gme = (IGMEOLEApp)project.GetClientByName("GME.Application").OLEServer;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                // if GME is not present, the interpreter is called from standalone test application
                if (ex.ErrorCode != -2023423888) // HResult 0x87650070: "Search by name failed"
                {
                    throw;
                }
                console.gme = null;
            }
            return console;
        }
    }


    public class GMETextWriter : System.IO.TextWriter
    {
        private msgtype_enum type;
        private GMEConsole console;

        public GMETextWriter(msgtype_enum type, GMEConsole console)
        {
            this.type = type;
            this.console = console;
        }

        override public Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        override public void WriteLine(string str)
        {
            Write(str + Environment.NewLine);
        }

        override public void Write(string str)
        {
            if (console.gme == null)
            {
                switch (type)
                {
                    case msgtype_enum.MSG_NORMAL:
                        Console.Out.Write(str);
                        break;
                    case msgtype_enum.MSG_INFO:
                        Console.Out.Write("Information: " + str);
                        break;
                    case msgtype_enum.MSG_WARNING:
                        Console.Out.Write("Warning: " + str);
                        break;
                    case msgtype_enum.MSG_ERROR:
                        Console.Error.Write(str);
#if(DEBUG)
                        System.Diagnostics.Debug.Write(str);
#endif
                        break;
                }
            }
            else
            {
                console.gme.ConsoleMessage(str, type);
            }
        }
    }
}
