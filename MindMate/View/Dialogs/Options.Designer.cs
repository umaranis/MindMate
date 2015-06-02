namespace MindMate.View.Dialogs
{
    partial class Options
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMapEditorBackColor = new System.Windows.Forms.Label();
            this.lblNoteEditorBackColor = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(245, 234);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(137, 234);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Map Background Color:";
            // 
            // lblMapEditorBackColor
            // 
            this.lblMapEditorBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMapEditorBackColor.Location = new System.Drawing.Point(239, 23);
            this.lblMapEditorBackColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMapEditorBackColor.Name = "lblMapEditorBackColor";
            this.lblMapEditorBackColor.Size = new System.Drawing.Size(93, 18);
            this.lblMapEditorBackColor.TabIndex = 5;
            this.lblMapEditorBackColor.Click += new System.EventHandler(this.lblMapEditorBackColor_Click);
            // 
            // lblNoteEditorBackColor
            // 
            this.lblNoteEditorBackColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNoteEditorBackColor.Location = new System.Drawing.Point(239, 54);
            this.lblNoteEditorBackColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoteEditorBackColor.Name = "lblNoteEditorBackColor";
            this.lblNoteEditorBackColor.Size = new System.Drawing.Size(93, 18);
            this.lblNoteEditorBackColor.TabIndex = 7;
            this.lblNoteEditorBackColor.Click += new System.EventHandler(this.lblNoteEditorBackColor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 55);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Note Background Color:";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 273);
            this.Controls.Add(this.lblNoteEditorBackColor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMapEditorBackColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMapEditorBackColor;
        private System.Windows.Forms.Label lblNoteEditorBackColor;
        private System.Windows.Forms.Label label3;
    }
}