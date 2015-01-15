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

namespace Complexity
{
    public partial class Design
    {
        /// <param name="fileName">The path to the output file</param>
        public string SerializeToFile(string fileName)
        {
            string jsonString = this.Serialize();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName)) { sw.WriteLine(jsonString); }
            return jsonString;
        }

        public string Serialize()
        {
            string jsonString = JsonConvert.SerializeObject(
                this,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    TypeNameHandling = TypeNameHandling.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    // TODO: ...
                }
            );

            // TODO: Catch exceptions for serialization function
            // TODO: Check for valid filename and path

            return jsonString;
        }

        public Dictionary<string,string> SerializeToCSVFormat()
        {
            Dictionary<string, string> d_rtn = new Dictionary<string, string>();

            Dictionary<ComponentInstance, int> d_oneIndexIDs = new Dictionary<ComponentInstance, int>();
            
            List<String> ls_compEntries = new List<String>();
            foreach (ComponentInstance ci in this.ComponentInstances)
            {
                int i_id = d_oneIndexIDs.Count + 1;
                d_oneIndexIDs[ci] = i_id;
                switch (ci.DistributionType)
                {
                    case DistributionTypeEnum.None:
                        ls_compEntries.Add(
                            //String.Format("{0},{1},{2}", ci.Name, d_oneIndexIDs[ci], ci.Complexity));
                            String.Format("{0},{1},{2}", ci.Name, d_oneIndexIDs[ci], ci.Complexity));
                        break;
                    default:
                        String s_entry = String.Empty;
                        //s_entry = String.Format("{0},{1},-1,{2}", ci.Name, d_oneIndexIDs[ci], ci.DistributionType.ToString());
                        s_entry = String.Format("{0},{1},-1,{2}", ci.Name, d_oneIndexIDs[ci], ci.DistributionType.ToString());
                        foreach (double d in ci.DistributionParameters)
                        {
                            s_entry += String.Format(",{0}", d);
                        }
                        ls_compEntries.Add(s_entry);
                        break;
                }
            }
            String csv_comp = "";
            foreach (String s_entry in ls_compEntries)
            {
                csv_comp += s_entry + "\r";
            }
            d_rtn["Components.csv"] = csv_comp;

            List<String> ls_connEntries = new List<String>();
            foreach (Connection c in this.Connections)
            {
                ls_connEntries.Add(
                    String.Format("{0},{1},{2}", d_oneIndexIDs[c.src], d_oneIndexIDs[c.dst], c.Complexity));
            }
            String csv_conn = "";
            foreach (String s_entry in ls_connEntries)
            {
                csv_conn += s_entry + "\r";
            }
            d_rtn["Connections.csv"] = csv_conn;

            return d_rtn;
        }
    }
}
