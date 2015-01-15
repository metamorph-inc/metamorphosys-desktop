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
    using System.Reflection;
    using System.Xml.Serialization;
    using System.Text.RegularExpressions;
    using System.Diagnostics;

    public class ComponentManufacturingData
    {
        public string Name  { get; set; }
        public string AVMID { get; set; }
        public string RevID { get; set; }
        public string VerID { get; set; }
        public string GUID  { get; set; }
        public string FileFormat { get; set; }
        public string Location   { get; set; }
        public string ID         { get; set; }
        public string NewLocation { get; set; }

        public Dictionary<string, string> ManufacturingParamters { get; set; }

        public ComponentManufacturingData()
        {
            ManufacturingParamters = new Dictionary<string,string>();
        }

        public void UpdateManufacturingSpec()
        {
            // [1] check xml exists
            // [2] deserialize xml file to a manufacturingDetail object
            // [3] update manufacturingDetail object with ManufacturingParamters objects
            // [4] serialize manufacturingDetail object
            char[] charsToTrim = {'{', '}'};
            string fileName = this.GUID.Trim(charsToTrim) + ".xml";
            if (!File.Exists(Location))
            {
                Trace.TraceError("Error: " + Location + " not found!");
                return;
            }

            if (!Directory.Exists(this.NewLocation))
                Directory.CreateDirectory(this.NewLocation);

            if (ManufacturingParamters.Count() > 0)             // update xml
            {
                XmlSerializer mfgDetails_Serializer = new XmlSerializer(typeof(Part.manufacturingDetails));
                Part.manufacturingDetails mfdetail = Part.manufacturingDetails.Deserialize(Location);
                StringWriter sw = new StringWriter();
                sw.NewLine = "";
                mfgDetails_Serializer.Serialize(sw, mfdetail);
                string xmldatastring = sw.ToString();

                try
                {
                    foreach (KeyValuePair<string, string> item in ManufacturingParamters)
                    {
                        xmldatastring = ReplaceParameters(item.Key, item.Value, xmldatastring);
                    }

                    var strreader = new StringReader(xmldatastring);
                    Part.manufacturingDetails mfdetails_revised = (Part.manufacturingDetails)mfgDetails_Serializer.Deserialize(strreader);

                    if (!Directory.Exists(this.NewLocation))
                        Directory.CreateDirectory(this.NewLocation);
                    StreamWriter myWriter = new StreamWriter(Path.Combine(this.NewLocation, fileName));
                    mfgDetails_Serializer.Serialize(myWriter, mfdetails_revised);


                    Trace.TraceInformation("Info: Updated manufacturing xml with manufacturing parameter from model. " + this.NewLocation + "\\" + fileName);
                }
                catch (System.InvalidOperationException ex)
                {
                    Console.WriteLine("Exception occured: {0}", ex.ToString());
                }

            }
            else                                   // copy the xml
            {
                File.Copy(Location, Path.Combine(this.NewLocation, fileName), true);
                Trace.TraceInformation("Info: Copied manufacturing xml. " + this.NewLocation + "\\" + fileName);
            }
        }

        private string ReplaceParameters(string parameter, string value, string xmlstring)
        {
            Trace.TraceInformation("Info: RegexMatch for [" + parameter + "]");
            string pattern = @">[0-9,a-z,A-Z.-_]*</\b(" + parameter + @")\b\>";
            string replacement = ">" + value + "</" + parameter + ">";
            Regex rgx = new Regex(pattern);
            string modified = rgx.Replace(xmlstring, replacement);

            MatchCollection mc = rgx.Matches(xmlstring);
            foreach (Match m in mc)
            {
                Trace.TraceInformation("Info: found = " + m.Value);
            }

            return modified;
        }
    }



    public class DesignManufactureManifest
    {
        public class ComponentManufactureManifestData
        {
            public string id { get; set; }
            public string ManufacturingModel { get; set; }

            public ComponentManufactureManifestData(string guid, 
                                                    string file)
            {
                id = guid;
                ManufacturingModel = file;
            }
        }

        public string DesignID { get; set; }
        public string Name { get; set; }
        public List<ComponentManufactureManifestData> ComponentManufactureList;

        public DesignManufactureManifest()
        {
            ComponentManufactureList = new List<ComponentManufactureManifestData>();
        }
    }

}   // end namespace
