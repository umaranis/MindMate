using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.WinXP
{
    public class MainMenu : MenuStrip
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.mFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asBMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSepRecentFilesStart = new System.Windows.Forms.ToolStripSeparator();
            this.mSepRecentFilesEnd = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mEditMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mTextColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mBackColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mShape = new System.Windows.Forms.ToolStripMenuItem();
            this.mShapeFork = new System.Windows.Forms.ToolStripMenuItem();
            this.mShapeBubble = new System.Windows.Forms.ToolStripMenuItem();
            this.mShapeBox = new System.Windows.Forms.ToolStripMenuItem();
            this.mShapeBullet = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineThick1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineThick2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineThick4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mLineSolid = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineDashed = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineDotted = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineMixed = new System.Windows.Forms.ToolStripMenuItem();
            this.mLineColor = new System.Windows.Forms.ToolStripMenuItem();
            this.mTools = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.mRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.mDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mSelectIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.mBold = new System.Windows.Forms.ToolStripMenuItem();
            this.mItalic = new System.Windows.Forms.ToolStripMenuItem();
            this.mFont = new System.Windows.Forms.ToolStripMenuItem();

            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFileMenu,
            this.mEditMenu,
            this.mFormat,
            this.mTools,
            this.mHelp});
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "menuStrip1";
            this.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.Size = new System.Drawing.Size(977, 28);
            this.TabIndex = 2;
            this.Text = "menuStrip1";
            // 
            // mFileMenu
            // 
            this.mFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.mSepRecentFilesStart,
            this.mSepRecentFilesEnd,
            this.exitToolStripMenuItem});
            this.mFileMenu.Name = "mFileMenu";
            this.mFileMenu.Size = new System.Drawing.Size(44, 24);
            this.mFileMenu.Text = "&File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asBMPToolStripMenuItem});
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // asBMPToolStripMenuItem
            // 
            this.asBMPToolStripMenuItem.Name = "asBMPToolStripMenuItem";
            this.asBMPToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.asBMPToolStripMenuItem.Text = "as BMP ...";
            // 
            // mSepRecentFilesStart
            // 
            this.mSepRecentFilesStart.Name = "mSepRecentFilesStart";
            this.mSepRecentFilesStart.Size = new System.Drawing.Size(170, 6);
            // 
            // mSepRecentFilesEnd
            // 
            this.mSepRecentFilesEnd.Name = "mSepRecentFilesEnd";
            this.mSepRecentFilesEnd.Size = new System.Drawing.Size(170, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // mEditMenu
            // 
            this.mEditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mUndo,
            this.mRedo,
            this.toolStripSeparator6,
            this.mCut,
            this.mCopy,
            this.mPaste,
            this.mDelete});
            this.mEditMenu.Name = "mEditMenu";
            this.mEditMenu.Size = new System.Drawing.Size(47, 24);
            this.mEditMenu.Text = "&Edit";
            // 
            // formatToolStripMenuItem
            // 
            this.mFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSelectIcon,
            this.mBold,
            this.mItalic,
            this.mFont,
            this.toolStripSeparator2,
            this.mTextColor,
            this.mBackColor,
            this.mShape,
            this.mLineStyle,
            this.mLineColor});
            this.mFormat.Name = "formatToolStripMenuItem";
            this.mFormat.Size = new System.Drawing.Size(68, 24);
            this.mFormat.Text = "For&mat";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // mTextColor
            // 
            this.mTextColor.Name = "mTextColor";
            this.mTextColor.Size = new System.Drawing.Size(216, 26);
            this.mTextColor.Text = "Text Color ...";
            // 
            // mBackColor
            // 
            this.mBackColor.Name = "mBackColor";
            this.mBackColor.Size = new System.Drawing.Size(216, 26);
            this.mBackColor.Text = "Background Color ...";
            // 
            // mShape
            // 
            this.mShape.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mShapeFork,
            this.mShapeBubble,
            this.mShapeBox,
            this.mShapeBullet});
            this.mShape.Name = "mShape";
            this.mShape.Size = new System.Drawing.Size(216, 26);
            this.mShape.Text = "Shape";
            // 
            // mShapeFork
            // 
            this.mShapeFork.Checked = true;
            this.mShapeFork.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mShapeFork.Name = "mShapeFork";
            this.mShapeFork.Size = new System.Drawing.Size(131, 26);
            this.mShapeFork.Text = "Fork";
            // 
            // mShapeBubble
            // 
            this.mShapeBubble.Name = "mShapeBubble";
            this.mShapeBubble.Size = new System.Drawing.Size(131, 26);
            this.mShapeBubble.Text = "Bubble";
            // 
            // mShapeBox
            // 
            this.mShapeBox.Name = "mShapeBox";
            this.mShapeBox.Size = new System.Drawing.Size(131, 26);
            this.mShapeBox.Text = "Box";
            // 
            // mShapeBullet
            // 
            this.mShapeBullet.Name = "mShapeBullet";
            this.mShapeBullet.Size = new System.Drawing.Size(131, 26);
            this.mShapeBullet.Text = "Bullet";
            // 
            // mLineStyle
            // 
            this.mLineStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mLineThick1,
            this.mLineThick2,
            this.mLineThick4,
            this.toolStripSeparator3,
            this.mLineSolid,
            this.mLineDashed,
            this.mLineDotted,
            this.mLineMixed});
            this.mLineStyle.Name = "mLineStyle";
            this.mLineStyle.Size = new System.Drawing.Size(216, 26);
            this.mLineStyle.Text = "Line Style";
            // 
            // mLineThick1
            // 
            this.mLineThick1.Name = "mLineThick1";
            this.mLineThick1.Size = new System.Drawing.Size(158, 26);
            this.mLineThick1.Text = "Thickness 1";
            // 
            // mLineThick2
            // 
            this.mLineThick2.Name = "mLineThick2";
            this.mLineThick2.Size = new System.Drawing.Size(158, 26);
            this.mLineThick2.Text = "Thickness 2";
            // 
            // mLineThick4
            // 
            this.mLineThick4.Name = "mLineThick4";
            this.mLineThick4.Size = new System.Drawing.Size(158, 26);
            this.mLineThick4.Text = "Thickness 4";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(155, 6);
            // 
            // mLineSolid
            // 
            this.mLineSolid.Name = "mLineSolid";
            this.mLineSolid.Size = new System.Drawing.Size(158, 26);
            this.mLineSolid.Text = "Solid";
            // 
            // mLineDashed
            // 
            this.mLineDashed.Name = "mLineDashed";
            this.mLineDashed.Size = new System.Drawing.Size(158, 26);
            this.mLineDashed.Text = "Dashed";
            // 
            // mLineDotted
            // 
            this.mLineDotted.Name = "mLineDotted";
            this.mLineDotted.Size = new System.Drawing.Size(158, 26);
            this.mLineDotted.Text = "Dotted";
            // 
            // mLineMixed
            // 
            this.mLineMixed.Name = "mLineMixed";
            this.mLineMixed.Size = new System.Drawing.Size(158, 26);
            this.mLineMixed.Text = "Mixed";
            // 
            // mLineColor
            // 
            this.mLineColor.Name = "mLineColor";
            this.mLineColor.Size = new System.Drawing.Size(216, 26);
            this.mLineColor.Text = "Line Color ...";
            // 
            // toolsToolStripMenuItem
            // 
            this.mTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem});
            this.mTools.Name = "toolsToolStripMenuItem";
            this.mTools.Size = new System.Drawing.Size(57, 24);
            this.mTools.Text = "&Tools";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.preferencesToolStripMenuItem.Text = "Preferences ...";
            // 
            // aboutToolStripMenuItem
            // 
            this.mHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mAbout});
            this.mHelp.Name = "mHelp";
            this.mHelp.Size = new System.Drawing.Size(53, 24);
            this.mHelp.Text = "Help";
            // 
            // mAbout
            // 
            this.mAbout.Name = "mAbout";
            this.mAbout.Size = new System.Drawing.Size(214, 26);
            this.mAbout.Text = "About Mind Mate ...";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(178, 6);
            // 
            // mUndo
            // 
            this.mUndo.Image = global::MindMate.Properties.Resources.undo;
            this.mUndo.Name = "mUndo";
            this.mUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mUndo.Size = new System.Drawing.Size(181, 26);
            this.mUndo.Text = "Undo";
            // 
            // mRedo
            // 
            this.mRedo.Image = global::MindMate.Properties.Resources.redo;
            this.mRedo.Name = "mRedo";
            this.mRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.mRedo.Size = new System.Drawing.Size(181, 26);
            this.mRedo.Text = "Redo";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::MindMate.Properties.Resources.filenew;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::MindMate.Properties.Resources.fileopen;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::MindMate.Properties.Resources.filesave;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::MindMate.Properties.Resources.filesaveas;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            // 
            // mCut
            // 
            this.mCut.Image = global::MindMate.Properties.Resources.cut;
            this.mCut.Name = "mCut";
            this.mCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mCut.Size = new System.Drawing.Size(181, 26);
            this.mCut.Text = "Cut";
            // 
            // mCopy
            // 
            this.mCopy.Image = global::MindMate.Properties.Resources.page_copy;
            this.mCopy.Name = "mCopy";
            this.mCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mCopy.Size = new System.Drawing.Size(181, 26);
            this.mCopy.Text = "Copy";
            // 
            // mPaste
            // 
            this.mPaste.Image = global::MindMate.Properties.Resources.page_paste;
            this.mPaste.Name = "mPaste";
            this.mPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mPaste.Size = new System.Drawing.Size(181, 26);
            this.mPaste.Text = "Paste";
            // 
            // mDelete
            // 
            this.mDelete.Image = global::MindMate.Properties.Resources.remove_arrows;
            this.mDelete.Name = "mDelete";
            this.mDelete.ShortcutKeyDisplayString = "Del";
            this.mDelete.Size = new System.Drawing.Size(181, 26);
            this.mDelete.Text = "Delete";
            // 
            // mSelectIcon
            // 
            this.mSelectIcon.Image = global::MindMate.Properties.Resources.smartart_change_color_gallery_16;
            this.mSelectIcon.Name = "mSelectIcon";
            this.mSelectIcon.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.I)));
            this.mSelectIcon.Size = new System.Drawing.Size(216, 26);
            this.mSelectIcon.Text = "Select Icon ...";
            // 
            // mBold
            // 
            this.mBold.Image = global::MindMate.Properties.Resources.text_bold;
            this.mBold.Name = "mBold";
            this.mBold.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.mBold.Size = new System.Drawing.Size(216, 26);
            this.mBold.Text = "Bold";
            // 
            // mItalic
            // 
            this.mItalic.Image = global::MindMate.Properties.Resources.text_italic;
            this.mItalic.Name = "mItalic";
            this.mItalic.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.mItalic.Size = new System.Drawing.Size(216, 26);
            this.mItalic.Text = "Italic";
            // 
            // mFont
            // 
            this.mFont.Image = global::MindMate.Properties.Resources.font;
            this.mFont.Name = "mFont";
            this.mFont.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mFont.Size = new System.Drawing.Size(216, 26);
            this.mFont.Text = "Font ...";

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem mFormat;
        public System.Windows.Forms.ToolStripMenuItem mTools;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStripMenuItem mSelectIcon;
        public System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem mBold;
        public System.Windows.Forms.ToolStripMenuItem mItalic;
        public System.Windows.Forms.ToolStripMenuItem mShapeFork;
        public System.Windows.Forms.ToolStripMenuItem mShapeBubble;
        public System.Windows.Forms.ToolStripMenuItem mShapeBox;
        public System.Windows.Forms.ToolStripMenuItem mShape;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripMenuItem mLineStyle;
        public System.Windows.Forms.ToolStripMenuItem mLineThick1;
        public System.Windows.Forms.ToolStripMenuItem mLineThick2;
        public System.Windows.Forms.ToolStripMenuItem mLineThick4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStripMenuItem mLineSolid;
        public System.Windows.Forms.ToolStripMenuItem mLineDashed;
        public System.Windows.Forms.ToolStripMenuItem mLineDotted;
        public System.Windows.Forms.ToolStripMenuItem mLineMixed;
        public System.Windows.Forms.ToolStripMenuItem mLineColor;
        public System.Windows.Forms.ToolStripMenuItem mFont;
        public System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem asBMPToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem mTextColor;
        public System.Windows.Forms.ToolStripMenuItem mBackColor;
        public System.Windows.Forms.ToolStripMenuItem mHelp;
        public System.Windows.Forms.ToolStripMenuItem mAbout;
        public System.Windows.Forms.ToolStripMenuItem mShapeBullet;
        public System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem mFileMenu;
        public System.Windows.Forms.ToolStripSeparator mSepRecentFilesStart;
        public System.Windows.Forms.ToolStripSeparator mSepRecentFilesEnd;
        public System.Windows.Forms.ToolStripMenuItem mCopy;
        public System.Windows.Forms.ToolStripMenuItem mPaste;
        public System.Windows.Forms.ToolStripMenuItem mCut;
        public System.Windows.Forms.ToolStripMenuItem mDelete;
        public System.Windows.Forms.ToolStripMenuItem mUndo;
        public System.Windows.Forms.ToolStripMenuItem mRedo;
        public System.Windows.Forms.ToolStripMenuItem mEditMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;



    }
}
