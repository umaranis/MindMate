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
            var helper = new HtmlTableHelper(noteGlue.Editor.Document.DomDocument as HTMLDocument);
            IHTMLTable table = helper.GetSelectedTable();

            if (table != null) //modify selected table
            {
                using (var dialog = new TablePropertyForm())
                {
                    dialog.TableProperties = helper.GetTableProperties(table);
                    if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        helper.TableModify(table, dialog.TableProperties);
                    }
                }
            }
            else //insert new table
            {
                using (var dialog = new TablePropertyForm())
                {                    
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        helper.TableInsert(dialog.TableProperties);
                    }
                }
            }
        }
    }
}
