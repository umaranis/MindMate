/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using System.Drawing;

namespace MindMate.View.MapControls.Drawing
{
    /// <summary>
    /// Performs all drawing on the panel surface.
    /// </summary>
    static class MapPainter
    {

        public static Brush HighlightBrush = new SolidBrush(Color.FromArgb(235, 235, 235));
        private static Pen dropHintPen = new Pen(Color.Red);

        static MapPainter()
        {
            dropHintPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        internal static void DrawTree(MapView mapView, System.Drawing.Graphics g)
        {
            //Draw root node
            NodeView nView = mapView.GetNodeView(mapView.Tree.RootNode);
            DrawRootNode(nView, g);

            //Draw rest
            DrawChildNodes(nView, mapView, g);
        }

        private static void DrawChildNodes(NodeView nodeView, MapView mapView, System.Drawing.Graphics g)
        {
            if (!nodeView.Node.Folded)
            {
                foreach (MapNode cNode in nodeView.Node.ChildNodes)
                {
                    NodeView child = mapView.GetNodeView(cNode);
                    DrawNode(child, true, mapView, g);
                }
            }
        }

        internal static void DrawNode(NodeView node, bool bDrawChildren, MapView mapView, System.Drawing.Graphics g)
        {

            DrawNode(node, g, mapView.HighlightedNode == node.Node);

            if (bDrawChildren)
            {
                DrawChildNodes(node, mapView, g);
            }

        }

        private static void DrawNode(NodeView nodeView, System.Drawing.Graphics g, bool highlight = false)
        {
            MapNode node = nodeView.Node;
            if (!nodeView.BackColor.IsEmpty)
            {
                using (Brush brush = new SolidBrush(nodeView.BackColor))
                {
                    g.FillRectangle(brush, new RectangleF(nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height));
                }
            }
            if (nodeView.Selected)
                g.FillRectangle(Brushes.LightGray, nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height);
            else if (highlight)
                g.FillRectangle(HighlightBrush, nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height);
            TextRenderer.DrawText(g, node.Text, nodeView.Font,
                new RectangleF(nodeView.RecText.Left, nodeView.RecText.Top, NodeView.MAXIMUM_TEXT_WIDTH, 5000),
                nodeView.TextColor);
            for (int i = 0; i < nodeView.RecIcons.Count; i++)
            {
                nodeView.RecIcons[i].Draw(g);
            }
            if (nodeView.NoteIcon != null) nodeView.NoteIcon.Draw(g);
            if (nodeView.Link != null) nodeView.Link.Draw(g);
        }

        private static void DrawRootNode(NodeView nodeView, System.Drawing.Graphics g)
        {
            MapNode node = nodeView.Node;

            System.Drawing.Drawing2D.GraphicsPath path = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top, (int)nodeView.Width, (int)nodeView.Height);

            if (!node.BackColor.IsEmpty)
                g.FillPath(new SolidBrush(node.BackColor), path);
            if (nodeView.Selected)
                g.FillPath(Brushes.LightGray, path);
            //g.DrawString(node.Text, nodeView.Font, nodeView.TextColor,
            //    new RectangleF(nodeView.RecText.Left, nodeView.RecText.Top, NodeView.MAXIMUM_TEXT_WIDTH, 0));
            TextRenderer.DrawText(g, node.Text, nodeView.Font,
                new RectangleF(nodeView.RecText.Left, nodeView.RecText.Top, NodeView.MAXIMUM_TEXT_WIDTH, 5000),
                nodeView.TextColor);
            for (int i = 0; i < nodeView.RecIcons.Count; i++)
            {
                nodeView.RecIcons[i].Draw(g);
            }
            if (nodeView.NoteIcon != null) nodeView.NoteIcon.Draw(g);
            if (nodeView.Link != null) nodeView.Link.Draw(g);

            System.Drawing.Drawing2D.GraphicsPath pathCap = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top - 2, (int)nodeView.Width, (int)nodeView.Height + 2);
            g.DrawPath(Pens.Gray, pathCap);
            pathCap = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top - 1, (int)nodeView.Width, (int)nodeView.Height + 1);
            g.DrawPath(Pens.LightGray, pathCap);

            Pen p = Pens.Gray;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawPath(p, path);



        }


        /// <summary>
        /// Draw node linker for the node and all its children
        /// </summary>
        /// <param name="node"></param>
        /// <param name="g"></param>
        internal static void DrawNodeLinker(MapNode node, MapView mapView, Graphics g)
        {
            DrawNodeLinker(node, mapView, g, true);
        }

        internal static void DrawNodeLinker(MapNode node, MapView mapView, Graphics g, bool drawChildren)
        {
            if (node.Parent != null)
            {

                NodeView nodeView = mapView.GetNodeView(node);
                NodeView parentView = mapView.GetNodeView(node.Parent);
                if (nodeView == null || parentView == null) return;

                float pos1X, pos1Y, // connector start point on parent node
                    pos2X, pos2Y;   // connector end point on current/child node

                float control1X, control1Y, control2X, control2Y;


                if (node.Pos == NodePosition.Right)
                {
                    pos1X = parentView.Left + parentView.Width;
                }
                else
                {
                    pos1X = parentView.Left;
                }

                pos1Y = node.Parent.Pos == NodePosition.Root || node.Parent.Shape == NodeShape.Bullet ?
                    parentView.Top + (int)(parentView.Height / 2) - 1 :
                    parentView.Top + parentView.Height - 1;
                
                
                pos2Y = node.Shape == NodeShape.Fork || node.Shape == NodeShape.None ?
                    nodeView.Top + nodeView.Height - 1 :
                    nodeView.Top + nodeView.Height / 2;



                if (node.Pos == NodePosition.Right)
                {
                    pos2X = nodeView.Left;
                    control1X = pos1X + 10;
                }
                else
                {
                    pos2X = nodeView.Left + nodeView.Width;
                    control1X = pos1X - 10;
                }

                control1Y = pos1Y;
                control2X = pos1X;
                control2Y = pos2Y;


                Pen p = Pens.Gray;
                bool disposePen = false;
                if (!node.LineColor.IsEmpty)
                {
                    p = new Pen(node.LineColor);
                    disposePen = true;
                }

                if (node.LineWidth != 0 && node.LineWidth != 1)
                {
                    if (disposePen == false)
                        p = new Pen(Color.Gray, node.LineWidth);
                    else
                        p.Width = node.LineWidth;
                    disposePen = true;
                }

                if (node.LinePattern != System.Drawing.Drawing2D.DashStyle.Solid &&
                    node.LinePattern != System.Drawing.Drawing2D.DashStyle.Custom)
                {
                    if (disposePen == false) p = new Pen(Color.Gray);
                    p.DashCap = System.Drawing.Drawing2D.DashCap.Round;
                    p.DashStyle = node.LinePattern;
                    disposePen = true;
                }


                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                g.DrawBezier(p, new PointF(pos1X, pos1Y), new PointF(control1X, control1Y),
                    new PointF(control2X, control2Y), new PointF(pos2X, pos2Y));


                DrawNodeShape(nodeView, g, p);

                DrawFoldedIndicator(nodeView, g, p);

                DrawFoldedIndicatorToNodeConnector(nodeView, g, p);

                if (disposePen) p.Dispose();

            }



            // recursive
            if (drawChildren && !node.Folded)
            {
                foreach (MapNode cNode in node.ChildNodes)
                {
                    DrawNodeLinker(cNode, mapView, g);
                }
            }
        }

        private static void DrawNodeShape(NodeView nodeView, Graphics g, Pen p)
        {
            switch (nodeView.Node.Shape)
            {
                case NodeShape.None:
                case NodeShape.Fork:
                    float y = nodeView.Top + nodeView.Height - 1;
                    g.DrawLine(p, nodeView.Left, y, nodeView.Left + nodeView.Width, y);
                    break;
                case NodeShape.Bubble:
                    System.Drawing.Drawing2D.GraphicsPath path = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top, (int)nodeView.Width, (int)nodeView.Height);
                    g.DrawPath(p, path);
                    break;
                case NodeShape.Box:
                    g.DrawRectangle(p, nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height);
                    break;  
                case NodeShape.Bullet:
                    float x = nodeView.Node.Pos == NodePosition.Right ? nodeView.Left : nodeView.Left + nodeView.Width;
                    using (Pen penBullet = new Pen(p.Color, 2f))
                    {
                        g.DrawLine(penBullet, x, nodeView.Top + 1, x, nodeView.Top + nodeView.Height - 1);
                    }
                    break;
            }
        }

        const int INDICATOR_MARGIN = 2;

        private static void DrawFoldedIndicator(NodeView nodeView, Graphics g, Pen p)
        {
            if (nodeView.Node.HasChildren && nodeView.Node.Folded)
            {
                float x;
                float y = nodeView.Node.Shape == NodeShape.Fork || nodeView.Node.Shape == NodeShape.None ?
                    y = nodeView.Top + nodeView.Height - 1 : // draw folded indicator at bottom
                    y = nodeView.Top + nodeView.Height / 2;  // draw folded indicator at mid point
                
                if (nodeView.Node.Pos == NodePosition.Right)
                {
                    x = nodeView.Left + nodeView.Width + INDICATOR_MARGIN;

                    // draw folded indicator
                    if(nodeView.Node.Shape != NodeShape.Bullet)
                        g.DrawEllipse(p, new RectangleF(new PointF(x, y - 3), new Size(6, 6))); 
                    else
                        g.FillPolygon(p.Brush, new PointF[3] {
                            new PointF(x, y - 5),
                            new PointF(x, y + 5),
                            new PointF(x + 6, y)
                        });
                    
                }
                else
                {
                    x = nodeView.Left - INDICATOR_MARGIN; 
                    
                    // draw folded indicator
                    if (nodeView.Node.Shape != NodeShape.Bullet)
                        g.DrawEllipse(p, new RectangleF(new PointF(x - 6, y - 3), new Size(6, 6)));
                    else
                        g.FillPolygon(p.Brush, new PointF[3] {
                                new PointF(x, y - 5),
                                new PointF(x, y + 5),
                                new PointF(x - 6, y)
                            });
                    
                }
            }
        }

        private static void DrawFoldedIndicatorToNodeConnector(NodeView nodeView, Graphics g, Pen p)
        {
            if (nodeView.Node.HasChildren && nodeView.Node.Folded && // only if node is folded
                (nodeView.Node.Shape == NodeShape.Fork || nodeView.Node.Shape == NodeShape.None)) // only if shape is fork
            {
                float x, y;
                y = nodeView.Top + nodeView.Height - 1;
                if (nodeView.Node.Pos == NodePosition.Right)
                {
                    x = nodeView.Left + nodeView.Width;
                    g.DrawLine(p, x, y, x + INDICATOR_MARGIN, y); // draw link between folded indicator and node shape
                }
                else
                {
                    x = nodeView.Left;
                    g.DrawLine(p, x, y, x - INDICATOR_MARGIN, y);
                }
            }

        }

        internal static void DrawNodeDropHint(DropLocation location, Graphics g)
        {
            NodeView pView = location.Parent.NodeView;
            
            g.DrawLine(dropHintPen, pView.Left, pView.Top, pView.Right, pView.Top);
            g.DrawLine(dropHintPen, pView.Left, pView.Bottom - 1, pView.Right, pView.Bottom - 1);
            g.DrawArc(dropHintPen, pView.Right - 2, pView.Top, 5, pView.Height - 1, 270, 180);
            g.DrawArc(dropHintPen, pView.Left - 3, pView.Top, 5, pView.Height - 1, 90, 180);            
        }

    }
}
