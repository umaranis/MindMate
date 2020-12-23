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

			tree.RootNode.InsertImage(testImage, true);

			//right
			var rNode = new MapNode(tree.RootNode, null, NodePosition.Right);
			rNode.InsertImage(testImage, true);

			var rWithText = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rWithText.InsertImage(testImage, true);

			var rSmallerImageWithText = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rSmallerImageWithText.InsertImage(testImage, true);
			rSmallerImageWithText.ImageSize = new Size(10, 10);

			var rWithLongText = new MapNode(tree.RootNode, longText, NodePosition.Right);
			rWithLongText.InsertImage(testImage, true);

			var rTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rTextnIcon.InsertImage(testImage, true);
			rTextnIcon.Icons.Add("button_ok");

			var rSmallTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rSmallTextnIcon.FontSize = 4;
			rSmallTextnIcon.InsertImage(testImage, true);
			rSmallTextnIcon.Icons.Add("button_ok");

			var rSmallTextnIconOnly = new MapNode(tree.RootNode, "Text", NodePosition.Right);
			rSmallTextnIconOnly.FontSize = 4;
			rSmallTextnIconOnly.Icons.Add("button_ok");
			rSmallTextnIconOnly.InsertImage(testImage, true);
			rSmallTextnIconOnly.RemoveImage();

			var rWithLongTextnIcon = new MapNode(tree.RootNode, longText, NodePosition.Right);
			rWithLongTextnIcon.InsertImage(testImage, true);
			rWithLongTextnIcon.Icons.Add("button_ok");

			var rIConOnly = new MapNode(tree.RootNode, null, NodePosition.Right);
			rIConOnly.Icons.Add("button_ok");

			var rEmpty = new MapNode(tree.RootNode, null, NodePosition.Right);
			//right

			//left
			var lNode = new MapNode(tree.RootNode, null, NodePosition.Left);
			lNode.InsertImage(testImage, true);

			var lWithText = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lWithText.InsertImage(testImage, true);

			var lSmallerImageWithText = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lSmallerImageWithText.InsertImage(testImage, true);
			lSmallerImageWithText.ImageSize = new Size(10, 10);

			var lWithLongText = new MapNode(tree.RootNode, longText, NodePosition.Left);
			lWithLongText.InsertImage(testImage, true);

			var lTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lTextnIcon.InsertImage(testImage, true);
			lTextnIcon.Icons.Add("button_ok");

			var lSmallTextnIcon = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lSmallTextnIcon.Icons.Add("button_ok");
			lSmallTextnIcon.FontSize = 4;
			lSmallTextnIcon.InsertImage(testImage, true);

			var lSmallTextnIconOnly = new MapNode(tree.RootNode, "Text", NodePosition.Left);
			lSmallTextnIconOnly.Icons.Add("button_ok");
			lSmallTextnIconOnly.FontSize = 4;
			lSmallTextnIconOnly.InsertImage(testImage, true);
			lSmallTextnIconOnly.RemoveImage();

			var lWithLongTextnIcon = new MapNode(tree.RootNode, longText, NodePosition.Left);
			lWithLongTextnIcon.InsertImage(testImage, true);
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

        [TestMethod]
        public void MapView_ExtendCanvas_ImageTest()
        {
            PersistentTree tree = new PersistenceManager().NewTree();

            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);

            Assert.AreEqual(view.Canvas.Width, MapView.CANVAS_DEFAULT_WIDTH);
            Assert.AreEqual(view.Canvas.Height, MapView.CANVAS_DEFAULT_HEIGHT);

            for(int i = 0; i < 55; i++)
            {
                var n = new MapNode(tree.RootNode, "This is a sample text");
                new MapNode(n, "This is a sample text");
                new MapNode(n, "This is a sample text");
                new MapNode(n, "This is a sample text");
                new MapNode(n, "This is a sample text");
                new MapNode(n, "This is a sample text");
                new MapNode(n, "This is a sample text");
            }

            var image = view.DrawToBitmap();
            if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\ExtendCanvas - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\ExtendCanvas.png");

            view.Canvas.Dispose();

            Assert.AreEqual(view.Canvas.Width, MapView.CANVAS_DEFAULT_WIDTH + MapView.CANVAS_SIZE_INCREMENT);
            Assert.AreEqual(view.Canvas.Height, MapView.CANVAS_DEFAULT_HEIGHT + MapView.CANVAS_SIZE_INCREMENT);
            Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for ExtendCanvas test.");            

            image.Dispose();
            refImage.Dispose();
        }

        [TestMethod]        
        public void MapView_Flags_ImageTest()
        {
            PersistentTree tree = new PersistenceManager().OpenTree(@"Resources\Flags.mm");

            MindMate.MetaModel.MetaModel.Initialize();
            MapView view = new MapView(tree);

            var image = view.DrawToBitmap();
            if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Flags - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Flags.png");

            view.Canvas.Dispose();

            Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

            image.Dispose();
            refImage.Dispose();
        }

        [TestMethod]
		public void MapView_FeatureDisplay3_Alignment()
		{
			PersistentTree tree = new PersistenceManager().NewTree();

			MindMate.MetaModel.MetaModel.Initialize();
			MapView view = new MapView(tree);
			Image testImage = Image.FromFile(@"Resources\TestImage.png");
			var longText = "This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. ";
			var shortText = "Text";

			MapNode CreateNode(string text, ImageAlignment align, NodePosition pos)
			{
				var n1 = new MapNode(tree.RootNode, text, pos);
				n1.InsertImage(testImage, false);
				n1.ImageAlignment = align;
				new MapNode(n1, align.ToString());
				return n1;
			}

			CreateNode(longText, ImageAlignment.Default, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AboveStart, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AboveCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AboveEnd, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BelowStart, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BelowCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BelowEnd, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AfterTop, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AfterCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AfterBottom, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BeforeTop, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BeforeCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BeforeBottom, NodePosition.Right);

			CreateNode(shortText, ImageAlignment.Default, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AboveStart, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AboveCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AboveEnd, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BelowStart, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BelowCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BelowEnd, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AfterTop, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AfterCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AfterBottom, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BeforeTop, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BeforeCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BeforeBottom, NodePosition.Right);

			CreateNode(longText, ImageAlignment.Default, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AboveStart, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AboveCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AboveEnd, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BelowStart, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BelowCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BelowEnd, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AfterTop, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AfterCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AfterBottom, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BeforeTop, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BeforeCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BeforeBottom, NodePosition.Left);

			CreateNode(shortText, ImageAlignment.Default, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AboveStart, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AboveCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AboveEnd, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BelowStart, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BelowCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BelowEnd, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AfterTop, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AfterCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AfterBottom, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BeforeTop, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BeforeCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BeforeBottom, NodePosition.Left);


			var image = view.DrawToBitmap();
			if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Feature Display3 - Actual.png");
			var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Feature Display3.png");

			view.Canvas.Dispose();

			Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

			image.Dispose();
			refImage.Dispose();
		}

		[TestMethod]
		public void MapView_FeatureDisplay4_Alignment()
		{
			PersistentTree tree = new PersistenceManager().NewTree();

			MindMate.MetaModel.MetaModel.Initialize();
			MapView view = new MapView(tree);
			Image testImage = Image.FromFile(@"Resources\TestImage.png");
			var longText = "This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. This is a very very long text. ";
			var shortText = "Text";

			MapNode CreateNode(string text, ImageAlignment align, NodePosition pos)
			{
				var n1 = new MapNode(tree.RootNode, text, pos);
				n1.InsertImage(testImage, false);

				n1.ImageAlignment = align;
				n1.Icons.Add("button_ok");
				new MapNode(n1, align.ToString());
				return n1;
			}

			CreateNode(longText, ImageAlignment.Default, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AboveStart, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AboveCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AboveEnd, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BelowStart, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BelowCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BelowEnd, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AfterTop, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AfterCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.AfterBottom, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BeforeTop, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BeforeCenter, NodePosition.Right);
			CreateNode(longText, ImageAlignment.BeforeBottom, NodePosition.Right);

			CreateNode(shortText, ImageAlignment.Default, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AboveStart, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AboveCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AboveEnd, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BelowStart, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BelowCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BelowEnd, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AfterTop, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AfterCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.AfterBottom, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BeforeTop, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BeforeCenter, NodePosition.Right);
			CreateNode(shortText, ImageAlignment.BeforeBottom, NodePosition.Right);

			CreateNode(longText, ImageAlignment.Default, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AboveStart, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AboveCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AboveEnd, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BelowStart, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BelowCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BelowEnd, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AfterTop, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AfterCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.AfterBottom, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BeforeTop, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BeforeCenter, NodePosition.Left);
			CreateNode(longText, ImageAlignment.BeforeBottom, NodePosition.Left);

			CreateNode(shortText, ImageAlignment.Default, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AboveStart, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AboveCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AboveEnd, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BelowStart, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BelowCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BelowEnd, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AfterTop, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AfterCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.AfterBottom, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BeforeTop, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BeforeCenter, NodePosition.Left);
			CreateNode(shortText, ImageAlignment.BeforeBottom, NodePosition.Left);


			var image = view.DrawToBitmap();
			if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Feature Display4 - Actual.png");
			var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Feature Display4.png");

			view.Canvas.Dispose();

			Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

			image.Dispose();
			refImage.Dispose();
		}

		[TestMethod]
		public void MapView_FeatureDisplay5_BackColor_NodeShape()
		{
			PersistentTree tree = new PersistenceManager().NewTree();

			MindMate.MetaModel.MetaModel.Initialize();
			MapView view = new MapView(tree);

			var n1 = new MapNode(tree.RootNode, "Sample", NodePosition.Right);
			n1.BackColor = Color.AliceBlue;
			n1.Shape = NodeShape.Box;

			var n2 = new MapNode(tree.RootNode, "Sample", NodePosition.Right);
			n2.BackColor = Color.Brown;
			n2.Shape = NodeShape.Bubble;

			var n3 = new MapNode(tree.RootNode, "Sample", NodePosition.Right);
			n3.BackColor = Color.ForestGreen;
			n3.Shape = NodeShape.Bullet;

			var n4 = new MapNode(tree.RootNode, "Sample", NodePosition.Right);
			n4.BackColor = Color.DeepPink;
			n4.Shape = NodeShape.Fork;

			var n5 = new MapNode(tree.RootNode, "Sample", NodePosition.Right);
			n5.BackColor = Color.Khaki;
			n5.Shape = NodeShape.None;

			var image = view.DrawToBitmap();
			if (SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Feature Display5 - Actual.png");  
			var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Feature Display5.png");

			view.Canvas.Dispose();

			Assert.AreEqual(0.0f, image.PercentageDifference(refImage, 0), "Images don't match for 'Feature Display.mm'");

			image.Dispose();
			refImage.Dispose();
		}
	}
}
