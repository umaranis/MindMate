using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MindMate.View.MapControls
{
    public class MapViewFormatPainter
    {
        private MapNode formatSource;
        private MapView mapView;

        public MapViewFormatPainter(MapView mapView)
        {
            this.mapView = mapView;
        }
        
        public FormatPainterStatus Status { get; private set; }

        public bool Active { get { return Status != FormatPainterStatus.Empty; } }

        public event Action<MapViewFormatPainter> StateChanged;

        public void Copy(MapNode node, bool multiApply = false)
        {
            this.formatSource = node;
            Status = multiApply? FormatPainterStatus.MultiApply : FormatPainterStatus.SingleApply;
            mapView.Canvas.KeyDown += Canvas_KeyDown;
            mapView.Canvas.NodeMouseEnter += Canvas_NodeMouseEnter;
            mapView.Canvas.NodeMouseExit += Canvas_NodeMouseExit;

            if(StateChanged != null) { StateChanged(this); }
        }
        public void EnableMultiApply()
        {
            Debug.Assert(formatSource != null && Status == FormatPainterStatus.SingleApply, "For enabling multi-apply, format source should be already selected.");

            Status = FormatPainterStatus.MultiApply;

            if (StateChanged != null) { StateChanged(this); }
        }

        public void Paste(MapNode target)
        {
            Debug.Assert(target != null, "Copy/Paste format: Target node is null");

            formatSource.CopyFormatTo(target);

            if(Status == FormatPainterStatus.SingleApply) { Clear(); }

            if (StateChanged != null) { StateChanged(this); }
        }

        public void Paste(IEnumerable<MapNode> nodes)
        {
            if(formatSource == null) { return; }

            foreach (MapNode node in nodes)
            {
                formatSource.CopyFormatTo(node);
            }

            if (Status == FormatPainterStatus.SingleApply) { Clear(); }

            if (StateChanged != null) { StateChanged(this); }
        }

        public void Clear()
        {
            Status = FormatPainterStatus.Empty;
            mapView.Canvas.KeyDown -= Canvas_KeyDown;
            mapView.Canvas.NodeMouseEnter -= Canvas_NodeMouseEnter;
            mapView.Canvas.NodeMouseExit -= Canvas_NodeMouseExit;
            mapView.Canvas.Cursor = System.Windows.Forms.Cursors.Default;

            if (StateChanged != null) { StateChanged(this); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">node is null if click is on canvas</param>
        internal void ExecuteMouseClick(MapNode node)
        {
            if(node == null)
            {
                Clear();
            }
            else
            {
                Paste(node);
                node.Selected = true;
            }
        }

        private void Canvas_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                Clear();
            }
        }

        private void Canvas_NodeMouseEnter(MapNode arg1, System.Windows.Forms.MouseEventArgs arg2)
        {
            mapView.Canvas.Cursor = new System.Windows.Forms.Cursor(new System.IO.MemoryStream(MindMate.Properties.Resources.painter)); 
        }

        private void Canvas_NodeMouseExit(MapNode arg1, System.Windows.Forms.MouseEventArgs arg2)
        {
            mapView.Canvas.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
    }

    public enum FormatPainterStatus { Empty, SingleApply, MultiApply }
}
