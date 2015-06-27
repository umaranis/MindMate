using MindMate.Controller;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins
{
    public class PluginManager : IPluginManager
    {
        private MainCtrl mainCtrl;

        public List<IPlugin> Plugins { get; private set; }

        public PluginManager(MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            Plugins = new List<IPlugin>();

            LoadPlugins();
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

        public void InitializeContextMenu(ContextMenuCtrl contextMenuCtrl)
        {
            foreach(IPlugin p in Plugins)
            {
                IPluginMapNodeContextMenu plugin = p as IPluginMapNodeContextMenu;
                if (plugin != null)
                {
                    var menu = plugin.CreateContextMenuItemsForNode();
                    if (menu != null)
                        contextMenuCtrl.InsertMenuItems(menu);
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

        internal void OnTreeCreating(MapTree tree)
        {
            foreach (IPlugin plugin in Plugins)
            {
                plugin.OnCreatingTree(tree);
            } 
        }

        internal void OnTreeDeleting(MapTree tree)
        {
            foreach (IPlugin plugin in Plugins)
            {
                plugin.OnDeletingTree(tree);
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

        #region IPluginManager Interface

        public void FocusMapEditor()
        {
            mainCtrl.ReturnFocusToMapView();
        }

        public void ScheduleTask(TaskSchedular.Task task)
        {
            mainCtrl.ScheduleTask(task);
        }        

        #endregion IPluginManager Interface
    }
}
