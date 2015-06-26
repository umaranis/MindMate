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
            

            mapCtrl.MapView.Canvas.mEditNode.Click += new EventHandler(mEditNode_Click);
            mapCtrl.MapView.Canvas.mInsertChild.Click += mInsertChild_Click;
            mapCtrl.MapView.Canvas.mDeleteNode.Click += mDeleteNode_Click;
            mapCtrl.MapView.Canvas.mSelectIcon.Click += mSelectIcon_Click;

            mapCtrl.MapView.Canvas.mSelectIcon.Image = MindMate.Properties.Resources.kalzium;

            mapCtrl.MapView.Canvas.NodeRightClick += Canvas_NodeRightClick;
            mapCtrl.MapView.Canvas.KeyDown += Canvas_KeyDown;

            mapCtrl.MapView.Canvas.contextMenu.Opening += ContextMenu_Opening;

            mapCtrl.MapView.NodeTextEditor.ContextMenu = mapCtrl.MapView.Canvas.contextMenu;

        }

        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(mapCtrl.MapView.NodeTextEditor.IsTextEditing)
            {
                mapCtrl.MapView.Canvas.mEditNode.Text = "End Editing";
            }
            else
            {
                mapCtrl.MapView.Canvas.mEditNode.Text = "Edit Node";
            }
        }

        public void InsertMenuItems(Plugins.MenuItem [] menuItems)
        {            
            ContextMenuStrip contextMenu = mapCtrl.MapView.Canvas.contextMenu;

            int index = contextMenu.Items.IndexOf(mapCtrl.MapView.Canvas.mSepPluginEnd);        
  
            contextMenu.Items.Insert(index++, new ToolStripSeparator());
            
            foreach(Plugins.MenuItem menu in menuItems)
            {
                contextMenu.Items.Insert(index++, menu.UnderlyingMenuItem);
                menu.UnderlyingMenuItem.Click += PluginMenuItem_Click;
                SetClickHandlerForSubMenu(menu);
            }
        }

        private void SetClickHandlerForSubMenu(Plugins.MenuItem menu)
        {
            foreach(ToolStripDropDownItem subMenuItem in menu.UnderlyingMenuItem.DropDownItems)
            {
                subMenuItem.Click += PluginMenuItem_Click;
                SetClickHandlerForSubMenu((Plugins.MenuItem)(subMenuItem.Tag));
            }
        }

        void PluginMenuItem_Click(object sender, EventArgs e)
        {
            Plugins.MenuItem menuItem = ((Plugins.MenuItem)((ToolStripMenuItem)sender).Tag);
            if(menuItem.Click != null)
                menuItem.Click(menuItem, this.mapCtrl.MapView.Tree.SelectedNodes);
        }

        void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Apps && mapCtrl.MapView.SelectedNodes.First != null)
            {
                NodeView nodeView = mapCtrl.MapView.GetNodeView(mapCtrl.MapView.SelectedNodes.First);
                mapCtrl.MapView.Canvas.contextMenu.Show(mapCtrl.MapView.Canvas, new Point((int)nodeView.Left + 2, (int)(nodeView.Top + nodeView.Height - 2)));                  
            }
        }

        
        /// <summary>
        /// Executes an action for each of the menu items added by Plugins.
        /// </summary>
        /// <param name="action"></param>
        public void ForEachPluginMenuItem(Action<Plugins.MenuItem> action)
        {
            ContextMenuStrip contextMenu = mapCtrl.MapView.Canvas.contextMenu;
            int index = contextMenu.Items.IndexOf(mapCtrl.MapView.Canvas.mSepPluginEnd);
            ToolStripItem menuItem = contextMenu.Items[--index];
            while (menuItem is ToolStripSeparator || (menuItem != null && menuItem.Tag != null))
            {
                if (!(menuItem is ToolStripSeparator))
                {
                    Plugins.MenuItem menuItemAdaptor = ((Plugins.MenuItem)menuItem.Tag);
                    action(menuItemAdaptor);
                }
                menuItem = contextMenu.Items[--index];
            }
        }
        
        void Canvas_NodeRightClick(MapNode node, NodeMouseEventArgs args)
        {
            mapCtrl.MapView.Canvas.contextMenu.Show(mapCtrl.MapView.Canvas, args.Location);
        }


        private void mDeleteNode_Click(object sender, EventArgs e)
        {
            mapCtrl.DeleteSelectedNodes();
        }

        private void mEditNode_Click(object sender, EventArgs e)
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing)
                mapCtrl.EndNodeEdit();
            else
                mapCtrl.BeginCurrentNodeEdit(TextCursorPosition.Undefined);
        }

        private void mInsertChild_Click(object sender, EventArgs e)
        {
            mapCtrl.appendChildNodeAndEdit();
        }

        void mSelectIcon_Click(object sender, EventArgs e)
        {
            mapCtrl.AppendIconFromIconSelectorExt();
        }


    }
}
