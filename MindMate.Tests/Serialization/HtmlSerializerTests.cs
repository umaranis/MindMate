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
            MetaModel.MetaModel.Initialize();
            var tree = new PersistenceManager().NewTree();
            var root = new MapNode(tree, "Center");
            new MapNode(root, "c1");
            new MapNode(root, "c2");
            new MapNode(root, "c3");

            var sut = new HtmlSerializer();
            sut.Serialize(tree, "HtmlSerializerTests_1.html");
            Assert.IsTrue(File.Exists("HtmlSerializerTests_1.html"));
        }

        [TestMethod]
        public void ExportBigFile()
        {
            MetaModel.MetaModel.Initialize();

            MindMapSerializer s = new MindMapSerializer();
            string originalText = File.ReadAllText(@"Resources\Sample Map.mm");
            MapTree tree = new MapTree();
            s.Deserialize(originalText, tree);

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

            string output = File.ReadAllText("HtmlSerializerTests_2.html");
            Assert.IsTrue(output.Contains("FIFA World Cup"));   // note
            Assert.IsTrue(output.Contains("ksmiletris.png"));   // icon
            Assert.IsTrue(output.Contains("ImageLob-"));        // image
            Assert.IsTrue(output.Contains("BytesLob-"));    // image in a note

        }

        [TestMethod]
        public void CompleteFeatureTest2()
        {
            MetaModel.MetaModel.Initialize();
            var m = new PersistenceManager();
            var tree = m.OpenTree(@"Resources\Feature Display Test.mm");

            var sut = new HtmlSerializer();
            sut.Serialize(tree, "HtmlSerializerTests_3.html");

            string output = File.ReadAllText("HtmlSerializerTests_3.html");
            Assert.IsTrue(output.Contains("Umar Anis"));        // node text            
            Assert.IsTrue(output.Contains("FIFA World Cup"));   // note            
            Assert.IsTrue(output.Contains("ImageLob-"));        // image
            Assert.IsTrue(output.Contains("BytesLob-"));        // image in a note

        }
    }
}
