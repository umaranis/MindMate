using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// Attaches <see cref="NodeContextMenu"/> to node right click event of MapView
    /// </summary>
    public class ContextMenuAttacher
    {
        private MapView MapView { get; }
        private ContextMenuStrip ContextMenu { get; }

        public ContextMenuAttacher(ContextMenuStrip cm, MapView mapView)
        {
            MapView = mapView;
            ContextMenu = cm;

            MapView.Canvas.NodeRightClick += Canvas_NodeRightClick;
            MapView.Canvas.KeyDown += Canvas_KeyDown;
            MapView.NodeTextEditor.ContextMenu = ContextMenu;
        }

        void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Apps && MapView.SelectedNodes.First != null)
            {
                NodeView nodeView = MapView.GetNodeView(MapView.SelectedNodes.First);
                ContextMenu.Show(MapView.Canvas, new Point((int)nodeView.Left + 2, (int)(nodeView.Top + nodeView.Height - 2)));
            }
        }

        void Canvas_NodeRightClick(MapNode node, NodeMouseEventArgs args)
        {
            ContextMenu.Show(MapView.Canvas, args.Location);
        }
    }
}
