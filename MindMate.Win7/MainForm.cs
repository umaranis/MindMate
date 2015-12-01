/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;
using MindMate.Plugins;
using MindMate.View.NoteEditing;
using MindMate.Controller;

namespace MindMate.Win7
{
    public partial class MainForm : Form, View.IMainForm
    {
        private View.MapControls.MapViewPanel mapViewPanel;
        private MainCtrl mainCtrl;
                
        public MainForm(MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;
            Ribbon = new RibbonLib.Ribbon();
            Ribbon.ResourceName = "MindMate.Win7.View.Ribbon.RibbonMarkup.ribbon";
            InitializeComponent();
            this.Controls.Add(Ribbon);
            SetupSideBar();

            notesEditor.GotFocus += (a, b) => this.focusedControl = notesEditor; 

            // moving splitter makes it the focused control, below event focuses the last control again
            splitContainer1.GotFocus += (a, b) => FocusLastControl();

            // changing side bar tab gives focus away to tab control header, below event focuses relevant control again
            SideBarTabs.SelectedIndexChanged += SideBarTabs_SelectedIndexChanged;

        }

        public RibbonLib.Ribbon Ribbon { get; private set; }

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

        public View.StatusBar StatusBar { get { return this.statusStrip1; } }

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
        
        public void InsertMenuItems(MainMenuItem[] menuItems)
        {
            //throw new NotImplementedException();
        }

        public void RefreshRecentFilesMenuItems()
        {
            //MainMenuCtrl.RefreshRecentFilesMenuItems();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Control) == Keys.Control)
            {
                if (keyData == (Keys.Control | Keys.N))
                {
                    mainCtrl.NewMap();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.O))
                {
                    mainCtrl.OpenMap();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.S))
                {
                    mainCtrl.SaveMap();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.Z))
                {
                    mainCtrl.Undo();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.Y))
                {
                    mainCtrl.Redo();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.C))
                {
                    mainCtrl.Copy();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.V))
                {
                    mainCtrl.Paste();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.X))
                {
                    mainCtrl.Cut();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.B))
                {
                    mainCtrl.mapCtrl.MakeSelectedNodeBold();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.I))
                {
                    mainCtrl.mapCtrl.MakeSelectedNodeItalic();
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.D))
                {
                    mainCtrl.mapCtrl.ChangeFont();
                    return true;
                }
            }
            else if ((keyData & Keys.Alt) == Keys.Alt)
            {
                if (keyData == (Keys.Alt | Keys.I))
                {
                    mainCtrl.mapCtrl.AppendIconFromIconSelectorExt();
                    return true;
                }
            }            

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
