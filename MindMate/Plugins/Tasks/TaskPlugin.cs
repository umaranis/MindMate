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
        public const string ATT_DUE_DATE = "Due Date";

        private DateTimePicker dateTimePicker; 
        private TasksList taskList;

        public void Initialize(IPluginManager pluginMgr)
        {
            dateTimePicker = new DateTimePicker();
            taskList = new TasksList();
            taskList.TaskViewEvent += taskList_TaskViewEvent;
        }

        void taskList_TaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            if(e == TaskView.TaskViewEvent.Remove)
            {
                tv.MapNode.DeleteAttribute(ATT_DUE_DATE);
            }
        }

        public MenuItem[] CreateContextMenuItemsForNode()
        {
            var t2 = new MenuItem("Quick Due Dates");
 
            t2.AddDropDownItem(new MenuItem("Today"));
            t2.AddDropDownItem(new MenuItem("Tomorrow"));
            t2.AddDropDownItem(new MenuItem("This Week"));
            t2.AddDropDownItem(new MenuItem("Next Week"));
            t2.AddDropDownItem(new MenuItem("This Month"));
            t2.AddDropDownItem(new MenuItem("Next Month"));
            t2.AddDropDownItem(new MenuItem("No Date"));

            var t3 = new MenuItem("Complete Task");
            t3.Opening = Complete_Opening;

            MenuItem[] menuItems = new MenuItem[] 
            {
                new MenuItem("Set Due Date ...", null, SetDueDate_Click),
                t2,
                t3
            };

            return menuItems;
        }

        private void Complete_Opening(MenuItem menuItem, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++ )
            {
                if (nodes[i].ContainsAttribute(ATT_DUE_DATE))
                {
                    menuItem.Enabled = true;
                    return;
                }
            }
            menuItem.Enabled = false;
        }

        private void SetDueDate_Click(MenuItem menu, SelectedNodes nodes)
        {

            MapTree.AttributeSpec aspec = nodes.First.Tree.GetAttributeSpec(ATT_DUE_DATE);
            if (aspec == null)
                aspec = CreateDueDateAttributeSpec(nodes.First.Tree);

            MapNode.Attribute att;
            if (nodes.First.GetAttribute(aspec, out att))
            {
                dateTimePicker.Value = DateHelper.ToDateTime(att.value);
            }
            else
            {
                dateTimePicker.Value = DateTime.Today.Date.AddHours(7);
            }

            if(dateTimePicker.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    MapNode node = nodes[i];

                    if (!node.GetAttribute(aspec, out att))
                        att.AttributeSpec = aspec;

                    att.value = dateTimePicker.Value.ToString();
                    node.AddUpdateAttribute(att);

                }
            }
        }
                
        private MapTree.AttributeSpec CreateDueDateAttributeSpec(MapTree tree)
        {
            return new MapTree.AttributeSpec(
                tree, ATT_DUE_DATE, true, MapTree.AttributeDataType.DateTime, 
                MapTree.AttributeListOption.NoList, null, MapTree.AttributeType.System);
        }
                        
        public void CreateMainMenuItems(out MenuItem[] menuItems, out MainMenuLocation position)
        {
            throw new NotImplementedException();
        }

        public Control[] CreateSideBarWindows()
        {
            taskList.Text = "Tasks";
            return new Control [] { taskList };
        }

        public void OnCreatingTree(Model.MapTree tree)
        {
            tree.AttributeChanged += tree_AttributeChanged;
        }

        void tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            if(e.ChangeType == AttributeChange.Removed)
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);
            }
            else if(e.ChangeType == AttributeChange.Added)
            {
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));
            }
            else if(e.ChangeType == AttributeChange.ValueUpdated)
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));
            }

        }
                
        public void OnDeletingTree(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }
                       
        public void CreateMainMenuItems(out ToolStripMenuItem[] menuItems, out MainMenuLocation location)
        {
            throw new NotImplementedException();
        }
        
        
    }
}
