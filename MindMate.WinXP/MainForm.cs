/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2016 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;
using MindMate.Plugins;
using MindMate.View.NoteEditing;
using MindMate.View;
using MindMate.View.EditorTabs;

namespace MindMate.WinXP
{
    public partial class MainForm : Form, View.IMainForm
    {

        public MainForm()
        {
            InitializeComponent();
            toolStrip1.MainMenu = MainMenu;
            SetupSideBar();

            // moving splitter makes it the focused control, below event focuses the last control again
            splitContainer1.GotFocus += SplitContainer1_GotFocus;

            EditorTabs = new EditorTabs();
            splitContainer1.Panel1.Controls.Add(EditorTabs);

            Shown += MainForm_Shown;
        }          

        #region Manage Focus

        private Control focusedControl;
        public Control FocusedControl
        {
            get
            {
                return focusedControl;
            }

            set
            {
                if (focusedControl != value)
                {
                    var oldvalue = focusedControl;
                    focusedControl = value;                    
                    FocusedControlChanged?.Invoke(value, oldvalue);
                }
            }
        }

        /// <summary>
        /// Occurs when focus is tranferred to a control which can have permanent focus. NoteEditor and EditorTab can have permanent focus.
        /// Focus can temporarily transfer to controls like Menu, Ribbon etc., but these are ignored by this event.
        /// </summary>
        public event FocusedControlChangeDelegate FocusedControlChanged;        

        private void FocusLastControl()
        {
            if (FocusedControl != null)
                FocusedControl.Focus();
            else
                EditorTabs.Focus();
        }

        public void FocusMapView()
        {
            EditorTabs.Focus();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            EditorTabs.ControlGotFocus += (a, b) => FocusedControl = EditorTabs;
            NoteEditor.Document.Focusing += (a, b) => FocusedControl = NoteEditor;
        }

        private void SplitContainer1_GotFocus(object sender, EventArgs e)
        {
            FocusLastControl();
        }

        #endregion

		public MainMenu MainMenu { get { return menuStrip; } }
        public MainMenuCtrl MainMenuCtrl { get; set; }        

        public EditorTabs EditorTabs { get; private set; }
        public SideTabControl SideBarTabs { get; private set; }
        public NoteEditor NoteEditor { get; private set; }

        public View.StatusBar StatusBar { get { return this.statusStrip1; } }

        public bool IsNoteEditorActive
        {
            get { return ActiveControl == splitContainer1 && splitContainer1.ActiveControl == NoteEditor; }
        }
        
        private void SetupSideBar()
        {
            SideBarTabs = new SideTabControl();
            NoteEditor = SideBarTabs.NoteEditor;
            
            this.splitContainer1.Panel2.Controls.Add(SideBarTabs);
        }

        public void InsertMenuItems(MainMenuItem[] menuItems)
        {
            MainMenuCtrl.InsertMenuItems(menuItems);
        }

        public void RefreshRecentFilesMenuItems()
        {
            MainMenuCtrl.RefreshRecentFilesMenuItems();
        }
    }
}
