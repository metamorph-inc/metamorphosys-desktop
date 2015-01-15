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

namespace GME.CSharp
{
    using GME.MGA;
    using GME.Util;

    /// <summary>
    /// Defines the static values for COM registration.
    /// </summary>
    internal abstract class ComponentConfig
    {
        /// <summary>
        /// Set paradigm name. Provide * if you want to register it for all paradigms.
        /// </summary>
        public const string ParadigmName = "CyPhyML";

        /// <summary>
        /// Set the human readable name of the interpreter. You can use white space characters.
        /// </summary>
        public const string ComponentName = "CyPhyElaborateCS";

        /// <summary>
        /// Specify an icon path
        /// </summary>
        public const string IconName = "CyPhyElaborateCS.ico";

        /// <summary>
        /// Tool tip will show this text on the GME tool bar.
        /// </summary>
        public const string Tooltip = "CyPhyElaborateCS";

        /// <summary>
        /// Type of the registration mode.
        /// </summary>
        public const regaccessmode_enum RegistrationMode = regaccessmode_enum.REGACCESS_SYSTEM;

        /// <summary>
        /// Programmatic Identifier of the COM component.
        /// </summary>
        public const string ProgID = "MGA.Interpreter.CyPhyElaborateCS";

        /// <summary>
        /// GUID of the COM component.
        /// </summary>
        public const string Guid = "01F4C76D-7980-4C5E-BD65-EA7C0267F55B";

        /// <summary>
        /// Gets or sets the icon path of this component. If null, updated with the assembly path + the iconName dynamically on registration.
        /// </summary>
        public static string IconPath { get; set; }

        /// <summary>
        /// Gets the type of the component. Uncomment the flag if your component is paradigm independent.
        /// </summary>
        public static componenttype_enum ComponentType
        {
            get
            {
                return componenttype_enum.COMPONENTTYPE_INTERPRETER;
            }
        }
    }
}
