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
using System.IO;
using System.Linq;
using System.Text;
using CyPhyComponentImporter;
using GME.CSharp;
using GME.MGA;
using GME.MGA.Core;
using GME.MGA.Meta;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using CyPhyML = ISIS.GME.Dsml.CyPhyML.Interfaces;
using ISIS.GME.Common.Interfaces;
using META;

namespace CyPhyDesignImporter
{
    public class AVMDesignImporter : AVM2CyPhyML.AVM2CyPhyMLBuilder
    {
        string projroot;

        public AVMDesignImporter(GMEConsole console, IMgaProject project, object messageConsoleParameter = null)
            : base(CyPhyClasses.RootFolder.GetRootFolder((MgaProject)project), messageConsoleParameter)
        {
            projroot = Path.GetDirectoryName(project.ProjectConnStr.Substring("MGA=".Length));
            init(true);
        }

        public Dictionary<string, CyPhy.Component> avmidComponentMap
        {
            get
            {
                // TODO memoize
                CyPhy.RootFolder rootFolder = ISIS.GME.Common.Utils.CreateObject<CyPhyClasses.RootFolder>(project.RootFolder as MgaObject);
                return CyPhyComponentImporterInterpreter.getCyPhyMLComponentDictionary_ByAVMID(rootFolder);
            }
        }

        public Model[] ImportFiles(string[] fileNames, DesignImportMode mode = AVMDesignImporter.DesignImportMode.CREATE_DS)
        {
            List<Model> ret = new List<Model>();
            CyPhy.RootFolder rootFolder = ISIS.GME.Common.Utils.CreateObject<CyPhyClasses.RootFolder>(project.RootFolder as MgaObject);
            Dictionary<string, CyPhy.Component> avmidComponentMap = CyPhyComponentImporterInterpreter.getCyPhyMLComponentDictionary_ByAVMID(rootFolder);

            foreach (var inputFilePath in fileNames)
            {
                var container = ImportFile(inputFilePath, mode);
                ret.Add(container);
            }
            return ret.ToArray();
        }

        public Model ImportFile(string inputFilePath, DesignImportMode mode = DesignImportMode.CREATE_DS)
        {
            writeMessage(String.Format("Importing {0}", inputFilePath), MessageType.INFO);

            UnzipToTemp unzip = null;
            bool bZipArchive = Path.GetExtension(inputFilePath).Equals(".adp");
            if (bZipArchive)
            {
                unzip = new UnzipToTemp(null);
                List<string> entries = unzip.UnzipFile(inputFilePath);
                inputFilePath = entries.Where(entry => Path.GetDirectoryName(entry) == "" && entry.ToLower().EndsWith(".adm")).FirstOrDefault();
                if (inputFilePath != null)
                {
                    inputFilePath = Path.Combine(unzip.TempDirDestination, inputFilePath);
                }
            }

            Model rtn = null;

            avm.Design ad_import = null;
            using (unzip)
            {
                using (StreamReader streamReader = new StreamReader(inputFilePath))
                {
                    ad_import = CyPhyDesignImporterInterpreter.DeserializeAvmDesignXml(streamReader);
                }
                if (ad_import == null)
                {
                    throw new Exception("Could not load ADM file.");
                }

                rtn = ImportDesign(ad_import, mode);

                // Copy artifacts
                if (bZipArchive)
                {
                    // Delete ADM file from tmp folder
                    File.Delete(inputFilePath);

                    var caRtn = rtn as CyPhy.ComponentAssembly;
                    var pathCA = caRtn.GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE);
                    DirectoryCopy(unzip.TempDirDestination, pathCA, true);
                }
            }

            return rtn;
        }

        // From http://msdn.microsoft.com/en-us/library/bb762914(v=vs.110).aspx
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }


        public enum DesignImportMode
        {
            CREATE_CAS,
            CREATE_DS,
            CREATE_CA_IF_NO_DS_CONCEPTS,
        }

        public Model ImportDesign(avm.Design ad_import, DesignImportMode mode = DesignImportMode.CREATE_DS)
        {
            // TODO: check ad_import.SchemaVersion
            CyPhy.DesignEntity cyphy_container;

            if (mode == DesignImportMode.CREATE_CA_IF_NO_DS_CONCEPTS)
            {
                bool containsNonCompound = false;
                Queue<avm.Container> containers = new Queue<avm.Container>();
                containers.Enqueue(ad_import.RootContainer);
                while (containers.Count > 0)
                {
                    avm.Container container = containers.Dequeue();
                    containsNonCompound |= container is avm.Optional || container is avm.Alternative;
                    foreach (var subcontainer in container.Container1)
                    {
                        containers.Enqueue(subcontainer);
                    }
                }
                if (containsNonCompound)
                {
                    cyphy_container = CreateDesignSpaceRoot(ad_import);
                }
                else
                {
                    cyphy_container = CreateComponentAssemblyRoot(ad_import);
                }
            }
            else if (mode == DesignImportMode.CREATE_CAS)
            {
                cyphy_container = CreateComponentAssemblyRoot(ad_import);
            }
            else if (mode == DesignImportMode.CREATE_DS)
            {
                cyphy_container = CreateDesignSpaceRoot(ad_import);
            }
            else
            {
                throw new Exception("Unrecognized mode " + mode.ToString());
            }

            var ad_container = ad_import.RootContainer;

            ImportContainer(cyphy_container, ad_container);

            processValues();
            processPorts();

            Dictionary<avm.ConnectorCompositionTarget, avm.ConnectorCompositionTarget> connectorMap = new Dictionary<avm.ConnectorCompositionTarget, avm.ConnectorCompositionTarget>();
            foreach (var obj in this._avmCyPhyMLObjectMap)
            {
                if (obj.Key is avm.ConnectorCompositionTarget)
                {
                    avm.ConnectorCompositionTarget ad_compositionTarget1 = (avm.ConnectorCompositionTarget)obj.Key;
                    foreach (var ad_compositionTarget2ID in ad_compositionTarget1.ConnectorComposition.Where(id => string.IsNullOrEmpty(id) == false))
                    {
                        var ad_compositionTarget2 = _idConnectorMap[ad_compositionTarget2ID];
                        var cyphy_target = _avmCyPhyMLObjectMap[ad_compositionTarget2]; // TODO: handle lookup failure
                        if (string.Compare(ad_compositionTarget1.ID, ad_compositionTarget2.ID) < 0)
                        {
                            continue;
                        }
                        makeConnection(obj.Value, cyphy_target, typeof(CyPhy.ConnectorComposition).Name);
                    }
                }
            }

            DoLayout();

            return (Model)cyphy_container;
        }

        private CyPhy.DesignContainer CreateDesignSpaceRoot(avm.Design ad_import)
        {
            CyPhy.DesignSpace ds;
            CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder((MgaProject)project);
            ds = rf.Children.DesignSpaceCollection.Where(d => d.Name == "DesignSpaces").FirstOrDefault();
            if (ds == null)
            {
                ds = CyPhyClasses.DesignSpace.Create(rf);
                ds.Name = "DesignSpaces";
            }

            CyPhy.DesignContainer cyphy_container = CyPhyClasses.DesignContainer.Create(ds);
            // container.Name = ad_import.Name; RootContainer has a name too
            int designID;
            if (int.TryParse(ad_import.DesignID, out designID))
            {
                cyphy_container.Attributes.ID = designID;
            }
            cyphy_container.Attributes.ContainerType = CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum.Compound;
            return cyphy_container;
        }

        private CyPhy.ComponentAssembly CreateComponentAssemblyRoot(avm.Design ad_import)
        {
            CyPhy.ComponentAssemblies cyphy_cas;
            CyPhy.RootFolder rf = CyPhyClasses.RootFolder.GetRootFolder((MgaProject)project);
            cyphy_cas = rf.Children.ComponentAssembliesCollection.Where(d => d.Name == typeof(CyPhyClasses.ComponentAssemblies).Name).FirstOrDefault();
            if (cyphy_cas == null)
            {
                cyphy_cas = CyPhyClasses.ComponentAssemblies.Create(rf);
                cyphy_cas.Name = typeof(CyPhyClasses.ComponentAssemblies).Name;
            }
            CyPhy.ComponentAssembly cyphy_container = CyPhyClasses.ComponentAssembly.Create(cyphy_cas);
            // container.Name = ad_import.Name; RootContainer has a name too
            // TODO: check ad_import.SchemaVersion
            int designID;
            if (int.TryParse(ad_import.DesignID, out designID))
            {
                cyphy_container.Attributes.ID = designID;
            }
            return cyphy_container;
        }

        private void ImportContainer(CyPhy.DesignEntity cyphy_container, avm.Container ad_container)
        {
            cyphy_container.Name = ad_container.Name;
            AVM2CyPhyML.CyPhyMLComponentBuilder.SetLayoutData(ad_container, cyphy_container.Impl);

            Dictionary<Type, CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum> typeToAttribute = new Dictionary<Type, CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum>()
            {
                {typeof(avm.DesignSpaceContainer), CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum.Compound},
                {typeof(avm.Alternative), CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum.Alternative},
                {typeof(avm.Optional), CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum.Optional},
                {typeof(avm.Compound), CyPhyClasses.DesignContainer.AttributesClass.ContainerType_enum.Compound},
            };
            if (cyphy_container is CyPhy.DesignContainer)
            {
                ((CyPhy.DesignContainer)cyphy_container).Attributes.ContainerType = typeToAttribute[ad_container.GetType()];
            }

            foreach (avm.Port avmPort in ad_container.Port)
            {
                if (cyphy_container is CyPhy.DesignContainer)
                {
                    process((CyPhy.DesignContainer)cyphy_container, avmPort);
                }
                else
                {
                    process((CyPhy.ComponentAssembly)cyphy_container, avmPort);
                }
            }
            foreach (var ad_connector in ad_container.Connector)
            {
                var cyphy_connector = CyPhyClasses.Connector.Cast(CreateChild((ISIS.GME.Common.Interfaces.Model)cyphy_container, typeof(CyPhyClasses.Connector)));
                processConnector(ad_connector, cyphy_connector);
            }

            foreach (var ad_prop in ad_container.Property)
            {
                if (cyphy_container is CyPhy.DesignContainer)
                {
                    process((CyPhy.DesignContainer)cyphy_container, ad_prop);
                }
                else
                {
                    process((CyPhy.ComponentAssembly)cyphy_container, ad_prop);
                }
            }

            foreach (var ad_componentinstance in ad_container.ComponentInstance)
            {
                CyPhy.ComponentRef cyphy_componentref;
                if (cyphy_container is CyPhy.DesignContainer)
                {
                    cyphy_componentref = CyPhyClasses.ComponentRef.Create((CyPhy.DesignContainer)cyphy_container);
                }
                else
                {
                    cyphy_componentref = CyPhyClasses.ComponentRef.Create((CyPhy.ComponentAssembly)cyphy_container);
                }
                ImportComponentInstance(ad_componentinstance, cyphy_componentref);
            }

            foreach (var ad_childcontainer in ad_container.Container1)
            {
                CyPhy.DesignEntity cyphy_childcontainer;
                if (cyphy_container is CyPhy.DesignContainer)
                {
                    cyphy_childcontainer = CyPhyClasses.DesignContainer.Create((CyPhy.DesignContainer)cyphy_container);
                }
                else
                {
                    cyphy_childcontainer = CyPhyClasses.ComponentAssembly.Create((CyPhy.ComponentAssembly)cyphy_container);
                }
                ImportContainer(cyphy_childcontainer, ad_childcontainer);
            }

            foreach (var constraint in ad_container.ContainerFeature.OfType<avm.schematic.eda.ExactLayoutConstraint>())
            {
                CyPhyML.ExactLayoutConstraint cyphy_constraint = CyPhyClasses.ExactLayoutConstraint.Cast(CreateChild((ISIS.GME.Common.Interfaces.Model)cyphy_container, typeof(CyPhyClasses.ExactLayoutConstraint)));
                cyphy_constraint.Name = typeof(CyPhyML.ExactLayoutConstraint).Name;
                SetLayoutData(constraint, cyphy_constraint.Impl);

                cyphy_constraint.Attributes.X = constraint.X.ToString();
                cyphy_constraint.Attributes.Y = constraint.Y.ToString();
                cyphy_constraint.Attributes.Layer = d_LayerEnumMap[constraint.Layer];
                if (constraint.RotationSpecified)
                {
                    cyphy_constraint.Attributes.Rotation = d_RotationEnumMap[constraint.Rotation];
                }
                if (string.IsNullOrWhiteSpace(constraint.ConstraintTarget) == false)
                {
                    CyPhyML.ComponentRef compInstance;
                    if (avmId2ComponentInstance.TryGetValue(constraint.ConstraintTarget, out compInstance))
                    {
                        CyPhyClasses.ApplyExactLayoutConstraint.Connect(cyphy_constraint, compInstance);
                    }
                }
            }
            foreach (var constraint in ad_container.ContainerFeature.OfType<avm.schematic.eda.RangeLayoutConstraint>())
            {
                CyPhyML.RangeLayoutConstraint cyphy_constraint = CyPhyClasses.RangeLayoutConstraint.Cast(CreateChild((ISIS.GME.Common.Interfaces.Model)cyphy_container, typeof(CyPhyClasses.RangeLayoutConstraint)));
                cyphy_constraint.Name = typeof(CyPhyML.RangeLayoutConstraint).Name;
                SetLayoutData(constraint, cyphy_constraint.Impl);

                cyphy_constraint.Attributes.LayerRange = d_LayerRangeEnumMap[constraint.LayerRange];
                if (constraint.XRangeMinSpecified && constraint.XRangeMaxSpecified)
                {
                    cyphy_constraint.Attributes.XRange = constraint.XRangeMin + "-" + constraint.XRangeMax;
                }
                if (constraint.YRangeMinSpecified && constraint.YRangeMaxSpecified)
                {
                    cyphy_constraint.Attributes.YRange = constraint.YRangeMin + "-" + constraint.YRangeMax;
                }
                foreach (var compId in constraint.ConstraintTarget)
                {
                    CyPhyML.ComponentRef compInstance;
                    if (avmId2ComponentInstance.TryGetValue(compId, out compInstance))
                    {
                        CyPhyClasses.ApplyRangeLayoutConstraint.Connect(cyphy_constraint, compInstance);
                    }
                }
            }
            foreach (var constraint in ad_container.ContainerFeature.OfType<avm.schematic.eda.RelativeLayoutConstraint>())
            {
                CyPhyML.RelativeLayoutConstraint cyphy_constraint = CyPhyClasses.RelativeLayoutConstraint.Cast(CreateChild((ISIS.GME.Common.Interfaces.Model)cyphy_container, typeof(CyPhyClasses.RelativeLayoutConstraint)));
                cyphy_constraint.Name = typeof(CyPhyML.RelativeLayoutConstraint).Name;
                SetLayoutData(constraint, cyphy_constraint.Impl);

                if (constraint.XOffsetSpecified)
                {
                    cyphy_constraint.Attributes.XOffset = constraint.XOffset.ToString();
                }
                if (constraint.YOffsetSpecified)
                {
                    cyphy_constraint.Attributes.YOffset = constraint.YOffset.ToString();
                }
                foreach (var compId in constraint.ConstraintTarget)
                {
                    CyPhyML.ComponentRef compInstance;
                    if (avmId2ComponentInstance.TryGetValue(compId, out compInstance))
                    {
                        CyPhyClasses.ApplyRelativeLayoutConstraint.Connect(cyphy_constraint, compInstance);
                    }
                }
                if (string.IsNullOrWhiteSpace(constraint.Origin) == false)
                {
                    CyPhyML.ComponentRef compInstance;
                    if (avmId2ComponentInstance.TryGetValue(constraint.Origin, out compInstance))
                    {
                        CyPhyClasses.RelativeLayoutConstraintOrigin.Connect(compInstance, cyphy_constraint);
                    }
                }
            }

            foreach (var simpleFormula in ad_container.Formula.OfType<avm.SimpleFormula>())
            {
                CyPhyML.SimpleFormula cyphy_simpleFormula = CyPhyClasses.SimpleFormula.Cast(CreateChild((ISIS.GME.Common.Interfaces.Model)cyphy_container, typeof(CyPhyClasses.SimpleFormula)));
                process(simpleFormula, cyphy_simpleFormula);
            }

            foreach (var complexFormula in ad_container.Formula.OfType<avm.ComplexFormula>())
            {
                var cyphy_customFormula = CyPhyClasses.CustomFormula.Cast(CreateChild((ISIS.GME.Common.Interfaces.Model)cyphy_container, typeof(CyPhyClasses.CustomFormula)));
                processComplexFormula(complexFormula, cyphy_customFormula);
            }

        }

        Dictionary<string, CyPhy.ComponentRef> avmId2ComponentInstance = new Dictionary<string, CyPhyML.ComponentRef>();

        private void ImportComponentInstance(avm.ComponentInstance ad_componentinstance, CyPhy.ComponentRef cyphy_componentref)
        {
            AVM2CyPhyML.CyPhyMLComponentBuilder.SetLayoutData(ad_componentinstance, cyphy_componentref.Impl);

            ISIS.GME.Dsml.CyPhyML.Interfaces.Component component;
            if (avmidComponentMap.TryGetValue(ad_componentinstance.ComponentID, out component) == false)
            {
                throw new ApplicationException(String.Format("Cannot find Component with ID {0}. Has it been imported?", ad_componentinstance.ComponentID));
            }
            cyphy_componentref.Referred.Component = component;
            cyphy_componentref.Name = ad_componentinstance.Name;
            //cyphy_componentref.Attributes.ID = ad_componentinstance.ID;
            cyphy_componentref.Attributes.InstanceGUID = ad_componentinstance.ID;
            avmId2ComponentInstance[ad_componentinstance.ID] = cyphy_componentref;

            foreach (var ad_propinstance in ad_componentinstance.PrimitivePropertyInstance)
            {
                var cyphy_component = this.avmidComponentMap[ad_componentinstance.ComponentID];
                var cyphy_componentPort = cyphy_component.AllChildren.OfType<CyPhy.ValueFlowTarget>()
                    .Where(x => ((MgaFCO)x.Impl).StrAttrByName["ID"] == ad_propinstance.IDinComponentModel).FirstOrDefault();

                _avmCyPhyMLObjectMap.Add(ad_propinstance, new KeyValuePair<ISIS.GME.Common.Interfaces.Reference, ISIS.GME.Common.Interfaces.FCO>(cyphy_componentref, cyphy_componentPort));
                registerValueNode(ad_propinstance.Value, ad_propinstance);
            }

            foreach (var ad_connectorInstance in ad_componentinstance.ConnectorInstance)
            {
                _idConnectorMap.Add(ad_connectorInstance.ID, ad_connectorInstance); // FIXME could be dup

                var cyphy_component = this.avmidComponentMap[ad_componentinstance.ComponentID];
                var cyphy_componentConnector = cyphy_component.AllChildren.OfType<CyPhy.Connector>()
                    .Where(x => ((MgaFCO)x.Impl).StrAttrByName["ID"] == ad_connectorInstance.IDinComponentModel).FirstOrDefault();
                if (cyphy_componentConnector == null)
                {
                    throw new ApplicationException("adm error: component instance " + ad_componentinstance.ID + " has connector with IDinComponentModel "
                        + ad_connectorInstance.IDinComponentModel + " that has no matching Connector in the Component");
                }

                _avmCyPhyMLObjectMap.Add(ad_connectorInstance, new KeyValuePair<ISIS.GME.Common.Interfaces.Reference, ISIS.GME.Common.Interfaces.FCO>(cyphy_componentref, cyphy_componentConnector));
            }

            foreach (var ad_port in ad_componentinstance.PortInstance)
            {
                registerPort(ad_port);

                var cyphy_component = this.avmidComponentMap[ad_componentinstance.ComponentID];
                var cyphy_componentConnector = cyphy_component.AllChildren.OfType<CyPhy.Port>()
                    .Where(x => ((MgaFCO)x.Impl).StrAttrByName["ID"] == ad_port.IDinComponentModel).FirstOrDefault();
                if (cyphy_componentConnector == null)
                {
                    throw new ApplicationException("adm error: component instance " + ad_componentinstance.ID + " has connector with IDinComponentModel "
                        + ad_port.IDinComponentModel + " that has no matching Connector in the Component");
                }
                _avmCyPhyMLObjectMap.Add(ad_port, new KeyValuePair<ISIS.GME.Common.Interfaces.Reference, ISIS.GME.Common.Interfaces.FCO>(cyphy_componentref, cyphy_componentConnector));
            }
        }

        private IMgaObject CreateChild(ISIS.GME.Common.Interfaces.Model parent, Type type)
        {
            var role = ((MgaMetaModel)parent.Impl.MetaBase).RoleByName[type.Name];
            return ((MgaModel)parent.Impl).CreateChildObject(role);
        }

        Dictionary<avm.schematic.eda.RotationEnum, string> d_RotationEnumMap = new Dictionary<avm.schematic.eda.RotationEnum,string>()
        {
            { avm.schematic.eda.RotationEnum.r0, "0"},
            { avm.schematic.eda.RotationEnum.r90, "1"},
            { avm.schematic.eda.RotationEnum.r180, "2"},
            { avm.schematic.eda.RotationEnum.r270, "3"}
        };

        Dictionary<avm.schematic.eda.LayerEnum, string> d_LayerEnumMap = new Dictionary<avm.schematic.eda.LayerEnum, string>()
        {
            { avm.schematic.eda.LayerEnum.Top, "0"},
            { avm.schematic.eda.LayerEnum.Bottom, "1"}
        };

        Dictionary<avm.schematic.eda.LayerRangeEnum, string> d_LayerRangeEnumMap = new Dictionary<avm.schematic.eda.LayerRangeEnum, string>()
        {
            { avm.schematic.eda.LayerRangeEnum.Top, "0"},
            { avm.schematic.eda.LayerRangeEnum.Bottom, "1"},
            { avm.schematic.eda.LayerRangeEnum.Either, "0-1"},
        };

    }
}
