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

        public WinFormsStatusBarCtrl statusBarCtrl;
        private NoteCtrl noteCrtl;

        private ColorDialog colorDialog;
        private CustomFontDialog.FontDialog fontDialog;

        public const string APPLICATION_NAME = "Mind Mapper";

        
        public MainForm LaunchMindMate()
        {
            mainForm = new MainForm();   
            mainForm.Load += mainForm_Load;

            return mainForm;
        }

        void mainForm_Load(object sender, EventArgs e)
        {
            MapTree tree = new MapTree("Node");
            new MapNode(tree.RootNode, "Karachi", NodePosition.Left);
            new MapNode(tree.RootNode, "Lahore", NodePosition.Right);
            new MapNode(tree.RootNode, "Sind", NodePosition.Left);
                      

            mapCtrl = new MapCtrl(tree, this);

            noteCrtl = new NoteCtrl(mainForm.NoteEditor);
            noteCrtl.Register(tree);
            mainForm.NoteEditor.LostFocus += (a, b) => this.lastFocused = mainForm.NoteEditor;

            new ContextMenuCtrl(mapCtrl);
            
            new MainMenuCtrl(mainForm, mapCtrl, this);
            statusBarCtrl = new WinFormsStatusBarCtrl(mainForm.statusStrip1);
            statusBarCtrl.Register(tree);

            // moving splitter makes it the focused control, below event focuses the last control again
            mainForm.splitContainer1.SplitterMoved += (a, b) => this.lastFocused.Focus();
            mainForm.splitContainer1.Click += (a, b) => this.lastFocused.Focus();
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

        public void OpenMap()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
            if (file.ShowDialog() != DialogResult.OK)
                return;

            Debugging.Utility.StartTimeCounter("Loading Map", file.FileName);

            LoadMap(file.FileName);

            Debugging.Utility.EndTimeCounter("Loading Map");

            UpdateTitleBar();
        }

        public void LoadMap(string fileName)
        {
            noteCrtl.Unregister(this.mapCtrl.MapView.tree);
            statusBarCtrl.Unregister(this.mapCtrl.MapView.tree);

            string xmlString = System.IO.File.ReadAllText(fileName);
            MapTree tree = new MindMapSerializer().Deserialize(xmlString);

            mapCtrl.MindMateFile = fileName;
            mapCtrl.ChangeTree(tree);

            noteCrtl.Register(tree);
            statusBarCtrl.Register(tree);
            
        }

        public void SaveAsMap()
        {
            SaveFileDialog file = new SaveFileDialog();
            file.AddExtension = true;
            file.DefaultExt = "mm";
            file.Filter = "MindMap files (*.mm)|*.mm|All files (*.*)|*.*|Text (*.txt)|*.txt";
            if (file.ShowDialog() == DialogResult.OK)
            {
                SaveAsMap(file.FileName);
            }
            UpdateTitleBar();
        }

        
        public void SaveAsMap(string fileName)
        {
            var serializer = new MindMapSerializer();
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            serializer.Serialize(fileStream, this.mapCtrl.MapView.tree);
            fileStream.Close();

            mapCtrl.MindMateFile = fileName;
        }

        public void SaveMap()
        {
            if (mapCtrl.MindMateFile != "")
            {
                var serializer = new MindMapSerializer();
                FileStream file = new FileStream(mapCtrl.MindMateFile, FileMode.Create);
                serializer.Serialize(file, mapCtrl.MapView.tree);
                file.Close();
            }
            else
            {
                SaveAsMap();
            }
        }

        
        void UpdateTitleBar()
        {
            mainForm.Text = mapCtrl.MapView.tree.RootNode.Text + " - " + APPLICATION_NAME + " - " + mapCtrl.MindMateFile;
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
