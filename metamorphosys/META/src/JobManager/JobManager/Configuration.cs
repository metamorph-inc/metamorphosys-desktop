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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace JobManager
{
    public partial class Configuration : Form
    {
        Jenkins.Jenkins jenkins;
        public Configuration(Jenkins.Jenkins jenkins, string password = null)
        {
            this.jenkins = jenkins;
            InitializeComponent();
            AcceptButton = btnSave;
            chbRemoteExec.CheckedChanged +=new EventHandler(delegate (object o, EventArgs args) {
                panelRemote.Enabled = chbRemoteExec.Checked;
            });
            panelRemote.Enabled = chbRemoteExec.Checked;


            this.txtUsername.Text = Properties.Settings.Default.UserID;
            
            if (string.IsNullOrEmpty(password) == false)
            {
                // auto-configure
                this.mtbPassword.Text = password;
                Properties.Settings.Default.RemoteExecution = true;
            }

            comboVehicleForgeURL.Items.Add("https://gamma.vehicleforge.org");
            comboVehicleForgeURL.Items.Add("https://beta.vehicleforge.org");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool cancel_Clicked = false;
        void linkCancelCheck_Click(object sender, System.EventArgs e)
        {
            lock (this)
            {
                cancel_Clicked = true;
            }
        }

        internal void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserID = this.txtUsername.Text;
            jenkins.Username = this.txtUsername.Text;
            jenkins.Password = this.mtbPassword.Text;

            if (!chbRemoteExec.Checked)
            {
                Properties.Settings.Default.Save();
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
                return;
            }
            else
            {
                // TODO: try to login with the u/p?
            }

            try
            {
                Uri jenkinsUri = new Uri(comboVehicleForgeURL.Text);

                while (Properties.Settings.Default.VehicleForgeUri.EndsWith("/"))
                    Properties.Settings.Default.VehicleForgeUri = Properties.Settings.Default.VehicleForgeUri.Substring(0, Properties.Settings.Default.VehicleForgeUri.Length - 1);

                btnSave.Enabled = false;
                pictureBoxLoading.Visible = true;
                labelJenkinsTest.Visible = true;
                linkCancelCheck.Visible = true;
                cancel_Clicked = false;
                jenkins.Username = txtUsername.Text;
                jenkins.Password = mtbPassword.Text;
                Action userCreateDelegate = delegate() { jenkins.Login(); };
                IAsyncResult userCreateResult = null;
                userCreateResult = userCreateDelegate.BeginInvoke(null, null);
                while (true)
                {
                    Application.DoEvents();
                    lock (this)
                    {
                        if (cancel_Clicked)
                            break;
                        if (userCreateResult != null && userCreateResult.AsyncWaitHandle.WaitOne(50))
                            break;
                    }
                }
                pictureBoxLoading.Visible = false;
                labelJenkinsTest.Visible = false;
                linkCancelCheck.Visible = false;
                btnSave.Enabled = true;
                if (cancel_Clicked)
                {
                    return;
                }
                if (userCreateResult != null)
                    userCreateDelegate.EndInvoke(userCreateResult);

                Properties.Settings.Default.Save();

                JobManager manager = (Owner as JobManager);
                if (manager != null)
                {
                    Dictionary<Job.TypeEnum, JobManager.TargetMachine> config = new Dictionary<Job.TypeEnum, JobManager.TargetMachine>();
                    JobManager.TargetMachine.TargetMachineType type = JobManager.TargetMachine.TargetMachineType.Local;
                    //if (cmd.Host.Equals("localhost"))
                    //{
                    //    type = JobManager.TargetMachine.TargetMachineType.Local;
                    //}
                    //else
                    //{
                    //    type = JobManager.TargetMachine.TargetMachineType.Remote;
                    //}
                    //config.Add(Job.TypeEnum.Command, new JobManager.TargetMachine(cmd, type));

                    //if (matlab.Host.Equals("localhost"))
                    //{
                    //    type = JobManager.TargetMachine.TargetMachineType.Local;
                    //}
                    //else
                    //{
                    //    type = JobManager.TargetMachine.TargetMachineType.Remote;
                    //}
                    //config.Add(Job.TypeEnum.Matlab, new JobManager.TargetMachine(matlab, type));

                    //if (cad.Host.Equals("localhost"))
                    //{
                    //    type = JobManager.TargetMachine.TargetMachineType.Local;
                    //}
                    //else
                    //{
                    //    type = JobManager.TargetMachine.TargetMachineType.Remote;
                    //}
                    //config.Add(Job.TypeEnum.CAD, new JobManager.TargetMachine(cad, type));

                    manager.UpdateRuntimeConfig(config);
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                    message += ": " + ex.InnerException.Message;
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenLink_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: check the uri is valid
                Uri uri = new Uri(comboVehicleForgeURL.Text);
                System.Diagnostics.Process.Start(uri.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}