using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using MindMate.Plugins.Tasks.Model;
using MindMate.Tests.TestDouble;
using MindMate.View.Dialogs;
using System.Windows.Forms;
using MindMate.MetaModel;

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
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
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
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
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

        [TestMethod()]
        public void SortAlphabeticallyAsc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            c1.Selected = true;

            mapCtrl.SortAlphabeticallyAsc();

            Assert.AreEqual(c15, c1.FirstChild);
            Assert.AreEqual(c12, c1.FirstChild.Next);
            Assert.AreEqual(c11, c1.LastChild.Previous);
            Assert.AreEqual(c14, c1.LastChild);
        }

        [TestMethod()]
        public void SortAlphabeticallyDesc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            c1.Selected = true;

            mapCtrl.SortAlphabeticallyDesc();

            Assert.AreEqual(c14, c1.FirstChild);
            Assert.AreEqual(c11, c1.FirstChild.Next);
            Assert.AreEqual(c12, c1.LastChild.Previous);
            Assert.AreEqual(c15, c1.LastChild);
        }

        [TestMethod()]
        public void SortByTaskAsc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6"); c11.AddTask(DateTime.Now);
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7"); c14.AddTask(DateTime.Now.AddSeconds(5));
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            c1.Selected = true;

            mapCtrl.SortByTaskAsc();

            Assert.AreEqual(c12, c1.FirstChild);
            Assert.AreEqual(c11, c1.LastChild.Previous);
            Assert.AreEqual(c14, c1.LastChild);
        }

        [TestMethod()]
        public void SortByTaskDesc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6"); c11.AddTask(DateTime.Now);
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7"); c14.AddTask(DateTime.Now.AddSeconds(5));
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            c1.Selected = true;

            mapCtrl.SortByTaskDesc();

            Assert.AreEqual(c17, c1.LastChild);
            Assert.AreEqual(c11, c1.FirstChild.Next);
            Assert.AreEqual(c14, c1.FirstChild);
        }

        [TestMethod()]
        public void SortByDescendentsCountAsc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            r.Selected = true;

            mapCtrl.SortByDescendentsCountAsc();

            Assert.AreEqual(c2, r.FirstChild);
            Assert.AreEqual(c1, r.LastChild);
        }

        [TestMethod()]
        public void SortByDescendentsCountDesc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "B");
            var c3 = new MapNode(r, "C", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            r.Selected = true;

            mapCtrl.SortByDescendentsCountDesc();

            Assert.AreEqual(c2, r.LastChild);
            Assert.AreEqual(c1, r.FirstChild);
        }

        [TestMethod()]
        public void SortByCreateDateCountAsc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "B");
            c2.Created = DateTime.Now.AddSeconds(5);
            var c3 = new MapNode(r, "C", NodePosition.Left);
            c3.Created = DateTime.Now.AddSeconds(10);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            r.Selected = true;

            mapCtrl.SortByCreateDateAsc();

            Assert.AreEqual(c3, r.LastChild);
            Assert.AreEqual(c1, r.FirstChild);
        }

        [TestMethod()]
        public void SortByCreateDateCountDesc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "C");
            c1.Created = DateTime.Now;
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "B");
            c2.Created = DateTime.Now.AddSeconds(5);
            var c3 = new MapNode(r, "C", NodePosition.Left);
            c3.Created = DateTime.Now.AddSeconds(10);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            r.Selected = true;

            mapCtrl.SortByCreateDateDesc();

            Assert.AreEqual(c1, r.LastChild);
            Assert.AreEqual(c3, r.FirstChild);
        }

        [TestMethod()]
        public void SortByModifiedDateCountAsc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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
            c2.Modified = DateTime.Now.AddSeconds(5);
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            c3.Modified = DateTime.Now.AddSeconds(10);
            r.Selected = true;

            mapCtrl.SortByModifiedDateAsc();

            Assert.AreEqual(c3, r.LastChild);
            Assert.AreEqual(c1, r.FirstChild);
        }

        [TestMethod()]
        public void SortByModifiedDateCountDesc()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Modified = DateTime.Now;
            var c11 = new MapNode(c1, "6");
            var c12 = new MapNode(c1, "2");
            var c13 = new MapNode(c1, "4");
            var c14 = new MapNode(c1, "7");
            var c15 = new MapNode(c1, "1");
            var c16 = new MapNode(c1, "5");
            var c17 = new MapNode(c1, "3");
            var c121 = new MapNode(c12, "c121");
            var c2 = new MapNode(r, "c2");
            c2.Modified = DateTime.Now.AddSeconds(5);
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");
            c3.Modified = DateTime.Now.AddSeconds(10);
            r.Selected = true;

            mapCtrl.SortByModifiedDateDesc();

            Assert.AreEqual(c1, r.LastChild);
            Assert.AreEqual(c3, r.FirstChild);
        }

        [TestMethod()]
        public void AddHyperlink()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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
            r.Selected = true;
            t.SelectedNodes.Add(c32, true);

            mapCtrl.AddHyperlink("abc");

            Assert.AreEqual("abc", r.Link);
            Assert.AreEqual("abc", c32.Link);
            Assert.IsNull(c31.Link);
        }

        [TestMethod()]
        public void ChangeHyperlink()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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
            r.Selected = true;
            t.SelectedNodes.Add(c32, true);

            mapCtrl.AddHyperlink("abc");
            mapCtrl.AddHyperlink("xyz");

            Assert.AreEqual("xyz", r.Link);
            Assert.AreEqual("xyz", c32.Link);
            Assert.IsNull(c31.Link);
        }

        [TestMethod()]
        public void RemoveHyperlink()
        {
            MapCtrl mapCtrl = SetupMapCtrlWithEmptyTree();
            var t = mapCtrl.MapView.Tree;
            var r = t.RootNode;
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
            r.Selected = true;
            t.SelectedNodes.Add(c32, true);
            mapCtrl.AddHyperlink("abc");

            mapCtrl.RemoveHyperlink();

            Assert.IsNull(r.Link);
            Assert.IsNull(c32.Link);
            Assert.IsNull(c31.Link);
        }
        
        [TestMethod()]
        public void ChangeNodeShapeFork()
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
            r.AddToSelection();

            mapCtrl.ChangeNodeShapeFork();

            Assert.AreEqual(NodeShape.Fork, r.Shape);
        }

        [TestMethod()]
        public void ChangeNodeShapeBubble()
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
            r.AddToSelection();

            mapCtrl.ChangeNodeShapeBubble();

            Assert.AreEqual(NodeShape.Bubble, r.Shape);
        }

        [TestMethod()]
        public void ChangeNodeShapeBox()
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
            c32.AddToSelection();

            mapCtrl.ChangeNodeShapeBox();

            Assert.AreEqual(NodeShape.Box, c32.Shape);
        }

        [TestMethod()]
        public void ChangeNodeShapeBullet()
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
            c32.AddToSelection();

            mapCtrl.ChangeNodeShapeBullet();

            Assert.AreEqual(NodeShape.Bullet, c32.Shape);
        }

        [TestMethod()]
        public void ChangeNodeShapeBullet_ChangeManagerUndoCount()
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
            c32.AddToSelection();
            c311.AddToSelection();
            var undoCount = t.ChangeManager.UndoStackCount;

            mapCtrl.ChangeNodeShapeBullet();

            Assert.AreEqual(undoCount + 1, t.ChangeManager.UndoStackCount);
        }

        [TestMethod()]
        public void ChangeNodeShapeBullet_NoSelection()
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
            var undoCount = t.ChangeManager.UndoStackCount;

            mapCtrl.ChangeNodeShapeBullet();

            Assert.AreEqual(undoCount, t.ChangeManager.UndoStackCount);
        }

        [TestMethod()]
        public void ClearNodeShape()
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
            c32.AddToSelection();
            c3111.AddToSelection();
            mapCtrl.ClearNodeShape();

            Assert.AreEqual(NodeShape.None, c32.Shape);
        }

        [TestMethod()]
        public void ChangeNodeShape()
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
            c32.AddToSelection();

            mapCtrl.ChangeNodeShape(NodeShape.Bullet);

            Assert.AreEqual(NodeShape.Bullet, c32.Shape);
        }

        [TestMethod()]
        public void ChangeNodeShape_StringParameter()
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
            c32.AddToSelection();

            mapCtrl.ChangeNodeShape("Bubble");

            Assert.AreEqual(NodeShape.Bubble, c32.Shape);
        }

        [TestMethod()]
        public void ChangeLineColor()
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
            c1.AddToSelection();
            c2.AddToSelection();

            mapCtrl.ChangeLineColor(Color.Brown);

            Assert.AreEqual(Color.Brown, c1.LineColor);
            Assert.AreEqual(Color.Brown, c2.LineColor);
            Assert.AreEqual(Color.Empty, c3.LineColor);
        }

        [TestMethod()]
        public void ChangeLineColorUsingPicker()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            var dialogs = A.Fake<DialogManager>();
            A.CallTo(() => dialogs.ShowColorPicker(Color.Empty)).WithAnyArguments().Returns(Color.Chocolate);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();

            mapCtrl.ChangeLineColorUsingPicker();

            Assert.AreEqual(Color.Chocolate, r.LineColor);

        }

        [TestMethod()]
        public void ChangeLinePattern()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();

            mapCtrl.ChangeLinePattern(DashStyle.Dash);

            Assert.AreEqual(DashStyle.Dash, r.LinePattern);
        }

        [TestMethod()]
        public void ChangeLineWidth()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();

            mapCtrl.ChangeLineWidth(4);

            Assert.AreEqual(4, r.LineWidth);
        }

        [TestMethod()]
        public void CreateNodeStyle_NodeStylesCountGoesUp()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            var dialogs = A.Fake<DialogManager>();
            A.CallTo(() => dialogs.ShowInputBox("Enter the style name:", null)).Returns(DateTime.Now.Ticks.ToString());
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            int count = MetaModel.MetaModel.Instance.NodeStyles.Count;

            mapCtrl.CreateNodeStyle();

            Assert.AreEqual(count + 1, MetaModel.MetaModel.Instance.NodeStyles.Count);
        }

        [TestMethod()]
        public void CreateNodeStyle_NullIfNothingSelected()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();

            var style = mapCtrl.CreateNodeStyle();

            Assert.IsNull(style);
        }

        [TestMethod()]
        public void CreateNodeStyle_NullIfMultipleSelected()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c1.AddToSelection();

            var style = mapCtrl.CreateNodeStyle();

            Assert.IsNull(style);
        }

        [TestMethod()]
        public void ApplyNodeStyle()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            r.FontSize = 15;
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            var dialogManager = A.Fake<DialogManager>();
            A.CallTo(() => dialogManager.ShowInputBox("Enter the style name:", null)).Returns("ApplyNodeStyle_" + Guid.NewGuid());
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogManager, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            var style = mapCtrl.CreateNodeStyle();
            r.Selected = false;
            c1.AddToSelection();
            c2.AddToSelection();

            mapCtrl.ApplyNodeStyle(style);

            Assert.AreEqual(15, c1.FontSize);
            Assert.AreEqual(15, c2.FontSize);
        }

        [TestMethod()]
        public void ChangeBackColor()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            r.FontSize = 15;
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();

            mapCtrl.ChangeBackColor(Color.Aqua);

            c2.AddToSelection();

            Assert.AreEqual(c1.BackColor, Color.Empty);
            Assert.AreEqual(c2.BackColor, Color.Empty);
            Assert.AreEqual(r.BackColor, Color.Aqua);

        }

        [TestMethod()]
        public void ClearFormatting()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            r.FontSize = 15;
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            mapCtrl.ChangeBackColor(Color.Aqua);
            c2.Selected = false;

            mapCtrl.ClearFormatting();

            Assert.AreEqual(c1.BackColor, Color.Empty);
            Assert.AreEqual(c2.BackColor, Color.Aqua);
            Assert.AreEqual(r.BackColor, Color.Empty);
        }

        [TestMethod()]
        public void ChangeStrikeout_MultiSelect()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            r.FontSize = 15;
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            c2.Strikeout = true;
            mapCtrl.ChangeStrikeout(false);
            
            Assert.IsFalse(c2.Strikeout);
            Assert.IsFalse(r.Strikeout);
        }

        [TestMethod()]
        public void ToggleStrikeout()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            c2.Strikeout = true;
            mapCtrl.ToggleStrikeout();

            Assert.IsFalse(c2.Strikeout);
            Assert.IsTrue(r.Strikeout);
        }


        [TestMethod()]
        public void ChangeBold()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            r.FontSize = 15;
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            c2.Bold = true;
            mapCtrl.ChangeBold(true);

            Assert.IsTrue(c2.Bold);
            Assert.IsTrue(r.Bold);
        }

        [TestMethod()]
        public void ToggleBold()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            c2.Bold = true;
            mapCtrl.ToggleBold();

            Assert.IsFalse(c2.Bold);
            Assert.IsTrue(r.Bold);
        }

        [TestMethod()]
        public void ChangeItalic()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            c2.Italic = true;
            mapCtrl.ChangeItalic(true);

            Assert.IsTrue(c2.Italic);
            Assert.IsTrue(r.Italic);
        }

        [TestMethod()]
        public void ToggleItalic()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            r.AddToSelection();
            c2.AddToSelection();
            c2.Italic = true;
            mapCtrl.ToggleItalic();

            Assert.IsFalse(c2.Italic);
            Assert.IsTrue(r.Italic);
        }

        [TestMethod()]
        public void DeleteSelectedNodes()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "SeekDeleteConfirmation").WithReturnType<bool>().Returns(true);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Selected = true;
            c1.AddToSelection();
            mapCtrl.DeleteSelectedNodes();

            Assert.AreEqual(0, r.ChildNodes.Count());
        }

        [TestMethod()]
        public void DeleteSelectedNodes_UserInputCancel()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "SeekDeleteConfirmation").WithReturnType<bool>().Returns(true);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Selected = true;
            mapCtrl.DeleteSelectedNodes();

            Assert.AreEqual(1, r.ChildNodes.Count());
        }

        [TestMethod()]
        public void InsertAndRemoveImage()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(() => dialogs.GetImageFile()).Returns(@"Resources\TestImage.png");
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Selected = true;
            c1.AddToSelection();

            mapCtrl.InsertImage();
            Assert.AreEqual(2, tree.RootNode.RollUpAggregate<int>(n => n.HasImage ? 1 : 0, (a, b) => a + b));

            mapCtrl.RemoveImage();
            Assert.AreEqual(0, tree.RootNode.RollUpAggregate<int>(n => n.HasImage ? 1 : 0, (a, b) => a + b));
        }

        [TestMethod()]
        public void IncreaseImageSize()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(() => dialogs.GetImageFile()).Returns(@"Resources\TestImage.png");
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Selected = true;
            c1.AddToSelection();

            mapCtrl.InsertImage();
            Size sc1 = c1.ImageSize;
            Size sc2 = c2.ImageSize;
            mapCtrl.IncreaseImageSize();

            Assert.IsTrue(sc1.Height < c1.ImageSize.Height);
            Assert.IsTrue(sc1.Width < c1.ImageSize.Width);
            Assert.IsTrue(sc2.Height < c2.ImageSize.Height);
            Assert.IsTrue(sc2.Width < c2.ImageSize.Width);
            Assert.IsTrue(r.ImageSize.IsEmpty);

        }

        [TestMethod()]
        public void DecreaseImageSize()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(() => dialogs.GetImageFile()).Returns(@"Resources\TestImage.png");
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Selected = true;
            c1.AddToSelection();

            mapCtrl.InsertImage();
            Size sc1 = c1.ImageSize;
            Size sc2 = c2.ImageSize;
            mapCtrl.DecreaseImageSize();

            Assert.IsTrue(sc1.Height > c1.ImageSize.Height);
            Assert.IsTrue(sc1.Width > c1.ImageSize.Width);
            Assert.IsTrue(sc2.Height > c2.ImageSize.Height);
            Assert.IsTrue(sc2.Width > c2.ImageSize.Width);
            Assert.IsTrue(r.ImageSize.IsEmpty);

        }

        [TestMethod()]
        public void SetImageAlignment()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "SeekDeleteConfirmation").WithReturnType<bool>().Returns(true);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Selected = true;
            mapCtrl.ImageAlignStart();
            mapCtrl.ImagePosAbove();
            Assert.AreEqual(ImageAlignment.AboveStart, c2.ImageAlignment);
            mapCtrl.ImagePosBelow();
            Assert.AreEqual(ImageAlignment.BelowStart, c2.ImageAlignment);
            mapCtrl.ImagePosBefore();
            mapCtrl.ImagePosAfter();
            Assert.AreEqual(ImageAlignment.AfterTop, c2.ImageAlignment);
            mapCtrl.ImageAlignEnd();
            mapCtrl.ImageAlignStart();
            mapCtrl.ImageAlignCenter();
            Assert.AreEqual(ImageAlignment.AfterCenter, c2.ImageAlignment);
        }

        [TestMethod()]
        public void SetSelectedNodeFormatAsDefault()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "SeekDeleteConfirmation").WithReturnType<bool>().Returns(true);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            c2.Shape = NodeShape.Bubble;
            c2.Selected = true;
            mapCtrl.SetSelectedNodeFormatAsDefault();
            Assert.AreEqual(NodeShape.Bubble, c1.NodeView.NodeFormat.Shape);
            tree.ChangeManager.Undo();
            Assert.AreNotEqual(NodeShape.Bubble, c1.NodeView.NodeFormat.Shape);            
        }

        [TestMethod()]
        public void SetDefaultFormatDialog_Cancel()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "ShowDefaultFormatSettingsDialog").WithReturnType<DialogResult>().Returns(DialogResult.Cancel);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();            
            mapCtrl.SetDefaultFormatDialog();
            Assert.AreEqual(NodeShape.Fork, c1.NodeView.NodeFormat.Shape);
        }

        [TestMethod()]
        public void SetDefaultFormatDialog_OK()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "ShowDefaultFormatSettingsDialog").WithReturnType<DialogResult>().Invokes(a =>
            {
                var f = (DefaultFormatSettings)a.Arguments[0];
                f.Prop_NodeShape = NodeShape.Bubble;
            }).Returns<DialogResult>(DialogResult.OK);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            tree.TurnOnChangeManager();
            mapCtrl.SetDefaultFormatDialog();
            Assert.AreEqual(NodeShape.Bubble, c1.NodeView.NodeFormat.Shape);
        }

        [TestMethod()]
        public void SetDefaultFormatDialog_ChangeAll()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();
            A.CallTo(dialogs).Where(call => call.Method.Name == "ShowDefaultFormatSettingsDialog").WithReturnType<DialogResult>().Invokes(a =>
            {
                var f = (DefaultFormatSettings)a.Arguments[0];
                f.Prop_NodeShape = NodeShape.Bubble;
                f.Prop_BackColor = Color.AliceBlue;
                f.Prop_DropHintColor = Color.Aquamarine;
                f.Prop_Font = new Font(FontFamily.GenericMonospace, 14, FontStyle.Bold);
                f.Prop_LineColor = Color.Chocolate;
                f.Prop_LinePattern = DashStyle.DashDotDot;
                f.Prop_LineWidth = 4;
                f.Prop_MapBackColor = Color.LightCyan;
                f.Prop_NoteEditorBackColor = Color.LightGreen;
                f.Prop_SelectedOutlineColor = Color.DarkKhaki;
                f.Prop_TextColor = Color.DarkGoldenrod;                
            }).Returns<DialogResult>(DialogResult.OK);
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            var expectedMapBackColor = tree.CanvasBackColor;
            tree.TurnOnChangeManager();
            mapCtrl.SetDefaultFormatDialog();
            tree.ChangeManager.Undo();
            Assert.AreEqual(NodeShape.Fork, c1.NodeView.NodeFormat.Shape);
            Assert.AreEqual(expectedMapBackColor, tree.CanvasBackColor);
        }

        [TestMethod()]
        public void ApplyTheme()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            DialogManager dialogs = A.Fake<DialogManager>();            
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), dialogs, null);
            form.Controls.Add(mapCtrl.MapView.Canvas);
            var expectedMapBackColor = tree.CanvasBackColor;
            tree.TurnOnChangeManager();
            foreach (var theme in MetaModel.MetaModel.Instance.Themes.Themes)
            {
                mapCtrl.ApplyTheme(theme);
                tree.ChangeManager.Undo();
            }            
            Assert.AreEqual(c1.NodeView.NodeFormat.LineColor, NodeFormat.DefaultLineColor);
            Assert.AreEqual(c1.NodeView.NodeFormat.Color, NodeFormat.DefaultColor);
            Assert.AreEqual(tree.CanvasBackColor, TreeFormat.DefaultCanvasBackColor);            
        }
    }
}
