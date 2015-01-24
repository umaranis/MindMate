using MindMate.Controller;
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
            Plugins.ForEach(a => a.Initialize(this));
        }

        public void InitializeContextMenu(ContextMenuCtrl contextMenuCtrl)
        {
            foreach(IPlugin plugin in Plugins)
            {
                var menu = plugin.CreateContextMenuItemsForNode();
                if(menu != null)
                    contextMenuCtrl.InsertMenuItems(menu);
            }
        }

        
    }
}
