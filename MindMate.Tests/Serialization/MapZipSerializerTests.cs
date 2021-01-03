using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.Serialization
{
    [TestClass()]
    public class MapZipSerializerTests
    {
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void DeserializeMap_XmlMapFile()
        {
            var sut = new MapZipSerializer();
            sut.DeserializeMap(new MapTree(), @"Resources\Sample Map.mm");            
        }

        [TestMethod()]
        public void DeserializeMap_ZipMapFile()
        {
            var sut = new MapZipSerializer();
            sut.DeserializeMap(new MapTree(), @"Resources\New Format.mm");
        }

        [TestMethod()]
        public void DeserializeMap_ZipFileWithImages()
        {
            var sut = new MapZipSerializer();
            sut.DeserializeMap(new MapTree(), @"Resources\New Format with Images.mm");
        }

        [TestMethod()]
        public void DeserializeMap_LargeObject()
        {
            var sut = new MapZipSerializer();
            var data = sut.DeserializeLargeObject<BytesLob>(@"Resources\New Format with Images.mm", "33046437-1659-4d39-91dd-5a420e7c4852.png");
            Assert.IsTrue(data.Bytes.Length > 1000);
        }

        [TestMethod()]
        public void SerializeMap()
        {
            var sut = new MapZipSerializer();
            var tree = new PersistenceManager().NewTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");

            sut.SerializeMap(tree, "MapZapTest1.mm", false);

            var tree2 = new MapTree();
            sut.DeserializeMap(tree2, "MapZapTest1.mm");

            Assert.AreEqual(3, tree2.RootNode.ChildNodes.Count());
        }

        [TestMethod()]
        public void SerializeMap_UpdateAfterSaving()
        {
            var sut = new MapZipSerializer();
            var tree = new PersistenceManager().NewTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");

            sut.SerializeMap(tree, "MapZapTest2.mm", false);

            new MapNode(root, "c4");
            new MapNode(root, "c5");

            sut.SerializeMap(tree, "MapZapTest2.mm", false);

            var tree2 = new MapTree();
            sut.DeserializeMap(tree2, "MapZapTest2.mm");

            Assert.AreEqual(5, tree2.RootNode.ChildNodes.Count());
        }

        [TestMethod()]
        public void SerializeMap_UpdateAfterSaving_WithLargeObject()
        {
            const string fileName = "MapZapTest3.mm";
            var sut = new MapZipSerializer();
            var tree = new PersistenceManager().NewTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");
            tree.SetLargeObject("1", new BytesLob(new byte[100]));            
            sut.SerializeMap(tree, fileName, false);

            new MapNode(root, "c4");
            new MapNode(root, "c5");
            tree.SetLargeObject("2", new BytesLob(new byte[200]));
            sut.SerializeMap(tree, fileName, false);

            var tree2 = new MapTree();
            sut.DeserializeMap(tree2, fileName);

            Assert.AreEqual(5, tree2.RootNode.ChildNodes.Count());
            Assert.AreEqual(100, sut.DeserializeLargeObject<BytesLob>(fileName, "1").Bytes.Length);
            Assert.AreEqual(200, sut.DeserializeLargeObject<BytesLob>(fileName, "2").Bytes.Length);
        }
    }
}