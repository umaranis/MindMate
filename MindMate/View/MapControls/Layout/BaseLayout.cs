using System;
using System.Collections.Generic;
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
    public abstract class BaseLayout : ILayout
    {
        public abstract IPainter Painter { get; }
        public abstract ITraverser Traverser { get; }

        public abstract MapNode GetMapNodeFromPoint(Point point);
        public abstract float GetNodeHeight(MapNode node, NodePosition pos);
        public abstract void OnParentResize(Panel parent);
        public abstract void RefreshNodePositions();        
        public abstract void RefreshChildNodePositions(MapNode parent, NodePosition sideToRefresh);

        public abstract Size CalculateChangeInCanvasSize(Size currentSize, Size mapSize, Size parentSize);

        public BaseLayout(MapView mapView)
        {
            this.mapView = mapView;
        }

        protected MapView mapView;

        protected Control Canvas
        {
            get
            {
                return mapView.Canvas;
            }
        }

        protected MapTree Tree
        {
            get
            {
                return mapView.Tree;
            }
        }


        /// <summary>
        /// Check if the given node is visible and to be drawn on the canvas (means not hidden by folding or deleted)
        /// </summary>
        /// <returns></returns>
        public static bool IsNodeVisible(MapNode node)
        {
            if (node != null && !node.Detached)
            {
                foreach (var n in node.Ancestors)
                {
                    if (n.Folded) return false;
                }
                return true;
            }

            return false;
        }

    }

}
