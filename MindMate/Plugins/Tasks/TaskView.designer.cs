namespace MindMate.Plugins.Tasks
{
    partial class TaskView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNodeName = new System.Windows.Forms.Label();
            this.lblDueOn = new System.Windows.Forms.Label();
            this.lblTaskPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblNodeName
            // 
            this.lblNodeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNodeName.AutoEllipsis = true;
            this.lblNodeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNodeName.Location = new System.Drawing.Point(4, 4);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(1192, 23);
            this.lblNodeName.TabIndex = 0;
            this.lblNodeName.Text = "Create a new Post on Animal Rights in Islam";
            // 
            // lblDueOn
            // 
            this.lblDueOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDueOn.ForeColor = System.Drawing.Color.Gray;
            this.lblDueOn.Location = new System.Drawing.Point(1183, 22);
            this.lblDueOn.Name = "lblDueOn";
            this.lblDueOn.Size = new System.Drawing.Size(57, 23);
            this.lblDueOn.TabIndex = 1;
            this.lblDueOn.Text = "12:00 AM";
            this.lblDueOn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTaskPath
            // 
            this.lblTaskPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaskPath.AutoEllipsis = true;
            this.lblTaskPath.ForeColor = System.Drawing.Color.Gray;
            this.lblTaskPath.Location = new System.Drawing.Point(7, 22);
            this.lblTaskPath.Name = "lblTaskPath";
            this.lblTaskPath.Size = new System.Drawing.Size(1130, 23);
            this.lblTaskPath.TabIndex = 2;
            this.lblTaskPath.Text = "Blogging -> My Tasks -> Important Ones -> My Mind";
            // 
            // TaskView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTaskPath);
            this.Controls.Add(this.lblDueOn);
            this.Controls.Add(this.lblNodeName);
            this.Name = "TaskView";
            this.Size = new System.Drawing.Size(1243, 42);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNodeName;
        private System.Windows.Forms.Label lblDueOn;
        private System.Windows.Forms.Label lblTaskPath;
    }
}
