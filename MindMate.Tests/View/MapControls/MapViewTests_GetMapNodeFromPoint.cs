using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.View.MapControls;
using System.Drawing;
using MindMate.Tests.TestDouble;
using System.Diagnostics;
using System.Linq;

namespace MindMate.Tests.View.MapControls
{
    [TestClass]
    public class MapViewTests_GetMapNodeFromPoint
    {
        [TestMethod]
        public void TestMethod1()
        {
            var t = new MapTree();
            var r = new MapNode(t, "1");
            AddFive(r, NodePosition.Right);
            AddFive(r, NodePosition.Left);

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            int count = 0;
            r.ForEach(n => {
                Assert.AreEqual(n, v.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top + 1)));
                Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Left - 1, (int)n.NodeView.Top + 1)));
                Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top - 1)));
                Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Right + 1, (int)n.NodeView.Top + 1)));
                Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Right - 1, (int)n.NodeView.Top - 1)));
                count++;
            });
            System.Diagnostics.Debug.WriteLine(count);
        }

        private void AddFive(MapNode parent, NodePosition pos, int level = 0)
        {
            if (level == 4) return;
            for (int i = 0; i < 5; i++)
            {
                AddFive(
                    new MapNode(parent, parent.Text + "-" + i, pos),
                    pos, level + 1);
            }
        }

        // test method on Algorithm version 1 (also uncomment MapViewTests_GetMapNodeFromPoint_AlgoVer1 class)
        // test method on Algorithm version 1 (also uncomment MapViewTests_GetMapNodeFromPoint_AlgoVer1 class)
        // test method on Algorithm version 1 (also uncomment MapViewTests_GetMapNodeFromPoint_AlgoVer1 class)
        //[TestMethod]
        //public void TestMethod2()
        //{
        //    var t = new MapTree();
        //    var r = new MapNode(t, "1");
        //    AddFive(r, NodePosition.Right);
        //    AddFive(r, NodePosition.Left);

        //    MetaModel.MetaModel.Initialize();
        //    var v = new MapView(t);

        //    int count = 0;
        //    r.ForEach(n =>
        //    {
        //        Assert.AreEqual(n, MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top + 1), r));
        //        Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Left - 1, (int)n.NodeView.Top + 1), r));
        //        Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top - 1), r));
        //        Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Right + 1, (int)n.NodeView.Top + 1), r));
        //        Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Right - 1, (int)n.NodeView.Top - 1), r));
        //        count++;
        //    });
        //    System.Diagnostics.Debug.WriteLine(count);
        //}



        [TestMethod]
        public void TestMethod1_SinglePoint()
        {
            var t = new MapTree();
            var r = new MapNode(t, "0");
            AddFive(r, NodePosition.Right);
            AddFive(r, NodePosition.Left);

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            MapNode n = r.ChildRightNodes.ElementAt(2).ChildNodes.ElementAt(1).ChildNodes.ElementAt(2);
            Assert.AreEqual(n, v.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top + 1)));
            Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Left - 1, (int)n.NodeView.Top + 1)));
            Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top - 1)));
            Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Right + 1, (int)n.NodeView.Top + 1)));
            Assert.IsNull(v.GetMapNodeFromPoint(new Point((int)n.NodeView.Right - 1, (int)n.NodeView.Top - 1)));
            
        }

        // test method on Algorithm version 1 (also uncomment MapViewTests_GetMapNodeFromPoint_AlgoVer1 class)
        // test method on Algorithm version 1 (also uncomment MapViewTests_GetMapNodeFromPoint_AlgoVer1 class)
        // test method on Algorithm version 1 (also uncomment MapViewTests_GetMapNodeFromPoint_AlgoVer1 class)
        //[TestMethod]
        //public void TestMethod2_SinglePoint()
        //{
        //    var t = new MapTree();
        //    var r = new MapNode(t, "0");
        //    AddFive(r, NodePosition.Right);
        //    AddFive(r, NodePosition.Left);

        //    MetaModel.MetaModel.Initialize();
        //    var v = new MapView(t);

        //    MapNode n = r.ChildRightNodes.ElementAt(2).ChildNodes.ElementAt(1).ChildNodes.ElementAt(2);
        //    Assert.AreEqual(n, MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top + 1), r));
        //    Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Left - 1, (int)n.NodeView.Top + 1), r));
        //    Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Left + 1, (int)n.NodeView.Top - 1), r));
        //    Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Right + 1, (int)n.NodeView.Top + 1), r));
        //    Assert.IsNull(MapViewTests_GetMapNodeFromPoint_AlgoVer1.GetMapNodeFromPoint(new Point((int)n.NodeView.Right - 1, (int)n.NodeView.Top - 1), r));

        //}


    }
}
