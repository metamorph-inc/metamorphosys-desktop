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
using System.Runtime.InteropServices;
using GME.MGA;
using GME.MGA.Core;
using GME.MGA.Parser;
using ISIS.GME.Common.Interfaces;

// using domain specific interfaces
using CyPhyML = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyMLClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2ComponentModel {
    
    public static class Convert {

        public static CyPhyML.Component AVMComponent2CyPhyML(CyPhyML.ComponentAssembly cyPhyMLComponentParent, avm.Component avmComponent, bool resetUnitLib = true, object messageConsole = null) {
            CyPhyML.Component c_rtn = AVM2CyPhyML.CyPhyMLComponentBuilder.AVM2CyPhyML(cyPhyMLComponentParent, avmComponent, resetUnitLib, messageConsole);
//            CyPhyComponentAutoLayout.LayoutComponent( c_rtn );
            return c_rtn;
        }

        public static CyPhyML.Component AVMComponent2CyPhyML(CyPhyML.Components cyPhyMLComponentParent, avm.Component avmComponent, bool resetUnitLib = true, object messageConsole = null)
        {
            CyPhyML.Component c_rtn = AVM2CyPhyML.CyPhyMLComponentBuilder.AVM2CyPhyML(cyPhyMLComponentParent, avmComponent, resetUnitLib, messageConsole);
//             CyPhyComponentAutoLayout.LayoutComponent(c_rtn);
             return c_rtn;
         }
        
        public static avm.Component CyPhyML2AVMComponent( CyPhyML.Component cyPhyMLComponent ) {
            return CyPhyML2AVM.AVMComponentBuilder.CyPhyML2AVM(cyPhyMLComponent);
        }
        
    }
    
}
