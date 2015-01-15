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
    using System;
    using System.Runtime.InteropServices;
    using GME.MGA.Core;
    using GME.Util;

    /// <summary>
    /// Used to register COM component in GME registry.
    /// </summary>
    [ComVisible(false)]
    internal static class Registrar
    {
        /// <summary>
        /// Registers component in the GME registry.
        /// </summary>
        /// <exception cref="RegistrationException">If GME Interface version does not match.</exception>
        public static void RegisterComponentsInGMERegistry()
        {
            if (ComponentConfig.IconPath == null)
            {
                ComponentConfig.IconPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + '\\' + ComponentConfig.IconName;
            }

            MgaRegistrar registrar = new MgaRegistrar();
            
            Registrar.CheckGMEInterfaceVersion(registrar);

            registrar.RegisterComponent(ComponentConfig.ProgID, ComponentConfig.ComponentType, ComponentConfig.ComponentName, ComponentConfig.RegistrationMode);
            registrar.set_ComponentExtraInfo(ComponentConfig.RegistrationMode, ComponentConfig.ProgID, "Icon", ComponentConfig.IconPath);
            registrar.set_ComponentExtraInfo(ComponentConfig.RegistrationMode, ComponentConfig.ProgID, "Tooltip", ComponentConfig.Tooltip);

            if (!ComponentConfig.ParadigmName.Equals("*"))
            {
                registrar.Associate(
                   ComponentConfig.ProgID,
                    ComponentConfig.ParadigmName,
                    ComponentConfig.RegistrationMode);
            }
        }

        /// <summary>
        /// Unregisters this component from the GME Registry.
        /// </summary>
        /// <exception cref="RegistrationException">If GME Interface version does not match.</exception>
        public static void UnregisterComponentsInGMERegistry()
        {
            MgaRegistrar registrar = new MgaRegistrar();
            CheckGMEInterfaceVersion(registrar);

            registrar.UnregisterComponent(ComponentConfig.ProgID, ComponentConfig.RegistrationMode);
        }

        /// <summary>
        /// Checks GME interface version
        /// </summary>
        /// <param name="registrar">Registrar to get the current version of GME, which is used to register the component.</param>
        /// <exception cref="RegistrationException">If GME Interface version does not match.</exception>
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
    }

    /// <summary>
    /// Represents errors that occur during component registration.
    /// </summary>
    [ComVisible(false)]
    internal class RegistrationException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationException"/> class with a specified
        /// error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RegistrationException(string message)
            : base(message)
        {
        }
    }
}