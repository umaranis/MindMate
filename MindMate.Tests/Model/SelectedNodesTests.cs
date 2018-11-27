using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class SelectedNodesTests
    {
        [TestMethod()]
        public void SelectedNodes()
        {
            var s = new SelectedNodes();
            Assert.AreEqual(0, s.Count);
        }

        [TestMethod]
        public void Add_Null_NoImpact()
        {
            var s = new SelectedNodes();
            s.Add(null);

            Assert.AreEqual(0, s.Count);
        }

        [TestMethod()]
        public void Add_ReAddingNode_OneSelectedEvent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            int eventSelectCount = 0;
            t.SelectedNodes.NodeSelected += (node, nodes) => eventSelectCount++;
            t.SelectedNodes.Add(r);
            t.SelectedNodes.Add(r);

            Assert.AreEqual(1, eventSelectCount);
        }

        [TestMethod()]
        public void Add_ReAddingNodeWithExpandSelection_OneSelectedEvent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            int eventSelectCount = 0;
            t.SelectedNodes.NodeSelected += (node, nodes) => eventSelectCount++;
            t.SelectedNodes.Add(r);
            t.SelectedNodes.Add(r, true);

            Assert.AreEqual(1, eventSelectCount);
        }

        [TestMethod()]
        public void Add_ReAddingNode_NoDeselectedEvent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            int eventDeselectCount = 0;
            t.SelectedNodes.NodeDeselected += (node, nodes) => eventDeselectCount++;
            t.SelectedNodes.Add(r);
            t.SelectedNodes.Add(r);

            Assert.AreEqual(0, eventDeselectCount);
        }

        [TestMethod()]
        public void Clear()
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
            t.SelectAllNodes();

            int count = 0;
            t.SelectedNodes.NodeDeselected += (node, nodes) => count++;

            t.SelectedNodes.Clear();

            Assert.AreEqual(12, count);
        }

        [TestMethod()]
        public void Remove()
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
            t.SelectAllNodes();

            int count = 0;
            t.SelectedNodes.NodeDeselected += (node, nodes) => count++;

            t.SelectedNodes.Remove(c131);

            Assert.AreEqual(1, count);
        }

        [TestMethod()]
        public void Remove_NotSelected_NoEvent()
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
            t.SelectAllNodes();

            int count = 0;
            t.SelectedNodes.NodeDeselected += (node, nodes) => count++;

            t.SelectedNodes.Remove(c131);
            t.SelectedNodes.Remove(c131);

            Assert.AreEqual(1, count);
        }

        [TestMethod()]
        public void RemoveAt()
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
            r.Selected = true;

            t.SelectedNodes.RemoveAt(0);

            Assert.IsNull(t.SelectedNodes.Last);
        }

        [TestMethod()]
        public void Contains()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");

            r.Selected = true;

            Assert.IsTrue(t.SelectedNodes.Contains(r));
        }

        [TestMethod()]
        public void GetEnumerator_IEnumerable()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");

            r.Selected = true;

            var iterator = ((IEnumerable) (t.SelectedNodes)).GetEnumerator();
            iterator.MoveNext();

            Assert.AreEqual(r, iterator.Current); 
        }

        [TestMethod()]
        public void GetEnumerator()
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

            t.SelectAllNodes();

            int count = 0;
            foreach (var node in t.SelectedNodes)
            {
                count++;
            }

            Assert.AreEqual(12, count);
        }

        [TestMethod()]
        public void ExcludeNodesAlreadyPartOfHierarchy()
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

            t.SelectAllNodes();

            Assert.AreEqual(1, t.SelectedNodes.ExcludeNodesAlreadyPartOfHierarchy().Count());
        }

        [TestMethod()]
        public void ExcludeNodesAlreadyPartOfHierarchy_Level1()
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

            t.SelectAllNodes();
            r.Selected = false;

            Assert.AreEqual(11, t.SelectedNodes.Count);
            Assert.AreEqual(3, t.SelectedNodes.ExcludeNodesAlreadyPartOfHierarchy().Count());
        }
    }
}