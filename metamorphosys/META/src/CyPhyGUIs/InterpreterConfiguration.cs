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
using System.Runtime.InteropServices;

namespace CyPhyGUIs
{
    [Guid("82FADB53-8EAA-471F-BE03-084338D9C830"),
    ProgId("ISIS.CyPhyML.InterpreterResult"),
    ClassInterface(ClassInterfaceType.AutoDual),
    ComVisible(true)]
    public class InterpreterResult : IInterpreterResult
    {
        public CyPhyCOMInterfaces.IMgaTraceability Traceability
        {
            get;
            set;
        }

        public bool Success
        {
            get;
            set;
        }

        public string RunCommand
        {
            get;
            set;
        }
        public string LogFileDirectory
        {
            get;
            set;
        }

        public bool ConsistencyCheckerResult
        {
            get;
            set;
        }

        public string ZippyServerSideHook
        {
            get;
            set;
        }

        public string Labels
        {
            get;
            set;
        }

        public string BuildQuery
        {
            get;
            set;
        }
    }

    [Guid("FA6EE6D3-97FA-4B9F-B350-AAE8AEFC4312")]
    [ProgId("ISIS.CyPhyML.PreConfigArgs"),
    ClassInterface(ClassInterfaceType.AutoDual),
    ComVisible(true)]
    public class PreConfigArgs : IPreConfigParameters
    {
        public string OutputDirectory
        {
            get;
            set;
        }

        public GME.MGA.IMgaProject Project
        {
            get;
            set;
        }

        /**
         * see component_startmode_enum in C:\Program Files (x86)\GME\Interfaces\Mga.idl
         */
        public int StartModeParam
        {
            get;
            set;
        }

        public string ProjectDirectory
        {
            get;
            set;
        }
    }


    [Guid("6C9DC950-A72A-40A2-8EFA-87FEF8777BA3")]
    [ProgId("ISIS.CyPhyML.InterpreterConfiguration"),
    ClassInterface(ClassInterfaceType.AutoDual),
    ComVisible(true)]
    public class InterpreterMainParameters : IInterpreterMainParameters
    {
        public InterpreterMainParameters()
        {
            ConsoleMessages = true;
        }

        public GME.MGA.MgaProject Project
        {
            get;
            set;
        }

        public GME.MGA.MgaFCO CurrentFCO
        {
            get;
            set;
        }

        public GME.MGA.MgaFCOs SelectedFCOs
        {
            get;
            set;
        }

        public int StartModeParam
        {
            get;
            set;
        }

        public string OutputDirectory
        {
            get;
            set;
        }

        public string ProjectDirectory
        {
            get;
            set;
        }

        public IInterpreterConfiguration config
        {
            get;
            set;
        }

        public bool ConsoleMessages
        {
            get;
            set;
        }

        public META.MgaTraceability Traceability
        {
            get;
            set;
        }

        public CyPhyCOMInterfaces.IMgaTraceability GetTraceability()
        {
            return Traceability;
        }

        public bool VerboseConsole
        {
            get;
            set;
        }
    }

    [Guid("846F6E5E-16B3-46FE-A3D9-16C6A2DA9CC6")]
    [ProgId("ISIS.CyPhyML.NullInterpreterConfiguration"),
    ClassInterface(ClassInterfaceType.AutoDual),
    ComVisible(true)]
    public class NullInterpreterConfiguration : IInterpreterConfiguration
    {
    }
}
