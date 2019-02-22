using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.View.MapControls.Interacting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MindMate.Tests.View.MapControls.Interacting
{
    [TestClass]
    public class MapTraverserTests
    {
        [TestMethod]
        public void TraverseDown()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n1.Selected = true;
            var sut = new MapTraverser();
            sut.TraverseDown(t, false);
            IsFalse(n1.Selected);
            IsTrue(n2.Selected);
        }

        [TestMethod]
        public void TraverseDown_ExpandSelection()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n1.Selected = true;
            var sut = new MapTraverser();
            sut.TraverseDown(t, true);
            IsTrue(n1.Selected);
            IsTrue(n2.Selected);
        }

        [TestMethod]
        public void TraverseDown_Deselect()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);            
            n2.AddToSelection();
            n1.AddToSelection();
            var sut = new MapTraverser();
            sut.TraverseDown(t, true);
            IsFalse(n1.Selected);
            IsTrue(n2.Selected);
        }

        [TestMethod]
        public void TraverseDown_Deselect_2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n2.AddToSelection();
            n1.AddToSelection();
            var sut = new MapTraverser();
            sut.TraverseDown(t, false);
            IsFalse(n1.Selected);
            IsTrue(n2.Selected);
        }

        [TestMethod]
        public void TraverseDown_NoSelection()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            sut.TraverseDown(t, false);
            IsFalse(n1.Selected);
            IsFalse(n2.Selected);
        }

        [TestMethod]
        public void TraverseDown_LastNode()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n3.Selected = true;
            sut.TraverseDown(t, false);
            IsFalse(n1.Selected);
            IsFalse(n2.Selected);
            IsTrue(n3.Selected);
        }

        [TestMethod]
        public void TraverseDown_LastNode_NodeBelowWithDifferentParent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n11.Selected = true;
            sut.TraverseDown(t, false);            
            IsTrue(n21.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseDown_LastNode_NodeBelowWithDifferentParent_Expand()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n11.Selected = true;
            sut.TraverseDown(t, true);
            IsTrue(n21.Selected);
            AreEqual(2, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseDown_LastNode_NodeBelowWithDifferentParent_Deselect()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n21.AddToSelection();
            n11.AddToSelection();
            sut.TraverseDown(t, true);
            IsTrue(n21.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseDown_LastNode_NodeBelowWithDifferentParent_Deselect_2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n21.AddToSelection();
            n11.AddToSelection();
            sut.TraverseDown(t, false);
            IsTrue(n21.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseUp()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n2.Selected = true;
            var sut = new MapTraverser();
            sut.TraverseUp(t, false);
            IsFalse(n2.Selected);
            IsTrue(n1.Selected);
        }

        [TestMethod]
        public void TraverseUp_ExpandSelection()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n2.Selected = true;
            var sut = new MapTraverser();
            sut.TraverseUp(t, true);
            IsTrue(n1.Selected);
            IsTrue(n2.Selected);
        }

        [TestMethod]
        public void TraverseUp_Deselect()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n1.AddToSelection();
            n2.AddToSelection();            
            var sut = new MapTraverser();
            sut.TraverseUp(t, true);
            IsFalse(n2.Selected);
            IsTrue(n1.Selected);
        }

        [TestMethod]
        public void TraverseUp_Deselect_2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            n1.AddToSelection();
            n2.AddToSelection();
            var sut = new MapTraverser();
            sut.TraverseUp(t, false);
            IsFalse(n2.Selected);
            IsTrue(n1.Selected);
        }

        [TestMethod]
        public void TraverseUp_NoSelection()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            sut.TraverseUp(t, false);
            IsFalse(n1.Selected);
            IsFalse(n2.Selected);
        }

        [TestMethod]
        public void TraverseUp_LastNode()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n1.Selected = true;
            sut.TraverseUp(t, false);
            IsTrue(n1.Selected);
            IsFalse(n2.Selected);
            IsFalse(n3.Selected);
        }

        [TestMethod]
        public void TraverseUp_LastNode_NodeBelowWithDifferentParent()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n21.Selected = true;
            sut.TraverseUp(t, false);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseUp_LastNode_NodeBelowWithDifferentParent_Expand()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n21.Selected = true;
            sut.TraverseUp(t, true);
            IsTrue(n11.Selected);
            AreEqual(2, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseUp_LastNode_NodeBelowWithDifferentParent_Deselect()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n11.AddToSelection();
            n21.AddToSelection();            
            sut.TraverseUp(t, true);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseUp_LastNode_NodeBelowWithDifferentParent_Deselect_2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n11.AddToSelection();
            n21.AddToSelection();
            sut.TraverseUp(t, false);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseRight()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n1.AddToSelection();            
            sut.TraverseRight(t);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseRight_Unfold()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n1.AddToSelection();
            n1.Folded = true;
            sut.TraverseRight(t);            
            AreEqual(1, t.SelectedNodes.Count);
            IsFalse(n1.Folded);
        }

        [TestMethod]
        public void TraverseRight_NothingOnRight()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n11.AddToSelection();            
            sut.TraverseRight(t);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);            
        }

        [TestMethod]
        public void TraverseRight_NothingSelected()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();            
            sut.TraverseRight(t);            
            AreEqual(0, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseRight_LeftNode()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Left);
            var n11 = new MapNode(n1, "n11", NodePosition.Left);
            var n2 = new MapNode(r, "n2", NodePosition.Left);
            var n21 = new MapNode(n2, "n21", NodePosition.Left);
            var n3 = new MapNode(r, "n3", NodePosition.Left);
            var sut = new MapTraverser();
            n11.AddToSelection();
            sut.TraverseRight(t);
            AreEqual(1, t.SelectedNodes.Count);
            IsTrue(n1.Selected);
        }

        [TestMethod]
        public void TraverseLeft()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Left);
            var n11 = new MapNode(n1, "n11", NodePosition.Left);
            var n2 = new MapNode(r, "n2", NodePosition.Left);
            var n21 = new MapNode(n2, "n21", NodePosition.Left);
            var n3 = new MapNode(r, "n3", NodePosition.Left);
            var sut = new MapTraverser();
            n1.AddToSelection();
            sut.TraverseLeft(t);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseLeft_Unfold()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Left);
            var n11 = new MapNode(n1, "n11", NodePosition.Left);
            var n2 = new MapNode(r, "n2", NodePosition.Left);
            var n21 = new MapNode(n2, "n21", NodePosition.Left);
            var n3 = new MapNode(r, "n3", NodePosition.Left);
            var sut = new MapTraverser();
            n1.AddToSelection();
            n1.Folded = true;
            sut.TraverseLeft(t);
            AreEqual(1, t.SelectedNodes.Count);
            IsFalse(n1.Folded);
        }

        [TestMethod]
        public void TraverseLeft_NothingOnLeft()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Left);
            var n11 = new MapNode(n1, "n11", NodePosition.Left);
            var n2 = new MapNode(r, "n2", NodePosition.Left);
            var n21 = new MapNode(n2, "n21", NodePosition.Left);
            var n3 = new MapNode(r, "n3", NodePosition.Left);
            var sut = new MapTraverser();
            n11.AddToSelection();
            sut.TraverseLeft(t);
            IsTrue(n11.Selected);
            AreEqual(1, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseLeft_NothingSelected()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Left);
            var n11 = new MapNode(n1, "n11", NodePosition.Left);
            var n2 = new MapNode(r, "n2", NodePosition.Left);
            var n21 = new MapNode(n2, "n21", NodePosition.Left);
            var n3 = new MapNode(r, "n3", NodePosition.Left);
            var sut = new MapTraverser();
            sut.TraverseLeft(t);
            AreEqual(0, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void TraverseLeft_LeftNode()
        {
            MetaModel.MetaModel.Initialize();
            var t = new MapTree();
            var r = new MapNode(t, "r");
            var n1 = new MapNode(r, "n1", NodePosition.Right);
            var n11 = new MapNode(n1, "n11", NodePosition.Right);
            var n2 = new MapNode(r, "n2", NodePosition.Right);
            var n21 = new MapNode(n2, "n21", NodePosition.Right);
            var n3 = new MapNode(r, "n3", NodePosition.Right);
            var sut = new MapTraverser();
            n11.AddToSelection();
            sut.TraverseLeft(t);
            AreEqual(1, t.SelectedNodes.Count);
            IsTrue(n1.Selected);
        }
    }
}
