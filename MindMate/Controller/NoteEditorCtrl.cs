using MindMate.Serialization;
using MindMate.View.NoteEditing;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.Controller
{
    public class NoteEditorCtrl
    {
        private readonly NoteMapGlue noteGlue;        

        public NoteEditorCtrl(NoteEditor editor, PersistenceManager pManager)
        {
            this.noteGlue = new NoteMapGlue(editor, pManager);
        }

        public void SetNoteEditorBackColor(Color color)
        {
            noteGlue.Editor.BackColor = color;
        }

        internal void UpdateNodeFromEditor()
        {
            noteGlue.UpdateNodeFromEditor();
        }

        public void InsertTable()
        {
            var tableHelper = new HtmlTableHelper(noteGlue.Editor.Document.DomDocument as HTMLDocument);
            tableHelper.TableInsertPrompt();
        }
    }
}
