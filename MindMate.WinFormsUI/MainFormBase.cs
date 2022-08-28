using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MindMate.Controller;
using MindMate.Plugins;
using MindMate.Properties;
using MindMate.View;
using MindMate.View.EditorTabs;
using MindMate.WinFormsUI.Controller;
using MindMate.WinFormsUI.NoteEditing;

namespace MindMate.WinFormsUI
{
    public abstract class MainFormBase : Form, IMainForm
    {
        protected SplitContainer splitContainer1;
        public StatusBar statusStrip1;
        
        public MainFormBase()
        {
            InitializeComponent();
            
            CreateMenuSystem();
            
            SetupSideBar();
            editorTabs = new EditorTabs();
            splitContainer1.Panel1.Controls.Add(editorTabs);
            
            // moving splitter makes it the focused control, below event focuses the last control again
            splitContainer1.GotFocus += SplitContainer1_GotFocus;
            Shown += MainForm_Shown;
        }

        private EditorTabs editorTabs;

        public IEditorTabs EditorTabs => editorTabs;
        
        public bool IsNoteEditorActive
        {
            get { return ActiveControl == splitContainer1 && splitContainer1.ActiveControl == NoteEditor; }
        }
        
        private NoteEditor noteEditor;
        public INoteEditor NoteEditor { get => noteEditor; private set => noteEditor = (NoteEditor)value; }
        public ISideBarControl SideBarTabs { get; private set; }
        public IStatusBar StatusBar { get { return this.statusStrip1; } }

        public abstract void CreateMenuSystem();
        
        #region Manage Focus

        private Control focusedControl;
        public Control FocusedControl
        {
            get => focusedControl;

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

        

        private void FocusLastControl()
        {
            if (FocusedControl != null)
                FocusedControl.Focus();
            else
                editorTabs.Focus();
        }

        public void FocusMapView()
        {
            editorTabs.Focus();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            editorTabs.ControlGotFocus += (a, b) => FocusedControl = editorTabs;
            noteEditor.Document.Focusing += (a, b) => FocusedControl = noteEditor;
        }

        private void SplitContainer1_GotFocus(object sender, EventArgs e)
        {
            FocusLastControl();
        }
        
        /// <summary>
        /// Occurs when focus is transferred to a control which can have permanent focus. NoteEditor and EditorTab can have permanent focus.
        /// Focus can temporarily transfer to controls like Menu, Ribbon etc., but these are ignored by this event.
        /// </summary>
        public event FocusedControlChangeDelegate FocusedControlChanged;

        #endregion
        
        
        public abstract void InsertMenuItems(MainMenuItem[] menuItems);
        public abstract void RefreshRecentFilesMenuItems();
        public INoteEditorCtrl CreateNoteEditorController(MainCtrl mainCtrl)
        {
            return new NoteEditorCtrl(noteEditor, mainCtrl.PersistenceManager, mainCtrl.Dialogs);
        }
        
        protected void SetupSideBar()
        {
            var sideBar = new SideTabControl();

            SideBarTabs = sideBar;
            NoteEditor = sideBar.NoteEditor;
            
            this.splitContainer1.Panel2.Controls.Add(sideBar);
        }

        public void SetupTabController(MainCtrl mainCtrl)
        {
            new TabController(mainCtrl, editorTabs);
        }
        
        private void InitializeComponent()
        {
            statusStrip1 = new StatusBar();
            splitContainer1 = new SplitContainer();
            ((ISupportInitialize)(splitContainer1)).BeginInit();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 55);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Panel2MinSize = 0;
            splitContainer1.Size = new Size(977, 598);
            splitContainer1.SplitterDistance = 734;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Snow;
            ClientSize = new Size(977, 678);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip1);
            DoubleBuffered = true;
            Icon = Resources.MindMap;
            Margin = new Padding(4);
            Name = "MainForm";
            Text = "Mind Mate";
            WindowState = FormWindowState.Maximized;
            ((ISupportInitialize)(splitContainer1)).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}