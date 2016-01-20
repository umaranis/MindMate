using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests
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
    }
}