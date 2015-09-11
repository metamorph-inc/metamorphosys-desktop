using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Schematic.Schematic
{
    public class Port : ModelBase<Tonka.SchematicModelPort>
    {
        public Port(Tonka.SchematicModelPort impl)
            : base(impl)
        {
            SrcConnections = new List<Connection>();
            DstConnections = new List<Connection>();
        }

        public Component Parent { get; set; }
        public List<Connection> SrcConnections { get; set; }
        public List<Connection> DstConnections { get; set; }

        public void accept(Visitor visitor)
        {
            visitor.visit(this);
        }
    }
}
