using MindMate.View;
using MindMate.View.EditorTabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Controller
{
    public class TabController
    {
        private readonly MainCtrl mainCtrl;
        private readonly EditorTabs editorTabs;

        public TabController(MainCtrl mainCtrl, EditorTabs editorTabs)
        {
            this.mainCtrl = mainCtrl;
            this.editorTabs = editorTabs;

            this.mainCtrl.PersistenceManager.NewTreeCreated += PersistenceManager_NewTreeCreated;
            this.mainCtrl.PersistenceManager.TreeOpened += PersistenceManager_TreeOpened;
            this.mainCtrl.PersistenceManager.TreeClosed += PersistenceManager_TreeClosed;
            this.mainCtrl.PersistenceManager.CurrentTreeChanged += PersistenceManager_CurrentTreeChanged;

            editorTabs.SelectedIndexChanged += EditorTabsOnSelectedIndexChanged;
        }

        private void PersistenceManager_NewTreeCreated(Serialization.PersistenceManager manager, Serialization.PersistentTree e)
        {
            Tab tab = editorTabs.OpenTab(e);
            tab.ControllerTag = new MapCtrl(tab.MapView, mainCtrl.Dialogs, mainCtrl.NodeContextMenu);
        }

        private void PersistenceManager_TreeOpened(Serialization.PersistenceManager manager, Serialization.PersistentTree e)
        {
            Tab tab = editorTabs.OpenTab(e);
            tab.ControllerTag = new MapCtrl(tab.MapView, mainCtrl.Dialogs, mainCtrl.NodeContextMenu);
        }

        private void PersistenceManager_TreeClosed(Serialization.PersistenceManager manager, Serialization.PersistentTree e)
        {
            editorTabs.CloseTab(e);
        }

        private void PersistenceManager_CurrentTreeChanged(Serialization.PersistenceManager manager, Serialization.PersistentTree oldTree, Serialization.PersistentTree newTree)
        {
            if (newTree != null)
            {
                Tab tab = editorTabs.FindTab(newTree);
                editorTabs.SelectedTab = tab;
            }
        }

        private void EditorTabsOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            mainCtrl.PersistenceManager.CurrentTree = (editorTabs.SelectedTab as Tab)?.Tree;
        }
    }
}
