using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.Model
{
    [TestClass]
    public class ClipboardManagerTests
    {
        [TestMethod]
        public void CanPaste_Image_True()
        {
            Clipboard.SetImage(Image.FromFile(@"Resources\MapCtrl1.png"));
            Assert.IsTrue(ClipboardManager.CanPaste);
        }

        [TestMethod]
        public void CanPaste_Image_False()
        {
            Clipboard.Clear();
            Assert.IsFalse(ClipboardManager.CanPaste);
        }

        [TestMethod]
        public void Paste_Image()
        {
            Clipboard.SetImage(Image.FromFile(@"Resources\MapCtrl1.png"));
            var n = new MapNode(new MapTree(), "node");
            Assert.IsFalse(n.HasImage);
            ClipboardManager.Paste(n);
            Assert.IsFalse(n.HasImage);
            Assert.IsTrue(n.FirstChild.HasImage);

        }
    }
}
