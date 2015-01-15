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
using GME.MGA;
using System.IO;
using System.Reflection;
using GME.CSharp;
using ISIS.GME.Dsml.CyPhyML.Interfaces;

namespace DesignSpaceTest
{
    public class ToyDSFixture : IDisposable
    {
        public ToyDSFixture()
        {
            string connection;
            MgaUtils.ImportXMEForTest(
                Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).CodeBase.Substring("file:///".Length)),
                @"..\..\..\..\models\DesignSpace\ToyDS.xme"),
                out connection);

            Type type = Type.GetTypeFromProgID("Mga.MgaProject");
            proj = Activator.CreateInstance(type) as MgaProject;
            proj.OpenEx(connection, "CyPhyML", null);
        }

        public IMgaProject proj { get; private set; }

        public void Dispose()
        {
            proj.Save();
            proj.Close(true);
        }
    }

    public class DesertTest : IUseFixture<ToyDSFixture>
    {
        [Fact]
        void TestDesertAutomation()
        {
            var gateway = new MgaGateway(project);
            Type desertType = Type.GetTypeFromProgID("MGA.Interpreter.DesignSpaceHelper");
            var desert = (IMgaComponentEx)Activator.CreateInstance(desertType);

            MgaFCO currentobj = null;
            gateway.PerformInTransaction(() =>
            {
                currentobj = (MgaFCO)project.RootFolder.ObjectByPath["/@DesignSpaces/@DesignContainer"];
            });

            desert.Initialize(project);
            desert.InvokeEx(project, currentobj, null, 128);
            Configurations configs = null;
            gateway.PerformInTransaction(() =>
            {
                var configurations = ISIS.GME.Dsml.CyPhyML.Classes.DesignContainer.Cast(currentobj).Children.ConfigurationsCollection;
                configs = configurations.First();
                Assert.Equal(1, configurations.Count());
                Assert.Equal(2, configurations.First().Children.CWCCollection.Count());
            });

            Type caExporterType = Type.GetTypeFromProgID("MGA.Interpreter.CyPhyCAExporter");
            var caExporter = (IMgaComponentEx)Activator.CreateInstance(caExporterType);


            gateway.PerformInTransaction(() =>
                {
                    MgaFCOs selected = (MgaFCOs)Activator.CreateInstance(Type.GetTypeFromProgID("Mga.MgaFCOs"));
                    foreach (MgaFCO cwc in configs.Children.CWCCollection.Select(x => (MgaFCO)x.Impl))
                    {
                        selected.Append(cwc);
                    }

                    caExporter.Initialize(project);
                    caExporter.InvokeEx(project, selected[1].ParentModel as MgaFCO, selected, 128);
                    foreach (var cwc in configs.Children.CWCCollection)
                    {
                        Assert.Equal(1, cwc.DstConnections.Config2CACollection.Count());
                        var caConn = cwc.DstConnections.Config2CACollection.First();
                        // ((MgaModel)ca.Impl).GetDescendantFCOs(project.CreateFilter()).Count
                        var ca = ISIS.GME.Dsml.CyPhyML.Classes.ComponentAssemblyRef.Cast(caConn.DstEnd.Impl).Referred.ComponentAssembly;
                        Assert.Equal(1, ca.Children.ConnectorCollection.Count());
                    }
                });
        }

        private MgaProject project { get { return (MgaProject)fixture.proj; } }

        ToyDSFixture fixture;
        public void SetFixture(ToyDSFixture data)
        {
            fixture = data;
        }
    }
}
