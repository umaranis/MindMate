using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.Tests.Stubs;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.Controller
{
    /// <summary>
    /// Testing Guidelines:
    /// 1- Test for root node
    /// 2- Test for scenario with no selected node
    /// </summary>
    [TestClass()]
    public class MapCtrlTests
    {
        private MapCtrl SetupMapCtrlWithFeaureDisplay()
        {
            string xmlString = System.IO.File.ReadAllText(@"Resources\Feature Display.mm");

            MapTree tree = new MapTree();
            new MindMapSerializer().Deserialize(xmlString, tree);

            tree.SelectedNodes.Add(tree.RootNode, false);

            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MetaModel.MetaModel.Instance.MapEditorBackColor = Color.White;
            MetaModel.MetaModel.Instance.NoteEditorBackColor = Color.White;
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), new MainCtrlStub(form));
            form.Controls.Add(mapCtrl.MapView.Canvas);

            tree.TurnOnChangeManager();

            return mapCtrl;
        }

        private MapCtrl SetupMapCtrlWithEmptyTree()
        {
            MapTree tree = new MapTree();
            MapNode root = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MetaModel.MetaModel.Instance.MapEditorBackColor = Color.White;
            MetaModel.MetaModel.Instance.NoteEditorBackColor = Color.White;
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), new MainCtrlStub(form));
            form.Controls.Add(mapCtrl.MapView.Canvas);

            tree.TurnOnChangeManager();

            return mapCtrl;
        }

        [TestMethod()]
        public void MapCtrl_WithFeatureDisplay()
        {
            Assert.IsNotNull(SetupMapCtrlWithFeaureDisplay());
        }

        [TestMethod()]
        public void MapCtrl_WithEmptyTree()
        {
            Assert.IsNotNull(SetupMapCtrlWithEmptyTree());
        }

        [TestMethod()]
        public void SelectAllNodes()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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

            mapCtrl.SelectAllNodes(false);

            Assert.AreEqual(7, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectAllNodes_Unfold()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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

            mapCtrl.SelectAllNodes(true);

            Assert.AreEqual(10, t.SelectedNodes.Count);
        }

        [TestMethod]
        public void SelectLevel_1UndoBatch()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Folded = true;
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            var undoStackCount = t.ChangeManager.UndoStackCount;

            mapCtrl.SelectLevel(2, false, true);

            Assert.AreEqual(undoStackCount + 1, t.ChangeManager.UndoStackCount);
        }

        [TestMethod]
        public void SelectLevel_CanvasLocationNoChange()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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
            var location = mapCtrl.MapView.Canvas.Location;

            mapCtrl.SelectLevel(2, false, true);

            Assert.AreEqual(location, mapCtrl.MapView.Canvas.Location);
        }


        [TestMethod()]
        public void SelectCurrentLevel_Level013()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectedNodes.Add(r, true);
            t.SelectedNodes.Add(c2, true);
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectCurrentLevel(false);

            Assert.AreEqual(5, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectCurrentLevel_Level013_Unfold()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            var c321 = new MapNode(c32, "c32");

            c32.Folded = true;

            t.SelectedNodes.Add(r, true);
            t.SelectedNodes.Add(c2, true);
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectCurrentLevel(true);

            Assert.AreEqual(6, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectCurrentLevel_Level013_SkipFolded()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            var c321 = new MapNode(c32, "c32");

            c32.Folded = true;

            t.SelectedNodes.Add(r, true);
            t.SelectedNodes.Add(c2, true);
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectCurrentLevel(false);

            Assert.AreEqual(5, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectCurrentLevel_Level023()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectedNodes.Add(r, true);
            t.SelectedNodes.Add(c31, true);
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectCurrentLevel(false);

            Assert.AreEqual(7, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectCurrentLevel_Level023WithFolded()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Folded = true;
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectedNodes.Add(r, true);
            t.SelectedNodes.Add(c31, true);
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectCurrentLevel(false);

            Assert.AreEqual(4, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectCurrentLevel_Level023WithFolded_Unfold()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Folded = true;
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            t.SelectedNodes.Add(r, true);
            t.SelectedNodes.Add(c31, true);
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectCurrentLevel(true);

            Assert.AreEqual(7, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectSiblings()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c12.Selected = true;

            mapCtrl.SelectSiblings();

            Assert.AreEqual(3, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectAncestors()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c13.Selected = true;
            t.SelectedNodes.Add(c311, true);

            mapCtrl.SelectAncestors();

            Assert.AreEqual(6, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectChildren()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c1.Selected = true;

            mapCtrl.SelectChildren(false);

            Assert.AreEqual(3, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectChildren_WithExpandSelection()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c1.Selected = true;

            mapCtrl.SelectChildren(true);

            Assert.AreEqual(4, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectDescendents()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c3.Selected = true;

            mapCtrl.SelectDescendents(false);

            Assert.AreEqual(4, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectDescendents_WithFolded_DonotSelectFolded()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c3.Selected = true;
            c31.Folded = true;

            mapCtrl.SelectDescendents(false);

            Assert.AreEqual(3, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectDescendents_WithFolded_Unfold()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c3.Selected = true;
            c31.Folded = true;

            mapCtrl.SelectDescendents(true);

            Assert.AreEqual(4, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectDescendents_Depth1()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c3.Selected = true;

            mapCtrl.SelectDescendents(1, false);

            Assert.AreEqual(3, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectDescendents_Depth2WithFolded()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c3.Selected = true;
            c31.Folded = true;

            mapCtrl.SelectDescendents(2, false);

            Assert.AreEqual(3, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectDescendents_Depth2WithFolded_Unfold()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            c3.Selected = true;
            c31.Folded = true;

            mapCtrl.SelectDescendents(2, true);

            Assert.AreEqual(4, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void FoldAll()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");

            mapCtrl.FoldAll();

            Assert.IsTrue(c3.Folded);
        }

        [TestMethod()]
        public void UnfoldAll()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            mapCtrl.FoldAll();

            mapCtrl.UnfoldAll();

            Assert.IsFalse(c3.Folded);
        }

        [TestMethod()]
        public void ToggleFolded()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            c3.Folded = true;
            c3.Selected = true;
            t.SelectedNodes.Add(c1, true);

            mapCtrl.ToggleFolded();

            Assert.IsFalse(c3.Folded);
            Assert.IsTrue(c1.Folded);
        }

        [TestMethod()]
        public void ToggleBranchFolding()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            c3.Folded = true;
            c31.Folded = true;
            c3.Selected = true;

            mapCtrl.ToggleBranchFolding();

            Assert.IsFalse(c3.Folded);
            Assert.IsFalse(c31.Folded);
        }

        [TestMethod()]
        public void UnfoldMapToCurrentLevel()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            c31.Selected = true;

            mapCtrl.UnfoldMapToCurrentLevel();

            Assert.IsTrue(c31.Folded);
            Assert.IsFalse(c1.Folded);
            Assert.IsTrue(c13.Folded);
        }

        [TestMethod()]
        public void UnfoldMapToCurrentLevel_Root()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            r.Selected = true;

            mapCtrl.UnfoldMapToCurrentLevel();

            Assert.IsFalse(r.Folded);
        }

        [TestMethod()]
        public void UnfoldMapToCurrentLevel_NoNodeSelected()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;

            mapCtrl.UnfoldMapToCurrentLevel();

            Assert.IsFalse(r.Folded);
        }

        [TestMethod()]
        public void UnfoldMapToLevel_Level1()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            c31.Selected = true;

            mapCtrl.UnfoldMapToLevel(1);

            Assert.IsTrue(c3.Folded);
            Assert.IsTrue(c1.Folded);
        }

        [TestMethod()]
        public void UnfoldMapToLevel_Level3()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            c31.Selected = true;

            mapCtrl.UnfoldMapToLevel(3);

            Assert.IsFalse(c3.Folded);
            Assert.IsFalse(c1.Folded);
            Assert.IsTrue(c311.Folded);
        }

        [TestMethod()]
        public void UnfoldMapToLevel_Level3_NoSelectedNode()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;

            mapCtrl.UnfoldMapToLevel(3);

            Assert.IsFalse(c3.Folded);
            Assert.IsFalse(c1.Folded);
            Assert.IsTrue(c311.Folded);
        }

        [TestMethod()]
        public void NavigateToCenter_EndNodeEditing()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            c3111.Selected = true;
            mapCtrl.BeginCurrentNodeEdit();

            mapCtrl.SelectRootNode();

            Assert.IsFalse(mapCtrl.MapView.NodeTextEditor.IsTextEditing);
        }

        [TestMethod()]
        public void NavigateToCenter_SelectRoot()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;

            mapCtrl.SelectRootNode();

            Assert.IsTrue(r.Selected);
        }

        [TestMethod()]
        public void SelectTopSibling()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            c3.Selected = true;

            mapCtrl.SelectTopSibling();

            Assert.IsTrue(c1.Selected);
            Assert.IsFalse(c3.Selected);
        }

        [TestMethod()]
        public void SelectBottomSibling()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            c2.Selected = true;

            mapCtrl.SelectBottomSibling();

            Assert.AreEqual(1, t.SelectedNodes.Count);
            Assert.IsTrue(c3.Selected);
        }

        [TestMethod()]
        public void SelectBottomSibling_WithRootSelected_NoChange()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c1.Folded = true;
            c31.Folded = true;
            r.Selected = true;

            mapCtrl.SelectBottomSibling();

            Assert.AreEqual(1, t.SelectedNodes.Count);
            Assert.IsTrue(r.Selected);
        }

        [TestMethod()]
        public void MoveNodeUp_NoSelection_NoChange()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");

            mapCtrl.MoveNodeUp();
        }

        [TestMethod()]
        public void MoveNodeUp()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c3.Selected = true;

            mapCtrl.MoveNodeUp();

            Assert.AreEqual(c3, c2.Previous);
        }

        [TestMethod()]
        public void MoveNodeDown()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c311 = new MapNode(c31, "c311");
            var c3111 = new MapNode(c311, "c3111");
            var c32 = new MapNode(c3, "c32");
            c2.Selected = true;

            mapCtrl.MoveNodeDown();

            Assert.AreEqual(c2, c3.Next);
        }

        //[TestMethod()]
        //public void EditHyperlink()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void BeginCurrentNodeEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void BeginNodeEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MultiLineNodeEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MultiLineNodeEdit1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void EndNodeEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void UpdateNodeText()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendNodeAndEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendChildNodeAndEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendChildNode()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendMultiLineNodeAndEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendSiblingNodeAndEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendSiblingAboveAndEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void InsertParentAndEdit()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DeleteSelectedNodes()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MoveNodeUp()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MoveNodeDown()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectAllSiblingsAbove()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectAllSiblingsBelow()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectNodeAbove()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectNodeBelow()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectNodeRightOrUnfold()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectNodeLeftOrUnfold()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ToggleFolded()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ToggleFolded1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RemoveLastIcon()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RemoveAllIcon()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendIcon()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AppendIconFromIconSelectorExt()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void FollowLink()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MakeSelectedNodeShapeBubble()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MakeSelectedNodeShapeBox()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MakeSelectedNodeShapeFork()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MakeSelectedNodeShapeBullet()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ToggleSelectedNodeItalic()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ToggleSelectedNodeBold()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ToggleSelectedNodeStrikeout()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetFontFamily()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetFontSize()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeLineWidth()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeLinePattern()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeLineColor()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeTextColorByPicker()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeTextColor()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeBackColorByPicker()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeBackColor()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ChangeFont()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Copy()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Paste()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Cut()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void MoveNodes()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetMapViewBackColor()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CopyFormat()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void EnableFormatMultiApply()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void PasteFormat()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ClearFormatPainter()
        //{
        //    Assert.Fail();
        //}
    }
}
