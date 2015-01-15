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

namespace LayoutJson 
{
    public class Constraint
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<int> group { get; set; }    // target packages of group constraint 
    }

    public class Package
    {
        public string name { get; set; }
        public string package { get; set; }     // packaging technology : BGA, UFGBA, SOT, ...
        public int? pkg_idx { get; set; }
        public string ComponentID { get; set; }
        public string RelComponentID { get; set; }  // if the component is placed relative to another component 
        public double width { get; set; }
        public double height { get; set; }
        // for assymetric components the actual origin relative to the symmetric assumption origin
        public double? originX { get; set; }  
        public double? originY { get; set; }
        public double? x { get; set; }
        public double? y { get; set; }
        public int? rotation { get; set; }
        public int? layer { get; set; }
        public List<Constraint> constraints { get; set; }
        public bool? multiLayer { get; set; }
        public bool? doNotPlace { get; set; }       // directive for layout solver to ignore a part for placement
    }

    public class Signal
    {
        public string name { get; set; }
        public double? length { get; set; }
        public int? bends { get; set; }
        public string RelComponentID { get; set; }     // the signal place relative to another component place
        public List<Pin> pins { get; set; }
        public List<Wire> wires { get; set; }
        public List<Via> vias { get; set; }
    }

    public class Pin
    {
        public string name { get; set; }
        public string gate { get; set; }
        public string pad { get; set; }
        public string package { get; set; }
    }

    public class Wire
    {
        public double x1 { get; set; }
        public double y1 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double width { get; set; }
        public int layer { get; set; }
    }

    public class Via
    {
        public double x { get; set; }
        public double y { get; set; }
        public double drill { get; set; }
        public int layerBegin { get; set; }
        public int layerEnd { get; set; }
    }

    public class Layout
    {
        public double boardWidth { get; set; }
        public double boardHeight { get; set; }
        public int numLayers { get; set; }
        public string boardTemplate { get; set; }       // name of the board template file to be used
        public string designRules { get; set; }         // name of the design rules file to be used
        public List<Package> packages { get; set; }
        public List<Signal> signals { get; set; }
        public List<Constraint> constraints { get; set; } // Global/Group Constraints
    }


    // Thoughts on Constraints:

    // Default constraint is applied within the scope of a single package
    // Relative constraints are applied to two packages
    // There are two categories of global constraints
    // 1) Global - applicable to all packages 
    // 2) Group - applicable to multiple packages
 
    // global & group constraints are included in the top level constraint list
    // group constraints have their group list defined which includes the 
    // list of packages to which the constraint applies

    // group constraints are simply a shortcut to applying the same constraint to multiple packages

    // examples of global/group constraints --> 
    //    place all packages within a region
    //    place all packages outside a region (negative of above)
    //    place all packages on a layer
    //    place all packages not on a layer
    //    etc.
}
