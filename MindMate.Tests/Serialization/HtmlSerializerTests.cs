using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using System;
using System.IO;

namespace MindMate.Tests.Serialization
{
    [TestClass]
    public class HtmlSerializerTests
    {
        [TestMethod]
        public void CreateFile()
        {
            var sut = new HtmlSerializer();

            var tree = new PersistenceManager().NewTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");

            sut.Serialize(tree, "HtmlSerializerTests_1.html");
            Assert.IsTrue(File.Exists("HtmlSerializerTests_1.html"));
        }

        [TestMethod]
        public void ExportBigFile()
        {
            MetaModel.MetaModel.Initialize();
            var m = new PersistenceManager();
            var tree = m.OpenTree(@"Resources\Sample Map.mm");

            var sut = new HtmlSerializer();
            sut.Serialize(tree, "HtmlSerializerTests_1.html");

            Assert.IsTrue(File.ReadAllText("HtmlSerializerTests_1.html").Contains("sample data for testing purposes"));

        }

        [TestMethod]
        public void CompleteFeatureTest()
        {
            MetaModel.MetaModel.Initialize();
            var m = new PersistenceManager();
            var tree = m.OpenTree(@"Resources\Feature Display3.mm");

            var sut = new HtmlSerializer();
            sut.Serialize(tree, "HtmlSerializerTests_2.html");

            //TOOD: Assert.IsTrue(File.ReadAllText("HtmlSerializerTests_2.html").Contains("sample data for testing purposes"));

        }
    }
}
