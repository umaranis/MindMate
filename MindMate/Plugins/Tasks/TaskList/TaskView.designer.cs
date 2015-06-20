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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskView));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnComplete = new TransparentPictureBox();
            this.btnRemove = new TransparentPictureBox();
            this.btnUp = new TransparentPictureBox();
            this.btnDown = new TransparentPictureBox();
            this.lblTaskPath = new MindMate.Plugins.Tasks.TransparentLabel();
            this.lblDueOn = new MindMate.Plugins.Tasks.TransparentLabel();
            this.lblNodeName = new MindMate.Plugins.Tasks.TransparentLabel();
            this.SuspendLayout();
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnRemove.Image = TaskRes.date_delete;
            this.btnRemove.Location = new System.Drawing.Point(1221, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(18, 18);
            this.btnRemove.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRemove, "Remove Task");
            // 
            // btnComplete
            // 
            this.btnComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComplete.BackColor = System.Drawing.Color.Transparent;
            this.btnComplete.Image = TaskRes.tick;
            this.btnComplete.Location = new System.Drawing.Point(1200, 3);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(18, 18);
            this.btnComplete.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnComplete, "Complete Task");
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.BackColor = System.Drawing.Color.Transparent;
            this.btnDown.Image = TaskRes.arrow_270_medium;
            this.btnDown.Location = new System.Drawing.Point(1179, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(18, 18);
            this.btnDown.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRemove, "Move Down");
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.BackColor = System.Drawing.Color.Transparent;
            this.btnUp.Image = TaskRes.arrow_090_medium;
            this.btnUp.Location = new System.Drawing.Point(1158, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(18, 18);
            this.btnUp.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnRemove, "Move Up");
            // 
            // lblTaskPath
            // 
            this.lblTaskPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaskPath.AutoEllipsis = true;
            this.lblTaskPath.ForeColor = System.Drawing.Color.Gray;
            this.lblTaskPath.Location = new System.Drawing.Point(7, 22);
            this.lblTaskPath.Name = "lblTaskPath";
            this.lblTaskPath.Size = new System.Drawing.Size(1170, 16);
            this.lblTaskPath.TabIndex = 2;
            this.lblTaskPath.Text = "Blogging -> My Tasks -> Important Ones -> My Mind";
            // 
            // lblDueOn
            // 
            this.lblDueOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDueOn.ForeColor = System.Drawing.Color.Gray;
            this.lblDueOn.Location = new System.Drawing.Point(1183, 22);
            this.lblDueOn.Name = "lblDueOn";
            this.lblDueOn.Size = new System.Drawing.Size(57, 16);
            this.lblDueOn.TabIndex = 1;
            this.lblDueOn.Text = "12:00 AM";
            this.lblDueOn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNodeName
            // 
            this.lblNodeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNodeName.AutoEllipsis = true;
            this.lblNodeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNodeName.Location = new System.Drawing.Point(4, 4);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(1235, 23);
            this.lblNodeName.TabIndex = 0;
            this.lblNodeName.Text = "Create a new Post on Animal Rights in Islam";
            // 
            // TaskView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.lblTaskPath);
            this.Controls.Add(this.lblDueOn);
            this.Controls.Add(this.lblNodeName);
            this.Name = "TaskView";
            this.Size = new System.Drawing.Size(1243, 42);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TaskView_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private TransparentLabel lblNodeName;
        private TransparentLabel lblDueOn;
        private TransparentLabel lblTaskPath;
        private System.Windows.Forms.ToolTip toolTip1;
        public TransparentPictureBox btnRemove;
        public TransparentPictureBox btnComplete;
        public TransparentPictureBox btnUp;
        public TransparentPictureBox btnDown;
    }
}
