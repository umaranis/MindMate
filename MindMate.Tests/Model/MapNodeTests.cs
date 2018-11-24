using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using MindMate.View.MapControls;
using MindMate.View.MapControls.Drawing;
using XnaFan.ImageComparison;
using MindMate.Serialization;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class MapNodeTests
    {
        [TestMethod()]
        public void MapNode_HasNote_False()
        {
            var r = new MapNode(new MapTree(), "r");
            Assert.IsFalse(r.HasNoteText);
        }

        [TestMethod()]
        public void MapNode_AddChildOnRight()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1", NodePosition.Right);

            Assert.AreEqual(r.GetFirstChild(NodePosition.Right), c1);
        }

        [TestMethod()]
        public void MapNode_AddChildOnLeft()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1", NodePosition.Left);

            Assert.AreEqual(r.GetFirstChild(NodePosition.Left), c1);
        }

        [TestMethod()]
        public void MapNode_AddSiblingAbove()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = new MapNode(c1, null, NodePosition.Undefined, null, c12, false);

            Assert.AreEqual(c12.Previous, n);
            Assert.AreEqual(n.Next, c12);
            Assert.IsTrue(c1.ChildNodes.Contains(n));
        }

        [TestMethod()]
        public void MapNode_AddSiblingBelow()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = new MapNode(c1, null, NodePosition.Undefined, null, c12, true);

            Assert.AreEqual(c12.Next, n);
            Assert.AreEqual(n.Previous, c12);
            Assert.IsTrue(c1.ChildNodes.Contains(n));
        }

        [TestMethod()]
        public void MapNode_AddLastChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = new MapNode(c1, null, NodePosition.Undefined, null, null, true);

            Assert.AreEqual(c1.LastChild, n);
        }

        [TestMethod()]
        public void MapNode_AddChildWithInsertAfterFalse()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = new MapNode(c1, null, NodePosition.Undefined, null, null, false);

            Assert.AreEqual(c1.LastChild.Previous, n);
        }

        [TestMethod()]
        public void MapNode_AddFirstChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = new MapNode(c1, null, NodePosition.Undefined, null, c11, false);

            Assert.AreEqual(c1.FirstChild, n);
        }

        [TestMethod()]
        public void MapNode_AddSecondChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = new MapNode(c1, null, NodePosition.Undefined, null, c11, true);

            Assert.AreEqual(c1.FirstChild.Next, n);
        }

        [TestMethod()]
        public void MapNode_RootNode_Position()
        {
            var r = new MapNode(new MapTree(), "r");

            Assert.AreEqual(r.Pos, NodePosition.Root);
            Assert.AreEqual(r.Parent, null);
        }

        [TestMethod()]
        public void MapNode_WithRightPosition()
        {
            var r = new MapNode(new MapTree(), "r");
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

            Assert.AreEqual(7, r.ChildRightNodes.Count());
            Assert.AreEqual(0, r.ChildLeftNodes.Count());
        }

        [TestMethod()]
        public void MapNode_WithLeftPosition()
        {
            var r = new MapNode(new MapTree(), "r");
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

            Assert.AreEqual(0, r.ChildRightNodes.Count());
            Assert.AreEqual(7, r.ChildLeftNodes.Count());
        }

        [TestMethod()]
        public void AttachTo()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c3.Detach();
            c3.AttachTo(c2);

            Assert.AreEqual(c2.FirstChild, c3);
        }

        [TestMethod()]
        public void AttachTo_ChildrenAttached()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c3.Detach();
            c3.AttachTo(c2);

            Assert.IsTrue(c32.IsDescendent(c2));
        }

        [TestMethod()]
        public void Detach()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c3.Detach();

            Assert.IsTrue(c3.Detached);
            Assert.IsFalse(r.ChildNodes.Contains(c3));
        }

        [TestMethod()]
        public void CloneAsDetached_SourceNotChanged()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);

            var n = c3.CloneAsDetached();

            Assert.AreNotEqual(c3, n);
            Assert.IsFalse(c3.Detached);
        }

        [TestMethod()]
        public void CloneAsDetached_CloneIsDetached()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");

            var n = c3.CloneAsDetached();

            Assert.IsTrue(n.Detached);
        }

        [TestMethod()]
        public void CloneAsDetached_CloneWithChildren()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = c3.CloneAsDetached();

            Assert.AreEqual(n.ChildNodes.Count(), 2);
        }

        [TestMethod()]
        public void CloneAsDetached_CheckAttributes()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left) {
                Image = "image",
                ImageAlignment = MindMate.Model.ImageAlignment.BeforeCenter,
                ImageSize = new Size(12, 20),
                Text = "text",
                NoteText = "notetext",
                Folded = true,
                FontName = "fontname",
                FontSize = 20,
                Link = "link"
            };
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            var n = c3.CloneAsDetached();

            Assert.AreEqual(n.ChildNodes.Count(), 2);
            Assert.AreEqual("image", n.Image);
            Assert.AreEqual(MindMate.Model.ImageAlignment.BeforeCenter, n.ImageAlignment);
            Assert.AreEqual(new Size(12, 20), n.ImageSize);
            Assert.AreEqual("text", n.Text);
            Assert.AreEqual("notetext", n.NoteText);
            Assert.AreEqual(true, n.Folded);
            Assert.AreEqual("fontname", n.FontName);
            Assert.AreEqual(20, n.FontSize);
            Assert.AreEqual("link", n.Link);
        }

        [TestMethod()]
        public void GetFirstSib()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);

            Assert.AreEqual(c3.GetFirstSib(), c1);
        }

        [TestMethod()]
        public void GetLastSib()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);

            Assert.AreEqual(c1.GetLastSib(), c3);
        }

        [TestMethod()]
        public void GetFirstChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");

            Assert.AreEqual(r.GetFirstChild(NodePosition.Right), c1);
        }

        [TestMethod()]
        public void GetLastChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);

            Assert.AreEqual(r.GetLastChild(NodePosition.Left), c3);
        }

        [TestMethod()]
        public void IsDescendent()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");

            Assert.IsTrue(cc2.IsDescendent(r));
        }

        [TestMethod()]
        public void IsDescendent_FalseCase()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");

            Assert.IsFalse(r.IsDescendent(c1));
        }

        [TestMethod()]
        public void GetSiblingLocation_Above()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");

            Assert.AreEqual(cc3.GetSiblingLocation(cc1), MindMate.Model.MapNode.SiblingLocaton.Above);
        }

        [TestMethod()]
        public void GetSiblingLocation_Above_NotFirstChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var cc4 = new MapNode(c1, "cc4");
            var cc5 = new MapNode(c1, "cc5");
            var c2 = new MapNode(r, "c2");

            Assert.AreEqual(cc4.GetSiblingLocation(cc2), MindMate.Model.MapNode.SiblingLocaton.Above);
        }

        [TestMethod()]
        public void GetSiblingLocation_Below()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");

            Assert.AreEqual(cc1.GetSiblingLocation(cc3), MindMate.Model.MapNode.SiblingLocaton.Below);
        }

        [TestMethod()]
        public void GetSiblingLocation_Below_NotLastChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var cc4 = new MapNode(c1, "cc4");
            var cc5 = new MapNode(c1, "cc5");
            var c2 = new MapNode(r, "c2");

            Assert.AreEqual(cc2.GetSiblingLocation(cc5), MindMate.Model.MapNode.SiblingLocaton.Below);
        }

        [TestMethod()]
        public void GetSiblingLocation_NotSibling()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var cc3 = new MapNode(c1, "cc3");
            var c2 = new MapNode(r, "c2");

            Assert.AreEqual(cc3.GetSiblingLocation(c1), MindMate.Model.MapNode.SiblingLocaton.NotSibling);
        }

        [TestMethod()]
        public void DeleteNode_RootNode()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            r.DeleteNode();

            Assert.AreEqual(r, r.Tree.RootNode);
        }

        [TestMethod()]
        public void DeleteNode_LastChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            cc2.DeleteNode();

            Assert.AreEqual(c1.LastChild, cc1);
        }

        [TestMethod()]
        public void DeleteNode_FirstChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            cc1.DeleteNode();

            Assert.AreEqual(c1.FirstChild, cc2);
        }

        [TestMethod()]
        public void LastSelectedChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "cc1");
            var c12 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            c12.Selected = true;
            c12.DeleteNode();

            Assert.IsNull(c1.LastSelectedChild);
        }

        [TestMethod()]
        public void MoveUp()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            cc2.MoveUp();

            Assert.AreEqual(c1.FirstChild, cc2);
        }

        [TestMethod()]
        public void MoveUp_FirstChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            cc1.MoveUp();

            Assert.AreEqual(c1.FirstChild, cc1);
            Assert.IsFalse(cc1.MoveUp());
        }

        [TestMethod()]
        public void MoveUp_RootNode_NoChange()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            bool result = r.MoveUp();

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void MoveUp_LeftToRight()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1", NodePosition.Right);
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2", NodePosition.Left);

            c2.MoveUp();

            Assert.AreEqual(c2.Pos, NodePosition.Right);
        }

        [TestMethod()]
        public void MoveUp_LeftToRightAndBack()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1", NodePosition.Right);
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2", NodePosition.Left);

            c2.MoveUp();
            c2.MoveDown();

            Assert.AreEqual(c2.Pos, NodePosition.Left);
        }

        [TestMethod()]
        public void MoveDown()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            cc1.MoveDown();

            Assert.AreEqual(c1.FirstChild, cc2);
        }

        [TestMethod()]
        public void MoveDown_LastChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            cc2.MoveDown();

            Assert.AreEqual(c1.LastChild, cc2);
            Assert.IsFalse(cc2.MoveDown());
        }

        [TestMethod()]
        public void MoveDown_RootNode_NoChange()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            bool result = r.MoveDown();

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void MoveDown_RightToLeft()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1", NodePosition.Right);
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2", NodePosition.Left);

            c1.MoveDown();

            Assert.AreEqual(c1.Pos, NodePosition.Left);
        }

        [TestMethod()]
        public void MoveDown_RightToLeftAndBack()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1", NodePosition.Right);
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2", NodePosition.Left);

            c1.MoveDown();
            c1.MoveUp();

            Assert.AreEqual(c1.Pos, NodePosition.Right);
        }

        [TestMethod()]
        public void Find()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            var result = r.Find(n => n.Text.EndsWith("2"));

            Assert.AreEqual(result, cc2);
        }

        [TestMethod()]
        public void FindAll()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var cc2 = new MapNode(c1, "cc2");
            var c2 = new MapNode(r, "c2");

            var list = r.FindAll(n => n.Text.StartsWith("r") || n.Text.EndsWith("2"));

            Assert.AreEqual(list.Count, 3);
        }

        [TestMethod()]
        public void ForEach()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var cc1 = new MapNode(c1, "cc1");
            var c2 = new MapNode(r, "c2");

            r.ForEach(n => n.Text = "Updated");

            Assert.AreEqual(r.Text, "Updated");
            Assert.AreEqual(c1.Text, "Updated");
            Assert.AreEqual(cc1.Text, "Updated");
            Assert.AreEqual(c2.Text, "Updated");
        }

        [TestMethod()]
        public void Find_ByTextSearch()
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

            var result = t.RootNode.Find(n => n.Text.EndsWith("2"));

            Assert.AreEqual(c12, result);

        }

        [TestMethod()]
        public void FindAll_ByTextSearch()
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

            var result = t.RootNode.FindAll(n => n.Text.EndsWith("2"));

            Assert.AreEqual(3, result.Count);
        }

        [TestMethod()]
        public void ForEach_UpdateText()
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

            t.RootNode.ForEach(n => n.Text = "Updated");

            Assert.AreEqual("Updated", c12.Text);
            Assert.AreEqual("Updated", c2.Text);
            Assert.AreEqual("Updated", c32.Text);
        }

        [TestMethod]
        public void ForEach_SkipFolded()
        {
            var r = new MapNode(new MapTree(), "r");
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

            r.ForEach(n => n.Text = "Updated", n => n.Folded == false);

            Assert.AreEqual(r.Text, "Updated");
            Assert.AreEqual(c1.Text, "Updated");
            Assert.AreEqual(c11.Text, "Updated");
            Assert.AreEqual(c12.Text, "Updated");
            Assert.AreEqual(c12.Text, "Updated");
            Assert.AreEqual(c2.Text, "Updated");
            Assert.AreEqual(c3.Text, "Updated");
            Assert.AreEqual(c31.Text, "c31");
            Assert.AreEqual(c311.Text, "c311");
            Assert.AreEqual(c32.Text, "c32");
        }

        [TestMethod()]
        public void ForEachAncestor()
        {
            var n = new MapNode(new MapTree(), null);
            for (int i = 0; i < 5; i++)
            {
                n = new MapNode(n, null);
            }

            n.ForEachAncestor(a => a.Text = "Updated");

            Assert.AreEqual(n.Text, null);
            Assert.AreEqual(n.Parent.Text, "Updated");
            Assert.AreEqual(n.Parent.Parent.Text, "Updated");
            Assert.AreEqual(n.Parent.Parent.Parent.Text, "Updated");
            Assert.AreEqual(n.Parent.Parent.Parent.Parent.Text, "Updated");
            Assert.AreEqual(n.Parent.Parent.Parent.Parent.Parent.Text, "Updated");
            Assert.AreEqual(n.Parent.Parent.Parent.Parent.Parent.Parent, null);
        }

        [TestMethod]
        public void ForEachAncestor_WithRoot()
        {
            var n = new MapNode(new MapTree(), null);
            n.ForEachAncestor(node => node.Text = "Try update text");
        }

        [TestMethod()]
        public void GetLinkType_Empty()
        {
            var n = new MapNode(new MapTree(), null);
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.Empty);
        }

        [TestMethod()]
        public void GetLinkType_MAILTO_Email()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "MAILTO:";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.EmailLink);
        }

        [TestMethod()]
        public void GetLinkType_MailTo_Email()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "mailto:";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.EmailLink);
        }

        [TestMethod()]
        public void GetLinkType_Mail_NotEmail()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "mail";
            Assert.AreNotEqual(n.GetLinkType(), NodeLinkType.EmailLink);
        }

        [TestMethod()]
        public void GetLinkType_Gif_Image()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "abc.gif";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.ImageFile);
        }

        [TestMethod()]
        public void GetLinkType_Exe()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "abc.exe";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.Executable);
        }

        [TestMethod()]
        public void GetLinkType_Mp4_Video()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "abc.Mp4";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.VideoFile);
        }

        [TestMethod()]
        public void GetLinkType_InternetLink()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = "HTTPS://";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.InternetLink);
        }

        [TestMethod()]
        public void GetLinkType_Folder()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = @"\abc";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.Folder);
        }

        [TestMethod()]
        public void GetLinkType_File()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = @"\abc.a";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.File);
        }

        [TestMethod()]
        public void GetLinkType_MindMateNode()
        {
            var n = new MapNode(new MapTree(), null);
            n.Link = @"#abc";
            Assert.AreEqual(n.GetLinkType(), NodeLinkType.MindMapNode);
        }

        [TestMethod()]
        public void GetNodeDepth_ForRoot_Zero()
        {
            var n = new MapNode(new MapTree(), null);
            Assert.AreEqual(n.GetNodeDepth(), 0);
        }

        [TestMethod()]
        public void GetNodeDepth()
        {
            var n = new MapNode(new MapTree(), null);
            for (int i = 0; i < 5; i++)
            {
                n = new MapNode(n, null);
            }

            Assert.AreEqual(n.GetNodeDepth(), 5);
        }

        [TestMethod()]
        public void IsEmpty_ForRootNode_NullText()
        {
            var rootNode = new MapNode(new MapTree(), null);
            Assert.IsTrue(rootNode.IsEmpty());
        }

        [TestMethod()]
        public void IsEmpty_ForRootNode_EmptyText()
        {
            var rootNode = new MapNode(new MapTree(), "");
            Assert.IsTrue(rootNode.IsEmpty());
        }

        [TestMethod()]
        public void CopyFormatTo_ValidateObjectState()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root");

            var n1 = new MapNode(r, "Testing Format Copy");
            var n2 = new MapNode(r, "Testing Format Copy");

            n1.Bold = true;
            n1.Strikeout = true;
            n1.Color = Color.BlueViolet;
            n1.BackColor = Color.Bisque;
            n1.FontSize = 16;

            n1.CopyFormatTo(n2);

            Assert.AreEqual(n1.Bold, n2.Bold);
            Assert.AreEqual(n1.Strikeout, n2.Strikeout);
            Assert.AreEqual(n1.Italic, n2.Italic);
            Assert.AreEqual(n1.Color, n2.Color);
            Assert.AreEqual(n1.BackColor, n2.BackColor);
            Assert.AreEqual(n1.FontSize, n2.FontSize);
            Assert.AreEqual(n1.FontName, n2.FontName);
        }

        [TestMethod()]
        public void CopyFormatTo_CompareOutputWithSavedImage()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root");

            var n1 = new MapNode(r, "Testing Format Copy");
            var n2 = new MapNode(r, "Testing Format Copy");

            n1.Bold = true;
            n1.Strikeout = true;
            n1.Color = Color.BlueViolet;
            n1.BackColor = Color.Bisque;
            n1.FontSize = 16;

            n1.CopyFormatTo(n2);

            MetaModel.MetaModel.Initialize();
            var mapView = new MapView(t);
            mapView.GetNodeView(n1);
            mapView.GetNodeView(n2);

            Bitmap bmp1 = new Bitmap(@"Resources\CopyFormatTo_CompareOutputWithSavedImage_Src.png");

            Bitmap bmp2 = new Bitmap(2500, 2500);
            Graphics g = Graphics.FromImage(bmp2);
            MapPainter.DrawNode(n2, false, mapView, g);
            g.Dispose();

            bmp2.Save(@"Resources\CopyFormatTo_CompareOutputWithSavedImage_Des.png", ImageFormat.Png);

            Assert.AreEqual<float>(0.00f, bmp1.PercentageDifference(bmp2, 0));

            bmp1.Dispose();
            bmp2.Dispose();
        }

        [TestMethod()]
        public void ToStringTest()
        {
            string text = "TestString";
            var node = new MapNode(new MapTree(), text);
            Assert.AreEqual<string>(text, node.ToString());
        }

        [TestMethod()]
        public void BoldSet_AlreadyBold_NoChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root") { Bold = true };
            DateTime time = r.Modified;
            t.NodePropertyChanged += (node, args) => Assert.Fail();
            r.Bold = true;
            Assert.IsTrue(r.Bold);
            Assert.AreEqual(r.Modified, time);
        }

        [TestMethod]
        public void ItalicSet_AlreadyItalic_NoChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root") { Italic = true };
            DateTime time = r.Modified;
            t.NodePropertyChanged += (node, args) => Assert.Fail();
            r.Italic = true;
            Assert.IsTrue(r.Italic);
            Assert.AreEqual(r.Modified, time);
        }

        [TestMethod]
        public void StrikeoutSet_AlreadyStrikeout_NoChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root") { Strikeout = true };
            DateTime time = r.Modified;
            t.NodePropertyChanged += (node, args) => Assert.Fail();
            r.Strikeout = true;
            Assert.IsTrue(r.Strikeout);
            Assert.AreEqual(r.Modified, time);
        }

        [TestMethod]
        public void FoldedSet_AlreadyFolded_NoChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root");
            var c1 = new MapNode(r, "c1");
            var c12 = new MapNode(c1, "c12");
            c1.Folded = true;
            DateTime time = c1.Modified;
            t.NodePropertyChanged += (node, args) => Assert.Fail();

            c1.Folded = true;

            Assert.IsTrue(c1.Folded);
            Assert.AreEqual(time, c1.Modified);
        }

        [TestMethod]
        public void FoldedSet_WithSelectedChild_DeselectChild()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root");
            var c1 = new MapNode(r, "c1");
            var c12 = new MapNode(c1, "c12");
            c12.Selected = true;

            c1.Folded = true;

            Assert.IsFalse(c12.Selected);
        }

        [TestMethod]
        public void Selected_Test()
        {
            var t = new MapTree();
            var r = new MapNode(t, "Root") { Selected = true };
            Assert.AreEqual(t.SelectedNodes.First, r);
            Assert.AreEqual(t.SelectedNodes.Count, 1);
        }

        [TestMethod()]
        public void RollUpAggregate_CountNodesWith2Param()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            int count = r.RollUpAggregate(n => 1, (a, b) => a + b);
            Assert.AreEqual(9, count);
        }

        [TestMethod()]
        public void RollUpAggregate_CountNodesWith4Param()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            int count = r.RollUpAggregate(n => 1, (a, b) => a + b, (node, i) => { }, node => false);
            Assert.AreEqual(9, count);
        }

        [TestMethod()]
        public void RollUpAggregate_CountNodesUnfolded()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            int count = r.RollUpAggregate(n => 1, (a, b) => a + b, (node, i) => { node.Text = i.ToString(); }, node => node.Folded);

            Assert.AreEqual(7, count);
            Assert.AreEqual("4", c1.Text);
        }

        [TestMethod]
        public void RollDownAggregate_SetNodeTextAsDepth()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            r.RollDownAggregate(
                (n, v) =>
                {
                    n.Text = v.ToString();
                    return v + 1;
                },
                0);

            Assert.AreEqual("0", r.Text);
            Assert.AreEqual("2", c13.Text);
        }

        [TestMethod()]
        public void RollDownAggregate_DeepestNodeUnfolded()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            c13.Folded = true;
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            int deepestYet = 0;
            MapNode deepNodeYet = r;

            r.RollDownAggregate(
                (n, v) =>
                {
                    if (v > deepestYet)
                    {
                        deepestYet = v;
                        deepNodeYet = n;
                    }
                    return v + 1;
                },
                0,
                (n, v) => n.Folded
                );

            Assert.AreEqual(c121, deepNodeYet);
            Assert.AreEqual(3, deepestYet);

        }

        [TestMethod]
        public void ForEachSibling()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c14 = new MapNode(c1, "c14");
            var c15 = new MapNode(c1, "c15");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c13.ForEachSibling(n => n.Text = "Updated");

            Assert.AreEqual("Updated", c11.Text);
            Assert.AreEqual("Updated", c12.Text);
            Assert.AreNotEqual("Updated", c13.Text);
            Assert.AreEqual("Updated", c14.Text);
            Assert.AreEqual("Updated", c15.Text);
            Assert.AreNotEqual("Updated", c2.Text);
        }

        [TestMethod()]
        public void ForEachSameLevelNode_AllBranchesHaveLevel2()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c14 = new MapNode(c1, "c14");
            var c15 = new MapNode(c1, "c15");
            var c2 = new MapNode(r, "c2");
            var c21 = new MapNode(c2, "c21");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c13.ForEachSameLevelNode(n => n.Text = "Updated");

            Assert.AreEqual(7, r.RollUpAggregate(n => n.Text == "Updated" ? 1 : 0, (i, i1) => i + i1));
        }

        [TestMethod()]
        public void ForEachSameLevelNode_AllBranchesBelowDonotHaveLevel2()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c14 = new MapNode(c1, "c14");
            var c15 = new MapNode(c1, "c15");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c13.ForEachSameLevelNode(n => n.Text = "Updated");

            Assert.AreEqual(6, r.RollUpAggregate(n => n.Text == "Updated" ? 1 : 0, (i, i1) => i + i1));
        }

        [TestMethod()]
        public void ForEachSameLevelNode_AllBranchesAboveDonotHaveLevel2()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c14 = new MapNode(c1, "c14");
            var c15 = new MapNode(c1, "c15");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            c31.ForEachSameLevelNode(n => n.Text = "Updated");

            Assert.AreEqual(6, r.RollUpAggregate(n => n.Text == "Updated" ? 1 : 0, (i, i1) => i + i1));
        }

        [TestMethod()]
        public void GetClosestSameLevelNodeAbove()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c14 = new MapNode(c1, "c14");
            var c15 = new MapNode(c1, "c15");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            Assert.AreEqual(c15, c31.GetClosestSameLevelNodeAbove());
        }

        [TestMethod()]
        public void GetClosestSameLevelNodeAbove_WithDoubleGap()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c4 = new MapNode(r, "c4");
            var c5 = new MapNode(r, "c5");
            var c6 = new MapNode(r, "c6");
            var c61 = new MapNode(c6, "c61");

            Assert.AreEqual(c31, c61.GetClosestSameLevelNodeAbove());
        }

        [TestMethod()]
        public void GetClosestSameLevelNodeBelow()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c4 = new MapNode(r, "c4");
            var c5 = new MapNode(r, "c5");
            var c6 = new MapNode(r, "c6");
            var c61 = new MapNode(c6, "c61");

            Assert.AreEqual(c61, c31.GetClosestSameLevelNodeBelow());
        }

        [TestMethod]
        public void Descendents()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c13 = new MapNode(c1, "c13");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            Assert.AreEqual(8, r.Descendents.Count());
        }        

        [TestMethod()]
        public void UnfoldDescendents()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            c13.Folded = true;
            var c131 = new MapNode(c13, "c131");
            c131.Folded = true;
            var c1311 = new MapNode(c131, "c1311");
            var c2 = new MapNode(r, "c2");
            var c3 = new MapNode(r, "c3", NodePosition.Left);
            c3.Folded = true;
            var c31 = new MapNode(c3, "c31");
            var c32 = new MapNode(c3, "c32");

            r.UnfoldDescendents();

            Assert.IsFalse(c13.Folded);
            Assert.IsFalse(c131.Folded);
            Assert.IsFalse(c2.Folded);
        }

        [TestMethod()]
        public void FoldDescendents()
        {
            var r = new MapNode(new MapTree(), "r");
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

            r.FoldDescendents();

            Assert.IsTrue(c1.Folded);
            Assert.IsTrue(c12.Folded);
            Assert.IsTrue(c131.Folded);
            Assert.IsTrue(c1311.Folded);
        }

        [TestMethod()]
        public void FoldDescendents_DonotFoldRoot()
        {
            var r = new MapNode(new MapTree(), "r");
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

            r.FoldDescendents();

            Assert.IsFalse(r.Folded);
        }

        [TestMethod()]
        public void ToggleDescendentsFolding_WithNoFolded_FoldNodes()
        {
            var r = new MapNode(new MapTree(), "r");
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

            r.ToggleDescendentsFolding();

            Assert.IsTrue(c1.Folded);
            Assert.IsTrue(c12.Folded);
            Assert.IsTrue(c131.Folded);
            Assert.IsTrue(c1311.Folded);
        }

        [TestMethod()]
        public void ToggleDescendentsFolding_WithFolded_UnfoldNodes()
        {
            var r = new MapNode(new MapTree(), "r");
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

            r.ToggleDescendentsFolding();

            Assert.IsFalse(c3.Folded);
            Assert.IsFalse(c1.Folded);
        }

        [TestMethod()]
        public void SortChildren_NoChildren()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");

            c1311.SortChildren((node1, node2) => string.CompareOrdinal(node2.Text, node1.Text));

            Assert.AreEqual(c1311, c131.FirstChild);
            Assert.AreEqual(c1311, c131.LastChild);
        }

        [TestMethod()]
        public void SortChildren_OneChild()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");
            var c11 = new MapNode(c1, "c11");
            var c12 = new MapNode(c1, "c12");
            var c121 = new MapNode(c12, "c121");
            var c13 = new MapNode(c1, "c13");
            var c131 = new MapNode(c13, "c131");
            var c1311 = new MapNode(c131, "c1311");

            c131.SortChildren((node1, node2) => string.CompareOrdinal(node2.Text, node1.Text));

            Assert.AreEqual(c1311, c131.FirstChild);
            Assert.AreEqual(c1311, c131.LastChild);
        }

        [TestMethod()]
        public void SortChildren()
        {
            var r = new MapNode(new MapTree(), "r");
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

            c1.SortChildren((node1, node2) => string.CompareOrdinal(node1.Text, node2.Text));

            Assert.AreEqual(c15, c1.FirstChild);
            Assert.AreEqual(c12, c1.FirstChild.Next);
            Assert.AreEqual(c11, c1.LastChild.Previous);
            Assert.AreEqual(c14, c1.LastChild);
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

            int count = c1.GetDescendentsCount();

            Assert.AreEqual(8, count);
        }

        [TestMethod()]
        public void AddToSelection()
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

            r.AddToSelection();
            c1.AddToSelection();

            Assert.AreEqual(2, r.Tree.SelectedNodes.Count);
        }

        [TestMethod()]
        public void CreateIsolatedNode()
        {
            var sut = MapNode.CreateIsolatedNode(NodePosition.Left);
            Assert.AreEqual(MapTree.Default, sut.Tree);
        }

        [TestMethod()]
        public void ClearFormatting()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            c1.Bold = true;
            c1.FontSize = 20;
            c1.BackColor = Color.Aqua;
            c1.Text = "ok";

            c1.ClearFormatting();

            Assert.IsFalse(c1.Bold);
            Assert.AreEqual(0, c1.FontSize);
            Assert.AreEqual(c1.BackColor, Color.Empty);
        }

        [TestMethod()]
        public void ClearFormatting_NotClearText()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            c1.Bold = true;
            c1.FontSize = 20;
            c1.BackColor = Color.Aqua;
            c1.Text = "ok";

            c1.ClearFormatting();

            Assert.AreEqual(c1.Text, "ok");
        }

        [TestMethod()]
        public void Image()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            c1.Image = "test";

            Assert.AreEqual("test", c1.Image);
            Assert.IsTrue(c1.HasImage);
        }

        [TestMethod()]
        public void Image_NullByDefault()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            Assert.IsNull(c1.Image);
            Assert.IsFalse(c1.HasImage);
        }

        [TestMethod()]
        public void ImageAlignment()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            c1.ImageAlignment = MindMate.Model.ImageAlignment.BeforeTop;

            Assert.AreEqual(MindMate.Model.ImageAlignment.BeforeTop, c1.ImageAlignment);
            Assert.IsTrue(c1.HasImageAlignment);
        }        

        [TestMethod()]
        public void ImageAlignment_DefaultValue()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            Assert.AreEqual(MindMate.Model.ImageAlignment.Default, c1.ImageAlignment);
            Assert.IsFalse(c1.HasImageAlignment);
        }

        [TestMethod()]
        public void ImageSize()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            c1.ImageSize = new Size(10, 20);

            Assert.AreEqual(20, c1.ImageSize.Height);
            Assert.IsTrue(c1.HasImageSize);
        }

        [TestMethod()]
        public void ImageSize_DefaultValue()
        {
            var r = new MapNode(new MapTree(), "r");
            var c1 = new MapNode(r, "C");

            Assert.AreEqual(Size.Empty, c1.ImageSize);
            Assert.IsFalse(c1.HasImageSize);
        }

        [TestMethod]
        public void InsertImage()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\MapCtrl1.png"), true);
            Assert.IsTrue(r.HasImage);
        }

        [TestMethod]
        public void InsertImage_Jpg()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\OrangeTestImage.jpg"), true);
            Assert.IsTrue(r.HasImage);
        }

        [TestMethod]
        public void InsertImage_Bmp()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\OrangeTestImage.bmp"), true);
            Assert.IsTrue(r.HasImage);
        }

        [TestMethod]
        public void InsertImage_Gif()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\OrangeTestImage.gif"), true);
            Assert.IsTrue(r.HasImage);
        }

        [TestMethod]
        public void InsertImage_Tif()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\OrangeTestImage.tif"), true);
            Assert.IsTrue(r.HasImage);
        }

        [TestMethod]
        public void GetImage_Null()
        {
            var r = new MapNode(new MapTree(), "r");
            Assert.IsNull(r.GetImage());
        }

        [TestMethod]
        public void GetImage()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\MapCtrl1.png"), true);
            Assert.IsNotNull(r.GetImage());
        }

        [TestMethod]
        public void RemoveImage()
        {
            var r = new MapNode(new MapTree(), "r");
            r.InsertImage(System.Drawing.Image.FromFile(@"Resources\MapCtrl1.png"), true);
            Assert.IsTrue(r.HasImage);
            r.RemoveImage();
            Assert.IsFalse(r.HasImage);
        }

#if !DEBUG
        /// <summary>
        /// Serialized attriute is only applicable in debug mode. It is used in code generation, not at runtime.
        /// </summary>
        [TestMethod]
        public void SerializedAttribute_Applicable()
        {
            var props = typeof(MapNode).GetProperties();
            foreach (var p in props)
            {
                if (p.CustomAttributes.Any(a => a.AttributeType == typeof(SerializedAttribute)))
                {
                    Assert.Fail();
                }
            }
        }
#endif

    }
}
