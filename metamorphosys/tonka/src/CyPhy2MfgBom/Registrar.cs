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
        public static void RegisterComponentsInGMERegistry()
        {
            if (ComponentConfig.iconPath == null)
            {
                ComponentConfig.iconPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + '\\' + ComponentConfig.iconName;
            }

            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);
            registrar.RegisterComponent(ComponentConfig.progID, ComponentConfig.componentType, ComponentConfig.componentName, ComponentConfig.registrationMode);
            registrar.set_ComponentExtraInfo(ComponentConfig.registrationMode, ComponentConfig.progID, "Icon", ComponentConfig.iconPath);
            registrar.set_ComponentExtraInfo(ComponentConfig.registrationMode, ComponentConfig.progID, "Tooltip", ComponentConfig.tooltip);

            if (!ComponentConfig.paradigmName.Equals("*"))
            {
                registrar.Associate(
                   ComponentConfig.progID,
                    ComponentConfig.paradigmName,
                    ComponentConfig.registrationMode);
            }
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


        public static void UnregisterComponentsInGMERegistry()
        {
            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);

            registrar.UnregisterComponent(ComponentConfig.progID, ComponentConfig.registrationMode);
        }
    }
}
