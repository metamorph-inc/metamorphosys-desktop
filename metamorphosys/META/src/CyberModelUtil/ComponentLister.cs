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
using Cyber = ISIS.GME.Dsml.CyberComposition.Interfaces;
using CyberClasses = ISIS.GME.Dsml.CyberComposition.Classes;

namespace CyberModelUtil
{
    public class ComponentLister
    {
        public static HashSet<Cyber.ModelicaComponent> getCyberComponentSet(Cyber.RootFolder CyberRootFolder)
        {

            HashSet<Cyber.ModelicaComponent> CyberComponentSet = new HashSet<Cyber.ModelicaComponent>();

            foreach (Cyber.LibraryFolder childLibraryFolder in CyberRootFolder.Children.LibraryFolderCollection)
            {
                CyberComponentSet.UnionWith(getCyberComponentSet(childLibraryFolder));
            }

            foreach (Cyber.Components childComponentFolder in CyberRootFolder.Children.ComponentsCollection)
            {
                CyberComponentSet.UnionWith(getCyberComponentSet(childComponentFolder));
            }

            return CyberComponentSet;

        }

        public static HashSet<Cyber.ModelicaComponent> getCyberComponentSet(Cyber.LibraryFolder CyberLibraryFolder)
        {

            HashSet<Cyber.ModelicaComponent> CyberComponentSet = new HashSet<Cyber.ModelicaComponent>();

            // A LibraryFolder has self-containment relationship
            foreach (Cyber.LibraryFolder libFolder in CyberLibraryFolder.Children.LibraryFolderCollection)
            {
                CyberComponentSet.UnionWith(getCyberComponentSet(libFolder));
            }

            foreach (Cyber.ModelicaComponent component in CyberLibraryFolder.Children.ModelicaComponentCollection)
            {
                CyberComponentSet.Add(component);
            }

            return CyberComponentSet;
        }

        public static HashSet<Cyber.ModelicaComponent> getCyberComponentSet(Cyber.Components CyberComponents)
        {

            HashSet<Cyber.ModelicaComponent> CyberComponentSet = new HashSet<Cyber.ModelicaComponent>();

            foreach (Cyber.ModelicaComponent childComponent in CyberComponents.Children.ModelicaComponentCollection)
            {
                CyberComponentSet.Add(childComponent);
            }

            return CyberComponentSet;
        }

        public static HashSet<Cyber.ModelicaComponent> getCyberComponentSet(Cyber.ModelicaComponent CyberComponent)
        {
            HashSet<Cyber.ModelicaComponent> CyberComponentSet = new HashSet<Cyber.ModelicaComponent>();
            CyberComponentSet.Add(CyberComponent);
            return CyberComponentSet;
        }

        public static HashSet<Cyber.ModelicaComponent> getCyberComponentSet(IMgaObject iMgaObject)
        {
            //GME.CSharp.GMEConsole console = GME.CSharp.GMEConsole.CreateFromProject(iMgaObject.Project);
            //console.Info.WriteLine("getCyberComponentSet: iMgaObject Overload!");

            if (iMgaObject == null) return new HashSet<Cyber.ModelicaComponent>();

            string metaName = iMgaObject.MetaBase.Name;

            HashSet<Cyber.ModelicaComponent> CyberComponentSet = null;
            if (metaName == "Components")
            {

                CyberComponentSet = getCyberComponentSet(CyberClasses.Components.Cast(iMgaObject));

            }
            else if (metaName == "LibraryFolder")
            {

                CyberComponentSet = getCyberComponentSet(CyberClasses.LibraryFolder.Cast(iMgaObject));

            }
            else if (metaName == "SimulinkWrapper" || metaName == "SignalFlowWrapper")
            {

                //console.Info.WriteLine("getCyberComponentSet: A ModelicaComponent! ");
                CyberComponentSet = getCyberComponentSet(CyberClasses.ModelicaComponent.Cast(iMgaObject));

            }
            else if (metaName == "RootFolder")
            {

                CyberComponentSet = getCyberComponentSet(CyberClasses.RootFolder.GetRootFolder(iMgaObject.Project));
            }

            return CyberComponentSet;
        }
    }
}
