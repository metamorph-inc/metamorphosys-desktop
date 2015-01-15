﻿/*
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

   namespace CyPhyPrepareIFab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    using GME;
    using GME.CSharp;
    using GME.MGA;
    using GME.MGA.Core;
    using Common = ISIS.GME.Common;
    using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
    using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using META;

    public partial class CyPhyPrepareIFabInterpreter
    {

        private void ManufacturingGeneration(MgaFCO currentobj)
        {
            if (currentobj.MetaBase.Name == "TestBench")
            {
                // DDP Generation
                CyPhy.TestBench tb = CyPhyClasses.TestBench.Cast(currentobj);
                var catlsut = tb.Children.ComponentAssemblyCollection.FirstOrDefault();     // should be an instance b/c elaborate was called earlier
                if (catlsut == null)
                {
                    throw new Exception("There is no elaborated system under test component assembly in the model!");
                }

                var design = CyPhy2DesignInterchange.CyPhy2DesignInterchange.Convert(catlsut);
                this.TestBenchName = tb.Name;
                this.AssemblyName = design.Name;
                design.SaveToFile(Path.Combine(this.OutputDirectory, this.TestBenchName + ".adm"));

                if (catlsut.Attributes.ConfigurationUniqueID.Contains("{"))
                    this.ManufacturingManifestData.DesignID = catlsut.Attributes.ConfigurationUniqueID;
                else
                    this.ManufacturingManifestData.DesignID = "{" + catlsut.Attributes.ConfigurationUniqueID + "}";
                this.ManufacturingManifestData.Name = catlsut.Name;
                PartManufacturingGeneration(catlsut);
            }
            else if (currentobj.MetaBase.Name == "ComponentAssembly")
            {
                // DDP Generation
                CyPhy.ComponentAssembly assembly = CyPhyClasses.ComponentAssembly.Cast(currentobj);

                var design = CyPhy2DesignInterchange.CyPhy2DesignInterchange.Convert(assembly);
                this.AssemblyName = design.Name;
                this.TestBenchName = design.Name;
                design.SaveToFile(Path.Combine(this.OutputDirectory, this.TestBenchName + ".adm"));

                this.ManufacturingManifestData.DesignID = "{" + assembly.Attributes.ConfigurationUniqueID + "}";
                this.ManufacturingManifestData.Name = assembly.Name;
                PartManufacturingGeneration(assembly);
            }
            else
                throw new NotImplementedException();
        }


        private void PartManufacturingGeneration(CyPhy.ComponentAssembly componentasm)
        {
            // [1] Create ManufacturingData
            // [2] Regenerate manufacturing xml file for each ManufacturingData
            // [3] Generate manifest file

            TraverseComponentAssembly(componentasm);
            foreach (ComponentManufacturingData data in this.ComponentManufacturingDataList)
            {
                data.UpdateManufacturingSpec();
            }

            // manifest
            string reportJson = Newtonsoft.Json.JsonConvert.SerializeObject(
                this.ManufacturingManifestData,
                Newtonsoft.Json.Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(Path.Combine(this.OutputDirectory, "manufacturing.manifest.json")))
            {
                writer.WriteLine(reportJson);
            }

        }
        
        private void TraverseComponentAssembly(CyPhy.ComponentAssembly componentasm)
        {
            foreach (CyPhy.ComponentRef cref in componentasm.Children.ComponentRefCollection)
            {
                throw new Exception("Model not fully elaborated, contains ComponentRef [" + cref.Path +  "]");
            }

            foreach (CyPhy.ComponentAssembly cainst in componentasm.Children.ComponentAssemblyCollection)
            {
                TraverseComponentAssembly(cainst);
            }

            foreach (CyPhy.Component cint in componentasm.Children.ComponentCollection)
            {
                foreach (CyPhy.ManufacturingModel manModel in cint.Children.ManufacturingModelCollection)
                {
                    string manfilename;
                    if ( ! manModel.TryGetResourcePath(out manfilename) || 
                           String.IsNullOrWhiteSpace(manfilename))
                    {
                        GMEConsole.Warning.Write("ManufacturingModel's FileLocation does not have a valid file name [" + manModel.Path + "]");
                        continue;
                    }

                    string componentjsonpath = cint.GetDirectoryPath();

                    // for xml data
                    ComponentManufacturingData mfdata = new ComponentManufacturingData();
                    mfdata.AVMID = cint.Attributes.AVMID;
                    mfdata.RevID = "";
                    mfdata.VerID = cint.Attributes.Version;
                    mfdata.GUID = cint.Attributes.InstanceGUID;
                    mfdata.Name = cint.Name;
                    //mfdata.Location = Path.Combine(this.ProjectRootDirectory, fullfilepath, manModel.Attributes.Location);         // form path
                    mfdata.Location = Path.Combine(this.ProjectRootDirectory, componentjsonpath, manfilename);         // form path
                    mfdata.NewLocation = Path.Combine(this.OutputDirectory, "ManufacturingModels");

                    foreach (CyPhy.ManufacturingModelParameter param in manModel.Children.ManufacturingModelParameterCollection)
                    {
                        int srcCount = param.AllSrcConnections.Count();
                        if (srcCount > 1)
                            throw new Exception("ManufacturingModelParameter is connected to >1 value flow targets [" + param.Path + "]");

                        if (String.IsNullOrEmpty(param.Attributes.Value))
                        {
                            if (String.IsNullOrEmpty(param.Attributes.DefaultValue))
                                continue;
                            else
                                mfdata.ManufacturingParamters.Add(param.Name, param.Attributes.DefaultValue);
                        }
                        else
                            mfdata.ManufacturingParamters.Add(param.Name, param.Attributes.Value);
                    }

                    this.ComponentManufacturingDataList.Add(mfdata);

                    // for manifest data
                    DesignManufactureManifest.ComponentManufactureManifestData componentmanifestdata = new DesignManufactureManifest.ComponentManufactureManifestData(cint.Attributes.InstanceGUID, mfdata.NewLocation);
                    char[] charsToTrim = { '{', '}' };
                    string fileName = cint.Attributes.InstanceGUID.Trim(charsToTrim) + ".xml";
                    componentmanifestdata.ManufacturingModel = "ManufacturingModels/" + fileName;
                    this.ManufacturingManifestData.ComponentManufactureList.Add(componentmanifestdata);
                }
            }
        }
    }
}