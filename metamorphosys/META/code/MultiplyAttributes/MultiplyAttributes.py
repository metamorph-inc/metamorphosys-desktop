# Copyright (C) 2013-2015 MetaMorph Software, Inc

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

# =======================
# This version of the META tools is a fork of an original version produced
# by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
# Their license statement:

# Copyright (C) 2011-2014 Vanderbilt University

# Developed with the sponsorship of the Defense Advanced Research Projects
# Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
# as defined in DFARS 252.227-7013.

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  


attr_map = { "Weight": 1000,
     "Volume": 100 * 1000 }

import sys
import os
import os.path

import udm

if os.path.dirname(udm.__file__) != os.path.dirname(os.path.abspath(__file__)):
    raise ImportError('Need to import the udm.pyd next to this file: ' + udm.__file__ + ";" + __file__)

def dfs(start, getter):
    q = []
    q.extend(start)
    while len(q) != 0:
        o = q.pop()
        q.extend(getter(o))
        yield o

if __name__ == '__main__':
    import shutil
    srcfile = sys.argv[1]
    destfile = os.path.splitext(srcfile)[0] + "_multiplied.mga"
    shutil.copy(srcfile, destfile)

    meta_dn = udm.SmartDataNetwork(udm.uml_diagram())
    meta_dn.open(os.path.join(os.path.dirname(__file__), r"CyPhyML_udm.xml"), "")
    meta = udm.map_uml_names(meta_dn.root)
    
    dn = udm.SmartDataNetwork(meta_dn.root)
    dn.open(destfile, "")
    
    q = [dn.root]
    while len(q) != 0:
        o = q.pop()
        q.extend(o.children())
        if o.is_instance:
            for (attrname, mult) in attr_map.items():
                try:
                    # i.e. o.Weight = o.archetype.Weight * 1000
                    o.__setattr__(attrname, o.archetype.__getattr__(attrname) * mult)
                except:
                    pass
        
    dn.close_with_update()
