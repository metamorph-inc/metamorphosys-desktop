﻿/*
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using CyPhyComponentExporter;
using GME.MGA;
using Microsoft.Win32;
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;
using META;

namespace CyPhyComponentAuthoring.Modules
{
    [CyPhyComponentAuthoringInterpreter.IsCATModule(ContainsCATmethod = true)]
    public class MfgModelImport : CATModule
    {
        // constants for adding new parameters to the GME display at the right coordinates
        private const int PARAMETER_START_X = 20;
        private const int PARAMETER_START_Y = 200;
        private const int PARAMETER_ADJUST_Y = 40;

        // constants for adding new resources to the GME display at the right coordinates
        private const int RESOURCE_START_X = 650;
        private const int RESOURCE_START_Y = 200;

        // constants for adding new manufacturing models to the GME display at the right coordinates
        private const int MFG_MODEL_START_X = 270;
        private const int MFG_MODEL_START_Y = 200;

        // used to determine the element that is the farthest "south" (Y axis) on the display
        private int greatest_current_y = 0;

        private const string MFG_MODEL_NAME = "ManufacturingModel"; // formerly Path.GetFileNameWithoutExtension(mfgFilename)

        public CyPhyGUIs.GMELogger Logger { get; set; }
        private bool Close_Dlg;

        [CyPhyComponentAuthoringInterpreter.CATName(
            NameVal = "Add Manufacturing",
            DescriptionVal = "Allows adding an existing Manufacturing Model file to this CyPhy Component model.",
            RoleVal = CyPhyComponentAuthoringInterpreter.Role.Construct
            )
        ]
        public void ImportMfgModel(object sender, EventArgs e)
        {
            import_manufacturing_model();

            // Close the calling dialog box if the module ran successfully
            if (Close_Dlg)
            {
                // calling object is a button
                Button callerBtn = (Button)sender;
                // the button is in a layout panel
                TableLayoutPanel innerTLP = (TableLayoutPanel)callerBtn.Parent;
                // the layout panel is a table within a table
                TableLayoutPanel outerTLP = (TableLayoutPanel)innerTLP.Parent;
                // the TLP is in the dialog box
                Form parentDB = (Form)outerTLP.Parent;
                parentDB.Close();
            }
        }

        public void import_manufacturing_model(string mfgFilename = "")
        {
            this.Logger = new CyPhyGUIs.GMELogger(CurrentProj, this.GetType().Name);

            this.Logger.WriteDebug("Starting Import Manufacturing Model module...");

            // Case 3211: determine if a manufacturing model already exists and give the user the option to abort if it does
            CyPhy.ManufacturingModel ExistingMfgModel = GetCurrentComp().Children.ManufacturingModelCollection.FirstOrDefault();

            if (ExistingMfgModel != null)
            {
                // no exception means a manufacturing model exists
                this.Logger.WriteDebug("Detected pre-existing Manufacturing Model.");

                // skip the dialog box if we are in autotest mode
                if (String.IsNullOrEmpty(mfgFilename))
                {
                    // YesNoDialog to ask the user to overwrite the existing manufacturing model
                    var dialogResult = MessageBox.Show("Do you wish to overwrite the existing Manufacturing Model?", "Manufacturing Model Detected", MessageBoxButtons.YesNo);

                    // if user selected anything besides YES then the tool will abort
                    if (dialogResult != DialogResult.Yes)
                    {
                        this.Logger.WriteError("Import manufacturing model aborted. Model already exists");
                        cleanup(false);
                        return;
                    }
                }

                // delete any resources attached to the manufacturing model about to be deleted
                foreach (var ur in ExistingMfgModel.SrcConnections.UsesResourceCollection)
                {
                    ur.SrcEnd.Delete();
                }

                // delete existing manufacturing model
                ExistingMfgModel.Delete();
            }

            //  - Display a dialog box to let the user choose their manufacturing model XML file
            bool mfg_file_chosen = false;
            if (String.IsNullOrEmpty(mfgFilename))
            {
                mfg_file_chosen = get_mfg_file(out mfgFilename);
            }
            else
            {
                mfg_file_chosen = true;
            }

            // Step 1 - Copy the Manufacturing Model XML file into the component's folder 
            #region Copy file to component folder

            // used in creating the resource object below
            string mfgFileCopyPath = null;

            if (mfg_file_chosen)
            {
                try
                {
                    // create the destination path
                    string PathforComp = META.ComponentLibraryManager.EnsureComponentFolder(GetCurrentComp());
                    PathforComp = GetCurrentComp().GetDirectoryPath(ComponentLibraryManager.PathConvention.ABSOLUTE);

                    string finalPathName = Path.Combine(PathforComp, "Manufacturing");

                    Directory.CreateDirectory(finalPathName);

                    // determine if one part file or all part and assembly files need to be copied
                    string cpsrcfile = System.IO.Path.GetFileName(mfgFilename);

                    // copy the selected file
                    mfgFileCopyPath = System.IO.Path.Combine(finalPathName, cpsrcfile);

                    // Check to see if the file exists
                    int counter = 1;
                    while (System.IO.File.Exists(mfgFileCopyPath))
                    {
                        string JustFileName = String.Format("{0}({1}){2}", System.IO.Path.GetFileNameWithoutExtension(mfgFilename), counter++, System.IO.Path.GetExtension(mfgFilename));
                        mfgFileCopyPath = System.IO.Path.Combine(finalPathName, JustFileName);
                    }

                    System.IO.File.Copy(mfgFilename, mfgFileCopyPath);

                }
                catch (Exception err_create_proj)
                {
                    this.Logger.WriteError("Error copying manufacturing file: " + err_create_proj.Message, " - Copy Failed. Possible disk drive issue.");
                    cleanup(false);
                    return;
                }
            }
            #endregion

            // Step 2 - In the CyPhy Component Model, create: 
            #region Create_Manufacturing_Model_Object
            // Step 2a  - create a ManufacturingModel object 
            // used in creating the resource object below, so kept outside of if loop for scope
            CyPhy.ManufacturingModel ProcessedMfgModel = null;

            if (mfg_file_chosen)
            {
                this.Logger.WriteDebug("Creating Manufacturing model object...");

                ProcessedMfgModel = CyPhyClasses.ManufacturingModel.Create(GetCurrentComp());
                ProcessedMfgModel.Name = MFG_MODEL_NAME;

                // find the largest current Y value so our new elements are added below the existing design elements
                foreach (var child in GetCurrentComp().AllChildren)
                {
                    foreach (MgaPart item in (child.Impl as MgaFCO).Parts)
                    {
                        int read_x, read_y;
                        string read_str;
                        item.GetGmeAttrs(out read_str, out read_x, out read_y);
                        greatest_current_y = (read_y > greatest_current_y) ? read_y : greatest_current_y;
                    }
                }

                // layout new model to the "south" of existing elements
                foreach (MgaPart item in (ProcessedMfgModel.Impl as MgaFCO).Parts)
                {
                    item.SetGmeAttrs(null, MFG_MODEL_START_X, greatest_current_y + MFG_MODEL_START_Y);
                }
            }
            #endregion

            // Step 2b - create a Resource object pointing to the copied XML file 
            #region Create_Resource_Object
            if (mfg_file_chosen)
            {
                this.Logger.WriteDebug("Creating Manufacturing model resource object...");
                CyPhy.Resource ResourceObj = CyPhyClasses.Resource.Create(GetCurrentComp());
                ResourceObj.Attributes.ID = Guid.NewGuid().ToString("B");
                // META-3136, fix PATH attribute to be relative to component directory
                // ResourceObj.Attributes.Path = mfgFileCopyPath;
                ResourceObj.Attributes.Path = Path.Combine("Manufacturing", Path.GetFileName(mfgFileCopyPath));
                ResourceObj.Attributes.Notes = "Manufacturing Model Import tool added this resource object";
                ResourceObj.Name = Path.GetFileName(mfgFileCopyPath);

                // layout Resource just to the side of the manufacturing model
                foreach (MgaPart item in (ResourceObj.Impl as MgaFCO).Parts)
                {
                    item.SetGmeAttrs(null, RESOURCE_START_X, greatest_current_y + RESOURCE_START_Y);
                }

                // Step 2c - Create a UsesResource association between the CyPhy ManufacturingModel object and the Resource object 
                this.Logger.WriteDebug("Creating Manufacturing model UsesResource association...");
                CyPhyClasses.UsesResource.Connect(ResourceObj, ProcessedMfgModel, null, null, GetCurrentComp());
            }
            #endregion

            // Clean up
            this.Logger.WriteDebug("Exiting Import Manufacturing Model module...");
            cleanup(mfg_file_chosen);
        }

        // Clean up resources on all return paths
        void cleanup(bool close_dlg = true)
        {
            Close_Dlg = close_dlg;
            this.Logger.Dispose();
        }

        // Display a file dialog box to select the manufacturing model file to import
        // parameter will contain the filename, returns true or false based on if a file is returned or not
        bool get_mfg_file(out string FileName)
        {
            // Open file dialog box
            DialogResult dr;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.CheckFileExists = true;
                ofd.DefaultExt = "test.mdl";
                ofd.Multiselect = false;
                ofd.Filter = "Manufacturing Model files (*.xml)|*.xml*|All files (*.*)|*.*";
                dr = ofd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    FileName = ofd.FileName;
                    return true;
                }
            }
            FileName = "";
            this.Logger.WriteError("No file was selected.  Manufacturing Model Import will not complete.");
            return false;
        }
    }
}