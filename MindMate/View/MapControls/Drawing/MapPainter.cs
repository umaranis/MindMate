/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System.Drawing;
using System;
using System.Drawing.Drawing2D;

namespace MindMate.View.MapControls.Drawing
{
    /// <summary>
    /// Performs all drawing on the panel surface.
    /// </summary>
    public static class MapPainter
    {

        /// <summary>
        /// Draw tree (nodes + connections)
        /// </summary>
        /// <param name="iView"></param>
        /// <param name="g"></param>
        public static void DrawTree(IView iView, Graphics g)
        {
            DrawTreeNodes(iView, g);
            DrawNodeLinker(iView.Tree.RootNode, iView, g);

            foreach (var node in iView.Tree.SelectedNodes)
            {
                DrawSelection(node.NodeView, g);    
            }
            if (iView.HighlightedNode != null)
            {
                DrawHighlight(iView.HighlightedNode.NodeView, g);
            }
        }

        /// <summary>
        /// Draw Tree Nodes without connections
        /// </summary>
        /// <param name="iView"></param>
        /// <param name="g"></param>
        public static void DrawTreeNodes(IView iView, Graphics g)
        {
            //Draw root node
            NodeView nView = iView.GetNodeView(iView.Tree.RootNode);
            DrawRootNode(nView, g);

            //Draw rest
            DrawChildNodes(nView, iView, g);
        }

        private static void DrawChildNodes(NodeView nodeView, IView iView, Graphics g)
        {
            if (!nodeView.Node.Folded)
            {
                foreach (MapNode cNode in nodeView.Node.ChildNodes)
                {
                    DrawNode(cNode, true, iView, g);
                }
            }
        }

        public static void DrawNode(MapNode node, bool bDrawChildren, IView iView, System.Drawing.Graphics g)
        {
            NodeView nodeView = iView.GetNodeView(node);
            DrawNode(nodeView, g, iView.HighlightedNode == node);

            if (bDrawChildren)
            {
                DrawChildNodes(nodeView, iView, g);
            }

        }

        private static void DrawNode(NodeView nodeView, Graphics g, bool highlight = false)
        {
            MapNode node = nodeView.Node;

            DrawBackColor(nodeView, g);
            
            TextRenderer.DrawText(g, node.Text, nodeView.NodeFormat.Font,
                new RectangleF(nodeView.RecText.Left, nodeView.RecText.Top, NodeView.MAXIMUM_TEXT_WIDTH, 5000),
                nodeView.NodeFormat.Color);
            
            for (int i = 0; i < nodeView.RecIcons.Count; i++)
            {
                nodeView.RecIcons[i].Draw(g);
            }
            
            nodeView.NoteIcon?.Draw(g);
            nodeView.Link?.Draw(g);
            nodeView.ImageView?.Draw(g);
        }

        private static void DrawRootNode(NodeView nodeView, Graphics g)
        {
            MapNode node = nodeView.Node;

            DrawBackColor(nodeView, g);

            TextRenderer.DrawText(g, node.Text, nodeView.NodeFormat.Font,
                new RectangleF(nodeView.RecText.Left, nodeView.RecText.Top, NodeView.MAXIMUM_TEXT_WIDTH, 5000),
                nodeView.NodeFormat.Color);
            for (int i = 0; i < nodeView.RecIcons.Count; i++)
            {
                nodeView.RecIcons[i].Draw(g);
            }
            nodeView.NoteIcon?.Draw(g);
            nodeView.Link?.Draw(g);
            nodeView.ImageView?.Draw(g);

            Pen p = nodeView.NodeFormat.LinePen;

            GraphicsPath pathCap = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top - 2, (int)nodeView.Width, (int)nodeView.Height + 2);
            g.DrawPath(p, pathCap);
            pathCap = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top - 1, (int)nodeView.Width, (int)nodeView.Height + 1);
            g.DrawPath(p, pathCap);

            
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            GraphicsPath path = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top, (int)nodeView.Width, (int)nodeView.Height);
            g.DrawPath(p, path);
        }

        private static void DrawBackColor(NodeView nodeView, Graphics g)
        {
            if (!nodeView.NodeFormat.BackColor.IsEmpty)
            {
                if (nodeView.Node.Pos == NodePosition.Root || nodeView.NodeFormat.Shape == NodeShape.Bubble)
                {
                    GraphicsPath path = RoundedRectangle.Create((int)nodeView.Left, (int)nodeView.Top, (int)nodeView.Width, (int)nodeView.Height);
                    g.FillPath(nodeView.NodeFormat.BackColorBrush, path);
                }
                else
                {
                    g.FillRectangle(nodeView.NodeFormat.BackColorBrush, new RectangleF(nodeView.Left, nodeView.Top, nodeView.Width, nodeView.Height));
                }
            }
        }

        private static void DrawSelection(NodeView nView, Graphics g)
        {
            Pen p = nView.Node.Tree.SelectedNodeOutlinePen;
            g.DrawLine(p, nView.Left, nView.Top, nView.Right, nView.Top);
            g.DrawLine(p, nView.Left, nView.Bottom - 1, nView.Right, nView.Bottom - 1);
            g.DrawArc(p, nView.Right - 2, nView.Top, 5, nView.Height - 1, 270, 180);
            g.DrawArc(p, nView.Left - 3, nView.Top, 5, nView.Height - 1, 90, 180);
        }

        /// <summary>
        /// Temporary change as mouse moves over a node
        /// </summary>
        /// <param name="nView"></param>
        /// <param name="g"></param>
        private static void DrawHighlight(NodeView nView, Graphics g)
        {
            Pen nodeHighlightPen = nView.Node.Tree.NodeHighlightPen;
            g.DrawLine(nodeHighlightPen, nView.Left, nView.Top, nView.Right, nView.Top);
            g.DrawLine(nodeHighlightPen, nView.Left, nView.Bottom - 1, nView.Right, nView.Bottom - 1);
            g.DrawArc(nodeHighlightPen, nView.Right - 2, nView.Top, 5, nView.Height - 1, 270, 180);
            g.DrawArc(nodeHighlightPen, nView.Left - 3, nView.Top, 5, nView.Height - 1, 90, 180);
        }

        /// <summary>
        /// Draw node linker for the node and all its children
        /// </summary>
        /// <param name="node"></param>
        /// <param name="iView"></param>
        /// <param name="g"></param>
        /// <param name="drawChildren">if false, linkers are not drawn for children</param>
        public static void DrawNodeLinker(MapNode node, IView iView, Graphics g, bool drawChildren = true)
        {
            if (node.Parent != null)
            {

                NodeView nodeView = iView.GetNodeView(node);
                NodeView parentView = iView.GetNodeView(node.Parent);
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

                pos1Y = node.Parent.Pos == NodePosition.Root || node.Parent.NodeView.NodeFormat.Shape == NodeShape.Bullet ?
                    parentView.Top + (int)(parentView.Height / 2) - 1 :
                    parentView.Top + parentView.Height - 1;
                
                
                pos2Y = node.NodeView.NodeFormat.Shape == NodeShape.Fork || node.NodeView.NodeFormat.Shape == NodeShape.None ?
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


                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                Pen p = nodeView.NodeFormat.LinePen;

                g.DrawBezier(p, new PointF(pos1X, pos1Y), new PointF(control1X, control1Y),
                    new PointF(control2X, control2Y), new PointF(pos2X, pos2Y));


                DrawNodeShape(nodeView, g, p);

                DrawFoldedIndicator(nodeView, g, p);

                DrawFoldedIndicatorToNodeConnector(nodeView, g, p);
            }



            // recursive
            if (drawChildren && !node.Folded)
            {
                foreach (MapNode cNode in node.ChildNodes)
                {
                    DrawNodeLinker(cNode, iView, g);
                }
            }
        }        

        private static void DrawNodeShape(NodeView nodeView, Graphics g, Pen p)
        {
            switch (nodeView.NodeFormat.Shape)
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

        public static void DrawNodeShape(MapNode node, IView iView, Graphics g)
        {
            DrawNodeShape(iView.GetNodeView(node), g, node.NodeView.NodeFormat.LinePen);
        }

        const int INDICATOR_MARGIN = 2;

        private static void DrawFoldedIndicator(NodeView nodeView, Graphics g, Pen p)
        {
            if (nodeView.Node.HasChildren && nodeView.Node.Folded)
            {
                float x;
                float y = nodeView.NodeFormat.Shape == NodeShape.Fork || nodeView.NodeFormat.Shape == NodeShape.None ?
                    y = nodeView.Top + nodeView.Height - 1 : // draw folded indicator at bottom
                    y = nodeView.Top + nodeView.Height / 2;  // draw folded indicator at mid point
                
                if (nodeView.Node.Pos == NodePosition.Right)
                {
                    x = nodeView.Left + nodeView.Width + INDICATOR_MARGIN;

                    // draw folded indicator
                    if(nodeView.NodeFormat.Shape != NodeShape.Bullet)
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
                    if (nodeView.NodeFormat.Shape != NodeShape.Bullet)
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
                (nodeView.NodeFormat.Shape == NodeShape.Fork || nodeView.NodeFormat.Shape == NodeShape.None)) // only if shape is fork
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

        public static void DrawNodeDropHint(DropLocation location, Graphics g)
        {
            NodeView pView = location.Parent.NodeView;
            Pen dropHintPen = pView.Node.Tree.DropHintPen;

            g.DrawLine(dropHintPen, pView.Left, pView.Top, pView.Right, pView.Top);
            g.DrawLine(dropHintPen, pView.Left, pView.Bottom - 1, pView.Right, pView.Bottom - 1);
            g.DrawArc(dropHintPen, pView.Right - 2, pView.Top, 5, pView.Height - 1, 270, 180);
            g.DrawArc(dropHintPen, pView.Left - 3, pView.Top, 5, pView.Height - 1, 90, 180);            
        }

    }
}
