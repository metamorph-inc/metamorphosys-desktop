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

namespace CyPhy2Modelica_v2.Modelica
{
    public class Connection : IComparable<Connection>
    {
        public Connection(string srcName, string dstName)
        {
            // TODO: Should these be empty strings or just left as null? see usage in ModelicaConnection.ToString()
            SrcConnectorName = srcName;
            DstConnectorName = dstName;

            SrcInstanceName = "";
            DstInstanceName = "";
        }
        public string SrcInstanceName { get; set; }
        public string SrcConnectorName { get; set; }
        public string DstInstanceName { get; set; }
        public string DstConnectorName { get; set; }
        public string Type { get; set; }
        public string Annotation { 
            get 
            { 
                var thickness = "";
                var color = "";
                if (Type.EndsWith(".FluidPort_a") || Type.EndsWith(".FluidPort_b"))
                {
                    thickness = ",thickness=0.5";
                    color = ",color={0,127,255}"; //Light Blue
                }
                else if (Type.EndsWith(".PositivePin") || Type.EndsWith(".NegativePin") || Type.EndsWith(".Pin"))
                {
                    color = ",color={0,0,255}"; //Blue
                }
                else if (Type.EndsWith(".HeatPort_a") || Type.EndsWith(".HeatPort_b") || Type.EndsWith(".HeatPort"))
                {
                    color = ",color={191,0,0}"; //Red
                }
                else if (Type.EndsWith(".Hydraulic_Port"))
                {
                    color = ",color={255,0,128}"; //Pink
                }
                else if (Type.EndsWith(".FlangeWithBearing"))
                {
                    color = ",color={95,95,95}";
                    thickness = ",thickness=0.5";
                }
                else if (Type.EndsWith(".Frame_a") || Type.EndsWith(".Frame_b") || Type.EndsWith(".Frame"))
                {
                    color = ",color={175,175,175}";
                    thickness = ",thickness=0.5";
                }
                
                return string.Format("annotation(Line(points = {{{{0.0,0.0}},{{0.1,0.1}}}}{0}{1}))", color, thickness);
            } 
        }
        public string JointName { 
            get
            {
                string srcFullName;
                if (string.IsNullOrWhiteSpace(this.SrcInstanceName))
                {
                    srcFullName = this.SrcConnectorName;
                }
                else
                {
                    srcFullName = this.SrcInstanceName + "." + this.SrcConnectorName;
                }

                string dstFullName;
                if (string.IsNullOrWhiteSpace(this.DstInstanceName))
                {
                    dstFullName = this.DstConnectorName;
                }
                else
                {
                    dstFullName = this.DstInstanceName + "." + this.DstConnectorName;
                }

                if (srcFullName.CompareTo(dstFullName) > 0)
                {
                    return dstFullName + ", " + srcFullName;
                }
                else
                {
                    return srcFullName + ", " + dstFullName;
                }
            }
        }

        public override string ToString()
        {
                return string.Format("  connect({0}) {1};", this.JointName, this.Annotation);
        }

        public int CompareTo(Connection other)
        {
            return this.JointName.CompareTo(other.JointName);
        }
    }

}
