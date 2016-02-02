using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;

namespace MindMate.Tests.Model
{
    [TestClass]
    public class MapNodeRootTest
    {
        [TestMethod]
        public void MoveNodeUp()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            r.MoveUp();
        }

        [TestMethod]
        public void MoveNodeDown()
        {
            var t = new MapTree();
            var r = new MapNode(t, "r");

            r.MoveDown();
        }
    }
}
