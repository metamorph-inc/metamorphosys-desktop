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

using Tonka = ISIS.GME.Dsml.CyPhyML.Interfaces;
using TonkaClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CyPhyGUIs;

namespace CyPhy2Schematic.Schematic
{
    public class CodeGenerator
    {
        public static GMELogger Logger { get; set; }
        public static bool verbose { get; set; }
        public static string BasePath { get; set; } // Path of the root testbench object in GME object tree
        public static Dictionary<string, int> partNames;
        public static Dictionary<Eagle.part, Component> partComponentMap;
        public static Dictionary<Component, Eagle.part> componentPartMap;

        public static Layout.LayoutParser signalIntegrityLayout { get; set; } // layout parser for signal integrity mode
        public static Dictionary<ComponentAssembly, Layout.LayoutParser> preRouted { get; set; } // layout parsers for prerouted assemblies/subcircuits

        public enum Mode 
        {
            EDA,
            SPICE,
            SPICE_SI
        };
        public Mode mode { get; private set; }

        static CodeGenerator()
        {
            verbose = false;
        }

        public class Result
        {
            // results of generate code
            public string runCommandArgs;       // arguments for the runCommand to be sent to job manager
        }

        private CyPhyGUIs.IInterpreterMainParameters mainParameters { get; set; }

        public CodeGenerator(CyPhyGUIs.IInterpreterMainParameters parameters, Mode mode)
        {
            this.mainParameters = parameters;
            CodeGenerator.verbose = ((CyPhy2Schematic.CyPhy2Schematic_Settings)parameters.config).Verbose;
            partNames = new Dictionary<string, int>();
            partComponentMap = new Dictionary<Eagle.part, Component>();
            componentPartMap = new Dictionary<Component, Eagle.part>();
            preRouted = new Dictionary<ComponentAssembly, Layout.LayoutParser>();

            this.mode = mode;
        }

        private void CommonTraversal(TestBench TestBench_obj)
        {
            // 1. A first traversal maps CyPhy objects to a corresponding but significantly lighter weight object network that only includes a 
            //     small set of concepts/classes : TestBench, ComponentAssembly, Component, Parameter, Port, Connection
            // 2. Second and third traversal passes compute the layout of the graph in schematic
            // 3. Forth traversal wires the object network
            //      the object network is hierarchical, but the wiring is direct and skips hierarchy. The dependency on CyPhy is largely localized to the 
            //      traversal/visitor code (CyPhyVisitors.cs)
            
            TestBench_obj.accept(new CyPhyBuildVisitor(this.mainParameters.ProjectDirectory, this.mode)
            {
                Logger = Logger
            });

            if (mode == Mode.EDA)
            {
                TestBench_obj.accept(new CyPhyLayoutVisitor() { Logger = Logger });
                TestBench_obj.accept(new CyPhyLayout2Visitor() { Logger = Logger });
            }

            TestBench_obj.accept(new CyPhyConnectVisitor(this.mode) { Logger = Logger });
        }

        private Eagle.eagle GenerateSchematicCode(TestBench TestBench_obj)
        {
            // load schematic library
            Eagle.eagle eagle = null;
            try
            {
                eagle = Eagle.eagle.Deserialize(CyPhy2Schematic.Properties.Resources.schematicTemplate);
                Logger.WriteInfo("Parsed Eagle Library schema version: " + eagle.version);
            }
            catch (Exception e)
            {
                eagle = new Eagle.eagle();  // create an empty eagle object network
                Logger.WriteError("Error parsing XML: " + e.Message + "<br>Inner: " + e.InnerException + "<br>Stack: " + e.StackTrace);
            }
            // 2. The second traversal walks the lighter weight (largely CyPhy independent) object network and maps to the eagle XML object network
            //    the classes of this object network are automatically derived from the eagle XSD using the XSD2Code tool in the META repo
            //    an important step of this traversal is the routing which is implemented currently as a simple rats nest routing, 
            //        the traversal and visitor code is localized in (SchematicTraversal.cs)
            TestBench_obj.accept(new EdaVisitor() 
            { 
                eagle_obj = eagle, 
                Logger = Logger
            });

            // 2.5  Finally a serializer (XSD generated code), walks the object network and generates the XML file
            System.IO.Directory.CreateDirectory(this.mainParameters.OutputDirectory);
            String outFile = Path.Combine(this.mainParameters.OutputDirectory, "schema.sch");
            try
            {
                eagle.SaveToFile(outFile);
            }
            catch (Exception ex)
            {
                Logger.WriteError("Error Saving Schema File: {0}<br> Exception: {1}<br> Trace: {2}", 
                    outFile, ex.Message, ex.StackTrace);
            }

            return eagle;
        }

        private void GenerateLayoutCode(Eagle.eagle eagle, Schematic.TestBench TestBench_obj)
        {
            // write layout file
            string layoutFile = Path.Combine(this.mainParameters.OutputDirectory, "layout-input.json");
            new Layout.LayoutGenerator(eagle.drawing.Item as Eagle.schematic, TestBench_obj, Logger).Generate(layoutFile);
        }

        private void GenerateSpiceCode(TestBench TestBench_obj)
        {
            var circuit = new Spice.Circuit() { name = TestBench_obj.Name };
            var siginfo = new Spice.SignalContainer() { name = TestBench_obj.Name };
            // now traverse the object network with Spice Visitor to build the spice and siginfo object network
            TestBench_obj.accept(new SpiceVisitor() { circuit_obj = circuit, siginfo_obj = siginfo, mode = this.mode });
            String spiceFile = Path.Combine(this.mainParameters.OutputDirectory, "schema.cir");
            circuit.Serialize(spiceFile);
            String siginfoFile = Path.Combine(this.mainParameters.OutputDirectory, "siginfo.json");
            siginfo.Serialize(siginfoFile);

        }
        
        private void GenerateChipFitCommandFile()
        {
            var chipFitBat = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "chipfit.bat"));
            chipFitBat.Write(CyPhy2Schematic.Properties.Resources.chipFit);
            chipFitBat.Close();
        }

        private void GenerateShowChipFitResultsCommandFile()
        {
            var showChipFitResultsBat = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "showChipFitResults.bat"));
            showChipFitResultsBat.Write(CyPhy2Schematic.Properties.Resources.showChipFitResults);
            showChipFitResultsBat.Close();
        }

        private void GeneratePlacementCommandFile()
        {
            var placeBat = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "placement.bat"));
            placeBat.Write(CyPhy2Schematic.Properties.Resources.placement);
            placeBat.Close();
        }

        private void GeneratePlaceOnlyCommandFile()
        {
            var placeBat = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "placeonly.bat"));
            placeBat.Write(CyPhy2Schematic.Properties.Resources.placeonly);
            placeBat.Close();
        }

        private string GenerateCommandArgs(TestBench Testbench_obj)
        {
            string commandArgs = "";
            var icg = Testbench_obj.Parameters.Where(p => p.Name.Equals("interChipSpace")).FirstOrDefault();       // in mm
            var eg = Testbench_obj.Parameters.Where(p => p.Name.Equals("boardEdgeSpace")).FirstOrDefault();    // in mm

            if (icg != null)
            {
                commandArgs += " -i " + icg.Value;
            }
            if (eg != null)
            {
                commandArgs += " -e " + eg.Value;
            }

            return commandArgs;
        }

        private void CopyBoardFiles(TestBench Testbench_obj)
        {
            CopyBoardFile(Testbench_obj, "designRules");
            CopyBoardFile(Testbench_obj, "boardTemplate");
        }

        private void CopyBoardFile(TestBench Testbench_obj, string param)
        {
            var par = Testbench_obj.Parameters.Where(p => p.Name.Equals(param)).FirstOrDefault();
            if (par == null)
                return;
            var pfn = par.Value; // check file name in par.value
            try
            {
                pfn = Path.GetFileName(par.Value);
            }
            catch (ArgumentException ex)
            {
                Logger.WriteError("Error extracting {0} filename: {1}", param, ex.Message);
                return;
            }

            var source = Path.Combine(this.mainParameters.ProjectDirectory, par.Value);
            var dest = Path.Combine(this.mainParameters.OutputDirectory, pfn);
            try
            {
                System.IO.File.Copy(source, dest);
            }
            catch (FileNotFoundException ex)
            {
                Logger.WriteError("Error copying {0} file: {1}", param, ex.Message);
            }
            catch (DirectoryNotFoundException ex2)
            {
                Logger.WriteError("Error copying {0} file: {1}", param, ex2.Message);
            }

        }


        private void GenerateSpiceCommandFile(TestBench Testbench_obj)
        {
            var spiceBat = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "runspice.bat"));
            spiceBat.Write(CyPhy2Schematic.Properties.Resources.runspice);

            // find a voltage source test component
            var voltageSrc = Testbench_obj.Impl.Children
                                               .TestComponentCollection
                                               .FirstOrDefault(c => c.Children.SPICEModelCollection
                                                                              .Select(x => x.Attributes.Class)
                                                                              .Contains("V"));
            if (voltageSrc != null)
            {
                // add a call to spice post process
                spiceBat.Write("if exist \"schema.raw\" (\n");
                spiceBat.Write("\t\"%META_PATH%\\bin\\python27\\scripts\\python.exe\" -m SpiceVisualizer.post_process -m {0} schema.raw\n", voltageSrc.Name);
                spiceBat.Write(")\n");
            }
            spiceBat.Close();
        }

        private void GenerateSpiceViewerLauncher()
        {
            var launchViewerBat = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "LaunchSpiceViewer.bat"));
            launchViewerBat.Write(CyPhy2Schematic.Properties.Resources.LaunchSpiceViewer);
            launchViewerBat.Close();
        }

        private void GenerateReferenceDesignatorMappingTable(TestBench Testbench_obj)
        {
            using (var sw = new StreamWriter(Path.Combine(this.mainParameters.OutputDirectory, "reference_designator_mapping_table.html")))
            {

                // Get all nested assemblies using an interative approach.
                var assemblies = new List<ComponentAssembly>();
                assemblies.AddRange(Testbench_obj.ComponentAssemblies);
                for (int i = 0; i < assemblies.Count; i++ )
                {
                    var assembly = assemblies[i];
                    assemblies.AddRange(assembly.ComponentAssemblyInstances);
                }

                // Get all instances from everywhere.
                var componentInstances = assemblies.SelectMany(a => a.ComponentInstances).ToList();
                componentInstances.AddRange(Testbench_obj.TestComponents);

                // Build mapping table
                List<XrefItem> xrefTable = new List<XrefItem>();
                foreach (var ci in componentInstances)
                {
                    String path = ci.Impl.Path;
                    String refDes = ci.Name;
                    XrefItem row = new XrefItem() { ReferenceDesignator = refDes, GmePath = path };
                    xrefTable.Add(row);
                }

                // Convert it to HTML
                string html = Xref2Html.makeHtmlFile(
                    "",
                    xrefTable,
                    "");

                // Write mapping table to file
                sw.Write(html);
            }
        }

        public Result GenerateCode()
        {
            Result result = new Result();

            // map the root testbench obj
            var testbench = TonkaClasses.TestBench.Cast(this.mainParameters.CurrentFCO);
            if (testbench == null)
            {
                Logger.WriteError("Invalid context of invocation <{0}>, invoke the interpreter from a Testbench model", 
                    this.mainParameters.CurrentFCO.Name);
                return result;
            }
            var TestBench_obj = new TestBench(testbench);
            BasePath = testbench.Path;

            CommonTraversal(TestBench_obj);

            GenerateReferenceDesignatorMappingTable(TestBench_obj);

            switch (mode)
            {
                case Mode.EDA:
                    var eagle = GenerateSchematicCode(TestBench_obj);
                    GenerateLayoutCode(eagle, TestBench_obj);
                    CopyBoardFiles(TestBench_obj);     // copy DRU/board template file if the testbench has it specified
                    GenerateChipFitCommandFile();
                    GenerateShowChipFitResultsCommandFile();
                    GeneratePlacementCommandFile();
                    GeneratePlaceOnlyCommandFile();
                    result.runCommandArgs = GenerateCommandArgs(TestBench_obj);
                    break;
                case Mode.SPICE_SI:
                    // parse and map the nets to ports
                    signalIntegrityLayout = new Layout.LayoutParser("layout.json", Logger)
                    {
                        mode = this.mode
                    };
                    signalIntegrityLayout.BuildMaps();

                    // spice code generator uses the mapped traces 
                    // to generate subcircuits for traces and inserts them appropriately
                    GenerateSpiceCode(TestBench_obj);
                    GenerateSpiceCommandFile(TestBench_obj);
                    break;
                case Mode.SPICE:
                    GenerateSpiceCode(TestBench_obj);
                    GenerateSpiceCommandFile(TestBench_obj);
                    GenerateSpiceViewerLauncher();
                    break;
                default:
                    throw new NotSupportedException(String.Format("Mode {0} is not supported", mode.ToString()));
            }

            return result;
        }
    }
}
