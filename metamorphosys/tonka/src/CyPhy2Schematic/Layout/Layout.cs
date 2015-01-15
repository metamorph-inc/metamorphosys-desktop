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
using Newtonsoft.Json;

using CyPhyGUIs;
using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;

using Eagle = CyPhy2Schematic.Schematic.Eagle;
using CyPhy2Schematic.Schematic;
using LayoutJson;

namespace CyPhy2Schematic.Layout
{
    class ExactConstraint : Constraint
    {
        public double? x { get; set; }
        public double? y { get; set; }
        public int? layer { get; set; }
        public int? rotation { get; set; }
    }

    class RangeConstraint : Constraint
    {
        public string x { get; set; }
        public string y { get; set; }
        public string layer { get; set; }
    }

    class RelativeConstraint : Constraint
    {
        public double? x { get; set; }
        public double? y { get; set; }
        public int pkg_idx { get; set; }
    }

    class LayoutGenerator
    {
        public LayoutJson.Layout boardLayout { get; set; }
        public GMELogger Logger { get; set; }
        public Dictionary<Package, Eagle.part> pkgPartMap { get; set; }
        public Dictionary<Eagle.part, Package> partPkgMap { get; set; }

        public Dictionary<ComponentAssembly, Package> preroutedPkgMap { get; set; }

        private string[] LayersForBoundingBoxComputation = new string[]
        {
            "17",  // Pads
            "21",  // tPlace  <-- top silkscreen:  may be too conservative 
            "22",  // bPlace  <-- bot silkscreen:  may be too conservative
            "29",  // tStop
            "30",  // bStop
            "31",  // tCream
            "32",  // bCream
            "39",  // tKeepout
            "40",  // bKeepout
            "44",  // Drills
            "45"   // Holes
        };
        
        public LayoutGenerator(Eagle.schematic sch_obj, TestBench tb_obj, GMELogger inLogger)
        {
            this.Logger = inLogger;
            this.pkgPartMap = new Dictionary<Package, Eagle.part>();
            this.partPkgMap = new Dictionary<Eagle.part, Package>();
            this.preroutedPkgMap = new Dictionary<ComponentAssembly, Package>();

            boardLayout = new LayoutJson.Layout();
            var bw = tb_obj.Parameters.Where(p => p.Name.Equals("boardWidth")).FirstOrDefault();
            var bh = tb_obj.Parameters.Where(p => p.Name.Equals("boardHeight")).FirstOrDefault();

            try
            {
                boardLayout.boardWidth = (bw != null) ? Convert.ToDouble(bw.Value) : 40.0;
                boardLayout.boardHeight = (bh != null) ? Convert.ToDouble(bh.Value) : 40.0;
            }
            catch (FormatException ex)
            {
                Logger.WriteWarning("Exception while reading board dimensions: {0}", ex.Message);
            }
            boardLayout.numLayers = 2;
            {
                // Look to see if there is a PCB component in the top level assembly 
                var compImpl = tb_obj.ComponentAssemblies.SelectMany(c => c.ComponentInstances).Select(i => i.Impl)
                    .Where(j => (j as Tonka.Component).Attributes.Classifications
                        .Contains("pcb_board")).FirstOrDefault();
                // Look to see if it has a resource labeled 'BoardTemplate'
                var comp = (compImpl != null) ? compImpl as Tonka.Component : null;
                var btRes = (comp != null) ? comp.Children.ResourceCollection.Where(r =>
                    r.Name.ToUpper().Contains("BOARDTEMPLATE")).FirstOrDefault() : null;
                if (btRes != null)
                {
                    var btPath = Path.Combine(comp.Attributes.Path, btRes.Attributes.Path);
                    boardLayout.boardTemplate = Path.GetFileName(btPath);
                }
            }

            // the prior code looks up for a boardTemplate file associated with a PCB component
            // the users can override it with a boardTemplate file specified as a testbench parameter
            {
                var bt = tb_obj.Parameters.Where(p => p.Name.Equals("boardTemplate")).FirstOrDefault();
                // if its a file path - we only want to pass the file name to board synthesis
                if (bt != null)
                    try
                    {
                        boardLayout.boardTemplate = Path.GetFileName(bt.Value);
                    }
                    catch (System.ArgumentException ex)
                    {
                        CodeGenerator.Logger.WriteError("Error extracting boardTemplate filename: {0}", ex.Message);
                    }
            }
            // the users also can specify a designRule file as a testbench parameter
            {
                var dr = tb_obj.Parameters.Where(p => p.Name.Equals("designRules")).FirstOrDefault();
                if (dr != null)
                    try
                    {
                        boardLayout.designRules = Path.GetFileName(dr.Value);
                    }
                    catch (System.ArgumentException ex)
                    {
                        CodeGenerator.Logger.WriteError("Error extracting designRules filename: {0}", ex.Message);
                    }
            }

            boardLayout.packages = new List<Package>();
            int pkg_idx = 0;

            // we want to handle prerouted assemblies as follows
            // 1) all parts that are part of the pre-routed assembly are tagged with a 'virtual' spaceClaim part
            // 2) we add their absolute location (in the subckt frame of refernece) in the output json
            // 3) we create virtual spaceClaim parts for pre-routed subcircuits

            // 4) all nets belonging to the pre-routed subcircuit are tagged with the spaceClaim part
            // 5) these nets are added to json with absolute location (same as parts above)

            foreach (var ca in tb_obj.ComponentAssemblies)
            {
                pkg_idx = HandlePreRoutedAsm(ca, pkg_idx);
            }

            // compute part dimensions from 
            foreach (var part in sch_obj.parts.part)
            {
                var dev = sch_obj.libraries.library.Where(l => l.name.Equals(part.library)).
                    SelectMany(l => l.devicesets.deviceset).Where(ds => ds.name.Equals(part.deviceset)).
                    SelectMany(ds => ds.devices.device).Where(d => d.name.Equals(part.device)).FirstOrDefault();
                var spkg = (dev != null)
                           ? sch_obj.libraries.library.Where(l => l.name.Equals(part.library))
                                    .SelectMany(l => l.packages.package).Where(p => p.name.Equals(dev.package))
                                    .FirstOrDefault()
                           : null;

                Package pkg = new Package();
                pkg.name = part.name;
                pkg.pkg_idx = pkg_idx++;

                if (dev == null || spkg == null)
                {
                    // emit warning
                    Logger.WriteWarning("Unable to get package size for part - layout/chipfit results may be inaccurate: {0}", part.name);
                }
                else
                {
                    pkg.package = spkg.name;        // note that the eagle package information may be incomplete - should really have curated version from CyPhy

                    #region ComputePackageSize

                    var minX = Double.MaxValue;
                    var minY = Double.MaxValue;
                    var maxX = Double.MinValue;
                    var maxY = Double.MinValue;

                    foreach (Eagle.wire wire in spkg.Items.Where(s => s is Eagle.wire))
                    {
                        if (wire == null) continue;

                        if (!LayersForBoundingBoxComputation.Any(wire.layer.Contains))
                            continue;

                        var x1 = Convert.ToDouble(wire.x1);
                        var x2 = Convert.ToDouble(wire.x2);
                        var y1 = Convert.ToDouble(wire.y1);
                        var y2 = Convert.ToDouble(wire.y2);
                        if (x1 < minX) minX = x1;
                        if (x2 < minX) minX = x2;
                        if (x1 > maxX) maxX = x1;
                        if (x2 > maxX) maxX = x2;
                        if (y1 < minY) minY = y1;
                        if (y2 < minY) minY = y2;
                        if (y1 > maxY) maxY = y1;
                        if (y2 > maxY) maxY = y2;
                    }
                    foreach (Eagle.pad pad in spkg.Items.Where(s => s is Eagle.pad))
                    {
                        if (pad == null) continue;
                        var pad_num = pad.name;
                        var x = Convert.ToDouble(pad.x);
                        var y = Convert.ToDouble(pad.y);
                        var drill = Convert.ToDouble(pad.drill);
                        var dia = Convert.ToDouble(pad.diameter);
                        var shape = pad.shape;  // enum padShape {round, octagon, @long, offset}
                        var rot = pad.rot;      // { R90, R180, R270, ...}  
                        var r = 0.0;
                        if (dia == 0.0)  // JS: Workaround for no diameter present, estimate dia to be 2x drill size 
                        {
                            dia = drill * 2.0;
                        }

                        if (shape == Eagle.padShape.@long)
                        {
                            // TODO: consider PAD rotation; for now, consider long pads are 2x diameter.

                            dia *= 2.0;  // diameter value from package desc is for the "short" side, for max calc, consider long side is 2x (by inspection)
                        }
                        r = dia / 2.0;

                        if ((x - r) < minX) minX = x - r;
                        if ((x + r) > maxX) maxX = x + r;
                        if ((y - r) < minY) minY = y - r;
                        if ((y + r) > maxY) maxY = y + r;
                    }
                    foreach (Eagle.circle circle in spkg.Items.Where(s => s is Eagle.circle))
                    {
                        if (circle == null) continue;
                        var x = Convert.ToDouble(circle.x);
                        var y = Convert.ToDouble(circle.y);
                        var r = Convert.ToDouble(circle.radius);
                        if ((x - r) < minX) minX = x - r;
                        if ((x + r) > maxX) maxX = x + r;
                        if ((y - r) < minY) minY = y - r;
                        if ((y + r) > maxY) maxY = y + r;
                    }
                    foreach (Eagle.smd smd in spkg.Items.Where(s => s is Eagle.smd))
                    {
                        if (smd == null) continue;
                        // SKN: after intense research on eagle, it seems that the way SMD pads are placed is as follows
                        // dx - dy tells the length of the pad, while the x is the center point of the SMD pad

                        var x = Convert.ToDouble(smd.x);
                        var y = Convert.ToDouble(smd.y);
                        var ddx = Convert.ToDouble(smd.dx);
                        var ddy = Convert.ToDouble(smd.dy);
                        var dx = ddx;  // should be /2??
                        var dy = ddy;
                        if (smd.rot.Equals("R90") || smd.rot.Equals("R270"))
                        {       // flip dx and dy if there is rotation
                            dx = ddy;
                            dy = ddx;
                        }
                        var x1 = x - dx / 2.0;
                        var x2 = x + dx / 2.0;
                        var y1 = y - dy / 2.0;
                        var y2 = y + dy / 2.0;
                        if (x1 < minX) minX = x1;
                        if (x2 > maxX) maxX = x2;
                        if (y1 < minY) minY = y1;
                        if (y2 > maxY) maxY = y2;
                    }
                    pkg.width = maxX - minX;
                    pkg.height = maxY - minY;
                    pkg.originX = Math.Floor(10.0 * (maxX + minX) / 2.0) / 10.0;
                    pkg.originY = Math.Floor(10.0 * (maxY + minY) / 2.0) / 10.0;

                    #endregion

                    // emit component ID for locating components in CAD assembly
                    if (CodeGenerator.partComponentMap.ContainsKey(part))
                    {
                        var comp_obj = CodeGenerator.partComponentMap[part];
                        var comp = comp_obj.Impl as Tonka.Component;

                        // emit component ID for locating components in CAD assembly
                        pkg.ComponentID = comp.Attributes.InstanceGUID;

                        Boolean isMultiLayer = comp.Children.EDAModelCollection.
                            Any(e => e.Attributes.HasMultiLayerFootprint);
                        if (isMultiLayer)
                            pkg.multiLayer = true;

                        #region ExactConstraints
                        // exact constraints related to this part
                        var exactCons =
                            from conn in comp.SrcConnections.ApplyExactLayoutConstraintCollection
                            select conn.SrcEnds.ExactLayoutConstraint;

                        foreach (var c in exactCons)
                        {
                            var pcons = new ExactConstraint();
                            pcons.type = "exact";
                            if (!c.Attributes.X.Equals(""))
                                pcons.x = Convert.ToDouble(c.Attributes.X);
                            if (!c.Attributes.Y.Equals(""))
                                pcons.y = Convert.ToDouble(c.Attributes.Y);
                            if (!c.Attributes.Layer.Equals(""))
                                pcons.layer = Convert.ToInt32(c.Attributes.Layer);
                            if (!c.Attributes.Rotation.Equals(""))
                                pcons.rotation = Convert.ToInt32(c.Attributes.Rotation);
                            if (pkg.constraints == null)
                                pkg.constraints = new List<Constraint>();
                            pkg.constraints.Add(pcons);
                        }
                        #endregion

                        #region RangeConstraints
                        // range constraints related to this part
                        var rangeCons =
                            from conn in comp.SrcConnections.ApplyRangeLayoutConstraintCollection
                            select conn.SrcEnds.RangeLayoutConstraint;
                        foreach (var c in rangeCons)
                        {
                            var pcons = new RangeConstraint();
                            pcons.type = "range";
                            if (!c.Attributes.XRange.Equals(""))
                                pcons.x = c.Attributes.XRange;
                            if (!c.Attributes.YRange.Equals(""))
                                pcons.y = c.Attributes.YRange;
                            if (!c.Attributes.LayerRange.Equals(""))
                                pcons.layer = c.Attributes.LayerRange;
                            if (pkg.constraints == null)
                                pkg.constraints = new List<Constraint>();
                            pkg.constraints.Add(pcons);
                        }
                        #endregion

                        #region PreRoutedAssemblyPart
                        // now handle if this component is part of a pre-routed sub-ckt
                        if (CodeGenerator.preRouted.ContainsKey(comp_obj.Parent))
                        {
                            var layoutParser = CodeGenerator.preRouted[comp_obj.Parent];
                            var prePkg = layoutParser.componentPackageMap[comp_obj];
                            pkg.x = prePkg.x;
                            pkg.y = prePkg.y;
                            pkg.rotation = prePkg.rotation;
                            pkg.RelComponentID = (comp_obj.Parent.Impl.Impl as GME.MGA.MgaFCO)
                                .RegistryValue["Elaborator/InstanceGUID_Chain"];
                            pkg.doNotPlace = true;
                        }
                        #endregion
                    }
                }
                pkgPartMap.Add(pkg, part);  // add to map
                partPkgMap.Add(part, pkg);
                boardLayout.packages.Add(pkg);
            }



            #region AddSignalsToBoardLayout
            boardLayout.signals = new List<Signal>();
            foreach (var net in sch_obj.sheets.sheet.FirstOrDefault().nets.net)
            {
                // dump pre-routed signals to board file
                var sig = new Signal();
                sig.name = net.name;
                sig.pins = new List<Pin>();

                Signal preRouted = null;
                string preRoutedAsmID = null; // ID of the parent pre-routed assembly
                string preRoutedAsm = null; // name the parent pre-routed assembly
                bool onlyOnePreroute = true;

                foreach (var seg in net.segment.SelectMany(s => s.Items).Where(s => s is Eagle.pinref))
                {
                    bool isPrerouted = false;

                    var pr = seg as Eagle.pinref;
                    var pin = new Pin();
                    pin.name = pr.pin;
                    pin.gate = pr.gate;
                    pin.package = pr.part.ToUpper();

                    // find package and pad
                    var part = sch_obj.parts.part.Where(p => p.name.Equals(pr.part)).FirstOrDefault();

                    var dev = (part != null) ?
                        sch_obj.libraries.library.Where(l => l.name.Equals(part.library)).
                        SelectMany(l => l.devicesets.deviceset).Where(ds => ds.name.Equals(part.deviceset)).
                        SelectMany(ds => ds.devices.device).Where(d => d.name.Equals(part.device)).FirstOrDefault()
                        : null;
                    var pad = (dev != null) ?
                        dev.connects.connect.Where(c => c.gate.Equals(pr.gate) && c.pin.Equals(pr.pin)).Select(c => c.pad).FirstOrDefault()
                        : null;
                    if (pad != null)
                        pin.pad = pad;

                    // check for preroutes - all pins in this signal should be in the prerouted net, otherwise reject it
                    if (part != null && CodeGenerator.partComponentMap.ContainsKey(part))
                    {
                        var comp_obj = CodeGenerator.partComponentMap[part];
                        if (CodeGenerator.preRouted.ContainsKey(comp_obj.Parent)) // is the parent assembly prerouted
                        {
                            Logger.WriteDebug("Net {0} in Prerouted Asm {1}", net.name, comp_obj.Parent.Name);

                            var layoutParser = CodeGenerator.preRouted[comp_obj.Parent];
                            // find the name/gate matching port in schematic domain model
                            var sch = comp_obj.Impl.Children.SchematicModelCollection.FirstOrDefault();

                            var port = (sch != null) ?
                                sch.Children.SchematicModelPortCollection.
                                Where(p => p.Attributes.EDAGate == pin.gate && p.Name == pin.name).
                                FirstOrDefault()
                                : null;

                            // find the buildPort
                            var buildPort = port != null && CyPhyBuildVisitor.Ports.ContainsKey(port.ID) ?
                                CyPhyBuildVisitor.Ports[port.ID] : null;


                            if (buildPort != null && layoutParser.portTraceMap.ContainsKey(buildPort))
                            {
                                isPrerouted = true;
                                Logger.WriteDebug("Found build Port and Associated Trace");

                                if (preRouted == null)
                                {
                                    preRouted = layoutParser.portTraceMap[buildPort];
                                    preRoutedAsmID = (comp_obj.Parent.Impl.Impl as GME.MGA.MgaFCO)
                                        .RegistryValue["Elaborator/InstanceGUID_Chain"];
                                    preRoutedAsm = comp_obj.Parent.Name;
                                }
                                else if (preRouted != layoutParser.portTraceMap[buildPort])
                                    isPrerouted = false;
                            }
                        }
                    }

                    onlyOnePreroute = onlyOnePreroute && isPrerouted;

                    // add pin to list of pins for this signal
                    sig.pins.Add(pin);
                }

                // if pre-routed then copy wires 
                if (onlyOnePreroute && preRouted != null)
                {
                    Logger.WriteInfo("Prerouted net {0}, from assembly {1}, originally as {2}", net.name, preRoutedAsm, preRouted.name);
                    sig.wires = new List<Wire>();
                    sig.vias = new List<Via>();
                    sig.RelComponentID = preRoutedAsmID;
                    foreach (var w in preRouted.wires)
                        sig.wires.Add(w);
                    foreach (var v in preRouted.vias)
                        sig.vias.Add(v);
                }

                boardLayout.signals.Add(sig);
            }
            #endregion

            #region AddRelativeConstraintsToBoardLayout
            // now process relative constraints - they require that all parts be mapped to packages already
            for (int i = 0; i < boardLayout.packages.Count; i++)
            {
                var pkg = boardLayout.packages[i];
                if (!pkgPartMap.ContainsKey(pkg))
                    continue;

                var part = pkgPartMap[pkg];
                var comp = CodeGenerator.partComponentMap.ContainsKey(part) ?
                    CodeGenerator.partComponentMap[part] : null;
                var impl = comp != null ? comp.Impl as Tonka.Component : null;
                var relCons =
                    from conn in impl.SrcConnections.ApplyRelativeLayoutConstraintCollection
                    select conn.SrcEnds.RelativeLayoutConstraint;

                foreach (var c in relCons)
                {
                    var pcons = new RelativeConstraint();
                    pcons.type = "relative-pkg";
                    if (!c.Attributes.XOffset.Equals(""))
                        pcons.x = Convert.ToDouble(c.Attributes.XOffset);
                    if (!c.Attributes.YOffset.Equals(""))
                        pcons.y = Convert.ToDouble(c.Attributes.YOffset);
                    // find origin comp
                    var origCompImpl = (from conn in c.SrcConnections.RelativeLayoutConstraintOriginCollection
                                        select conn.SrcEnds.Component).FirstOrDefault();
                    var origComp =
                        ((origCompImpl != null) && CyPhyBuildVisitor.Components.ContainsKey(origCompImpl.ID)) ?
                        CyPhyBuildVisitor.Components[origCompImpl.ID] :
                        null;
                    var origPart =
                        ((origComp != null) && CodeGenerator.componentPartMap.ContainsKey(origComp)) ?
                        CodeGenerator.componentPartMap[origComp] :
                        null;
                    var origPkg = ((origPart != null) && partPkgMap.ContainsKey(origPart)) ?
                        partPkgMap[origPart] :
                        null;
                    pcons.pkg_idx = origPkg.pkg_idx.Value;
                    if (pkg.constraints == null)
                        pkg.constraints = new List<Constraint>();
                    pkg.constraints.Add(pcons);
                    boardLayout.packages[i] = pkg;
                }

            }
            #endregion

        }

        public int HandlePreRoutedAsm(ComponentAssembly obj, int pkg_idx)
        {
            if (CodeGenerator.preRouted.ContainsKey(obj))
            {
                var layoutBox = (obj.Impl.Impl as GME.MGA.MgaFCO).RegistryValue["layoutBox"];
                // format x1,y1,w1,h1; x2,y2,w2,h2; x3,y3,w3,h3
                // NOTE: if there are multiple bounding boxes 
                // - we expect the first one to be at the origin of the reference frame
                // - this minor limitation would be easy to address later by adding code to identify 
                // - the bounding box at origin even if its not the first one

                var bBoxs = layoutBox.Split(';');
                int bbidx = 0;
                int origPkgIdx = 0;
                foreach (var bbox in bBoxs)
                {
                    Package pkg = new Package();
                    pkg.pkg_idx = pkg_idx++;
                    pkg.name = obj.Name;
                    pkg.package = "__spaceClaim__";
                    pkg.ComponentID = (obj.Impl.Impl as GME.MGA.MgaFCO)
                        .RegistryValue["Elaborator/InstanceGUID_Chain"];
                    var pts = bbox.Split(',');
                    if (pts.Length >= 4)
                    {
                        double d = Double.NaN;
                        if (Double.TryParse(pts[0], out d))
                            pkg.x = d;
                        if (Double.TryParse(pts[1], out d))
                            pkg.y = d;
                        if (Double.TryParse(pts[2], out d))
                            pkg.width = d;
                        if (Double.TryParse(pts[3], out d))
                            pkg.height = d;
                    }
                    if (bbidx == 0)
                    {
                        preroutedPkgMap.Add(obj, pkg);
                        origPkgIdx = (int)pkg.pkg_idx;
                    }
                    else
                    {
                        pkg.ComponentID += "." + bbidx.ToString();
                        // add constraint relative to first bounding box
                        var pcons = new RelativeConstraint();
                        pcons.type = "relative-pkg";
                        pcons.x = pkg.x;
                        pcons.y = pkg.y;
                        pcons.pkg_idx = origPkgIdx;
                        if (pkg.constraints == null)
                            pkg.constraints = new List<Constraint>();
                        pkg.constraints.Add(pcons);
                    }

                    boardLayout.packages.Add(pkg);
                    bbidx++;
                }
                // TBD SKN figure out relative constraint between multiple bboxes
            }
            foreach (var ca in obj.ComponentAssemblyInstances)
            {
                pkg_idx = HandlePreRoutedAsm(ca, pkg_idx);
            }
            return pkg_idx;
        }

        public void Generate(string layoutFile)
        {
            StreamWriter writer = new StreamWriter(layoutFile);
            string sjson = JsonConvert.SerializeObject(boardLayout, Formatting.Indented, 
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore }) ;
            writer.Write(sjson);
            writer.Close();
        }

    }

    public class LayoutParser
    {
        public Dictionary<Port, Signal> portTraceMap { get; set; }
        public Dictionary<Component, Package> componentPackageMap { get; set; }

        public CodeGenerator.Mode mode { get; set; }
        public string parentInstanceGUID { get; set; }

        private GMELogger gmeLogger;
        private string layoutFile;

        public LayoutParser(string inFile, GMELogger inLogger) 
        {
            mode = CodeGenerator.Mode.EDA;      // default mode
            gmeLogger = inLogger;
            layoutFile = inFile;
            portTraceMap = new Dictionary<Port, Signal>();
            componentPackageMap = new Dictionary<Component, Package>();
        }

        public void BuildMaps()
        {
            var layout = Parse(layoutFile);
            var packageComponent = new Dictionary<string, Component>();

            // the parsing here basically maps components from the read in json file to Component 
            // objects in build network
            // and it maps the nets (signal) to:  a) schematic ports, and b) spice ports 

            foreach (var p in layout.packages)  // populate package name --> component map
            {
                var pguid = parentInstanceGUID == null ? p.ComponentID :
                    parentInstanceGUID + p.ComponentID;
                if (CyPhyBuildVisitor.ComponentInstanceGUIDs.ContainsKey(pguid))
                {
                    var comp = CyPhyBuildVisitor.ComponentInstanceGUIDs[pguid];
                    packageComponent.Add(p.name.ToUpper(), comp);
                    componentPackageMap.Add(comp, p);
                }
                else
                    gmeLogger.WriteError("Parsing Layout Json: Instance GUID {0} not found in model",
                        pguid);
            }

            // from nets --> pins --> packages --> ComponentID --> components --> ports
            foreach (var n in layout.signals)
            {
                foreach (var pin in n.pins)
                {
                    if (packageComponent.ContainsKey(pin.package))
                    {
                        var comp = packageComponent[pin.package];
                        var sch = comp.Impl.Children.SchematicModelCollection.FirstOrDefault();

                        // find the name/gate matching port in schematic domain model
                        var port = (sch != null) ?
                            sch.Children.SchematicModelPortCollection.
                            Where(p => p.Attributes.EDAGate == pin.gate && p.Name == pin.name).
                            FirstOrDefault()
                            : null;

                        if (mode == CodeGenerator.Mode.EDA) // if we are doing EDA then just map to sch domain ports
                            MapPortToNet(port, n);
                        else // SPICE_SI mode -- map to spice ports
                        {
                            // find the associated port in the component with the schematic port
                            foreach (var compPort in port.DstConnections.PortCompositionCollection.
                                Select(p => p.DstEnd).
                                Union(port.SrcConnections.PortCompositionCollection.Select(p => p.SrcEnd)))
                            {
                                // cast 
                                var compSchPort = compPort != null ? compPort as Tonka.SchematicModelPort : null;
                                // from the component port find the associated spice port
                                var spicePort = compSchPort != null ?
                                compSchPort.DstConnections.PortCompositionCollection.Select(p => p.DstEnd).
                                    Union(compSchPort.SrcConnections.PortCompositionCollection.Select(p => p.SrcEnd)).
                                    Where(q => q.ParentContainer is Tonka.SPICEModel).FirstOrDefault() : null;

                                MapPortToNet(spicePort as Tonka.SchematicModelPort, n);
                            }
                        }
                    }
                    else
                        gmeLogger.WriteError("Package {0} not found in Map, problem with the layout.json file",
                            pin.package);
                }
            }
        }

        public void MapPortToNet(Tonka.SchematicModelPort port, Signal n)
        {
            // get the build port for this schematic domain port & store the net with the build port
            var buildPort = port != null &&
                CyPhyBuildVisitor.Ports.ContainsKey(port.ID) ?
                CyPhyBuildVisitor.Ports[port.ID] : null;

            // now remember this net with the spice-build port 
            // - we will use it when building the spice model
            if (buildPort != null && !portTraceMap.ContainsKey(buildPort))
                portTraceMap.Add(buildPort, n);

            if (CodeGenerator.verbose)
            {
                if (buildPort == null)
                    gmeLogger.WriteWarning("No Matching Build Port  for {0}, ignoring trace", port.Name);
                else
                    gmeLogger.WriteInfo("Mapping Build Port {0} to Net {1}", buildPort.Name, n.name);
            }

        }
       

        public LayoutJson.Layout Parse(string layoutFile)
        {
            StreamReader reader = new StreamReader(layoutFile);
            string sjson = reader.ReadToEnd();
            LayoutJson.Layout layout = JsonConvert.DeserializeObject<LayoutJson.Layout>(sjson);
            return layout;
        }

    }
}
