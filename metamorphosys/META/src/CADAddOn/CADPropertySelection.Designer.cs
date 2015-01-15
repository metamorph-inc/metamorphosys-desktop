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

namespace CADAddOn
{
    partial class CADPropertySelection
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
            this.CGCB = new System.Windows.Forms.CheckBox();
            this.DensityCB = new System.Windows.Forms.CheckBox();
            this.MassCB = new System.Windows.Forms.CheckBox();
            this.VolumeCB = new System.Windows.Forms.CheckBox();
            this.SurfaceAreaCB = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelBT = new System.Windows.Forms.Button();
            this.okBT = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CGCB
            // 
            this.CGCB.AutoSize = true;
            this.CGCB.Location = new System.Drawing.Point(31, 39);
            this.CGCB.Name = "CGCB";
            this.CGCB.Size = new System.Drawing.Size(41, 17);
            this.CGCB.TabIndex = 0;
            this.CGCB.Text = "CG";
            this.CGCB.UseVisualStyleBackColor = true;
            this.CGCB.CheckedChanged += new System.EventHandler(this.CGCB_CheckedChanged);
            // 
            // DensityCB
            // 
            this.DensityCB.AutoSize = true;
            this.DensityCB.Location = new System.Drawing.Point(31, 62);
            this.DensityCB.Name = "DensityCB";
            this.DensityCB.Size = new System.Drawing.Size(61, 17);
            this.DensityCB.TabIndex = 1;
            this.DensityCB.Text = "Density";
            this.DensityCB.UseVisualStyleBackColor = true;
            this.DensityCB.CheckedChanged += new System.EventHandler(this.DensityCB_CheckedChanged);
            // 
            // MassCB
            // 
            this.MassCB.AutoSize = true;
            this.MassCB.Location = new System.Drawing.Point(31, 84);
            this.MassCB.Name = "MassCB";
            this.MassCB.Size = new System.Drawing.Size(51, 17);
            this.MassCB.TabIndex = 2;
            this.MassCB.Text = "Mass";
            this.MassCB.UseVisualStyleBackColor = true;
            this.MassCB.CheckedChanged += new System.EventHandler(this.MassCB_CheckedChanged);
            // 
            // VolumeCB
            // 
            this.VolumeCB.AutoSize = true;
            this.VolumeCB.Location = new System.Drawing.Point(31, 130);
            this.VolumeCB.Name = "VolumeCB";
            this.VolumeCB.Size = new System.Drawing.Size(61, 17);
            this.VolumeCB.TabIndex = 3;
            this.VolumeCB.Text = "Volume";
            this.VolumeCB.UseVisualStyleBackColor = true;
            this.VolumeCB.CheckedChanged += new System.EventHandler(this.VolumeCB_CheckedChanged);
            // 
            // SurfaceAreaCB
            // 
            this.SurfaceAreaCB.AutoSize = true;
            this.SurfaceAreaCB.Location = new System.Drawing.Point(31, 107);
            this.SurfaceAreaCB.Name = "SurfaceAreaCB";
            this.SurfaceAreaCB.Size = new System.Drawing.Size(85, 17);
            this.SurfaceAreaCB.TabIndex = 4;
            this.SurfaceAreaCB.Text = "SurfaceArea";
            this.SurfaceAreaCB.UseVisualStyleBackColor = true;
            this.SurfaceAreaCB.CheckedChanged += new System.EventHandler(this.SurfaceAreaCB_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select a CADProperty";
            // 
            // cancelBT
            // 
            this.cancelBT.Location = new System.Drawing.Point(149, 202);
            this.cancelBT.Name = "cancelBT";
            this.cancelBT.Size = new System.Drawing.Size(75, 23);
            this.cancelBT.TabIndex = 6;
            this.cancelBT.Text = "Cancel";
            this.cancelBT.UseVisualStyleBackColor = true;
            this.cancelBT.Click += new System.EventHandler(this.cancelBT_Click);
            // 
            // okBT
            // 
            this.okBT.Location = new System.Drawing.Point(21, 202);
            this.okBT.Name = "okBT";
            this.okBT.Size = new System.Drawing.Size(75, 23);
            this.okBT.TabIndex = 7;
            this.okBT.Text = "Ok";
            this.okBT.UseVisualStyleBackColor = true;
            this.okBT.Click += new System.EventHandler(this.okBT_Click);
            // 
            // CADPropertySelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 237);
            this.Controls.Add(this.okBT);
            this.Controls.Add(this.cancelBT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SurfaceAreaCB);
            this.Controls.Add(this.VolumeCB);
            this.Controls.Add(this.MassCB);
            this.Controls.Add(this.DensityCB);
            this.Controls.Add(this.CGCB);
            this.Name = "CADPropertySelection";
            this.Text = "CADPropertySelection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CGCB;
        private System.Windows.Forms.CheckBox DensityCB;
        private System.Windows.Forms.CheckBox MassCB;
        private System.Windows.Forms.CheckBox VolumeCB;
        private System.Windows.Forms.CheckBox SurfaceAreaCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelBT;
        private System.Windows.Forms.Button okBT;
    }
}