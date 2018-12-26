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
        //protected override void DrawNodeLinker(NodeView nodeView, Graphics g, Pen p)
        //{
        //    MapNode node = nodeView.Node;
        //    NodeView parentView = NodeView.GetNodeView(node.Parent);
        //    if (nodeView == null || parentView == null) return;

        //    float pos1X, pos1Y, // connector start point on parent node
        //        pos2X, pos2Y;   // connector end point on current/child node

        //    float control1X, control1Y, control2X, control2Y;


        //    pos1X = parentView.Left;
            
        //    pos1Y = node.Parent.Pos == NodePosition.Root || node.Parent.Shape == NodeShape.Bullet ?
        //        parentView.Top + (int)(parentView.Height / 2) - 1 :
        //        parentView.Top + parentView.Height - 1;


        //    pos2Y = node.Shape == NodeShape.Fork || node.Shape == NodeShape.None ?
        //        nodeView.Top + nodeView.Height - 1 :
        //        nodeView.Top + nodeView.Height / 2;



        //    pos2X = nodeView.Left;
        //    control1X = pos1X + 10;
            
            

        //    control1Y = pos1Y;
        //    control2X = pos1X;
        //    control2Y = pos2Y;

        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //    g.DrawBezier(p, new PointF(pos1X, pos1Y), new PointF(control1X, control1Y),
        //        new PointF(control2X, control2Y), new PointF(pos2X, pos2Y));
        //}

        protected override void DrawNodeLinker(NodeView nodeView, Graphics g, Pen p)
        {
            MapNode node = nodeView.Node;
            NodeView parentView = NodeView.GetNodeView(node.Parent);
            if (nodeView == null || parentView == null) return;

            float pos1X, pos1Y, // connector start point on parent node                
                pos2X, pos2Y;   // connector end point on current/child node

            pos1X = parentView.Left;
            pos1Y = parentView.Top + (int)(parentView.Height / 2);


            pos2Y = nodeView.Top + nodeView.Height / 2;
            pos2X = nodeView.Left;

            float y_mid = node.Parent.Shape == NodeShape.Fork || node.Parent.Shape == NodeShape.None? 
                nodeView.Top + nodeView.Height - 1 : nodeView.Top + nodeView.Height / 2;

            var points = new PointF[]
            {
                new PointF(parentView.Left, parentView.Top + parentView.Height),
                new PointF(parentView.Left, y_mid),
                new PointF(nodeView.Left, y_mid)
            };

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.AssumeLinear;
            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            //g.DrawLine(p, pos1X, pos1Y, pos1X, pos2Y);
            //g.DrawLine(p, pos1X, pos2Y, pos2X, pos2Y);            

            g.DrawLines(p, points);

        }

        protected override void DrawFoldedIndicator(NodeView nodeView, Graphics g, Pen p)
        {
            if (nodeView.Node.HasChildren && nodeView.Node.Folded)
            {
                float x;
                float y = nodeView.Node.Shape == NodeShape.Fork || nodeView.Node.Shape == NodeShape.None ?
                    y = nodeView.Top + nodeView.Height - 1 : // draw folded indicator at bottom
                    y = nodeView.Top + nodeView.Height / 2;  // draw folded indicator at mid point


                x = nodeView.Left - BasePainter.INDICATOR_MARGIN;

                // draw folded indicator
                if (nodeView.Node.Shape != NodeShape.Bullet)
                    g.FillEllipse(p.Brush, new RectangleF(new PointF(x - 6, y - 3), new Size(6, 6))); 
                else
                    g.FillPolygon(p.Brush, new PointF[3] {
                                new PointF(x, y - 5),
                                new PointF(x, y + 5),
                                new PointF(x - 6, y)
                            });

            }
        }
    }
}
