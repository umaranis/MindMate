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
        
        public NoteEditor notesEditor;

        public MainForm()
        {
            InitializeComponent();

            //mainUICtrl = new MainCtrl(this);

            
            notesEditor = new NoteEditor();
            notesEditor.Dock = DockStyle.Fill;
            this.splitContainer1.Panel2.Controls.Add(notesEditor);
        }

        
        public NoteEditor NoteEditor
        {
            get { return this.notesEditor; }
        }                
    }
}
