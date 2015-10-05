using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.MapControls
{
    class MapViewDragHandler
    {
        private Object dragObject;
        private Point dragStartPoint;

        private MapView MapView { get; set; }

        public MapViewDragHandler(MapView mapView)
        {
            MapView = mapView;
        }

        public void OnMouseDrag(MouseEventArgs e)
        {
            if (this.dragObject == null)
            {
                MapNode node = MapView.GetMapNodeFromPoint(e.Location);
                if (node == null)
                {
                    this.dragObject = this;
                    this.dragStartPoint = e.Location;
                    MapView.Canvas.Focus();
                }
                else
                {
                    this.dragObject = node;
                }
            }
            else if (this.dragObject == this)
            {
                MapView.Canvas.SuspendLayout();
                MapView.Canvas.Top = MapView.Canvas.Top + (e.Y - this.dragStartPoint.Y);
                MapView.Canvas.Left = MapView.Canvas.Left + (e.X - this.dragStartPoint.X);
                MapView.Canvas.ResumeLayout();

                MapView.Canvas.Cursor = Cursors.SizeAll;
                //new Cursor(new System.IO.MemoryStream(MindMate.Properties.Resources.move_r));
            }
            else
            {

            }
        }        

        public void OnMouseDrop(MouseEventArgs e)
        {
            dragObject = null;
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
        
    }
}
