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
using System.Text.RegularExpressions;


namespace CyPhy2CAD_CSharp.DataRep
{
    public class GeometryMarkerRep
    {
        public double x;
        public double y;
        public double z;
        public double i;
        public double j;
        public double k;
        public double pi;
        public string ComponentID;

        public GeometryMarkerRep(string marker)
        {
            if (!String.IsNullOrEmpty(marker))
            {
                marker = Regex.Replace(marker, @"\t|\n|\r", " ");
                string[] words = marker.Split(' ');
                foreach (string str in words)
                {
                    string[] field = str.Split(':');
                    if (field.Count() == 2)
                    {
                        string type = field[0];
                        if (type == "x")
                            x = Convert2Double(field[1]);
                        else if (type == "y")
                            y = Convert2Double(field[1]);
                        else if (type == "z")
                            z = Convert2Double(field[1]);
                        else if (type == "i")
                            i = Convert2Double(field[1]);
                        else if (type == "j")
                            j = Convert2Double(field[1]);
                        else if (type == "k")
                            k = Convert2Double(field[1]);
                        else if (type == "pi")
                            pi = Convert2Double(field[1]);
                    }
                }
            }
        }

        private double Convert2Double(string sValue)
        {
            try
            {
                double dValue = Convert.ToDouble(sValue);
                return dValue;
            }
            catch (FormatException)
            {
                return 0;
            }
            catch (OverflowException)
            {
                return 0;
            }
        }

        public CAD.GeometryMarkerType ToCADXml()
        {
            CAD.GeometryMarkerType marker = new CAD.GeometryMarkerType();
            marker._id = UtilityHelpers.MakeUdmID();
            marker.ComponentID = ComponentID;
            marker.x = x;
            marker.y = y;
            marker.z = z;
            marker.i = i;
            marker.j = j;
            marker.k = k;
            marker.pi = pi;
            return marker;
        }
    }
}
