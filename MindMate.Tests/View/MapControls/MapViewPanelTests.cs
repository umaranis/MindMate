using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.MapControls
{
    [TestClass()]
    public class MapViewPanelTests
    {
        [TestMethod()]
        public void MapViewPanel()
        {
            MapTree tree = new MapTree();
            new MapNode(tree, "Root");
            
            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);
            
            view.Canvas.GetType().GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(view.Canvas, new object[] { new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)});
            view.Canvas.Dispose();

            Assert.IsNull(view.HighlightedNode);
        }        
    }
}