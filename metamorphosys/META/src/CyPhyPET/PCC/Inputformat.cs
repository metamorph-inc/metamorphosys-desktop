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

using System.Collections.Generic;
using CyPhyGUIs;

namespace CyPhyPET.PCC
{

    //public class InputDistribution
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public List<string> TestBenchParameterNames { get; set; }
    //    public string Distribution { get; set; }
    //    public double Param1 { get; set; }
    //    public double Param2 { get; set; }
    //    public double Param3 { get; set; }
    //    public double Param4 { get; set; }

    //    public InputDistribution()
    //    {
    //        TestBenchParameterNames = new List<string>();
    //    }
    //}

    public class StochasticInputs
    {
        public List<PCCInputDistribution> InputDistributions { get; set; }

        public StochasticInputs()
        {
            InputDistributions = new List<PCCInputDistribution>();
        }
    }

    public class Limits
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public string op { get; set; }
        public string minrange { get; set; }
        public string maxrange { get; set; }
    }

    public class PCCMetric
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string TestBenchMetricName { get; set; }
        public double PCC_Calc { get; set; }
        public double PCC_Spec { get; set; }
        public Limits Limits { get; set; }

        public PCCMetric()
        {
            Limits = new Limits();
        }
    }

    public class PCCInputArguments
    {
        [System.Xml.Serialization.XmlArray]
        [System.Xml.Serialization.XmlArrayItem(ElementName = "OutputID")]
        public List<string> OutputIDs { get; set; }
        [System.Xml.Serialization.XmlArray]
        [System.Xml.Serialization.XmlArrayItem(ElementName = "InputID")]
        public List<string> InputIDs { get; set; }
        public StochasticInputs StochasticInputs { get; set; }
        public List<PCCMetric> PCCMetrics { get; set; }
        public List<int> Methods { get; set; }
    }

    public class Part
    {
        public string ModelConfigFileName { get; set; }
        public string ToolConfigFileName { get; set; }
    }

    public class Configuration
    {
        public string Feasible { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public List<Part> Parts { get; set; }
        public PCCInputArguments PCCInputArguments { get; set; }
    }

    public class Configurations
    {
        public Configuration Configuration { get; set; }
    }

    public class RootObject
    {
        public Configurations Configurations { get; set; }
    }
}