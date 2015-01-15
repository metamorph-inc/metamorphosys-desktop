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
using System.Windows.Forms;

namespace CyPhy2Schematic.GUI
{
    public partial class CyPhy2Schematic_GUI : Form
    {
        RadioButton checkedRadioButton;

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control control in groupBox1.Controls)
            {
                RadioButton radioButton = control as RadioButton;
                if (radioButton != null)
                {
                    radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (radioButton.Checked)
                {
                    checkedRadioButton = radioButton;
                }
                else if (checkedRadioButton == radioButton)
                {
                    checkedRadioButton = null;
                }
            }
        }

        public CyPhy2Schematic_Settings settings
        {
            get
            {
                return new CyPhy2Schematic_Settings()
                {
                    doSpice = spiceModeButton.Checked ? "true" : null,
                    doChipFit = (!spiceModeButton.Checked && cb_TestForChipFit.Checked) ? "true" : null,
                    doPlaceRoute = (!spiceModeButton.Checked && cb_PlaceAndRoute.Checked) ? "true" : null
                };
            }
            set
            {
                if (value != null)
                {
                    spiceModeButton.Checked = value.doSpice != null;
                    edaModeButton.Checked = !spiceModeButton.Checked;
                    cb_TestForChipFit.Checked = value.doChipFit == "true";
                    cb_PlaceAndRoute.Checked = value.doPlaceRoute == "true";

                    edaModeButton_CheckedChanged(this, new EventArgs());
                }
            }
        }
    }
}
