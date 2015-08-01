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
        private View.MainForm mainForm;
        

        private Plugins.PluginManager pluginManager;

        private MapCtrl mapCtrl;

        private ChangeManager changeManager;
        internal ChangeManager ChangeManager { get { return changeManager; } }

        private MainMenuCtrl mainMenuCtrl;
        private bool unsavedChanges = false;

        public WinFormsStatusBarCtrl statusBarCtrl;
        private NoteCtrl noteCrtl;

        private ColorDialog colorDialog;
        private CustomFontDialog.FontDialog fontDialog;

        private TaskSchedular.TaskSchedular schedular;

        public const string APPLICATION_NAME = "Mind Mate";

		#region Launch MindMate application
        
        public MainForm LaunchMindMate()
        {
            MetaModel.MetaModel.Initialize();
            schedular = new TaskSchedular.TaskSchedular();
            mainForm = new MainForm();
            pluginManager = new Plugins.PluginManager(this);
            pluginManager.Initialize();
            mainForm.Load += mainForm_Load;
            mainForm.Shown += mainForm_AfterReady;

            return mainForm;
        }

        void mainForm_Load(object sender, EventArgs e)
        {
            MapTree tree;

            if (MetaModel.MetaModel.Instance.LastOpenedFile == null)
            {
                tree = CreateNewMapTree();          
            }
            else
            {
                try
                {
                    string xmlString = System.IO.File.ReadAllText(MetaModel.MetaModel.Instance.LastOpenedFile);
                    tree = CreateEmptyTree();
                    new MindMapSerializer().Deserialize(xmlString, tree);
                }
                catch(Exception exp)
                {
                    tree = CreateNewMapTree();
                    MetaModel.MetaModel.Instance.LastOpenedFile = null;
                    System.Diagnostics.Trace.TraceWarning(DateTime.Now.ToString() + ": Couldn't load last opened file. " + exp.Message);
                }
            }
            tree.SelectedNodes.Add(tree.RootNode);

            mapCtrl = new MapCtrl(tree, this);
            changeManager = new ChangeManager();
            changeManager.RegisterMap(tree);
            mainForm.AddMainView(mapCtrl.MapView.Canvas);            
            mapCtrl.MindMateFile = MetaModel.MetaModel.Instance.LastOpenedFile;

            noteCrtl = new NoteCtrl(mainForm.NoteEditor);
            noteCrtl.MapTree = tree;            

            ContextMenuCtrl cmCtrl = new ContextMenuCtrl(mapCtrl);
            pluginManager.InitializeContextMenu(cmCtrl);
            mapCtrl.MapView.Canvas.contextMenu.Opening += 
                (s, evt) => pluginManager.OnMapNodeContextMenuOpening(mapCtrl.MapView.SelectedNodes); 

            pluginManager.InitializeSideBarWindow(mainForm.SideBarTabs);
            
            mainMenuCtrl = new MainMenuCtrl(mainForm, mapCtrl, this);
            pluginManager.InitializeMainMenu(mainMenuCtrl);
            statusBarCtrl = new WinFormsStatusBarCtrl(mainForm.statusStrip1);
            statusBarCtrl.Register(tree);

            UpdateTitleBar();
            RegisterForMapChangedNotification();                 // register for map changes (register/unregister with tree)
            mainForm.NoteEditor.OnDirty += (a) => MapChanged(); // register for NoteEditor changes

            mainForm.FormClosing += mainForm_FormClosing;

        }
                
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
            MetaModel.MetaModel.Instance.LastOpenedFile = mapCtrl.MindMateFile;
            MetaModel.MetaModel.Instance.Save();
        }

        #endregion Shutdown MindMate application

        #region Coordinating actions and dialogs
        
        private void MapChanged()
        {
            unsavedChanges = true;
            UpdateTitleBar();
        }

        public void ReturnFocusToMapView()
        {
            mainForm.FocusMapView();
        }

        public void ShowApplicationOptions()
        {
            Options frm = new Options(this, this.noteCrtl);
            frm.ShowDialog();
        }

        void UpdateTitleBar()
        {
            mainForm.Text = mapCtrl.MapView.Tree.RootNode.Text + " - " + APPLICATION_NAME + " - " + mapCtrl.MindMateFile;

            if (unsavedChanges) mainForm.Text += "*";
        }

        public void ExportAsBMP()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "bmp";
            file.Filter = "Bitmap Image (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                using (var bmp = mapCtrl.MapView.DrawToBitmap())
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
            mapCtrl.SetMapViewBackColor(color);
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
                mapCtrl.Copy();
        }

        public void Cut()
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.Cut();
            else
                mapCtrl.Cut();
        }        

        public void Paste()
        {
            if (mainForm.IsNoteEditorActive)
                mainForm.NoteEditor.Paste();
            else
                mapCtrl.Paste();
        }

        public void Undo()
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing)
                mapCtrl.MapView.NodeTextEditor.Undo();
            else
                ChangeManager.Undo();
        }

        public void Redo()
        {
            if (!mapCtrl.MapView.NodeTextEditor.IsTextEditing)
                ChangeManager.Redo();
        }

        #endregion Coordinating actions and dialogs

        #region New / Open Map

        /// <summary>
        /// Creates an empty MapTree which could be used to (1) deserialize tree or (2) create a default new map      
        /// </summary>
        /// <returns></returns>
        private MapTree CreateEmptyTree()
        {
            MapTree tree = new MapTree();
            pluginManager.OnTreeCreating(tree);
            return tree;
        }

        /// <summary>
        /// Creates a new MapTree with default node
        /// </summary>
        /// <returns></returns>
        private MapTree CreateNewMapTree()
        {
            MapTree tree = CreateEmptyTree();
            tree.RootNode = new MapNode(tree, "New Map");

            return tree;
        }

        /// <summary>
        /// Create a new Mind Map
        /// </summary>
        public void NewMap()
        {
            if (PromptForUnsavedChanges() == ContinueOperation.Cancel)
                return;

            CloseMap();

            MapTree tree = CreateNewMapTree();

            mapCtrl.MindMateFile = null;
            mapCtrl.ChangeTree(tree);
            changeManager.RegisterMap(tree);

            noteCrtl.MapTree = tree;
            statusBarCtrl.Register(tree);
            RegisterForMapChangedNotification();

            unsavedChanges = false;
            UpdateTitleBar();
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

            string xmlString;
            try
            {
                xmlString = System.IO.File.ReadAllText(fileName);
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

            MapTree tree = CreateEmptyTree();
            new MindMapSerializer().Deserialize(xmlString, tree);

            CloseMap();

            mapCtrl.MindMateFile = fileName;
            mapCtrl.ChangeTree(tree);
            changeManager.RegisterMap(tree);

            noteCrtl.MapTree = tree;
            statusBarCtrl.Register(tree);
            RegisterForMapChangedNotification();

            Debugging.Utility.EndTimeCounter("Loading Map");

            unsavedChanges = false;
            UpdateTitleBar();
            MetaModel.MetaModel.Instance.RecentFiles.Add(fileName);
            mainMenuCtrl.RefreshRecentFilesMenuItems();
        }

        private void RegisterForMapChangedNotification()
        {
            mapCtrl.MapView.Tree.NodePropertyChanged += (a, b) => MapChanged();
            mapCtrl.MapView.Tree.TreeStructureChanged += (a, b) => MapChanged();
            mapCtrl.MapView.Tree.IconChanged += (a, b) => MapChanged();
            mapCtrl.MapView.Tree.AttributeChanged += (a, b) => MapChanged();
        }

        #endregion New / Open Map

        #region Save Map

        public void SaveMap()
        {
            if (mapCtrl.MindMateFile != null)
            {
                SaveMap(mapCtrl.MindMateFile);
            }
            else
            {
                SaveAsMap();
            }
        }

        public void SaveAsMap()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "mm";
            file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
            file.FileName = mapCtrl.MindMateFile;
            if (file.ShowDialog() == DialogResult.OK)
            {
                mapCtrl.MindMateFile = file.FileName;
                SaveMap(mapCtrl.MindMateFile);
            }
        }

        /// <summary>
        /// Method which actually saves the file to disk. Other methods like SaveAsMap and SaveMap invoke this.
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveMap(string fileName)
        {
            noteCrtl.UpdateNodeFromEditor();

            var serializer = new MindMapSerializer();
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            serializer.Serialize(fileStream, this.mapCtrl.MapView.Tree);
            fileStream.Close();

            unsavedChanges = false;
            UpdateTitleBar();
            MetaModel.MetaModel.Instance.RecentFiles.Add(fileName);
            mainMenuCtrl.RefreshRecentFilesMenuItems();
        }

        #endregion Save Map

        #region Close Map

        private void CloseMap()
        {
            changeManager.Unregister(this.mapCtrl.MapView.Tree);
            pluginManager.OnTreeDeleting(this.mapCtrl.MapView.Tree);
            statusBarCtrl.Unregister(this.mapCtrl.MapView.Tree);
            UnregisterForMapChangedNotification();
        }

        private enum ContinueOperation { Continue, Cancel };

        private ContinueOperation PromptForUnsavedChanges()
        {
            if (unsavedChanges)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes?", "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (result)
                {
                    case DialogResult.Yes:
                        SaveMap();
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

        private void UnregisterForMapChangedNotification()
        {
            //TODO: check if this works with lambda expressions
            mapCtrl.MapView.Tree.NodePropertyChanged -= (a, b) => MapChanged();
            mapCtrl.MapView.Tree.TreeStructureChanged -= (a, b) => MapChanged();
            mapCtrl.MapView.Tree.IconChanged -= (a, b) => MapChanged();
            mapCtrl.MapView.Tree.AttributeChanged -= (a, b) => MapChanged();
        }

        #endregion Close Map
    }
}
