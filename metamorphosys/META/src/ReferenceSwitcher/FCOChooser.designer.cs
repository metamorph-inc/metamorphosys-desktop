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

using System.Windows.Forms;
namespace ReferenceSwitcher
{
    partial class FCOChooser
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
            this.fco1 = new System.Windows.Forms.TextBox();
            this.choose1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.choose2 = new System.Windows.Forms.Button();
            this.link = new System.Windows.Forms.Button();
            this.fco2 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fco1
            // 
            this.fco1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fco1.Enabled = false;
            this.fco1.Location = new System.Drawing.Point(12, 28);
            this.fco1.Name = "fco1";
            this.fco1.Size = new System.Drawing.Size(357, 20);
            this.fco1.TabIndex = 0;
            this.fco1.TabStop = false;
            this.fco1.Text = "Old Library";
            // 
            // choose1
            // 
            this.choose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.choose1.Location = new System.Drawing.Point(375, 28);
            this.choose1.Name = "choose1";
            this.choose1.Size = new System.Drawing.Size(75, 23);
            this.choose1.TabIndex = 0;
            this.choose1.Text = "Choose";
            this.choose1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(377, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Use the GME tree browser to select an FCO, then press the \"Choose\" buttons.";
            // 
            // choose2
            // 
            this.choose2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.choose2.Location = new System.Drawing.Point(375, 56);
            this.choose2.Name = "choose2";
            this.choose2.Size = new System.Drawing.Size(75, 23);
            this.choose2.TabIndex = 3;
            this.choose2.Text = "Choose";
            this.choose2.UseVisualStyleBackColor = true;
            // 
            // link
            // 
            this.link.Location = new System.Drawing.Point(12, 82);
            this.link.Name = "link";
            this.link.Size = new System.Drawing.Size(129, 23);
            this.link.TabIndex = 4;
            this.link.Text = "Update References";
            this.link.UseVisualStyleBackColor = true;
            // 
            // fco2
            // 
            this.fco2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fco2.Enabled = false;
            this.fco2.Location = new System.Drawing.Point(12, 56);
            this.fco2.Name = "fco2";
            this.fco2.Size = new System.Drawing.Size(357, 20);
            this.fco2.TabIndex = 5;
            this.fco2.TabStop = false;
            this.fco2.Text = "New Library";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.fco2);
            this.panel1.Controls.Add(this.fco1);
            this.panel1.Controls.Add(this.link);
            this.panel1.Controls.Add(this.choose1);
            this.panel1.Controls.Add(this.choose2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 133);
            this.panel1.TabIndex = 6;
            // 
            // FCOChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 133);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(465, 158);
            this.Name = "FCOChooser";
            this.Text = "Reference Switcher";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fco1;
        private System.Windows.Forms.Button choose1;
        private Label label1;
        private Button choose2;
        private Button link;
        private TextBox fco2;
        private Panel panel1;

    }
}
