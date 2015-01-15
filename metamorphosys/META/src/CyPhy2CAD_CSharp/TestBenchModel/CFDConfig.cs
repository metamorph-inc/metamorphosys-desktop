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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;

namespace CyPhy2CAD_CSharp.TestBenchModel
{
    public class CFDConfig
    {
        public int Tier { get; set; }

        // META-2940
        // META-2941
        public double SimulationTime { get; set; }
        public int MeshFineness { get; set; }
        public string Core { get; set; }

        public enum MaterialType
        {
            Fresh,
            Salt,
            None
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public MaterialType FluidMaterial { get; set; }

        public void SerializeToJson(string outputDir)
        {
            string reportContent = Newtonsoft.Json.JsonConvert.SerializeObject(this,
                                                                               Newtonsoft.Json.Formatting.Indented);
            using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, "CFDConfig.json")))
            {
                writer.WriteLine(reportContent);
            }
        }

        public CFDConfig(CyPhy.CFDSolverSetting setting,
                         CyPhy.CFDTestBench testBench)
        {
            MeshFineness = (int)testBench.Attributes.MeshFineness + 1;
            SimulationTime = testBench.Attributes.SimulationTime;
            Core = testBench.Attributes.Core.ToString();
        }

        public CFDConfig()
        {
        }

        public class HydrostaticsSolverSettings : CFDConfig
        {
            public double FluidTemperature { get; set; }
            public double RollAngleStart { get; set; }
            public double RollAngleEnd { get; set; }
            public double RollAngleStep { get; set; }
            public double PitchAngleStart { get; set; }
            public double PitchAngleEnd { get; set; }
            public double PitchAngleStep { get; set; }
            public HydrostaticsSolverSettings(CyPhy.HydrostaticsSolverSettings settings,
                                              CyPhy.CFDTestBench testBench)
                : base(settings,
                       testBench)
            {
                FluidMaterial = (MaterialType)(int)settings.Attributes.FluidMaterial;
                FluidTemperature = settings.Attributes.FluidTemperature;
                RollAngleStart = settings.Attributes.RollAngleStart;
                RollAngleEnd = settings.Attributes.RollAngleEnd;
                RollAngleStep = settings.Attributes.RollAngleStepSize;
                PitchAngleStart = settings.Attributes.PitchAngleStart;
                PitchAngleEnd = settings.Attributes.PitchAngleEnd;
                PitchAngleStep = settings.Attributes.PitchAngleStepSize;
            }
        }


        public class CalmWaterSolverSettings : CFDConfig
        {
            public double VehicleVelocity { get; set; }
            public double FluidTemperature { get; set; }
            public CalmWaterSolverSettings(CyPhy.CalmWaterSolverSettings settings,
                                           CyPhy.CFDTestBench testBench)
                : base(settings,
                       testBench)
            {
                Tier = 3;
                FluidMaterial = (MaterialType)(int)settings.Attributes.FluidMaterial;
                FluidTemperature = settings.Attributes.FluidTemperature;
                VehicleVelocity = settings.Attributes.VehicleVelocity;
            }
        }

        public class WaveResistanceSolverSettings : CFDConfig
        {
            public double VehicleVelocity { get; set; }
            public double FluidTemperature { get; set; }
            public double WaveDirection { get; set; }
            public double WaveFrequency { get; set; }
            public double WaveNumber { get; set; }
            public double WaveHeight { get; set; }

            public WaveResistanceSolverSettings(CyPhy.WaveResistanceSolverSettings settings,
                                                CyPhy.CFDTestBench testBench)
                : base(settings,
                       testBench)
            {
                Tier = 5;
                FluidMaterial = (MaterialType)(int)settings.Attributes.FluidMaterial;
                FluidTemperature = settings.Attributes.FluidTemperature;
                WaveDirection = settings.Attributes.WaveDirection;
                WaveFrequency = settings.Attributes.WaveFrequency;
                WaveNumber = settings.Attributes.WaveNumber;
                WaveHeight = settings.Attributes.WaveHeight;
                VehicleVelocity = settings.Attributes.VehicleVelocity;
            }

        }

        public class CorrelationSolverSettings : CFDConfig
        {
            public double VehicleVelocity { get; set; }
            public CorrelationSolverSettings(CyPhy.CorrelationSettings settings,
                                             CyPhy.CFDTestBench testBench)
                : base(settings,
                       testBench)
            {
                Tier = 1;
                FluidMaterial = MaterialType.None;
                VehicleVelocity = settings.Attributes.VehicleVelocity;
            }
        }
    }
}
