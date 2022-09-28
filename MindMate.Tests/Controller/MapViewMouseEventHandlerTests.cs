using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using MindMate.Model;
using MindMate.View.Dialogs;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.Controller
{
    [TestClass()]
    public class MapViewMouseEventHandlerTests
    {
        private static MapView view;

        [ClassInitialize]
        public static void ClassSetup(TestContext c)
        {
            view = SetupMap();
        }

        private static MapView SetupMap()
        {
            MapTree tree = new MapTree();
            MapNode r = new MapNode(tree, "r");
            var form = new System.Windows.Forms.Form();
            MetaModel.MetaModel.Initialize();
            MetaModel.MetaModel.Instance.MapEditorBackColor = Color.White;
            MetaModel.MetaModel.Instance.NoteEditorBackColor = Color.White;
            MapCtrl mapCtrl = new MapCtrl(new MapView(tree), A.Fake<DialogManager>(), null);
            form.Controls.Add(mapCtrl.MapView.Canvas);

            tree.TurnOnChangeManager();

            return mapCtrl.MapView;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            //clear tree except root node
            while(view.Tree.RootNode.FirstChild != null)
            {
                view.Tree.RootNode.FirstChild.DeleteNode();
            }
        }

        private static void FireMouseMove(int x, int y, MouseButtons mouseButtons = MouseButtons.None)
        {
            view.Canvas.GetType().GetMethod("OnMouseMove", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(view.Canvas, new object[] {
                    new MouseEventArgs(mouseButtons, 0, x, y, 0) });
        }

        //private static void FireMouseDown(int x, int y, MouseButtons mouseButtons = MouseButtons.None)
        //{
        //    view.Canvas.GetType().GetMethod("OnMouseDown", BindingFlags.Instance | BindingFlags.NonPublic)
        //            .Invoke(view.Canvas, new object[] {
        //            new MouseEventArgs(mouseButtons, 0, x, y, 0) });
        //}

        private static void FireMouseUp(int x, int y, MouseButtons mouseButtons = MouseButtons.None)
        {
            view.Canvas.GetType().GetMethod("OnMouseUp", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(view.Canvas, new object[] {
                    new MouseEventArgs(mouseButtons, 0, x, y, 0) });
        }

        private static void FireMouseHover()
        {

            view.Canvas.GetType().GetMethod("OnMouseHover", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(view.Canvas, new object[] {
                    new EventArgs() });
        }

        //private static void FirePreviewKeyDown()
        //{
        //    view.Canvas.GetType().GetMethod("OnPreviewKeyDown", BindingFlags.Instance | BindingFlags.NonPublic)
        //            .Invoke(view.Canvas, new object[] {
        //            new PreviewKeyDownEventArgs(Keys.Right) });
        //}

        [TestMethod()]
        public void MouseMove_DefaultCursor()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");

            FireMouseMove((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);
            FireMouseMove((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);

            Assert.AreEqual(Cursors.Default, view.Canvas.Cursor);
        }

        [TestMethod()]
        public void MouseMove_Link_HandCursor()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Link = "http://testlink";

            FireMouseMove((int)c1.NodeView.Left + 5, (int)c1.NodeView.Top + 5);
            FireMouseMove((int)c1.NodeView.Left + 5, (int)c1.NodeView.Top + 5);

            Assert.AreEqual(Cursors.Hand, view.Canvas.Cursor);
        }

        [TestMethod()]
        public void MouseMove_Link_DefaultCursor()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Link = "http://testlink";
            var c2 = new MapNode(c1, "c11");

            FireMouseMove((int)c1.NodeView.Right - 1, (int)c1.NodeView.Bottom - 1);
            FireMouseMove((int)c1.NodeView.Right - 1, (int)c1.NodeView.Bottom - 1);

            Assert.AreEqual(Cursors.Default, view.Canvas.Cursor);
        }

        [TestMethod()]
        public void MouseMove_LinkWithChildren_HandCursor()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Link = "http://testlink";
            var c2 = new MapNode(c1, "c11");

            FireMouseMove((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);
            FireMouseMove((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);

            Assert.AreEqual(Cursors.Hand, view.Canvas.Cursor);
        }

        [TestMethod()]
        public void MouseMove_SubControl_NoteIcon()
        {
            bool result = false;
            view.Canvas.NodeMouseMove += (node, e) => result = e.SubControlType == SubControlType.Note;
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.NoteText = "This is a test note";            

            FireMouseMove((int)c1.NodeView.Left + 10, (int)c1.NodeView.Top + 10);

            view.Canvas.NodeMouseMove -= (node, e) => result = e.SubControlType == SubControlType.Note;

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void MouseMove_SubControl_Icon()
        {
            bool result = false;
            view.Canvas.NodeMouseMove += (node, e) => result = e.SubControlType == SubControlType.Icon && "button_ok".Equals(e.SubControl);
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.Icons.Add("button_ok");

            FireMouseMove((int)c1.NodeView.Left + 10, (int)c1.NodeView.Top + 10);

            view.Canvas.NodeMouseMove -= (node, e) => result = e.SubControlType == SubControlType.Icon && "button_ok".Equals(e.SubControl);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void MouseMove_SubControl_Image()
        {
            bool result = false;
            view.Canvas.NodeMouseMove += (node, e) => result = e.SubControlType == SubControlType.Image;
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.InsertImage(Image.FromFile(@"Resources\OrangeTestImage.jpg"), true);

            FireMouseMove((int)c1.NodeView.Left + 10, (int)c1.NodeView.Top + 10);

            view.Canvas.NodeMouseMove -= (node, e) => result = e.SubControlType == SubControlType.Image;

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void MouseMove_SubControl_Text()
        {
            bool result = false;
            view.Canvas.NodeMouseMove += (node, e) => result = e.SubControlType == SubControlType.Text;
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            c1.InsertImage(Image.FromFile(@"Resources\OrangeTestImage.jpg"), true);

            FireMouseMove((int)c1.NodeView.Left + 10, (int)c1.NodeView.Top + 22);

            view.Canvas.NodeMouseMove -= (node, e) => result = e.SubControlType == SubControlType.Text;

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void MapNodeClick_Fold()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            var c2 = new MapNode(c1, "c11");
            FireMouseUp((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);
            Assert.IsTrue(c1.Folded);
        }

        [TestMethod()]
        public void MapNodeClick_EditText()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");            
            FireMouseUp((int)c1.NodeView.Left + 10, (int)c1.NodeView.Top + 10);
            Assert.IsTrue(view.NodeTextEditor.IsTextEditing);
            view.NodeTextEditor.EndNodeEdit(true, true);
        }

        [TestMethod()]
        public void MapNodeClick_EditText_EmptyNode()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "");
            FireMouseUp((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);
            Assert.IsTrue(view.NodeTextEditor.IsTextEditing);
            view.NodeTextEditor.EndNodeEdit(true, true);
        }

        [TestMethod()]
        public void MapNodeClick_FollowLink()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "");
            c1.Link = "Dummy Link";
            FireMouseUp((int)c1.NodeView.Left + 1, (int)c1.NodeView.Top + 1);
            Assert.IsFalse(view.NodeTextEditor.IsTextEditing);
        }

        [TestMethod()]
        public void CanvasClick()
        {
            var r = view.Tree.RootNode;
            var c1 = new MapNode(r, "c1");
            var c11 = new MapNode(c1, "c11");
            r.ForEach(n => view.SelectedNodes.Add(n, true));
            FireMouseUp(0, 0);
            Assert.AreEqual(1, view.SelectedNodes.Count);
        }       
        
    }
}