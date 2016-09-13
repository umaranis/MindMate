using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;

namespace MindMate.Tests.View.NoteEditing
{
    [TestClass()]
    public class NoteEditorContextMenuTests
    {
        [TestMethod()]
        public void NoteEditorContextMenu()
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
               NoteEditor editor = new NoteEditor();
               new NoteEditorContextMenu(editor);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}