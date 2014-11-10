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
    public class ContextMenuCtrl
    {
        private MapCtrl mapCtrl;
        

        public ContextMenuCtrl(MapCtrl c)
        {
            this.mapCtrl = c;
            

            mapCtrl.MapView.Canvas.contextMenu.Opening += contextMenu_Opening;

            mapCtrl.MapView.Canvas.mEditNode.Click += new EventHandler(mEditNode_Click);
            mapCtrl.MapView.Canvas.mInsertChild.Click += mInsertChild_Click;
            mapCtrl.MapView.Canvas.mDeleteNode.Click += mDeleteNode_Click;
            mapCtrl.MapView.Canvas.mSelectIcon.Click += mSelectIcon_Click;

            mapCtrl.MapView.Canvas.mSelectIcon.Image = MindMate.Properties.Resources.kalzium;
        }
        
        public void InsertMenuItems(ToolStripMenuItem [] menuItems)
        {
            ContextMenuStrip contextMenu = mapCtrl.MapView.Canvas.contextMenu;

            int index = contextMenu.Items.IndexOf(mapCtrl.MapView.Canvas.mSepPluginEnd);        
  
            contextMenu.Items.Insert(index++, new ToolStripSeparator());
            
            foreach(ToolStripMenuItem menu in menuItems)
            {
                contextMenu.Items.Insert(index++, menu);
            }
        }
        
        private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Point p = new Point(mapCtrl.MapView.Canvas.contextMenu.Left, mapCtrl.MapView.Canvas.contextMenu.Top);
            p = mapCtrl.MapView.Canvas.PointToClient(p);
            MapNode node = mapCtrl.MapView.GetMapNodeFromPoint(p);
            if (node == null) e.Cancel = true;

        }



        private void mDeleteNode_Click(object sender, EventArgs e)
        {
            mapCtrl.DeleteSelectedNodes();
        }

        private void mEditNode_Click(object sender, EventArgs e)
        {
            mapCtrl.BeginCurrentNodeEdit(TextCursorPosition.Undefined);
        }

        private void mInsertChild_Click(object sender, EventArgs e)
        {
            mapCtrl.appendChildNodeAndEdit();
        }

        void mSelectIcon_Click(object sender, EventArgs e)
        {
            mapCtrl.appendIconFromIconSelectorExt();
        }

    }
}
