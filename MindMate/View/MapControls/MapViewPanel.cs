/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.View.MapControls;
using MindMate.Model;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// The drawing canvas for MapView.
    /// Manages and fires Node related events.
    /// </summary>
    public partial class MapViewPanel : Control
    {

        public delegate void NodeClickDelegate(MapNode node, NodeMouseEventArgs args);
        public event NodeClickDelegate NodeClick = delegate { };

        public event Action<MouseEventArgs> CanvasClick = delegate { };

        public delegate void NodeMouseOverDelegate(MapNode node, NodeMouseEventArgs args);
        public event NodeMouseOverDelegate NodeMouseOver = delegate { };

        public event Action<MapNode, MouseEventArgs> NodeMouseEnter = delegate { };
        public event Action<MapNode, MouseEventArgs> NodeMouseExit = delegate { };

        /// <summary>
        /// Node where mouse lies right now.
        /// </summary>
        private MapNode mouseOverNode;
        private Object dragObject;
        private Point dragStartPoint;
        
        public MapViewPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

        }

                
        protected override void OnPaint(PaintEventArgs pe)
        {
            
            if (mapView != null && mapView.tree != null)
            {
                MapControls.Drawing.MapPainter.DrawTree(mapView, pe.Graphics);
                MapControls.Drawing.MapPainter.drawNodeLinker(mapView.tree.RootNode, mapView, pe.Graphics);                
            }
            ////base.OnPaint(pe);            
        }

        private MapView mapView;
        public MapView MapView
        {
            get
            {
                return  mapView;
            }
            set
            {
                mapView = value;
            }
        }

                
        private void NodeLinksPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Tab:
                    e.IsInputKey = true;
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            MapNode node = mapView.GetMapNodeFromPoint(e.Location);
            if (node != null)
            {
                NodeMouseEventArgs args = new NodeMouseEventArgs(e);
                args.NodePortion = mapView.GetNodeView(node).GetNodeClickPortion(e.Location);
                NodeClick(node, args);
            }
            else
            {
                this.dragObject = this;
                this.dragStartPoint = e.Location;
                if (mapView.NodeTextEditor.IsTextEditing)
                {
                    mapView.NodeTextEditor.EndNodeEdit(true, true);
                }
                else
                {
                    mapView.Canvas.Focus();
                }
                
            }

            //base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.dragObject != null && !mapView.NodeTextEditor.IsTextEditing)
            {
                mapView.Canvas.SuspendLayout();
                mapView.Canvas.Top = mapView.Canvas.Top + (e.Y - this.dragStartPoint.Y);
                mapView.Canvas.Left = mapView.Canvas.Left + (e.X - this.dragStartPoint.X);
                mapView.Canvas.ResumeLayout();

                mapView.Canvas.Cursor = Cursors.SizeAll;
                    //new Cursor(new System.IO.MemoryStream(MindMate.Properties.Resources.move_r));
            }
            else
            {
                MapNode node = mapView.GetMapNodeFromPoint(e.Location);
                                
                if (node != null)
                {
                    if (node != mouseOverNode)
                    {
                        if(mouseOverNode != null)
                        {
                            NodeMouseExit(mouseOverNode, e);
                        }
                        mouseOverNode = node;
                        NodeMouseEnter(node, e);
                    }

                    NodeMouseEventArgs args = new NodeMouseEventArgs(e);
                    args.NodePortion = mapView.GetNodeView(node).GetNodeClickPortion(e.Location);
                    NodeMouseOver(node, args);
                }
                else if(mouseOverNode != null)
                {
                    NodeMouseExit(mouseOverNode, e);
                    mouseOverNode = null;
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mouseOverNode == null && this.Cursor == Cursors.Default) // IF 'event is not over node' AND 'canvas is not dragged'
                CanvasClick(e);

            this.dragObject = null;
            this.Cursor = Cursors.Default;
                        
            //base.OnMouseUp(e);            
        }

                                   

    }
}
