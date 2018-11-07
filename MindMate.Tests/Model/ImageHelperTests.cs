using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class ImageHelperTests
    {
        [TestMethod()]
        public void GetExtension()
        {
            Image image = Image.FromFile(@"Resources\MindMap-2m.ico");
            Assert.AreEqual("ico", ImageHelper.GetExtension(image));
        }

        [TestMethod()]
        public void CalculateDefaultSize_MoreHeight()
        {
            Assert.AreEqual(new Size(200, 300), ImageHelper.CalculateDefaultSize(new Size(1000, 1500)));
        }

        [TestMethod()]
        public void CalculateDefaultSize_MoreWidth()
        {
            Assert.AreEqual(new Size(400, 266), ImageHelper.CalculateDefaultSize(new Size(1500, 1000)));
        }

        [TestMethod()]
        public void CalculateDefaultSize_DefaultSize_NoChange()
        {
            Assert.AreEqual(new Size(400, 300), ImageHelper.CalculateDefaultSize(new Size(400, 300)));
        }

        [TestMethod()]
        public void CalculateDefaultSize_SmallThanDefault_NoChange()
        {
            Assert.AreEqual(new Size(370, 270), ImageHelper.CalculateDefaultSize(new Size(370, 270)));
        }

        [TestMethod]
        public void GetImageFromFile()
        {
            Assert.IsFalse(ImageHelper.GetImageFromFile(@"abcdefghij.png", out Image image));
        }
    }
}