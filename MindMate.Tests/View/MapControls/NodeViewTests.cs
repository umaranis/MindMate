using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using System.Drawing;

namespace MindMate.Tests.View.MapControls
{
    [TestClass()]
    public class NodeViewTests
    {
        [TestMethod()]
        public void RefreshNoteIcon()
        {
            MetaModel.MetaModel.Initialize();
            MapNode r = new MapNode(new MapTree(), "root");
            NodeView sut = new NodeView(r);
            r.NoteText = "note";
            sut.RefreshNoteIcon();

            Assert.IsNotNull(sut.NoteIcon);
        }

        [TestMethod()]
        public void RefreshNoteIcon_ClearIcon()
        {
            MetaModel.MetaModel.Initialize();
            MapNode r = new MapNode(new MapTree(), "root");
            NodeView sut = new NodeView(r);
            r.NoteText = "note";
            sut.RefreshNoteIcon();
            r.NoteText = null;
            sut.RefreshNoteIcon();

            Assert.IsNull(sut.NoteIcon);
        }

        /// <summary>
        /// NodeView.ICON_SIZE set as 16, it is used in laying out a Node. All icons should have this size.
        /// In case of change in icon default size, NodeView.ICON_SIZE should be updated accordingly.
        /// </summary>
        [TestMethod]
        public void DefaultIconSize_16Pixels()
        {
            MetaModel.MetaModel.Initialize();
            
            Assert.IsTrue(MetaModel.MetaModel.Instance.IconsList.All(m => m.Bitmap.Height == NodeView.ICON_SIZE));
        }

        [TestMethod]
        public void Height_GreaterThanAnyComponent()
        {
            MetaModel.MetaModel.Initialize();
            MapNode r = new MapNode(new MapTree(), "root");            
            r.NoteText = "note";
            r.Link = "link";
            r.Icons.Add("button_ok");
            NodeView sut = new NodeView(r);

            Assert.IsTrue(sut.Height >= sut.RecText.Height);
            Assert.IsTrue(sut.Height >= sut.NoteIcon.Size.Height);
            Assert.IsTrue(sut.Height >= sut.Link.Size.Height);
            Assert.IsTrue(sut.Height >= sut.RecIcons[0].Size.Height);
        }

        [TestMethod]
        public void Height_GreaterThanAnyComponent_WithImage()
        {
            MetaModel.MetaModel.Initialize();
            MapNode r = new MapNode(new MapTree(), "root");
            r.NoteText = "note";
            r.Link = "link";
            r.Icons.Add("button_ok");
            r.InsertImage(Image.FromFile(@"Resources\NodeStyle-GenerateImage.bmp"), false);
            NodeView sut = new NodeView(r);

            Assert.IsTrue(sut.Height >= sut.RecText.Height);
            Assert.IsTrue(sut.Height >= sut.NoteIcon.Size.Height);
            Assert.IsTrue(sut.Height >= sut.Link.Size.Height);
            Assert.IsTrue(sut.Height >= sut.RecIcons[0].Size.Height);
            Assert.IsTrue(sut.Height >= sut.ImageView.Size.Height);
        }
    }
}