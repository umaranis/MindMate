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
using MindMate.MetaModel;
using System.Diagnostics;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// Represent view state of the tree
    /// Encapsulates NodeLinksPanel
    /// Doesn't update model, instead generates event for the controller
    /// 
    /// Preparing MapView for drawing is a 3 step process:
    /// 1- Creating subcontrol like text, icon, image etc.
    /// 2- Setting the size of subcontrols and node
    /// 3- Setting the position of node and subcontrols (this step uses the sizes set for nodes in the previous step)
    ///    Setting the position of a node will require the height all child nodes, thats why, node size has to be determined first.
    /// </summary>
    public class MapView : Drawing.IView
    {

        public const int HOR_MARGIN = 20;
        public const int VER_MARGIN = 3;

        public const int CANVAS_DEFAULT_HEIGHT = 4096;
        public const int CANVAS_DEFAULT_WIDTH = 4096;
        public const int CANVAS_SIZE_INCREMENT = 1000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        public MapView(MapTree tree)
        {
            this.tree = tree;
            MetaModel.MetaModel.Instance.SystemIconList.ForEach(a => a.StatusChange += systemIcon_StatusChange);

            Canvas = new MapViewPanel(this);            
            
            Canvas.BackColor = tree.CanvasBackColor;
            Canvas.Location = new Point(0, 0);
            Canvas.TabIndex = 0;
            Canvas.Width = CANVAS_DEFAULT_WIDTH;
            Canvas.Height = CANVAS_DEFAULT_HEIGHT;

            RegisterTreeEvents();
            RefreshNodePositions();
            Canvas.Invalidate();

            this.nodeTextEditor = new MapViewTextEditor(this, NodeFormat.DefaultFont);
            FormatPainter = new MapViewFormatPainter(this);                 

        }

        private readonly MapTree tree;
        public MapTree Tree
        {
            get { return tree; }            
        }

        public void CenterOnForm()
        {
            //Canvas.Left = (Canvas.Parent.Width - Canvas.Width) / 2;
            //Canvas.Top = (Canvas.Parent.Height - Canvas.Height) / 2;

            //Console.WriteLine($"Canvas: {Canvas.Size} | Parent: {Canvas.Parent.Size}");
            ((EditorTabs.Tab)Canvas.Parent).ScrollToPoint((Canvas.Width - Canvas.Parent.Width) / 2, (Canvas.Height - Canvas.Parent.Height) / 2);
        }

        private void RegisterTreeEvents()
        {
            this.tree.NodePropertyChanged += tree_NodePropertyChanged;
            this.tree.TreeStructureChanged += tree_TreeStructureChanged;
            this.tree.IconChanged += tree_IconChanged;
            this.tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            this.tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;
            this.tree.TreeFormatChanged += Tree_TreeFormatChanged;
        }        

        public MapViewPanel Canvas { get; set; }

        public SelectedNodes SelectedNodes
        {
            get
            {
                return Tree.SelectedNodes;
            }
        }        

        private readonly MapViewTextEditor nodeTextEditor;
        public MapViewTextEditor NodeTextEditor
        {
            get
            {
                return nodeTextEditor;
            }
        }

        public MapViewFormatPainter FormatPainter { get; private set; }

        private MapNode highlightedNode;
        public MapNode HighlightedNode
        {
            get
            {
                return highlightedNode;
            }
        }

        public void HighlightNode(MapNode node) { highlightedNode = node; Canvas.Invalidate(); }

        public void ClearHighlightedNode() { highlightedNode = null; Canvas.Invalidate(); }

        void tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            if (node.NodeView == null) return;

            switch (e.ChangedProperty)
            {
                case NodeProperties.Text:
                    node.NodeView.RefreshText();
					if (node == tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;
                case NodeProperties.Label:
                    throw new NotImplementedException();
                case NodeProperties.NoteText:
                    node.NodeView.RefreshNoteIcon();
					if (node == Tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(node.Parent, NodePosition.Undefined);
                    break;
                case NodeProperties.Bold:
                case NodeProperties.Italic:
                case NodeProperties.Strikeout:
                    node.NodeView.RefreshFontAndFormat();
					if (node == tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;                
                case NodeProperties.Folded:
                    RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;
                case NodeProperties.FontName:
                case NodeProperties.FontSize:
                    node.NodeView.RefreshFontAndFormat();
					if (node == tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;
                case NodeProperties.Link:
                    node.NodeView.RefreshLink();
					if (node == tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(node.Parent, NodePosition.Undefined);
                    break;
                case NodeProperties.Image:
                case NodeProperties.ImageAlignment:
                case NodeProperties.ImageSize:
                    node.NodeView.RefreshImageView();
					if (node == tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;
                case NodeProperties.BackColor:
                case NodeProperties.Color:
                case NodeProperties.LineColor:
                case NodeProperties.LinePattern:
                case NodeProperties.LineWidth:
                case NodeProperties.Shape:
                    node.NodeView.RefreshFontAndFormat();
                    break;
            }

            Canvas.Invalidate();
            
        }

        void tree_IconChanged(MapNode node, IconChangedEventArgs e)
        {
            if (node.NodeView == null) return;

            switch(e.ChangeType)
            {
                case IconChange.Added:
                    node.NodeView.AddIcon(e.Icon);
                    
                    break;
                case IconChange.Removed:
                    node.NodeView.RemoveIcon(e.Icon);
                    
                    break;
            }

            if (node == tree.RootNode) RefreshNodePositions();
            else RefreshChildNodePositions(node.Parent, NodePosition.Undefined);

            Canvas.Invalidate();
        }

        void tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs e)
        {
            switch(e.ChangeType)
            {
                case TreeStructureChange.Detached:
                case TreeStructureChange.Deleted:
                    RefreshChildNodePositions(tree.RootNode, NodePosition.Undefined);
                    break;
                case TreeStructureChange.Attached:
                case TreeStructureChange.New:
                    RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;
                case TreeStructureChange.MovedLeft:
                case TreeStructureChange.MovedRight:
                case TreeStructureChange.MovedUp:
                case TreeStructureChange.MovedDown:
                    RefreshChildNodePositions(node.Parent ?? node, NodePosition.Undefined);
                    AdjustLocationToShowNodeView(node.NodeView);
                    break;
            }

            Canvas.Invalidate();
        }

        private void Tree_TreeFormatChanged(MapTree tree, TreeDefaultFormatChangedEventArgs e)
        {
            switch(e.ChangeType)
            {
                case TreeFormatChange.NodeFormat:
                    Tree.RootNode.ForEach(n => n.NodeView?.RefreshFontAndFormat());
                    RefreshNodePositions();
                    break;                
                case TreeFormatChange.NodeDropHintColor:
                case TreeFormatChange.NodeHighlightColor:
                    break;
                case TreeFormatChange.MapCanvasBackColor:
                    Canvas.BackColor = Tree.CanvasBackColor;
                    return; //no need to invalidate canvas
                case TreeFormatChange.NoteEditorBackColor: //this event is handled by NoteMapGlue 
                    return; //no need to invalidate canvas
            }

            Canvas.Invalidate();
        }

        void SelectedNodes_NodeDeselected(MapNode node, SelectedNodes selectedNodes)
        {            
            Canvas.Invalidate();
        }

        void SelectedNodes_NodeSelected(MapNode node, SelectedNodes selectedNodes)
        {
            NodeView nView = GetNodeView(node);
            AdjustLocationToShowNodeView(nView);         

            Canvas.Invalidate();
        }        

        internal void AdjustLocationToShowNodeView(NodeView nView)
        {
            Rectangle visibleRect = Canvas.GetVisibleRectangle();
            int x = 0, y = 0;

            if (visibleRect.Left > nView.Left)
            {
                x = (int)(nView.Left - visibleRect.Left - 10);
            }
            if (visibleRect.Top > nView.Top)
            {
                y = (int)(nView.Top - visibleRect.Top - 10);
            }
            int visibleRectRight = visibleRect.Right;
            int nViewRight = (int)(nView.Left + nView.Width);
            if (visibleRectRight < nViewRight)
            {
                x = (int)(nViewRight - visibleRectRight + 10);
            }
            int visibleRectBottom = visibleRect.Bottom;
            int nViewBottom = (int)(nView.Top + nView.Height);
            if (visibleRectBottom < nViewBottom)
            {
                y = (int)(nViewBottom - visibleRectBottom + 10);
            }

            (Canvas.Parent as ICanvasContainer)?.ScrollToPoint(x, y);

            Canvas.IgnoreNextMouseMove = true;
        }

        void systemIcon_StatusChange(MapNode node, ISystemIcon icon, MetaModel.SystemIconStatusChange e)
        {
            if (node.Tree != Tree) return; //event could be due to change in some other MapView
            if (node.NodeView == null) return;

            switch(e)
            {
                case SystemIconStatusChange.Show:
                    node.NodeView.AddIcon(icon);
                    break;
                case SystemIconStatusChange.Hide:
                    node.NodeView.RemoveIcon(icon);
                    break;
            }

			if (node == tree.RootNode) RefreshNodePositions();
            else RefreshChildNodePositions(node.Parent, NodePosition.Undefined);

            Canvas.Invalidate();
        }

        #region Refresh MapView

        public bool LayoutSuspended { get; private set; }

        /// <summary>
        /// Suspends recalculating NodeView positions for MapView.
        /// Useful if multiple changes are to be done in MapTree.
        /// Call <see cref="ResumeLayout"/> when changes are completed.
        /// </summary>
        public void SuspendLayout() { LayoutSuspended = true; }

        public void ResumeLayout(bool refreshLayout = false, NodePosition sideToRefresh = NodePosition.Undefined)
        {
            LayoutSuspended = false;
            if(refreshLayout)
            {
                RefreshNodePositions();
                Canvas.Invalidate();
            }
        }

        /// <summary>
        /// Refreshes or initializes node positions for the whole tree
        /// </summary>
        private void RefreshNodePositions()
        {
            if (LayoutSuspended) return;

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
        /// <param name="parent">Parent Node</param>
        /// <param name="sideToRefresh">Which side to refresh (left or right). For Undefined or Root, both sides will be refreshed.</param>
        internal void RefreshChildNodePositions(MapNode parent, NodePosition sideToRefresh)
        {
            if (LayoutSuspended) return;

            bool success = RefreshChildNodePositionsRecursive(parent, sideToRefresh);

            if(!success) //means that canvas was not big enough, therefore refresh operation was aborted
            {
                Canvas.Size = new Size(Canvas.Width + 1000, Canvas.Height + 1000);
                RefreshNodePositions();                
                (Canvas.Parent as ICanvasContainer)?.ScrollToPoint(500, 500);                
            }
        }

        /// <summary>
        /// Returns true if successfully refreshes all node positions. If canvas is not big enough, the operation is aborted and 'false' is returned.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sideToRefresh"></param>
        /// <returns></returns>
        private bool RefreshChildNodePositionsRecursive(MapNode parent, NodePosition sideToRefresh)
        {
            NodeView nView = this.GetNodeView(parent);

            if (!parent.HasChildren || parent.Folded)
            {
                if (!NodeWithinCanvas(parent, 50))
                {
                    return false;
                }
                return true;
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
                        childNodes = parent.ChildLeftNodes;
                    }
                    else
                    {
                        rpos = NodePosition.Right;
                        childNodes = parent.ChildRightNodes;
                    }

                    float left = nView.Left + nView.Width + HOR_MARGIN;
                    float top = nView.Top - (int)((this.GetNodeHeight(parent, rpos) - nView.Height) / 2) - ((parent.Pos == NodePosition.Root) ? (int)(nView.Height / 2) : 0);
                    int topOffset;
                    foreach (MapNode rnode in childNodes)
                    {
                        NodeView tView = this.GetNodeView(rnode);


                        topOffset = (int)((this.GetNodeHeight(rnode, rpos) - tView.Height) / 2);
                        if (i == 0)
                        {
                            left = nView.Left - tView.Width - HOR_MARGIN;
                        }

                        tView.RefreshPosition(left, top + topOffset);

                        top += (topOffset * 2) + tView.Height + VER_MARGIN;

                        if (!rnode.Folded)
                        {
                            // recursive call
                            bool continueProcess = RefreshChildNodePositionsRecursive(rnode, NodePosition.Undefined);
                            if (!continueProcess) return false;
                        }
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// If the point is on the margin, it is still considered outside
        /// </summary>
        /// <param name="node"></param>
        /// <param name="margin">If the point is on the margin, it is still considered outside</param>
        /// <returns></returns>
        private bool NodeWithinCanvas(MapNode node, int margin = 0)
        {
            if (node.Pos == NodePosition.Right || node.Pos == NodePosition.Root)
            {
                if(node.NodeView.Right + margin > Canvas.Width
                    || node.NodeView.Bottom + margin > Canvas.Height
                    || node.NodeView.Top - margin < 0)
                {
                    return false;
                }               
            }

            if(node.Pos == NodePosition.Left || node.Pos == NodePosition.Root)
            {
                if(node.NodeView.Left - margin < 0
                    || node.NodeView.Top - margin < 0
                    || node.NodeView.Bottom + margin > Canvas.Height)
                {
                    return false;
                }
            }
            return true;
            
        }        

        #endregion Refresh MapView

        public NodeView GetNodeView(MapNode node)
        {
            if (node.NodeView == null)
            {
                node.NodeView = new NodeView(node);
            }
            return node.NodeView;
        }

        /// <summary>
        /// Get height of the node including child nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float GetNodeHeight(MapNode node, NodePosition pos)
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
                height += this.GetNodeHeight(cNode, pos);
            }


            height += (sibCnt - 1) * VER_MARGIN;
            return (nView.Height > height ? nView.Height : height);
        }


        public System.Drawing.Bitmap DrawToBitmap()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(this.Canvas.Width, this.Canvas.Height);

            //built-in way of getting image
            //it is not working properly (shows a black rectangle) from the Ribbon project
            //works perfectly from WinXP project
            //this.Canvas.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, this.Canvas.Width, this.Canvas.Height));

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Canvas.BackColor); //set background color
                Drawing.MapPainter.DrawTree(this, g);
            }

            return bmp;
        }

        /// <summary>
        /// New Algorithm:
        /// 1- Start with root node
        ///     - check if point is within node
        ///     - if x > width + H_Margin and hasRightChildren then 
        ///         Check children on Right from top down
        ///     - if x < -H_Margin and hasLeftChildren then 
        ///         Check children on Left from bottom up
        /// 
        /// 2- Check children Right with direction=top down
        ///   -loop through children
        ///     - check if point is within node
        ///     - if x > width + H_Margin and y > node.next.top and hasChildren and !folded then 
        ///         if y is posiive then Check children on Right from top down
        ///                         else Check children on Right from bottom up
        ///     - if y < node.bottom then break loop
        /// 3- Check children Right with direction=bottom up    
        ///    -loop through children
        ///     - check if point is within node
        ///     - if x > width + H_Margin and y < node.previous.bottom and hasChildren and !folded then 
        ///         if y is posiive then Check children on Right from top down
        ///                         else Check children on Right from bottom up
        ///     - if y > node.top then break loop
        /// 4- Check children Left with direction=top down    
        ///    -loop through children
        ///     - check if point is within node
        ///     - if x < -H_Margin and y < node.next.top and hasChildren and !folded then 
        ///         if y is posiive then Check children on Left from top down
        ///                         else Check children on Left from bottom up
        ///     - if y < node.bottom then break loop
        /// 5- Check children Right with direction=bottom up    
        ///    -loop through children
        ///     - check if point is within node
        ///     - if x < -H_Margin and y < node.previous.bottom and hasChildren and !folded then 
        ///         if y is posiive then Check children on Left from top down
        ///                         else Check children on Left from bottom up
        ///     - if y > node.top then break loop
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public MapNode GetMapNodeFromPoint(System.Drawing.Point point)
        {
            return GetMapNodeFromPoint(Tree, point);
        }
        
        private static MapNode GetMapNodeFromPoint(MapTree tree, Point point)
        {
            MapNode node = tree.RootNode;
            if (node.NodeView == null) return null;
            if (node.NodeView.IsPointInsideNode(point))
            {
                return node;
            }
            else if(node.ChildRightNodes.Any() && point.X > node.NodeView.Right + MapView.HOR_MARGIN) //start from top sibling for Right nodes
            {
                return GetMapNodeFromPoint(node.GetFirstChild(NodePosition.Right), point, true);
            }
            else if (node.ChildLeftNodes.Any() && point.X < node.NodeView.Left - MapView.HOR_MARGIN)  //start from bottom sibling for Left nodes
            {
                return GetMapNodeFromPoint(node.GetLastChild(NodePosition.Left), point, false);                
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapNode">Shouldn't be root node</param>
        /// <param name="point"></param>
        /// <param name="topDown"></param>
        /// <returns></returns>
        private static MapNode GetMapNodeFromPoint(MapNode mapNode, Point point, bool topDown)
        {
            MapNode node = mapNode;
            MapNode nextNode = null;

            do
            {
                if (node.NodeView == null) return null;
                if (node.NodeView.IsPointInsideNode(point))
                {
                    return node;
                }
                else
                {
                    nextNode = topDown ? node.Next : node.Previous;
                    if (nextNode?.Pos != node.Pos) nextNode = null; //this is required for first level only (children of root)
                    if (node.HasChildren && !node.Folded)
                    {
                        if (
                            (
                              (node.Pos == NodePosition.Right && point.X > node.NodeView.Right + MapView.HOR_MARGIN) || //right node
                              (node.Pos == NodePosition.Left && point.X < node.NodeView.Left - MapView.HOR_MARGIN)      //left node
                            )
                            &&
                            (  nextNode == null ||
                               (
                                 (topDown && point.Y < nextNode.NodeView.Top) ||
                                 (!topDown && point.Y > nextNode.NodeView.Bottom)
                               )
                            )
                            )
                        {
                            bool topDownForChildren = point.Y - node.NodeView.Top < 0;
                            var result = GetMapNodeFromPoint(topDownForChildren? node.FirstChild : node.LastChild, point, topDownForChildren);
                            if (result != null) return result;
                        }
                    }
                }

                if (topDown)
                {
                    if (point.Y < node.NodeView.Bottom) break;
                }
                else
                {
                    if (point.Y > node.NodeView.Top) break;
                }

                //get next sibling
                node = nextNode;
                
            }
            while (node != null);

            return null;
        }

        public Point GetMouseOffset(Control target, MouseEventArgs evt)
        {

            Point docPos = Canvas.Location;
            Point mousePos = Canvas.PointToScreen(evt.Location);
            return new Point(mousePos.X - docPos.X, mousePos.Y - docPos.Y);

        }

    }
        
}
