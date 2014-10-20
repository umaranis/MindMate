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

        public Options(MainCtrl ctrl)
        {
            InitializeComponent();
            mainCtrl = ctrl;

            lblMapEditorBackColor.BackColor = MetaModel.MetaModel.Instance.MapEditorBackColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lblMapEditorBackColor.Tag != null)
            {
                MetaModel.MetaModel.Instance.MapEditorBackColor = lblMapEditorBackColor.BackColor;
                MetaModel.MetaModel.Instance.Save();

                mainCtrl.SetMapViewBackColor(lblMapEditorBackColor.BackColor);
            }

            lblMapEditorBackColor.Tag = null;
        }

        private void lblMapEditorBackColor_Click(object sender, EventArgs e)
        {
            lblMapEditorBackColor.BackColor = mainCtrl.ShowColorPicker(lblMapEditorBackColor.BackColor);
            lblMapEditorBackColor.Tag = "d";  // mark dirty
        }
    }
}
