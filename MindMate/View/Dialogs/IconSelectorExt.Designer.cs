namespace MindMate.View.Dialogs
{
    partial class IconSelectorExt
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbnRemove = new System.Windows.Forms.ToolStripButton();
            this.tbnRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.listView = new System.Windows.Forms.ListView();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 305);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(305, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel1.Text = "Select icon...";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbnRemove,
            this.tbnRemoveAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(305, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbnRemove
            // 
            this.tbnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnRemove.Image = global::MindMate.Properties.Resources.remove;
            this.tbnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnRemove.Name = "tbnRemove";
            this.tbnRemove.Size = new System.Drawing.Size(23, 22);
            this.tbnRemove.Text = "toolStripButton1";
            // 
            // tbnRemoveAll
            // 
            this.tbnRemoveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnRemoveAll.Image = global::MindMate.Properties.Resources.edittrash;
            this.tbnRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnRemoveAll.Name = "tbnRemoveAll";
            this.tbnRemoveAll.Size = new System.Drawing.Size(23, 22);
            this.tbnRemoveAll.Text = "toolStripButton1";
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Location = new System.Drawing.Point(10, 28);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(282, 264);
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // IconSelectorExt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 327);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IconSelectorExt";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select an Icon...";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IconSelector_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbnRemove;
        private System.Windows.Forms.ToolStripButton tbnRemoveAll;
        private System.Windows.Forms.ListView listView;
    }
}