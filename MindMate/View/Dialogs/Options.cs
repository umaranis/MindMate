/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.Dialogs
{
    public partial class Options : Form
    {
        private MainCtrl mainCtrl;
        private NoteEditorCtrl noteCtrl;

        public Options(MainCtrl mainCtrl, NoteEditorCtrl noteCtrl)
        {
            InitializeComponent();
            this.mainCtrl = mainCtrl;
            this.noteCtrl = noteCtrl;

            lblMapEditorBackColor.BackColor = MetaModel.MetaModel.Instance.MapEditorBackColor;
            lblNoteEditorBackColor.BackColor = MetaModel.MetaModel.Instance.NoteEditorBackColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lblMapEditorBackColor.Tag != null)
            {
                MetaModel.MetaModel.Instance.MapEditorBackColor = lblMapEditorBackColor.BackColor;                
                mainCtrl.SetMapViewBackColor(lblMapEditorBackColor.BackColor);
            }
            lblMapEditorBackColor.Tag = null;


            if(lblNoteEditorBackColor.Tag != null)
            {
                MetaModel.MetaModel.Instance.NoteEditorBackColor = lblNoteEditorBackColor.BackColor;
                noteCtrl.SetNoteEditorBackColor(lblNoteEditorBackColor.BackColor);
            }
            lblNoteEditorBackColor.Tag = null;

            MetaModel.MetaModel.Instance.Save();
        }

        private void lblMapEditorBackColor_Click(object sender, EventArgs e)
        {
            lblMapEditorBackColor.BackColor = mainCtrl.Dialogs.ShowColorPicker(lblMapEditorBackColor.BackColor);
            lblMapEditorBackColor.Tag = "d";  // mark dirty
        }

        private void lblNoteEditorBackColor_Click(object sender, EventArgs e)
        {
            lblNoteEditorBackColor.BackColor = mainCtrl.Dialogs.ShowColorPicker(lblNoteEditorBackColor.BackColor);
            lblNoteEditorBackColor.Tag = "d"; // make dirty
        }
    }
}
