using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using System.Drawing;

namespace MindMate.Tests.View.MapControls
{
    [TestClass()]
    public class MapViewTests
    {
        [TestMethod()]
        public void MapView_Create()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            Assert.IsNotNull(v.Tree);
        }


        [TestMethod()]
        public void GetMapNodeFromPoint_RootNode()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var rv = v.GetNodeView(r);

            var result = v.GetMapNodeFromPoint(new Point((int)rv.Left + 1, (int)rv.Top + 1));

            Assert.AreEqual(r, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_RootNodeBottomRightCorner()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var rv = v.GetNodeView(r);

            var result = v.GetMapNodeFromPoint(new Point((int)rv.Right - 1, (int)rv.Bottom - 1));

            Assert.AreEqual(r, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_AboveRootNode_Null()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var rv = v.GetNodeView(r);

            var result = v.GetMapNodeFromPoint(new Point((int)rv.Left + 1, (int)rv.Top - 1));

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_BelowRootNode_Null()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var rv = v.GetNodeView(r);

            var result = v.GetMapNodeFromPoint(new Point((int)rv.Left + 1, (int)rv.Bottom + 1));

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_JustLeftOfRoot_Null()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var rv = v.GetNodeView(r);

            var result = v.GetMapNodeFromPoint(new Point((int)rv.Left - 1, (int)rv.Top + 1));

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_JustRightOfRoot_Null()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var rv = v.GetNodeView(r);

            var result = v.GetMapNodeFromPoint(new Point((int)rv.Right + 1, (int)rv.Top + 1));

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c1()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c1v = v.GetNodeView(c1);

            var result = v.GetMapNodeFromPoint(new Point((int)c1v.Left + 1, (int)c1v.Top + 1));

            Assert.AreEqual(c1, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c11()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c11v = v.GetNodeView(c11);

            var result = v.GetMapNodeFromPoint(new Point((int)c11v.Left + 1, (int)c11v.Top + 1));

            Assert.AreEqual(c11, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c12()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c12v = v.GetNodeView(c12);

            var result = v.GetMapNodeFromPoint(new Point((int)c12v.Left + 1, (int)c12v.Top + 1));

            Assert.AreEqual(c12, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c121()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c121v = v.GetNodeView(c121);

            var result = v.GetMapNodeFromPoint(new Point((int)c121v.Left + 1, (int)c121v.Top + 1));

            Assert.AreEqual(c121, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c13()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c13v = v.GetNodeView(c13);

            var result = v.GetMapNodeFromPoint(new Point((int)c13v.Left + 1, (int)c13v.Top + 1));

            Assert.AreEqual(c13, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c2v = v.GetNodeView(c2);

            var result = v.GetMapNodeFromPoint(new Point((int)c2v.Left + 1, (int)c2v.Top + 1));

            Assert.AreEqual(c2, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c3()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c3v = v.GetNodeView(c3);

            var result = v.GetMapNodeFromPoint(new Point((int)c3v.Left + 1, (int)c3v.Top + 1));

            Assert.AreEqual(c3, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_c31()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c31v = v.GetNodeView(c31);

            var result = v.GetMapNodeFromPoint(new Point((int)c31v.Left + 1, (int)c31v.Top + 1));

            Assert.AreEqual(c31, result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_ForFoldedNode()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            c3.Folded = true;

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c31v = v.GetNodeView(c31);

            var result = v.GetMapNodeFromPoint(new Point((int)c31v.Left + 1, (int)c31v.Top + 1));

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_RightNodesOnly()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1", NodePosition.Right);
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2", NodePosition.Right);
            var c3 = new MapNode(r, "c3", NodePosition.Right);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            var result = v.GetMapNodeFromPoint(new Point((int)c32.NodeView.Left + 1, (int)c32.NodeView.Bottom + 1));
            Assert.IsNull(result);
            result = v.GetMapNodeFromPoint(new Point((int)r.NodeView.Left - 1, (int)r.NodeView.Bottom + 1));
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetMapNodeFromPoint_LeftNodesOnly()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1", NodePosition.Left);
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2", NodePosition.Left);
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);
            var c32v = v.GetNodeView(c32);

            var result = v.GetMapNodeFromPoint(new Point((int)c32v.Left + 1, (int)c32v.Bottom + 1));

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void AddNote()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            t.SelectedNodes.Add(c32, true);

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            c32.NoteText = "Sample Note";

            Assert.IsNotNull(c32.NodeView.NoteIcon);
        }

        [TestMethod()]
        public void RemoveNote()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            t.SelectedNodes.Add(c32, true);

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            c32.NoteText = "Sample Note";
            c32.NoteText = null;

            Assert.IsNull(c32.NodeView.NoteIcon);
        }

        [TestMethod]
        public void ExpandCanvas_Right()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            Assert.AreEqual(v.Canvas.Width, MapView.CANVAS_DEFAULT_WIDTH);
            Assert.AreEqual(v.Canvas.Height, MapView.CANVAS_DEFAULT_HEIGHT);

            var parent = r;
            for (int i = 0; i < 40; i++)    parent = new MapNode(parent, $"n{i + 1}", NodePosition.Right);

            Assert.AreEqual(MapView.CANVAS_DEFAULT_WIDTH + MapView.CANVAS_SIZE_INCREMENT, v.Canvas.Width);
            Assert.AreEqual(MapView.CANVAS_DEFAULT_HEIGHT + MapView.CANVAS_SIZE_INCREMENT, v.Canvas.Height);

        }

        [TestMethod]
        public void ExpandCanvas_Left()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            MetaModel.MetaModel.Initialize();
            var v = new MapView(t);

            Assert.AreEqual(v.Canvas.Width, MapView.CANVAS_DEFAULT_WIDTH);
            Assert.AreEqual(v.Canvas.Height, MapView.CANVAS_DEFAULT_HEIGHT);

            var parent = r;
            for (int i = 0; i < 40; i++) parent = new MapNode(parent, $"n{i + 1}", NodePosition.Left);

            Assert.AreEqual(MapView.CANVAS_DEFAULT_WIDTH + MapView.CANVAS_SIZE_INCREMENT, v.Canvas.Width);
            Assert.AreEqual(MapView.CANVAS_DEFAULT_HEIGHT + MapView.CANVAS_SIZE_INCREMENT, v.Canvas.Height);

        }

        //[TestMethod()]
        //public void GetMouseOffset()
        //{
        //    Assert.Fail();
        //}
    }
}