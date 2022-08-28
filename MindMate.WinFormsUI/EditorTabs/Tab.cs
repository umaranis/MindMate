using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.EditorTabs
{
    /// <summary>
    /// Derived class of TabPage for containing MapView
    /// </summary>
    public class Tab : TabBase, ICanvasContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapView">Must not be null</param>
        /// <param name="tree"></param>
        public Tab(MapView mapView, PersistentTree tree) : base(mapView.Canvas)
        {
            Tree = tree;
            MapView = mapView;
            tree.DirtyChanged += Tree_DirtyChanged;            
            AutoScroll = true;            
        }        

        private void Tree_DirtyChanged(PersistentTree tree)
        {
            UpdateTitle();
        }

        public MapView MapView { get; private set; }

        public PersistentTree Tree { get; private set; }

        public object ControllerTag { get; set; }

        public void UpdateTitle()
        {
            Text = Tree.RootNode.Text;

            if (Tree.IsDirty)
            {
                Text += "*";
            }
        }

        public override void Close()
        {
            base.Close();
        }

        
        // Overriden to avoid scrolling to start of the map when canvas is focussed
        protected override Point ScrollToControl(Control activeControl)
        {
            return DisplayRectangle.Location;
        }

        public void ScrollToPoint(int x, int y)
        {
            //breakpoints don't help with debugging scrollbar issues, as the behaviour is affected by breaks.
            //Console.WriteLine($"ScrollToPoint called with ({x},{y})");
            //Console.WriteLine($"Display Rect Before ScrollToPoint: {DisplayRectangle}");
            SetDisplayRectLocation(DisplayRectangle.X - x, DisplayRectangle.Y - y);            
            AdjustFormScrollbars(true);
            //Console.WriteLine($"Display Rect After ScrollToPoint: {DisplayRectangle}");
        }

    }    
}
