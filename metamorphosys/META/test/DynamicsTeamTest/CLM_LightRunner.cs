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
using Xunit;
using System.IO;
using GME.MGA;

namespace DynamicsTeamTest
{
    public static class CLM_LightRunner
    {
        //public List<IMgaFCO> InsertComponents(
        //    IMgaFCO designContainer,
        //    IMgaFCO componentRef,
        //    List<IMgaFCO> components,
        //    List<KeyValuePair<IMgaFCO, string>> messages)

        /// <summary>
        /// Calls CLM_LIght without showing any GUI.
        /// </summary>
        /// <param name="outputdirname">xme folder from trunk/models/DynamicsTeam</param>
        /// <param name="projectPath">name of mga-file</param>
        /// <param name="absPath">Folder-path to test-bench</param>
        /// <returns>Boolean - True -> interpreter call was successful</returns>
        public static bool Run(
            string projectPath, 
            string absPathDesignContainer, 
            List<string> absPathComponentsToAdd)
        {
            bool result = false;

            Assert.True(File.Exists(projectPath), "Project file does not exist.");
            string ProjectConnStr = "MGA=" + projectPath;

            //Type CLM_light_interpreter = Type.GetTypeFromProgID("MGA.Interpreter.CLM_light");

            MgaProject project = new MgaProject();
            project.OpenEx(ProjectConnStr, "CyPhyML", null);
            try
            {
                //dynamic interpreter = Activator.CreateInstance(CLM_light_interpreter);
                var interpreter = new CLM_light.CLM_lightInterpreter();
                interpreter.Initialize(project);

                var terr = project.BeginTransactionInNewTerr();
                var designContainer = project.ObjectByPath[absPathDesignContainer] as MgaFCO;

                List<IMgaFCO> componentsToAdd = new List<IMgaFCO>();

                var defaultComponentRef = designContainer.ChildObjects.Cast<MgaFCO>().FirstOrDefault(x => x.Meta.Name == "ComponentRef");
                var compRefCountBeforeInsert = designContainer.ChildObjects.Cast<MgaFCO>().Count(x => x.Meta.Name == "ComponentRef");

                Assert.False(defaultComponentRef == null, string.Format("Design Container has no Component References: {0}", absPathDesignContainer));

                foreach (string path in absPathComponentsToAdd)
                {
                    var component = project.ObjectByPath[path] as MgaFCO;
                    Assert.False(component == null, string.Format("Component was not found in the project: {0}", path));
                    componentsToAdd.Add(component);
                }

                List<KeyValuePair<IMgaFCO, string>> messages = new List<KeyValuePair<IMgaFCO, string>>();

                if (interpreter.CheckForValidContext(designContainer))
                {
                    List<IMgaFCO> results = interpreter.InsertComponents(designContainer, defaultComponentRef, componentsToAdd, messages);

                    var compRefCountAfterInsert = designContainer.ChildObjects.Cast<MgaFCO>().Count(x => x.Meta.Name == "ComponentRef");

                    result = compRefCountBeforeInsert + absPathComponentsToAdd.Count == compRefCountAfterInsert;

                    project.AbortTransaction();

                    Assert.True(File.Exists(ProjectConnStr.Substring("MGA=".Length)));
                }
            }
            finally
            {
                project.Close(true);
            }

            return result;
        }
    }
}
