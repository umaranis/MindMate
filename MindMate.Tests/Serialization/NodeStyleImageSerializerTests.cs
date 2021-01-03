using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.MetaModel;
using MindMate.Model;
using System.Drawing;

namespace MindMate.Tests.Serialization
{
    [TestClass()]
    public class NodeStyleImageSerializerTests
    {
        [TestMethod()]
        public void SerializeImage()
        {
            MetaModel.MetaModel.Initialize();
            var refNode = new MapNode(new MapTree(), "OK");
            refNode.Italic = true;
            var nodeStyle = new NodeStyle("TestStyle1", refNode, true);
            new NodeStyleImageSerializer().SerializeImage(nodeStyle.Image, nodeStyle.Title + Guid.NewGuid());
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void SerializeImage_InvalidChar()
        {
            MetaModel.MetaModel.Initialize();
            var refNode = new MapNode(new MapTree(), "OK");
            refNode.Italic = true;
            var nodeStyle = new NodeStyle("TestStyle\"1", refNode, true);
            new NodeStyleImageSerializer().SerializeImage(nodeStyle.Image, nodeStyle.Title);
        }

        [TestMethod]
        public void DeserializeImage()
        {
            MetaModel.MetaModel.Initialize();
            var refNode = new MapNode(new MapTree(), "OK");
            refNode.Italic = true;
            var nodeStyle = new NodeStyle("TestStyle1", refNode, true);
            var fileName = nodeStyle.Title + Guid.NewGuid();
            new NodeStyleImageSerializer().SerializeImage(nodeStyle.Image, fileName);
            Bitmap image = new NodeStyleImageSerializer().DeserializeImage(fileName);
            Assert.IsNotNull(image);
        }

        [TestMethod]
        public void DeserializeImage_Nonexistant()
        {
            Bitmap image = new NodeStyleImageSerializer().DeserializeImage("ajskldfjalsdkfjasdlfjasldfj74");
            Assert.IsNull(image);
        }

        [TestMethod()]
        public void DeleteImage()
        {
            MetaModel.MetaModel.Initialize();
            var refNode = new MapNode(new MapTree(), "OK");
            refNode.Italic = true;
            var nodeStyle = new NodeStyle("TestStyle2", refNode, true);
            new NodeStyleImageSerializer().SerializeImage(nodeStyle.Image, nodeStyle.Title);
            new NodeStyleImageSerializer().DeleteImage("TestStyle2");
        }

        [TestMethod()]
        public void DeleteImage_Nonexistant()
        {
            new NodeStyleImageSerializer().DeleteImage("ajskldfjalsdkfjasdlfjasldfj74");
        }
    }
}