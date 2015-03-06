namespace MindMate.Plugins.Tasks.SideBar
{
    partial class CollapsiblePanel
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pictureBoxExpandCollapse = new System.Windows.Forms.PictureBox();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.timerAnimation = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExpandCollapse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.pictureBoxExpandCollapse);
            this.pnlHeader.Controls.Add(this.pictureBoxImage);
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(249, 24);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.MouseLeave += new System.EventHandler(this.pnlHeader_MouseLeave);
            this.pnlHeader.MouseHover += new System.EventHandler(this.pnlHeader_MouseHover);
            // 
            // pictureBoxExpandCollapse
            // 
            this.pictureBoxExpandCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxExpandCollapse.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxExpandCollapse.Image = global::MindMate.Plugins.Tasks.TaskRes.collapse;
            this.pictureBoxExpandCollapse.Location = new System.Drawing.Point(224, 2);
            this.pictureBoxExpandCollapse.Name = "pictureBoxExpandCollapse";
            this.pictureBoxExpandCollapse.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxExpandCollapse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxExpandCollapse.TabIndex = 2;
            this.pictureBoxExpandCollapse.TabStop = false;
            this.pictureBoxExpandCollapse.MouseLeave += new System.EventHandler(this.pictureBoxExpandCollapse_MouseLeave);
            this.pictureBoxExpandCollapse.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxExpandCollapse_MouseMove);
            this.pictureBoxExpandCollapse.Click += new System.EventHandler(this.pictureBoxExpandCollapse_Click);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxImage.TabIndex = 1;
            this.pictureBoxImage.TabStop = false;
            this.pictureBoxImage.Visible = false;
            // 
            // timerAnimation
            // 
            this.timerAnimation.Interval = 50;
            this.timerAnimation.Tick += new System.EventHandler(this.timerAnimation_Tick);
            // 
            // CollapsiblePanel
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlHeader);
            this.Name = "CollapsiblePanel";
            this.Size = new System.Drawing.Size(250, 150);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExpandCollapse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pictureBoxExpandCollapse;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Timer timerAnimation;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
