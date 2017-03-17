using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.Serialization
{
    [TestClass()]
    public class PersistenceManagerTests
    {
        [TestMethod()]
        public void PersistenceManager_Creation_CurrentTreeNull()
        {
            var sut = new PersistenceManager();
            Assert.IsNull(sut.CurrentTree);
        }

        [TestMethod()]
        public void PersistenceManager_Creation_FileCountZero()
        {
            var sut = new PersistenceManager();
            Assert.AreEqual(0, sut.FileCount);
        }

        [TestMethod()]
        public void PersistenceManager_Creation_IsDirtyFalse()
        {
            var sut = new PersistenceManager();
            Assert.IsFalse(sut.IsDirty);
        }

        [TestMethod()]
        public void PersistenceManager_Creation_GetEnumerator()
        {
            var sut = new PersistenceManager();
            Assert.IsNull(sut.GetEnumerator().Current);
        }

        [TestMethod()]
        public void GetEnumerator()
        {
            var sut = new PersistenceManager();
            sut.NewTree();
            sut.NewTree();
            var count = 0;
            foreach (var tree in sut)
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        [TestMethod()]
        public void GetEnumerator_IEnumerable()
        {
            var sut = new PersistenceManager();
            sut.NewTree();
            sut.NewTree();
            var count = 0;
            foreach (var tree in ((IEnumerable)sut))
            {
                count++;
            }
            Assert.AreEqual(2, count);
        }

        [TestMethod()]
        public void Find_EmptyTree()
        {
            var sut = new PersistenceManager();
            Predicate<PersistentTree> del = t => true;
            Assert.IsNull(sut.Find(del));

            del(null); //for code coverage
        }

        [TestMethod()]
        public void Find()
        {
            var sut = new PersistenceManager();
            var pTree = sut.NewTree();
            pTree.RootNode.Text = "changed";
            Assert.IsNotNull(sut.Find(t => t.IsDirty));
        }

        [TestMethod()]
        public void Find_NoMatch()
        {
            var sut = new PersistenceManager();
            sut.NewTree();
            sut.NewTree();
            Assert.IsNull(sut.Find(t => t.IsDirty));
        }

        [TestMethod()]
        public void NewTree_IsNewMap()
        {
            var sut = new PersistenceManager();
            Assert.IsTrue(sut.NewTree().IsNewMap);
        }

        [TestMethod()]
        public void NewTree_IsDirty()
        {
            var sut = new PersistenceManager();
            Assert.IsFalse(sut.NewTree().IsDirty);
        }

        [TestMethod()]
        public void OpenTree_IsDirty()
        {
            var sut = new PersistenceManager();
            var pTree = sut.OpenTree(@"Resources\Feature Display.mm");
            Assert.IsFalse(pTree.IsDirty);
        }

        [TestMethod()]
        public void OpenTree_IsNewMap()
        {
            var sut = new PersistenceManager();
            var pTree = sut.OpenTree(@"Resources\Feature Display.mm");
            Assert.IsFalse(pTree.IsNewMap);
        }

        [TestMethod()]
        public void CloseCurerntTree()
        {
            var sut = new PersistenceManager();
            var pTree = sut.NewTree();
            sut.CloseCurerntTree();
            Assert.IsNull(sut.CurrentTree);
        }

        [TestMethod()]
        public void CloseCurerntTree_MultipleTree()
        {
            var sut = new PersistenceManager();
            sut.NewTree();
            sut.NewTree();
            sut.CloseCurerntTree();
            Assert.IsNull(sut.CurrentTree);
        }

        [TestMethod()]
        public void Close()
        {
            var sut = new PersistenceManager();
            var pTree1 = sut.NewTree();
            var pTree2 = sut.NewTree();
            sut.Close(pTree2);
            Assert.IsNull(sut.CurrentTree);
        }

        [TestMethod()]
        public void CurrentTree_Get()
        {
            var sut = new PersistenceManager();
            var pTree = sut.NewTree();
            Assert.AreEqual(pTree, sut.CurrentTree);
        }

        [TestMethod()]
        public void CurrentTree_Set()
        {
            var sut = new PersistenceManager();
            var pTree1 = sut.NewTree();
            var pTree2 = sut.NewTree();
            sut.CurrentTree = pTree1;
            Assert.AreEqual(pTree1, sut.CurrentTree);
        }

        [TestMethod()]
        public void CurrentTree_SetValueToItself()
        {
            var sut = new PersistenceManager();
            var change = false;
            PersistenceManager.CurrentTreeChangedDelete del = (manager, tree, newTree) => change = true;
            var pTree1 = sut.NewTree();
            sut.CurrentTreeChanged += del;
            sut.CurrentTree = pTree1;
            
            Assert.IsFalse(change);

            del(null, null, null); //for code coverage
        }

        [TestMethod()]
        public void NewTree_BecomesCurrent()
        {
            var sut = new PersistenceManager();
            var pTree1 = sut.NewTree();
            var pTree2 = sut.NewTree();
            Assert.AreEqual(pTree2, sut.CurrentTree);
        }

        [TestMethod()]
        public void IsDirty()
        {
            var sut = new PersistenceManager();
            var pTree = sut.NewTree();
            pTree.RootNode.Text = "changed";
            Assert.IsTrue(sut.IsDirty);
        }

        [TestMethod()]
        public void IndexerProperty()
        {
            var sut = new PersistenceManager();
            var pTree1 = sut.NewTree();
            var pTree2 = sut.NewTree();
            Assert.AreEqual(pTree2, sut[1]);
        }

        [TestMethod()]
        public void DirtyChanged()
        {
            var sut = new PersistenceManager();
            var pTree1 = sut.NewTree();
            var result = false;
            pTree1.DirtyChanged += tree => result = true;
            pTree1.RootNode.Text = "changed";
            Assert.IsTrue(result);
            Assert.IsTrue(sut.IsDirty);
        }

        [TestMethod()]
        public void CurrentTreeChangedEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.CurrentTreeChanged += (manager, tree, newTree) => result = true;
            var pTree1 = sut.NewTree();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NewTreeCreatingEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.NewTreeCreating += (manager, tree) => { if (sut.FileCount == 0) result = true; };
            var pTree1 = sut.NewTree();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NewTreeCreatedEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.NewTreeCreated += (manager, tree) => { if (sut.FileCount == 1) result = true; };
            var pTree1 = sut.NewTree();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TreeClosingEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.TreeClosing += (manager, tree) => { if (sut.FileCount == 1) result = true; };
            var pTree1 = sut.NewTree();
            sut.CloseCurerntTree();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TreeClosedEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.TreeClosed += (manager, tree) => { if (sut.FileCount == 0) result = true; };
            var pTree1 = sut.NewTree();
            sut.CloseCurerntTree();
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TreeOpeningEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.TreeOpening += (manager, tree) => { if (sut.FileCount == 0) result = true; };
            var pTree = sut.OpenTree(@"Resources\Feature Display.mm");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TreeOpenedEvent()
        {
            var sut = new PersistenceManager();
            var result = false;
            sut.TreeOpened += (manager, tree) => { if (sut.FileCount == 1) result = true; };
            var pTree = sut.OpenTree(@"Resources\Feature Display.mm");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TreeSavedEvent()
        {
            var sut = new PersistenceManager();
            var pTree = sut.NewTree();
            var result = false;
            sut.TreeSaved += (manager, tree) => result = true; 
            pTree.RootNode.Text = "changed";
            pTree.Save("temp.mm");
            Assert.IsTrue(result);
        }
    }
}