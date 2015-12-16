/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;
using MindMate.Plugins;
using MindMate.View.NoteEditing;
using MindMate.Controller;
using MindMate.View;
using MindMate.View.EditorTabs;

namespace MindMate.Win7
{
    public partial class MainForm : Form, View.IMainForm
    {
        private readonly MainCtrl mainCtrl;
                
        public MainForm(MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;
            Ribbon = new RibbonLib.Ribbon {ResourceName = "MindMate.Win7.View.Ribbon.RibbonMarkup.ribbon"};
            InitializeComponent();
            this.Controls.Add(Ribbon);
            SetupSideBar();

            // moving splitter makes it the focused control, below event focuses the last control again
            splitContainer1.GotFocus += (a, b) => FocusLastControl();
            splitContainer1.MouseDown += SplitContainer1_MouseDown;

            // changing side bar tab gives focus away to tab control header, below event focuses relevant control again
            SideBarTabs.SelectedIndexChanged += SideBarTabs_SelectedIndexChanged;

            EditorTabs = new EditorTabs();
            splitContainer1.Panel1.Controls.Add(EditorTabs);
        }

        #region Manage Focus

        private Control focusedControl;

        private void FocusLastControl()
        {
            if (focusedControl != null)
                focusedControl.Focus();
            else
                EditorTabs.SelectedTab.Control.Focus();
        }

        public void FocusMapView()
        {
            EditorTabs.Focus();
        }

        private void SplitContainer1_MouseDown(object sender, MouseEventArgs e)
        {
            focusedControl = NoteEditor.Focused ? NoteEditor : EditorTabs.SelectedTab.Control;
        }

        private void SideBarTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SideBarTabs.SelectedTab.Text == "Note Editor")
                SideBarTabs.SelectedTab.Controls[0].Focus();
            else
                FocusMapView();
        }

        #endregion

        public RibbonLib.Ribbon Ribbon { get; private set; }

        public EditorTabs EditorTabs { get; private set; }
        public TabControl SideBarTabs { get; private set; }
        public NoteEditor NoteEditor { get; private set; }

        public View.StatusBar StatusBar { get { return this.statusStrip1; } }

        public bool IsNoteEditorActive
        {
            get { return ActiveControl == splitContainer1 && splitContainer1.ActiveControl == NoteEditor; }
        }

        private void SetupSideBar()
        {
            SideBarTabs = new TabControl();
            SideBarTabs.Dock = DockStyle.Fill;
            //SideBarTabs.Alignment = TabAlignment.Bottom;

            ImageList imageList = new ImageList();
            imageList.Images.Add(MindMate.Properties.Resources.knotes);
            SideBarTabs.ImageList = imageList;

            TabPage tPage = new TabPage("Note Editor") {ImageIndex = 0};
            NoteEditor = new NoteEditor {Dock = DockStyle.Fill};
            tPage.Controls.Add(NoteEditor);


            SideBarTabs.TabPages.Add(tPage);
            this.splitContainer1.Panel2.Controls.Add(SideBarTabs);
        }

        //TODO: Implement this functionality for Ribbon
        public void InsertMenuItems(MainMenuItem[] menuItems)
        {
            //throw new NotImplementedException();
        }

        //TODO: Implement this functionality for Ribbon
        public void RefreshRecentFilesMenuItems()
        {
            //MainMenuCtrl.RefreshRecentFilesMenuItems();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Control) == Keys.Control)
            {
                switch (keyData)
                {
                    case (Keys.Control | Keys.N):
                        mainCtrl.NewMap();
                        return true;
                    case (Keys.Control | Keys.O):
                        mainCtrl.OpenMap();
                        return true;
                    case (Keys.Control | Keys.S):
                        mainCtrl.SaveCurrentMap();
                        return true;
                    case (Keys.Control | Keys.Z):
                        mainCtrl.Undo();
                        return true;
                    case (Keys.Control | Keys.Y):
                        mainCtrl.Redo();
                        return true;
                    case (Keys.Control | Keys.C):
                        mainCtrl.Copy();
                        return true;
                    case (Keys.Control | Keys.V):
                        mainCtrl.Paste();
                        return true;
                    case (Keys.Control | Keys.X):
                        mainCtrl.Cut();
                        return true;
                    case (Keys.Control | Keys.B):
                        mainCtrl.CurrentMapCtrl.MakeSelectedNodeBold();
                        return true;
                    case (Keys.Control | Keys.I):
                        mainCtrl.CurrentMapCtrl.MakeSelectedNodeItalic();
                        return true;
                    case (Keys.Control | Keys.D):
                        mainCtrl.CurrentMapCtrl.ChangeFont();
                        return true;
                }
            }
            else if ((keyData & Keys.Alt) == Keys.Alt)
            {
                switch (keyData)
                {
                    case (Keys.Alt | Keys.I):
                        mainCtrl.CurrentMapCtrl.AppendIconFromIconSelectorExt();
                        return true;
                }
            }            

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
