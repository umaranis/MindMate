/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using System.Windows.Forms;
using System.Drawing;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// Represent view state of the tree
    /// Encapsulates NodeLinksPanel
    /// Doesn't update model, instead generates event for the controller
    /// </summary>
    public class MapView
    {

        public const int HOR_MARGIN = 20;
        public const int VER_MARGIN = 3;

        private MapTree tree;
        public MapTree Tree
        {
            get { return tree; }            
        }

        public void ChangeTree(MapTree tree)
        {
            if (this.tree != null) 
            {
                this.tree.NodePropertyChanged -= tree_NodePropertyChanged;
                this.tree.TreeStructureChanged -= tree_TreeStructureChanged;
            }
            this.tree = tree;
            this.tree.NodePropertyChanged += tree_NodePropertyChanged;
            this.tree.TreeStructureChanged += tree_TreeStructureChanged;
        }

        public MapViewPanel Canvas { get; set; }

        public SelectedNodes SelectedNodes
        {
            get
            {
                return Tree.SelectedNodes;
            }
        }        

        private MapViewTextEditor nodeTextEditor;
        public MapViewTextEditor NodeTextEditor
        {
            get
            {
                return nodeTextEditor;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        public MapView(MapTree tree)
        {
            this.ChangeTree(tree);
            this.Canvas = new MapViewPanel();
            this.Canvas.MapView = this;
            
            this.Canvas.BackColor = System.Drawing.Color.White;
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Size = new System.Drawing.Size(200, 300);
            this.Canvas.TabIndex = 0;

            this.nodeTextEditor = new MapViewTextEditor(this.Canvas, NodeView.DefaultFont);                     

        }

        //TODO: all updates to the view should handled this way (rather than relying on controller) 
        void tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            switch (e.ChangedProperty)
            {
                case NodeProperties.RichContentText:

                    node.NodeView.RefreshNoteIcon();
                    if (node == Tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                    RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);

                    Canvas.Invalidate();
                    break;              
            }
            
        }

        void tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs e)
        {
            switch(e.ChangeType)
            {
                case TreeStructureChange.Detach:
                case TreeStructureChange.Delete:
                    if (node.Parent != null && node.Parent.NodeView != null && node.Parent.NodeView.LastSelectedChild == node)
                        node.Parent.NodeView.LastSelectedChild = null; // clear LastSelectedChild in case it is deleted or detached
                    break;
            }
        }

        
        /// <summary>
        /// Refreshes or initializes node positions for the whole tree
        /// </summary>
        public void RefreshNodePositions()
        {
            NodeView nodeView = this.GetNodeView(Tree.RootNode);

            //var left = this.rootPosX;
            var left = this.Canvas.Width / 2;
            //var top = this.rootPosY;
            var top = this.Canvas.Height / 2;
            // add unit string for xhtml


            nodeView.RefreshPosition(left - (nodeView.Width / 2), top);

            RefreshChildNodePositions(Tree.RootNode, NodePosition.Undefined);

        }

        /// <summary>
        /// Refreshes or initializes node positions relative to the parent node position. 
        /// Parent node position must already be set.
        /// </summary>
        /// <param name="node">Parent Node</param>
        /// <param name="sideToRefresh">Which side to refresh (left or right). For Undefined or Root, both sides will be refreshed.</param>
        public void RefreshChildNodePositions(MapNode node, NodePosition sideToRefresh)
        {
            NodeView nView = this.GetNodeView(node);

            if (!node.HasChildren || node.Folded)
            {
                return;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    IEnumerable<MapNode> childNodes;
                    NodePosition rpos;

                    if (i == 0)
                    {
                        rpos = NodePosition.Left;
                        childNodes = node.ChildLeftNodes;
                    }
                    else
                    {
                        rpos = NodePosition.Right;
                        childNodes = node.ChildRightNodes;
                    }

                    float left = nView.Left + nView.Width + HOR_MARGIN;
                    float top = nView.Top - (int)((this.getNodeHeight(node, rpos) - nView.Height) / 2) - ((node.Pos == NodePosition.Root) ? (int)(nView.Height / 2) : 0);
                    int topOffset;
                    foreach (MapNode rnode in childNodes)
                    {
                        NodeView tView = this.GetNodeView(rnode);


                        topOffset = (int)((this.getNodeHeight(rnode, rpos) - tView.Height) / 2);
                        if (i == 0)
                        {
                            left = nView.Left - tView.Width - HOR_MARGIN;
                        }

                        tView.RefreshPosition(left, top + topOffset);

                        top += (topOffset * 2) + tView.Height + VER_MARGIN;

                        if (!rnode.Folded)
                        {
                            // recursive call
                            RefreshChildNodePositions(rnode, NodePosition.Undefined);
                        }
                    }
                }

            }
        }

        public NodeView GetNodeView(MapNode node)
        {
            if (node.NodeView == null)
            {
                node.NodeView = new NodeView(node);
            }
            return node.NodeView;
        }

        /// <summary>
        /// Get hieght of the node including child nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float getNodeHeight(MapNode node, NodePosition pos)
        {
            NodeView nView = this.GetNodeView(node);
            if (!node.HasChildren || node.Folded)
            {
                if (nView != null)
                {
                    return nView.Height;
                }                
            }

            // accumulate all children's height
            float height = 0;
            var sibCnt = 0;

            IEnumerable<MapNode> childNodes = pos == NodePosition.Left ? node.ChildLeftNodes : node.ChildRightNodes;

            foreach (MapNode cNode in childNodes)
            {
                sibCnt++;
                height += this.getNodeHeight(cNode, pos);
            }


            height += (sibCnt - 1) * VER_MARGIN;
            return (nView.Height > height ? nView.Height : height);
        }


        public System.Drawing.Bitmap DrawToBitmap()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(this.Canvas.Width, this.Canvas.Height);
            this.Canvas.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, this.Canvas.Width, this.Canvas.Height));
            return bmp;
        }

        public MapNode getNearestUnSelectedNode(MapNode node)
        {
            if (node == null)
            {
                return this.Tree.RootNode;
            }

            var parentNode = node.Parent;
            var prevNode = node.Previous;
            var nextNode = node.Next;

            while (parentNode != null && parentNode.Pos != NodePosition.Root)
            {
                if (!this.SelectedNodes.Contains(parentNode))
                {
                    parentNode = parentNode.Parent;
                    continue;
                }
                return this.getNearestUnSelectedNode(parentNode);
            }

            while (nextNode != null)
            {
                if (this.SelectedNodes.Contains(nextNode))
                {
                    nextNode = nextNode.Next;
                    continue;
                }
                return nextNode;
            }

            while (prevNode != null)
            {
                if (this.SelectedNodes.Contains(prevNode))
                {
                    prevNode = prevNode.Previous;
                    continue;
                }
                return prevNode;
            }

            return node.Parent;
        }

        
        public MapNode GetMapNodeFromPoint(System.Drawing.Point point)
        {
            MapNode node = this.Tree.RootNode;
            return GetMapNodeFromPoint(point, node);

        }

        private MapNode GetMapNodeFromPoint(System.Drawing.Point point, MapNode node)
        {
            float xdiff = 0, ydiff = 0;
            if (node.NodeView != null)
            {
                xdiff = point.X - node.NodeView.Left;
                ydiff = point.Y - node.NodeView.Top;
                if (
                    (xdiff > 0 && xdiff < node.NodeView.Width) &&
                    (ydiff > 0 && ydiff < node.NodeView.Height))
                {
                    return node;
                }

                if (!node.Folded && node.HasChildren)
                {
                    if (
                        (node.Pos == NodePosition.Right && xdiff > (node.NodeView.Width + MindMate.View.MapControls.MapView.HOR_MARGIN))
                        ||
                        (node.Pos == NodePosition.Left && xdiff < (-MindMate.View.MapControls.MapView.HOR_MARGIN))
                        )
                    {
                        foreach (var cNode in node.ChildNodes)
                        {
                            MapNode tnode = GetMapNodeFromPoint(point, cNode);
                            if (tnode != null)
                            {
                                return tnode;
                            }
                        }
                    }
                    else if (node.Pos == NodePosition.Root)
                    {
                        NodePosition posToProcess = NodePosition.Undefined;
                        if (xdiff > (node.NodeView.Width + MindMate.View.MapControls.MapView.HOR_MARGIN))
                        {
                            posToProcess = NodePosition.Right;
                        }
                        else if (xdiff < (-MindMate.View.MapControls.MapView.HOR_MARGIN))
                        {
                            posToProcess = NodePosition.Left;
                        }

                        if (posToProcess != NodePosition.Undefined)
                        {
                            foreach (var cNode in node.ChildNodes)
                            {
                                if (cNode.Pos == posToProcess)
                                {
                                    var tNode = GetMapNodeFromPoint(point, cNode);
                                    if (tNode != null)
                                        return tNode;
                                }
                            }

                        }


                    }

                }
            }
            return null;

        }

        public Point getMouseOffset(Control target, MouseEventArgs evt)
        {

            Point docPos = Canvas.Location;
            Point mousePos = Canvas.PointToScreen(evt.Location);
            return new Point(mousePos.X - docPos.X, mousePos.Y - docPos.Y);

        }




    }

        
}
