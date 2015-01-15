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
using Microsoft.Win32;

// new GME registrar
namespace GME
{
    [ComVisible(false)]
    class RegistrationException : ApplicationException
    {
        public RegistrationException(string message) : base(message) { }
    }

    [ComVisible(false)]
    class Registrar
    {
        private Registrar()
        {
        }

        public static void RegisterComponentsInGMERegistry()
        {

            if (ComponentConfig.iconPath == null )
            {
                ComponentConfig.iconPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + '\\' + ComponentConfig.iconName;
            }

            GME.Util.MgaRegistrar registrar = new GME.Util.MgaRegistrar();
            if ((int)GME.MGA.Core.GMEInterfaceVersion_enum.GMEInterfaceVersion_Current != (int)((GME.MGA.Core.IGMEVersionInfo)registrar).version)
            {
                throw new RegistrationException("MgaInterfaceVersion mismatch: this assembly is using " +
                    (int)GME.MGA.Core.GMEInterfaceVersion_enum.GMEInterfaceVersion_Current +
                    " but the GME interface version is " + (int)((GME.MGA.Core.IGMEVersionInfo)registrar).version +
                    "\n\nPlease install a compatible GME version or update the interop dlls.");
            }

            registrar.RegisterComponent(ComponentConfig.progID, ComponentConfig.componentType, ComponentConfig.componentName, ComponentConfig.registrationMode);
            registrar.set_ComponentExtraInfo(ComponentConfig.registrationMode, ComponentConfig.progID, "Icon", ComponentConfig.iconPath);

            if (!ComponentConfig.paradigmName.Equals("*"))
            {
                foreach (String paradigm in ComponentConfig.paradigmName.Split(','))
                {
                    registrar.Associate(ComponentConfig.progID, paradigm, ComponentConfig.registrationMode);
                }
            }
        }

        public static void UnregisterComponentsInGMERegistry()
        {
            GME.Util.MgaRegistrar registrar = new GME.Util.MgaRegistrar();
            if ((int)GME.MGA.Core.GMEInterfaceVersion_enum.GMEInterfaceVersion_Current != (int)((GME.MGA.Core.IGMEVersionInfo)registrar).version)
            {
                throw new RegistrationException("MgaInterfaceVersion mismatch: this assembly is using " +
                    (int)GME.MGA.Core.GMEInterfaceVersion_enum.GMEInterfaceVersion_Current +
                    " but the GME interface version is " + (int)((GME.MGA.Core.IGMEVersionInfo)registrar).version +
                    "\n\nPlease install a compatible GME version or update the interop dlls.");
            }

            registrar.UnregisterComponent(ComponentConfig.progID, ComponentConfig.registrationMode);
        }

        public void DLLRegisterServer(int regMode)
        {
            //register dll 
            //same as:
            //post build event: C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\regasm.exe $(TargetPath) /codebase

            RegistrationServices regAsm = new RegistrationServices();
            bool bResult = regAsm.RegisterAssembly(ComponentConfig.typeToRegister.Assembly, AssemblyRegistrationFlags.SetCodeBase);
        }
    }
}

#region Old-GME style registrar
namespace GME.CSharp
{
    [ComVisible(false)]
    public class Registrar
    {
        public Registrar()
        {
        }

        GME.Util.regaccessmode_enum regacc_translate(int x)
        {
            return (GME.Util.regaccessmode_enum)x;
        }

        public void DLLRegisterServer(int regMode)
        {
            //register dll 
            //same as:
            //post build event: C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\regasm.exe $(TargetPath) /codebase

            RegistrationServices regAsm = new RegistrationServices();
            bool bResult = regAsm.RegisterAssembly(
                ComponentConfig.typeToRegister.Assembly,
                AssemblyRegistrationFlags.SetCodeBase);

            GME.Util.MgaRegistrar reg = new GME.Util.MgaRegistrar();
            if ((int)GME.MGA.Core.GMEInterfaceVersion_enum.GMEInterfaceVersion_Current != (int)((GME.MGA.Core.IGMEVersionInfo)reg).version)
            {
                throw new Exception("MgaInterfaceVersion mismatch: this assembly is using " +
                    (int)GME.MGA.Core.GMEInterfaceVersion_enum.GMEInterfaceVersion_Current +
                    " but the GME interface version is " + (int)((GME.MGA.Core.IGMEVersionInfo)reg).version +
                    "\n\nPlease install a compatible GME version or update the interop dlls.");
            }
            reg.RegisterComponent(
                regAsm.GetProgIdForType(ComponentConfig.typeToRegister),
                GME.MGA.componenttype_enum.COMPONENTTYPE_INTERPRETER,
                ComponentConfig.componentName,
                regacc_translate(regMode));

            reg.set_ComponentExtraInfo(regacc_translate(regMode), regAsm.GetProgIdForType(ComponentConfig.typeToRegister), "Icon", ComponentConfig.iconId);

            if (!ComponentConfig.paradigmName.Equals("*"))
            {
                reg.Associate(regAsm.GetProgIdForType(ComponentConfig.typeToRegister), ComponentConfig.paradigmName, (GME.Util.regaccessmode_enum)regMode);
            }

        }
    }
}
#endregion
