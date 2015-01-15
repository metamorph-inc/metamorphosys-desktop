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
using System.Text.RegularExpressions;
using System.IO;
using Eagle = CyPhy2Schematic.Schematic.Eagle;
using Newtonsoft.Json;
using LayoutJson;


namespace BoardSynthesis
{

    class Program
    {
        const string ATTRIB_LAYER = "27";
        const string BOARD_LAYER = "20";

        static void Main(string[] args)
        {
            #region ProcessArguments
            // need schematic input file / layout json input file name / output board file name as command line args
            if (args.Length < 2)
            {
                Console.WriteLine("usage: BoardSynthesis <schema.sch> <layout.json> [-r]");
                Console.WriteLine("     Input: <schema.sch>, <layout.json>;  Output: <schema.brd>");
                Console.WriteLine("[-r] Input: <schema.sch> (implicit schema.brd), <layout.json>; Output: <layout.json>");
                return;
            }
            var schematicFile = args[0];
            var layoutFile = args[1];
            var boardFile = schematicFile.Replace(".sch", ".brd");
            var programMode = 0;   // forward
            if (args.Length == 3 && args[2].CompareTo("-r") == 0)
            {
                programMode = 1;
            }
            #endregion

            #region LoadSchematic
            // load schematic file
            Eagle.eagle schematic = null;
            try
            {
                schematic = Eagle.eagle.LoadFromFile(schematicFile);
                Console.WriteLine("Parsed Schematic File : " + schematicFile);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR Reading Schematic XML: " + e.Message);
                System.Environment.Exit(-1);
            }
            #endregion

            #region LoadLayout
            // load layout file
            Layout boardLayout = null;
            Dictionary<string, Package> packageMap = new Dictionary<string, Package>
                (StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, Signal> signalMap = new Dictionary<string, Signal>
                (StringComparer.InvariantCultureIgnoreCase);

            try
            {
                string sjson = "{}";
                using (StreamReader reader = new StreamReader(layoutFile))
                {
                    sjson = reader.ReadToEnd();
                    boardLayout = JsonConvert.DeserializeObject<Layout>(sjson);
                    reader.Close();
                }
                foreach (var pkg in boardLayout.packages)
                {
                    packageMap[pkg.name] = pkg;
                    packageMap[pkg.ComponentID] = pkg;  // for ID search
                }
                if (boardLayout.signals == null) boardLayout.signals = new List<Signal>();
                foreach (var sig in boardLayout.signals)
                {
                    signalMap[sig.name] = sig;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR Reading Layout File: " + e.Message);
                System.Environment.Exit(-1);
            }
            #endregion

            Eagle.eagle eagle = null;

            #region LoadBoardTemplate
            // load board template file 
            // if there is a user-defined load that one, 
            // otherwise, load the one bundled as a project resource
            if (boardLayout.boardTemplate != null && boardLayout.boardTemplate != "")
            {
                try
                {
                    eagle = Eagle.eagle.LoadFromFile(boardLayout.boardTemplate);
                    Console.WriteLine("Parsed Eagle Board Template File {0}: " + eagle.version, boardLayout.boardTemplate);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: Unable to load Board Template XML: " + e.Message);
                }
            }

            if (eagle == null)  // unable 
            {
                try
                {
                    var brdTempl = Properties.Resources.boardTemplate;
                    eagle = Eagle.eagle.Deserialize(brdTempl);
                    Console.WriteLine("Parsed Eagle Library schema version: " + eagle.version);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: Failed parsing default board Template XML: " + e.Message);
                    System.Environment.Exit(-1);
                }
            }
            #endregion

            #region Layout2Board
            if (programMode == 0)   // forward mode: layout --> board
            {
                // Create Board File
                {
                    // Mark board boundaries
                    var plain = (eagle.drawing.Item as Eagle.board).plain;
                    AddBoundary(boardLayout, plain);

                    // Design Rules
                    if (boardLayout.designRules != null || boardLayout.designRules != "")
                        CopyDesignRules(boardLayout.designRules, (eagle.drawing.Item as Eagle.board).designrules);

                    // Copy Libraries
                    var srcLib = (schematic.drawing.Item as Eagle.schematic).libraries;
                    var dstLib = (eagle.drawing.Item as Eagle.board).libraries;
                    foreach (var lib in srcLib.library)
                    {
                        dstLib.library.Add(lib);
                    }

                    // Add Elements with Position
                    var parts = (schematic.drawing.Item as Eagle.schematic).parts;
                    var elements = (eagle.drawing.Item as Eagle.board).elements;
                    AddElements(parts, packageMap, dstLib, elements);

                    // Add Signals 
                    var nets = (schematic.drawing.Item as Eagle.schematic).sheets.sheet.Select(s => s.nets).FirstOrDefault();
                    var signals = (eagle.drawing.Item as Eagle.board).signals;
                    AddSignals(nets, parts, signalMap, packageMap, dstLib, signals);
                }

                eagle.SaveToFile(boardFile);
            }
            #endregion

            #region Board2Layout
            else if (programMode == 1)  // reverse mode: board --> layout
            {
                try
                {
                    eagle = Eagle.eagle.LoadFromFile(boardFile);
                    Console.WriteLine("Parsed Board File : " + boardFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR Reading Board XML: " + e.Message);
                    System.Environment.Exit(-1);
                }

                Dictionary<string, Eagle.element> elementMap = new Dictionary<string, Eagle.element>();

                var elements = (eagle.drawing.Item as Eagle.board).elements;
                foreach (var element in elements.element)
                {
                    if (packageMap.ContainsKey(element.name))
                    {
                        var pkg = packageMap[element.name];
                        if (element.rot.Contains("R90"))
                            pkg.rotation = 1;
                        else if (element.rot.Contains("R180"))
                            pkg.rotation = 2;
                        else if (element.rot.Contains("R270"))
                            pkg.rotation = 3;
                        if (element.rot.Contains("M"))
                            pkg.layer = 1;
                        double x = Convert.ToDouble(element.x) -
                            ((pkg.rotation == 1 || pkg.rotation == 3) ? pkg.height/2.0 : pkg.width/2.0);
                        double y = Convert.ToDouble(element.y) - 
                            ((pkg.rotation == 1 || pkg.rotation == 3) ? pkg.width/2.0 : pkg.height/2.0);
                        pkg.x = Math.Ceiling(x * 10.0) / 10.0;      // ceil to compensate for floor that we do in forward operation
                        pkg.y = Math.Ceiling(y * 10.0) / 10.0;      // ceil to compensate for floor that we do in forward operation
                    }
                    else
                    {
                        Console.WriteLine("WARNING: part {0} not found in layout file", element.name);
                    }
                    elementMap.Add(element.name, element);
                }

                var signals = (eagle.drawing.Item as Eagle.board).signals;
                foreach (var signal in signals.signal)
                {
                    Signal jsig = null;
                    bool update = true;
                    if (signalMap.ContainsKey(signal.name))
                        jsig = signalMap[signal.name];
                    else
                    {
                        jsig = new Signal();
                        jsig.name = signal.name;
                        update = false;
                    }
                    jsig.pins = new List<Pin>();
                    jsig.wires = new List<Wire>();
                    jsig.vias = new List<Via>();
                    double traceLength = 0.0;
                    Dictionary<string, Pin> pinMap = new Dictionary<string,Pin>();
                    foreach (var item in signal.Items)
                    {
                        if (item is Eagle.contactref)
                        {
                            var cref = item as Eagle.contactref;
                            var pin = new Pin();
                            var element = elementMap.ContainsKey(cref.element) ? elementMap[cref.element] : null;
                            if (element != null)
                            {
                                var lib = (schematic.drawing.Item as Eagle.schematic).libraries.library.Where(l => l.name.Equals(element.library)).FirstOrDefault();
                                var dev = (lib != null) ? lib.devicesets.deviceset.SelectMany(ds => ds.devices.device).Where(d => d.package.Equals(element.package)).FirstOrDefault() : null;
                                var connect = (dev != null) ? dev.connects.connect.Where(c => c.pad.Equals(cref.pad)).FirstOrDefault() : null;
                                pin.name = (connect != null) ? connect.pin : cref.pad;
                                pin.pad = (connect != null) ? connect.pad : cref.pad;
                                pin.gate = (connect != null) ? connect.gate : null;
                            }
                            pin.package = cref.element;
                            var pinName = string.Format("{0}.{1}", pin.package, pin.name);
                            // prevent creation of duplicate pins from the same package
                            if (!pinMap.ContainsKey(pinName))
                            {
                                jsig.pins.Add(pin);
                                pinMap[pinName] = pin;
                            }
                        }
                        else if (item is Eagle.wire)
                        {
                            var wire = item as Eagle.wire;
                            var jwire = new Wire();
                            jwire.x1 = Convert.ToDouble(wire.x1);
                            jwire.x2 = Convert.ToDouble(wire.x2);
                            jwire.y1 = Convert.ToDouble(wire.y1);
                            jwire.y2 = Convert.ToDouble(wire.y2);
                            jwire.width = Convert.ToDouble(wire.width);
                            jwire.layer = Convert.ToInt32(wire.layer);
                            jsig.wires.Add(jwire);
                            double w = jwire.x2 - jwire.x1;
                            double h = jwire.y2 - jwire.y1;
                            double l = Math.Sqrt(w * w + h * h);
                            traceLength += l;
                        }
                        else if (item is Eagle.via)
                        {
                            var via = item as Eagle.via;
                            var jvia = new Via();
                            jvia.x = Convert.ToDouble(via.x);
                            jvia.y = Convert.ToDouble(via.y);
                            jvia.drill = Convert.ToDouble(via.drill);
                            string[] layerRange = via.extent.Split('-');
                            if (layerRange.Count() >= 2)
                            {
                                jvia.layerBegin = Convert.ToInt32(layerRange[0]);
                                jvia.layerEnd = Convert.ToInt32(layerRange[1]);
                            }
                            jsig.vias.Add(jvia);
                        }
                        jsig.length = traceLength;
                        jsig.bends = jsig.wires.Count + jsig.vias.Count - 1;
                    }
                    if (!update)
                        boardLayout.signals.Add(jsig);
                }
                Console.WriteLine("Writing Output to {0}", layoutFile);
                var sjson = JsonConvert.SerializeObject(boardLayout, Formatting.Indented);
                StreamWriter writer = new StreamWriter(layoutFile);
                writer.Write(sjson);
                writer.Close();
            }
            #endregion
        }

        static void AddElements(Eagle.parts parts, Dictionary<string, Package> packageMap, 
            Eagle.libraries libs, Eagle.elements elements)
        {
            foreach (var part in parts.part)
            {
                Eagle.element element = new Eagle.element();
                element.name = part.name;
                element.library = part.library;
                var devset =
                    libs.library.Where(l => l.name.Equals(part.library)).                     // find library
                    SelectMany(l => l.devicesets.deviceset).Where(ds => ds.name.Equals(part.deviceset)).    // find deviceset
                    FirstOrDefault();
                var device = devset != null ?
                    devset.devices.device.Where(d => d.name.Equals(part.device)).             // find device
                    FirstOrDefault() : null;
                var tech = device != null ?
                    device.technologies.technology.Select(t => t.name).FirstOrDefault() : null;

                if (devset == null || device == null)
                {
                    Console.WriteLine("ERROR: part {0} does not have device or deviceset information in schema file", part.name); 
                    continue;
                }
                element.package = device.package;

                if (devset.uservalue.ToString().Equals("no"))
                {
                    var value = devset.name + device.name;
                    element.value = value.Replace("*", tech);
                }

                if (packageMap.ContainsKey(part.name))
                {
                    var pkg = packageMap[part.name];
                    // multiple with 10, floor and divide by 10 to align place on a 0.1 mm grid 
                    double pkgx = (pkg.x != null) ? (double)pkg.x : 0.0;
                    double pkgy = (pkg.y != null) ? (double)pkg.y : 0.0;
                    int prot = (pkg.rotation != null) ? (int)pkg.rotation : 0;
                    double x = 0.1 * Math.Floor(10.0 * (pkgx + 
                        ((prot % 2)  == 1 ? pkg.height/2.0 : pkg.width/2.0)));
                    double y = 0.1 * Math.Floor(10.0 * (pkgy + 
                        ((prot % 2)  == 1 ? pkg.width/2.0 : pkg.height/2.0)));
                    double xorg = pkg.originX != null ? (double)pkg.originX : 0.0;
                    double yorg = pkg.originY != null ? (double)pkg.originY : 0.0;
                    switch (prot)
                    {
                        case 0: x -= xorg; y -= yorg; break;
                        case 2: x += xorg; y += yorg; break;
                        case 1: x -= yorg; y -= xorg; break;
                        case 3: x += yorg; y += xorg; break;
                    }

                    double rx = x;
                    double ry = y;

                    if (pkg.RelComponentID != null &&
                        packageMap.ContainsKey(pkg.RelComponentID))
                    // part has a relative placement
                    {
                        var refPkg = packageMap[pkg.RelComponentID];
                        // rotate the contained parts according to refPkg rotation
                        if (refPkg.rotation != null) prot += (int)refPkg.rotation;
                        if (prot > 3) prot = prot % 4;

                        RotateAndTranslate(refPkg, x, y, out rx, out ry);
                    }
                    element.x = rx.ToString();
                    element.y = ry.ToString();
                    string layer = ((pkg.layer == 1) ? "M" : "");
                    string rot = (90 * prot).ToString();
                    element.rot = layer + "R" + rot;
                }
                else
                {
                    Console.WriteLine("WARNING: part {0} not found in layout file", part.name);
                }

                foreach (var sattrib in device.technologies.technology.SelectMany(t => t.attribute))
                {
                    Eagle.attribute dattrib = new Eagle.attribute();
                    dattrib.name = sattrib.name;
                    dattrib.value = sattrib.value;
                    dattrib.layer = ATTRIB_LAYER;
                    dattrib.display = Eagle.attributeDisplay.off;

                    element.attribute.Add(dattrib);
                }


                elements.element.Add(element);
            }
        }

        static void AddSignals(Eagle.nets nets, Eagle.parts parts, 
            Dictionary<string, Signal> signalMap, Dictionary<string, Package> packageMap,
            Eagle.libraries libs, Eagle.signals signals)
        {
            foreach (var net in nets.net)
            {
                var signal = new Eagle.signal();
                signal.name = net.name;
                var pinrefs = net.segment.SelectMany(s => s.Items).
                    Where(i => i is Eagle.pinref).
                    Select(i => i as Eagle.pinref );

                foreach (var pinref in pinrefs )
                {
                    var part = parts.part.Where(p => p.name.Equals(pinref.part)).FirstOrDefault();
                    var device =
                        libs.library.Where(l => l.name.Equals(part.library)).                                   // find library
                        SelectMany(l => l.devicesets.deviceset).Where(ds => ds.name.Equals(part.deviceset)).    // find deviceset
                        SelectMany(ds => ds.devices.device).Where(d => d.name.Equals(part.device)).             // find device
                        FirstOrDefault();

                    if (device == null)
                    {
                        // same error as reported above 
                        continue;
                    }

                    var connect = device.connects.connect.
                        Where(c => c.pin.Equals(pinref.pin) && c.gate.Equals(pinref.gate)).
                        FirstOrDefault();

                    var pads = connect.pad.Split(' ');
                    foreach (var pad in pads)
                    {
                        var contactref = new Eagle.contactref();
                        contactref.element = pinref.part;
                        contactref.pad = pad;
                        if (connect.route != CyPhy2Schematic.Schematic.Eagle.connectRoute.all)
                            contactref.route = (CyPhy2Schematic.Schematic.Eagle.contactrefRoute)connect.route;
                        signal.Items.Add(contactref);
                    }
                }

                if (signalMap.ContainsKey(net.name))        // add pre-route if we already have it 
                {
                    var jsig = signalMap[net.name];
                    Package refPkg = null;
                    if (jsig.RelComponentID != null && packageMap.ContainsKey(jsig.RelComponentID))
                        refPkg = packageMap[jsig.RelComponentID];

                    if (jsig.wires != null)
                    {
                        foreach (var jwire in jsig.wires)
                        {
                            double rx1 = jwire.x1;
                            double rx2 = jwire.x2;
                            double ry1 = jwire.y1;
                            double ry2 = jwire.y2;

                            var wire = new Eagle.wire();
                            if (refPkg != null)
                            {
                                RotateAndTranslate(refPkg, jwire.x1, jwire.y1, out rx1, out ry1);
                                RotateAndTranslate(refPkg, jwire.x2, jwire.y2, out rx2, out ry2);
                            }
                            wire.x1 = rx1.ToString();
                            wire.x2 = rx2.ToString();
                            wire.y1 = ry1.ToString();
                            wire.y2 = ry2.ToString();
                            wire.width = jwire.width.ToString();
                            wire.layer = jwire.layer.ToString();
                            signal.Items.Add(wire);
                        }
                    }
                    if (jsig.vias != null)
                    {
                        foreach (var jvia in jsig.vias)
                        {
                            var via = new Eagle.via();
                            double rx = jvia.x;
                            double ry = jvia.y;
                            if (refPkg != null)
                            {
                                RotateAndTranslate(refPkg, jvia.x, jvia.y, out rx, out ry);
                            }
                            via.x = rx.ToString();
                            via.y = ry.ToString();
                            via.extent = jvia.layerBegin.ToString() + '-' + jvia.layerEnd.ToString();
                            via.drill = jvia.drill.ToString();
                            signal.Items.Add(via);
                        }
                    }
                }

                signals.signal.Add(signal);
            }
        }

 
        static void AddBoundary(Layout boardLayout, Eagle.plain plain)
        {
            {
                var w = new Eagle.wire();
                w.x1 = Convert.ToString(0);
                w.x2 = Convert.ToString(boardLayout.boardWidth);
                w.y1 = Convert.ToString(0);
                w.y2 = Convert.ToString(0);
                w.width = Convert.ToString(0);
                w.layer = BOARD_LAYER;
                plain.Items.Add(w);
            }
            {
                var w = new Eagle.wire();
                w.x1 = Convert.ToString(0);
                w.x2 = Convert.ToString(boardLayout.boardWidth);
                w.y1 = Convert.ToString(boardLayout.boardHeight);
                w.y2 = Convert.ToString(boardLayout.boardHeight);
                w.width = Convert.ToString(0);
                w.layer = BOARD_LAYER;
                plain.Items.Add(w);
            }
            {
                var w = new Eagle.wire();
                w.x1 = Convert.ToString(0);
                w.x2 = Convert.ToString(0);
                w.y1 = Convert.ToString(0);
                w.y2 = Convert.ToString(boardLayout.boardHeight);
                w.width = Convert.ToString(0);
                w.layer = BOARD_LAYER;
                plain.Items.Add(w);
            }
            {
                var w = new Eagle.wire();
                w.x1 = Convert.ToString(boardLayout.boardWidth);
                w.x2 = Convert.ToString(boardLayout.boardWidth);
                w.y1 = Convert.ToString(0);
                w.y2 = Convert.ToString(boardLayout.boardHeight);
                w.width = Convert.ToString(0);
                w.layer = BOARD_LAYER;
                plain.Items.Add(w);
            }
        }

        static void RotateAndTranslate(Package refPkg, double x, double y, 
            out double rx, out double ry)
        {
            // point-wise translate the center of the part  
            // corresponding to frame rotation
            double theta = (double)refPkg.rotation * Math.PI / 2.0;
            rx = x * Math.Cos(theta) - y * Math.Sin(theta);
            ry = x * Math.Sin(theta) + y * Math.Cos(theta);

            if (refPkg.rotation == 1) rx += refPkg.height;
            else if (refPkg.rotation == 2) { rx += refPkg.width; ry += refPkg.height; }
            else if (refPkg.rotation == 3) ry += refPkg.width;

            // now translate
            rx += (double)refPkg.x;
            ry += (double)refPkg.y;
        }

        static void CopyDesignRules(string designRuleFile, Eagle.designrules designRules)
        {
            try
            {
                using (StreamReader druIn = new StreamReader(designRuleFile))
                {

                    // initialize the design rules section
                    designRules.name = Path.GetFileNameWithoutExtension(designRuleFile);
                    designRules.description.Clear();
                    designRules.param.Clear();

                    var line = druIn.ReadLine();
                    var regex = new Regex("[a-zA-Z]*\\[(.*)\\]");
                    while (line != null)
                    {
                        string[] nameValues = line.Split('=');
                        if (nameValues.Count() < 2)
                        {
                            // error 
                            continue;
                        }
                        var name = nameValues[0].Trim();
                        var value = nameValues[1].Trim();

                        if (name.Contains("description"))
                        {
                            var desc = new Eagle.description();
                            desc.Text.Add(value);
                            Console.WriteLine(name);
                            desc.language = regex.Replace(name, "$1");
                            Console.WriteLine(desc.language);
                            designRules.description.Add(desc);
                        }
                        else
                        {
                            var param = new Eagle.param();
                            param.name = name;
                            param.value = value;
                            designRules.param.Add(param);
                        }

                        // read the next line
                        line = druIn.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in reading design rules file {0}", e.Message);
            }

        }
    }
}
