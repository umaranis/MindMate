using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class NodeStyleTests
    {
        [TestMethod()]
        public void NodeStyle_RefNodeIsNull()
        {
            var sut = new NodeStyle();
            Assert.IsNull(sut.RefNode);
        }

        [TestMethod()]
        public void NodeStyle_ImageIsNull()
        {
            var sut = new NodeStyle();
            Assert.IsNull(sut.Image);
        }

        [TestMethod()]
        public void NodeStyle_ImageIsNotNull()
        {
            MetaModel.MetaModel.Initialize();
            var node = new MapNode(new MapTree(), "Node");
            var sut = new NodeStyle("Sample", node);
            Assert.IsNotNull(sut.Image);
        }

        [TestMethod()]
        public void NodeStyle_SourceNodeNotEqualRefNode()
        {
            MetaModel.MetaModel.Initialize();
            var node = new MapNode(new MapTree(), "Node");
            var sut = new NodeStyle("Sample", node);
            Assert.AreNotEqual(node, sut.RefNode);
        }

        [TestMethod]
        public void NodeStyle_NodePositionShouldBeLeft()
        {
            MetaModel.MetaModel.Initialize();
            var root = new MapNode(new MapTree(), "Node");
            var node = new MapNode(root, "Node", NodePosition.Right);
            var sut = new NodeStyle("Sample", node);
            Assert.AreEqual(NodePosition.Right, sut.RefNode.Pos);
        }

        [TestMethod()]
        public void ApplyTo()
        {
            MetaModel.MetaModel.Initialize();
            var root = new MapNode(new MapTree(), "Node");
            var node = new MapNode(root, "Node", NodePosition.Right);
            node.Bold = true;
            var sut = new NodeStyle("Sample", node);
            
            sut.ApplyTo(root);

            Assert.IsTrue(root.Bold);
        }

        [TestMethod()]
        public void ApplyTo_List()
        {
            MetaModel.MetaModel.Initialize();
            var r = new MapNode(new MapTree(), "root");
            r.FontSize = 15;
            var c1 = new MapNode(r, "c1", NodePosition.Right);
            var c2 = new MapNode(r, "c2");
            var sut = new NodeStyle("Sample", r);
            var a = new [] {c1, c2};

            sut.ApplyTo(a);

            Assert.AreEqual(15, c1.FontSize);
            Assert.AreEqual(15, c2.FontSize);
        }
    }
}