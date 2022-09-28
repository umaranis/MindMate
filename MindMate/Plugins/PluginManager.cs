using MindMate.Controller;
using MindMate.Model;
using MindMate.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.View.MapControls;

namespace MindMate.Plugins
{
    public class PluginManager : IPluginManager
    {
        private readonly MainCtrl mainCtrl;

        public List<IPlugin> Plugins { get; private set; }

        public PluginManager(MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            Plugins = new List<IPlugin>();

            LoadPlugins();

            mainCtrl.PersistenceManager.NewTreeCreating += PersistentManager_NewTreeCreating;
            mainCtrl.PersistenceManager.TreeOpening += PersistentManager_TreeOpening;
            mainCtrl.PersistenceManager.TreeClosing += PersistentManager_TreeClosing;

        }

        private void LoadPlugins()
        {
            Plugins.Add(new Tasks.TaskPlugin());
        }

        public void Initialize()
        {
            Plugins.ForEach(a => { 
                
                a.Initialize(this); 

                if(a is IPluginSystemIcon)
                {
                    var icons = ((IPluginSystemIcon)a).CreateSystemIcons();
                    MetaModel.MetaModel.Instance.SystemIconList.AddRange(icons);
                }
            });
        }

        #region Initialize ContextMenu

        public void InitializeContextMenu(NodeContextMenu nodeContextMenu)
        {
            foreach (IPlugin p in Plugins)
            {
                IPluginMapNodeContextMenu plugin = p as IPluginMapNodeContextMenu;
                if (plugin != null)
                {
                    var menu = plugin.CreateContextMenuItemsForNode();
                    if (menu != null)
                        InsertMenuItems(menu, nodeContextMenu);
                }
            }

            //register event to notify plugins on context menu opening
            nodeContextMenu.Opening +=
                (s, evt) => this.OnMapNodeContextMenuOpening(mainCtrl.CurrentMapCtrl.MapView.SelectedNodes);
        }

        private void InsertMenuItems(Plugins.MenuItem[] menuItems, NodeContextMenu nodeContextMenu)
        {
            ContextMenuStrip contextMenu = nodeContextMenu;

            int index = contextMenu.Items.IndexOf(nodeContextMenu.mSepPluginEnd);

            contextMenu.Items.Insert(index++, new ToolStripSeparator());

            foreach (Plugins.MenuItem menu in menuItems)
            {
                contextMenu.Items.Insert(index++, menu.UnderlyingMenuItem);
                menu.UnderlyingMenuItem.Click += PluginMenuItem_Click;
                SetClickHandlerForSubMenu(menu);
            }
        }

        private void SetClickHandlerForSubMenu(Plugins.MenuItem menu)
        {
            foreach (ToolStripDropDownItem subMenuItem in menu.UnderlyingMenuItem.DropDownItems)
            {
                subMenuItem.Click += PluginMenuItem_Click;
                SetClickHandlerForSubMenu((Plugins.MenuItem)(subMenuItem.Tag));
            }
        }

        void PluginMenuItem_Click(object sender, EventArgs e)
        {
            Plugins.MenuItem menuItem = ((Plugins.MenuItem)((ToolStripMenuItem)sender).Tag);
            if (menuItem.Click != null)
                menuItem.Click(menuItem, mainCtrl.CurrentMapCtrl.MapView.Tree.SelectedNodes);
        }

        /// <summary>
        /// Executes an action for each of the menu items added by Plugins.
        /// </summary>
        /// <param name="action"></param>
        public void ForEachPluginMenuItem(Action<Plugins.MenuItem> action)
        {
            ContextMenuStrip contextMenu = mainCtrl.NodeContextMenu;
            int index = contextMenu.Items.IndexOf(mainCtrl.NodeContextMenu.mSepPluginEnd);
            ToolStripItem menuItem = contextMenu.Items[--index];
            while (menuItem is ToolStripSeparator || (menuItem?.Tag != null))
            {
                if (!(menuItem is ToolStripSeparator))
                {
                    Plugins.MenuItem menuItemAdaptor = ((Plugins.MenuItem)menuItem.Tag);
                    action(menuItemAdaptor);
                }
                menuItem = contextMenu.Items[--index];
            }
        }

        #endregion

        internal void InitializeMainMenu(View.IMainForm mainManuCtrl)
        {
            foreach(IPlugin p in Plugins)
            {
                IPluginMainMenu plugin = p as IPluginMainMenu;
                if(plugin != null)
                {
                    var menu = plugin.CreateMainMenuItems();
                    if (menu != null)
                        mainManuCtrl.InsertMenuItems(menu);
                }
            }
        }

        public void InitializeSideBarWindow(TabControl sidebar)
        {
            foreach(IPlugin plugin in Plugins)
            {
                var controls = plugin.CreateSideBarWindows();
                
                foreach(Control ctrl in controls)
                {
                    TabPage tPage = new TabPage(ctrl.Text);
                    ctrl.Dock = DockStyle.Fill;
                    tPage.Controls.Add(ctrl);
                    sidebar.TabPages.Add(tPage);
                }
            }
        }

        internal void OnMapNodeContextMenuOpening(SelectedNodes selectedNodes)
        {
            for(int i = 0; i < Plugins.Count; i++)
            {
                IPluginMapNodeContextMenu plugin = Plugins[i] as IPluginMapNodeContextMenu;
                if(plugin != null)
                {
                    plugin.OnContextMenuOpening(selectedNodes);
                }
            }
        }

        internal void OnMainMenuOpening(Plugins.MainMenuLocation menu, SelectedNodes selectedNodes)
        {

        }

        internal void OnApplicationReady()
        {
            Plugins.ForEach(p => p.OnApplicationReady());
        }

        #region New/Open/Close Tree

        private void OnTreeCreating(MapTree tree)
        {
            foreach (IPlugin plugin in Plugins)
            {
                plugin.OnCreatingTree(tree);
            } 
        }

        private void OnTreeDeleting(MapTree tree)
        {
            foreach (IPlugin plugin in Plugins)
            {
                plugin.OnDeletingTree(tree);
            }
        }

        private void PersistentManager_NewTreeCreating(Serialization.PersistenceManager manager, Serialization.PersistentTree tree)
        {
            OnTreeCreating(tree);         
        }


        private void PersistentManager_TreeOpening(Serialization.PersistenceManager manager, Serialization.PersistentTree tree)
        {
            OnTreeCreating(tree);
        }

        private void PersistentManager_TreeClosing(Serialization.PersistenceManager manager, Serialization.PersistentTree tree)
        {
            OnTreeDeleting(tree);
        }

        #endregion New/Open/Close Tree

        #region IPluginManager Interface

        public void FocusMapEditor()
        {
            mainCtrl.ReturnFocusToMapView();
        }

        public void ScheduleTask(TaskScheduler.ITask task)
        {
            mainCtrl.ScheduleTask(task);
        }        

        public void RescheduleTask(TaskScheduler.ITask task, DateTime startTime)
        {
            mainCtrl.RescheduleTask(task, startTime);
        }

        /// <summary>
        /// Currently active MapTree
        /// </summary>
        public MapTree ActiveTree
        {
            get { return mainCtrl.CurrentMapCtrl.MapView.Tree; }
        }

        /// <summary>
        /// Selected Nodes of the currently active MapTree
        /// </summary>
        public SelectedNodes ActiveNodes
        {
            get { return mainCtrl.ActiveNodes; }
        }

        #endregion IPluginManager Interface
    }
}
