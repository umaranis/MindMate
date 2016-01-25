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

            mapCtrl.SelectAllNodes();

            Assert.AreEqual(7, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectLevel_Level3()
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

            mapCtrl.SelectLevel(3);

            Assert.AreEqual(1, t.SelectedNodes.Count);
            Assert.IsTrue(c311.Selected);
        }

        [TestMethod()]
        public void SelectLevel_Level3Folded_NoSelection()
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

            mapCtrl.SelectLevel(3);

            Assert.AreEqual(0, t.SelectedNodes.Count);
        }

        [TestMethod()]
        public void SelectLevel_Level2()
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

            mapCtrl.SelectLevel(2);

            Assert.AreEqual(3, t.SelectedNodes.Count);
            Assert.IsTrue(c12.Selected);
        }

        [TestMethod()]
        public void SelectLevel_Level2_ExpandSelection()
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

            r.Selected = true;
            mapCtrl.SelectLevel(2, true);

            Assert.AreEqual(6, t.SelectedNodes.Count);
            Assert.IsTrue(c12.Selected);
            Assert.IsTrue(r.Selected);
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

            mapCtrl.SelectCurrentLevel();

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

            mapCtrl.SelectCurrentLevel();

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

            mapCtrl.SelectCurrentLevel();

            Assert.AreEqual(4, t.SelectedNodes.Count);
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
        //public void SelectTopSibling()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SelectBottomSibling()
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