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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace MfgBom.Bom
{
    public class MfgBom
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MfgBom()
        {
            Parts = new List<Part>();
        }

        public string designName;  // For MOT-256

        public List<Part> Parts { get; private set; }

        public void AddPart(Part part)
        {
            /* See if we already have a part with this MPN.
             * If so, we'll add this instance data to the existing part.
             * If not, we'll add this as a new part.
             */

            if (part.octopart_mpn != null && 
                Parts.Any(p => p.octopart_mpn == part.octopart_mpn))
            {
                // Consolidate with the existing part

                var existingPart = Parts.First(p => p.octopart_mpn == part.octopart_mpn);
                foreach (var instance in part.instances_in_design)
                {
                    existingPart.AddInstance(instance);
                }
            }
            else
            {
                // Add as a new part
                Parts.Add(part);
            }
        }

        /// <summary>
        /// Get the current object as a JSON string.
        /// </summary>
        /// <returns>JSON-formatted string</returns>
        public String Serialize()
        {
            JsonSerializer serializer = new JsonSerializer()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            String rtn = null;
            using (StringWriter sw = new StringWriter())
            using (JsonTextWriter jtw = new JsonTextWriter(sw))
            {
                serializer.Serialize(jtw, this);
                rtn = sw.ToString();
            }

            return rtn;
        }

        public static MfgBom Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<MfgBom>(json);
        }

        /// <summary>
        /// Create and return a randomly-generated BOM data structure.
        /// </summary>
        /// <returns></returns>
        public static MfgBom CreateFakeBOM()
        {
            var bom = new MfgBom()
            {
                Parts = new List<Part>()
            };

            Random random = new Random();
            for (int i = 0; i <= 3; i++)
            {
                var part = new Part()
                {
                    octopart_mpn = Path.GetRandomFileName()
                };
                bom.Parts.Add(part);

                for (int j = 0; j <= 3; j++)
                {
                    var instance = new ComponentInstance()
                    {
                        gme_object_id = random.Next(100000).ToString(),
                        path = Path.GetRandomFileName()
                    };
                    part.AddInstance(instance);
                }
            }

            return bom;
        }
    }
}
