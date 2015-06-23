/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.MetaModel;
using MindMate.Serialization;
using MindMate.Model;
using MindMate.Controller;
using MindMate.View.Dialogs;

namespace MindMate.View
{
    public partial class MainForm : Form
    {
        
        private NoteEditor notesEditor;
        private TabControl sideBarTabs;        

        public MainForm()
        {
            InitializeComponent();
            SetupSideBar();
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

        
        public NoteEditor NoteEditor
        {
            get { return this.notesEditor; }
        }

        public TabControl SideBarTabs
        {
            get { return sideBarTabs; }
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
        #endregion toolbar click events (routed to main menu items)

    }
}
