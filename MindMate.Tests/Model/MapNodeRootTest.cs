using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;

namespace MindMate.Tests.Model
{
    [TestClass]
    public class MapNodeRootTest
    {
        [TestMethod]
        public void MoveNodeUp()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            r.MoveUp();
        }

        [TestMethod]
        public void MoveNodeDown()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            r.MoveDown();
        }

        [TestMethod()]
        public void SortChildren_WithRootNodeSortABC_Asc()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            var c4 = new MapNode(r, "A");
            var c5 = new MapNode(r, "F");
            var c6 = new MapNode(r, "D");
            var c7 = new MapNode(r, "E");

            r.SortChildren((node1, node2) => string.CompareOrdinal(node1.Text, node2.Text));

            Assert.AreEqual(c4, r.FirstChild);
            Assert.AreEqual(c2, r.FirstChild.Next);
            Assert.AreEqual(c7, r.LastChild.Previous);
            Assert.AreEqual(c5, r.LastChild);
        }

        [TestMethod()]
        public void SortChildren_WithRootNodeSortABC_Desc()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            var c4 = new MapNode(r, "A");
            var c5 = new MapNode(r, "F");
            var c6 = new MapNode(r, "D");
            var c7 = new MapNode(r, "E");

            r.SortChildren((node1, node2) => string.CompareOrdinal(node2.Text, node1.Text));

            Assert.AreEqual(c5, r.FirstChild);
            Assert.AreEqual(c7, r.FirstChild.Next);
            Assert.AreEqual(c2, r.LastChild.Previous);
            Assert.AreEqual(c4, r.LastChild);
        }

        [TestMethod()]
        public void GetDescendentsCount()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            int count = r.GetDescendentsCount();

            Assert.AreEqual(13, count);
        }
    }
}
