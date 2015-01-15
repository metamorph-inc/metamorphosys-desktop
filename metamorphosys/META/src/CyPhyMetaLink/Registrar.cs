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
using System.Text;
using System.Runtime.InteropServices;
using GME;
using GME.Util;
using GME.MGA;
using GME.MGA.Core;
using Microsoft.Win32;

namespace GME.CSharp
{
    [ComVisible(false)]
    internal class RegistrationException : ApplicationException
    {
        public RegistrationException(string message) : base(message) { }
    }

    [ComVisible(false)]
    internal static class Registrar
    {
        public static void RegisterAddon()
        {
            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);
            registrar.RegisterComponent(ComponentConfig_Addon.progID, ComponentConfig_Addon.componentType, ComponentConfig_Addon.componentName, ComponentConfig_Addon.registrationMode);

            if (!ComponentConfig_Addon.paradigmName.Equals("*"))
            {
                registrar.Associate(
                   ComponentConfig_Addon.progID,
                    ComponentConfig_Addon.paradigmName,
                    ComponentConfig_Addon.registrationMode);
            }
        }

        public static MgaRegistrar RegisterInterpreter()
        {
            if (ComponentConfig_Intf.iconPath == null)
            {
                ComponentConfig_Intf.iconPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + '\\' + ComponentConfig_Intf.iconName;
            }

            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);
            registrar.RegisterComponent(ComponentConfig_Intf.progID, ComponentConfig_Intf.componentType, ComponentConfig_Intf.componentName, ComponentConfig_Intf.registrationMode);
            registrar.set_ComponentExtraInfo(ComponentConfig_Intf.registrationMode, ComponentConfig_Intf.progID, "Icon", ComponentConfig_Intf.iconPath);
            registrar.set_ComponentExtraInfo(ComponentConfig_Intf.registrationMode, ComponentConfig_Intf.progID, "Tooltip", ComponentConfig_Intf.tooltip);

            if (!ComponentConfig_Intf.paradigmName.Equals("*"))
            {
                registrar.Associate(
                   ComponentConfig_Intf.progID,
                    ComponentConfig_Intf.paradigmName,
                    ComponentConfig_Intf.registrationMode);
            }
            return registrar;
        }

        private static void CheckGMEInterfaceVersion(MgaRegistrar registrar)
        {
            if ((int)GMEInterfaceVersion_enum.GMEInterfaceVersion_Current != (int)((IGMEVersionInfo)registrar).version)
            {
                throw new RegistrationException("GMEInterfaceVersion mismatch: this assembly is using " +
                    (int)GMEInterfaceVersion_enum.GMEInterfaceVersion_Current +
                    " but the GME interface version is " + (int)((IGMEVersionInfo)registrar).version +
                    "\n\nPlease install a compatible GME version or update the interop dlls.");
            }

        }

        public static void UnregisterInterpreter()
        {
            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);

            registrar.UnregisterComponent(ComponentConfig_Intf.progID, ComponentConfig_Intf.registrationMode);
        }

        public static void UnregisterAddon()
        {
            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);

            registrar.UnregisterComponent(ComponentConfig_Addon.progID, ComponentConfig_Addon.registrationMode);
        }
    }
}
