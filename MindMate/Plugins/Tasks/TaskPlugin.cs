using MindMate.MetaModel;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskPlugin : IPlugin
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
                        
        private MapTree.AttributeSpec CreateDueDateAttributeSpec(MapTree tree)
        {
            return new MapTree.AttributeSpec(
                tree, ATT_DUE_DATE, true, MapTree.AttributeDataType.DateTime, 
                MapTree.AttributeListOption.NoList, null, MapTree.AttributeType.System);
        }

        public static bool IsDueDateAttributeSpec(MapTree.AttributeSpec aspec)
        {
            return aspec.Name == ATT_DUE_DATE && aspec.Type == MapTree.AttributeType.System;
        }

        public static MapTree.AttributeSpec GetDueDateAttributeSpec(MapTree tree)
        {
            MapTree.AttributeSpec aspec = tree.GetAttributeSpec(ATT_DUE_DATE);
            return (aspec != null && aspec.Type == MapTree.AttributeType.System) ? aspec : null;
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

        private void tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            if (e.ChangeType == AttributeChange.Removed && IsDueDateAttributeSpec(e.oldValue.AttributeSpec)) 
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
            else if (e.ChangeType == AttributeChange.Added && IsDueDateAttributeSpec(e.newValue.AttributeSpec))
            {
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if (e.ChangeType == AttributeChange.ValueUpdated && IsDueDateAttributeSpec(e.newValue.AttributeSpec))
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
                       
        
    }
}
