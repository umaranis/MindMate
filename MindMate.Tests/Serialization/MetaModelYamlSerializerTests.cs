using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using System.IO;

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
    }
}