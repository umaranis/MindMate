using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.MetaModel;
using MindMate.Model;
using XnaFan.ImageComparison;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class StyleImageGeneratorTests
    {
        public const bool SaveActualImage = true;

        [TestMethod()]
        public void GenerateImage()
        {
            MetaModel.MetaModel.Initialize();
            var n = new MapNode(
                new MapNode(new MapTree(), null),
                "Sample",
                NodePosition.Left);
            var sut = new StyleImageGenerator(n);
            using (var image = sut.GenerateImage())
            {
                using (var refImage = Image.FromFile(@"Resources\NodeStyle-GenerateImage.bmp"))
                {
                    Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0));
                }
                if (SaveActualImage) image.Save(@"Resources\NodeStyle-GenerateImage-Actual.bmp");
            }
        }

        [TestMethod()]
        public void GetNodeView()
        {
            MetaModel.MetaModel.Initialize();
            var n = new MapNode(
                new MapNode(new MapTree(), null),
                "Sample",
                NodePosition.Left);
            var sut = new StyleImageGenerator(n);
            var image = sut.GenerateImage();

            Assert.IsNotNull(sut.GetNodeView(n));
        }

        [TestMethod]
        public void HighlightedNode()
        {
            MetaModel.MetaModel.Initialize();
            var n = new MapNode(
                new MapNode(new MapTree(), null),
                "Sample",
                NodePosition.Left);
            var sut = new StyleImageGenerator(n);

            Assert.IsNull(sut.HighlightedNode);
        }

    }
}