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
        
        internal static void DrawTree(MapView mapView, System.Drawing.Graphics g)
        {
            //Draw root node
            NodeView nView = mapView.GetNodeView(mapView.tree.RootNode);
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
            
            DrawNode(node, g);

            if (bDrawChildren)
            {
                DrawChildNodes(node, mapView, g);
            }

        }

        private static void DrawNode(NodeView nodeView, System.Drawing.Graphics g)
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
                g.FillRectangle(Brushes.LightGray, new RectangleF(nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height));
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
        internal static void drawNodeLinker(MapNode node, MapView mapView, Graphics g)
        {
            drawNodeLinker(node, mapView, g, true);
        }

        internal static void drawNodeLinker(MapNode node, MapView mapView, Graphics g, bool drawChildren)
        {
            if (node.Parent != null)
            {
                
                NodeView nodeView = mapView.GetNodeView(node);
                NodeView parentView = mapView.GetNodeView(node.Parent);
                if (nodeView == null || parentView == null) return;

                //var oLink = null;
                float pos1X, pos1Y, pos2X, pos2Y;
                //int toX, toY;
                float control1X, control1Y, control2X, control2Y;
                float tipLen = 0;

                if (node.Pos == NodePosition.Right)
                {
                    pos1X = parentView.Left + parentView.Width;
                }
                else
                {
                    pos1X = parentView.Left;
                }
                pos1Y = parentView.Top + ((node.Parent.Pos == NodePosition.Root) ? (int)(parentView.Height / 2) : parentView.Height) - 1;

                pos2Y = nodeView.Top + nodeView.Height - 1;

                
                if (node.HasChildren && node.Folded)
                {
                    tipLen = 2;	// add dot
                }

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
                    if(disposePen == false) 
                        p = new Pen(Color.Gray, node.LineWidth);
                    else
                        p.Width = node.LineWidth;
                    disposePen = true;
                }

                if(node.LinePattern != System.Drawing.Drawing2D.DashStyle.Solid && 
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

                float x = 0;
                if (node.Pos == NodePosition.Right)
                {
                    x = (pos2X + nodeView.Width + tipLen);
                }
                else
                {
                    x = (pos2X - nodeView.Width - tipLen);
                }

                //p.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;

                switch (node.Shape)
                {
                    case NodeShape.None:
                    case NodeShape.Fork:
                        g.DrawLine(p, new PointF(pos2X, pos2Y), new PointF(x, pos2Y));
                        break;
                    case NodeShape.Bubble:
                        System.Drawing.Drawing2D.GraphicsPath path = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top, (int)nodeView.Width, (int)nodeView.Height);
                        g.DrawPath(p, path);
                        break;
                    case NodeShape.Box:
                        g.DrawRectangle(p, nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height);
                        break;
                }
                

                if (tipLen > 0)
                {
                    if (node.Pos == NodePosition.Right)
                    {
                        g.DrawEllipse(p, new RectangleF(new PointF(x, pos2Y - 3), new Size(6, 6)));
                    }
                    else
                    {
                        g.DrawEllipse(p, new RectangleF(new PointF(x - 6, pos2Y - 3), new Size(6, 6)));
                    }
                }

                if(disposePen) p.Dispose();

            }           
 
            

            // recursive
            if (drawChildren && !node.Folded)
            {
                foreach (MapNode cNode in node.ChildNodes)
                {
                    drawNodeLinker(cNode, mapView, g);
                }
            }
        }
    }
}
