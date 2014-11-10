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
        public void Initialize(PluginManager pluginMgr)
        {
        }

        public MenuItem[] CreateContextMenuItemsForNode()
        {
            var t2 = new MenuItem("Quick Due Dates", null, 
                new MenuItem("Today"),
                new MenuItem("Tomorrow"),
                new MenuItem("This Week"),
                new MenuItem("Next Week"),
                new MenuItem("This Month"),
                new MenuItem("Next Month"),
                new MenuItem("No Date")
                );
            

            MenuItem[] menuItems = new MenuItem[] 
            {
                new MenuItem("Set Due Date ...", null, SetDueDate_Click),
                t2
            };

            return menuItems;
        }

        private void SetDueDate_Click(MenuItem menu, SelectedNodes nodes)
        {
            new DateTimePicker().ShowDialog();
        }
                        
        public void CreateMainMenuItems(out MenuItem[] menuItems, out MainMenuLocation position)
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
