using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.WinXP
{
    internal class Toolbar : ToolStrip
    {
        internal Toolbar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolbarButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonCut = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonPaste = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonFormatBold = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonFormatItalic = new System.Windows.Forms.ToolStripButton();
            this.toolbarButtonFormatFont = new System.Windows.Forms.ToolStripButton();

            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbarButtonNew,
            this.toolbarButtonOpen,
            this.toolbarButtonSave,
            this.toolStripSeparator4,
            this.toolbarButtonCut,
            this.toolbarButtonCopy,
            this.toolbarButtonPaste,
            this.toolbarButtonDelete,
            this.toolStripSeparator5,
            this.toolbarButtonFormatBold,
            this.toolbarButtonFormatItalic,
            this.toolbarButtonFormatFont});
            this.Location = new System.Drawing.Point(0, 28);
            this.Name = "toolStrip1";
            this.Size = new System.Drawing.Size(977, 27);
            this.TabIndex = 5;
            this.Text = "toolStrip1";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // toolbarButtonNew
            // 
            this.toolbarButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonNew.Image = global::MindMate.Properties.Resources.filenew;
            this.toolbarButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonNew.Name = "toolbarButtonNew";
            this.toolbarButtonNew.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonNew.ToolTipText = "New";
            this.toolbarButtonNew.Click += new System.EventHandler(this.toolbarButtonNew_Click);
            // 
            // toolbarButtonOpen
            // 
            this.toolbarButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonOpen.Image = global::MindMate.Properties.Resources.fileopen;
            this.toolbarButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonOpen.Name = "toolbarButtonOpen";
            this.toolbarButtonOpen.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonOpen.ToolTipText = "Open";
            this.toolbarButtonOpen.Click += new System.EventHandler(this.toolbarButtonOpen_Click);
            // 
            // toolbarButtonSave
            // 
            this.toolbarButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonSave.Image = global::MindMate.Properties.Resources.filesave;
            this.toolbarButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonSave.Name = "toolbarButtonSave";
            this.toolbarButtonSave.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonSave.ToolTipText = "Save";
            this.toolbarButtonSave.Click += new System.EventHandler(this.toolbarButtonSave_Click);
            // 
            // toolbarButtonCut
            // 
            this.toolbarButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonCut.Image = global::MindMate.Properties.Resources.cut;
            this.toolbarButtonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonCut.Name = "toolbarButtonCut";
            this.toolbarButtonCut.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonCut.ToolTipText = "Cut";
            this.toolbarButtonCut.Click += new System.EventHandler(this.toolbarButtonCut_Click);
            // 
            // toolbarButtonCopy
            // 
            this.toolbarButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonCopy.Image = global::MindMate.Properties.Resources.page_copy;
            this.toolbarButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonCopy.Name = "toolbarButtonCopy";
            this.toolbarButtonCopy.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonCopy.ToolTipText = "Copy";
            this.toolbarButtonCopy.Click += new System.EventHandler(this.toolbarButtonCopy_Click);
            // 
            // toolbarButtonPaste
            // 
            this.toolbarButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonPaste.Image = global::MindMate.Properties.Resources.page_paste;
            this.toolbarButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonPaste.Name = "toolbarButtonPaste";
            this.toolbarButtonPaste.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonPaste.ToolTipText = "Paste";
            this.toolbarButtonPaste.Click += new System.EventHandler(this.toolbarButtonPaste_Click);
            // 
            // toolbarButtonDelete
            // 
            this.toolbarButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonDelete.Image = global::MindMate.Properties.Resources.remove_arrows;
            this.toolbarButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonDelete.Name = "toolbarButtonDelete";
            this.toolbarButtonDelete.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonDelete.ToolTipText = "Delete";
            this.toolbarButtonDelete.Click += new System.EventHandler(this.toolbarButtonDelete_Click);
            // 
            // toolbarButtonFormatBold
            // 
            this.toolbarButtonFormatBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonFormatBold.Image = global::MindMate.Properties.Resources.text_bold;
            this.toolbarButtonFormatBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonFormatBold.Name = "toolbarButtonFormatBold";
            this.toolbarButtonFormatBold.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonFormatBold.ToolTipText = "Bold";
            this.toolbarButtonFormatBold.Click += new System.EventHandler(this.toolbarButtonFormatBold_Click);
            // 
            // toolbarButtonFormatItalic
            // 
            this.toolbarButtonFormatItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonFormatItalic.Image = global::MindMate.Properties.Resources.text_italic;
            this.toolbarButtonFormatItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonFormatItalic.Name = "toolbarButtonFormatItalic";
            this.toolbarButtonFormatItalic.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonFormatItalic.ToolTipText = "Italic";
            this.toolbarButtonFormatItalic.Click += new System.EventHandler(this.toolbarButtonFormatItalic_Click);
            // 
            // toolbarButtonFormatFont
            // 
            this.toolbarButtonFormatFont.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbarButtonFormatFont.Image = global::MindMate.Properties.Resources.font;
            this.toolbarButtonFormatFont.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarButtonFormatFont.Name = "toolbarButtonFormatFont";
            this.toolbarButtonFormatFont.Size = new System.Drawing.Size(24, 24);
            this.toolbarButtonFormatFont.ToolTipText = "Font";
            this.toolbarButtonFormatFont.Click += new System.EventHandler(this.toolbarButtonFormatFont_Click);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ToolStripButton toolbarButtonNew;
        private System.Windows.Forms.ToolStripButton toolbarButtonOpen;
        private System.Windows.Forms.ToolStripButton toolbarButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolbarButtonCut;
        private System.Windows.Forms.ToolStripButton toolbarButtonCopy;
        private System.Windows.Forms.ToolStripButton toolbarButtonPaste;
        private System.Windows.Forms.ToolStripButton toolbarButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolbarButtonFormatBold;
        private System.Windows.Forms.ToolStripButton toolbarButtonFormatItalic;
        private System.Windows.Forms.ToolStripButton toolbarButtonFormatFont;

        #region toolbar click events (routed to main menu items)

        public MainMenu MainMenu { get; set; }

        private void toolbarButtonNew_Click(object sender, EventArgs e)
        {
            MainMenu.newToolStripMenuItem.PerformClick();
        }

        private void toolbarButtonOpen_Click(object sender, EventArgs e)
        {
            MainMenu.openToolStripMenuItem.PerformClick();
        }

        private void toolbarButtonSave_Click(object sender, EventArgs e)
        {
            MainMenu.saveToolStripMenuItem.PerformClick();
        }
        private void toolbarButtonCut_Click(object sender, EventArgs e)
        {
            MainMenu.mCut.PerformClick();
        }
        private void toolbarButtonCopy_Click(object sender, EventArgs e)
        {
            MainMenu.mCopy.PerformClick();
        }

        private void toolbarButtonPaste_Click(object sender, EventArgs e)
        {
            MainMenu.mPaste.PerformClick();
        }

        private void toolbarButtonDelete_Click(object sender, EventArgs e)
        {
            MainMenu.mDelete.PerformClick();
        }

        private void toolbarButtonFormatBold_Click(object sender, EventArgs e)
        {
            MainMenu.mBold.PerformClick();
        }

        private void toolbarButtonFormatItalic_Click(object sender, EventArgs e)
        {
            MainMenu.mItalic.PerformClick();
        }

        private void toolbarButtonFormatFont_Click(object sender, EventArgs e)
        {
            MainMenu.mFont.PerformClick();
        }

        #endregion toolbar click events (routed to main menu items)
    }
}
