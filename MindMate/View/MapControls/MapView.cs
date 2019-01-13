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
using MindMate.View.MapControls.Layout;
using MindMate.View.MapControls.Drawing;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// Represent view state of the tree
    /// Encapsulates NodeLinksPanel
    /// Doesn't update model, instead generates event for the controller
    /// 
    /// Preparing MapView for drawing is a 4 step process:
    /// 1- Creating subcontrol like text, icon, image etc.
    /// 2- Setting the size of subcontrols and node
    /// 3- Setting the position of node and subcontrols (this step uses the sizes set for nodes in the previous step)
    ///    Setting the position of a node will require the height all child nodes, thats why, node size has to be determined first.
    /// 4- Actual drawing in the paint event.
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
            
            Canvas.TabIndex = 0;
            Canvas.BackColor = System.Drawing.Color.White;
            Canvas.Location = new System.Drawing.Point(0, 0);            
            Canvas.Width = CANVAS_DEFAULT_WIDTH;
            Canvas.Height = CANVAS_DEFAULT_HEIGHT;

            RegisterTreeEvents();
            ChangeViewLayout(tree.ViewLayout);
            RefreshNodePositions();
            Canvas.Invalidate();

            this.nodeTextEditor = new MapViewTextEditor(this, NodeView.DefaultFont);
            FormatPainter = new MapViewFormatPainter(this);

        }

                
        private readonly MapTree tree;
        public MapTree Tree
        {
            get { return tree; }            
        }

        public ILayout Layout { get; private set; }
        public IPainter Painter => Layout.Painter;

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
                    node.NodeView.RefreshFont();
					if (node == tree.RootNode) RefreshNodePositions();
                    else RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;                
                case NodeProperties.Folded:
                    RefreshChildNodePositions(tree.RootNode, node.Pos);
                    break;
                case NodeProperties.FontName:
                case NodeProperties.FontSize:
                    node.NodeView.RefreshFont();
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

            Layout.RefreshNodePositions();

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

            Layout.RefreshChildNodePositions(parent, sideToRefresh);
        }        

        /// <summary>
        /// If the point is on the margin, it is still considered outside
        /// </summary>
        /// <param name="node"></param>
        /// <param name="margin">If the point is on the margin, it is still considered outside</param>
        /// <returns></returns>
        public bool NodeWithinCanvas(MapNode node, int margin = 0)
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

        public MapNode GetMapNodeFromPoint(System.Drawing.Point point)
        {
            return Layout.GetMapNodeFromPoint(point);
        }

        public NodeView GetNodeView(MapNode node)
        {
            return NodeView.GetNodeView(node);
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
                Layout.Painter.DrawTree(this, g);
            }

            return bmp;
        }        

        public void ChangeViewLayout(ViewLayout viewLayout)
        {
            Tree.ViewLayout = viewLayout;
            switch (viewLayout)
            {
                case ViewLayout.MindMap:
                    Layout = new MapLayout(this);
                    break;
                case ViewLayout.Tree:
                    Layout = new TreeLayout(this);
                    break;
            }
            RefreshNodePositions();
            Canvas.Invalidate();            
            if(Canvas.Parent != null)   AdjustLocationToShowNodeView(Tree.SelectedNodes.Last.NodeView);
            
        }
    }
        
}
