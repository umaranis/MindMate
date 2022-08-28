using System.Drawing;

namespace MindMate.View
{
    public interface INoteEditorCtrl
    {
        void SetNoteEditorBackColor(Color color);
        void InsertTable();
        void ModifyTable();
        void InsertTableRowAbove();
        void InsertTableRowBelow();
        void InsertTableColumnLeft();
        void InsertTableColumnRight();
        void DeleteTableRow();
        void DeleteTableColumn();
        void DeleteTable();
        void MoveTableRowUp();
        void MoveTableRowDown();
        void MoveTableColumnLeft();
        void MoveTableColumnRight();
        void InsertImage();
        void ShowHtmlSourceDialog();
        void CleanHtmlCode();
        void UpdateNodeFromEditor();
    }
}