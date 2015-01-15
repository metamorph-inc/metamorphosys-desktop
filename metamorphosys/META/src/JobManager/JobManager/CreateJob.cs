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

namespace JobManager
{
	public partial class CreateJob : Form
	{
		public CreateJob()
		{
			InitializeComponent();

			cbType.DataSource = Enum.GetValues(typeof(Job.TypeEnum));
			cbType.SelectedItem = Job.TypeEnum.Command;

		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				// validate inputs
				string title = txtTitle.Text;
				string workingDirectory = System.IO.Path.GetFullPath(txtWorkingDir.Text);
				string runCommand = txtRunCommand.Text;
				Job.TypeEnum type = Job.TypeEnum.Command;

				if (Enum.TryParse(cbType.SelectedItem.ToString(), out type) == false)
				{
					MessageBox.Show(
						"Invalid type.",
						"Enum type is invalid",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					return;
				}

				// create new job(s) on the server
				for (int i = 1; i <= nupNumJob.Value; ++i)
				{
					if (nupNumJob.Value > 1)
					{
						title = String.Format("{0} ({1})", txtTitle.Text, i);
					}

					Job j = (Owner as JobManager).server.CreateJob();
					j.RunCommand = runCommand;
					j.Title = title;
					j.WorkingDirectory = workingDirectory;
					j.Type = type;
                    j.Status = Job.StatusEnum.Ready;
					(Owner as JobManager).server.AddJob(j);
				}

				DialogResult = System.Windows.Forms.DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					ex.Message,
					"Exception",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}

		}
	}
}
