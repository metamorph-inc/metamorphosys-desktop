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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Xunit;
using System.IO;
using GME.MGA;
using GME.CSharp;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using META;
using SystemCParser;
using SystemCAttributesClass = ISIS.GME.Dsml.CyPhyML.Classes.SystemCPort.AttributesClass;

namespace ComponentAndArchitectureTeamTest
{
    public class ComponentAuthoringFixture : IDisposable
    {
        public string mgaPath;

        public ComponentAuthoringFixture()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(ComponentAuthoring.existingxmePath, out mgaConnectionString);
            mgaPath = mgaConnectionString.Substring("MGA=".Length);

            Assert.True(File.Exists(Path.GetFullPath(mgaPath)),
                        String.Format("{0} not found. Model import may have failed.", mgaPath));

            proj = new MgaProject();
            bool ro_mode;
            proj.Open("MGA=" + Path.GetFullPath(mgaPath), out ro_mode);
            proj.EnableAutoAddOns(true);
        }

        public void Dispose()
        {
            proj.Save();
            proj.Close();
            proj = null;
        }

        public MgaProject proj { get; private set; }
    }

    public class ComponentAuthoring : IUseFixture<ComponentAuthoringFixture>
    {
        #region Path Variables
        // this first set is for the created and deleted model
        public static string testcreatepath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "ComponentAuthoringToolTests");

        public static string testpartpath = Path.Combine(
            META.VersionInfo.MetaPath,
            "models",
            "MassSpringDamper",
            "Creo Files for MSD",
            "damper.prt.36");
        public static string testiconpath = Path.Combine(
            META.VersionInfo.MetaPath,
            "meta",
            "CyPhyML",
            "icons",
            "AcousticPowerPort.png");

        public static string testmfgmdlpath = Path.Combine(
            META.VersionInfo.MetaPath,
            "models",
            "Box",
            "components",
            "BottomPlate120x100",
            "generic_cots_mfg.xml");

        public static string RenamedFileName = "FileNameChanged.prt";
        public static string RenamedFileNameWithoutExtension = Path.GetFileNameWithoutExtension(RenamedFileName);
        public static string RenamedFileNameRelativePath = Path.Combine("CAD", RenamedFileName);

        // this set is the saved and reused model
        public static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "models",
            "ComponentsAndArchitectureTeam",
            "ComponentAuthoringTool"
            );
        public static readonly string existingxmePath = Path.Combine(
            testPath,
            "ComponentAuthoringTool.xme"
            );
        #endregion

        #region Fixture
        ComponentAuthoringFixture fixture;
        public void SetFixture(ComponentAuthoringFixture data)
        {
            fixture = data;
        }
        #endregion

        [Fact]
        public void TestComponentInstance()
        {
            // these are the actual steps of the test
            fixture.proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(fixture.proj);
                CyPhy.Component testcomp = fixture.proj.GetComponentsByName("JustAComponent").First();
                string ret_msg;

                // new instance of the class to test
                CyPhyComponentAuthoring.CyPhyComponentAuthoringInterpreter testcai = new CyPhyComponentAuthoring.CyPhyComponentAuthoringInterpreter();
                // these class variables need to be set to avoid NULL references
                var CurrentObj = testcomp.Impl as MgaFCO;

                // We are in a Component, check valid preconditions
                Assert.True(testcai.CheckPreConditions(CurrentObj, out ret_msg),
                            String.Format("{0} should allow CAT to run in it, but it is not. Err=({1})", testcomp.Name, ret_msg)
                            );

                // create a subcomponent and set it as current 
                CyPhy.Component testsubcomp = fixture.proj.GetComponentsByName("ComponentSubtype").First();
                CurrentObj = testsubcomp.Impl as MgaFCO;
                // We are in a sub-Component, check valid preconditions
                Assert.True(testcai.CheckPreConditions(CurrentObj, out ret_msg),
                            String.Format("{0} should allow CAT to run in it, but it is not. Err=({1})", testsubcomp.Name, ret_msg)
                            );

                // create a component instance and set it as current 
                CyPhy.Component testcompinst = fixture.proj.GetComponentsByName("ComponentInstance").First();
                CurrentObj = testcompinst.Impl as MgaFCO;
                // We are in a Component instance, check valid preconditions
                Assert.False(testcai.CheckPreConditions(CurrentObj, out ret_msg),
                            String.Format("{0} should NOT allow CAT to run in it, yet it is. Err=({1})", testcompinst.Name, ret_msg)
                            );

                // create a library object and set it as current 
                CyPhy.Component testlibcomp = fixture.proj.GetComponentsByName("LibComponent").First();
                CurrentObj = testlibcomp.Impl as MgaFCO;
                // We are in a library object, check valid preconditions
                Assert.False(testcai.CheckPreConditions(CurrentObj, out ret_msg),
                            String.Format("{0} should NOT allow CAT to run in it, yet it is. Err=({1})", testlibcomp.Name, ret_msg)
                            );
            });
        }

        [Fact]
        public void TestCADImportResourceNaming()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string path_Test = Path.Combine(testcreatepath, TestName);
            string path_Mga = Path.Combine(path_Test, TestName + ".mga");

            // delete any previous test results
            if (Directory.Exists(path_Test))
            {
                Directory.Delete(path_Test, true);
            }

            // create a new blank project
            MgaProject proj = new MgaProject();
            proj.Create("MGA=" + path_Mga, "CyPhyML");

            // these are the actual steps of the test
            proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(proj);
                CyPhy.Components components = CyPhyClasses.Components.Create(rf);
                CyPhy.Component testcomp = CyPhyClasses.Component.Create(components);

                // new instance of the class to test
                CyPhyComponentAuthoring.Modules.CADModelImport testcam = new CyPhyComponentAuthoring.Modules.CADModelImport();
                // these class variables need to be set to avoid NULL references
                testcam.SetCurrentComp(testcomp);
                testcam.CurrentProj = proj;
                testcam.CurrentObj = testcomp.Impl as MgaFCO;

                // call the module with a part file to skip the CREO steps
                testcam.ImportCADModel(testpartpath);

                // verify results
                // insure the resource path was created correctly
                var correct_name = testcomp.Children.ResourceCollection.Where(p => p.Name == "damper.prt.36").First();
                Assert.True(correct_name.Attributes.Path == "CAD\\damper.prt.36",
                            String.Format("{0} should have had value {1}; instead found {2}", correct_name.Name, "CAD\\damper.prt.36", correct_name.Attributes.Path)
                            );
                // insure the part file was copied to the back-end folder correctly
                var getcadmdl = testcomp.Children.CADModelCollection.First();
                string returnedpath;
                getcadmdl.TryGetResourcePath(out returnedpath, ComponentLibraryManager.PathConvention.ABSOLUTE);
                Assert.True(File.Exists(returnedpath),
                    String.Format("Could not find the source file for the created resource, got {0}", returnedpath));
            });
            proj.Save();
            proj.Close();
        }

        [Fact]
        public void TestCATDialogBoxCentering()
        {
            // these are the actual steps of the test
            fixture.proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(fixture.proj);
                CyPhy.Component testcomp = fixture.proj.GetComponentsByName("JustAComponent").First();

                // new instance of the class to test
                CyPhyComponentAuthoring.CyPhyComponentAuthoringInterpreter testcai = new CyPhyComponentAuthoring.CyPhyComponentAuthoringInterpreter();

                // Call the create dialog box method
                testcai.PopulateDialogBox(true);
                // Get the dialog box location and verify it is in the center of the screen
                Assert.True(testcai.ThisDialogBox.StartPosition == FormStartPosition.CenterScreen,
                            String.Format("CAT dialog box is not in the center of the screen")
                            );
            });
        }

        [Fact]
        public void TestAddCustomIconTool()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string path_Test = Path.Combine(testcreatepath, TestName);
            string path_Mga = Path.Combine(path_Test, TestName + ".mga");

            // delete any previous test results
            if (Directory.Exists(path_Test))
            {
                Directory.Delete(path_Test, true);
            }

            // create a new blank project
            MgaProject proj = new MgaProject();
            proj.Create("MGA=" + path_Mga, "CyPhyML");

            // these are the actual steps of the test
            proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(proj);
                CyPhy.Components components = CyPhyClasses.Components.Create(rf);
                CyPhy.Component testcomp = CyPhyClasses.Component.Create(components);

                // new instance of the class to test
                CyPhyComponentAuthoring.Modules.CustomIconAdd CATModule = new CyPhyComponentAuthoring.Modules.CustomIconAdd();

                //// these class variables need to be set to avoid NULL references
                CATModule.SetCurrentComp(testcomp);
                CATModule.CurrentProj = proj;
                CATModule.CurrentObj = testcomp.Impl as MgaFCO;

                // call the primary function directly
                CATModule.AddCustomIcon(testiconpath);

                // verify results
                // 1. insure the icon file was copied to the back-end folder correctly
                string iconAbsolutePath = Path.Combine(testcomp.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE), "Icon.png");
                Assert.True(File.Exists(iconAbsolutePath),
                    String.Format("Could not find the source file for the created resource, got {0}", iconAbsolutePath));

                // 2. insure the resource path was created correctly
                var iconResource = testcomp.Children.ResourceCollection.Where(p => p.Name == "Icon.png").First();
                Assert.True(iconResource.Attributes.Path == "Icon.png",
                            String.Format("{0} Resource should have had value {1}; instead found {2}", iconResource.Name, "Icon.png", iconResource.Attributes.Path)
                            );

                // 3. Verify the registry entry exists
                string expected_registry_entry = Path.Combine(testcomp.GetDirectoryPath(ComponentLibraryManager.PathConvention.REL_TO_PROJ_ROOT), "Icon.png");
                string registry_entry = (testcomp.Impl as GME.MGA.IMgaFCO).get_RegistryValue("icon");
                Assert.True(registry_entry.Equals(expected_registry_entry),
                            String.Format("Registry should have had value {0}; instead found {1}", expected_registry_entry, registry_entry)
                            );
            });
            proj.Save();
            proj.Close();
        }

        [Fact]
        public void TestAddDocumentation()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string path_Test = Path.Combine(testcreatepath, TestName);
            string path_Mga = Path.Combine(path_Test, TestName + ".mga");

            // delete any previous test results
            if (Directory.Exists(path_Test))
            {
                Directory.Delete(path_Test, true);
            }

            // create a new blank project
            MgaProject proj = new MgaProject();
            proj.Create("MGA=" + path_Mga, "CyPhyML");

            // these are the actual steps of the test
            proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(proj);
                CyPhy.Components components = CyPhyClasses.Components.Create(rf);
                CyPhy.Component testcomp = CyPhyClasses.Component.Create(components);

                // new instance of the class to test
                var CATModule = new CyPhyComponentAuthoring.Modules.AddDocumentation()
                {
                    CurrentObj = testcomp.Impl as MgaFCO,
                    CurrentProj = proj
                };
                CATModule.SetCurrentComp(testcomp);

                var path_DocToAdd = Path.Combine(META.VersionInfo.MetaPath,
                                                 "src",
                                                 "CyPhyComponentAuthoring",
                                                 "Modules",
                                                 "AddDocumentation.cs");

                // Import it once and check
                {
                    CATModule.AddDocument(path_DocToAdd);

                    var resource = testcomp.Children.ResourceCollection.First(r => r.Name.Equals("AddDocumentation.cs"));
                    Assert.Equal(Path.GetFileName(path_DocToAdd), resource.Name);
                    var relpath_Resource = resource.Attributes.Path;
                    var path_Resource = Path.Combine(testcomp.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE),
                                                     relpath_Resource);
                    Assert.True(File.Exists(path_Resource));
                }

                // Import again and make sure the collision is prevented.
                {
                    CATModule.AddDocument(path_DocToAdd);

                    var resource = testcomp.Children.ResourceCollection.First(r => r.Name.Equals("AddDocumentation_(1).cs"));
                    Assert.Equal("AddDocumentation_(1).cs", resource.Name);
                    var relpath_Resource = resource.Attributes.Path;
                    var path_Resource = Path.Combine(testcomp.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE),
                                                     relpath_Resource);
                    Assert.True(File.Exists(path_Resource));
                }
            });
            proj.Save();
            proj.Close();
        }
        
        [Fact]
        public void SpiceImport()
        {
            String nameTest = "SpiceImport";

            fixture.proj.PerformInTransaction(delegate
            {
                // Set the SPICE file name
                string shortSpiceFileName = "test1.cir";

                // Set the SPICE file path
                string spiceFilesDirectoryPath = getSourceSpiceFileDirectory();

                // Set the combined path to the SPICE file
                string fullSpiceFileName = Path.Combine(spiceFilesDirectoryPath, shortSpiceFileName);

                var rf = CyPhyClasses.RootFolder.GetRootFolder(fixture.proj);
                var cf = CyPhyClasses.Components.Create(rf);
                cf.Name = nameTest;
                CyPhy.Component component = CyPhyClasses.Component.Create(cf);
                component.Name = nameTest;

                var catModule = new CyPhyComponentAuthoring.Modules.SpiceModelImport();

                catModule.ImportSpiceModel(component, fullSpiceFileName);

                // Check that one and only one CyPhy SPICE model exists in the component.
                Assert.Equal(1, component.Children.SPICEModelCollection.Count());

                // Check that the CyPhy SPICE model has the correct class.
                var newSpiceModel = component.Children.SPICEModelCollection.First();
                Assert.True(newSpiceModel.Attributes.Class == "X:RES_0603");

                // Check that the CyPhy SPICE model has two pins.
                Assert.Equal(2, newSpiceModel.Children.SchematicModelPortCollection.Count());

                // Check the spice model's pins names and numbers.
                Dictionary<string, int> pinList = new Dictionary<string, int>();
                foreach (var newPort in newSpiceModel.Children.SchematicModelPortCollection)
                {
                    pinList.Add(newPort.Name, newPort.Attributes.SPICEPortNumber);
                }
                Assert.Equal(0, pinList["1"]);  // Schematic pin 1 is SPICE pin 0
                Assert.Equal(1, pinList["2"]);  // Schematic pin 2 is SPICE pin 1

                // Check that the component has two schematic pins
                Assert.Equal(2, component.Children.SchematicModelPortCollection.Count());

                // Check that there is one resource.
                Assert.Equal(1, component.Children.ResourceCollection.Count());
                var newResource = component.Children.ResourceCollection.First();

                // Check that the resource is named correctly.
                Assert.Equal("SPICEModelFile", newResource.Name);

                // Check that the resource has the copied-SPICE-file's relative path.
                Assert.Equal(Path.Combine("Spice", "test1.cir"), newResource.Attributes.Path);

                // Check connection between the SPICE model and the resource
                var srcConnections = newSpiceModel.SrcConnections.UsesResourceCollection;
                var dstConnections = newSpiceModel.DstConnections.UsesResourceCollection;
                var connUnion = srcConnections.Union(dstConnections);
                int connectionCount = 0;
                foreach( var connection in connUnion )
                {
                    if ((connection.DstEnd.ID == newResource.ID) || 
                        (connection.SrcEnd.ID == newResource.ID))
                    {
                        connectionCount += 1;
                    }
                }
                Assert.Equal(1, connectionCount );

                // Check that there are two port compositions
                Assert.Equal(2, component.Children.PortCompositionCollection.Count());

                // Check the connections between the component schematic pins and the SPICE model pins
                Dictionary<string,string> pinMap = new Dictionary<string,string>();
                foreach (var newPortComp in component.Children.PortCompositionCollection)
                {
                    var dstId = newPortComp.DstEnd.ID;
                    var srcId = newPortComp.SrcEnd.ID;

                    // Prove that this port composition connects a schematic pin
                    bool foundSchematicPin = false;
                    foreach (var newSchematicPin in component.Children.SchematicModelPortCollection)
                    {
                        var pinId = newSchematicPin.ID;
                        if ((pinId == dstId) || (pinId == srcId))
                        {
                            foundSchematicPin = true;
                            break;
                        }
                    }
                    Assert.True(foundSchematicPin);

                    // Prove that it also connects to a SPICE model pin
                    bool foundSpicePin = false;
                    foreach (var newSpicePin in newSpiceModel.Children.SchematicModelPortCollection)
                    {
                        var pinId = newSpicePin.ID;
                        if ((pinId == dstId) || (pinId == srcId))
                        {
                            foundSpicePin = true;
                            break;
                        }
                    }
                    Assert.True(foundSpicePin);
                }

                // Check that there is one CyPhy SPICE parameter.
                Assert.Equal(1, component.Children.SPICEModelParameterMapCollection.Count());
                var newParamMap = component.Children.SPICEModelParameterMapCollection.First();

                Assert.Equal(1, newSpiceModel.Children.SPICEModelParameterCollection.Count());
                var newParam = newSpiceModel.Children.SPICEModelParameterCollection.First();

                // Check that there is one component property.
                Assert.Equal(1, component.Children.PropertyCollection.Count());
                var newProp = component.Children.PropertyCollection.First();

                // Check that the spice model parameter and the component property are connected.
                List<string> mapIds = new List<string> {
                    newParamMap.SrcEnd.ID,
                    newParamMap.DstEnd.ID};
                mapIds.Sort();
                List<string> endIds = new List<string> {
                    newParam.ID,
                    newProp.ID};
                endIds.Sort();
                Assert.Equal(mapIds, endIds);

                // Create a path to the current component folder
                string PathForComp = component.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE);

                // Verify that the SPICE file has been copied to its destination
                string destinationFilePath = Path.Combine( PathForComp, newResource.Attributes.Path );
                string sourceFilePath = fullSpiceFileName;
                Assert.True(FileCompare( sourceFilePath, destinationFilePath ));
            });
        }

        [Fact]
        public void CheckMissingSpiceFileException()
        {
            String nameTest = "CheckMissingSpiceFileException";

            fixture.proj.PerformInTransaction(delegate
            {
                // Set the SPICE file name
                string shortSpiceFileName = "this_file_should_not_exist.cir";

                // Set the SPICE file's path
                string spiceFilesDirectoryPath = getSourceSpiceFileDirectory();

                // Set the combined path to the missing SPICE file
                string fullSpiceFileName = Path.Combine(spiceFilesDirectoryPath, shortSpiceFileName);

                var rf = CyPhyClasses.RootFolder.GetRootFolder(fixture.proj);
                var cf = CyPhyClasses.Components.Create(rf);
                cf.Name = nameTest;
                CyPhy.Component component = CyPhyClasses.Component.Create(cf);
                component.Name = nameTest;

                var catModule = new CyPhyComponentAuthoring.Modules.SpiceModelImport();

                Assert.DoesNotThrow(delegate
                {
                    catModule.ImportSpiceModel(component, fullSpiceFileName);
                });
                // Assert.True(expectedException.Message.Contains("does not exist"));
            });
        }

        
        //--------------------------------------------------------------------------------------------------
        /// <summary>
        /// FileCompare -- compares the contents of two files for equality.
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns>true if the files are the same</returns>
        private bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }

        /// <summary>
        /// getSourceSpiceFileDirectory
        /// </summary>
        /// <returns>The path of the source SPICE-file directory.</returns>
        private string getSourceSpiceFileDirectory()
        {
            // Set the SPICE file path to \tonka\tonka\models\SpiceTestModels
            string part1 = Path.Combine(META.VersionInfo.MetaPath, "..", "tonka", "models");
            string rVal = Path.Combine(part1, "SpiceTestModels");

            return rVal;
        }

        /// <summary>
        /// getScTestFileDirectory
        /// </summary>
        /// <returns>The path of the SystemC test-file directory.</returns>
        private string getScTestFileDirectory()
        {
            // Set the SystemC test-file path to \tonka\tonka\models\SystemCTestModels
            string part1 = Path.Combine(META.VersionInfo.MetaPath, "..", "tonka", "models");
            string rVal = Path.Combine(part1, "SystemCTestModels");

            return rVal;
        }



        [Fact]
        public void TestAddMfgModelTool()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string path_Test = Path.Combine(testcreatepath, TestName);
            string path_Mga = Path.Combine(path_Test, TestName + ".mga");

            // delete any previous test results
            if (Directory.Exists(path_Test))
            {
                Directory.Delete(path_Test, true);
            }

            // create a new blank project
            MgaProject proj = new MgaProject();
            proj.Create("MGA=" + path_Mga, "CyPhyML");

            // these are the actual steps of the test
            proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(proj);
                CyPhy.Components components = CyPhyClasses.Components.Create(rf);
                CyPhy.Component testcomp = CyPhyClasses.Component.Create(components);

                // new instance of the class to test
                CyPhyComponentAuthoring.Modules.MfgModelImport CATModule = new CyPhyComponentAuthoring.Modules.MfgModelImport();

                //// these class variables need to be set to avoid NULL references
                CATModule.SetCurrentComp(testcomp);
                CATModule.CurrentProj = proj;
                CATModule.CurrentObj = testcomp.Impl as MgaFCO;

                // call the primary function directly
                CATModule.import_manufacturing_model(testmfgmdlpath);

                // verify results
                // 1. insure the mfg file was copied to the backend folder correctly
                // insure the part file was copied to the backend folder correctly
                var getmfgmdl = testcomp.Children.ManufacturingModelCollection.First();
                string returnedpath;
                getmfgmdl.TryGetResourcePath(out returnedpath, ComponentLibraryManager.PathConvention.ABSOLUTE);
                string demanglepathpath = returnedpath.Replace("\\", "/");
                Assert.True(File.Exists(demanglepathpath),
                    String.Format("Could not find the source file for the created resource, got {0}", demanglepathpath));

                // 2. insure the resource path was created correctly
                var mfgResource = testcomp.Children.ResourceCollection.Where(p => p.Name == "generic_cots_mfg.xml").First();
                Assert.True(mfgResource.Attributes.Path == Path.Combine("Manufacturing", "generic_cots_mfg.xml"),
                            String.Format("{0} Resource should have had value {1}; instead found {2}", mfgResource.Name, "generic_cots_mfg.xml", mfgResource.Attributes.Path)
                            );
            });
            proj.Save();
            proj.Close();
        }

        [Fact]
        public void TestCADFileReNaming()
        {
            string TestName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string path_Test = Path.Combine(testcreatepath, TestName);
            string path_Mga = Path.Combine(path_Test, TestName + ".mga");

            // delete any previous test results
            if (Directory.Exists(path_Test))
            {
                Directory.Delete(path_Test, true);
            }

            // create a new blank project
            MgaProject proj = new MgaProject();
            proj.Create("MGA=" + path_Mga, "CyPhyML");

            // these are the actual steps of the test
            proj.PerformInTransaction(delegate
            {
                // create the environment for the Component authoring class
                CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder(proj);
                CyPhy.Components components = CyPhyClasses.Components.Create(rf);
                CyPhy.Component testcomp = CyPhyClasses.Component.Create(components);

                // Import a CAD file into the test project
                CyPhyComponentAuthoring.Modules.CADModelImport importcam = new CyPhyComponentAuthoring.Modules.CADModelImport();
                // these class variables need to be set to avoid NULL references
                importcam.SetCurrentComp(testcomp);
                importcam.CurrentProj = proj;
                importcam.CurrentObj = testcomp.Impl as MgaFCO;

                // import the CAD file
                importcam.ImportCADModel(testpartpath);
                // Get a path to the imported Cad file
                var getcadmdl = testcomp.Children.CADModelCollection.First();
                string importedpath;
                getcadmdl.TryGetResourcePath(out importedpath, ComponentLibraryManager.PathConvention.ABSOLUTE);

                // Rename the CAD file
                CyPhyComponentAuthoring.Modules.CADFileRename renamecam = new CyPhyComponentAuthoring.Modules.CADFileRename();
                // these class variables need to be set to avoid NULL references
                renamecam.SetCurrentComp(testcomp);
                renamecam.CurrentProj = proj;
                renamecam.CurrentObj = testcomp.Impl as MgaFCO;

                // call the module with a part file and the new file name
                renamecam.RenameCADFile(importedpath, RenamedFileNameWithoutExtension);

                // verify results
                // 1. Verify the file was renamed
                var renamecadmdl = testcomp.Children.CADModelCollection.First();
                string renamedpath;
                renamecadmdl.TryGetResourcePath(out renamedpath, ComponentLibraryManager.PathConvention.ABSOLUTE);
                Assert.True(File.Exists(renamedpath),
                    String.Format("Could not find the renamed CAD file, found {0}", renamedpath));

                // 2. Verify the Model was renamed
                bool model_not_found = false;
                try
                {
                    var model_name = testcomp.Children.CADModelCollection.Where(p => p.Name == RenamedFileName).First();
                }
                catch (Exception ex)
                {
                    model_not_found = true;
                }
                Assert.False(model_not_found,
                            String.Format("No CAD model found with the renamed name {0}", RenamedFileName)
                            );

                // 3. Verify the resource name and path were changed
                CyPhy.Resource resource_name = null;
                bool resource_not_found = false;
                try
                {
                    resource_name = testcomp.Children.ResourceCollection.Where(p => p.Name == RenamedFileName).First();
                }
                catch (Exception ex)
                {
                    resource_not_found = true;
                }
                Assert.False(resource_not_found,
                            String.Format("No resource found with the renamed name {0}", RenamedFileName)
                            );
                Assert.True(resource_name.Attributes.Path == RenamedFileNameRelativePath,
                            String.Format("{0} should have had value {1}; instead found {2}", resource_name.Name, RenamedFileNameRelativePath, resource_name.Attributes.Path)
                            );
            });
            proj.Save();
            proj.Close();
        }

        public struct portData_s
        {
            public string name;
            public SystemCAttributesClass.Directionality_enum direction;
            public SystemCAttributesClass.DataType_enum type;
            public int dimension;

            public portData_s(
                string name, 
                SystemCAttributesClass.Directionality_enum direction, 
                SystemCAttributesClass.DataType_enum type, 
                int dimension)
            {
                this.name = name;
                this.direction = direction;
                this.type = type;
                this.dimension = dimension;
            }
        };


        [Fact]
        public void SystemCImport()     // MOT-419
        {
            String nameTest = "SystemCImport";

            fixture.proj.PerformInTransaction(delegate
            {
                // SystemC header file name
                string shortScHeaderFileName = "ccled.h";

                // SystemC source file name
                string shortScSourceFileName = "ccled.cpp";

                // Set the SystemC file path
                string scFilesDirectoryPath = getScTestFileDirectory();

                // Set the combined path to the SPICE file
                string[] fullSystemCFileNames = 
                {
                    Path.Combine(scFilesDirectoryPath, shortScHeaderFileName),
                    Path.Combine(scFilesDirectoryPath, shortScSourceFileName)
                };

                Assert.True( File.Exists( fullSystemCFileNames[ 0 ] ) );
                Assert.True( File.Exists( fullSystemCFileNames[ 1 ] ) );


                var rf = CyPhyClasses.RootFolder.GetRootFolder(fixture.proj);
                var cf = CyPhyClasses.Components.Create(rf);
                cf.Name = nameTest;
                CyPhy.Component component = CyPhyClasses.Component.Create(cf);
                component.Name = nameTest;

                var catModule = new CyPhyComponentAuthoring.Modules.SystemCModelImport();

                catModule.ImportSystemCModel(component, fullSystemCFileNames);

                // Check that one and only one CyPhy SystemC model exists in the component.
                Assert.Equal(1, component.Children.SystemCModelCollection.Count());

                var newSystemCModel = component.Children.SystemCModelCollection.First();

                // Check that the CyPhy SystemC model has 13 ports.
                Assert.Equal(13, newSystemCModel.Children.SystemCPortCollection.Count());

                portData_s[] expectedPortData = 
                {
                    new portData_s( "le", SystemCAttributesClass.Directionality_enum.@in, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "oeBar", SystemCAttributesClass.Directionality_enum.@in, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),

                    new portData_s( "out0", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out1", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out2", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out3", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out4", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out5", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out6", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "out7", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),

                    new portData_s( "sdi", SystemCAttributesClass.Directionality_enum.@in, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                    new portData_s( "sdi_clk", SystemCAttributesClass.Directionality_enum.@in, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),

                    new portData_s( "sdo", SystemCAttributesClass.Directionality_enum.@out, SystemCAttributesClass.DataType_enum.sc_logic, 1 ),
                };


                // Check the SystemC model's port names, etc.
                Dictionary<string, portData_s> portDict = new Dictionary<string, portData_s>();
                foreach (var newPort in expectedPortData )
                {
                    portDict.Add(newPort.name, newPort);
                }

                // Check that each port matches an expected set of attributes.
                foreach (var newPort in newSystemCModel.Children.SystemCPortCollection)
                {
                    portData_s expectedPort = portDict[newPort.Name];
                    Assert.NotNull(expectedPort);
                    Assert.Equal<string>(expectedPort.name, newPort.Name);
                    Assert.Equal(expectedPort.type, newPort.Attributes.DataType);
                    Assert.Equal(expectedPort.direction, newPort.Attributes.Directionality);
                    Assert.Equal(expectedPort.dimension, newPort.Attributes.DataTypeDimension);
                }

                // Check that there are two resources.
                Assert.Equal(2, component.Children.ResourceCollection.Count());
                var firstResource = component.Children.ResourceCollection.First();
                var secondResource = component.Children.ResourceCollection.ElementAt(1);
                var sourceResource = firstResource;
                var headerResource = secondResource;
                if (Path.GetExtension(sourceResource.Attributes.Path) == ".h")
                {
                    headerResource = sourceResource;
                    sourceResource = secondResource;
                }

                // Check the sourceResource file.

                // Check that the it's named correctly.
                Assert.Equal("SystemCModelSourceFile", sourceResource.Name);

                // Check that the resource has the copied-SystemC-file's relative path.
                Assert.Equal(Path.Combine("SystemC", "ccled.cpp"), sourceResource.Attributes.Path);

                // Check connection between the SystemC model and the sourceResource
                var srcConnections = sourceResource.SrcConnections.UsesResourceCollection;
                var dstConnections = sourceResource.DstConnections.UsesResourceCollection;
                var connUnion = srcConnections.Union(dstConnections);
                int connectionCount = 0;
                foreach (var connection in connUnion)
                {
                    if ((connection.DstEnd.ID == sourceResource.ID) ||
                        (connection.SrcEnd.ID == sourceResource.ID))
                    {
                        connectionCount += 1;
                    }
                }
                Assert.Equal(1, connectionCount);

                // Check the header resource file

                // Check that the resource is named correctly.
                Assert.Equal("SystemCModelHeaderFile", headerResource.Name);

                // Check that the resource has the copied-SystemC-file's relative path.
                Assert.Equal(Path.Combine("SystemC", "ccled.h"), headerResource.Attributes.Path);

                // Check connection between the SystemC model and the headerResource
                srcConnections = headerResource.SrcConnections.UsesResourceCollection;
                dstConnections = headerResource.DstConnections.UsesResourceCollection;
                connUnion = srcConnections.Union(dstConnections);
                connectionCount = 0;
                foreach (var connection in connUnion)
                {
                    if ((connection.DstEnd.ID == headerResource.ID) ||
                        (connection.SrcEnd.ID == headerResource.ID))
                    {
                        connectionCount += 1;
                    }
                }
                Assert.Equal(1, connectionCount);

                //// Check that there are 13 port compositions
                Assert.Equal(13, newSystemCModel.Children.SystemCPortCollection.Count());

                // Create a path to the current component folder
                string PathForComp = component.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE);

                // Verify that the systemC header file has been copied to its destination
                string destinationFilePath = Path.Combine(PathForComp, headerResource.Attributes.Path);
                string sourceFilePath = fullSystemCFileNames[0];
                Assert.True(FileCompare(sourceFilePath, destinationFilePath));

                // Verify that the systemC source file has been copied to its destination
                destinationFilePath = Path.Combine(PathForComp, sourceResource.Attributes.Path);
                sourceFilePath = fullSystemCFileNames[1];
                Assert.True(FileCompare(sourceFilePath, destinationFilePath));
            });
        }
    }
}
