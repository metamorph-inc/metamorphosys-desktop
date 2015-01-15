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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;

namespace CyPhy2DesignInterchange_ddp1format
{
    public class DesignPrimitivesWrapper
    {
        private object Primitive { get; set; }

        public DesignPrimitivesWrapper(object primitive)
        {
            Primitive = primitive;
        }

        public string Path
        {
            get
            {
                if (Primitive is CyPhy.DesignEntity)
                    return ((CyPhy.DesignEntity)Primitive).Path;
                return string.Empty;
            }
        }

        public List<CyPhy.Component> Components
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.Component>(((CyPhy.ComponentAssembly)Primitive).Children.ComponentCollection);
                else
                    return new List<CyPhy.Component>(((CyPhy.DesignContainer)Primitive).Children.ComponentCollection);
            }
        }

        public List<CyPhy.ComponentRef> ComponentRefs
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.ComponentRef>(((CyPhy.ComponentAssembly) Primitive).Children.ComponentRefCollection);
                else
                    return new List<CyPhy.ComponentRef>(((CyPhy.DesignContainer)Primitive).Children.ComponentRefCollection);
            }
        }

        public List<CyPhy.Parameter> Parameters
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.Parameter>(((CyPhy.ComponentAssembly)Primitive).Children.ParameterCollection);
                else
                    return new List<CyPhy.Parameter>(((CyPhy.DesignContainer)Primitive).Children.ParameterCollection);
            }
        }

        public List<CyPhy.Property> Properties
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.Property>(((CyPhy.ComponentAssembly)Primitive).Children.PropertyCollection);
                else
                    return new List<CyPhy.Property>(((CyPhy.DesignContainer)Primitive).Children.PropertyCollection);
            }
        }

        public List<CyPhy.ComponentAssembly> ComponentAssemblies
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.ComponentAssembly>(((CyPhy.ComponentAssembly)Primitive).Children.ComponentAssemblyCollection);
                else
                    return new List<CyPhy.ComponentAssembly>(((CyPhy.DesignContainer)Primitive).Children.ComponentAssemblyCollection);
                
            }
        }

        public List<CyPhy.DesignEntity> DesignContainers
        {
            get
            {
                if (Primitive is CyPhy.DesignContainer)
                    return new List<CyPhy.DesignEntity>(((CyPhy.DesignContainer)Primitive).Children.DesignContainerCollection);
                return new List<CyPhy.DesignEntity>();
            }
        }

        public List<CyPhy.ValueFlow> ValueFlows
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.ValueFlow>(((CyPhy.ComponentAssembly)Primitive).Children.ValueFlowCollection);
                else
                    return new List<CyPhy.ValueFlow>(((CyPhy.DesignContainer)Primitive).Children.ValueFlowCollection);
            }
        }

        public List<CyPhy.SimpleFormula> SimpleFormulas
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.SimpleFormula>(((CyPhy.ComponentAssembly)Primitive).Children.SimpleFormulaCollection);
                else
                    return new List<CyPhy.SimpleFormula>(((CyPhy.DesignContainer)Primitive).Children.SimpleFormulaCollection);
            }
        }

        public List<CyPhy.Port> Ports
        {
            get
            {
                if (Primitive is CyPhy.ComponentAssembly)
                    return new List<CyPhy.Port>(((CyPhy.ComponentAssembly)Primitive).Children.PortCollection);
                else
                    return new List<CyPhy.Port>(((CyPhy.DesignContainer)Primitive).Children.PortCollection);
            }
        }

    }
}