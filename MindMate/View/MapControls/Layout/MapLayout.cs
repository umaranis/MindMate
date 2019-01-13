using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MindMate.Model;
using MindMate.View.MapControls.Drawing;
using MindMate.View.MapControls.Interacting;

namespace MindMate.View.MapControls.Layout
{
    class MapLayout : BaseLayout
    {

        public MapLayout(MapView mapView) : base(mapView)
        {
        }

        public override IPainter Painter { get; } = new MapPainter();

        public override ITraverser Traverser { get; } = new MapTraverser();

        private MapNode rightMostNode;
        private MapNode leftMostNode;        

        
        /// <summary>
        /// Get height of the node including child nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override float GetNodeHeight(MapNode node, NodePosition pos)
        {
            NodeView nView = NodeView.GetNodeView(node);
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
                height += GetNodeHeight(cNode, pos);
            }


            height += (sibCnt - 1) * MapView.VER_MARGIN;
            return (nView.Height > height ? nView.Height : height);
        }

        public override void RefreshNodePositions()
        {
            NodeView nodeView = NodeView.GetNodeView(Tree.RootNode);

            var left = this.Canvas.Width / 2;
            var top = this.Canvas.Height / 2;

            nodeView.RefreshPosition(left - (nodeView.Width / 2), top);

            RefreshChildNodePositions(Tree.RootNode, NodePosition.Undefined);

        }

        public override void RefreshChildNodePositions(MapNode parent, NodePosition sideToRefresh)
        {
            if (!BaseLayout.IsNodeVisible(rightMostNode) || parent.NodeView.Right > rightMostNode.NodeView.Right)
            {
                rightMostNode = parent;
            }
            if(!BaseLayout.IsNodeVisible(leftMostNode) || parent.NodeView.Left < leftMostNode.NodeView.Left)
            {
                leftMostNode = parent;
            }            

            if(parent.Pos == NodePosition.Right || parent.Pos == NodePosition.Root)
                RefreshChildNodePositionsRight(parent);

            if (parent.Pos == NodePosition.Left || parent.Pos == NodePosition.Root)
                RefreshChildNodePositionsLeft(parent);

            var bottomMostNode = GetBottomMostNode(parent.Tree);
            var topMostNode = GetTopMostNode(parent.Tree);

            var mapSize = new Size((int)(rightMostNode.NodeView.Right + (leftMostNode.NodeView.Left < 0? -leftMostNode.NodeView.Left : 0)), 
                             (int)(bottomMostNode.NodeView.Bottom + (topMostNode.NodeView.Top < 0? -topMostNode.NodeView.Top : 0)));

            var increment = CalculateChangeInCanvasSize(Canvas.Size, mapSize, Canvas.Parent?.Size ?? Canvas.Size);

            if (!increment.IsEmpty) //means that canvas was not big enough, therefore refresh operation was aborted
            {
                Canvas.Size = new Size(Canvas.Width + increment.Width, Canvas.Height + increment.Height);
                RefreshNodePositions();
                (Canvas.Parent as ICanvasContainer)?.ScrollToPoint(increment.Width / 2, increment.Height / 2);
            }
        }        

        private void RefreshChildNodePositionsRight(MapNode parent)
        {
            NodeView parentView = NodeView.GetNodeView(parent);

            float left = parentView.Left + parentView.Width + MapView.HOR_MARGIN; ;
            float top = parentView.Top - (int)((GetNodeHeight(parentView.Node, NodePosition.Right) - parentView.Height) / 2) - ((parentView.Node.Pos == NodePosition.Root) ? (int)(parentView.Height / 2) : 0);
            int topOffset;
            foreach (MapNode childNode in parent.ChildRightNodes)
            {
                NodeView childView = NodeView.GetNodeView(childNode);

                topOffset = (int)((GetNodeHeight(childNode, NodePosition.Right) - childView.Height) / 2);
                
                childView.RefreshPosition(left, top + topOffset);

                top += (topOffset * 2) + childView.Height + MapView.VER_MARGIN;

                if(childView.Right > rightMostNode.NodeView.Right)
                {
                    rightMostNode = childNode;
                }

                if (!childNode.Folded && childNode.HasChildren)
                {
                    // recursive call
                    RefreshChildNodePositionsRight(childNode);
                }
            }
        }

        private void RefreshChildNodePositionsLeft(MapNode parent)
        {
            NodeView parentView = NodeView.GetNodeView(parent);            

            float left = parentView.Left + parentView.Width + MapView.HOR_MARGIN; ;
            float top = parentView.Top - (int)((GetNodeHeight(parentView.Node, NodePosition.Left) - parentView.Height) / 2) - ((parentView.Node.Pos == NodePosition.Root) ? (int)(parentView.Height / 2) : 0);
            int topOffset;
            foreach (MapNode childNode in parent.ChildLeftNodes)
            {
                NodeView childView = NodeView.GetNodeView(childNode);


                topOffset = (int)((GetNodeHeight(childNode, NodePosition.Left) - childView.Height) / 2);
                left = parentView.Left - childView.Width - MapView.HOR_MARGIN;                    

                childView.RefreshPosition(left, top + topOffset);

                top += (topOffset * 2) + childView.Height + MapView.VER_MARGIN;

                if(childView.Left < leftMostNode.NodeView.Left)
                {
                    leftMostNode = childNode;
                }

                if (!childNode.Folded && childNode.HasChildren)
                {
                    // recursive call
                    RefreshChildNodePositionsLeft(childNode);
                }
            }
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
        public override MapNode GetMapNodeFromPoint(System.Drawing.Point point)
        {
            return GetMapNodeFromPoint(mapView.Tree, point);
        }

        private static MapNode GetMapNodeFromPoint(MapTree tree, Point point)
        {
            MapNode node = tree.RootNode;
            if (node.NodeView == null) return null;
            if (node.NodeView.IsPointInsideNode(point))
            {
                return node;
            }
            else if (node.ChildRightNodes.Any() && point.X > node.NodeView.Right + MapView.HOR_MARGIN) //start from top sibling for Right nodes
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
                            (nextNode == null ||
                               (
                                 (topDown && point.Y < nextNode.NodeView.Top) ||
                                 (!topDown && point.Y > nextNode.NodeView.Bottom)
                               )
                            )
                            )
                        {
                            bool topDownForChildren = point.Y - node.NodeView.Top < 0;
                            var result = GetMapNodeFromPoint(topDownForChildren ? node.FirstChild : node.LastChild, point, topDownForChildren);
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

        public override void OnParentResize(Panel parent)
        {
        }

        public override Size CalculateChangeInCanvasSize(Size currentSize, Size mapSize, Size parentSize)
        {
            var mapSizeWithMargin = new Size(mapSize.Width + 50, mapSize.Height + 50);
            if(currentSize.Width < mapSizeWithMargin.Width || currentSize.Height < mapSizeWithMargin.Height)
            {
                int increment = (int)Math.Ceiling((double)
                    Math.Max(mapSizeWithMargin.Width - currentSize.Width, mapSizeWithMargin.Height - currentSize.Height) 
                    / 1000) * 1000;
                return new Size(increment, increment);
            }

            return Size.Empty;
        }

        public static MapNode GetBottomMostNode(MapTree tree)
        {
            MapNode rightBottom = tree.RootNode.GetLastChild(NodePosition.Right);
            if (rightBottom != null)
            {
                rightBottom = GetBottomMostNode(rightBottom);
            }
            else
            {
                rightBottom = tree.RootNode;
            }

            MapNode leftBottom = tree.RootNode.GetLastChild(NodePosition.Left);
            if (leftBottom != null)
            {
                leftBottom = GetBottomMostNode(leftBottom);
            }
            else
            {
                leftBottom = tree.RootNode;
            }

            if (NodeView.GetNodeView(rightBottom).Bottom > NodeView.GetNodeView(leftBottom).Bottom)
                return rightBottom;
            else
                return leftBottom;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">Must NOT be root node or null</param>
        /// <returns></returns>
        public static MapNode GetBottomMostNode(MapNode parent)
        {
            Debug.Assert(parent.Pos != NodePosition.Root || parent.Pos != NodePosition.Undefined);

            var tmp = parent;
            while (true)
            {
                if (!tmp.HasChildren || tmp.Folded)
                {
                    return tmp;
                }
                tmp = tmp.LastChild;
            }
        }

        public static MapNode GetTopMostNode(MapTree tree)
        {
            MapNode rightTop = tree.RootNode.GetFirstChild(NodePosition.Right);
            if (rightTop != null)
            {
                rightTop = GetTopMostNode(rightTop);
            }
            else
            {
                rightTop = tree.RootNode;
            }

            MapNode leftTop = tree.RootNode.GetFirstChild(NodePosition.Left);
            if (leftTop != null)
            {
                leftTop = GetTopMostNode(leftTop);
            }
            else
            {
                leftTop = tree.RootNode;
            }

            if (NodeView.GetNodeView(rightTop).Top < NodeView.GetNodeView(leftTop).Top)
                return rightTop;
            else
                return leftTop;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">Must NOT be root node or null</param>
        /// <returns></returns>
        public static MapNode GetTopMostNode(MapNode parent)
        {
            Debug.Assert(parent.Pos != NodePosition.Root || parent.Pos != NodePosition.Undefined);

            var tmp = parent;
            while (true)
            {
                if (!tmp.HasChildren || tmp.Folded)
                {
                    return tmp;
                }
                tmp = tmp.FirstChild;
            }
        }
    }
}
