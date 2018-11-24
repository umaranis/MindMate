using MindMate.View.NoteEditing;
using MindMate.View.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View
{
    public sealed class SideTabControl : TabControl
    {
        private const string NoteTabTitle = "Note Editor";
        private const string TaskListTabTitle = "Tasks";
        private const string SearchTabTitle = "Search";

        public SideTabControl()
        {
            Dock = DockStyle.Fill;
            //SideBarTabs.Alignment = TabAlignment.Bottom;

            ImageList imageList = new ImageList();
            imageList.Images.Add(MindMate.Properties.Resources.sticky_note_pin);
            ImageList = imageList;

            //create Note Editor tab in sidebar
            NoteTab = new TabPage(NoteTabTitle) { ImageIndex = 0 };
            NoteEditor = new NoteEditor { Dock = DockStyle.Fill };
            NoteTab.Controls.Add(NoteEditor);
            TabPages.Add(NoteTab);

            //create Search Tab in sidebar
            var searchTab = new TabPage(SearchTabTitle);
            var searchControl = new Search.SearchControl()
            {
                Dock = DockStyle.Fill
            };
            searchTab.Controls.Add(searchControl);
            TabPages.Add(searchTab);
        }

        public NoteEditor NoteEditor { get; set; }

        public TabPage NoteTab { get; private set; }

        //TODO: Should be handled the way 'View Calendar' ribbon command is done.
        public TabPage TaskListTab
        {
            get
            {
                return TabPages.Cast<TabPage>().FirstOrDefault(p => p.Text == TaskListTabTitle);
            }
        }

        public TabPage SearchTab
        {
            get
            {
                return TabPages.Cast<TabPage>().First(p => p.Text == SearchTabTitle);
            }
        }

        public SearchControl SearchControl
        {
            get
            {
                return SearchTab.Controls[0] as SearchControl;
            }
        }

    }
}
