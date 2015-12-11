/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
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

namespace MindMate.Controller
{
    /// <summary>
    /// Controlller for:
    /// - Launching and Closing of MindMate application
    /// - Creating (New), Opening, Saving and Closing maps 
    /// - Launching Dialog boxes and coordinating other actions
    /// </summary>
    public class MainCtrl : IMainCtrl
    {
        private View.IMainForm mainForm;
        

        private Plugins.PluginManager pluginManager;

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
        private NoteCtrl noteCrtl;

        private ColorDialog colorDialog;
        private CustomFontDialog.FontDialog fontDialog;

        private TaskSchedular.TaskSchedular schedular;

        public PersistenceManager PersistenceManager
        {
            get; private set;
        }

        public const string APPLICATION_NAME = "Mind Mate";

		#region Launch MindMate application
        
        public void LaunchMindMate(IMainForm mainForm)
        {
            this.mainForm = mainForm;
            MetaModel.MetaModel.Initialize();
            schedular = new TaskSchedular.TaskSchedular();
            PersistenceManager = new PersistenceManager();            
            pluginManager = new Plugins.PluginManager(this);
            new TabController(this, mainForm);
            pluginManager.Initialize();
            mainForm.Load += mainForm_Load;
            mainForm.Shown += mainForm_AfterReady;
        }

        void mainForm_Load(object sender, EventArgs e)
        {
            MapTree tree;

            if (MetaModel.MetaModel.Instance.LastOpenedFile == null)
            {
                tree = PersistenceManager.NewTree().Tree;          
            }
            else
            {
                try
                {
                    tree = PersistenceManager.OpenTree(MetaModel.MetaModel.Instance.LastOpenedFile).Tree;
                }
                catch(Exception exp)
                {
                    tree = PersistenceManager.NewTree().Tree;
                    MetaModel.MetaModel.Instance.LastOpenedFile = null;
                    System.Diagnostics.Trace.TraceWarning(DateTime.Now.ToString() + ": Couldn't load last opened file. " + exp.Message);
                }
            }
            
            noteCrtl = new NoteCtrl(mainForm.NoteEditor);
            //TODO: Should used PersistenceManager events
            noteCrtl.MapTree = tree;            

            ContextMenuCtrl cmCtrl = new ContextMenuCtrl(CurrentMapCtrl);
            pluginManager.InitializeContextMenu(cmCtrl);
            CurrentMapCtrl.MapView.Canvas.contextMenu.Opening += 
                (s, evt) => pluginManager.OnMapNodeContextMenuOpening(CurrentMapCtrl.MapView.SelectedNodes); 

            pluginManager.InitializeSideBarWindow(mainForm.SideBarTabs);
            
            pluginManager.InitializeMainMenu(mainForm);
            statusBarCtrl = new WinFormsStatusBarCtrl(mainForm.StatusBar);
            statusBarCtrl.Register(tree); //TODO: Handle through persistence manager events

            mainForm.NoteEditor.OnDirty += (a) => {
                if(PersistenceManager.CurrentTree != null)
                {
                    PersistenceManager.CurrentTree.IsDirty = true;
                }
                }; // register for NoteEditor changes

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
            //TODO: Save changes only when a new file is saved or opened
            MetaModel.MetaModel.Instance.LastOpenedFile = PersistenceManager.CurrentTree?.FileName;
            MetaModel.MetaModel.Instance.Save();
        }

        #endregion Shutdown MindMate application

        #region Coordinating actions and dialogs

        public void ReturnFocusToMapView()
        {
            mainForm.FocusMapView();
        }

        public void ShowApplicationOptions()
        {
            Options frm = new Options(this, this.noteCrtl);
            frm.ShowDialog();
        }

        public void ExportAsBMP()
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

        public System.Drawing.Color ShowColorPicker(System.Drawing.Color currentColor)
        {
            if (colorDialog == null) colorDialog = new ColorDialog();
            if (!currentColor.IsEmpty) colorDialog.Color = currentColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                return colorDialog.Color;
            }
            else
            {
                return new System.Drawing.Color();
            }
        }

        public System.Drawing.Font ShowFontDialog(System.Drawing.Font currentFont)
        {
            if (fontDialog == null) fontDialog = new CustomFontDialog.FontDialog();
            if (currentFont != null) fontDialog.Font = currentFont;
            //fd.ShowEffects = false;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                return fontDialog.Font;
            }
            else
            {
                return null;
            }
        }

        public bool SeekDeleteConfirmation(string msg)
        {
            var result = MessageBox.Show(msg, "Delete Confirmation", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            return result == DialogResult.Yes;
        }

        public void ShowStatusNotification(string msg)
        {
            this.statusBarCtrl.SetStatusUpdate(msg);
        }

        public void ShowAboutBox()
        {
            new AboutBox().ShowDialog();
        }

        public void SetMapViewBackColor(System.Drawing.Color color)
        {
            CurrentMapCtrl.SetMapViewBackColor(color);
        }

        public void ScheduleTask(TaskSchedular.ITask task)
        {
            schedular.AddTask(task);
        }

        public void RescheduleTask(TaskSchedular.ITask task, DateTime startTime)
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

        #endregion Coordinating actions and dialogs

        #region New / Open Map

        /// <summary>
        /// Create a new Mind Map
        /// </summary>
        public void NewMap()
        {
            if (PromptForUnsavedChanges() == ContinueOperation.Cancel)
                return;

            CloseMap();

            MapTree tree = PersistenceManager.NewTree().Tree;

            //TODO: Handle through persistence manager
            noteCrtl.MapTree = tree;
            statusBarCtrl.Register(tree);
        }

        public void OpenMap(string fileName = null)
        {
            if (PromptForUnsavedChanges() == ContinueOperation.Cancel)
                return;

            if (fileName == null)
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
                if (file.ShowDialog() != DialogResult.OK)
                    return;
                else
                    fileName = file.FileName;
            }

            Debugging.Utility.StartTimeCounter("Loading Map", fileName);

            MapTree tree;
            try
            {
                tree = PersistenceManager.OpenTree(fileName).Tree;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debugging.Utility.EndTimeCounter("Loading Map");
                return;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("File not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debugging.Utility.EndTimeCounter("Loading Map");
                return;
            }

            CloseMap();

            //TODO: Handle through persistence manager events
            noteCrtl.MapTree = tree;
            statusBarCtrl.Register(tree);

            Debugging.Utility.EndTimeCounter("Loading Map");

            MetaModel.MetaModel.Instance.RecentFiles.Add(fileName);
            mainForm.RefreshRecentFilesMenuItems();
        }        

        #endregion New / Open Map

        #region Save Map

        public void SaveMap()
        {
            SaveMap(PersistenceManager.CurrentTree);            
        }

        public void SaveMap(PersistentTree tree)
        {
            if (tree != null)
            {
                if (tree.IsNewMap)
                {
                    SaveCurrentMapAs();
                }
                else
                {
                    SaveMapInternal();
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
            file.FileName = tree.IsNewMap? CurrentMapCtrl.MapView.Tree.RootNode.Text : PersistenceManager.CurrentTree.FileName;
            if (file.ShowDialog() == DialogResult.OK)
            {
                SaveMapInternal(file.FileName);
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
        /// Method which actually saves the file to disk. Other methods like SaveAsMap and SaveMap invoke this.
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveMapInternal(string fileName = null)
        {
            noteCrtl.UpdateNodeFromEditor();

            if (PersistenceManager.CurrentTree != null)
            {
                if (fileName == null)
                {
                    PersistenceManager.CurrentTree.Save();
                }
                else
                {
                    PersistenceManager.CurrentTree.Save(fileName);
                }
                
                MetaModel.MetaModel.Instance.RecentFiles.Add(fileName);
                mainForm.RefreshRecentFilesMenuItems();
            }
        }

        #endregion Save Map

        #region Close Map

        private void CloseMap()
        {
            //TODO: Should be managed through PersistenceManager events
            statusBarCtrl.Unregister(this.CurrentMapCtrl.MapView.Tree);
        }

        private enum ContinueOperation { Continue, Cancel };

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
