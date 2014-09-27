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
            this.tbnRemoveLast = new System.Windows.Forms.ToolStripButton();
            this.tbnRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.listView = new System.Windows.Forms.ListView();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbnViewLargeIcons = new System.Windows.Forms.ToolStripButton();
            this.tbnViewSmallIcons = new System.Windows.Forms.ToolStripButton();
            this.tbnViewList = new System.Windows.Forms.ToolStripButton();
            this.tbnViewTile = new System.Windows.Forms.ToolStripButton();
            this.tbnViewDetail = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
            this.tbnRemoveLast,
            this.tbnRemoveAll,
            this.toolStripSeparator1,
            this.tbnViewLargeIcons,
            this.tbnViewSmallIcons,
            this.tbnViewList,
            this.tbnViewTile,
            this.tbnViewDetail,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(305, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbnRemoveLast
            // 
            this.tbnRemoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnRemoveLast.Image = global::MindMate.Properties.Resources.remove;
            this.tbnRemoveLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnRemoveLast.Name = "tbnRemoveLast";
            this.tbnRemoveLast.Size = new System.Drawing.Size(23, 22);
            this.tbnRemoveLast.Text = "Remove Last Icon";
            this.tbnRemoveLast.Click += new System.EventHandler(this.tbnRemoveLast_Click);
            // 
            // tbnRemoveAll
            // 
            this.tbnRemoveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnRemoveAll.Image = global::MindMate.Properties.Resources.edittrash;
            this.tbnRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnRemoveAll.Name = "tbnRemoveAll";
            this.tbnRemoveAll.Size = new System.Drawing.Size(23, 22);
            this.tbnRemoveAll.Text = "Remove All Icons";
            this.tbnRemoveAll.ToolTipText = "Remove all Icons for selected node";
            this.tbnRemoveAll.Click += new System.EventHandler(this.tbnRemoveAll_Click);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbnViewLargeIcons
            // 
            this.tbnViewLargeIcons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnViewLargeIcons.Image = global::MindMate.Properties.Resources.application_view_tile;
            this.tbnViewLargeIcons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnViewLargeIcons.Name = "tbnViewLargeIcons";
            this.tbnViewLargeIcons.Size = new System.Drawing.Size(23, 22);
            this.tbnViewLargeIcons.Text = "Large Icon";
            this.tbnViewLargeIcons.Click += new System.EventHandler(this.tbnViewLargeIcons_Click);
            // 
            // tbnViewSmallIcons
            // 
            this.tbnViewSmallIcons.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnViewSmallIcons.Image = global::MindMate.Properties.Resources.application_view_icons;
            this.tbnViewSmallIcons.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnViewSmallIcons.Name = "tbnViewSmallIcons";
            this.tbnViewSmallIcons.Size = new System.Drawing.Size(23, 22);
            this.tbnViewSmallIcons.Text = "Small Icon";
            this.tbnViewSmallIcons.Click += new System.EventHandler(this.tbnViewSmallIcons_Click);
            // 
            // tbnViewList
            // 
            this.tbnViewList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnViewList.Image = global::MindMate.Properties.Resources.application_view_columns;
            this.tbnViewList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnViewList.Name = "tbnViewList";
            this.tbnViewList.Size = new System.Drawing.Size(23, 22);
            this.tbnViewList.Text = "List";
            this.tbnViewList.Click += new System.EventHandler(this.tbnViewList_Click);
            // 
            // tbnViewTile
            // 
            this.tbnViewTile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnViewTile.Image = global::MindMate.Properties.Resources.application_view_list;
            this.tbnViewTile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnViewTile.Name = "tbnViewTile";
            this.tbnViewTile.Size = new System.Drawing.Size(23, 22);
            this.tbnViewTile.Text = "Tile";
            this.tbnViewTile.Click += new System.EventHandler(this.tbnViewTile_Click);
            // 
            // tbnViewDetail
            // 
            this.tbnViewDetail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbnViewDetail.Image = global::MindMate.Properties.Resources.application_view_detail;
            this.tbnViewDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnViewDetail.Name = "tbnViewDetail";
            this.tbnViewDetail.Size = new System.Drawing.Size(23, 22);
            this.tbnViewDetail.Text = "Detail";
            this.tbnViewDetail.Click += new System.EventHandler(this.tbnViewDetail_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            this.Activated += new System.EventHandler(this.IconSelectorExt_Activated);
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
        private System.Windows.Forms.ToolStripButton tbnRemoveLast;
        private System.Windows.Forms.ToolStripButton tbnRemoveAll;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbnViewLargeIcons;
        private System.Windows.Forms.ToolStripButton tbnViewSmallIcons;
        private System.Windows.Forms.ToolStripButton tbnViewList;
        private System.Windows.Forms.ToolStripButton tbnViewTile;
        private System.Windows.Forms.ToolStripButton tbnViewDetail;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}