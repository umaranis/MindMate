using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace MindMate.Tests.Serialization
{
    [TestClass()]
    public class MapYamlSerializerTests
    {
        [TestMethod()]
        public void Serialize()
        {
            var sut = new MapYamlSerializer();
            var node = MapNode.CreateIsolatedNode(NodePosition.Left);
            node.Text = "Node1";
            node.Label = "This is the Label";
            node.Color = Color.Azure;

            var writer = new StringWriter();
            var emitter = new Emitter(writer);

            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());

            sut.Serialize(node, emitter);

            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());

            Assert.IsNotNull(writer.ToString());
        }

        [TestMethod()]
        public void Serialize_ContainsColor()
        {
            var sut = new MapYamlSerializer();
            var node = MapNode.CreateIsolatedNode(NodePosition.Left);
            node.Text = "Node1";
            node.Label = "This is the Label";
            node.Color = Color.Azure;

            var writer = new StringWriter();
            var emitter = new Emitter(writer);

            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());

            sut.Serialize(node, emitter);

            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());

            Assert.IsTrue(writer.ToString().Contains(MapYamlSerializer.Color + ":"));
        }

        [TestMethod()]
        public void Serialize_NotContainsBackColor()
        {
            var sut = new MapYamlSerializer();
            var node = MapNode.CreateIsolatedNode(NodePosition.Left);
            node.Text = "Node1";
            node.Label = "This is the Label";
            node.Color = Color.Azure;

            var writer = new StringWriter();
            var emitter = new Emitter(writer);

            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());

            sut.Serialize(node, emitter);

            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());

            Assert.IsFalse(writer.ToString().Contains(MapYamlSerializer.BackColor));
        }

        [TestMethod()]
        public void Deserialize()
        {
            var sut = new MapYamlSerializer();
            var node = MapNode.CreateIsolatedNode(NodePosition.Left);
            node.Text = "Node1";
            node.Label = "This is the Label";
            node.Color = Color.Azure;

            var writer = new StringWriter();
            var emitter = new Emitter(writer);

            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());

            sut.Serialize(node, emitter);

            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());

            string text = writer.ToString();

            var parser = new Parser(new StringReader(text));
            var eventReader = new EventReader(parser);
            eventReader.Expect<StreamStart>();
            eventReader.Expect<DocumentStart>();
            var result = sut.Deserialize(eventReader);
            eventReader.Expect<DocumentEnd>();
            eventReader.Expect<StreamEnd>();

            Assert.AreEqual(Color.Azure, result.Color);
            Assert.IsNull(result.Image);
            Assert.AreEqual("This is the Label", result.Label);
        }

        /// <summary>
        /// last node property (Label) doesn't exists in serialized string used for deserialization
        /// </summary>
        [TestMethod()]
        public void Deserialize_LastPropertyEmpty()
        {
            var sut = new MapYamlSerializer();
            var node = MapNode.CreateIsolatedNode(NodePosition.Left);
            node.Text = "Node1";
            node.Color = Color.Azure;

            var writer = new StringWriter();
            var emitter = new Emitter(writer);

            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());

            sut.Serialize(node, emitter);

            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());

            string text = writer.ToString();

            var parser = new Parser(new StringReader(text));
            var eventReader = new EventReader(parser);
            eventReader.Expect<StreamStart>();
            eventReader.Expect<DocumentStart>();
            var result = sut.Deserialize(eventReader);
            eventReader.Expect<DocumentEnd>();
            eventReader.Expect<StreamEnd>();

            Assert.AreEqual(Color.Azure, result.Color);
            Assert.IsNull(result.Image);
            Assert.IsNull(result.Label);
        }
    }
}