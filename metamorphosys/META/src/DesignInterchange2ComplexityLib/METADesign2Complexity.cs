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
using avm;
using Complexity;

namespace METADesignInterchange2ComplexityLib
{
    public static class METADesign2Complexity
    {
        private static List<avm.ComponentInstance> RecursivelyGetAllComponentInstances(Compound c_root)
        {
            List<avm.ComponentInstance> lci_rtn = new List<avm.ComponentInstance>();

            if (c_root.ComponentInstance != null)
            {
                lci_rtn.InsertRange(0, c_root.ComponentInstance);
            }

            if (c_root.Container1 != null)
            {
                foreach (Compound c in c_root.Container1.Where(c => c is Compound))
                {
                    lci_rtn.InsertRange(0, RecursivelyGetAllComponentInstances(c));
                }
            }

            return lci_rtn;
        }

        private static List<avm.ComponentInstance> RecursivelyGetAllComponentInstances(avm.Design dm_root)
        {
            List<avm.ComponentInstance> lci_rtn = new List<avm.ComponentInstance>();
            foreach (Compound c in dm_root.RootContainer.Container1.Where(c => c is Compound))
            {
                lci_rtn.InsertRange(0, RecursivelyGetAllComponentInstances(c));
            }
            return lci_rtn;
        }

        public static Complexity.Design Design2Complexity(avm.Design dmInput)
        {
            var dMain = new Complexity.Design();
            dMain.ComponentInstances = new List<Complexity.ComponentInstance>();
            dMain.Connections = new List<Complexity.Connection>();
            dMain.AVMID = dmInput.DesignID;
            dMain.Name = dmInput.Name;

            var componentInstances = RecursivelyGetAllComponentInstances(dmInput);
            var componentMapping = new Dictionary<avm.ComponentInstance, Complexity.ComponentInstance>();
            foreach (var ci in componentInstances)
            {
                var cci_new = new Complexity.ComponentInstance
                              {
                                  AVMID = ci.ComponentID,
                                  Name = ci.Name,
                                  Complexity = 1,
                                  DistributionType = DistributionTypeEnum.None
                              };

                ((List<Complexity.ComponentInstance>)dMain.ComponentInstances).Add(cci_new);

                if (!componentMapping.ContainsKey(ci))
                    componentMapping[ci] = cci_new;
            }

            foreach (var ci in componentInstances)
            {
                if (!componentMapping.ContainsKey(ci)) continue;

                foreach (var pi in ci.PortInstance)
                {
                    foreach (var portMap in pi.PortMap)
                    {
                        var parentComponent = componentInstances.Where(x => x.PortInstance.Any(y => y.ID == portMap)).FirstOrDefault();
                        
                        if (parentComponent == null) continue;
                        if (!componentMapping.ContainsKey(parentComponent)) continue;

                        Complexity.Connection cc_new = new Complexity.Connection();
                        ((List<Complexity.Connection>)dMain.Connections).Add(cc_new);
                        cc_new.src = componentMapping[ci];
                        cc_new.dst = componentMapping[parentComponent];
                        cc_new.Complexity = 0.1;
                        cc_new.DistributionType = Complexity.DistributionTypeEnum.None;
                    }
                }
            }
            
            return dMain;
        }
    }
}
