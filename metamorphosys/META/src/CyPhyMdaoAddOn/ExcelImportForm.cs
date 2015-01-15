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

namespace CyPhyMdaoAddOn
{
	public partial class ExcelImportForm : Form
	{
		public ExcelImportForm()
		{
			InitializeComponent();
		}

		private void btnAddParameter_Click(object sender, EventArgs e)
		{
			List<string> names = new List<string>();
			foreach (string item in lbNamedCells.SelectedItems)
			{
				names.Add(item);
			}
			foreach (var item in names)
			{
				lbParameters.Items.Add(item);
				lbNamedCells.Items.Remove(item);
			}
		}

		private void btnAddMetric_Click(object sender, EventArgs e)
		{
			List<string> names = new List<string>();
			foreach (string item in lbNamedCells.SelectedItems)
			{
				names.Add(item);
			}
			foreach (var item in names)
			{
				lbMetrics.Items.Add(item);
				lbNamedCells.Items.Remove(item);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void btnRemoveParameter_Click(object sender, EventArgs e)
		{
			List<string> names = new List<string>();
			foreach (string item in lbParameters.SelectedItems)
			{
				names.Add(item);
			}
			foreach (var item in names)
			{
				lbNamedCells.Items.Add(item);
				lbParameters.Items.Remove(item);
			}
		}

		private void btnRemoveMetric_Click(object sender, EventArgs e)
		{
			List<string> names = new List<string>();
			foreach (string item in lbMetrics.SelectedItems)
			{
				names.Add(item);
			}
			foreach (var item in names)
			{
				lbNamedCells.Items.Add(item);
				lbMetrics.Items.Remove(item);
			}
		}

		private void SelectAll(ListBox listBox)
		{
			for (int i = 0; i < listBox.Items.Count; ++i)
			{
				listBox.SetSelected(i, true);
			}
		}

		private void lbNamedCells_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A &&
				e.Modifiers == Keys.Control)
			{
				SelectAll(lbNamedCells);
			}
		}

		private void lbParameters_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A &&
				e.Modifiers == Keys.Control)
			{
				SelectAll(lbParameters);
			}
		}

		private void lbMetrics_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A &&
				e.Modifiers == Keys.Control)
			{
				SelectAll(lbMetrics);
			}
		}

	}
}
