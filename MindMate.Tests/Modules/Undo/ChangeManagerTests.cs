using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Modules.Undo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using System.Drawing;

namespace MindMate.Tests.Modules.Undo
{
    [TestClass()]
    public class ChangeManagerTests
    {
        [TestMethod()]
        public void ChangeManager()
        {
            ChangeManager manager = new ChangeManager();
            Assert.IsFalse(manager.CanRedo);
            Assert.IsFalse(manager.CanUndo);
        }

        [TestMethod()]
        public void Undo_WithEmptyStack()
        {
            ChangeManager manager = new ChangeManager();
            manager.Undo();
        }

        [TestMethod()]
        public void Redo_WithEmptyStack()
        {
            ChangeManager manager = new ChangeManager();
            manager.Redo();
        }

        [TestMethod()]
        public void RegisterMap()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            ChangeManager manager = new ChangeManager();
            manager.RegisterMap(t);

            r.Text = "changed";

            Assert.IsTrue(manager.CanUndo);
        }

        [TestMethod()]
        public void Unregister()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            ChangeManager manager = new ChangeManager();
            manager.RegisterMap(t);
            manager.Unregister(t);

            r.Text = "changed";

            Assert.IsFalse(manager.CanUndo);
        }

        [TestMethod()]
        public void StartBatch()
        {
            ChangeManager manager = new ChangeManager();
            manager.StartBatch("batch");

            Assert.IsTrue(manager.IsBatchOpen);
        }

        [TestMethod()]
        public void EndBatch_EmptyBatch()
        {
            ChangeManager manager = new ChangeManager();
            manager.StartBatch("Empty Batch");
            manager.EndBatch();
            Assert.IsFalse(manager.CanUndo);
        }

        [TestMethod]
        public void ImageChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            r.Image = "image";

            Assert.AreEqual(1, sut.UndoStackCount);
        }

        [TestMethod]
        public void ImageChange_Undo()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            r.Image = "image";
            sut.Undo();

            Assert.AreEqual(0, sut.UndoStackCount);
            Assert.IsNull(r.Image);
        }

        [TestMethod]
        public void ImageAlignmentChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            r.ImageAlignment = ImageAlignment.AfterBottom;

            Assert.AreEqual(1, sut.UndoStackCount);
            Assert.AreEqual(ImageAlignment.AfterBottom, r.ImageAlignment);
        }

        [TestMethod]
        public void ImageAlignmentChange_Undo()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            r.ImageAlignment = ImageAlignment.AfterBottom;
            sut.Undo();

            Assert.AreEqual(0, sut.UndoStackCount);
            Assert.AreNotEqual(ImageAlignment.AfterBottom, r.ImageAlignment);
        }

        [TestMethod]
        public void ImageSizeChange()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            r.ImageSize = new Size(15, 10);

            Assert.AreEqual(1, sut.UndoStackCount);
            Assert.AreEqual(10, r.ImageSize.Height);
        }

        [TestMethod]
        public void ImageSizeChange_Undo()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            r.ImageSize = new Size(15, 10);
            sut.Undo();

            Assert.AreEqual(0, sut.UndoStackCount);
            Assert.AreNotEqual(15, r.ImageSize.Width);
        }

        [TestMethod]
        public void ChangeRecorded()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");
            t.TurnOnChangeManager();
            var sut = t.ChangeManager;

            var result = false;
            sut.ChangeRecorded += (m, c) => result = true;
            
            r.Text = "changed";          

            Assert.IsTrue(result);
        }

        
    }
}