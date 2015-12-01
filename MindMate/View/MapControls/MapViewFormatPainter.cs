using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        public void Paste(MapNode target, bool clearPainter = true)
        {
            Debug.Assert(target != null, "Copy/Paste format: Target node is null");

            formatSource.CopyFormatTo(target);

            if(Status == FormatPainterStatus.SingleApply && clearPainter) { Clear(); }

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
                mapView.SuspendLayout();
                mapView.Tree.ChangeManager.StartBatch("Copy/Paste Format");

                bool clearPainter = (Control.ModifierKeys & Keys.Control) != Keys.Control;
                bool shiftKey = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

                Paste(node, clearPainter);
                if(shiftKey) { ApplyFormatToNodesInBetween(node); }
                node.Selected = true;

                if (mapView.Tree.ChangeManager.IsBatchOpen) { mapView.Tree.ChangeManager.EndBatch(); }
                mapView.ResumeLayout(true, !shiftKey? mapView.SelectedNodes.First.Pos : NodePosition.Undefined);
            }
        }

        private void ApplyFormatToNodesInBetween(MapNode node)
        {
            MapNode startNode = null, endNode = null;
            switch (node.GetSiblingLocation(formatSource))
            {
                case MapNode.SiblingLocaton.Above:
                    startNode = formatSource;
                    endNode = node;
                    break;
                case MapNode.SiblingLocaton.Below:
                    startNode = node;
                    endNode = formatSource;
                    break;
                case MapNode.SiblingLocaton.NotSibling:
                    return;
            }

            startNode = startNode.Next;
            while(startNode != endNode)
            {
                formatSource.CopyFormatTo(startNode);
                startNode = startNode.Next;
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
