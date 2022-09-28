using MindMate.Serialization;
using MindMate.View.Dialogs;
using MindMate.View.NoteEditing;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Controller
{
    public class NoteEditorCtrl
    {
        private readonly NoteMapGlue noteGlue;
        private readonly DialogManager dialogs;

        public NoteEditorCtrl(NoteEditor editor, PersistenceManager pManager, DialogManager dialogs)
        {
            this.noteGlue = new NoteMapGlue(editor, pManager);
            new ImageLocalSaver(editor, pManager);
            new ImageLocalProvider(pManager);
            new ImagePaster(editor, pManager);
            this.dialogs = dialogs;
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
                    dialog.UpdateTable = true;
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
                    dialog.UpdateTable = true;
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

		public void InsertImage()
		{
            var fileName = dialogs.GetImageFile();
			if (fileName != null)
			{
				ImagePaster.InsertFormFile(noteGlue.Editor, fileName, noteGlue.CurrentMapNpde.Tree);
			}
		}

        public void ShowHtmlSourceDialog()
        {
            var form = new HtmlSourceDialog();            

            form.HtmlSource = noteGlue.Editor.HTML;
            if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                noteGlue.Editor.UpdateHtmlSource(form.HtmlSource); //this makes the editor dirty as opposed to setting 'noteGlue.Editor.HTML'
            }
        }

        public void CleanHtmlCode()
        {
            HtmlCodeCleaner.Clean(noteGlue.Editor);

            // - makes the editor dirty 
            // - also revolves an issue where editor is stuck in an odd state if 
            //     1) a fixed sized div was selected earlier
            //     2) and the div size is cleared during Clean operation
            noteGlue.Editor.UpdateHtmlSource(noteGlue.Editor.HTML);             
        }
    }
}
