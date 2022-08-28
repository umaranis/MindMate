using System.Windows.Forms;
using MindMate.View.MapControls;
using System;
using MindMate.Controller;

namespace MindMate.View
{
    public delegate void FocusedControlChangeDelegate(Control gotFocus, Control lostFocus);

    public interface IMainForm
    {
        IEditorTabs EditorTabs { get; }
        bool IsNoteEditorActive { get; }
        INoteEditor NoteEditor { get; }
        ISideBarControl SideBarTabs { get; }
        IStatusBar StatusBar { get; }
        string Text { get; set; }

        event EventHandler Load;
        event EventHandler Shown;
        event FormClosingEventHandler FormClosing;        
        event FocusedControlChangeDelegate FocusedControlChanged;

        void FocusMapView();
        void InsertMenuItems(Plugins.MainMenuItem[] menuItems);
        void RefreshRecentFilesMenuItems();

        INoteEditorCtrl CreateNoteEditorController(MainCtrl mainCtrl);

        void SetupTabController(MainCtrl mainCtrl);
    }
}