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
        private readonly IMainForm mainForm;

        public TabController(MainCtrl mainCtrl, IMainForm mainForm)
        {
            this.mainCtrl = mainCtrl;
            this.mainForm = mainForm;

            this.mainCtrl.PersistenceManager.NewTreeCreated += PersistenceManager_NewTreeCreated;
            this.mainCtrl.PersistenceManager.TreeOpened += PersistenceManager_TreeOpened;
            this.mainCtrl.PersistenceManager.TreeClosed += PersistenceManager_TreeClosed;
            this.mainCtrl.PersistenceManager.CurrentTreeChanged += PersistenceManager_CurrentTreeChanged;

            this.mainForm.EditorTabs.SelectedIndexChanged += EditorTabsOnSelectedIndexChanged;
        }

        private void PersistenceManager_NewTreeCreated(Serialization.PersistenceManager manager, Serialization.PersistentTree e)
        {
            Tab tab = mainForm.EditorTabs.OpenTab(e);
            tab.ControllerTag = new MapCtrl(tab.MapView, mainCtrl.Dialogs, mainCtrl.NodeContextMenu);
        }

        private void PersistenceManager_TreeOpened(Serialization.PersistenceManager manager, Serialization.PersistentTree e)
        {
            Tab tab = mainForm.EditorTabs.OpenTab(e);
            tab.ControllerTag = new MapCtrl(tab.MapView, mainCtrl.Dialogs, mainCtrl.NodeContextMenu);
        }

        private void PersistenceManager_TreeClosed(Serialization.PersistenceManager manager, Serialization.PersistentTree e)
        {
            mainForm.EditorTabs.CloseTab(e);
        }

        private void PersistenceManager_CurrentTreeChanged(Serialization.PersistenceManager manager, Serialization.PersistentTree oldTree, Serialization.PersistentTree newTree)
        {
            if (newTree != null)
            {
                Tab tab = mainForm.EditorTabs.FindTab(newTree);
                mainForm.EditorTabs.SelectedTab = tab;
            }
        }

        private void EditorTabsOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            mainCtrl.PersistenceManager.CurrentTree = (mainForm.EditorTabs.SelectedTab as Tab)?.Tree;
        }
    }
}
