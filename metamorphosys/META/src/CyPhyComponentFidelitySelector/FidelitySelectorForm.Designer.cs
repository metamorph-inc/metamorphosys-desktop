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

namespace CyPhyComponentFidelitySelector
{
    partial class FidelitySelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FidelitySelectorForm));
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.gbApplyToScope = new System.Windows.Forms.GroupBox();
            this.rbThisProject = new System.Windows.Forms.RadioButton();
            this.rbThisFolder = new System.Windows.Forms.RadioButton();
            this.rbThisTestbench = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.gbApplyToScope.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(12, 12);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.Size = new System.Drawing.Size(460, 249);
            this.dgvData.TabIndex = 0;
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndClose.Location = new System.Drawing.Point(327, 300);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(145, 50);
            this.btnSaveAndClose.TabIndex = 1;
            this.btnSaveAndClose.Text = "Save and Close";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // gbApplyToScope
            // 
            this.gbApplyToScope.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gbApplyToScope.Controls.Add(this.rbThisProject);
            this.gbApplyToScope.Controls.Add(this.rbThisFolder);
            this.gbApplyToScope.Controls.Add(this.rbThisTestbench);
            this.gbApplyToScope.Location = new System.Drawing.Point(12, 267);
            this.gbApplyToScope.Name = "gbApplyToScope";
            this.gbApplyToScope.Size = new System.Drawing.Size(244, 83);
            this.gbApplyToScope.TabIndex = 2;
            this.gbApplyToScope.TabStop = false;
            this.gbApplyToScope.Text = "Apply Fidelity Settings to:";
            // 
            // rbThisProject
            // 
            this.rbThisProject.AutoSize = true;
            this.rbThisProject.Location = new System.Drawing.Point(6, 57);
            this.rbThisProject.Name = "rbThisProject";
            this.rbThisProject.Size = new System.Drawing.Size(167, 17);
            this.rbThisProject.TabIndex = 2;
            this.rbThisProject.Text = "All Testbenches in this Project";
            this.rbThisProject.UseVisualStyleBackColor = true;
            // 
            // rbThisFolder
            // 
            this.rbThisFolder.AutoSize = true;
            this.rbThisFolder.Location = new System.Drawing.Point(6, 38);
            this.rbThisFolder.Name = "rbThisFolder";
            this.rbThisFolder.Size = new System.Drawing.Size(149, 17);
            this.rbThisFolder.TabIndex = 1;
            this.rbThisFolder.Text = "Testbenches in this Folder";
            this.rbThisFolder.UseVisualStyleBackColor = true;
            // 
            // rbThisTestbench
            // 
            this.rbThisTestbench.AutoSize = true;
            this.rbThisTestbench.Checked = true;
            this.rbThisTestbench.Location = new System.Drawing.Point(6, 19);
            this.rbThisTestbench.Name = "rbThisTestbench";
            this.rbThisTestbench.Size = new System.Drawing.Size(123, 17);
            this.rbThisTestbench.TabIndex = 0;
            this.rbThisTestbench.TabStop = true;
            this.rbThisTestbench.Text = "This Testbench Only";
            this.rbThisTestbench.UseVisualStyleBackColor = true;
            // 
            // FidelitySelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.gbApplyToScope);
            this.Controls.Add(this.btnSaveAndClose);
            this.Controls.Add(this.dgvData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 375);
            this.Name = "FidelitySelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FidelitySelectorForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.gbApplyToScope.ResumeLayout(false);
            this.gbApplyToScope.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn selectedItemDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.GroupBox gbApplyToScope;
        public System.Windows.Forms.RadioButton rbThisProject;
        public System.Windows.Forms.RadioButton rbThisFolder;
        public System.Windows.Forms.RadioButton rbThisTestbench;

    }
}