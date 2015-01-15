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

namespace CADManifestLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public static class CADManifestLib
    {

        public static void CreateCADManifest(string projectJsonPath)
        {
            // [1] Parse project json file
            // [2] Go through Component list
            // [3] Parse each ComponentData.component.json
           
            if (!File.Exists(projectJsonPath))
                return;

            string projectRoot = Path.GetDirectoryName(projectJsonPath);
            AVM.DDP.MetaAvmProject avmProj = new AVM.DDP.MetaAvmProject();

            if (File.Exists(projectJsonPath))
            {
                string sjson = "{}";
                using (StreamReader reader = new StreamReader(projectJsonPath))
                {
                    sjson = reader.ReadToEnd();
                    avmProj = JsonConvert.DeserializeObject<AVM.DDP.MetaAvmProject>(sjson);
                }
            }

            List<string> cadFileDirs = FindComponentCreoFiles(avmProj.Project.Components, projectRoot);
            if (cadFileDirs.Any())
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(projectRoot, "cadmanifest.txt")))
                {
                    foreach (string dir in cadFileDirs)
                    {
                        writer.WriteLine(dir);
                    }
                }
            }
        }

        public static List<string> FindComponentCreoFiles(List<AVM.DDP.MetaAvmProject.Component> avmComponents,
                                                          string projectRoot)
        {
            List<string> cadFileDirs = new List<string>();
            foreach (AVM.DDP.MetaAvmProject.Component component in avmComponents)
            {
                string cadModelPath = component.modelpath.TrimStart('/');      // Path.GetDirectoryName(component.modelpath);
                string componentModelPath = Path.Combine(projectRoot, cadModelPath);
                if (Path.GetFileName(cadModelPath).Contains(".json"))
                    cadModelPath = Path.GetDirectoryName(cadModelPath);

                if (File.Exists(componentModelPath))
                {
                    AVM.Component acimport = AVM.Component.DeserializeFromFile(componentModelPath);
                    foreach (AVM.Feature feature in acimport.Features.Where(cm => cm is AVM.META.CADModel))
                    {
                        AVM.META.CADModel acimport_cadModel = (AVM.META.CADModel)feature;
                        if (String.IsNullOrEmpty(acimport_cadModel.FileFormat))
                        {
                            cadFileDirs.Add(Path.Combine(cadModelPath, acimport_cadModel.Location));
                            break;
                        }
                    }
                }
            }

            return cadFileDirs;
        }
    }
}
