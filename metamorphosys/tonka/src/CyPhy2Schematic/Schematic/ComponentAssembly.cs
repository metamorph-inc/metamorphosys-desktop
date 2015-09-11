using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Schematic.Schematic
{
    public class ComponentAssembly : ModelBase<Tonka.ComponentAssembly>
    {
        public ComponentAssembly(Tonka.ComponentAssembly impl)
            : base(impl)
        {
            ComponentAssemblyInstances = new SortedSet<ComponentAssembly>();
            ComponentInstances = new SortedSet<Component>();
            Parameters = new SortedSet<Parameter>();
        }
        public SortedSet<ComponentAssembly> ComponentAssemblyInstances { get; set; }
        public SortedSet<Component> ComponentInstances { get; set; }
        public SortedSet<Parameter> Parameters { get; set; }
        public ComponentAssembly Parent { get; set; }

        public int CanvasXMax { get; set; }
        public int CanvasYMax { get; set; }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
            foreach (var componentassembly_obj in this.ComponentAssemblyInstances)
            {
                componentassembly_obj.accept(visitor);
            }
            foreach (var component_obj in this.ComponentInstances)
            {
                component_obj.accept(visitor);
            }
            visitor.upVisit(this);
        }

    }

}
