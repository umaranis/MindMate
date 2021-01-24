using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MindMate.Tests
{
    [TestClass()]
    public class MindMapSerializerTests
    {
        
        [TestMethod()]
        public void Deserialize_FeatureDisplay()
        {
            MindMapSerializer s = new MindMapSerializer();
            string originalText = File.ReadAllText(@"Resources\Feature Display.mm");
            MapTree tree = new MapTree();
            s.Deserialize(originalText, tree);

            Assert.AreEqual(tree.RootNode.ChildNodes.Count(), 9);
        }

        [TestMethod()]
        public void Deserialize_RootNode()
        {
            MindMapSerializer s = new MindMapSerializer();
            string XMLString = File.ReadAllText(@"Resources\Feature Display.mm");
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument(); 
            xmlDoc.LoadXml(XMLString);
            XmlElement x = xmlDoc.DocumentElement;

            var t = new MapTree();
            var r = new MapNode(t, null);

            for (int i = 0; i < x.ChildNodes.Count; i++)
            {
                XmlNode xnode = x.ChildNodes[i];
                if (xnode.Name == "node")
                {
                    s.Deserialize(xnode, t);
                }
            }

            Assert.AreEqual(t.RootNode.ChildNodes.Count(), 9);
        }

        [TestMethod]
        public void DeserializeSerialize_FeatureDisplay()
        {
            MindMapSerializer s = new MindMapSerializer();
            string originalText = File.ReadAllText(@"Resources\Feature Display.mm");
            MapTree tree = new MapTree();
            s.Deserialize(originalText, tree);

            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, tree);
                stream.Position = 0;
                string generatedText = new StreamReader(stream).ReadToEnd();

                Assert.AreEqual(originalText, generatedText, "Serialized xml doesn't match this original.");
            }
        }

        [TestMethod]
        public void DeserializeSerialize_RichNotesMap()
        {
            MindMapSerializer s = new MindMapSerializer();
            string originalText = File.ReadAllText(@"Resources\RichNotesMap.mm");
            MapTree tree = new MapTree();
            s.Deserialize(originalText, tree);

            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, tree);
                stream.Position = 0;
                string generatedText = new StreamReader(stream).ReadToEnd();

                Assert.AreEqual(originalText, generatedText, "Serialized xml doesn't match this original.");
            }
        }

        [TestMethod]
        public void Serialize_WithImagePath()
        {
            MapTree tree = new MapTree();
            var r = new MapNode(tree, "r");
            r.Image = "MyImage.png";
            r.ImageAlignment = ImageAlignment.AfterCenter;

            MindMapSerializer s = new MindMapSerializer();
            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, tree);
                stream.Position = 0;
                string serializedText = new StreamReader(stream).ReadToEnd();

                var tree2 = new MapTree();
                s.Deserialize(serializedText, tree2);
                Assert.AreEqual("MyImage.png", tree2.RootNode.Image);
                Assert.AreEqual(ImageAlignment.AfterCenter, tree2.RootNode.ImageAlignment);
            }
        }

        [TestMethod]
        public void Serialize_WithImageSize()
        {
            MapTree tree = new MapTree();
            var r = new MapNode(tree, "r");
            r.Image = "MyImage.png";
            r.ImageSize = new System.Drawing.Size(10, 20);

            MindMapSerializer s = new MindMapSerializer();
            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, tree);
                stream.Position = 0;
                string serializedText = new StreamReader(stream).ReadToEnd();

                var tree2 = new MapTree();
                s.Deserialize(serializedText, tree2);
                Assert.AreEqual("MyImage.png", tree2.RootNode.Image);
                Assert.AreEqual(10, tree2.RootNode.ImageSize.Width);
            }
        }

        [TestMethod]
        public void DeserializeSerialize_Format()
        {
            MindMapSerializer s = new MindMapSerializer();
            MapTree tree = new MapTree();
            new MapNode(tree, "root");

            tree.DefaultFormat = new NodeFormat(Color.Brown, true, Color.DarkGoldenrod, FontFamily.GenericMonospace.Name, 12, true, true,
                Color.Red, DashStyle.Dot, 4, NodeShape.Box);
            tree.CanvasBackColor = Color.Cyan;
            tree.NoteBackColor = Color.Crimson;
            tree.NoteForeColor = Color.Coral;
            tree.SelectedOutlineColor = Color.DarkOrchid;
            tree.DropHintColor = Color.DarkSalmon;

            string serializedText;
            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, tree);
                stream.Position = 0;
                serializedText = new StreamReader(stream).ReadToEnd();
            }

            var tree2 = new MapTree();
            s.Deserialize(serializedText, tree2);

            Assert.AreEqual(tree.DropHintColor, tree2.DropHintColor);
            Assert.AreEqual(tree.DefaultFormat.FontName, tree2.DefaultFormat.FontName);
            Assert.AreEqual(tree.DefaultFormat.Color, tree2.DefaultFormat.Color);
            Assert.AreEqual(tree.NoteForeColor, tree.NoteForeColor);
        }

    }
}