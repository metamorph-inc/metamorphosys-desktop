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

namespace MfgBom.CostEstimation
{
    public class CostEstimationResult
    {
        /// <summary>
        /// The original request data structure.
        /// </summary>
        public CostEstimationRequest request;

        /// <summary>
        /// A modified copy of the original BOM, with
        /// request results populated in the Part objects.
        /// </summary>
        public Bom.MfgBom result_bom;

        /// <summary>
        /// The parts cost per unit built.
        /// </summary>
        public float per_design_parts_cost;
        
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

        public static CostEstimationResult Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<CostEstimationResult>(json);
        }
    }
}
