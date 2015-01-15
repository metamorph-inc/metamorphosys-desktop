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
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace CyPhy2ComponentModel.Validation
{
    public class ComponentImportRules
    {
        public static bool CheckUniqueNames(string inputXMLFile, ref List<string> errorMessages)
        {
            var result = true;
            using (var sr = new StreamReader(inputXMLFile))
            {
                var xdoc = XDocument.Load(sr);

                foreach (var node in xdoc.DescendantNodes().Where(x=> x.NodeType == XmlNodeType.Element))
                {
                    var xElement = (XElement)node;
                    var names = new HashSet<string>();
                    foreach (var xChild in xElement.Elements())
                    {
                        var nameAttr = xChild.Attribute("Name");
                        if (nameAttr == null) continue;

                        var nameVal = nameAttr.Value;
                        if (string.IsNullOrEmpty(nameVal)) continue;

                        if (names.Contains(nameVal))
                        {
                            var path =
                                xChild.AncestorsAndSelf()
                                    .Select(x => x.Name.LocalName)
                                    .Aggregate((workingSentence, next) => next + "\\" + workingSentence);
                            errorMessages.Add(string.Format("ERROR: Duplicated names: {0} - {1}", nameVal, path));
                            result = false;
                        }
                        names.Add(nameVal);
                    }
                }
            }

            return result;

        }

        public static bool ValidateXsd(string inputXMLFile, ref List<string> oErrorMessages)
        {
            var noErrors = true;
            var errorMessages = new List<string>();
            using (var sr = new StreamReader(inputXMLFile))
            {
                var schemas = new XmlSchemaSet();

                #region Collect XSD files

                var xsdDirectory = new DirectoryInfo(META.VersionInfo.MetaPath).GetFiles("*.xsd", SearchOption.AllDirectories).FirstOrDefault();
                if (xsdDirectory == null || xsdDirectory.Directory==null)
                {
                    errorMessages.Add("XSD directory cannot be found");
                    return false;
                }

                var xsdFileSet =
                    xsdDirectory.Directory.GetFiles("*.xsd", SearchOption.TopDirectoryOnly).Select(x => x.FullName);

                #endregion

                foreach (var xsd in xsdFileSet)
                {
                    if (File.Exists(xsd))
                        schemas.Add(XmlSchema.Read(XmlReader.Create(new StreamReader(xsd)), (sender, args) =>
                                                                                            {
                                                                                                errorMessages.Add("Schema is wrong: " + args.Message);
                                                                                            }));
                    else
                        errorMessages.Add("XSD file doesn't exist: " + xsd);
                }

                var xdoc = XDocument.Load(sr);

                xdoc.Validate(schemas, (o, e) =>
                {
                    errorMessages.Add(e.Message);
                    noErrors = false;
                }, true);

            }
            oErrorMessages.AddRange(errorMessages);
            return noErrors;
        }

        public static bool ExecuteAll(string inputXMLFile, ref List<string> errorMessages)
        {
            var results = new List<bool>
                          {
                              CheckUniqueNames(inputXMLFile, ref errorMessages),
                              ValidateXsd(inputXMLFile, ref errorMessages)
                          };

            return results.All(x => x);
        }

    }
}
