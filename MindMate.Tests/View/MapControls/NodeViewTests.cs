using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;

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
    }
}