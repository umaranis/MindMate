/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.Serialization;
using MindMate.Model;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Event Handler for MapView. Handles mouse events and passes them to the Controller.
    /// </summary>
    public class MapViewMouseEventHandler
    {

        private MapCtrl mapCtrl = null;
        public MapViewMouseEventHandler(MapCtrl mapCtrl)
        {
            this.mapCtrl = mapCtrl;
        }

        /// <summary>
        /// Currently it is hooked up to mouse down event
        /// </summary>
        /// <param name="node"></param>
        /// <param name="evt"></param>
        public void MapNodeClick(MapNode node, NodeMouseEventArgs evt)
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing)
            {
                mapCtrl.EndNodeEdit();
                return;
            }

            bool shiftKeyDown = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            bool ctrlKeyDown = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            
            // Toggle folding or follow link (node has children + no key modifier)
            if (node.HasChildren && !shiftKeyDown && !ctrlKeyDown)
            {
                mapCtrl.MapView.SelectedNodes.Add(node, shiftKeyDown || ctrlKeyDown);
                if (node.Link == null || evt.NodePortion == NodePortion.Body)
                {
                    if (node.Parent != null) mapCtrl.ToggleFolded(node);
                }
                else
                {
                    mapCtrl.FollowLink(node);
                }
            }
            // deselect already selected node (ctrl key + node already selected)
            else if (mapCtrl.MapView.SelectedNodes.Count > 1 && mapCtrl.MapView.SelectedNodes.Contains(node) && ctrlKeyDown)
            {
                mapCtrl.MapView.SelectedNodes.Remove(node);
            }
            else
            {
                mapCtrl.MapView.SelectedNodes.Add(node, shiftKeyDown || ctrlKeyDown);

                //edit node or follow link (no children + only selected node + no key modifier)
                if (mapCtrl.MapView.SelectedNodes.Count == 1 && !node.HasChildren && 
                    !shiftKeyDown && !ctrlKeyDown)
                {
                    if (node.Link != null)
                    {
                        mapCtrl.FollowLink(node);
                    }
                    else if(evt.SubControlType == SubControlType.Text || node.IsEmpty())
                    {
                        mapCtrl.BeginNodeEdit(node, TextCursorPosition.End);
                    }
                }
            }

        }
        
        internal void NodeRightClick(MapNode node, NodeMouseEventArgs e)
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing)
                mapCtrl.EndNodeEdit();

            bool shiftKeyDown = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            bool ctrlKeyDown = (Control.ModifierKeys & Keys.Control) == Keys.Control;

            if (!shiftKeyDown && !ctrlKeyDown)
                mapCtrl.MapView.Tree.SelectedNodes.Add(node);
        }


        public void CanvasClick(MouseEventArgs e)
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing)
                mapCtrl.EndNodeEdit();

            MapNode lastSelectedNode = mapCtrl.MapView.SelectedNodes.Last;
            if (lastSelectedNode != null)
            {
                mapCtrl.MapView.SelectedNodes.Add(lastSelectedNode, false);
            }            
        }

        internal void MapNodeMouseMove(MapNode node, NodeMouseEventArgs evt)
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing) return;

            if (Control.ModifierKeys != Keys.None || mapCtrl.MapView.SelectedNodes.Count > 1)
            {
                return;
            }

            if (node.Link != null)
            {
                if (!node.HasChildren)
                {
                    mapCtrl.MapView.Canvas.Cursor = Cursors.Hand;
                }
                else if (node.HasChildren)
                {
                    if (evt.NodePortion == NodePortion.Head)
                        mapCtrl.MapView.Canvas.Cursor = Cursors.Hand;
                    else
                        mapCtrl.MapView.Canvas.Cursor = Cursors.Default;
                }
            }
            else
            {
                mapCtrl.MapView.Canvas.Cursor = Cursors.Default;
            }

            return;
        }

        public void MapNodeMouseHover(MapNode node, NodeMouseEventArgs evt)
        {
            if (mapCtrl.MapView.NodeTextEditor.IsTextEditing) return;

            if (Control.ModifierKeys != Keys.None || mapCtrl.MapView.SelectedNodes.Count > 1)
            {
                return;
            }

            
            mapCtrl.MapView.SelectedNodes.Add(node, false);
            
            return;
            
            
        }

        internal void NodeMouseEnter(MapNode node, MouseEventArgs e)
        {
            mapCtrl.MapView.HighlightNode(node);
        }

        public void NodeMouseExit(MapNode node, MouseEventArgs e)
        {
            if (mapCtrl.MapView.Canvas.Cursor != Cursors.Default)
            {
                mapCtrl.MapView.Canvas.Cursor = Cursors.Default;
            }

            mapCtrl.MapView.ClearHighlightedNode();
        }

        internal void NodeDragStart(MapNode node, NodeMouseEventArgs e)
        {
            if(!node.Selected)
            {
                node.Selected = true;
            }
        }

        internal void NodeDragDrop(MapTree tree, DropLocation location)
        {
            mapCtrl.MoveNodes(location);
        }
        
    }
}
