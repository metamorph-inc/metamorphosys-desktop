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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AVM;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace AVM
{
    public partial class Component
    {
        static JsonSerializerSettings jss_defaults = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameHandling = TypeNameHandling.Objects,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<Newtonsoft.Json.JsonConverter>()
            { 
                new Newtonsoft.Json.Converters.StringEnumConverter()
            }
        };

        /// <param name="fileName">The path to the output file</param>
        public string SerializeToFile(string fileName)
        {
            // TODO: Catch exceptions for serialization function
            // TODO: Check for valid filename and path
            
            string jsonString = this.Serialize();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName)) { sw.WriteLine(jsonString); }
            return jsonString;
        }

        public string Serialize()
        {
            this.DDPSpecVersion = "1.2.1";

            string json = JsonConvert.SerializeObject(
                this,
                Formatting.Indented,
                jss_defaults
            );

            Dictionary<String, String> d_OldIDToNew = new Dictionary<string, string>();
            json = ProcessJSON.CrawlJSONDictionary( json, 
                                        new ParseSettings(){b_FixIDs = true, operation = Operation.SERIALIZE, b_ManipulateMode = true},
                                        d_OldIDToNew);
            json = ProcessJSON.RedirectRefs(json, d_OldIDToNew);

            return json;
        }

        public static Component Deserialize(string json)
        {
            string json_ToImport = json;

            // Auto-migrate Structural Interfaces
            Dictionary<String, String> d_ToAutoMigrate = new Dictionary<string, string>()
            {
                { "\"AVM.META.StructuralInterface\"", "\"AVM.StructuralInterface\"" },
                { "\"AVM.META.StructuralInterfaceDatum\"", "\"AVM.StructuralInterfaceDatum\"" },
            };

            foreach (KeyValuePair<String, String> kvp in d_ToAutoMigrate)
                json_ToImport = json_ToImport.Replace(kvp.Key, kvp.Value);

            json_ToImport = ProcessJSON.CrawlJSONDictionary(json_ToImport, 
                                                new ParseSettings() { b_FixIDs = true, operation = Operation.DESERIALIZE, b_ManipulateMode = true });
            Component rtn = JsonConvert.DeserializeObject(json_ToImport, typeof(AVM.Component), jss_defaults) as AVM.Component;
            return rtn;
        }

        public static Component DeserializeFromFile(string path)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            String json = sr.ReadToEnd();
            sr.Close();

            return Deserialize(json);
        }
    }
}
