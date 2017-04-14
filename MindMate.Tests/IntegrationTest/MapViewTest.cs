using System;
using MindMate.Model;
using MindMate.View.MapControls;
using System.IO;
using System.Drawing;
using MindMate.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XnaFan.ImageComparison;
using MindMate.Controller;

namespace MindMateTest
{
    [TestClass]
    public class MapViewTest
    {
        public const bool SAVE_ACTUAL_IMAGE = true;

        [TestMethod]
        public void MapView_SampleMap_ImageTest()
        {
            string xmlString = System.IO.File.ReadAllText(@"Resources\Sample Map.mm");
            MapTree tree = new MapTree();
            new MindMapSerializer().Deserialize(xmlString, tree);

            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);

            var image = view.DrawToBitmap();
            if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Sample Map - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Sample Map.png");

            //form.Close();
            view.Canvas.Dispose();

            Assert.AreEqual<float>(0.0f, image.PercentageDifference(refImage, 0));

            image.Dispose();
            refImage.Dispose();
        }

        /// <summary>
        /// With highlighted node
        /// </summary>
        [TestMethod]
        public void MapView_SampleMap_ImageTest2()
        {
            string xmlString = System.IO.File.ReadAllText(@"Resources\Sample Map.mm");
            MapTree tree = new MapTree();
            new MindMapSerializer().Deserialize(xmlString, tree);

            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);
            view.HighlightNode(tree.RootNode.FirstChild);

            var image = view.DrawToBitmap();
            if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Sample Map1 - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Sample Map1.png");

            //form.Close();
            view.Canvas.Dispose();

            Assert.AreEqual<float>(0.00f, image.PercentageDifference(refImage, 0));

            image.Dispose();
            refImage.Dispose();
        }

        [TestMethod]
        public void MapView_FeatureDisplay_ImageTest()
        {
            string xmlString = System.IO.File.ReadAllText(@"Resources\Feature Display.mm");
            MapTree tree = new MapTree();
            new MindMapSerializer().Deserialize(xmlString, tree);

            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);

            var image = view.DrawToBitmap();
            if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Feature Display - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Feature Display.png");

            //form.Close();
            view.Canvas.Dispose();

            Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

            image.Dispose();
            refImage.Dispose();
        }

        /// <summary>
        /// Includes testing for Image in Map
        /// </summary>
        [TestMethod]
        public void MapView_FeatureDisplay2_AddImages()
        {
            PersistentTree tree = new PersistenceManager().OpenTree(@"Resources\Feature Display.mm");

            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);

			Image testImage = Image.FromFile(@"Resources\TestImage.png");
			var longText = "This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. ";

			tree.RootNode.InsertImage(testImage);

			//right
			var rNode = new MapNode(tree.RootNode, null, NodePosition.Right);
            rNode.InsertImage(testImage);

            var rWithText = new MapNode(tree.RootNode, "Text", NodePosition.Right);
            rWithText.InsertImage(testImage);

			var rSmallerImageWithText = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rSmallerImageWithText.InsertImage(testImage);
			rSmallerImageWithText.ImageSize = new Size(10, 10);

			var rWithLongText = new MapNode(tree.RootNode, longText, NodePosition.Right);
            rWithLongText.InsertImage(testImage);

			var rTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rTextnIcon.InsertImage(testImage);
			rTextnIcon.Icons.Add("button_ok");

			var rSmallTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rSmallTextnIcon.FontSize = 4;
			rSmallTextnIcon.InsertImage(testImage);
			rSmallTextnIcon.Icons.Add("button_ok");

			var rSmallTextnIconOnly = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rSmallTextnIconOnly.FontSize = 4;
			rSmallTextnIconOnly.Icons.Add("button_ok");
			rSmallTextnIconOnly.InsertImage(testImage);
			rSmallTextnIconOnly.RemoveImage();

			var rWithLongTextnIcon = new MapNode(tree.RootNode, longText, NodePosition.Right);
			rWithLongTextnIcon.InsertImage(testImage);
			rWithLongTextnIcon.Icons.Add("button_ok");

			var rIConOnly = new MapNode(tree.RootNode, null, NodePosition.Right);
			rIConOnly.Icons.Add("button_ok");

			var rEmpty = new MapNode(tree.RootNode, null, NodePosition.Right);
			//right

			//left
			var lNode = new MapNode(tree.RootNode, null, NodePosition.Left);
            lNode.InsertImage(testImage);

            var lWithText = new MapNode(tree.RootNode, "Text", NodePosition.Left);
            lWithText.InsertImage(testImage);

			var lSmallerImageWithText = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lSmallerImageWithText.InsertImage(testImage);
			lSmallerImageWithText.ImageSize = new Size(10, 10);

			var lWithLongText = new MapNode(tree.RootNode, longText, NodePosition.Left);
            lWithLongText.InsertImage(testImage);

			var lTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lTextnIcon.InsertImage(testImage);
			lTextnIcon.Icons.Add("button_ok");

			var lSmallTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lSmallTextnIcon.Icons.Add("button_ok");
			lSmallTextnIcon.FontSize = 4;
			lSmallTextnIcon.InsertImage(testImage);

			var lSmallTextnIconOnly = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lSmallTextnIconOnly.Icons.Add("button_ok");
			lSmallTextnIconOnly.FontSize = 4;
			lSmallTextnIconOnly.InsertImage(testImage);
			lSmallTextnIconOnly.RemoveImage();

			var lWithLongTextnIcon = new MapNode(tree.RootNode, longText, NodePosition.Left);
			lWithLongTextnIcon.InsertImage(testImage);
			lWithLongTextnIcon.Icons.Add("button_ok");

			var lIConOnly = new MapNode(tree.RootNode, null, NodePosition.Left);
			lIConOnly.Icons.Add("button_ok");

			var lEmpty = new MapNode(tree.RootNode, null, NodePosition.Left);
			//left

			var image = view.DrawToBitmap();
            if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Feature Display2 - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Feature Display2.png");
            //new MapZipSerializer().SerializeMap(tree, @"Resources\Feature Display2.mm", true);

            //form.Close();
            view.Canvas.Dispose();

            Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

            image.Dispose();
            refImage.Dispose();
        }

		/// <summary>
		/// Verifies serialization/deserialization for MapTree(Feature Display2.mm) generated by <see cref="MapView_FeatureDisplay2_AddImages"/>
		/// </summary>
		[TestMethod]
		public void MapView_FeatureDisplay2_ImageTest()
		{
			PersistentTree tree = new PersistenceManager().OpenTree(@"Resources\Feature Display2.mm");

			MindMate.MetaModel.MetaModel.Initialize();
			MapView view = new MapView(tree);

			var image = view.DrawToBitmap();
			if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Feature Display2 - Actual.png");
			var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Feature Display2.png");

			view.Canvas.Dispose();

			Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

			image.Dispose();
			refImage.Dispose();
		}

	}
}
