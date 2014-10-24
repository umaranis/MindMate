using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public class TaskPlugin : IPlugin
    {
        private IPluginManager pluginMgr;
        public void Initialize(PluginManager pluginMgr)
        {
            this.pluginMgr = pluginMgr;
        }

        public ToolStripMenuItem[] CreateContextMenuItemsForNode()
        {
            var t2 = new ToolStripMenuItem("Quick Due Dates", null, 
                new ToolStripMenuItem("Today"),
                new ToolStripMenuItem("Tomorrow"),
                new ToolStripMenuItem("This Week"),
                new ToolStripMenuItem("Next Week"),
                new ToolStripMenuItem("This Month"),
                new ToolStripMenuItem("Next Month"),
                new ToolStripMenuItem("No Date")
                );
            

            ToolStripMenuItem[] menuItems = new ToolStripMenuItem[] 
            {
                new ToolStripMenuItem("Set Due Date ..."),
                t2
            };

            return menuItems;
        }
        
        public void CreateMainMenuItems(out ToolStripMenuItem[] menuItems, out MainMenuLocation position, out int priority)
        {
            throw new NotImplementedException();
        }

        public void RegisterTreeEvents(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }

        public void UnregisterTreeEvents(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }
                       
        public void CreateMainMenuItems(out ToolStripMenuItem[] menuItems, out MainMenuLocation location)
        {
            throw new NotImplementedException();
        }
        
    }
}
