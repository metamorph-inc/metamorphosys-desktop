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
using System.Text.RegularExpressions;

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Schematic.Schematic
{
    public class Component : ModelBase<Tonka.ComponentType>
    {
        public Component(Tonka.ComponentType impl)
            : base(impl)
        {
            Parameters = new SortedSet<Parameter>();
            Ports = new List<Port>();
            string iname = Regex.Replace(impl.Name, "[ ]", "_");
            string name = iname;
            int partCount = 1;
            if (CodeGenerator.partNames.ContainsKey(name))
            {
                partCount = CodeGenerator.partNames[name];
                name = String.Format("{0}${1}", name, partCount++);
            }
            CodeGenerator.partNames[iname] = partCount;
            this.Name = name;
        }
        public SortedSet<Parameter> Parameters { get; set; }
        public List<Port> Ports { get; set; }
        public ComponentAssembly Parent { get; set; }
        public Eagle.eagle SchematicLib { get; set; }
        public string SpiceLib { get; set; }
        public void accept(Visitor visitor)
        {
            visitor.visit(this);
            foreach (var port_obj in Ports)
            {
                port_obj.accept(visitor);
            }
            visitor.upVisit(this);
        }
    }
}
