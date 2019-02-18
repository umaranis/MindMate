using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using static MindMate.Model.ClipboardManager;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class MindMateClipboardDataObjectTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        [ExcludeFromCodeCoverage]
        public void SetData_1()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            var sut = new MindMateClipboardDataObject(new []{ n }.ToList());
            sut.SetData(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        [ExcludeFromCodeCoverage]
        public void SetData_2()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            var sut = new MindMateClipboardDataObject(new[] { n }.ToList());
            sut.SetData("", null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        [ExcludeFromCodeCoverage]
        public void SetData_3()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            var sut = new MindMateClipboardDataObject(new[] { n }.ToList());
            sut.SetData(typeof(string), null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        [ExcludeFromCodeCoverage]
        public void SetData_4()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            var sut = new MindMateClipboardDataObject(new[] { n }.ToList());
            sut.SetData("", true, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        [ExcludeFromCodeCoverage]
        public void SetData_byType()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            var sut = new MindMateClipboardDataObject(new[] { n }.ToList());
            sut.GetData(typeof(string));
        }

        [TestMethod]
        public void GetFormats()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            n.InsertImage(Image.FromFile(@"Resources\OrangeTestImage.jpg"), false);
            var nodes = new[] { n }.ToList();
            var sut = new MindMateClipboardDataObject(nodes);
            n.Selected = true;

            ClipboardManager.Copy(n.Tree.SelectedNodes);

            var formats = sut.GetFormats();
            Assert.IsTrue(formats.Contains(ClipboardManager.MindMateClipboardFormat));
            Assert.IsTrue(formats.Contains(DataFormats.Bitmap));
            Assert.IsTrue(formats.Contains(DataFormats.Text));            

            Assert.IsTrue(sut.GetDataPresent(DataFormats.Text));                       
        }

        [TestMethod]
        public void GetData()
        {
            var t = new MapTree();
            var r = new MapNode(t, "root");
            var n = new MapNode(r, "node");
            n.InsertImage(Image.FromFile(@"Resources\OrangeTestImage.jpg"), false);
            var nodes = new[] { n }.ToList();
            var sut = new MindMateClipboardDataObject(nodes);
            n.Selected = true;

            ClipboardManager.Copy(n.Tree.SelectedNodes);

            Assert.AreEqual("node", Clipboard.GetData(MindMateClipboardFormat));
            Assert.IsNotNull(Clipboard.GetData(DataFormats.Bitmap));
            Assert.IsNull(Clipboard.GetData(DataFormats.CommaSeparatedValue));
        }
    }
}