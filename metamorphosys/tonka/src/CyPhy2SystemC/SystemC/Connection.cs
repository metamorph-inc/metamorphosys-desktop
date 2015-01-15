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

namespace CyPhy2SystemC.SystemC
{
    public class Connection : IComparable<Connection>
    {
        private static Dictionary<string, Connection> connections;

        static Connection()
        {
            connections = new Dictionary<string, Connection>();
        }

        public Connection(Port srcPort, Port dstPort)
        {
            SrcPort = srcPort;
            DstPort = dstPort;
            Name = srcPort.Name;
            DataType = srcPort.DataType;
        }
        public Port SrcPort { get; set; }
        public Port DstPort { get; set; }
        public string DataType { get; set; }

        private string _name = null;

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                // Find a unique name by appending a sequence to the basename
                string baseName = value.Replace(' ', '_');
                string name = baseName;
                int seq = 2;
                while (connections.ContainsKey(name))
                {
                    name = baseName + (seq++);
                }
                this._name = name;
                connections.Add(name, this);
            }
        }

        public int CompareTo(Connection obj)
        {
            string me = string.Format("{0}-{2}->{1}", SrcPort.Name, DstPort.Name, obj.DataType);
            string other = string.Format("{0}-{2}->{1}", obj.SrcPort.Name, obj.DstPort.Name, obj.DataType);
            return me.CompareTo(other);
        }
       
    }

}
