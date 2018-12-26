using MindMate.Model;
using MindMate.View.MapControls.Drawing;
using MindMate.View.MapControls.Interacting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls.Layout
{
    class TreeLayout : ILayout
    {
        public TreeLayout(MapView mapView)
        {
            this.mapView = mapView;
        }

        private MapView mapView;

        public IPainter Painter { get; } = new TreePainter();
        public ITraverser Traverser { get; } = new TreeTraverser();

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
            float height = nView.Height;
            var sibCnt = 0;            

            foreach (MapNode cNode in node.ChildNodes)
            {
                sibCnt++;
                height += GetNodeHeight(cNode, pos);
            }


            height += (sibCnt - 1) * MapView.VER_MARGIN;
            return height;
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

                float left = nView.Left + MapView.HOR_MARGIN;
                float top = nView.Bottom + MapView.VER_MARGIN;
                int topOffset;
                foreach (MapNode rnode in parent.ChildNodes)
                {
                    NodeView tView = NodeView.GetNodeView(rnode);

                    topOffset = (int)((GetNodeHeight(rnode, NodePosition.Right) - tView.Height) / 2);

                    tView.RefreshPosition(left, top);

                    top += GetNodeHeight(rnode, NodePosition.Right) + MapView.VER_MARGIN;

                    if (!rnode.Folded)
                    {
                        // recursive call
                        bool continueProcess = RefreshChildNodePositions(rnode, NodePosition.Undefined);
                        if (!continueProcess) return false;
                    }
                }


            }
            return true;
        }

        public MapNode GetMapNodeFromPoint(Point point)
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
    }
}
