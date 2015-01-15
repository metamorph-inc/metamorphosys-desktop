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

namespace CyPhyComponentFidelitySelector
{
    public partial class FidelitySelectorForm : Form
    {
        private const string Classification = "Classification";
        private const string ModelicaModelOptions = "ModelicaModelOptions";

        // List for the data grid view control
        public List<ComponentItem> componentItems = new List<ComponentItem>();
        public string FidelitySettings { get; set; }

        public List<string> consoleMessages { get; set; }

        public FidelitySelectorForm()
        {
            this.consoleMessages = new List<string>();

            InitializeComponent();
        }

        public void PopulateDgv()
        {
            var fidelity = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                fidelity = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(this.FidelitySettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            dgvData.AutoGenerateColumns = false;
            dgvData.DataSource = componentItems;
            dgvData.DataBindingComplete += (o, e) =>
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    var cboCell = new DataGridViewComboBoxCell
                    {
                        DataSource = ((ComponentItem)row.DataBoundItem).ModelicaModelOptions,
                        ValueMember = "Key",
                        DisplayMember = "Name",
                    };
                    row.Cells[0] = cboCell;
                    row.Cells[0].Value = ((ComponentItem)row.DataBoundItem).ModelicaModelOptions.FirstOrDefault().Key;

                    // update value based on saved settings from FidelitySettings
                    var currentSettings = new Dictionary<string, string>();
                    if (fidelity.TryGetValue(((ComponentItem)row.DataBoundItem).Classification, out currentSettings))
                    {
                        string savedValue = ((ComponentItem)row.DataBoundItem).ModelicaModelOptions.FirstOrDefault().Name;
                        if (currentSettings.TryGetValue("ModelicaModel", out savedValue))
                        {
                            var opt = ((ComponentItem)row.DataBoundItem).ModelicaModelOptions.FirstOrDefault(x => x.Name == savedValue);
                            if (opt != null)
                            {
                                row.Cells[0].Value = opt.Key;
                            }
                        }
                    }
                }
            };

            dgvData.Columns.Add(
                new DataGridViewComboBoxColumn
                {
                    HeaderText = ModelicaModelOptions,
                    Name = ModelicaModelOptions
                });
            dgvData.Columns[0].Width = 200;

            dgvData.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = Classification,
                    HeaderText = Classification,
                    Name = Classification
                });
            dgvData.Columns[1].Width = 200;
            dgvData.Columns[1].ReadOnly = true;

            //dgvData.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //dgvData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dgvData.Update();
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            Dictionary<string, Dictionary<string, string>> settings =
                new Dictionary<string, Dictionary<string, string>>();

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                var selection = new Dictionary<string, string>();

                selection.Add("ModelicaModel", row.Cells[ModelicaModelOptions].FormattedValue as string);
                // TODO: add CAD

                settings.Add(row.Cells[Classification].Value as string, selection);

                string settingInfo = string.Format(
                    "Components with Classification '{0}' will use ModelicaModel '{1}'",
                    row.Cells[Classification].Value as string,
                    row.Cells[ModelicaModelOptions].FormattedValue as string);

                this.consoleMessages.Add(settingInfo);
            }

            this.FidelitySettings = Newtonsoft.Json.JsonConvert.SerializeObject(settings, Newtonsoft.Json.Formatting.Indented);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
