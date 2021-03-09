using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaFan.ImageComparison;

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
            tree.RootNode.Text = "Testing";
            tree.Save("PersistentTreeTests1.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests1.mm");
            Assert.AreEqual("Testing", tree2.RootNode.Text);
        }

        [TestMethod()]
        public void Save_WithIcon()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.RootNode.Icons.Add("stop");
            tree.Save("PersistentTreeTests_Icon.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests_Icon.mm");
            Assert.AreEqual("stop", tree.RootNode.Icons[0]);
        }

        [TestMethod()]
        public void Save_WithAttribute()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.RootNode.Icons.Add("stop");
            tree.RootNode.AddAttribute("att1", "value1");
            tree.Save("PersistentTreeTests_Icon.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests_Icon.mm");
            Assert.AreEqual("value1", tree.RootNode.GetAttribute(0).Value);
        }

        [TestMethod()]
        public void Save_WithLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));
            tree.Save("PersistentTreeTests2.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests2.mm");
            Assert.AreEqual(3, tree2.GetLargeObject<BytesLob>("t").Bytes[2]);
        }

        [TestMethod()]
        public void Save_AddLargeObjectAndOverwrite()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));
            tree.Save("PersistentTreeTests3.mm");

            tree.SetLargeObject("v", new BytesLob(new byte[] { 5, 6, 7, 8 }));
            tree.Save("PersistentTreeTests3.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests3.mm");

            Assert.AreEqual(3, tree2.GetLargeObject<BytesLob>("t").Bytes[2]);
            Assert.AreEqual(7, tree2.GetLargeObject< BytesLob>("v").Bytes[2]);
        }

        [TestMethod()]
        public void Save_AppendLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));
            tree.Save("PersistentTreeTests4.mm");

            tree.SetLargeObject("v", new BytesLob(new byte[] { 5, 6, 7, 8 }));
            tree.Save();

            var tree2 = manager.OpenTree("PersistentTreeTests4.mm");

            Assert.AreEqual(3, tree2.GetLargeObject<BytesLob>("t").Bytes[2]);
            Assert.AreEqual(7, tree2.GetLargeObject<BytesLob>("v").Bytes[2]);
        }

        [TestMethod()]
        public void Save_AppendLargeObject_Image()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            Image image1 = Image.FromFile(@"Resources\MapCtrl1.png");
            tree.SetLargeObject("t", new ImageLob(image1));
            tree.Save("PersistentTreeTests5.mm");

            tree.SetLargeObject("v", new ImageLob(Image.FromFile(@"Resources\MapCtrl2.png")));
            tree.Save();

            var tree2 = manager.OpenTree("PersistentTreeTests5.mm");

            Assert.IsTrue(tree2.TryGetLargeObject("t", out ImageLob lob1));
            Assert.IsTrue(tree2.TryGetLargeObject("v", out ImageLob lob2));
            Assert.AreNotEqual(image1, lob1.Image); //reference are not equal, but images are same
            Assert.AreEqual(0.0f, lob1.Image.PercentageDifference(image1, 0));
        }

        [TestMethod()]
        public void SetLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));
            tree.Save("PersistentTreeTests6.mm");

            var tree2 = manager.OpenTree("PersistentTreeTests6.mm");

            Assert.AreEqual(3, tree2.GetLargeObject<BytesLob>("t").Bytes[2]);
        }

        [TestMethod()]
        public void GetLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));

            Assert.AreEqual(3, tree.GetLargeObject<BytesLob>("t").Bytes[2]);
        }

        [TestMethod()]
        public void TryGetLargeObject_Twice()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));

            tree.TryGetLargeObject("t", out BytesLob obj);
            
            Assert.AreEqual(3, obj.Bytes[2]);
        }

        [TestMethod()]
        public void RemoveLargeObject()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));

            tree.RemoveLargeObject("t");

            Assert.IsFalse(tree.TryGetLargeObject("t", out BytesLob obj));
        }

        [TestMethod()]
        public void RemoveLargeObject_Nonexistent()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";            

            tree.RemoveLargeObject("t");

            Assert.IsFalse(tree.TryGetLargeObject("t", out BytesLob obj));
        }

        [TestMethod()]
        public void RemoveLargeObject_AfterSaving()
        {
            var manager = new PersistenceManager();
            var tree = manager.NewTree();
            tree.RootNode.Text = "Testing";
            tree.SetLargeObject("t", new BytesLob(new byte[] { 1, 2, 3, 4 }));
            tree.Save("PersistentTreeTests_Remove1.mm");

            tree.RemoveLargeObject("t");
            tree.Save();

            Assert.IsFalse(tree.TryGetLargeObject("t", out BytesLob obj));
        }

        /// <summary>
        /// Open file in old Xml format and then save it in new Zip format
        /// </summary>
        [TestMethod()]
        public void OpenXmlAndSaveNewZipFormat()
        {
            var manager = new PersistenceManager();
            var tree = manager.OpenTree(@"Resources\OldFormat_OverWritten_PersistentTree.mm");
            tree.RootNode.Text = "Testing";
            tree.Save();

            var tree2 = manager.OpenTree(@"Resources\OldFormat_OverWritten_PersistentTree.mm");
            Assert.AreEqual("Testing", tree2.RootNode.Text);
        }

        /// <summary>
        /// Ensure lazy loaded large objects are not missed in SaveAs operation
        /// </summary>
        [TestMethod]
        public void SaveAsTest()
        {
            var manager = new PersistenceManager();
            var tree = manager.OpenTree(@"Resources\Html Code Cleaner.mm");
            Assert.AreEqual(0, tree.LargeObjectsDictionary.Count()); // no large objects loaded yet
            tree.Save(@"Resources\Html Code Cleaner - SaveAs.mm");
            Assert.AreEqual(8, tree.LargeObjectsDictionary.Count()); // all large objects loaded
        }


    }
}