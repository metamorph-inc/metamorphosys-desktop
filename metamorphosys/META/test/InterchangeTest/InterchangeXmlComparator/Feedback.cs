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
using System.Xml;
using System.Xml.Linq;

namespace InterchangeXmlComparator
{
    public class Feedback
    {
        private static readonly List<Feedback> Feedbacks = new List<Feedback>();

        public static void Add(Feedback fb)
        {
            Feedbacks.Add(fb);
        }

        public static List<Feedback> FeedBacklist
        {
            get
            {
                return Feedbacks;
            }
        }

        public FeedbackType FeedbackType { get; set; }

        public XObject ExportedNode { get; set; }
        public XObject DesiredNode { get; set; }

        public string ExportedNodeName
        {
            get { return GetNodeName(ExportedNode); }
        }

        public string DesiredNodeName
        {
            get { return GetNodeName(DesiredNode); }
        }

        public string Message { get; set; }

        public string ExportedNodePath
        {
            get { return ExportedNode.GetPath(); }
        }

        public string DesiredNodePath
        {
            get { return DesiredNode.GetPath(); }
        }

        private static string GetNodeName(XObject node)
        {
            if (node == null) return string.Empty;
            if (node.NodeType == XmlNodeType.Element)
            {
                return ((XElement)node).Name.ToString();
            }
            if (node.NodeType == XmlNodeType.Attribute)
            {
                return ((XAttribute)node).Name.ToString();
            }
            return string.Empty;
        }
    }

    public enum FeedbackType
    {
        Warning,
        Error,
    }
}