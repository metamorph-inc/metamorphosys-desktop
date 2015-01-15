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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using GME.MGA;
using System.IO;

namespace META
{
    public static class ComponentLibraryManagerExtensions
    {
        /// <summary>
        /// Given a component, ensures that the component has a backend folder
        /// for storing resources. Ensures that the Component has an AVMID unique
        /// to the project. Creates a folder if necessary.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="pathConvention">The desired convention for the path.</param>
        /// <param name="ProjectDirectory">Directory in which component files reside. Defaults to project directory of <paramref name="component"/></param>
        /// <returns>The path of the Component folder, according to the convention provided</returns>
        public static String GetDirectoryPath(this CyPhy.Component component, ComponentLibraryManager.PathConvention pathConvention = ComponentLibraryManager.PathConvention.REL_TO_PROJ_ROOT, string ProjectDirectory = null)
        {
            return ComponentLibraryManager.GetComponentFolderPath(component, pathConvention, ProjectDirectory: ProjectDirectory);
        }
        /// <summary>
        /// Given a component, ensures that the component has a backend folder
        /// for storing resources. Ensures that the Component has an AVMID unique
        /// to the project. Creates a folder if necessary.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="pathConvention">The desired convention for the path.</param>
        /// <param name="ProjectDirectory">Directory in which component files reside. Defaults to project directory of <paramref name="component"/></param>
        /// <returns>The path of the Component folder, according to the convention provided</returns>
        public static String GetDirectoryPath(this CyPhy.ComponentAssembly assembly, ComponentLibraryManager.PathConvention pathConvention = ComponentLibraryManager.PathConvention.REL_TO_PROJ_ROOT, string ProjectDirectory = null)
        {
            var relPath = ComponentLibraryManager.EnsureComponentAssemblyFolder(assembly, ProjectDirectory);
            switch (pathConvention)
            {
                case ComponentLibraryManager.PathConvention.REL_TO_PROJ_ROOT:
                    return relPath;
                case ComponentLibraryManager.PathConvention.ABSOLUTE:
                    return Path.Combine(GetRootDirectoryPath(assembly.Impl.Project), relPath);
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Path convention of {0} is not supported", 
                                                                        pathConvention.ToString()));
            }
        }

        /// <summary>
        /// Given a DomainModel object, find the path to the file resource.
        /// This will traverse the UsesResource connection to a Resource object,
        /// then return that Resource object's path.
        /// </summary>
        /// <param name="path">The path to the resource, relative to the component's resource folder</param>
        /// <param name="pathConvention"></param>
        /// <returns>True if the resource was found</returns>
        public static bool TryGetResourcePath(this CyPhy.DomainModel domainModel, out string path, ComponentLibraryManager.PathConvention pathConvention = ComponentLibraryManager.PathConvention.REL_TO_COMP_DIR)
        {
            return ComponentLibraryManager.TryGetResourcePath(domainModel, out path, pathConvention);
        }

        /// <summary>
        /// Given a CyPhy MGA project, return the root directory of the project
        /// </summary>
        /// <param name="project"></param>
        /// <returns>The absolute path to the project's root directory</returns>
        public static String GetRootDirectoryPath(this IMgaProject project)
        {
            return ComponentLibraryManager.GetProjectRootPath(project);
        }

    }
}
