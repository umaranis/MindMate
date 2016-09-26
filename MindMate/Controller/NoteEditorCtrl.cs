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
            var helper = noteGlue.Editor.TableEditor;
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

        public void ModifyTable()
        {
            var helper = noteGlue.Editor.TableEditor;
            IHTMLTable table = helper.GetTableElement();

            if (table != null) //modify selected table
            {
                using (var dialog = new TablePropertyForm())
                {
                    dialog.TableProperties = helper.GetTableProperties(table);
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        helper.TableModify(table, dialog.TableProperties);
                    }
                }
            }
        }

        public void InsertTableRowAbove()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.InsertRowAbove();
        }

        public void InsertTableRowBelow()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.InsertRowBelow();
        }

        public void InsertTableColumnLeft()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.InsertColumnLeft();
        }

        public void InsertTableColumnRight()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.InsertColumnRight();
        }

        public void DeleteTableRow()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.DeleteRow();
        }

        public void DeleteTableColumn()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.DeleteColumn();
        }

        public void DeleteTable()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.DeleteTable();
        }

        public void MoveTableRowUp()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.RowMoveUp();
        }

        public void MoveTableRowDown()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.RowMoveDown();
        }

        public void MoveTableColumnLeft()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.ColumnMoveLeft();
        }

        public void MoveTableColumnRight()
        {
            var helper = noteGlue.Editor.TableEditor;
            helper.ColumnMoveRight();
        }
    }
}
