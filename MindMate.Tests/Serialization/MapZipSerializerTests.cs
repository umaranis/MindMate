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
            byte[] data = sut.DeserializeLargeObject(@"Resources\New Format with Images.mm", "33046437-1659-4d39-91dd-5a420e7c4852.png");
            Assert.IsTrue(data.Length > 1000);
        }

        [TestMethod()]
        public void SerializeMap()
        {
            var sut = new MapZipSerializer();
            var tree = new MapTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");

            sut.SerializeMap(tree, null, "MapZapTest1.mm", false);

            var tree2 = new MapTree();
            sut.DeserializeMap(tree2, "MapZapTest1.mm");

            Assert.AreEqual(3, tree2.RootNode.ChildNodes.Count());
        }

        [TestMethod()]
        public void SerializeMap_UpdateAfterSaving()
        {
            var sut = new MapZipSerializer();
            var tree = new MapTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");

            sut.SerializeMap(tree, null, "MapZapTest2.mm", false);

            new MapNode(root, "c4");
            new MapNode(root, "c5");

            sut.SerializeMap(tree, null, "MapZapTest2.mm", false);

            var tree2 = new MapTree();
            sut.DeserializeMap(tree2, "MapZapTest2.mm");

            Assert.AreEqual(5, tree2.RootNode.ChildNodes.Count());
        }

        [TestMethod()]
        public void SerializeMap_UpdateAfterSaving_WithLargeObject()
        {
            var lobs = new List<KeyValuePair<string, byte[]>>();
            
            var sut = new MapZipSerializer();
            var tree = new MapTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");
            lobs.Add(new KeyValuePair<string, byte[]>("1", new byte[100]));
            sut.SerializeMap(tree, lobs, "MapZapTest2.mm", false);

            new MapNode(root, "c4");
            new MapNode(root, "c5");
            lobs.Add(new KeyValuePair<string, byte[]>("2", new byte[200]));
            sut.SerializeMap(tree, lobs, "MapZapTest2.mm", false);

            var tree2 = new MapTree();
            sut.DeserializeMap(tree2, "MapZapTest2.mm");

            Assert.AreEqual(5, tree2.RootNode.ChildNodes.Count());
            Assert.AreEqual(100, sut.DeserializeLargeObject("MapZapTest2.mm", "1").Length);
            Assert.AreEqual(200, sut.DeserializeLargeObject("MapZapTest2.mm", "2").Length);
        }
    }
}