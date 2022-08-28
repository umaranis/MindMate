using MindMate.Plugins.Tasks.SideBar;
using MindMate.View;
using MindMate.WinFormsUI.NoteEditing;
using MindMate.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using MindMate.Controller;

namespace MindMate.WinFormsUI
{
    public sealed class SideTabControl : TabControl, ISideBarControl
    {
        private const string SearchTabTitle = "Search";

        /// <summary>
        /// Invoked when SideTabControl gets focus if it doesn't need it.
        /// Some of the tabs don't need keyboard focus.
        /// </summary>
        public event EventHandler GotExtraFocus;

        public SideTabControl()
        {
            Dock = DockStyle.Fill;
            //SideBarTabs.Alignment = TabAlignment.Bottom;

            ImageList imageList = new ImageList();
            imageList.Images.Add(MindMate.Properties.Resources.sticky_note_pin);
            ImageList = imageList;

            //create Note Editor tab in sidebar
            NoteTab = new TabPage(MainCtrl.NoteEditorWindowTitle) { ImageIndex = 0 };
            var ctrl = new NoteEditor { Dock = DockStyle.Fill };
            NoteEditor = ctrl;
            NoteTab.Controls.Add(ctrl);
            TabPages.Add(NoteTab);

            //create Search Tab in sidebar
            var searchTab = new TabPage(SearchTabTitle);
            var searchControl = new SearchControl()
            {
                Dock = DockStyle.Fill
            };
            searchTab.Controls.Add(searchControl);
            TabPages.Add(searchTab);

            SelectedIndexChanged += SideTabControl_SelectedIndexChanged;
        }

        private void SideTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedTab == NoteTab)
                SelectedTab.Controls[0].Focus();
            else if (SelectedTab == SearchTab)
                SearchTab.Controls[0].Focus();
            else
                GotExtraFocus?.Invoke(this, e);
        }

        public INoteEditor NoteEditor { get; set; }

        public TabPage NoteTab { get; private set; }

        //TODO: Should be handled the way 'View Calendar' ribbon command is done.
        public TabPage TaskListTab
        {
            get
            {
                return TabPages.Cast<TabPage>().FirstOrDefault(p => p.Text == MindMate.Plugins.Tasks.TaskPlugin.TasksWindowTitle);
            }
        }

        public TabPage SearchTab
        {
            get
            {
                return TabPages.Cast<TabPage>().First(p => p.Text == SearchTabTitle);
            }
        }

        public ISearchControl SearchControl
        {
            get
            {
                return SearchTab.Controls[0] as ISearchControl;
            }
        }

        public new void SelectTab(string tabTitle)
        {
            SelectedTab = TabPages.Cast<TabPage>().FirstOrDefault(p => p.Text == tabTitle);
        }

        public void AddTab(object control)
        {
            if (control is Control ctrl)
            {
                TabPage tPage = new TabPage(ctrl.Text);
                ctrl.Dock = DockStyle.Fill;
                tPage.Controls.Add(ctrl);
                TabPages.Add(tPage);
            }
            else
            {
                throw new ArgumentException("Given 'control' parameter is not a WinForms Control object.");
            }
        }

    }
}
