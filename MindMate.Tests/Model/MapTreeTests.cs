using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        [TestMethod()]
        public void RebalanceTree_AllRightNodes()
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
            var c4 = new MapNode(r, "c4", NodePosition.Right);
            var c5 = new MapNode(r, "c5", NodePosition.Right);
            var c6 = new MapNode(r, "c6", NodePosition.Right);
            var c7 = new MapNode(r, "c7", NodePosition.Right);

            t.RebalanceTree();

            Assert.IsTrue(Math.Abs(r.ChildRightNodes.Count() - r.ChildLeftNodes.Count()) <= 1, "Difference between left and right nodes count should be <= 1");
        }

        [TestMethod()]
        public void RebalanceTree_AllLeftNodes()
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
            var c4 = new MapNode(r, "c4", NodePosition.Left);
            var c5 = new MapNode(r, "c5", NodePosition.Left);
            var c6 = new MapNode(r, "c6", NodePosition.Left);
            var c7 = new MapNode(r, "c7", NodePosition.Left);

            t.RebalanceTree();

            Assert.IsTrue(Math.Abs(r.ChildRightNodes.Count() - r.ChildLeftNodes.Count()) <= 1, "Difference between left and right nodes count should be <= 1");
        }

        [TestMethod()]
        public void RebalanceTree_MixedPositions()
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
            var c2 = new MapNode(r, "c2", NodePosition.Right);
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            var c4 = new MapNode(r, "c4", NodePosition.Right);
            var c5 = new MapNode(r, "c5", NodePosition.Left);
            var c6 = new MapNode(r, "c6", NodePosition.Right);
            var c7 = new MapNode(r, "c7", NodePosition.Left);

            t.RebalanceTree();

            Assert.AreEqual(4, r.ChildLeftNodes.Count());
            Assert.IsTrue(Math.Abs(r.ChildRightNodes.Count() - r.ChildLeftNodes.Count()) <= 1, "Difference between left and right nodes count should be <= 1");
        }

        [TestMethod()]
        public void RebalanceTree_WithRightOverWeight()
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
            var c2 = new MapNode(r, "c2", NodePosition.Right);
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            var c4 = new MapNode(r, "c4", NodePosition.Right);
            var c5 = new MapNode(r, "c5", NodePosition.Right);
            var c6 = new MapNode(r, "c6", NodePosition.Right);
            var c7 = new MapNode(r, "c7", NodePosition.Right);

            t.RebalanceTree();

            Assert.IsTrue(Math.Abs(r.ChildRightNodes.Count() - r.ChildLeftNodes.Count()) <= 1, "Difference between left and right nodes count should be <= 1");
        }

        [TestMethod()]
        public void RebalanceTree_WithLeftOverWeight()
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
            var c2 = new MapNode(r, "c2", NodePosition.Right);
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            var c4 = new MapNode(r, "c4", NodePosition.Right);
            var c5 = new MapNode(r, "c5", NodePosition.Left);
            var c6 = new MapNode(r, "c6", NodePosition.Left);
            var c7 = new MapNode(r, "c7", NodePosition.Left);

            t.RebalanceTree();

            Assert.IsTrue(Math.Abs(r.ChildRightNodes.Count() - r.ChildLeftNodes.Count()) <= 1, "Difference between left and right nodes count should be <= 1");
        }

        [TestMethod()]
        public void SetLargeObject()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var lob = new BytesLob();
            lob.Bytes = new byte[3] { 3, 1, 2 };
            t.SetLargeObject("key1", lob);

            Assert.AreEqual(3, t.GetLargeObject<BytesLob>("key1").Bytes[0]);
        }

        [TestMethod()]
        public void SetLargeObject_Overwrite()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var lob = new BytesLob();
            lob.Bytes = new byte[3] { 3, 1, 2 };
            t.SetLargeObject("key1", lob);
            lob.Bytes = new byte[3] { 0, 1, 2 };
            t.SetLargeObject("key1", lob);

            Assert.AreEqual(0, t.GetLargeObject<BytesLob>("key1").Bytes[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetLargeObject_NonExistent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");            

            var a = t.GetLargeObject<BytesLob>("key1");
        }

        [TestMethod]
        public void TryGetLargeObject_NonExistent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            Assert.IsFalse(t.TryGetLargeObject<BytesLob>("key1", out BytesLob obj));
        }

        [TestMethod]
        public void TryGetLargeObject()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var lob = new BytesLob();
            lob.Bytes = new byte[3] { 3, 1, 2 };
            t.SetLargeObject("key1", lob);

            Assert.IsTrue(t.TryGetLargeObject<BytesLob>("key1", out BytesLob obj));
            Assert.AreEqual(2, obj.Bytes[2]);
        }

        [TestMethod()]
        public void RemoveLargeObject()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var lob = new BytesLob();
            lob.Bytes = new byte[3] { 3, 1, 2 };
            t.SetLargeObject("key1", lob);

            Assert.IsTrue(t.RemoveLargeObject("key1"));
        }

        [TestMethod()]
        public void RemoveLargeObject_Image()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var lob = new ImageLob();
            lob.Image = Image.FromFile(@"Resources\Feature Display.png");
            t.SetLargeObject("key1", lob);

            Assert.IsTrue(t.RemoveLargeObject("key1"));
        }

        [TestMethod()]
        public void RemoveLargeObject_NonExistent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var lob = new BytesLob();
            lob.Bytes = new byte[3] { 3, 1, 2 };
            t.SetLargeObject("key1", lob);

            Assert.IsFalse(t.RemoveLargeObject("key2"));
        }

        [TestMethod]
        public void MapNodes()
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
            var c32 = new MapNode(c3, "c32");

            Assert.AreEqual(9, t.MapNodes.Count());
        }

        [TestMethod]
        public void MapNodes_SingleChildNodes()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c111 = new MapNode(c11, "c111");
            var c1111 = new MapNode(c111, "c1111");
            var c11111 = new MapNode(c1111, "c11111");
            var c2 = new MapNode(r, "c3", NodePosition.Left);
            var c21 = new MapNode(c2, "c21");
            var c211 = new MapNode(c21, "c21");

            Assert.AreEqual(9, t.MapNodes.Count());
        }


        [TestMethod]
        public void MapNodes_LoadMap()
        {
            var manager = new PersistenceManager();
            var tree = manager.OpenTree(@"Resources\Feature Display.mm");

            Assert.AreEqual(tree.MapNodes.Count(), tree.RootNode.RollUpAggregate<int>(n => 1, (a, b) => a + b));
        }
    }
}