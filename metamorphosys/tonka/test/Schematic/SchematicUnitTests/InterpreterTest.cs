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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SchematicUnitTests
{
    public class InterpreterTestFixture : IDisposable
    {
        public static String path_Test = Path.Combine(META.VersionInfo.MetaPath,
                                                      "..",
                                                      "tonka",
                                                      "models",
                                                      "SchematicTestModel");

        private static String path_XME = Path.Combine(path_Test,
                                                      "Schematic.xme");

        public readonly String path_MGA;
        public MgaProject proj { get; private set; }

        public InterpreterTestFixture()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(path_XME, out mgaConnectionString);
            path_MGA = mgaConnectionString.Substring("MGA=".Length);

            Assert.True(File.Exists(Path.GetFullPath(path_MGA)),
                        String.Format("{0} not found. Model import may have failed.", path_MGA));

            if (Directory.Exists(Path.Combine(path_Test, "output")))
            {
                Directory.Delete(Path.Combine(path_Test, "output"), true);
            }
            if (Directory.Exists(Path.Combine(path_Test, "results")))
            {
                Directory.Delete(Path.Combine(path_Test, "results"), true);
            }

            proj = new MgaProject();
            bool ro_mode;
            proj.Open("MGA=" + Path.GetFullPath(path_MGA), out ro_mode);
            proj.EnableAutoAddOns(true);
        }

        public void Dispose()
        {
            proj.Save();
            proj.Close();
            proj = null;
        }
    }
 
    public class InterpreterTest : IUseFixture<InterpreterTestFixture>
    {
        public const string generatedSchemaFile = "schema.sch";
        public const string generatedSpiceFile = "schema.cir";
        public const string generatedLayoutFile = "layout-input.json";
        public const string generatedSpiceViewerLauncher = "LaunchSpiceViewer.bat";

        #region Fixture
        InterpreterTestFixture fixture;
        public void SetFixture(InterpreterTestFixture data)
        {
            fixture = data;
        }
        #endregion

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
                return InterpreterTestFixture.path_Test;
            }
        }
        
        [Fact]
        public void SpiceGeneration()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/Spice_Test|kind=TestBench|relpos=0";

            RunInterpreterMain(OutputDir,
                               TestbenchPath,
                               new CyPhy2Schematic.CyPhy2Schematic_Settings() { doSpice = "true" });

            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Failed to generate " + generatedSpiceFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Generated EAGLE schematic (" + generatedSchemaFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedLayoutFile)), "Generated layout file (" + generatedLayoutFile + "), but shouldn't have.");
            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Failed to generate " + generatedSpiceFile);
        } 

        [Fact]
        public void BasicSchematicGeneration()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/BasicSchematicGeneration|kind=TestBench|relpos=0";

            RunInterpreterMain(OutputDir, TestbenchPath);

            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Failed to generate " + generatedSchemaFile);
            Assert.True(File.Exists(Path.Combine(OutputDir, generatedLayoutFile)), "Failed to generate " + generatedLayoutFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Generated SPICE model (" + generatedSpiceFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Generated " + generatedSpiceFile + ", but shouldn't have.");
        }

        [Fact]
        public void ChipFit()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/ChipFit|kind=TestBench|relpos=0";

            RunInterpreterMain(OutputDir,
                               TestbenchPath,
                               new CyPhy2Schematic.CyPhy2Schematic_Settings() { doChipFit = "true" });

            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Failed to generate " + generatedSchemaFile);
            Assert.True(File.Exists(Path.Combine(OutputDir, generatedLayoutFile)), "Failed to generate " + generatedLayoutFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Generated SPICE model (" + generatedSpiceFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Generated " + generatedSpiceFile + ", but shouldn't have.");
        }

        [Fact]
        public void PlaceRoute()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/PlaceRoute|kind=TestBench|relpos=0";
            
            RunInterpreterMain(OutputDir,
                               TestbenchPath,
                               new CyPhy2Schematic.CyPhy2Schematic_Settings() { doPlaceRoute = "true" });

            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Failed to generate " + generatedSchemaFile);
            Assert.True(File.Exists(Path.Combine(OutputDir, generatedLayoutFile)), "Failed to generate " + generatedLayoutFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Generated SPICE model (" + generatedSpiceFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Generated " + generatedSpiceFile + ", but shouldn't have.");
        }

        [Fact]
        public void SpacingParameters()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            
            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/SchematicSpacing|kind=TestBench|relpos=0";

            MasterInterpreterTest.CyPhyMasterInterpreterRunner.RunMasterInterpreter(fixture.path_MGA,
                                                                                    TestbenchPath,
                                                                                    "/@A_SpiceTests|kind=ComponentAssemblies|relpos=0/@Test1|kind=ComponentAssembly|relpos=0");

            // Currently this is only thing producing output in the "results" folder.
            // So we'll look for the first such folder.
            var directories = Directory.EnumerateDirectories(Path.Combine(TestPath, "results"));
            var OutputDir = Path.Combine(TestPath, "results", directories.First());
            
            // Verify normal outputs
            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Failed to generate " + generatedSchemaFile);
            Assert.True(File.Exists(Path.Combine(OutputDir, generatedLayoutFile)), "Failed to generate " + generatedLayoutFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Generated SPICE model (" + generatedSpiceFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Generated " + generatedSpiceFile + ", but shouldn't have.");

            // Check for spacing parameters in manifest's first execution command.
            var manifest = AVM.DDP.MetaTBManifest.OpenForUpdate(OutputDir);
            var invocation = manifest.Steps.First().Invocation;

            Assert.Contains("-i 0.1", invocation);
            Assert.Contains("-e 0.2", invocation);
        }

        [Fact]
        public void MultiLayerAttribute()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/MultiLayerAttribute|kind=TestBench|relpos=0";
            
            RunInterpreterMain(OutputDir,
                               TestbenchPath,
                               new CyPhy2Schematic.CyPhy2Schematic_Settings() { doPlaceRoute = "true" });

            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Failed to generate " + generatedSchemaFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Generated SPICE model (" + generatedSpiceFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Generated " + generatedSpiceFile + ", but shouldn't have.");

            String pathLayoutFile = Path.Combine(OutputDir, generatedLayoutFile);
            Assert.True(File.Exists(pathLayoutFile),
                        "Failed to generate " + generatedLayoutFile);

            // Check layout file for "HasMultiLayerFootprint" indicators
            var jsonLayoutFile = File.ReadAllText(pathLayoutFile);
            var jobjLayoutFile = Newtonsoft.Json.Linq.JObject.Parse(jsonLayoutFile);
            var pkg = jobjLayoutFile["packages"].First(p => p["name"].ToString() == "SpicySingleOpAmp_HasMultiLayer");
            Assert.NotNull(pkg["multiLayer"]);
            Assert.Equal(true, pkg["multiLayer"].ToObject<Boolean>());
        }

        [Fact]
        public void CompHasNoEDAModel()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches|kind=Testing|relpos=0/CornerCases|kind=Testing|relpos=0/CompHasNoEDAModel|kind=TestBench|relpos=0";
            
            RunInterpreterMain(OutputDir,
                               TestbenchPath,
                               new CyPhy2Schematic.CyPhy2Schematic_Settings() { doPlaceRoute = "true" });

            Assert.True(File.Exists(Path.Combine(OutputDir, generatedSchemaFile)), "Failed to generate " + generatedSchemaFile);
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceFile)), "Generated SPICE model (" + generatedSpiceFile + "), but shouldn't have.");
            Assert.False(File.Exists(Path.Combine(OutputDir, generatedSpiceViewerLauncher)), "Generated " + generatedSpiceFile + ", but shouldn't have.");
        }

        public void RunInterpreterMain(string outputdirname, string testBenchPath, CyPhy2Schematic.CyPhy2Schematic_Settings config = null)
        {
            if (Directory.Exists(outputdirname))
            {
                Directory.Delete(outputdirname, true);
            }
            Directory.CreateDirectory(outputdirname);
            Assert.True(Directory.Exists(outputdirname), "Output directory wasn't created for some reason.");

            MgaFCO testObj = null;
            project.PerformInTransaction(delegate
            {
                testObj = project.ObjectByPath[testBenchPath] as MgaFCO;
            });

            var interpreter = new CyPhy2Schematic.CyPhy2SchematicInterpreter();
            interpreter.Initialize(project);

            var mainParameters = new CyPhyGUIs.InterpreterMainParameters()
            {
                config = (config == null) ? new CyPhy2Schematic.CyPhy2Schematic_Settings() { Verbose = false }
                                          : config,
                Project = project,
                CurrentFCO = testObj,
                SelectedFCOs = (MgaFCOs)Activator.CreateInstance(Type.GetTypeFromProgID("Mga.MgaFCOs")),
                StartModeParam = 128,
                ConsoleMessages = false,
                ProjectDirectory = project.GetRootDirectoryPath(),
                OutputDirectory = outputdirname
            };

            var result = interpreter.Main(mainParameters);
            interpreter.DisposeLogger();

            Assert.True(result.Success, "Interpreter run was unsuccessful");
        }

        [Fact]
        public void Elaborator_RLC_RefAsOrigin()
        {
            project.PerformInTransaction(delegate
            {
                string pathComponentAssembly = "/@A_ElaboratorTest|kind=ComponentAssemblies|relpos=0/CA_ElabTest_RLC_RefAsOrigin|kind=ComponentAssembly|relpos=0";
                var objComponentAssembly = project.get_ObjectByPath(pathComponentAssembly);
                Assert.NotNull(objComponentAssembly);

                var asm = CyPhyClasses.ComponentAssembly.Cast(objComponentAssembly);
                Assert.NotNull(asm);
                
                var relativeLayoutConstraint_RefAsOrigin = asm.Children.RelativeLayoutConstraintCollection.First(rlc => rlc.Name == "RLC__RefAsOrigin");
                Assert.NotNull(relativeLayoutConstraint_RefAsOrigin);

                var CInst__Resistor_R0603 = asm.Children.ComponentCollection.First(c => c.Name == "CInst__Resistor_R0603");
                Assert.NotNull(CInst__Resistor_R0603);

                
                // Run Elaborator
                var elaborator = new CyPhyElaborateCS.CyPhyElaborateCSInterpreter();
                bool result = elaborator.RunInTransaction(project, asm.Impl as MgaFCO, null, 128);
                Assert.True(result);


                // Formerly ComponentRefs, these objects should now be Component instances
                var CRef__Resistor_R0603 = asm.Children.ComponentCollection.First(c => c.Name == "CRef__Resistor_R0603");
                Assert.NotNull(CInst__Resistor_R0603);

                var CRef__SpicySingleOpAmp = asm.Children.ComponentCollection.First(c => c.Name == "CRef__SpicySingleOpAmp");
                Assert.NotNull(CRef__SpicySingleOpAmp);

                #region Check RLC__RefAsOrigin
                Assert.Equal(1,
                             relativeLayoutConstraint_RefAsOrigin.DstConnections
                                                                  .ApplyRelativeLayoutConstraintCollection
                                                                  .Where(c => c.DstEnd.ID == CInst__Resistor_R0603.ID)
                                                                  .Count());

                Assert.Equal(1,
                             relativeLayoutConstraint_RefAsOrigin.DstConnections
                                                                  .ApplyRelativeLayoutConstraintCollection
                                                                  .Where(c => c.DstEnd.ID == CRef__SpicySingleOpAmp.ID)
                                                                  .Count());

                Assert.Equal(1,
                             relativeLayoutConstraint_RefAsOrigin.SrcConnections
                                                                  .RelativeLayoutConstraintOriginCollection
                                                                  .Where(c => c.SrcEnd.ID == CRef__Resistor_R0603.ID)
                                                                  .Count());
                #endregion

                // Sanity check
                Assert.Equal(1, asm.Children.RelativeLayoutConstraintOriginCollection.Count());
                Assert.Equal(2, asm.Children.ApplyRelativeLayoutConstraintCollection.Count());                
            });
        }
            
        [Fact]
        public void Elaborator_Exact()
        {
            project.PerformInTransaction(delegate
            {
                string pathComponentAssembly = "/@A_ElaboratorTest|kind=ComponentAssemblies|relpos=0/CA_ElabTest_Exact|kind=ComponentAssembly|relpos=0";
                var objComponentAssembly = project.get_ObjectByPath(pathComponentAssembly);
                Assert.NotNull(objComponentAssembly);

                var asm = CyPhyClasses.ComponentAssembly.Cast(objComponentAssembly);
                Assert.NotNull(asm);

                var exactLayoutConstraint = asm.Children.ExactLayoutConstraintCollection.First();
                Assert.NotNull(exactLayoutConstraint);

                var CInst__Resistor_R0603 = asm.Children.ComponentCollection.First(c => c.Name == "CInst__Resistor_R0603");
                Assert.NotNull(CInst__Resistor_R0603);

                
                // Run Elaborator
                var elaborator = new CyPhyElaborateCS.CyPhyElaborateCSInterpreter();
                bool result = elaborator.RunInTransaction(project, asm.Impl as MgaFCO, null, 128);
                Assert.True(result);

                
                // Formerly ComponentRefs, these objects should now be Component instances
                var CRef__SpicySingleOpAmp = asm.Children.ComponentCollection.First(c => c.Name == "CRef__SpicySingleOpAmp");
                Assert.NotNull(CRef__SpicySingleOpAmp);

                #region Check ExactLayoutConstraint
                Assert.Equal(1,
                             exactLayoutConstraint.DstConnections
                                                  .ApplyExactLayoutConstraintCollection
                                                  .Where(c => c.DstEnd.ID == CInst__Resistor_R0603.ID)
                                                  .Count());

                Assert.Equal(1,
                             exactLayoutConstraint.DstConnections
                                                  .ApplyExactLayoutConstraintCollection
                                                  .Where(c => c.DstEnd.ID == CRef__SpicySingleOpAmp.ID)
                                                  .Count());
                #endregion
                
                // Sanity check
                Assert.Equal(2, asm.Children.ApplyExactLayoutConstraintCollection.Count());
            });
        }

        [Fact]
        public void Elaborator_Range()
        {
            project.PerformInTransaction(delegate
            {
                string pathComponentAssembly = "/@A_ElaboratorTest|kind=ComponentAssemblies|relpos=0/CA_ElabTest_Range|kind=ComponentAssembly|relpos=0";
                var objComponentAssembly = project.get_ObjectByPath(pathComponentAssembly);
                Assert.NotNull(objComponentAssembly);

                var asm = CyPhyClasses.ComponentAssembly.Cast(objComponentAssembly);
                Assert.NotNull(asm);

                var rangeLayoutConstraint = asm.Children.RangeLayoutConstraintCollection.First();
                Assert.NotNull(rangeLayoutConstraint);

                var CInst__SpicySingleOpAmp = asm.Children.ComponentCollection.First(c => c.Name == "CInst__SpicySingleOpAmp");
                Assert.NotNull(CInst__SpicySingleOpAmp);


                // Run Elaborator
                var elaborator = new CyPhyElaborateCS.CyPhyElaborateCSInterpreter();
                bool result = elaborator.RunInTransaction(project, asm.Impl as MgaFCO, null, 128);
                Assert.True(result);


                // Formerly ComponentRefs, these objects should now be Component instances
                var CRef__Resistor_R0603 = asm.Children.ComponentCollection.First(c => c.Name == "CRef__Resistor_R0603");
                Assert.NotNull(CRef__Resistor_R0603);

                #region Check RangeLayoutConstraint
                Assert.Equal(1,
                             rangeLayoutConstraint.DstConnections
                                                  .ApplyRangeLayoutConstraintCollection
                                                  .Where(c => c.DstEnd.ID == CInst__SpicySingleOpAmp.ID)
                                                  .Count());

                Assert.Equal(1,
                             rangeLayoutConstraint.DstConnections
                                                  .ApplyRangeLayoutConstraintCollection
                                                  .Where(c => c.DstEnd.ID == CRef__Resistor_R0603.ID)
                                                  .Count());
                #endregion
                                
                // Sanity check
                Assert.Equal(2, asm.Children.ApplyRangeLayoutConstraintCollection.Count());
            });
        }


        [Fact]
        public void Elaborator_RLC_InstAsOrigin()
        {
            project.PerformInTransaction(delegate
            {
                string pathComponentAssembly = "/@A_ElaboratorTest|kind=ComponentAssemblies|relpos=0/CA_ElabTest_RLC_InstAsOrigin|kind=ComponentAssembly|relpos=0";
                var objComponentAssembly = project.get_ObjectByPath(pathComponentAssembly);
                Assert.NotNull(objComponentAssembly);

                var asm = CyPhyClasses.ComponentAssembly.Cast(objComponentAssembly);
                Assert.NotNull(asm);

                var relativeLayoutConstraint_InstAsOrigin = asm.Children.RelativeLayoutConstraintCollection.First(rlc => rlc.Name == "RLC__InstAsOrigin");
                Assert.NotNull(relativeLayoutConstraint_InstAsOrigin);

                var CInst__Resistor_R0603 = asm.Children.ComponentCollection.First(c => c.Name == "CInst__Resistor_R0603");
                Assert.NotNull(CInst__Resistor_R0603);

                var CInst__SpicySingleOpAmp = asm.Children.ComponentCollection.First(c => c.Name == "CInst__SpicySingleOpAmp");
                Assert.NotNull(CInst__SpicySingleOpAmp);


                // Run Elaborator
                var elaborator = new CyPhyElaborateCS.CyPhyElaborateCSInterpreter();
                bool result = elaborator.RunInTransaction(project, asm.Impl as MgaFCO, null, 128);
                Assert.True(result);


                // Formerly ComponentRefs, these objects should now be Component instances
                var CRef__SpicySingleOpAmp = asm.Children.ComponentCollection.First(c => c.Name == "CRef__SpicySingleOpAmp");
                Assert.NotNull(CRef__SpicySingleOpAmp);
                                
                #region Check RLC__InstAsOrigin
                Assert.Equal(1,
                             relativeLayoutConstraint_InstAsOrigin.DstConnections
                                                                  .ApplyRelativeLayoutConstraintCollection
                                                                  .Where(c => c.DstEnd.ID == CInst__Resistor_R0603.ID)
                                                                  .Count());

                Assert.Equal(1,
                             relativeLayoutConstraint_InstAsOrigin.DstConnections
                                                                  .ApplyRelativeLayoutConstraintCollection
                                                                  .Where(c => c.DstEnd.ID == CRef__SpicySingleOpAmp.ID)
                                                                  .Count());

                Assert.Equal(1,
                             relativeLayoutConstraint_InstAsOrigin.SrcConnections
                                                                  .RelativeLayoutConstraintOriginCollection
                                                                  .Where(c => c.SrcEnd.ID == CInst__SpicySingleOpAmp.ID)
                                                                  .Count());
                #endregion

                // Sanity check
                Assert.Equal(1, asm.Children.RelativeLayoutConstraintOriginCollection.Count());
                Assert.Equal(2, asm.Children.ApplyRelativeLayoutConstraintCollection.Count());                
            });
        }

        [Fact]
        [Trait("this","thing")]
        public void UsePreRoutedLayout()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string OutputDir = Path.Combine(TestPath,
                                            "output",
                                            TestName);

            string TestbenchPath = "/@TestBenches/PR_PreRoute";

            RunInterpreterMain(OutputDir,
                                TestbenchPath,
                                new CyPhy2Schematic.CyPhy2Schematic_Settings() { doPlaceRoute = "true" });
            
            // Load outputs and check the dictionary for expected stuff.
            var nameLayoutInput = "layout-input.json";
            var pathLayoutInput = Path.Combine(OutputDir, nameLayoutInput);
            Assert.True(File.Exists(pathLayoutInput), "Failed to generate layout-input.json");

            var json = JObject.Parse(File.ReadAllText(pathLayoutInput));

            var packages = json["packages"];
            var spaceClaims = packages.Where(p =>    p["name"].ToString().Equals("LedDriver")
                                                  && p["package"].ToString().Equals("__spaceClaim__"));
            Assert.Equal(2, spaceClaims.Count());

            var signals = json["signals"];
            var signalsWithWires = signals.Where(s => s["wires"] != null);
            Assert.Equal(2, signalsWithWires.Count());

            foreach (var wires in signalsWithWires.SelectMany(s => s["wires"]))
            {
                Assert.NotNull(wires["x1"]);
                Assert.NotNull(wires["y1"]);
                Assert.NotNull(wires["x2"]);
                Assert.NotNull(wires["y2"]);
                Assert.NotNull(wires["width"]);
                Assert.NotNull(wires["layer"]);
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
}
