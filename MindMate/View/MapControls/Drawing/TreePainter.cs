using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls.Drawing
{
    public class TreePainter : BasePainter
    {
        protected override void DrawNodeLinker(NodeView nodeView, Graphics g, Pen p)
        {
            MapNode node = nodeView.Node;
            NodeView parentView = NodeView.GetNodeView(node.Parent);
            if (nodeView == null || parentView == null) return;

            float pos1X, pos1Y, // connector start point on parent node
                pos2X, pos2Y;   // connector end point on current/child node

            float control1X, control1Y, control2X, control2Y;


            pos1X = parentView.Left;
            
            pos1Y = node.Parent.Pos == NodePosition.Root || node.Parent.Shape == NodeShape.Bullet ?
                parentView.Top + (int)(parentView.Height / 2) - 1 :
                parentView.Top + parentView.Height - 1;


            pos2Y = node.Shape == NodeShape.Fork || node.Shape == NodeShape.None ?
                nodeView.Top + nodeView.Height - 1 :
                nodeView.Top + nodeView.Height / 2;



            pos2X = nodeView.Left;
            control1X = pos1X + 10;
            
            

            control1Y = pos1Y;
            control2X = pos1X;
            control2Y = pos2Y;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawBezier(p, new PointF(pos1X, pos1Y), new PointF(control1X, control1Y),
                new PointF(control2X, control2Y), new PointF(pos2X, pos2Y));
        }

        protected override void DrawFoldedIndicator(NodeView nodeView, Graphics g, Pen p)
        {
            DrawFoldedIndicatorOnRight(nodeView, g, p);
        }
    }
}
