/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.Model;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Handle events for NodeContextMenu items
    /// </summary>
    public class ContextMenuCtrl
    {
        private MapCtrl CurrentMapCtrl { get { return mainCtrl.CurrentMapCtrl; } }

        private readonly MainCtrl mainCtrl;

        public NodeContextMenu NodeContextMenu { get; private set; }

        public ContextMenuCtrl(MainCtrl c, NodeContextMenu nodeContextMenu)
        {
            this.mainCtrl = c;
            NodeContextMenu = nodeContextMenu;

            NodeContextMenu.mEditNode.Click += mEditNode_Click;
            NodeContextMenu.mInsertChild.Click += mInsertChild_Click;
            NodeContextMenu.mDeleteNode.Click += mDeleteNode_Click;
            NodeContextMenu.mSelectIcon.Click += mSelectIcon_Click;

            NodeContextMenu.mSelectIcon.Image = MindMate.Properties.Resources.smartart_change_color_gallery_16;
            
            NodeContextMenu.Opening += ContextMenu_Opening;
        }

        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NodeContextMenu.mEditNode.Text = CurrentMapCtrl.MapView.NodeTextEditor.IsTextEditing ? "End Editing" : "Edit Text";
        }

        private void mDeleteNode_Click(object sender, EventArgs e)
        {
            CurrentMapCtrl.DeleteSelectedNodes();
        }

        private void mEditNode_Click(object sender, EventArgs e)
        {
            if (CurrentMapCtrl.MapView.NodeTextEditor.IsTextEditing)
                CurrentMapCtrl.EndNodeEdit();
            else
                CurrentMapCtrl.BeginCurrentNodeEdit(TextCursorPosition.Undefined);
        }

        private void mInsertChild_Click(object sender, EventArgs e)
        {
            CurrentMapCtrl.AppendChildNodeAndEdit();
        }

        void mSelectIcon_Click(object sender, EventArgs e)
        {
            CurrentMapCtrl.AppendIconFromIconSelectorExt();
        }


    }
}
