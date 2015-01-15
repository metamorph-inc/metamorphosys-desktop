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
using System.Xml;
using System.Xml.Linq;

namespace InterchangeXmlComparator
{
    public enum PathTypes
    {
        FullPath,
        ShortPath
    }

    public static class Extensions
    {
        public static string GetPath(this XObject node, PathTypes pathType = PathTypes.FullPath)
        {
            if (node == null) return string.Empty;
            if (node.NodeType == XmlNodeType.Attribute)
            {
                var attribute = (XAttribute)node;
                var attrName = attribute.Name.ToString();

                return string.Format("{0}[{1}]", GetPath(attribute.Parent), attrName);
            }
            if (node.NodeType == XmlNodeType.Element)
            {
                return GetPath((XElement)node, pathType);
            }

            return string.Empty;
        }

        private static string GetPath(this XElement element, PathTypes pathType = PathTypes.FullPath)
        {
            if (element == null) return string.Empty;
            var ancestors = element.Ancestors().ToList();

            if (!ancestors.Any())
            {
                return element.Name.ToString();
            }

            var p = ancestors.Select(x => x.Name.LocalName).Aggregate((path, next) => next + "\\" + path);
            p += "\\" + element.Name.LocalName;

            if (pathType == PathTypes.ShortPath) return p;

            if (element.NodeType == XmlNodeType.Element)
            {
                var index = element.Parent.Elements().ToList().IndexOf(element);
                p = p + '[' + index + ']';

                // Add name attribute (if any)
                if (element.Attribute("Name") != null)
                    p = string.Format("{0}[Name=='{1}']", p, element.Attribute("Name").Value);
                else if (element.Attribute("ID") != null)
                    p = string.Format("{0}[ID=='{1}']", p, element.Attribute("ID").Value);

                if (element.HasElements) return p;
                var textValue = element.DescendantNodes().OfType<XText>().Select(x => x.Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(textValue))
                {
                    p = String.Format("{0}\\{{\"{1}\"}}", p, textValue);
                }
            }
            return p;
        }

        public static void AddRange<T>(this HashSet<T> hash, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                hash.Add(item);
            }
        }
    }
}