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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using GmeCommon = ISIS.GME.Common;
using GME.CSharp;

namespace CyPhy2CAD_CSharp.DataRep
{
    public enum DatumType
    {
        CSYS,
        Point,
        Surface,
        Axis
    }

    public class Datum
    {

        public string DatumName { get; set; }
        public DatumType Type { get; set; }
        public string Orientation { get; set; }
        public string Alignment { get; set; }
        public string ComponentID { get; set; }                     // InstanceGUID
        public bool Guide { get; set; }
        public string DatumID { get; set; }                         // UDM ID
        public GeometryMarkerRep Marker { get; set; }

        public Datum(string name, string type, string compid, bool guide)
        {
            DatumName = name;
            if (type == "CoordinateSystem")
                Type = DatumType.CSYS;
            else if (type == "Point")
                Type = DatumType.Point;
            else if (type == "Surface")
                Type = DatumType.Surface;
            else if (type == "Axis")
                Type = DatumType.Axis;

            ComponentID = compid;
            DatumID = UtilityHelpers.MakeUdmID();

            Guide = guide;
        }

        public Datum(CyPhy.CADDatum datum,
                     string compid, bool guide)
        {
            DatumName = datum.Attributes.DatumName;
            if (datum.Kind == "CoordinateSystem")
                Type = DatumType.CSYS;
            else if (datum.Kind == "Point")
                Type = DatumType.Point;
            else if (datum.Kind == "Surface")
                Type = DatumType.Surface;
            else if (datum.Kind == "Axis")
                Type = DatumType.Axis;

            ComponentID = compid;
            DatumID = datum.ID;
            Guide = guide;
            if (!String.IsNullOrEmpty(datum.Attributes.GeometricMarker))
                Marker = new GeometryMarkerRep(datum.Attributes.GeometricMarker);
        }
    }


}
