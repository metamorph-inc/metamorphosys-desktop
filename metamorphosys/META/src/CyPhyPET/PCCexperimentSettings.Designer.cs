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

namespace CyPhyPET
{
    partial class PCCexperimentSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PCCexperimentSettings));
            this.txtPCCCorePath = new System.Windows.Forms.TextBox();
            this.btnBrowsePCCCorePath = new System.Windows.Forms.Button();
            this.lblPCCCorePath = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPCCCorePath
            // 
            this.txtPCCCorePath.Location = new System.Drawing.Point(104, 23);
            this.txtPCCCorePath.Name = "txtPCCCorePath";
            this.txtPCCCorePath.Size = new System.Drawing.Size(296, 20);
            this.txtPCCCorePath.TabIndex = 0;
            // 
            // btnBrowsePCCCorePath
            // 
            this.btnBrowsePCCCorePath.Location = new System.Drawing.Point(407, 22);
            this.btnBrowsePCCCorePath.Name = "btnBrowsePCCCorePath";
            this.btnBrowsePCCCorePath.Size = new System.Drawing.Size(27, 23);
            this.btnBrowsePCCCorePath.TabIndex = 1;
            this.btnBrowsePCCCorePath.Text = "...";
            this.btnBrowsePCCCorePath.UseVisualStyleBackColor = true;
            this.btnBrowsePCCCorePath.Click += new System.EventHandler(this.btnBrowsePCCCorePath_Click);
            // 
            // lblPCCCorePath
            // 
            this.lblPCCCorePath.AutoSize = true;
            this.lblPCCCorePath.Location = new System.Drawing.Point(12, 27);
            this.lblPCCCorePath.Name = "lblPCCCorePath";
            this.lblPCCCorePath.Size = new System.Drawing.Size(81, 13);
            this.lblPCCCorePath.TabIndex = 2;
            this.lblPCCCorePath.Text = "PCC Core Path:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(186, 113);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PCCexperimentSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(446, 148);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPCCCorePath);
            this.Controls.Add(this.btnBrowsePCCCorePath);
            this.Controls.Add(this.txtPCCCorePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PCCexperimentSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PCCexperimentSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowsePCCCorePath;
        private System.Windows.Forms.Label lblPCCCorePath;
        private System.Windows.Forms.Button btnOK;
        internal System.Windows.Forms.TextBox txtPCCCorePath;
    }
}