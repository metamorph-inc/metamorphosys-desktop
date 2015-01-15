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
using GME.MGA;
using GME.CSharp;
using System.IO;
using Xunit;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace ComponentInterchangeTest
{
    public class LayoutFixture : IDisposable
    {
        #region Paths
        public static String testPath = Path.Combine(META.VersionInfo.MetaPath,
                                       "test",
                                       "InterchangeTest",
                                       "ComponentInterchangeTest",
                                       "SharedModels",
                                       "Layout");
        public static String xmePath = Path.Combine(testPath, "Layout.xme");
        public String mgaPath;
        #endregion

        public MgaProject project;

        public LayoutFixture()
        {
            String connString;
            MgaUtils.ImportXMEForTest(xmePath, out connString);
            mgaPath = connString.Substring("MGA=".Length);
            Assert.True(File.Exists(mgaPath), "Input model not found; import may have failed.");

            project = CommonFunctions.GetProject(mgaPath);
        }

        public void Dispose()
        {
            project.Save();
            project.Close();
        }
    }

    public class LayoutModelImport
    {
        [Fact]
        [Trait("ProjectImport/Open", "Layout")]
        public void ProjectXmeImport()
        {
            Assert.DoesNotThrow(() => { 
                var fixture = new LayoutFixture();
                fixture.Dispose();
            });
        }
    }

    public class Layout : IUseFixture<LayoutFixture>
    {
        #region Fixture
        LayoutFixture fixture;
        public void SetFixture(LayoutFixture data)
        {
            fixture = data;
        }
        #endregion

        private CyPhy.Component GetComponentByName(String name)
        {
            MgaFilter filter = fixture.project.CreateFilter();
            filter.Kind = "Component";
            filter.Name = name;

            return fixture.project.AllFCOs(filter)
                                  .Cast<MgaFCO>()
                                  .Select(x => CyPhyClasses.Component.Cast(x))
                                  .Cast<CyPhy.Component>()
                                  .Where(c => c.ParentContainer.Kind == "Components")
                                  .First();
        }

        private void PerformInTransaction(MgaGateway.voidDelegate del)
        {
            var mgaGateway = new MgaGateway(fixture.project);
            fixture.project.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(del);
        }

        private String GetLayout(IMgaFCO fco)
        {
            Boolean hasAllAspect = false;
            foreach (IMgaPart part in fco.Parts)
            {
                if (part.MetaAspect.Name == "All")
                    hasAllAspect = true;
            }

            foreach (IMgaPart part in fco.Parts)
            {
                if (part.MetaAspect.Name == "All" || hasAllAspect == false)
                {
                    String icon;
                    int xpos = 0;
                    int ypos = 0;
                    part.GetGmeAttrs(out icon, out xpos, out ypos);

                    return String.Format("{0},{1}", xpos, ypos);
                }
            }

            return "";
        }

        [Fact]
        public void LayoutPreserved()
        {
            var layoutsDontMatch = new List<String>();

            PerformInTransaction(delegate
            {
                var component = GetComponentByName("CyPhyCore");
                var componentsFolder = CyPhyClasses.Components.Cast(component.ParentContainer.Impl);
                var d_Layouts = new Dictionary<String, String>();

                List<IMgaFCO> fcos = new List<IMgaFCO>();
                foreach (var child in component.AllChildren
                                               .Select(child => child.Impl)
                                               .OfType<IMgaFCO>())
                {
                    fcos.Add(child);
                    if (child is IMgaModel)
                    {
                        foreach (var nestedChild in (child as IMgaModel).ChildFCOs)
                        {
                            fcos.Add(nestedChild as IMgaFCO);
                        }
                    }
                }

                foreach (var fco in fcos)
                {
                    String path = String.Format("{0}/{1}", fco.ParentModel.Name, fco.Name);
                    d_Layouts[path] = GetLayout(fco);
                }

                var avmComponent = CyPhy2ComponentModel.Convert.CyPhyML2AVMComponent(component);
                avmComponent.Supercedes = new List<String>();
                CyPhyComponentExporter.CyPhyComponentExporterInterpreter
                                      .SerializeAvmComponent(avmComponent, 
                                                             Path.Combine(LayoutFixture.testPath, 
                                                                          "layout.acm"));

                component.Delete();
                component = CyPhy2ComponentModel.Convert.AVMComponent2CyPhyML(componentsFolder, avmComponent);
                
                fcos.Clear();
                foreach (var child in component.AllChildren
                                               .Select(child => child.Impl)
                                               .OfType<IMgaFCO>())
                {
                    fcos.Add(child);
                    if (child is IMgaModel)
                    {
                        foreach (var nestedChild in (child as IMgaModel).ChildFCOs)
                        {
                            fcos.Add(nestedChild as IMgaFCO);
                        }
                    }
                }

                foreach (var fco in fcos)
                {
                    String path = String.Format("{0}/{1}", fco.ParentModel.Name, fco.Name);
                    if (d_Layouts.ContainsKey(path))
                    {
                        var expected = d_Layouts[path];
                        var current = GetLayout(fco);
                        if (expected != current)
                            layoutsDontMatch.Add(path + " " + fco.GetGuidDisp());
                    }
                    else
                        layoutsDontMatch.Add(path + " " + fco.GetGuidDisp());
                }
            });

            if (layoutsDontMatch.Any())
            {
                String msg = "Some elements don't have preserved layouts:";
                foreach (var path in layoutsDontMatch)
                    msg += Environment.NewLine + path;
                Assert.True(false, msg);
            }
        }
    }
}
