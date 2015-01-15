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
using System.IO;
using System.Windows.Forms;
using GME.CSharp;
using Exp = CyPhyComponentExporter;

namespace CyPhyComponentAuthoring.Modules
{
    [CyPhyComponentAuthoringInterpreter.IsCATModule(ContainsCATmethod = true)]
    public class ComponentExport : CATModule
    {
        public CyPhyGUIs.GMELogger Logger { get; set; }
        private bool Close_Dlg;

        [CyPhyComponentAuthoringInterpreter.CATName(
            NameVal = "Export Component Package",
            DescriptionVal = "Export an AVM Component Package in ZIP format, which will include any of this model's dependent artifacts.",
            RoleVal = CyPhyComponentAuthoringInterpreter.Role.Share
            )
        ]
        public void ExportComponentPackage(object sender, EventArgs e)
        {
            export_component_package();

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

        void export_component_package()
        {
            var component = this.GetCurrentComp();

            this.Logger = new CyPhyGUIs.GMELogger(CurrentProj, this.GetType().Name);
            this.Logger.WriteDebug("Starting Component Export module...");

            // check for valid component and bail if there is not one
            try
            {
                string checkforinvalidcomponent = Path.GetDirectoryName(component.Impl.Project.ProjectConnStr.Substring("MGA=".Length));
            }
            catch(Exception)
            {
                this.Logger.WriteError("Error, No Current Componet");
                cleanup(true);
                return;
            }

            #region Prompt for Output Path
            string startupPath = Path.GetDirectoryName(component.Impl.Project.ProjectConnStr.Substring("MGA=".Length));

            // Get an output path from the user.
            String s_outPath;
            using (META.FolderBrowserDialog fbd = new META.FolderBrowserDialog()
            {
                Description = "Choose a path for the generated files.",
                //ShowNewFolderButton = true,
                SelectedPath = startupPath,
            })
            {

                DialogResult dr = fbd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    s_outPath = fbd.SelectedPath;
                }
                else
                {
                    this.Logger.WriteWarning("Component Exporter cancelled");
                    cleanup(false);
                    return;
                }
            }
            #endregion

            // FIXME: this could throw an exception (but this code is currently unreachable)
            var pkgPath = Exp.CyPhyComponentExporterInterpreter.ExportComponentPackage(component, s_outPath);
            this.Logger.WriteInfo("Component package created: {0}", pkgPath);
            cleanup(true);
        }

        // Clean up resources on all return paths
        void cleanup(bool close_dlg = true)
        {
            Close_Dlg = close_dlg;
            this.Logger.Dispose();
        }
    }
}
