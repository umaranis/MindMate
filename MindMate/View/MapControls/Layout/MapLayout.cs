using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using MindMate.View.MapControls.Drawing;
using MindMate.View.MapControls.Interacting;

namespace MindMate.View.MapControls.Layout
{
    class MapLayout : ILayout
    {

        public MapLayout(MapView mapView)
        {
            this.mapView = mapView;
        }

        private MapView mapView;

        public IPainter Painter { get; } = new MapPainter();
        public ITraverser Traverser { get; } = new MapTraverser();

        /// <summary>
        /// Get height of the node including child nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float GetNodeHeight(MapNode node, NodePosition pos)
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

        /// <summary>
        /// Returns true if successfully refreshes all node positions. If canvas is not big enough, the operation is aborted and 'false' is returned.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sideToRefresh"></param>
        /// <returns></returns>
        public bool RefreshChildNodePositions(MapNode parent, NodePosition sideToRefresh)
        {
            NodeView nView = NodeView.GetNodeView(parent);

            if (!parent.HasChildren || parent.Folded)
            {
                if (!mapView.NodeWithinCanvas(parent, 50))
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

                    float left = nView.Left + nView.Width + MapView.HOR_MARGIN; ;
                    float top = nView.Top - (int)((GetNodeHeight(nView.Node, rpos) - nView.Height) / 2) - ((nView.Node.Pos == NodePosition.Root) ? (int)(nView.Height / 2) : 0);
                    int topOffset;
                    foreach (MapNode rnode in childNodes)
                    {
                        NodeView tView = NodeView.GetNodeView(rnode);


                        topOffset = (int)((GetNodeHeight(rnode, rpos) - tView.Height) / 2);
                        if (i == 0)
                        {
                            left = nView.Left - tView.Width - MapView.HOR_MARGIN;
                        }

                        tView.RefreshPosition(left, top + topOffset);

                        top += (topOffset * 2) + tView.Height + MapView.VER_MARGIN;

                        if (!rnode.Folded)
                        {
                            // recursive call
                            bool continueProcess = RefreshChildNodePositions(rnode, NodePosition.Undefined);
                            if (!continueProcess) return false;
                        }
                    }
                }

            }
            return true;
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


    }
}
