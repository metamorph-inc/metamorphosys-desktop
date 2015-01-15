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

using CyPhyGUIs;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Modelica_v2
{
    public static class FolderFilter
    {
        public static void PopulateWithFilteredNodes(ref FolderTreeNode folder)
        {
            var components = folder.Folder;
            foreach (var component in components.Children.ComponentCollection
                .Where(c => (c.Impl as GME.MGA.IMgaModel).GetChildrenOfKind(typeof(CyPhy.ModelicaModel).Name).Count +
                            (c.Impl as GME.MGA.IMgaModel).GetChildrenOfKind(typeof(CyPhy.CyberModel).Name).Count > 0))
            {
                folder.Children.Add(new ComponentTreeNode(component));
            }

            foreach (var subComponents in components.Children.ComponentsCollection.Where(f => FolderHasModelicaComponents(f)))
            {
                var subFolder = new FolderTreeNode(subComponents);
                folder.Children.Add(subFolder);
                PopulateWithFilteredNodes(ref subFolder);
            }
        }

        public static bool FolderHasModelicaComponents(CyPhy.Components folder)
        {
            foreach (var component in folder.Children.ComponentCollection)
            {
                if ((component.Impl as GME.MGA.IMgaModel).GetChildrenOfKind(typeof(CyPhy.ModelicaModel).Name).Count +
                    (component.Impl as GME.MGA.IMgaModel).GetChildrenOfKind(typeof(CyPhy.CyberModel).Name).Count > 0)
                {
                    return true;
                }
            }

            bool subFolderHasModelicaComponents = false;

            foreach (var subFolder in folder.Children.ComponentsCollection)
            {
                if (FolderHasModelicaComponents(subFolder))
                {
                    subFolderHasModelicaComponents = true;
                    break;
                }
            }

            return subFolderHasModelicaComponents;
        }
    }

    public class TreeNodeBase
    {
        public string Name;
    }

    public class ComponentTreeNode : TreeNodeBase
    {
        public CyPhy.Component Component { get; set; }

        public ComponentTreeNode(CyPhy.Component component)
        {
            this.Component = component;
            this.Name = component.Name;
        }
    }

    public class FolderTreeNode : TreeNodeBase
    {
        public CyPhy.Components Folder { get; set; }
        public List<TreeNodeBase> Children { get; set; }

        public FolderTreeNode(CyPhy.Components folder)
        {
            this.Folder = folder;
            this.Children = new List<TreeNodeBase>();
            this.Name = folder.Name;
        }
    }
}