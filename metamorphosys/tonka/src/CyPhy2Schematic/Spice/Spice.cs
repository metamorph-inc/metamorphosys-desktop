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
using System.IO;

namespace CyPhy2Schematic.Spice
{
    class Circuit
    {
        public string name { get; set; }
        public List<Node> nodes { get; set; }       // list of devices in the circuit
        public Dictionary<string, string> subcircuits { get; set; }
        public string analysis { get; set; }

        public Circuit()
        {
            nodes = new List<Node>();
            subcircuits = new Dictionary<string, string>();
        }

        public void Serialize(string spiceFile)
        {
            StreamWriter writer = new StreamWriter(spiceFile);
            writer.WriteLine("CyPhy2Schematic Circuit {0}", name);
            writer.WriteLine("* Network Topology");
            foreach (var node in nodes)
            {
                node.Serialize(writer);
            }
            writer.WriteLine();
            writer.WriteLine("* Sub Circuits");
            foreach (var sub in subcircuits)
            {
                writer.Write(sub.Value);
                writer.WriteLine();
            }
            writer.WriteLine();
            if (analysis != null)
            {
                writer.WriteLine(analysis);
            }
            writer.WriteLine();
            writer.WriteLine(".end");
            writer.Close();


            /// TBD Testbench aspects of circuit???
        }
    }

    class Node
    {
        public string name { get; set; }                // device instance name
        public char type { get; set; }                  // circuit device type: R, C, X, ...
        public string classType { get; set; }           // subcircuit class type
        public SortedDictionary<int, string> nets { get; set; } // nets sorted by an order parameter
        public SortedDictionary<string, string> parameters { get; set; }    // device parameters

        public Node()
        {
            nets = new SortedDictionary<int, string>();
            parameters = new SortedDictionary<string, string>();
        }

        public void Serialize(StreamWriter writer)
        {
            writer.Write("{0}{1} ", type, name);
            foreach (var net in nets)                   // ports are index-ordered
                writer.Write("{0} ", net.Value);
            if (classType != null) 
                writer.Write("{0} ", classType);            // sub-circuit name or model name
            foreach (var param in parameters)           // params are name-ordered
            {
                // value parameters are special - don't require a name= prefix
                if (param.Key.Contains("value"))
                    writer.Write("{0} ", param.Value);
                else
                    writer.Write("{0}={1} ", param.Key, param.Value);
            }
            writer.WriteLine();
        }
    }

}
