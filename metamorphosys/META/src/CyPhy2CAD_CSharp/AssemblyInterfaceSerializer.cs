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
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace CyPhy2CAD_CSharp
{
    /*
    public partial class AssembliesType
    {
        String Serialize()
        {
            // Additional information: Could not load file or assembly 'CyPhy2CAD_CSharp.XmlSerializers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8adbc89a2d94c2a4' or one of its dependencies. The system cannot find the file specified.
            XmlSerializer xs = new XmlSerializer(typeof(AssembliesType)); // n.b. An exception is expected here. It is caught inside of the .NET framework code
            StringWriter sw = new Utf8StringWriter();       //StringWriter sw = new StringWriter();

            xs.Serialize(sw, this);
            return sw.ToString();
        }

        public void SerializeToFile(String path)
        {
            String xml = this.Serialize();

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(xml);
            }
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }       
    }
    */

    public class MyXmlSerializeBase
    {
        String Serialize()
        {
            // Additional information: Could not load file or assembly 'CyPhy2CAD_CSharp.XmlSerializers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8adbc89a2d94c2a4' or one of its dependencies. The system cannot find the file specified.
            XmlSerializer xs = new XmlSerializer(this.GetType()); // n.b. An exception is expected here. It is caught inside of the .NET framework code
            StringWriter sw = new Utf8StringWriter();       //StringWriter sw = new StringWriter();

            xs.Serialize(sw, this);
            return sw.ToString();
        }

        public void SerializeToFile(String path)
        {
            String xml = this.Serialize();

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(xml);
            }
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }
}

namespace CAD
{
    public partial class AssembliesType : CyPhy2CAD_CSharp.MyXmlSerializeBase
    {
    }
}


