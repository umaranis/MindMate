using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.Serialization
{
    [TestClass()]
    public class PersistentTreeTests
    {
        [TestMethod()]
        public void Save()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.Tree.RootNode.Text = "Testing";
            tree.Save("PersistentTreeTests1.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests1.mm");
            Assert.AreEqual("Testing", tree2.Tree.RootNode.Text);
        }

        [TestMethod()]
        public void Save_WithLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.Tree.RootNode.Text = "Testing";
            tree.SetByteArray("t", new byte[] { 1, 2, 3, 4 });
            tree.Save("PersistentTreeTests2.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests2.mm");
            Assert.AreEqual(3, tree2.GetByteArray("t")[2]);
        }

        [TestMethod()]
        public void Save_AddLargeObjectAndOverwrite()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.Tree.RootNode.Text = "Testing";
            tree.SetByteArray("t", new byte[] { 1, 2, 3, 4 });
            tree.Save("PersistentTreeTests3.mm");

            tree.SetByteArray("v", new byte[] { 5, 6, 7, 8 });
            tree.Save("PersistentTreeTests3.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests3.mm");

            Assert.AreEqual(3, tree2.GetByteArray("t")[2]);
            Assert.AreEqual(7, tree2.GetByteArray("v")[2]);
        }

        [TestMethod()]
        public void Save_AppendLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.Tree.RootNode.Text = "Testing";
            tree.SetByteArray("t", new byte[] { 1, 2, 3, 4 });
            tree.Save("PersistentTreeTests4.mm");

            tree.SetByteArray("v", new byte[] { 5, 6, 7, 8 });
            tree.Save();

            var tree2 = manager.OpenTree("PersistentTreeTests4.mm");

            Assert.AreEqual(3, tree2.GetByteArray("t")[2]);
            Assert.AreEqual(7, tree2.GetByteArray("v")[2]);
        }

        [TestMethod()]
        public void SetByteArray()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.Tree.RootNode.Text = "Testing";
            tree.SetByteArray("t", new byte[] { 1, 2, 3, 4 });
            tree.Save("PersistentTreeTests5.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests5.mm");

            Assert.AreEqual(3, tree2.GetByteArray("t")[2]);
        }

        [TestMethod()]
        public void GetByteArray()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.Tree.RootNode.Text = "Testing";
            tree.SetByteArray("t", new byte[] { 1, 2, 3, 4 });                

            Assert.AreEqual(3, tree.GetByteArray("t")[2]);
        }

        
    }
}