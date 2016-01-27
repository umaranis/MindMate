using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Modules.Undo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;

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
    }
}