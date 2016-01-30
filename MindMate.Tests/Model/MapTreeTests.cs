using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class MapTreeTests
    {
        [TestMethod()]
        public void MapTree()
        {
            Assert.IsNull(new MapTree().RootNode);
        }

        [TestMethod()]
        public void GetAttributeSpec()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c13.AddAttribute("Speed", "120 kph");

            Assert.IsNotNull(t.GetAttributeSpec("Speed"));
        }

        [TestMethod()]
        public void GetClosestUnselectedNode()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            Assert.AreEqual(c13, t.GetClosestUnselectedNode(c12));
        }

        [TestMethod()]
        public void GetClosestUnselectedNode_NoSibling_GetParent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            Assert.AreEqual(c31, t.GetClosestUnselectedNode(c311));
        }

        [TestMethod()]
        public void GetClosestUnselectedNode_NoSiblingBelow_GetSiblingAbove()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            Assert.AreEqual(c31, t.GetClosestUnselectedNode(c32));
        }

        [TestMethod()]
        public void TurnOnChangeManager()
        {
            var t = new MapTree();

            t.TurnOnChangeManager();

            Assert.IsTrue(t.ChangeManagerOn);
        }

        [TestMethod()]
        public void TurnOffChangeManager()
        {
            var t = new MapTree();

            t.TurnOnChangeManager();
            t.TurnOffChangeManager();

            Assert.IsFalse(t.ChangeManagerOn);
        }

        [TestMethod()]
        public void SelectAllNodes()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectAllNodes();

            Assert.AreEqual(7, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectLevel_Level3()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectLevel(3, false, false);

            Assert.AreEqual(1, t.SelectedNodes.Count);
            Assert.IsTrue(c311.Selected);
        }

        [TestMethod()]
        public void SelectLevel_Level3Folded_NoSelection()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectLevel(3, false, false);

            Assert.AreEqual(0, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectLevel_Level3Folded_Unfold()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectLevel(3, false, true);

            Assert.AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectLevel_Level2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectLevel(2, false, false);

            Assert.AreEqual(3, t.SelectedNodes.Count);
            Assert.IsTrue(c12.Selected);
        }

        [TestMethod()]
        public void SelectLevel_Level2_Unfold()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectLevel(2, false, true);

            Assert.AreEqual(5, t.SelectedNodes.Count);
            Assert.IsTrue(c12.Selected);
        }

        [TestMethod()]
        public void SelectLevel_Level2_ExpandSelection()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            r.Selected = true;
            t.SelectLevel(2, true, false);

            Assert.AreEqual(6, t.SelectedNodes.Count);
            Assert.IsTrue(c12.Selected);
            Assert.IsTrue(r.Selected);
        }

        [TestMethod()]
        public void ExpandMapToLevel_Level1()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.UnfoldMapToLevel(1);

            Assert.IsTrue(c1.Folded);
            Assert.IsTrue(c3.Folded);
            Assert.IsFalse(c31.Folded);
            Assert.IsFalse(r.Folded);
        }

        [TestMethod()]
        public void ExpandMapToLevel_Level2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.UnfoldMapToLevel(2);

            Assert.IsFalse(c1.Folded);
            Assert.IsFalse(c3.Folded);
            Assert.IsTrue(c31.Folded);
            Assert.IsFalse(r.Folded);
        }

        [TestMethod()]
        public void ExpandMapToLevel_Level2WithLevel1Folded()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            t.UnfoldMapToLevel(1);

            t.UnfoldMapToLevel(2);

            Assert.IsFalse(c1.Folded);
            Assert.IsFalse(c3.Folded);
            Assert.IsTrue(c31.Folded);
            Assert.IsFalse(r.Folded);
        }
    }
}