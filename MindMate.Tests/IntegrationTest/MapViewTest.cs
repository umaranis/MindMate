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
            if(SAVE_ACTUAL_IMAGE) image.Save(@"Resources\Sample Map - Actual.png");
            var refImage = (Bitmap)Bitmap.FromFile(@"Resources\Sample Map.png");
            
            //form.Close();
            view.Canvas.Dispose();

            Assert.AreEqual<float>(0.0f, image.PercentageDifference(refImage, 0));

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
    }
}
