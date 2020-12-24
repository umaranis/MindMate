using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.View.MapControls
{
    [TestClass()]
    public class NodeFormatTests
    {
        [TestMethod()]
        public void CreateDefaultFormat_FontNotNull()
        {
            Assert.IsNotNull(NodeFormat.CreateDefaultFormat().Font);
        }

        [TestMethod()]
        public void CreateDefaultFormat_SameDefaultObject()
        {
            Assert.AreEqual(NodeFormat.CreateDefaultFormat(), NodeFormat.CreateDefaultFormat());
        }

        [TestMethod()]
        public void CreateNodeFormat()
        {
            var n = new MapNode(new MapTree(), "");
            Assert.IsNotNull(NodeFormat.CreateNodeFormat(n));
        }

        [TestMethod()]
        public void CreateNodeFormat_NoFormating_ReturnsDefault()
        {
            var n = new MapNode(new MapTree(), "");
            Assert.AreEqual(NodeFormat.CreateNodeFormat(n), NodeFormat.CreateDefaultFormat());
        }
    }
}