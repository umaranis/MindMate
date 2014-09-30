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

namespace MindMate.Controller
{
    public class MainCtrl : IMainCtrl
    {
        private View.MainForm mainForm;
        private Control lastFocused;

        private MapCtrl mapCtrl;
        private bool unsavedChanges = true;

        public WinFormsStatusBarCtrl statusBarCtrl;
        private NoteCtrl noteCrtl;

        private ColorDialog colorDialog;
        private CustomFontDialog.FontDialog fontDialog;

        public const string APPLICATION_NAME = "Mind Mate";

        
        public MainForm LaunchMindMate()
        {
            MetaModel.MetaModel.Initialize();
            mainForm = new MainForm();   
            mainForm.Load += mainForm_Load;

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
                    tree = new MindMapSerializer().Deserialize(xmlString);
                    unsavedChanges = false;
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
            mapCtrl.MindMateFile = MetaModel.MetaModel.Instance.LastOpenedFile;

            noteCrtl = new NoteCtrl(mainForm.NoteEditor);
            noteCrtl.Register(tree);
            mainForm.NoteEditor.LostFocus += (a, b) => this.lastFocused = mainForm.NoteEditor;

            new ContextMenuCtrl(mapCtrl);
            
            new MainMenuCtrl(mainForm, mapCtrl, this);
            statusBarCtrl = new WinFormsStatusBarCtrl(mainForm.statusStrip1);
            statusBarCtrl.Register(tree);

            // moving splitter makes it the focused control, below event focuses the last control again
            mainForm.splitContainer1.GotFocus += (a, b) => this.lastFocused.Focus();

            UpdateTitleBar();
            RegisterForMapChangedNotification();

            mainForm.FormClosing += mainForm_FormClosing;
            
        }

        private MapTree CreateNewMapTree()
        {
            MapTree tree = new MapTree("Node");
            new MapNode(tree.RootNode, "Karachi", NodePosition.Left);
            new MapNode(tree.RootNode, "Lahore", NodePosition.Right);
            new MapNode(tree.RootNode, "Sind", NodePosition.Left);

            return tree;
        }

        void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(PromptForUnsavedChanges() == ContinueOperation.Cancel)
                e.Cancel = true;

            SaveLastOpenedFilePath();
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

        private void SaveLastOpenedFilePath()
        {
            if(!string.Equals(mapCtrl.MindMateFile, MetaModel.MetaModel.Instance.LastOpenedFile))
            {
                MetaModel.MetaModel.Instance.LastOpenedFile = mapCtrl.MindMateFile;
                MetaModel.MetaModel.Instance.Save();
            }
        }

        
        private void RegisterForMapChangedNotification()
        {
            mapCtrl.MapView.tree.NodePropertyChanged += (a, b) => MapChanged();
            mapCtrl.MapView.tree.TreeStructureChanged += (a, b) => MapChanged();
            mapCtrl.MapView.tree.IconChanged += (a, b) => MapChanged();
        }

        private void UnregisterForMapChangedNotification()
        {
            //TODO: check if this works with lambda expressions
            mapCtrl.MapView.tree.NodePropertyChanged -= (a, b) => MapChanged();
            mapCtrl.MapView.tree.TreeStructureChanged -= (a, b) => MapChanged();
            mapCtrl.MapView.tree.IconChanged -= (a, b) => MapChanged();
        }

        private void MapChanged()
        {
            unsavedChanges = true;
            UpdateTitleBar();
        }

        public void AddMainPanel(View.MapControls.MapViewPanel mapViewPanel)
        {
            mainForm.splitContainer1.Panel1.Controls.Add(mapViewPanel);
            mapViewPanel.LostFocus += (sender, e) => this.lastFocused = mapViewPanel;
        }

        
        public void ShowApplicationOptions()
        {
            Options frm = new Options();
            frm.ShowDialog();
        }

        public void NewMap()
        {
            if (PromptForUnsavedChanges() == ContinueOperation.Cancel)
                return;

            noteCrtl.Unregister(this.mapCtrl.MapView.tree);
            statusBarCtrl.Unregister(this.mapCtrl.MapView.tree);
            UnregisterForMapChangedNotification();

            MapTree tree = CreateNewMapTree();

            mapCtrl.MindMateFile = null;
            mapCtrl.ChangeTree(tree);
            
            noteCrtl.Register(tree);
            statusBarCtrl.Register(tree);
            RegisterForMapChangedNotification();

            unsavedChanges = true;
            UpdateTitleBar();
        }

        public void OpenMap()
        {
            if (PromptForUnsavedChanges() == ContinueOperation.Cancel)
                return;

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
            if (file.ShowDialog() != DialogResult.OK)
                return;

            Debugging.Utility.StartTimeCounter("Loading Map", file.FileName);

            LoadMap(file.FileName);

            Debugging.Utility.EndTimeCounter("Loading Map");

            unsavedChanges = false;
            UpdateTitleBar();
        }

        public void LoadMap(string fileName)
        {
            noteCrtl.Unregister(this.mapCtrl.MapView.tree);
            statusBarCtrl.Unregister(this.mapCtrl.MapView.tree);
            UnregisterForMapChangedNotification();

            string xmlString = System.IO.File.ReadAllText(fileName);
            MapTree tree = new MindMapSerializer().Deserialize(xmlString);

            mapCtrl.MindMateFile = fileName;
            mapCtrl.ChangeTree(tree);

            noteCrtl.Register(tree);
            statusBarCtrl.Register(tree);
            RegisterForMapChangedNotification();
            
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
        /// Method which actually saves the file to disk. Other methods like SaveAsMap and SaveMap use this.
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveMap(string fileName)
        {
            var serializer = new MindMapSerializer();
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            serializer.Serialize(fileStream, this.mapCtrl.MapView.tree);
            fileStream.Close();

            unsavedChanges = false;
            UpdateTitleBar();
        }

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

        
        void UpdateTitleBar()
        {
            mainForm.Text = mapCtrl.MapView.tree.RootNode.Text + " - " + APPLICATION_NAME + " - " + mapCtrl.MindMateFile;

            if(unsavedChanges) mainForm.Text += "*";
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
            if(colorDialog == null) colorDialog = new ColorDialog();
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
            if(fontDialog == null)  fontDialog = new CustomFontDialog.FontDialog();
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
    }
}
