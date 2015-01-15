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
using System.Xml.Linq;

namespace InterchangeXmlComparator
{
    public class DesignComparator
    {
        private XDocument ExportedDocument { get; set; }
        private XDocument DesiredDocument { get; set; }

        public DesignComparator(XDocument exportedDocument, XDocument desiredDocument)
        {
            ExportedDocument = exportedDocument;
            DesiredDocument = desiredDocument;
        }

        public void Check()
        {
            #region Check Design
            var eDesign = ExportedDocument.Root;
            var dDesign = DesiredDocument.Root;

            if (eDesign == null || dDesign == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Root element is missing!");
                Environment.Exit(1);
            }

            XmlFramework.CheckElements(eDesign, dDesign);

            #endregion

            // Remove unclear namespaces (like empty xmlns attributes and qX namespaces.
            XmlFramework.RemoveEmptyNamespaces(eDesign);
            XmlFramework.RemoveEmptyNamespaces(dDesign);

            #region Check design containers

            var eContainers = eDesign.Descendants().Where(x => x.Name == "Container" || x.Name == "RootContainer").ToList();
            var dContainers = dDesign.Descendants().Where(x => x.Name == "Container" || x.Name == "RootContainer").ToList();

            XmlFramework.CheckElements(eContainers, dContainers);

            #endregion

            #region Check ComponentInstances

            var dComponentInstances = new Dictionary<XElement, List<XElement>>();
            var eComponentInstances = new Dictionary<XElement, List<XElement>>();

            foreach (var dContainer in dContainers)
            {
                dComponentInstances[dContainer] = dContainer.Elements("ComponentInstance").ToList();
            }

            foreach (var eContainer in eContainers)
            {
                eComponentInstances[eContainer] = eContainer.Elements("ComponentInstance").ToList();
            }

            XmlFramework.CheckInParent(dComponentInstances, eComponentInstances, "Name");

            #endregion

            #region Check properties and Values

            var dProperties = new List<XElement>(dDesign.Descendants().Where(x => x.Name == "Property" || x.Name == "PrimitivePropertyInstance"));
            var eProperties = new List<XElement>(eDesign.Descendants().Where(x => x.Name == "Property" || x.Name == "PrimitivePropertyInstance"));

            foreach (var eProperty in eProperties)
            {
                var dProperty = XmlFramework.GetPairElement(eProperty, dProperties, new List<string>() { "Name", "ID", "IDinComponentModel" });

                if (dProperty == null)
                {
                    Feedback.Add(new Feedback()
                    {
                        ExportedNode = eProperty,
                        FeedbackType = FeedbackType.Error,
                        Message = "This property is not in the desired model."
                    });
                    continue;
                }

                XmlFramework.CheckElements(eProperty, dProperty);
                XmlFramework.CheckValue(eProperty.Element("Value"), dProperty.Element("Value"));
            }

            foreach (var dProperty in dProperties)
            {
                var eProperty = XmlFramework.GetPairElement(dProperty, eProperties, new List<string>() { "Name", "ID", "IDinComponentModel" });

                if (eProperty == null)
                {
                    Feedback.Add(new Feedback()
                    {
                        DesiredNode = dProperty,
                        FeedbackType = FeedbackType.Error,
                        Message = "This property has not exported, but should be."
                    });
                    continue;
                }

                XmlFramework.CheckElements(eProperty, dProperty);
                XmlFramework.CheckValue(eProperty.Element("Value"), dProperty.Element("Value"));
            }
            
            #endregion
            

            #region Check Join Data
            
            var eJDs = new List<XElement>(eDesign.Descendants().Where(x => x.Name == "JoinData"));
            var dJDs = new List<XElement>(dDesign.Descendants().Where(x => x.Name == "JoinData"));
            foreach (var eJD in eJDs)
            {
                var dJD = XmlFramework.GetPairElement(eJD, dJDs,"id",false);

                if (dJD == null)
                {
                    Feedback.Add(new Feedback()
                        {
                            ExportedNode = eJD,
                            FeedbackType = FeedbackType.Error,
                            Message = "This JoinData is not in the desired model."
                        });
                    continue;
                }

                XmlFramework.CheckElements(eJD, dJD);
            }
            foreach (var dJD in dJDs)
            {
                var eJD = XmlFramework.GetPairElement(dJD, eJDs, "id",false);

                if (eJD == null)
                {
                    Feedback.Add(new Feedback()
                    {
                        ExportedNode = dJD,
                        FeedbackType = FeedbackType.Error,
                        Message = "This JoinData is missing from the export."
                    });
                    continue;
                }

                XmlFramework.CheckElements(dJD, eJD);
            }
            
            #endregion
            
            #region Check connections

            #region Collectiong connections

            var ePortmaps = new Dictionary<string, HashSet<string>>();
            foreach (var ePortmapNode in eDesign.Descendants().Where(x => x.Attribute("PortMap") != null && !string.IsNullOrEmpty(x.Attribute("PortMap").Value)))
            {
                var ePortmapIds = ePortmapNode.Attribute("PortMap").Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!ePortmapIds.Any()) continue;

                var baseId = ePortmapNode.Attribute("ID") == null ? string.Empty : ePortmapNode.Attribute("ID").Value;
                if (string.IsNullOrEmpty(baseId)) continue;

                if (!ePortmaps.ContainsKey(baseId))
                    ePortmaps[baseId] = new HashSet<string>();

                ePortmaps[baseId].AddRange(ePortmapIds);

                foreach (var ePortmapId in ePortmapIds)
                {
                    if (!ePortmaps.ContainsKey(ePortmapId))
                        ePortmaps[ePortmapId] = new HashSet<string>();

                    ePortmaps[ePortmapId].Add(baseId);
                }
            }

            var dPortmaps = new Dictionary<string, HashSet<string>>();
            foreach (var dPortmapNode in dDesign.Descendants().Where(x => x.Attribute("PortMap") != null && !string.IsNullOrEmpty(x.Attribute("PortMap").Value)))
            {
                var dPortmapIds = dPortmapNode.Attribute("PortMap").Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!dPortmapIds.Any()) continue;

                var baseId = dPortmapNode.Attribute("ID") == null ? string.Empty : dPortmapNode.Attribute("ID").Value;
                if (string.IsNullOrEmpty(baseId)) continue;

                if (!dPortmaps.ContainsKey(baseId))
                    dPortmaps[baseId] = new HashSet<string>();

                dPortmaps[baseId].AddRange(dPortmapIds);

                foreach (var dPortmapId in dPortmapIds)
                {
                    if (!dPortmaps.ContainsKey(dPortmapId))
                        dPortmaps[dPortmapId] = new HashSet<string>();

                    dPortmaps[dPortmapId].Add(baseId);
                }
            }

            #endregion

            #region Check connection pairs

            foreach (var dPortmap in dPortmaps)
            {
                if (!ePortmaps.ContainsKey(dPortmap.Key))
                {
                    Feedback.Add(new Feedback
                    {
                        DesiredNode = dDesign.Descendants().FirstOrDefault(x => x.Attribute("ID") != null && x.Attribute("ID").Value == dPortmap.Key),
                        FeedbackType = FeedbackType.Error,
                        Message = "Missing connection in the exported file"
                    });
                    continue;
                }

                var result = dPortmap.Value.All(x => ePortmaps[dPortmap.Key].Contains(x));
                if (!result)
                    Feedback.Add(new Feedback
                    {
                        DesiredNode = dDesign.Descendants().FirstOrDefault(x => x.Attribute("ID") != null && x.Attribute("ID").Value == dPortmap.Key),
                        FeedbackType = FeedbackType.Error,
                        Message = "Missing connection in the exported file"
                    });
            }

            foreach (var ePortmap in ePortmaps)
            {
                if (!dPortmaps.ContainsKey(ePortmap.Key))
                {
                    Feedback.Add(new Feedback
                    {
                        ExportedNode = eDesign.Descendants().FirstOrDefault(x => x.Attribute("ID") != null && x.Attribute("ID").Value == ePortmap.Key),
                        FeedbackType = FeedbackType.Error,
                        Message = "Unnecessary connection in the exported file"
                    });
                    continue;
                }

                var result = ePortmap.Value.All(x => dPortmaps[ePortmap.Key].Contains(x));
                if (!result)
                    Feedback.Add(new Feedback
                    {
                        ExportedNode = eDesign.Descendants().FirstOrDefault(x => x.Attribute("ID") != null && x.Attribute("ID").Value == ePortmap.Key),
                        FeedbackType = FeedbackType.Error,
                        Message = "Unnecessary connection in the exported file"
                    });
            }

            #endregion

            #endregion
        }
    }
}