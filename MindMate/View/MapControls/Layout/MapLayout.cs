using System;
using System.Collections.Generic;
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

        
    }
}
