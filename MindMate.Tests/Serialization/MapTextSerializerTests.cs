using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Serialization;

namespace MindMate.Tests.Serialization
{
    [TestClass]
    public class MapTextSerializerTests
    {
        [TestMethod]
        public void Serialize()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n1 = new MapNode(r, "n1");
            var n11 = new MapNode(n1, "n11");
            var n12 = new MapNode(n1, "n12");
            var n2 = new MapNode(r, "n2");

            var sut = new MapTextSerializer();
            var str = new StringBuilder();
            sut.Serialize(r, str);

            Assert.AreEqual("root" + Environment.NewLine +
                "\tn1" + Environment.NewLine +
                "\t\tn11" + Environment.NewLine +
                "\t\tn12" + Environment.NewLine +
                "\tn2",str.ToString());
        }

        [TestMethod]
        public void Deserialize()
        {
            string str = "root" + Environment.NewLine +
                "\tn1" + Environment.NewLine +
                "\t\tn11" + Environment.NewLine +
                "\t\tn12" + Environment.NewLine +
                "\tn2";

            var n = new MapNode(new MapTree(), "1");

            var sut = new MapTextSerializer();
            sut.Deserialize(str, n, (p, t) => new MapNode(p, t));

            Assert.AreEqual(6, n.RollUpAggregate(node => 1, (a, b) => a + b));
        }
    }
}
