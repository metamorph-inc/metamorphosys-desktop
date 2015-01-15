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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GME.MGA;
using ISIS.Web;

namespace CLM_light
{
    public partial class ComponentSelectionForm : Form
    {
        public System.IO.TextWriter ErrorWriter { get; set; }
        public System.IO.TextWriter InfoWriter { get; set; }

        private List<ComponentListItem> dataSource;
        private IMgaModel component;
        private IMgaModel alternativeContainer;
        private Dictionary<string, ComponentLibraryFilterParameter> filterParameters;
        private ClmLib.Clm clm;

        public ComponentSelectionForm(IMgaModel component, IMgaModel alternativeContainer, System.IO.TextWriter errorWriter=null, System.IO.TextWriter infoWriter=null)
        {
            InitializeComponent();

            #region Initialize Class Variables
            this.component = component;
            this.alternativeContainer = alternativeContainer;
            this.dataSource = new List<ComponentListItem>();
            this.ErrorWriter = (errorWriter == null) ? Console.Error : errorWriter;
            this.InfoWriter = (infoWriter == null) ? Console.Out : infoWriter;
            this.clm = new ClmLib.Clm(this.component);
            #endregion

            this.filterParameters = this.clm.GenerateFilter(alternativeContainer, this.ErrorWriter, this.InfoWriter);

            RefreshLocalComponentList();
        }

        private void RefreshLocalComponentList()
        {
            this.dataSource.Clear();
            var relatedComponents = this.clm.GetRelatedComponents();
            relatedComponents.ForEach(x => this.dataSource.Add(new ComponentListItem(x as MgaFCO)));
            this.dgvSelector.DataSource = null;
            this.dgvSelector.DataSource = this.dataSource;
            this.dgvSelector.ClearSelection();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.dgvSelector.SelectedRows.Count == 0)
            {
                var msgBoxResponse = MessageBox.Show(
                    "No Components have been selected for insertion. CLM_Light will close.",
                    "Nothing Happened",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                if (msgBoxResponse == System.Windows.Forms.DialogResult.OK)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void btnCheckVF_Click(object sender, EventArgs e)
        {
            var result = clm.ShowAndSelectVFComponents(this.filterParameters);

            RefreshLocalComponentList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        bool sortByNameAscending = true;
        bool sortByPathAscending = true;

        private void dgvSelector_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvSelector.DataSource = null;

            if (this.dgvSelector.Columns[e.ColumnIndex].HeaderText == "Name")
            {
                if (sortByNameAscending)
                {
                    dataSource = dataSource.OrderBy(x => x.Name).ToList();
                    sortByNameAscending = !sortByNameAscending;
                }
                else
                {
                    dataSource = dataSource.OrderByDescending(x => x.Name).ToList();
                    sortByNameAscending = !sortByNameAscending;
                }
                //dataSource.Sort((x, y) => x.Name.CompareTo(y.Name));
            }
            else if (this.dgvSelector.Columns[e.ColumnIndex].HeaderText == "PathWithinProject")
            {
                if (sortByPathAscending)
                {
                    dataSource = dataSource.OrderBy(x => x.PathWithinProject).ToList();
                    sortByPathAscending = !sortByPathAscending;
                }
                else
                {
                    dataSource = dataSource.OrderByDescending(x => x.PathWithinProject).ToList();
                    sortByPathAscending = !sortByPathAscending;
                }
                //dataSource.Sort((x, y) => x.PathWithinProject.CompareTo(y.PathWithinProject));
            }

            this.dgvSelector.DataSource = dataSource;
            this.dgvSelector.Refresh();
        }

    }
}
