using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using System.IO;
using MindMate.MetaModel;
using MindMate.Model;
using MindMate.Tests.TestDouble;

namespace MindMate.Tests.Serialization
{
    [TestClass()]
    public class MetaModelYamlSerializerTests
    {
        [TestMethod()]
        public void SerializeToString()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new MetaModelYamlSerializer();
            var textWriter = new StringWriter();
            sut.Serialize(MetaModel.MetaModel.Instance, textWriter);
            var result = textWriter.ToString();

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void SerializeToString_IconsSectionExists()
        {
            MetaModel.MetaModel.Initialize();
            var sut = new MetaModelYamlSerializer();
            var textWriter = new StringWriter();
            sut.Serialize(MetaModel.MetaModel.Instance, textWriter);
            var result = textWriter.ToString();

            Assert.IsTrue(result.Contains("icons"));
        }

        [TestMethod()]
        public void SerializeToString_EmptyMetaModel()
        {
            var model = new MetaModel.MetaModel();
            model.IconsList.Add(new ModelIcon("button_ok", "ok", "k"));
            var sut = new MetaModelYamlSerializer();
            var textWriter = new StringWriter();
            sut.Serialize(model, textWriter);
            var result = textWriter.ToString();

            Assert.IsTrue(result.Contains("icons"));
        }

        [TestMethod()]
        public void SerializeToString_NodeStyle()
        {
            var model = MetaModelHelper.Create();
            model.IconsList.Add(new ModelIcon("button_ok", "ok", "k"));
            var refNode = new MapNode(new MapTree(), "Text");
            refNode.Bold = true;
            refNode.FontSize = 15;
            model.NodeStyles.Add(new NodeStyle("Stylish", refNode, false));
            var sut = new MetaModelYamlSerializer();
            var textWriter = new StringWriter();
            sut.Serialize(model, textWriter);
            var result = textWriter.ToString();

            Assert.IsTrue(result.Contains(MetaModelYamlSerializer.NodeStyles));
            Assert.IsTrue(result.Contains("Stylish"));
            MetaModelHelper.CreateWithDefaultSettingsFile();
        }

        [TestMethod()]
        public void SerializeToString_NodeStyle_NoLinePattern()
        {
            MetaModel.MetaModel.Initialize(); //required by StyleImageGenerator
            var model = new MetaModel.MetaModel();
            model.IconsList.Add(new ModelIcon("button_ok", "ok", "k"));
            var refNode = new MapNode(new MapTree(), "Text");
            refNode.Bold = true;
            refNode.FontSize = 15;
            model.NodeStyles.Add(new NodeStyle("Stylish", refNode, false));
            var sut = new MetaModelYamlSerializer();
            var textWriter = new StringWriter();
            sut.Serialize(model, textWriter);
            var result = textWriter.ToString();

            Assert.IsFalse(result.Contains(MapYamlSerializer.LinePattern));
        }

        [TestMethod()]
        public void SerializeToString_NoNodeStyle()
        {
            MetaModel.MetaModel.Initialize(); //required by StyleImageGenerator
            var model = new MetaModel.MetaModel();
            model.IconsList.Add(new ModelIcon("button_ok", "ok", "k"));
            var sut = new MetaModelYamlSerializer();
            var textWriter = new StringWriter();
            sut.Serialize(model, textWriter);
            var result = textWriter.ToString();

            Assert.IsFalse(result.Contains(MetaModelYamlSerializer.NodeStyles));
        }



        [TestMethod()]
        public void Deserialize_CheckIconsCount()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\Settings.Yaml");
            
            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual(71, model.IconsList.Count);
        }

        [TestMethod()]
        public void Deserialize_RecentFilesCount()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\Settings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual(8, model.RecentFiles.Count);
        }

        [TestMethod()]
        public void Deserialize_DefaultFile_CheckIconsCount()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\DefaultSettings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual(71, model.IconsList.Count);
        }

        [TestMethod()]
        public void Deserialize_DefaultFile_RecentFilesEmpty()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\DefaultSettings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual(0, model.RecentFiles.Count);
        }

        [TestMethod()]
        public void Deserialize_NodeStyles()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\Settings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual(2, model.NodeStyles.Count);
        }

        [TestMethod()]
        public void Deserialize_NodeStyle_Title()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\Settings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual("Good Style", model.NodeStyles[0].Title);
        }

        [TestMethod()]
        public void Deserialize_NodeStyle_RefNodeLabel()
        {
            MetaModel.MetaModel model = new MetaModel.MetaModel();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\Settings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.AreEqual("This is label", model.NodeStyles[1].RefNode.Label);
        }

        /// <summary>
        /// MetaModelYamlSerializer doesn't deserialize NodeStyle images. It is taken care of by MetaModel class itself.
        /// </summary>
        [TestMethod()]
        public void Deserialize_NodeStyle_ImageNull()
        {
            MetaModel.MetaModel model = MetaModelHelper.Create();
            var sut = new MetaModelYamlSerializer();
            var textReader = new StreamReader(@"Resources\Settings.Yaml");

            sut.Deserialize(model, textReader);
            textReader.Close();

            Assert.IsNull(model.NodeStyles[0].Image);
            Assert.IsNull(model.NodeStyles[1].Image);
            MetaModelHelper.CreateWithDefaultSettingsFile();
        }
    }
}