/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;

namespace MindMate.View
{
    public partial class MainForm : Form
    {
        private MapControls.MapViewPanel mapViewPanel;
                
        public MainForm()
        {
            InitializeComponent();
            SetupSideBar();

            notesEditor.GotFocus += (a, b) => this.focusedControl = notesEditor; 

            // moving splitter makes it the focused control, below event focuses the last control again
            splitContainer1.GotFocus += (a, b) => FocusLastControl();

            // changing side bar tab gives focus away to tab control header, below event focuses relevant control again
            SideBarTabs.SelectedIndexChanged += SideBarTabs_SelectedIndexChanged;
        }

        private void SideBarTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SideBarTabs.SelectedTab.Text == "Note Editor")
                SideBarTabs.SelectedTab.Controls[0].Focus();
            else
                FocusMapView();
        }

        private NoteEditor notesEditor;
        public NoteEditor NoteEditor
        {
            get { return this.notesEditor; }
        }

        private TabControl sideBarTabs;
        public TabControl SideBarTabs
        {
            get { return sideBarTabs; }
        }

        private Control focusedControl;

        private void FocusLastControl()
        {
            if (focusedControl != null)
                focusedControl.Focus();
        }

        public bool IsNoteEditorActive
        {
            get { return ActiveControl == splitContainer1 && splitContainer1.ActiveControl == notesEditor; }
        }        

        public void FocusMapView()
        {
            mapViewPanel.Focus();
        }

        private void SetupSideBar()
        {
            sideBarTabs = new TabControl();
            sideBarTabs.Dock = DockStyle.Fill;

            ImageList imageList = new ImageList();
            imageList.Images.Add(MindMate.Properties.Resources.knotes);
            sideBarTabs.ImageList = imageList;

            TabPage tPage = new TabPage("Note Editor");
            tPage.ImageIndex = 0;
            notesEditor = new NoteEditor();
            notesEditor.Dock = DockStyle.Fill;
            tPage.Controls.Add(notesEditor);


            sideBarTabs.TabPages.Add(tPage);
            this.splitContainer1.Panel2.Controls.Add(sideBarTabs);
        }

        public void AddMainView(View.MapControls.MapViewPanel mapViewPanel)
        {
            this.mapViewPanel = mapViewPanel;
            splitContainer1.Panel1.Controls.Add(mapViewPanel);
            mapViewPanel.MapView.CenterOnForm();
            mapViewPanel.GotFocus += (sender, e) => focusedControl = this.mapViewPanel;
        }

        #region toolbar click events (routed to main menu items)
        private void toolbarButtonNew_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem.PerformClick();
        }

        private void toolbarButtonOpen_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem.PerformClick();
        }

        private void toolbarButtonSave_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem.PerformClick();
        }
        private void toolbarButtonCut_Click(object sender, EventArgs e)
        {
            mCut.PerformClick();
        }
        private void toolbarButtonCopy_Click(object sender, EventArgs e)
        {
            mCopy.PerformClick();
        }

        private void toolbarButtonPaste_Click(object sender, EventArgs e)
        {
            mPaste.PerformClick();
        }

        private void toolbarButtonDelete_Click(object sender, EventArgs e)
        {
            mDelete.PerformClick();
        }

        private void toolbarButtonFormatBold_Click(object sender, EventArgs e)
        {
            mBold.PerformClick();
        }

        private void toolbarButtonFormatItalic_Click(object sender, EventArgs e)
        {
            mItalic.PerformClick();
        }

        private void toolbarButtonFormatFont_Click(object sender, EventArgs e)
        {
            mFont.PerformClick();
        }
        #endregion toolbar click events (routed to main menu items)

    }
}
