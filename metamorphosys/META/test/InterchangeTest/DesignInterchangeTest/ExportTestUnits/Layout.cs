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
using Xunit;
using GME.CSharp;
using GME.MGA;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace DesignExporterUnitTests
{
    public class LayoutFixture : ExporterFixture
    {
        public override String pathXME
        {
            get
            {
                return Path.Combine(META.VersionInfo.MetaPath,
                                    "test",
                                    "InterchangeTest",
                                    "DesignInterchangeTest",
                                    "ExportTestModels",
                                    "Layout",
                                    "Layout.xme");
            }
        }
    }

    internal static class Utils
    {
        public static void PerformInTransaction(this MgaProject project, MgaGateway.voidDelegate del)
        {
            var mgaGateway = new MgaGateway(project);
            project.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(del);
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

        private MgaProject proj { get { return fixture.proj; } }
        
        [Fact]
        public void Export()
        {
            avm.Design design = null;
            proj.PerformInTransaction(delegate
            {
                // Retrieve design contain as MgaObject
                MgaObject objDesignContainer = null;
                objDesignContainer = proj.get_ObjectByPath(@"DesignSpaces/DesignContainer");
                Assert.NotNull(objDesignContainer);

                // Cast as DesignContainer and run converter
                var designContainer = ISIS.GME.Dsml.CyPhyML.Classes.DesignContainer.Cast(objDesignContainer);
                design = CyPhy2DesignInterchange.CyPhy2DesignInterchange.Convert(designContainer);
            });
            Assert.NotNull(design);

            String pathXmlOut = Path.Combine(fixture.PathTest, "DesignContainer.adm");
            using (StreamWriter sw = new StreamWriter(pathXmlOut, false))
            {
                sw.Write(design.Serialize());
            }

            var checker = new LayoutDataChecker();
            var result = checker.Check(design.RootContainer);

            if (result.Any())
            {
                String msg = Environment.NewLine + 
                             String.Join(Environment.NewLine,
                                         result);
                Assert.True(false, msg);
            }
        }
    }

    internal class LayoutDataChecker
    {
        private ConcurrentBag<String> ObjectsMissingLayout;

        public IEnumerable<String> Check(avm.Container container)
        {
            ObjectsMissingLayout = new ConcurrentBag<String>();
            RecursivelyCheckForLayoutData(container);
            return ObjectsMissingLayout.AsEnumerable();
        }

        private void TestLayoutData(object obj, String path)
        {
            Boolean pass = true;
            var objType = obj.GetType();

            var xpos = objType.GetProperty("XPositionSpecified");
            Boolean? xposSpecified = xpos.GetValue(obj, null) as Boolean?;
            if (xposSpecified.HasValue == false || xposSpecified.Value == false)
            {
                pass = false;
            }

            var ypos = objType.GetProperty("YPositionSpecified");
            Boolean? yposSpecified = ypos.GetValue(obj, null) as Boolean?;
            if (yposSpecified.HasValue == false || yposSpecified.Value == false)
            {
                pass = false;
            }

            if (!pass)
            {
                ObjectsMissingLayout.Add(String.Format("{0} ({1}) doesn't contain X&Y layout data", path, objType.Name));
            }
        }

        private void RecursivelyCheckForLayoutData(avm.Container container, String path = null)
        {
            String containerPath = String.Join("/", path, container.Name);

            // Test this container for layout data
            if (false == String.IsNullOrWhiteSpace(path))
            {
                TestLayoutData(container, containerPath);
            }

            // Test child objects for layout data
            container.ComponentInstance.ForEach(x => TestLayoutData(x, String.Join("/", containerPath, x.Name)));
            container.Connector.ForEach(x => TestLayoutData(x, String.Join("/", containerPath, x.Name)));
            container.Formula.ForEach(x => TestLayoutData(x, String.Join("/", containerPath, x.Name)));
            container.Port.ForEach(x => TestLayoutData(x, String.Join("/", containerPath, x.Name)));
            container.Property.ForEach(x => TestLayoutData(x, String.Join("/", containerPath, x.Name)));

            // Recursively check child containers
            container.Container1
                     .AsParallel()
                     .ForAll(c => RecursivelyCheckForLayoutData(c, containerPath));
        }
    }
}
