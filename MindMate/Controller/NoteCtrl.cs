/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is license under MIT license (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Note Window Controlller
    /// </summary>
    class NoteCtrl
    {
        private bool isNodeWindowEmpty = true;
        private Design.Editor editor;

        public NoteCtrl(Design.Editor editor)
        {
            this.editor = editor;
            this.editor.BackColor = System.Drawing.Color.LightYellow;            
        }

        public void Register(MapTree tree)
        {
            tree.SelectedNodes.NodeSelected += MapView_nodeSelected;
            tree.SelectedNodes.NodeDeselected += MapView_nodeDeselected;
        }

        public void Unregister(MapTree tree)
        {
            tree.SelectedNodes.NodeSelected -= MapView_nodeSelected;
            tree.SelectedNodes.NodeDeselected -= MapView_nodeDeselected;
        }

        void MapView_nodeSelected(Model.MapNode node, SelectedNodes selectedNodes)
        {
            if (selectedNodes.First.RichContentType == NodeRichContentType.NOTE &&
                selectedNodes.Count == 1)
            {
                editor.Document.Body.InnerHtml = selectedNodes.First.RichContentText; 
                isNodeWindowEmpty = false;
            }
            else 
            {
                if (!isNodeWindowEmpty)
                {
                    editor.Clear();
                    isNodeWindowEmpty = true;
                }
            }
        }

        void MapView_nodeDeselected(MapNode node, SelectedNodes selectedNodes) 
        {
            if (!isNodeWindowEmpty || editor.Document.Body.InnerHtml != null) 
            {
                node.RichContentType = NodeRichContentType.NOTE;
                node.RichContentText = "<HTML>" + editor.Document.Body.OuterHtml + "</HTML>";
                isNodeWindowEmpty = false; 
            }
        }

    }
}