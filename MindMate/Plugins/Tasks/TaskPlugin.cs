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
        public static NodeAttribute DueDateAttribute = new NodeAttribute("Due Date");

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
                DueDateAttribute.Delete(tv.MapNode);
            }
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
            if (e.ChangeType == AttributeChange.Removed && DueDateAttribute.SameSpec(e.oldValue.AttributeSpec)) 
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
            else if (e.ChangeType == AttributeChange.Added && DueDateAttribute.SameSpec(e.newValue.AttributeSpec))
            {
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if (e.ChangeType == AttributeChange.ValueUpdated && DueDateAttribute.SameSpec(e.newValue.AttributeSpec))
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
