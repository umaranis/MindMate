/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using MindMate.View;
using MindMate.View.Dialogs;
using System.Windows.Forms;
using MindMate.Model;
using MindMate.Serialization;
using System.IO;
using MindMate.Modules.Undo;
using MindMate.View.EditorTabs;
using MindMate.View.MapControls;
using MindMate.Modules.Logging;

namespace MindMate.Controller
{
    /// <summary>
    /// Controlller for:
    /// - Launching and Closing of MindMate application
    /// - Creating (New), Opening, Saving and Closing maps 
    /// - Passing on actions to other controllers
    /// </summary>
    public class MainCtrl
    {
        private View.IMainForm mainForm;
        private Plugins.PluginManager pluginManager;
        public DialogManager Dialogs { get; private set; }

        public MapCtrl CurrentMapCtrl
        {
            get
            {
                Tab tab = mainForm.EditorTabs.SelectedTab as Tab;
                return (MapCtrl)tab?.ControllerTag;
            }
        }

        public ChangeManager ChangeManager { get { return CurrentMapCtrl.MapView.Tree.ChangeManager; } }

        public WinFormsStatusBarCtrl statusBarCtrl;

        private NoteEditorCtrl noteCrtl;
        public NoteEditorCtrl NoteCrtl
        {
            get
            {
                return noteCrtl;
            }            
        }


        public NodeContextMenu NodeContextMenu { get; private set; }        

        private TaskScheduler.TaskScheduler schedular;

        public PersistenceManager PersistenceManager
        {
            get; private set;
        }

        public const string APPLICATION_NAME = "Mind Mate";

		#region Launch MindMate application
        
        public void InitMindMate(IMainForm mainForm, DialogManager dialogs)
        {
            this.mainForm = mainForm;
            MetaModel.MetaModel.Initialize();
            schedular = new TaskScheduler.TaskScheduler();
            PersistenceManager = new PersistenceManager();            
            pluginManager = new Plugins.PluginManager(this);
            new TabController(this, mainForm);
            pluginManager.Initialize();
            Dialogs = dialogs;
            Dialogs.StatusBarCtrl = new WinFormsStatusBarCtrl(mainForm.StatusBar, PersistenceManager);
            NodeContextMenu = new NodeContextMenu();
            mainForm.Load += mainForm_Load;
            mainForm.Shown += mainForm_AfterReady;
            // changing side bar tab gives focus away to tab control header, below event focuses relevant control again
            mainForm.SideBarTabs.SelectedIndexChanged += SideBarTabs_SelectedIndexChanged;
        }

        void mainForm_Load(object sender, EventArgs e)
        {
            MapTree tree;

            string fileArg = ProgramMainHelper.GetFileToOpenFromAppArguments(mainForm);
            if (fileArg != null)
            {
                try
                {
                    tree = PersistenceManager.OpenTree(fileArg);
                }
                catch (Exception exp)
                {
                    tree = PersistenceManager.NewTree();
                    MetaModel.MetaModel.Instance.LastOpenedFile = null;
                    Log.Write("Couldn't load the file provided in application argument. " + exp.Message);
                }
            } 
            else if (MetaModel.MetaModel.Instance.LastOpenedFile == null)
            {
                tree = PersistenceManager.NewTree();          
            }
            else
            {
                try
                {
                    tree = PersistenceManager.OpenTree(MetaModel.MetaModel.Instance.LastOpenedFile);
                }
                catch(Exception exp)
                {
                    tree = PersistenceManager.NewTree();
                    MetaModel.MetaModel.Instance.LastOpenedFile = null;
                    Log.Write("Couldn't load last opened file. " + exp.Message);
                }
            }

            noteCrtl = new NoteEditorCtrl(mainForm.NoteEditor, PersistenceManager, Dialogs);             

            pluginManager.InitializeContextMenu(NodeContextMenu);
            
            new ContextMenuCtrl(this, NodeContextMenu);
            
            pluginManager.InitializeSideBarWindow(mainForm.SideBarTabs);
            
            pluginManager.InitializeMainMenu(mainForm);
            
            mainForm.NoteEditor.OnDirty += (a) => {
                if(PersistenceManager.CurrentTree != null)
                {
                    PersistenceManager.CurrentTree.IsDirty = true;
                }
                }; // register for NoteEditor changes

            mainForm.NoteEditor.OnSave += (obj) =>
            {
                if (this.PersistenceManager.CurrentTree.IsNewMap)
                {
                    // bug fix: if the map is new and following call will trigger a file save dialog, we have to do it through a separate thread to avoid 'S' being written in the note editor
                    Action action = () => SaveCurrentMap();
                    this.schedular.AddTask(() => ((Control)mainForm).Invoke(action), DateTime.Now);
                }
                else
                {
                    SaveCurrentMap();
                }
            };

            mainForm.FormClosing += mainForm_FormClosing;

        }
                
        /// <summary>
        /// Form.Shown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void mainForm_AfterReady(object sender, EventArgs args)
        {
            pluginManager.OnApplicationReady();
            schedular.Start();
            new SearchController(mainForm.SideBarTabs.SearchControl, () => CurrentMapCtrl.MapView.Tree, action => ScheduleTask(action));
        }

        #endregion Launch MindMate application

        #region Shutdown MindMate application

        void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PromptForUnsavedChanges() == ContinueOperation.Continue &&
                PromptForLosingClipboardData() == ContinueOperation.Continue)
            {
                SaveSettingsAtClose();
                schedular.Stop();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void SaveSettingsAtClose()
        {
            MetaModel.MetaModel.Instance.LastOpenedFile = PersistenceManager.CurrentTree?.FileName;
            MetaModel.MetaModel.Instance.Save();
        }

        #endregion Shutdown MindMate application

        #region Coordinating actions and dialogs

        public void ReturnFocusToMapView()
        {
            mainForm.FocusMapView();
        }

        private void SideBarTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mainForm.SideBarTabs.SelectedTab == mainForm.SideBarTabs.NoteTab)
                mainForm.SideBarTabs.SelectedTab.Controls[0].Focus();
            else if (mainForm.SideBarTabs.SelectedTab == mainForm.SideBarTabs.SearchTab)
                mainForm.SideBarTabs.SearchControl.Focus();
            else
                mainForm.FocusMapView();
        }

        public void ShowApplicationOptions()
        {
            Options frm = new Options(this, this.noteCrtl);
            frm.ShowDialog();
        }

        public void ExportAsBmp()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "bmp";
            file.Filter = "Bitmap Image (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                using (var bmp = CurrentMapCtrl.MapView.DrawToBitmap())
                {
                    bmp.Save(file.FileName);
                }
            }
        }

        public void ExportAsPng()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "png";
            file.Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                using (var bmp = CurrentMapCtrl.MapView.DrawToBitmap())
                {
                    bmp.Save(file.FileName, ImageFormat.Png);
                }
            }
        }

        public void ExportAsJpg()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "jpg";
            file.Filter = "JPEG Image (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                using (var bmp = CurrentMapCtrl.MapView.DrawToBitmap())
                {
                    bmp.Save(file.FileName, ImageFormat.Jpeg);
                }
            }
        }

        public void ExportAsHtml()
        {
            try
            {


                SaveFileDialog file = new SaveFileDialog();
                file.AddExtension = true;
                file.DefaultExt = "html";
                file.Filter = "HTML (*.html)|*.html|All files (*.*)|*.*";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    var serializer = new HtmlSerializer();
                    serializer.Serialize(PersistenceManager.CurrentTree, file.FileName);
                }
            }
            catch(Exception exp)
            {
                Log.Write("Couldn't export to HTML: " + exp.Message);
                MessageBox.Show("Export to HTML failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ShowAboutBox()
        {
            new AboutBox().ShowDialog();
        }

        public void SetMapViewBackColor(System.Drawing.Color color)
        {
            CurrentMapCtrl.SetMapViewBackColor(color);
        }

        /// <summary>
        /// Schedule task to run in a separate thread
        /// </summary>
        /// <param name="task"></param>
        public void ScheduleTask(TaskScheduler.ITask task)
        {
            schedular.AddTask(task);
        }

        /// <summary>
        /// Schedule task to run in a separate thread
        /// </summary>
        public void ScheduleTask(Action action)
        {
            schedular.AddTask(action, DateTime.Now);
        }

        /// <summary>
        /// ReSchedule task running in a separate thread
        /// </summary>
        public void RescheduleTask(TaskScheduler.ITask task, DateTime startTime)
        {
            schedular.UpdateTask(task, startTime);
        }

        public void Copy()
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.Copy();
            else
                CurrentMapCtrl.Copy();
        }

        public void Cut()
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.Cut();
            else
                CurrentMapCtrl.Cut();
        }            

        public void Paste(bool asText = false)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.Paste();
            else
                CurrentMapCtrl.Paste(asText);
        }

        public void Undo()
        {
            if (CurrentMapCtrl.MapView.NodeTextEditor.IsTextEditing)
                CurrentMapCtrl.MapView.NodeTextEditor.Undo();
            else
                ChangeManager.Undo();
        }

        public void Redo()
        {
            if (!CurrentMapCtrl.MapView.NodeTextEditor.IsTextEditing)
                ChangeManager.Redo();
        }

        public void Bold(bool bold)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.ToggleSelectionBold();
            else
                CurrentMapCtrl.ChangeBold(bold);
        }

        public void Italic(bool italic)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.ToggleSelectionItalic();
            else
                CurrentMapCtrl.ChangeItalic(italic);
        }

        public void Underline(bool underline)
        {
            mainForm.NoteEditor.ToggleSelectionUnderline();
        }

        public void Strikethrough(Boolean strikethrough)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.ToggleSelectionStrikethrough();
            else
                CurrentMapCtrl.ChangeStrikeout(strikethrough);
        }

        public void Subscript()
        {
            mainForm.NoteEditor.SetSelectionAsSubscript();
        }

        public void Superscript()
        {
            mainForm.NoteEditor.SetSelectionAsSuperscript();
        }

        public void SetFontFamily(string fontName)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.SetSelectionFontFamily(fontName);
            else
                CurrentMapCtrl.SetFontFamily(fontName);
        }

        public void SetFontSize(float size)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.SetSelectionFontSize(size);
            else
                CurrentMapCtrl.SetFontSize(size);
        }

        public void SetForeColor(System.Drawing.Color color)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.SetSelectionForeColor(color);
            else
                CurrentMapCtrl.ChangeTextColor(color);
        }

        public void SetBackColor(System.Drawing.Color color)
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.SetSelectionBackColor(color);
            else
                CurrentMapCtrl.ChangeBackColor(color);
        }

        public void ClearSelectionFormatting()
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.ClearSelectionFormatting();
            else
                CurrentMapCtrl.ClearFormatting();
        }

        /// <summary>
        /// Selected Nodes of the currently active MapView
        /// </summary>
        public SelectedNodes ActiveNodes
        {
            get
            {
                return CurrentMapCtrl.MapView.Tree.SelectedNodes;
            }
        }

        public void StartNoteEditing()
        {
            mainForm.SideBarTabs.SelectedTab = mainForm.SideBarTabs.NoteTab;
            mainForm.SideBarTabs.NoteEditor.Focus();
        }

		public void InsertImage()
		{
            if (mainForm.IsNoteEditorActive)
                NoteCrtl.InsertImage();
            else
            
                CurrentMapCtrl.InsertImage();            
		}

        public void ViewNoteTab()
        {
            mainForm.SideBarTabs.SelectedTab = mainForm.SideBarTabs.NoteTab;
        }

        public void ViewTaskListTab()
        {
            mainForm.SideBarTabs.SelectedTab = mainForm.SideBarTabs.TaskListTab;
        }

        #endregion Coordinating actions and dialogs

        #region New / Open Map

        /// <summary>
        /// Create a new Mind Map
        /// </summary>
        public void NewMap()
        {
            PersistenceManager.NewTree();
        }

        public void OpenMap(string fileName = null)
        {
            if (fileName == null)
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
                if (file.ShowDialog() != DialogResult.OK)
                    return;
                else
                    fileName = file.FileName;
            }

            //file already open
            PersistentTree persistentTree = PersistenceManager.Find(t => t.FileName == fileName);
            if (persistentTree != null)
            {
                PersistenceManager.CurrentTree = persistentTree;
                return;
            }

            MapTree tree;
            try
            {
                tree = PersistenceManager.OpenTree(fileName);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MetaModel.MetaModel.Instance.RecentFiles.Add(fileName);
            mainForm.RefreshRecentFilesMenuItems();
        }

        #endregion New / Open Map

        #region Save Map

        public void SaveCurrentMap()
        {
            SaveMap(PersistenceManager.CurrentTree);       
        }

        public void SaveMap(PersistentTree tree)
        {
            if (tree != null)
            {
                if (tree.IsNewMap)
                {
                    SaveAsMap(tree);
                }
                else
                {
                    SaveMapInternal(tree);
                }
            }
        }

        public void SaveCurrentMapAs()
        {
            if (PersistenceManager.CurrentTree != null)
            {
                SaveAsMap(PersistenceManager.CurrentTree);
            }
        }

        public void SaveAll()
        {
            foreach (PersistentTree tree in PersistenceManager)
            {
                if (tree.IsDirty)
                {
                    SaveMap(tree);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree">should not be null</param>
        public void SaveAsMap(PersistentTree tree)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "mm";
            file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
            file.InitialDirectory = Path.GetDirectoryName(PersistenceManager.CurrentTree.FileName);
            file.FileName = tree.IsNewMap? CurrentMapCtrl.MapView.Tree.RootNode.Text : PersistenceManager.CurrentTree.FileName;
            if (file.ShowDialog() == DialogResult.OK)
            {
                SaveMapInternal(tree, file.FileName);
            }
            mainForm.EditorTabs.UpdateAppTitle();
        }

        /// <summary>
        /// Method which actually saves the file to disk. Other methods like SaveAsMap and SaveMap invoke this.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="fileName"></param>
        private void SaveMapInternal(PersistentTree tree, string fileName = null)
        {
            Debug.Assert(tree.FileName != null || fileName != null, "Saving: Missing file name.");

            noteCrtl.UpdateNodeFromEditor();

            if (tree != null)
            {
                if (fileName == null)
                {
                    tree.Save();
                }
                else
                {
                    tree.Save(fileName);
                }
                
                MetaModel.MetaModel.Instance.RecentFiles.Add(tree.FileName);
            }
        }

        #endregion Save Map

        #region Close Map

        public void CloseCurrentMap()
        {
            if (PromptForUnsavedChanges(PersistenceManager.CurrentTree) == ContinueOperation.Continue)
            {
                var tree = PersistenceManager.CurrentTree;
                if (mainForm.EditorTabs.TabCount == 1)
                {
                    NewMap();
                }
                PersistenceManager.Close(tree);
                mainForm.RefreshRecentFilesMenuItems();
            }
        }

        private enum ContinueOperation { Continue, Cancel };

        private ContinueOperation PromptForUnsavedChanges(PersistentTree tree)
        {
            if (tree.IsDirty)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveMap(tree);
                        break;
                    case DialogResult.Cancel:
                        return ContinueOperation.Cancel;
                }
            }

            return ContinueOperation.Continue;
        }        

        private ContinueOperation PromptForUnsavedChanges()
        {
            if (PersistenceManager.IsDirty)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveAll();
                        break;
                    case DialogResult.Cancel:
                        return ContinueOperation.Cancel;
                }
            }

            return ContinueOperation.Continue;
        }

        private ContinueOperation PromptForLosingClipboardData()
        {
            if (ClipboardManager.HasCutNode &&
                MessageBox.Show("You will lose data cut to clipboard. Are you sure you want to exit?",
                    "Unsaved Changes", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return ContinueOperation.Cancel;
            }
            else
            {
                return ContinueOperation.Continue;
            }
        }       

        #endregion Close Map
    }
}
