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
using System.IO;
using META;

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2Schematic.Schematic
{
    class EdaVisitor : Visitor
    {
        private int netCount = 0;
        const float netLength = 2.54f;  // 0.1 inch = 2.54 mm
        const string netLayer = "91";
        const string nameLayer = "95";
        const string wireWidth = "0.3";
        const string labelSize = "1.27";

        private Dictionary<Port, Eagle.net> PortNetMap;

        private Eagle.eagle _eagle_obj;
        public Eagle.eagle eagle_obj {
            get
            {
                return _eagle_obj;
            }
            set
            {
                this._eagle_obj = value;
                this.schematic_obj = value.drawing.Item as Eagle.schematic;
            } 
        }
        private Eagle.schematic schematic_obj { get; set; }

        public EdaVisitor()
        {
            PortNetMap = new Dictionary<Port, Eagle.net>();
        }

        public override void visit(TestBench obj)
        {
            // Create a sheet for the testbench
            var sheet_obj = new Eagle.sheet();
            schematic_obj.sheets.sheet.Add(sheet_obj);
        }

        public override void visit(ComponentAssembly obj)
        {
            var layoutFile = (obj.Impl.Impl as GME.MGA.MgaFCO).RegistryValue["layoutFile"];
            if (layoutFile != null)
            {
                var pathLayoutFile = Path.Combine(obj.Impl.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE), layoutFile);
                var layoutParser = new Layout.LayoutParser(pathLayoutFile,
                    CodeGenerator.Logger)
                    {
                        parentInstanceGUID = (obj.Impl.Impl as GME.MGA.MgaFCO).
                        RegistryValue["Elaborator/InstanceGUID_Chain"]
                    };
                Logger.WriteDebug("Parent GUID : {0}", layoutParser.parentInstanceGUID);
                layoutParser.BuildMaps();
                CodeGenerator.preRouted.Add(obj, layoutParser);
            }
        }

        public override void visit(Component obj)
        {
            if (obj.Impl is Tonka.TestComponent)
                return;

            var parts = schematic_obj.parts;
            var instances = schematic_obj.sheets.sheet.FirstOrDefault().instances;
            var libraries = schematic_obj.libraries;

            var schObj = obj.Impl.Children.EDAModelCollection.FirstOrDefault();
            if (schObj == null) // no schematic model in this component, skip from generating 
                return;

            var part = new Eagle.part();
            part.name = obj.Name;
            var device = schObj.Attributes.Device;
            part.device = (device != null) ? device : "device-unknown";
            var deviceset = schObj.Attributes.DeviceSet;
            part.deviceset = (deviceset != null) ? deviceset : "deviceset-unknown";
            var libName = schObj.Attributes.Library;
            part.library = (String.IsNullOrWhiteSpace(libName) == false) ? libName : "library-noname";
            var parVal = obj.Parameters.Where(p => p.Name.Equals("value")).FirstOrDefault();
            if (parVal != null)
                part.value = parVal.Value;

            MergeLibrary(obj, obj.SchematicLib, libraries, libName);

            var devLib = (obj.SchematicLib != null) ? obj.SchematicLib.drawing.Item as Eagle.library : null;
            if (devLib != null)
            {
                var techs = devLib.devicesets.deviceset.SelectMany(p => p.devices.device).SelectMany(q => q.technologies.technology).Select(t => t.name);
                part.technology = techs.FirstOrDefault();
            }

            parts.part.Add(part);
            CodeGenerator.partComponentMap[part] = obj; // add to map
            CodeGenerator.componentPartMap[obj] = part; // add to reverse map

            if (devLib != null)
            {
                var gates = devLib.devicesets.deviceset.SelectMany(p => p.gates.gate);
                foreach (var gate in gates)
                {
                    var instance = new Eagle.instance();
                    instance.part = part.name;
                    instance.gate = gate.name;
                    double x = float.Parse(gate.x) + obj.CenterX;
                    double y = float.Parse(gate.y) + obj.CenterY;

                    instance.x = x.ToString("F2");
                    instance.y = y.ToString("F2");

                    instances.instance.Add(instance);
                }
            }
        }

        public override void visit(Port obj)
        {
            if (obj.Parent.Impl is Tonka.TestComponent)
                return;

            Logger.WriteDebug("CyPhySchematicVisitor::visit({0}, dest connections: {1})", 
                              obj.Name, 
                              obj.DstConnections.Count);

            if (PortNetMap.ContainsKey(obj))// port already mapped to a net object - no need to visit further
                return;
            if (obj.DstConnections.Count <= 0 && obj.SrcConnections.Count <= 0)  // no source and dest connections - skip this port
                return;

            var net_obj = new Eagle.net();
            net_obj.name = string.Format("N${0}", netCount++);
            visit(obj, net_obj);
            schematic_obj.sheets.sheet.FirstOrDefault().nets.net.Add(net_obj);    // relying that a sheet has been created already
        }

        private void visit(Port obj, Eagle.net net_obj)
        {
            if (!(obj.Parent.Impl is Tonka.TestComponent))
            {
                // create a segment for this object
                var segment_obj = new Eagle.segment();
                CreateWireSegment(obj, segment_obj);  // simple routing
                CreatePinRef(obj, segment_obj); // destination pin
                net_obj.segment.Add(segment_obj);
            }
            PortNetMap[obj] = net_obj;  // add to map

            var allPorts = 
                (from conn in obj.DstConnections select conn.DstPort).Union
                (from conn in obj.SrcConnections select conn.SrcPort);

            foreach (var port in allPorts) // visit sources
            {
                if (!PortNetMap.ContainsKey(port))
                    this.visit(port, net_obj);
            }
        }

        private void CreatePinRef(Port obj, Eagle.segment segment_obj)
        {
            var gate = obj.Impl.Attributes.EDAGate;
            var pinref_obj = new Eagle.pinref();
            pinref_obj.gate = (String.IsNullOrWhiteSpace(gate) == false) ? gate : "gate-unknown";
            pinref_obj.part = obj.Parent.Name;
            pinref_obj.pin = obj.Name;
            segment_obj.Items.Add(pinref_obj);
        }

        private void CreateWireSegment(Port port, Eagle.segment segment_obj)
        {
            // create two short wire segments: 1. from src pin to, and 2. to dst pin
            // TODO: create vertical segments for vertical pins (or rotated symbols)
            var wire_obj = new Eagle.wire();
            var rot = port.Impl.Attributes.EDASymbolRotation;
            double x1 = port.CanvasX;
            double y1 = port.CanvasY;
            double x2 = x1;
            double y2 = y1;
            if (rot.Equals("R90") || rot.Equals("90"))
                y2 -= netLength; // 90 pointing down
            else if (rot.Equals("R270") || rot.Equals("270"))
                y2 += netLength; // 270 pointing up
            else if (rot.Equals("R180") || rot.Equals("180"))
                x2 += netLength; // 180 going right
            else
                x2 -= netLength; // 0 going left

            wire_obj.x1 = x1.ToString("F2");
            wire_obj.y1 = y1.ToString("F2");
            wire_obj.x2 = x2.ToString("F2");
            wire_obj.y2 = y2.ToString("F2");
            wire_obj.layer = netLayer;
            wire_obj.width = wireWidth;
            segment_obj.Items.Add(wire_obj);

            var label_obj = new Eagle.label();
            label_obj.x = wire_obj.x2;
            label_obj.y = wire_obj.y2;
            label_obj.size = labelSize;
            label_obj.layer = nameLayer;
            segment_obj.Items.Add(label_obj);
        }

        private void MergeLibrary(Component obj, Eagle.eagle eagleSrc, Eagle.libraries libsDst, string libName)
        {
            var libSrc = eagleSrc.drawing.Item as Eagle.library;
            if (libSrc == null)
            {
                Logger.WriteWarning("No Schematic Library for Component: <a href=\"MGA:{0}\">{1}</a>", obj.Impl.ID, obj.Impl.Name);
                return;
            }
            if (String.IsNullOrWhiteSpace(libName))
                libName = "library-noname";
            var libDst = libsDst.library.Where(l => l.name.Equals(libName)).FirstOrDefault();
            if (libDst == null)
            {
                libSrc.name = libName;
                libsDst.library.Add(libSrc);
            }
            else
            {
                // keep the description
                // add to packages, add to symbols, add to device-sets/devices
                foreach (Eagle.package pkg in libSrc.packages.package)
                {
                    if (libDst.packages.package.Where(p => p.name.Equals(pkg.name)).Count() == 0)
                        libDst.packages.package.Add(pkg);
                }
                foreach (Eagle.symbol sym in libSrc.symbols.symbol)
                {
                    if (libDst.symbols.symbol.Where(s => s.name.Equals(sym.name)).Count() == 0)
                        libDst.symbols.symbol.Add(sym);
                }
                foreach (Eagle.deviceset dset in libSrc.devicesets.deviceset)
                {
                    if (libDst.devicesets.deviceset.Where(d => d.name.Equals(dset.name)).Count() == 0)
                        libDst.devicesets.deviceset.Add(dset);
                    else
                    {
                        var dstDset = libDst.devicesets.deviceset.Where(d => d.name.Equals(dset.name)).FirstOrDefault();
                        foreach (Eagle.device dev in dset.devices.device)
                        {
                            if (dstDset.devices.device.Where(dd => dd.name.Equals(dev.name)).Count() == 0)
                                dstDset.devices.device.Add(dev);
                        }
                    }
                }

            }

        }
    }
}
