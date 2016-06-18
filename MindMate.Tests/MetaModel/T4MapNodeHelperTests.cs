using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;

#if DEBUG

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class T4MapNodeHelperTests
    {
        [TestMethod()]
        public void GetProperties()
        {
            var sut = new T4MapNodeHelper();
            var props = sut.GetProperties();

            Assert.IsTrue(props.Count() > 10);
        }

        [TestMethod()]
        public void GetSerializedProperties()
        {
            var sut = new T4MapNodeHelper();
            Assert.IsTrue(sut.GetSerializedProperties().Count() < sut.GetProperties().Count());
        }

        [TestMethod()]
        public void GetSerializedPropertiesInOrder()
        {
            var sut = new T4MapNodeHelper();
            Assert.IsTrue(sut.GetSerializedPropertiesInOrder().Count() > 10);
        }

        [TestMethod()]
        public void GetSerializedPropertiesInOrder_Count()
        {
            var sut = new T4MapNodeHelper();
            Assert.IsTrue(sut.GetSerializedProperties().Count() == sut.GetSerializedPropertiesInOrder().Count());
        }

        [TestMethod()]
        public void GetSerializedPropertiesInOrder_TextIsFirst()
        {
            var sut = new T4MapNodeHelper();
            Assert.AreEqual("Text", sut.GetSerializedPropertiesInOrder().First().Name);
        }

        [TestMethod()]
        public void GetSerializedPropertiesInOrder_PosIsSecond()
        {
            var sut = new T4MapNodeHelper();
            Assert.AreEqual("Pos", sut.GetSerializedPropertiesInOrder().ElementAt(1).Name);
        }

        [TestMethod()]
        public void GetSerializedPropertiesInOrder_IdIsThird()
        {
            var sut = new T4MapNodeHelper();
            Assert.AreEqual("Id", sut.GetSerializedPropertiesInOrder().ElementAt(2).Name);
        }

        [TestMethod()]
        public void GetSerializedScalarPropertiesInOrder_Count()
        {
            var sut = new T4MapNodeHelper();
            Assert.IsTrue(sut.GetSerializedProperties().Count() > sut.GetSerializedScalarPropertiesInOrder().Count());
        }

        [TestMethod()]
        public void GetSerializedScalarPropertiesInOrder_StringTypeIsThere()
        {
            var sut = new T4MapNodeHelper();
            Assert.IsTrue(sut.GetSerializedScalarPropertiesInOrder().First().Name.Equals("Text"));
        }

        [TestMethod()]
        public void GetSerializedScalarPropertiesInOrder_IconsIsNotThere()
        {
            var sut = new T4MapNodeHelper();
            Assert.IsTrue(sut.GetSerializedScalarPropertiesInOrder().All(p => !p.Name.Equals("Icons")));
        }
    }
}

#endif