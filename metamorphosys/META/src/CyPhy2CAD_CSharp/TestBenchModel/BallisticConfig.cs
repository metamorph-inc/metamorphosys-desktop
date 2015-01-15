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
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CyPhy2CAD_CSharp.TestBenchModel
{
    namespace Survivability
    {
        public class BallisticConfig : SurvivabilityConfigBase
        {
            public class Analysis
            {
                public string suiteName { get; set; }
                public string ID { get; set; }
                public int tier { get; set; }
            }

            public class CriticalComponent
            {
                public enum CriticalityTypeEnum
                {
                    Crew,
                    Mobility,
                    Magazine
                }

                public string componentID { get; set; }

                [JsonConverter(typeof(StringEnumConverter))]
                public CriticalityTypeEnum type { get; set; }
            }

            public class BallisticThreat
            {
                public string name { get; set; }
                public string materialRef { get; set; }
                public double speed_metersPerSec { get; set; }
                public double length_meters { get; set; }
                public double diameter_meters { get; set; }

                public enum BallisticThreatTypeEnum
                {
                    ShapedChargeJet,
                    Ballistic
                }

                public enum ChargeQualityEnum
                {
                    Low,
                    Medium,
                    High
                }

                [JsonConverter(typeof(StringEnumConverter))]
                public BallisticThreatTypeEnum type { get; set; }

                [JsonConverter(typeof(StringEnumConverter))]
                public ChargeQualityEnum chargeQuality { get; set; }
                public double standoff_meters { get; set; }
            };

            public bool ShouldSerializeballisticThreats()
            {
                // don't serialize the ballisticThreats property if its count == 0
                return (ballisticThreats.Count > 0);
            }


            // members
            public Analysis analysis { get; set; }
            //public List<SurvivabilityConfig.FileLocation> fileLocations { get; set; }
            public List<CriticalComponent> criticalComponents { get; set; }
            public List<BallisticThreat> ballisticThreats { get; set; }


            // constructor
            public BallisticConfig() :
                   base()
            {
                analysis = new Analysis();
                //fileLocations = new List<SurvivabilityConfig.FileLocation>();
                criticalComponents = new List<CriticalComponent>();
                ballisticThreats = new List<BallisticThreat>();
            }

            /*
            public void SerializeToJson(string outputDir)
            {
                string reportContent = Newtonsoft.Json.JsonConvert.SerializeObject(this, 
                                                                                   Newtonsoft.Json.Formatting.Indented);
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, "BallisticConfig.json")))
                {
                    writer.WriteLine(reportContent);
                }
            }
            */
        }

    }   // end namespace
}   // end namespace
