using MindMate.Model;
using MindMate.View.MapControls.Drawing;
using MindMate.View.MapControls.Interacting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.View.MapControls.Layout
{
    class TreeLayout : BaseLayout
    {
        public TreeLayout(MapView mapView) : base(mapView)
        {
        }

        public override IPainter Painter { get; } = new TreePainter();
        public override ITraverser Traverser { get; } = new TreeTraverser();

        private MapNode rightMostNode;

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
            float height = nView.Height;
            var sibCnt = 0;            

            foreach (MapNode cNode in node.ChildNodes)
            {
                sibCnt++;
                height += GetNodeHeight(cNode, pos);
            }


            height += sibCnt * MapView.VER_MARGIN;
            return height;
        }

        private bool CanvasResizing { get; set; } = false;

        public override void RefreshNodePositions()
        {
            NodeView nodeView = NodeView.GetNodeView(Tree.RootNode);

            var left = 10;
            var top = 10;


            nodeView.RefreshPosition(left , top);

            RefreshChildNodePositions(Tree.RootNode, NodePosition.Undefined);

        }

        public override void RefreshChildNodePositions(MapNode parent, NodePosition sideToRefresh)
        {
            if(!BaseLayout.IsNodeVisible(rightMostNode) || parent.NodeView.Right > rightMostNode.NodeView.Right)
            {
                rightMostNode = parent;
            }
            var bottomMostNode = GetBottomMostNode(parent.Tree);

            RefreshChildNodePositionsRecursive(parent);

            var mapSize = new Size((int)rightMostNode.NodeView.Right, (int)bottomMostNode.NodeView.Bottom);

            var increment = CalculateChangeInCanvasSize(Canvas.Size, mapSize, Canvas.Parent?.ClientRectangle.Size ?? Canvas.Size);

            if (!increment.IsEmpty) //means that canvas was not big enough, therefore refresh operation was aborted
            {
                CanvasResizing = true;
                Canvas.Size = new Size(Canvas.Width + increment.Width, Canvas.Height + increment.Height);
                CanvasResizing = false;                
                (Canvas.Parent as ICanvasContainer)?.ScrollToPoint(increment.Width / 2, increment.Height / 2);
            }
        }

        /// <summary>
        /// Refreshes child node positions recursively and also calculates the rightMostNode
        /// </summary>
        /// <param name="parent"></param>
        private void RefreshChildNodePositionsRecursive(MapNode parent)
        {
            NodeView parentView = NodeView.GetNodeView(parent);

            float left = parentView.Left + MapView.HOR_MARGIN;
            float top = parentView.Bottom + MapView.VER_MARGIN;
            int topOffset;

            foreach (MapNode childNode in parent.ChildNodes)
            {
                NodeView childView = NodeView.GetNodeView(childNode);

                topOffset = (int)((GetNodeHeight(childNode, NodePosition.Right) - childView.Height) / 2);

                childView.RefreshPosition(left, top);

                top += GetNodeHeight(childNode, NodePosition.Right) + MapView.VER_MARGIN;

                if (childView.Right > rightMostNode.NodeView.Right)
                {
                    rightMostNode = childNode;
                }

                if (!childNode.Folded && childNode.HasChildren)
                {
                    RefreshChildNodePositionsRecursive(childNode); // recursive call
                }
            }            
        }

        public override MapNode GetMapNodeFromPoint(Point point)
        {
            var node = mapView.Tree.SelectedNodes.Last ?? mapView.Tree.RootNode;
            if(node != null && node.NodeView.IsPointInsideNode(point))
            {
                return node;
            }
            else
            {
                Func<MapNode, MapNode> GetNextNode;
                if (node.NodeView.Top < point.Y)
                    GetNextNode = TreeTraverser.GetNodeBelow;
                else
                    GetNextNode = TreeTraverser.GetNodeAbove;

                var tmp = GetNextNode(node);
                while(tmp != null)
                {
                    if (tmp.NodeView.IsPointInsideNode(point)) return tmp;
                    tmp = GetNextNode(tmp);
                }
            }

            return null;
        }

        public override void OnParentResize(Panel parent)
        {
            if (!CanvasResizing)
            {
                if (rightMostNode.NodeView.Right < parent.ClientRectangle.Width)
                    mapView.Canvas.Width = parent.ClientRectangle.Width;
                else
                    mapView.Canvas.Width = (int)rightMostNode.NodeView.Right + 10;
            }
        }

        public Size CalculateChangeInCanvasSize(Size currentSize, Size mapSize, Size parentSize)
        {
            var result = new Size(0,0);
            if(mapSize.Width < parentSize.Width)
            {
                result.Width = parentSize.Width - currentSize.Width;
            }
            else
            {
                result.Width = mapSize.Width + 50 - currentSize.Width;
            }
            if (mapSize.Height < parentSize.Height)
            {
                result.Height = parentSize.Height - currentSize.Width;
            }
            else
            {
                result.Height = mapSize.Height + 50 - currentSize.Height;
            }
                        
            return result;  
        }

        public static MapNode GetBottomMostNode(MapTree tree)
        {
            var tmp = tree.RootNode;
            while (true)
            {
                if (!tmp.HasChildren || tmp.Folded)
                {
                    return tmp;
                }
                tmp = tmp.LastChild;
            }
        }
        
    }
}
