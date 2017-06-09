using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.MapControls
{
    [TestClass()]
    public class MapViewFormatPainterTests
    {
        private MapView view;
        private MapNode r, c1, c2;

        public MapViewFormatPainterTests()
        {
            MapTree tree = new MapTree();
            tree.TurnOnChangeManager();
            r = new MapNode(tree, "Root");
            c1 = new MapNode(r, "c1");
            c2 = new MapNode(r, "c2");

            MindMate.MetaModel.MetaModel.Initialize();
            view = new MapView(tree);
            var form = new Form();
            form.Controls.Add(view.Canvas);
        }

        [TestMethod()]
        public void MapViewFormatPainter()
        {
            var sut = view.FormatPainter;
            Assert.IsFalse(sut.Active);
        }

        [TestMethod()]
        public void Copy()
        {
            var sut = view.FormatPainter;
            sut.Copy(c1);
            Assert.IsTrue(sut.Active);
        }

        [TestMethod()]
        public void EnableMultiApply()
        {
            var sut = view.FormatPainter;
            sut.Copy(c1);
            sut.EnableMultiApply();
            Assert.AreEqual(FormatPainterStatus.MultiApply, sut.Status);
        }

        [TestMethod()]
        public void Paste()
        {
            var sut = view.FormatPainter;
            c1.Bold = true; c2.Bold = false; r.Bold = false;
            sut.Copy(c1);
            Assert.IsFalse(c2.Bold);
            sut.Paste(c2);
            Assert.IsTrue(c2.Bold);
            Assert.IsFalse(sut.Active);
        }

        [TestMethod()]
        public void Paste_MultiApply()
        {
            var sut = view.FormatPainter;
            c1.Bold = true; c2.Bold = false; r.Bold = false;
            sut.Copy(c1);
            sut.EnableMultiApply();
            Assert.IsFalse(c2.Bold);
            sut.Paste(c2);
            Assert.IsTrue(c2.Bold);
            Assert.IsTrue(sut.Active);
        }

        [TestMethod()]
        public void Paste_MultiApplyThroughEvent()
        {
            var sut = view.FormatPainter;
            c1.Bold = true; c2.Bold = false; r.Bold = false;
            
            sut.Copy(c1, true);
            Assert.IsTrue(sut.Active);

            sut.EnableMultiApply();

            FireMouseUp((int)c2.NodeView.Left + 1, (int)c2.NodeView.Top + 1);
            Assert.IsTrue(c2.Bold);

            FireMouseUp((int)r.NodeView.Left + 1, (int)r.NodeView.Top + 1);
            Assert.IsTrue(r.Bold);

            Assert.IsTrue(sut.Active);
            FireMouseUp(0, 0);
            Assert.IsFalse(sut.Active);
        }

        [TestMethod()]
        public void Paste_OnMultipleNodes()
        {
            var sut = view.FormatPainter;
            c1.Italic = true; c2.Italic = false; r.Italic = false;
            sut.Copy(c1);
            Assert.IsFalse(c2.Italic);
            sut.Paste(new MapNode[] { r, c2 });
            Assert.IsTrue(c2.Italic);
            Assert.IsTrue(r.Italic);
            Assert.IsFalse(sut.Active);
        }
                
        [TestMethod()]
        public void Paste_OnItself()
        {
            var sut = view.FormatPainter;
            c1.Strikeout = true; c2.Strikeout = false; r.Strikeout = false;
            sut.Copy(c1);
            sut.Paste(c1);
            Assert.IsTrue(c1.Strikeout);
            Assert.IsFalse(c2.Strikeout);
            Assert.IsFalse(sut.Active);
        }

        [TestMethod()]
        public void Clear()
        {
            var sut = view.FormatPainter;
            sut.Copy(c1);
            sut.Clear();
            Assert.IsFalse(sut.Active);
        }

        [TestMethod()]
        public void Clear_ThrouhEscapeKey()
        {
            var sut = view.FormatPainter;
            sut.Copy(c1);
            FireKeyDown(Keys.Escape);
            Assert.IsFalse(sut.Active);
        }

        private void FireMouseUp(int x, int y, MouseButtons mouseButtons = MouseButtons.None)
        {
            view.Canvas.GetType().GetMethod("OnMouseUp", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(view.Canvas, new object[] {
                    new MouseEventArgs(mouseButtons, 0, x, y, 0) });
        }

        private void FireKeyDown(Keys key)
        {
            view.Canvas.GetType().GetMethod("OnKeyDown", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(view.Canvas, new object[] {
                    new KeyEventArgs(key) });
        }
    }
}