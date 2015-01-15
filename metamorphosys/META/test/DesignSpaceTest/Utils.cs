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
using GME.CSharp;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace DesignSpaceTest
{
    internal static class Utils
    {
        public static void RunUdmInterpreter(MgaFCO currentobj, String ProgID)
        {
            // create interpreter type
            Type typeUdmInterpreter = Type.GetTypeFromProgID(ProgID);
            IMgaComponentEx udmInterpreter = Activator.CreateInstance(typeUdmInterpreter) as IMgaComponentEx;

            // empty selected object set
            Type typeMgaFCOs = Type.GetTypeFromProgID("Mga.MgaFCOs");
            MgaFCOs selectedObjs = Activator.CreateInstance(typeMgaFCOs) as MgaFCOs;

            // initialize interpreter
            udmInterpreter.Initialize(currentobj.Project);

            // automation means no UI element shall be shown by the interpreter
            udmInterpreter.ComponentParameter["automation"] = "true";

            // do not write to the console
            udmInterpreter.ComponentParameter["console_messages"] = "off";

            // do not expand nor collapse the model
            udmInterpreter.ComponentParameter["expanded"] = "true";

            // call the formula evaluator and update all parameters starting from the current object
            udmInterpreter.InvokeEx(currentobj.Project, currentobj, selectedObjs, 16);
        }

        public static void RunDSRefactorer(MgaFCO currentobj, List<MgaFCO> selected, String refactoredType)
        {
            // create interpreter type
            Type typeInterpreter = Type.GetTypeFromProgID("MGA.Interpreter.CyphyDesignSpaceRefactor");
            IMgaComponentEx dsRefactorInterpreter = Activator.CreateInstance(typeInterpreter) as IMgaComponentEx;

            // empty selected object set
            Type typeMgaFCOs = Type.GetTypeFromProgID("Mga.MgaFCOs");
            MgaFCOs selectedObjs = Activator.CreateInstance(typeMgaFCOs) as MgaFCOs;
            foreach (MgaFCO fco in selected)
            {
                selectedObjs.Append(fco);
            }

            // initialize interpreter
            dsRefactorInterpreter.Initialize(currentobj.Project);

            // automation means no UI element shall be shown by the interpreter
            dsRefactorInterpreter.ComponentParameter["automation"] = "true";
            dsRefactorInterpreter.ComponentParameter["refactored_type_or_action"] = refactoredType;

            // do not write to the console
            dsRefactorInterpreter.ComponentParameter["console_messages"] = "off";

            // do not expand nor collapse the model
            dsRefactorInterpreter.ComponentParameter["expanded"] = "true";

            // call the formula evaluator and update all parameters starting from the current object
            dsRefactorInterpreter.InvokeEx(currentobj.Project, currentobj, selectedObjs, 16);
        }

        public static void PerformInTransaction(this MgaProject project, MgaGateway.voidDelegate del)
        {
            var mgaGateway = new MgaGateway(project);
            project.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(del);
        }

        public static IEnumerable<CyPhy.Component> GetComponentDefinitionsByName(this MgaProject project, String name)
        {
            MgaFilter filter = project.CreateFilter();
            filter.Kind = "Component";
            filter.Name = name;

            return project.AllFCOs(filter)
                          .Cast<MgaFCO>()
                          .Select(x => CyPhyClasses.Component.Cast(x))
                          .Where(c => c.ParentContainer.Kind == "Components");
        }

        public static IEnumerable<CyPhy.ComponentAssembly> GetComponentAssembliesByName(this MgaProject project, String name)
        {
            MgaFilter filter = project.CreateFilter();
            filter.Kind = "ComponentAssembly";
            filter.Name = name;

            return project.AllFCOs(filter)
                          .Cast<MgaFCO>()
                          .Select(x => CyPhyClasses.ComponentAssembly.Cast(x));
        }

        public static IEnumerable<CyPhy.DesignContainer> GetDesignContainersByName(this MgaProject project, String name)
        {
            MgaFilter filter = project.CreateFilter();
            filter.Kind = "DesignContainer";
            filter.Name = name;

            return project.AllFCOs(filter)
                          .Cast<MgaFCO>()
                          .Select(x => CyPhyClasses.DesignContainer.Cast(x));
        }
    }
}
