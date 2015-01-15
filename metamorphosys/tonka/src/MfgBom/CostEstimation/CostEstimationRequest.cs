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
    public class CostEstimationRequest
    {
        /// <summary>
        /// The number of units to be built.
        /// This is used to factor in favorable price breaks
        /// for ordering higher quantities of parts.
        /// </summary>
        public int design_quantity;

        /// <summary>
        /// An ordered list of suppliers. If a part is available
        /// from the first supplier, then the cost from that
        /// supplier will be used. If the part is unavailable, the
        /// cost from the second supplier will be used, and so on.
        /// If the part is not available from any of the suppliers
        /// in the affinity list, then the supplier with the lowest
        /// cost will be chosen.
        /// </summary>
        public List<String> supplier_affinity;

        /// <summary>
        /// The BOM to be used in the cost estimation.
        /// </summary>
        public Bom.MfgBom bom;

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

        public static CostEstimationRequest Deserialize(String json)
        {
            return JsonConvert.DeserializeObject<CostEstimationRequest>(json);
        }
    }
}
