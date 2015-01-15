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
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Modelica_v2.Modelica
{
    public class ComponentInfo
    {
        public ComponentInfo(GME.MGA.IMgaFCO gmeObject, string modelType = null, CyPhyCOMInterfaces.IMgaTraceability traceability = null)
        {
            this.Name = gmeObject.Name;
            this.AbsPath = gmeObject.AbsPath;
            this.ID = gmeObject.ID;
            this.Path = this.GetPath();
            this.Kind = gmeObject.Meta.Name;
            this.ModelType = modelType;
            if (this.Kind == typeof(CyPhy.Component).Name)
            {
                var component = CyPhyClasses.Component.Cast(gmeObject);
                this.AVMID = component.Attributes.AVMID;
            }

            if (traceability != null)
            {
                string id = string.Empty;
                if (traceability.TryGetMappedObject(gmeObject.ID, out id))
                {
                    GME.MGA.MgaFCO baseComponent = gmeObject.Project.GetFCOByID(id);
                    if (baseComponent != null)
                    {
                        if (baseComponent.Status == 1)
                        {
                            // object is already deleted
                            // try to get it in a different way.
                            var idChain = gmeObject.RegistryValue["Elaborator/ID_Chain"];
                            if (string.IsNullOrWhiteSpace(idChain) == false)
                            {
                                id = idChain.Split(new char[] { ',' }).LastOrDefault();
                                if (id != null)
                                {
                                    baseComponent = gmeObject.Project.GetFCOByID(id);
                                    if (baseComponent != null)
                                    {
                                        this.Reference = new ComponentInfo(baseComponent, modelType);
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.Reference = new ComponentInfo(baseComponent, modelType);
                        }
                    }
                }
            }
        }

        public string Name { get; set; }
        public string AbsPath { get; set; }
        public string ID { get; set; }
        public string Path { get; set; }
        public string Kind { get; set; }
        public string AVMID { get; set; }
        public string ModelType { get; set; }

        public ComponentInfo Reference { get; set; }

        private string GetPath()
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var item in this.AbsPath.Split(new string[] { "/@" }, StringSplitOptions.None))
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                if (item.IndexOf('|') > 0)
                {
                    sb.Append("/@" + item.Substring(0, item.IndexOf('|')));
                }
                else
                {
                    sb.Append("/@" + item);
                }
            }

            return sb.ToString();
        }
    }
}
