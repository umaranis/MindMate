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
    class ContextMenuCtrl
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
        }

        void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Apps && mapCtrl.MapView.SelectedNodes.First != null)
            {
                NodeView nodeView = mapCtrl.MapView.GetNodeView(mapCtrl.MapView.SelectedNodes.First);
                mapCtrl.MapView.Canvas.contextMenu.Show(mapCtrl.MapView.Canvas, new Point((int)nodeView.Left + 2, (int)(nodeView.Top + nodeView.Height - 2)));                  
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
