using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.MapControls
{
    public class MapViewDragHandler
    {
        private Object dragObject;
        private Point dragStartPoint;

        private MapView MapView { get; set; }

        public delegate void NodeDragStartDelegate(MapNode node, NodeMouseEventArgs e);
        public event NodeDragStartDelegate NodeDragStart;
        public delegate void NodeDragDropDelegate(MapTree tree, DropLocation location);
        public event NodeDragDropDelegate NodeDragDrop;        

        internal MapViewDragHandler(MapView mapView)
        {
            MapView = mapView;
            nodeDragCursor = new Cursor(new System.IO.MemoryStream(MindMate.Properties.Resources.DragMove));
            canvasDragCursor = new Cursor(new System.IO.MemoryStream(MindMate.Properties.Resources.HandDrag));
            
        }

        internal void OnMouseDrag(MouseEventArgs e)
        {
            if (!IsDragging)
            {
                DragStart(e);
            }
            else if (IsCanvasDragging)
            {
                MoveCanvas(e);
            }
            else if(IsNodeDragging)
            {
                ShowDropHint(e);
            }
        }        

        internal void OnMouseDrop(MouseEventArgs e)
        {
            if (IsNodeDragging)
            {
                RefreshNodeDropLocation(e.Location);
                if (!NodeDropLocation.IsEmpty && NodeDragDrop != null)
                {
                    NodeDragDrop(MapView.Tree, NodeDropLocation);
                }
                MapView.Canvas.KeyDown -= Canvas_KeyDown;
            }           

            dragObject = null;
            NodeDropLocation = new DropLocation();
            MapView.Canvas.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool IsDragging
        {
            get { return this.dragObject != null; }
        }

        public bool IsCanvasDragging
        {
            get { return dragObject == MapView.Canvas; }
        }

        public bool IsNodeDragging
        {
            get { return dragObject != null && dragObject != MapView.Canvas;  }
        }

        public DropLocation NodeDropLocation
        {
            get; private set;
        }

        private Cursor nodeDragCursor;
        private Cursor canvasDragCursor;

        #region Private Methods

        private void DragStart(MouseEventArgs e)
        {
            MapNode node = MapView.GetMapNodeFromPoint(e.Location);
            if (node == null)
            {
                this.dragObject = MapView.Canvas;
                this.dragStartPoint = e.Location;
                MapView.Canvas.Cursor = canvasDragCursor;
                //MapView.Canvas.Cursor = Cursors.SizeAll;                
            }
            else
            {
                this.dragObject = node;
                if(NodeDragStart != null) { NodeDragStart(node, new NodeMouseEventArgs(e)); }
                MapView.Canvas.Cursor = nodeDragCursor;
                MapView.Canvas.KeyDown += Canvas_KeyDown;
            }
        }        

        private void MoveCanvas(MouseEventArgs e)
        {
            MapView.Canvas.SuspendLayout();
            MapView.Canvas.Top = MapView.Canvas.Top + (e.Y - this.dragStartPoint.Y);
            MapView.Canvas.Left = MapView.Canvas.Left + (e.X - this.dragStartPoint.X);
            MapView.Canvas.ResumeLayout();            
        }

        private void RefreshNodeDropLocation(Point p)
        {
            DropLocation location = CalculateDropLocation(p);
            if(!location.Equals(NodeDropLocation))
            {
                if(IsValidDropLocation(location))
                {
                    NodeDropLocation = location;
                    MapView.Canvas.Invalidate();
                }
                else if(!NodeDropLocation.IsEmpty)
                {
                    NodeDropLocation = new DropLocation();
                    MapView.Canvas.Invalidate();
                }
                
            }            
        }

        private DropLocation CalculateDropLocation(Point p)
        {
            MapNode node = MapView.GetMapNodeFromPoint(p);

            if (node != null)
            {
                return new DropLocation() { Parent = node, InsertAfterSibling = true };
            }
            else
            {
                return new DropLocation();
            }
        }

        private bool IsValidDropLocation(DropLocation location)
        {
            if (location.IsEmpty) { return false; }

            foreach(MapNode n in MapView.Tree.SelectedNodes)
            {
                if(n == location.Parent) { return false; } //drop location is included in moved nodes

                if (n.Parent == location.Parent)
                {
                    if(n.Next != null && n.Next == location.Sibling && !location.InsertAfterSibling) //same location as present
                    { return false; }
                    if(n.Previous != null && n.Previous == location.Sibling && location.InsertAfterSibling) //same location as present
                    { return false; }                    
                }

                if (n.Pos == NodePosition.Root) { return false; } //can't move root

                if (location.Parent.IsDescendent(n)) { return false; } //can't move ancentor to child
            }                
            
            return true;
        }       

        private void ShowDropHint(MouseEventArgs e)
        {
            RefreshNodeDropLocation(e.Location);            
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                MapView.Canvas.KeyDown -= Canvas_KeyDown;
                this.dragObject = null;
                MapView.Canvas.Cursor = Cursors.Default;
            }
        }

        #endregion Private Methods

    }

    public struct DropLocation
    {
        public MapNode Parent;
        public MapNode Sibling;
        public bool InsertAfterSibling;
        
        public bool IsEmpty
        {
            get { return Parent == null; }
        }

        public override bool Equals(object obj)
        {
            return obj is DropLocation  && Equals((DropLocation)obj);
        }

        public bool Equals(DropLocation loc)
        {
            return loc.Parent == this.Parent && loc.Sibling == this.Sibling && loc.InsertAfterSibling == this.InsertAfterSibling;

        }
        
    }
}
