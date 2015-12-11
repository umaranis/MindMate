using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.EditorTabs
{
    /// <summary>
    /// Derived class of TabPage for containing MapView
    /// </summary>
    public class Tab : TabBase
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
            Text = Tree.Tree.RootNode.Text;

            if (Tree.IsDirty)
            {
                Text += "*";
            }
        }

        public override void Close()
        {
            base.Close();
        }        
    }
}
