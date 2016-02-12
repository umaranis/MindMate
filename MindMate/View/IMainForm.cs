using System.Windows.Forms;
using MindMate.View.MapControls;
using MindMate.View.NoteEditing;
using System;

namespace MindMate.View
{
    public interface IMainForm
    {
        EditorTabs.EditorTabs EditorTabs { get; }
        bool IsNoteEditorActive { get; }
        NoteEditor NoteEditor { get; }
        SideTabControl SideBarTabs { get; }
        StatusBar StatusBar { get; }
        string Text { get; set; }

        event EventHandler Load;
        event EventHandler Shown;
        event FormClosingEventHandler FormClosing;

        void FocusMapView();
        void InsertMenuItems(Plugins.MainMenuItem[] menuItems);
        void RefreshRecentFilesMenuItems();
    }
}