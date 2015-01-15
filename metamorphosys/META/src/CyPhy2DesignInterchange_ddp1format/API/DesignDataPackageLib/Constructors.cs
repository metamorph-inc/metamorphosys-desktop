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
using System.Threading.Tasks;

namespace AVM
{
    public partial class Component
    {
        public Component()
        {
            this.Features = new List<AVM.Feature>();
            this.Associations = new List<AVM.Association>();
            this.Files = new List<AVM.File>();
            this.Category = new List<String>();
        }
    }
    public partial class StructuralInterface
    {
        public StructuralInterface()
        {
            this.Datums = new List<StructuralInterfaceDatum>();
            this.DefaultJoins = new List<iFAB.Join>();
            this.id = Guid.NewGuid().ToString();
        }
    }
    public partial class Associable
    {
        public Associable()
        {
            this.id = Guid.NewGuid().ToString();
        }
    }

    namespace iFAB
    {
    }

    namespace META
    {
        public partial class BehaviorModel
        {
            public BehaviorModel()
            {
                this.Interfaces = new List<Interface>();
                this.MaterialSpecs = new List<MaterialSpec>();
                this.LimitChecks = new List<LimitCheck>();
                this.Phenomena = new List<Phenomenon>();
            }
        }
        public partial class CADModel
        {
            public CADModel()
            {
                this.Datums = new List<Datum>();
                this.CADParameters = new List<CADParameter>();
                this.Metrics = new List<Metric>();
            }
        }
        public partial class AggregatePort
        {
            public AggregatePort()
            {
                this.AggregatedPorts = new List<ExternalPort>();
            }
        }
        public partial class AggregateInterface
        {
            public AggregateInterface()
            {
                this.AggregatedInterfaces = new List<AVM.META.Interface>();
            }
        }
        public partial class ArchitectureModel
        {
            public ArchitectureModel()
            {
                this.Properties = new List<Property>();
            }
        }

        namespace Design
        {   
            public partial class DesignModel
            {
                public DesignModel()
                {
                    this.Containers = new List<Container>();
                    this.Connectors = new List<Connector>();                    
                }
            }
            public abstract partial class Container
            {
                public Container()
                {
                    this.ComponentInstances = new List<ComponentInstance>();
                    this.Containers = new List<Container>();
                    this.ContainerValues = new List<ContainerValue>();
                    this.ContainerStructuralInterfaces = new List<ContainerStructuralInterface>();
                    this.id = Guid.NewGuid().ToString();
                }
            }
            public partial class ComponentInstance
            {
                public ComponentInstance()
                {
                    this.NamedValueInstances = new List<ComponentNamedValueInstance>();
                    this.PortInstances = new List<PortInstance>();
                    this.StructuralInterfaceInstances = new List<StructuralInterfaceInstance>();
                    this.id = Guid.NewGuid().ToString();
                }
            }
            public partial class AbstractPort
            {
                public AbstractPort()
                {
                    this.id = Guid.NewGuid().ToString();
                }
            }
            public partial class AbstractStructuralInterface
            {
                public AbstractStructuralInterface()
                {
                    this.id = Guid.NewGuid().ToString();
                }
            }
            public partial class ValueType
            {
                public ValueType()
                {
                    this.id = Guid.NewGuid().ToString();
                }
            }
            public partial class PortConnector
            {
                public PortConnector()
                {
                    this.EndPoints = new List<AbstractPort>();
                }
            }
            public partial class StructuralInterfaceConnector
            {
                public StructuralInterfaceConnector()
                {
                    this.Joins = new List<iFAB.Join>();
                    this.EndPoints = new List<AbstractStructuralInterface>();
                }
            }
        }
    }

}
