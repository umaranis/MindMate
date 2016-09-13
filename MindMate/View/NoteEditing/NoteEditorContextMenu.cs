using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    public class NoteEditorContextMenu : ContextMenuStrip
    {
        private NoteEditor editor;

        private ToolStripItem mCut;
        private ToolStripItem mCopy;
        private ToolStripItem mPaste;

        public NoteEditorContextMenu(NoteEditor editor)
        {
            this.editor = editor;
            
            mCut = new ToolStripButton("Cut", null, (a, b) => editor.Cut());
            mCopy = new ToolStripButton("Copy", null, (a, b) => editor.Copy());
            mPaste = new ToolStripButton("Paste", null, (a, b) => editor.Paste());

            Items.Add(mCut);
            Items.Add(mCopy);
            Items.Add(mPaste);

            editor.Document.ContextMenuShowing += Document_ContextMenuShowing;
        }
                
        private void Document_ContextMenuShowing(object sender, HtmlElementEventArgs e)
        {
            e.ReturnValue = false;
            mCut.Enabled = editor.CanExecuteCommand(NoteEditorCommand.Cut);
            mCopy.Enabled = editor.CanExecuteCommand(NoteEditorCommand.Copy);
            mPaste.Enabled = editor.CanExecuteCommand(NoteEditorCommand.Paste);
            Show(editor, e.ClientMousePosition);
        }
    }
}
