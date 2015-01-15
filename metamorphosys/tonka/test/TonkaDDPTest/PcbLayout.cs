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
using META;
using Xunit;
using System.IO;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using GME.MGA;
using GME.CSharp;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using CyPhy2DesignInterchange;

namespace TonkaDDPTest
{
    public class PcbLayoutFixture : IDisposable
    {
        public static String path_Test = Path.Combine(META.VersionInfo.MetaPath,
                                                      "..",
                                                      "tonka",
                                                      "test",
                                                      "TonkaDDPTest",
                                                      "Model");

        private static String path_XME = Path.Combine(path_Test,
                                                      "testmodel.xme");

        private String path_MGA;
        public MgaProject proj { get; private set; }

        public PcbLayoutFixture()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(path_XME, out mgaConnectionString);
            path_MGA = mgaConnectionString.Substring("MGA=".Length);

            Assert.True(File.Exists(Path.GetFullPath(path_MGA)),
                        String.Format("{0} not found. Model import may have failed.", path_MGA));

            proj = new MgaProject();
            bool ro_mode;
            proj.Open("MGA=" + Path.GetFullPath(path_MGA), out ro_mode);
            proj.EnableAutoAddOns(true);
            mgaGateway = new MgaGateway(proj);
            proj.CreateTerritoryWithoutSink(out mgaGateway.territory);

            PerformInTransaction(delegate
            {
                var objAsm = proj.get_ObjectByPath("/@ComponentAssemblies/@Assembly");
                Assert.NotNull(objAsm);
                var ca = CyPhyClasses.ComponentAssembly.Cast(objAsm);
                Assert.NotNull(ca);

                exportedDesign = CyPhy2DesignInterchange.CyPhy2DesignInterchange.Convert(ca);

                var importer = new CyPhyDesignImporter.AVMDesignImporter(null, proj as IMgaProject, null);
                var objImported = importer.ImportDesign(exportedDesign, CyPhyDesignImporter.AVMDesignImporter.DesignImportMode.CREATE_CAS);
                Assert.NotNull(objImported);
                importedDesign = CyPhyClasses.ComponentAssembly.Cast(objImported.Impl);
            });

            System.IO.File.WriteAllText(Path.Combine(path_Test, "export.adm"),
                                        XSD2CSharp.AvmXmlSerializer.Serialize(exportedDesign));
        }

        private MgaGateway mgaGateway;
        public void PerformInTransaction(Action del)
        {
            mgaGateway.PerformInTransaction(() => del(), abort: false);
        }

        public avm.Design exportedDesign;
        public CyPhy.ComponentAssembly importedDesign;

        public void Dispose()
        {
            proj.Save();
            proj.Close();
            proj = null;
        }
    }

    public class PcbLayoutFixtureTest
    {
        [Fact]
        [Trait("Schematic", "Layout")]
        public void TestFixture()
        {
            using (var fixture = new PcbLayoutFixture()) { }
        }
    }

    public class PcbLayout : IUseFixture<PcbLayoutFixture>
    {
        #region Fixture
        PcbLayoutFixture fixture;
        public void SetFixture(PcbLayoutFixture data)
        {
            fixture = data;
        }
        #endregion

        #region Convenience Functions
        private MgaProject project
        {
            get
            {
                return fixture.proj;
            }
        }

        private String TestPath
        {
            get
            {
                return PcbLayoutFixture.path_Test;
            }
        }

        private avm.Design design
        {
            get
            {
                return fixture.exportedDesign;
            }
        }

        private avm.Container container
        {
            get
            {
                return design.RootContainer;
            }
        }

        private CyPhy.ComponentAssembly componentAssembly
        {
            get
            {
                return fixture.importedDesign;
            }
        }
        #endregion

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_HasExactConstraints()
        {
            var constraints = container.ContainerFeature.OfType<avm.schematic.eda.ExactLayoutConstraint>();
            Assert.Equal(4, constraints.Count());
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Exact1()
        {
            String nameComp = "C1a";
            double x = 1.1;
            double y = 2.2;
            var layer = avm.schematic.eda.LayerEnum.Top;
            var rot = avm.schematic.eda.RotationEnum.r0;

            Export_TestExactLayoutConstraint(nameComp, x, y, layer, rot);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Exact2()
        {
            String nameComp = "C1b";
            double x = 1.1;
            double y = 2.2;
            var layer = avm.schematic.eda.LayerEnum.Bottom;
            var rot = avm.schematic.eda.RotationEnum.r90;

            Export_TestExactLayoutConstraint(nameComp, x, y, layer, rot);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Exact3()
        {
            String nameComp = "C1c";
            double x = 1.1;
            double y = 2.2;
            var layer = avm.schematic.eda.LayerEnum.Bottom;
            var rot = avm.schematic.eda.RotationEnum.r180;

            Export_TestExactLayoutConstraint(nameComp, x, y, layer, rot);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Exact4()
        {
            String nameComp = "C1d";
            double x = 1.1;
            double y = 2.2;
            var layer = avm.schematic.eda.LayerEnum.Bottom;
            var rot = avm.schematic.eda.RotationEnum.r270;

            Export_TestExactLayoutConstraint(nameComp, x, y, layer, rot);
        }

        private void Export_TestExactLayoutConstraint(String nameComp, double x, double y, avm.schematic.eda.LayerEnum layer, avm.schematic.eda.RotationEnum rot)
        {
            fixture.PerformInTransaction(delegate
            {
                var comp = container.ComponentInstance.First(c => c.Name.Equals(nameComp));
                Assert.NotNull(comp);

                var constraints = container.ContainerFeature.OfType<avm.schematic.eda.ExactLayoutConstraint>()
                                                           .Where(c => c.ConstraintTarget.Contains(comp.ID));
                Assert.Equal(1, constraints.Count());

                var constraint = constraints.First();
                Assert.Equal(x, constraint.X);
                Assert.Equal(y, constraint.Y);
                Assert.Equal(layer, constraint.Layer);
                Assert.Equal(rot, constraint.Rotation);
            });
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_HasRangeConstraints()
        {
            var constraints = container.ContainerFeature.OfType<avm.schematic.eda.RangeLayoutConstraint>();
            Assert.Equal(4, constraints.Count());
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Range1()
        {
            var nameComp = "C2a";

            bool hasXRange = true;
            var xMin = 1.1;
            var xMax = 5.5;

            bool hasYRange = true;
            var yMin = 5.5;
            var yMax = 10.1;

            var layerRange = avm.schematic.eda.LayerRangeEnum.Either;

            Export_TestRangeLayoutConstraint(nameComp, hasXRange, xMin, xMax, hasYRange, yMin, yMax, layerRange);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Range2()
        {
            var nameComp = "C2b";

            bool hasXRange = true;
            var xMin = 1.1;
            var xMax = 5.5;

            bool hasYRange = false;
            var yMin = 5.5;
            var yMax = 10.1;

            var layerRange = avm.schematic.eda.LayerRangeEnum.Top;

            Export_TestRangeLayoutConstraint(nameComp, hasXRange, xMin, xMax, hasYRange, yMin, yMax, layerRange);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Range3()
        {
            var nameComp = "C2c";

            bool hasXRange = false;
            var xMin = 1.1;
            var xMax = 5.5;

            bool hasYRange = true;
            var yMin = 5.5;
            var yMax = 10.1;

            var layerRange = avm.schematic.eda.LayerRangeEnum.Bottom;

            Export_TestRangeLayoutConstraint(nameComp, hasXRange, xMin, xMax, hasYRange, yMin, yMax, layerRange);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Range4()
        {
            var nameComp = "C2d";

            bool hasXRange = false;
            var xMin = 0.0;
            var xMax = 0.0;

            bool hasYRange = false;
            var yMin = 0.0;
            var yMax = 0.0;

            var layerRange = avm.schematic.eda.LayerRangeEnum.Either;

            Export_TestRangeLayoutConstraint(nameComp, hasXRange, xMin, xMax, hasYRange, yMin, yMax, layerRange);
        }

        private void Export_TestRangeLayoutConstraint(string nameComp, bool hasXRange, double xMin, double xMax, bool hasYRange, double yMin, double yMax, avm.schematic.eda.LayerRangeEnum layerRange)
        {
            fixture.PerformInTransaction(delegate
            {
                var comp = container.ComponentInstance.First(c => c.Name.Equals(nameComp));
                Assert.NotNull(comp);

                var constraints = container.ContainerFeature.OfType<avm.schematic.eda.RangeLayoutConstraint>()
                                                            .Where(c => c.ConstraintTarget.Contains(comp.ID));
                Assert.Equal(1, constraints.Count());

                var constraint = constraints.First();

                if (hasXRange)
                {
                    Assert.Equal(xMin, constraint.XRangeMin);
                    Assert.Equal(xMax, constraint.XRangeMax);
                }

                if (hasYRange)
                {
                    Assert.Equal(yMin, constraint.YRangeMin);
                    Assert.Equal(yMax, constraint.YRangeMax);
                }

                Assert.Equal(layerRange, constraint.LayerRange);
            });
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_HasRelativeConstraints()
        {
            var constraints = container.ContainerFeature.OfType<avm.schematic.eda.RelativeLayoutConstraint>();
            Assert.Equal(2, constraints.Count());
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Relative1()
        {
            var nameCompConstrained = "C3a1";
            var nameCompOrigin = "C3a2";
            var xOffset = 5.1;
            var yOffset = 10.2;

            Export_TestRelativeLayoutConstraint(nameCompConstrained, nameCompOrigin, xOffset, yOffset);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Export_Relative2()
        {
            var nameCompConstrained = "C3b1";
            var nameCompOrigin = "C3b2";
            var xOffset = -5.1;
            var yOffset = -10.2;

            Export_TestRelativeLayoutConstraint(nameCompConstrained, nameCompOrigin, xOffset, yOffset);
        }

        private void Export_TestRelativeLayoutConstraint(string nameCompConstrained, string nameCompOrigin, double xOffset, double yOffset)
        {
            fixture.PerformInTransaction(delegate
            {
                var compConstrained = container.ComponentInstance.First(c => c.Name.Equals(nameCompConstrained));
                Assert.NotNull(compConstrained);

                var constraints = container.ContainerFeature.OfType<avm.schematic.eda.RelativeLayoutConstraint>()
                                                            .Where(c => c.ConstraintTarget.Contains(compConstrained.ID));
                Assert.Equal(1, constraints.Count());

                var constraint = constraints.First();

                var compOrigin = container.ComponentInstance.FirstOrDefault(ci => ci.ID.Equals(constraint.Origin));
                Assert.NotNull(compOrigin);
                Assert.Equal(nameCompOrigin, compOrigin.Name);

                Assert.Equal(xOffset, constraint.XOffset);
                Assert.Equal(yOffset, constraint.YOffset);
            });
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Exact1()
        {
            var nameComp = "C1a";
            var x = 1.1;
            var y = 2.2;
            var layer = 0;
            var rotation = 0;

            Import_TestExactLayoutConstraint(nameComp, x, y, layer, rotation);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Exact2()
        {
            var nameComp = "C1b";
            double x = 1.1;
            double y = 2.2;
            var layer = 1;
            var rotation = 1;

            Import_TestExactLayoutConstraint(nameComp, x, y, layer, rotation);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Exact3()
        {
            var nameComp = "C1c";
            var x = 1.1;
            var y = 2.2;
            var layer = 1;
            var rotation = 2;

            Import_TestExactLayoutConstraint(nameComp, x, y, layer, rotation);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Exact4()
        {
            var nameComp = "C1d";
            var x = 1.1;
            var y = 2.2;
            var layer = 1;
            var rotation = 3;

            Import_TestExactLayoutConstraint(nameComp, x, y, layer, rotation);
        }

        private void Import_TestExactLayoutConstraint(string nameComp, double x, double y, int layer, int rotation)
        {
            fixture.PerformInTransaction(delegate
            {
                var comp = componentAssembly.Children.ComponentRefCollection.FirstOrDefault(c => c.Name.Equals(nameComp));
                Assert.NotNull(comp);

                var constraints = comp.SrcConnections.ApplyExactLayoutConstraintCollection.Select(c => c.SrcEnds.ExactLayoutConstraint);
                Assert.Equal(1, constraints.Count());
                var constraint = constraints.First();

                Assert.Equal(x, double.Parse(constraint.Attributes.X));
                Assert.Equal(y, double.Parse(constraint.Attributes.Y));
                Assert.Equal(layer.ToString(), constraint.Attributes.Layer);
                Assert.Equal(rotation.ToString(), constraint.Attributes.Rotation);
            });
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Range1()
        {
            var nameComp = "C2a";

            var xRange = String.Format("{0}-{1}", 1.1f, 5.5f);
            var yRange = String.Format("{0}-{1}", 5.5f, 10.1f);

            var layerRange = "0-1";

            Import_TestRangeConstraint(nameComp, xRange, yRange, layerRange);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Range2()
        {
            var nameComp = "C2b";

            var xRange = String.Format("{0}-{1}", 1.1f, 5.5f);
            var yRange = "";

            var layerRange = "0";

            Import_TestRangeConstraint(nameComp, xRange, yRange, layerRange);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Range3()
        {
            var nameComp = "C2c";

            var xRange = "";
            var yRange = String.Format("{0}-{1}", 5.5f, 10.1f);

            var layerRange = "1";

            Import_TestRangeConstraint(nameComp, xRange, yRange, layerRange);
        }

        private void Import_TestRangeConstraint(string nameComp, string xRange, string yRange, string layerRange)
        {
            fixture.PerformInTransaction(delegate
            {
                var comp = componentAssembly.Children.ComponentRefCollection.FirstOrDefault(c => c.Name.Equals(nameComp));
                Assert.NotNull(comp);

                var constraints = comp.SrcConnections.ApplyRangeLayoutConstraintCollection.Select(c => c.SrcEnds.RangeLayoutConstraint);
                Assert.Equal(1, constraints.Count());
                var constraint = constraints.First();

                Assert.Equal(xRange, constraint.Attributes.XRange);
                Assert.Equal(yRange, constraint.Attributes.YRange);

                Assert.Equal(layerRange, constraint.Attributes.LayerRange);
            });
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Relative1()
        {
            var nameComp = "C3a1";
            var nameCompOrigin = "C3a2";
            double xOffset = 5.1;
            double yOffset = 10.2;

            Import_TestRelativeLayoutConstraint(nameComp, nameCompOrigin, xOffset, yOffset);
        }

        [Fact]
        [Trait("Schematic", "Layout")]
        public void Import_Relative2()
        {
            var nameComp = "C3b1";
            var nameCompOrigin = "C3b2";
            double xOffset = -5.1;
            double yOffset = -10.2;

            Import_TestRelativeLayoutConstraint(nameComp, nameCompOrigin, xOffset, yOffset);
        }

        private void Import_TestRelativeLayoutConstraint(string nameComp, string nameCompOrigin, double xOffset, double yOffset)
        {
            fixture.PerformInTransaction(delegate
            {
                var compTarget = componentAssembly.Children.ComponentRefCollection.FirstOrDefault(c => c.Name.Equals(nameComp));
                Assert.NotNull(compTarget);

                var constraints = compTarget.SrcConnections.ApplyRelativeLayoutConstraintCollection.Select(c => c.SrcEnds.RelativeLayoutConstraint);
                Assert.Equal(1, constraints.Count());
                var constraint = constraints.First();

                var compOrigin = constraint.SrcConnections.RelativeLayoutConstraintOriginCollection.First().SrcEnds.ComponentRef;
                Assert.Equal(nameCompOrigin, compOrigin.Name);

                Assert.Equal(xOffset, double.Parse(constraint.Attributes.XOffset));
                Assert.Equal(yOffset, double.Parse(constraint.Attributes.YOffset));
            });
        }

    }
}
