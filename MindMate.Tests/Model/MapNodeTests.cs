using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.View.MapControls;
using MindMate.View.MapControls.Drawing;
using XnaFan.ImageComparison;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class MapNodeTests
    {
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
            MapPainter.DrawNode(n2.NodeView, false, mapView, g);
            g.Dispose();

            bmp2.Save(@"Resources\CopyFormatTo_CompareOutputWithSavedImage_Des.png", ImageFormat.Png);

            Assert.IsTrue(bmp1.PercentageDifference(bmp2, 0) == 0);

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
    }
}
