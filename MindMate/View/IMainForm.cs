using System.Windows.Forms;
using MindMate.View.MapControls;
using MindMate.View.NoteEditing;
using System;

namespace MindMate.View
{
    public interface IMainForm
    {
        bool IsNoteEditorActive { get; }
        NoteEditor NoteEditor { get; }
        TabControl SideBarTabs { get; }
        StatusBar StatusBar { get; }
        string Text { get; set; }

        event EventHandler Load;
        event EventHandler Shown;
        event FormClosingEventHandler FormClosing;

        void AddMainView(MapViewPanel mapViewPanel);
        void FocusMapView();
        void InsertMenuItems(Plugins.MainMenuItem[] menuItems);
        void RefreshRecentFilesMenuItems();
    }
}