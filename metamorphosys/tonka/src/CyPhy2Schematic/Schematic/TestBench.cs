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

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Schematic.Schematic
{
    public class TestBench : ModelBase<Tonka.TestBench>
    {
        public TestBench(Tonka.TestBench impl)
            : base(impl)
        {
            ComponentAssemblies = new SortedSet<ComponentAssembly>();
            TestComponents = new SortedSet<Component>();
            Parameters = new SortedSet<Parameter>();
            SolverParameters = new Dictionary<string, string>();
        }

        public SortedSet<ComponentAssembly> ComponentAssemblies { get; set; }
        public SortedSet<Component> TestComponents { get; set; }
        public SortedSet<Parameter> Parameters { get; set; }
        public Dictionary<string, string> SolverParameters { get; set; }
        public int CanvasXMax { get; set; }
        public int CanvasYMax { get; set; }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
            foreach (var testcomponent_obj in TestComponents)
            {
                testcomponent_obj.accept(visitor);
            }
            foreach (var componentassembly_obj in ComponentAssemblies)
            {
                componentassembly_obj.accept(visitor);
            }
        }

    }

}
