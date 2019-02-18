using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public void PasteText()
        {
            string text = "Testing!";
            Clipboard.SetText(text);
            var n = new MapNode(new MapTree(), "node");
            ClipboardManager.Paste(n, true);
            Assert.AreEqual(text, n.FirstChild.Text);
        }

        [TestMethod]
        public void PasteText_Unfold()
        {
            string text = "Testing!";
            Clipboard.SetText(text);
            var n = new MapNode(new MapNode(new MapTree(), "root"), "parent");
            new MapNode(n, "child");
            n.Folded = true;            
            ClipboardManager.Paste(n);
            Assert.IsFalse(n.Folded);
        }

        [TestMethod]
        public void CanPaste_Image_True()
        {
            Clipboard.SetImage(Image.FromFile(@"Resources\OrangeTestImage.jpg"));
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
            Clipboard.SetImage(Image.FromFile(@"Resources\OrangeTestImage.jpg"));
            var n = new MapNode(new MapTree(), "node");
            Assert.IsFalse(n.HasImage);
            ClipboardManager.Paste(n);
            Assert.IsFalse(n.HasImage);
            Assert.IsTrue(n.FirstChild.HasImage);

        }

        [TestMethod]
        public void Paste_ImageFileList()
        {
            var fileList = new StringCollection();
            fileList.Add(@"Resources\OrangeTestImage.jpg");
            Clipboard.SetFileDropList(fileList);
            var n = new MapNode(new MapTree(), "node");
            Assert.IsFalse(n.HasImage);
            ClipboardManager.Paste(n);
            Assert.IsFalse(n.HasImage);
            Assert.IsFalse(n.FirstChild.HasImage);

        }

        [TestMethod]
        public void Paste_ImageFileListAsImage()
        {
            var fileList = new StringCollection();
            fileList.Add(@"Resources\OrangeTestImage.jpg");
            Clipboard.SetFileDropList(fileList);
            var n = new MapNode(new MapTree(), "node");
            Assert.IsFalse(n.HasImage);
            ClipboardManager.Paste(n, false, true);
            Assert.IsTrue(n.HasImage);            

        }

        [TestMethod]
        public void Paste_ImageFileListAsImageTwice()
        {
            var fileList = new StringCollection();
            fileList.Add(@"Resources\OrangeTestImage.jpg");
            Clipboard.SetFileDropList(fileList);
            var n = new MapNode(new MapTree(), "node");
            Assert.IsFalse(n.HasImage);
            ClipboardManager.Paste(n, false, true);
            Assert.IsTrue(n.HasImage);
            ClipboardManager.Paste(n, false, true);
            Assert.IsTrue(n.FirstChild.HasImage);

        }

        [TestMethod]
        public void Cut()
        {
            var n = new MapNode(new MapNode(new MapTree(), "root"), "parent");
            new MapNode(n, "child");
            n.Selected = true;
            ClipboardManager.Cut(n.Tree.SelectedNodes);
            Assert.IsTrue(Clipboard.ContainsData(ClipboardManager.MindMateClipboardFormat));
        }
    }
}
