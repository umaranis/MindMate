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
            switch(e)
            { 
                case TaskView.TaskViewEvent.Remove:
                    RemoveTask(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.Complete:
                    CompleteTask(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.MoveDown:
                    MoveDown(tv);
                    break;
                case TaskView.TaskViewEvent.MoveUp:
                    MoveUp(tv);
                    break;
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

            tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;
        }

        void SelectedNodes_NodeSelected(MapNode node, SelectedNodes selectedNodes)
        {
            if(node.DueDateExists())
            {
                TaskView tv = taskList.FindTaskView(node, node.GetDueDate());
                if(tv != null) tv.Selected = true;
            }
        }

        void SelectedNodes_NodeDeselected(MapNode node, SelectedNodes selectedNodes)
        {
            if (node.DueDateExists())
            {
                TaskView tv = taskList.FindTaskView(node, node.GetDueDate());
                if(tv != null) tv.Selected = false;
            }
        }

        private void tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            // Due Date attribute changed
            if (e.ChangeType == AttributeChange.Removed && e.oldValue.AttributeSpec.IsDueDate()) 
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
            else if (e.ChangeType == AttributeChange.Added && e.newValue.AttributeSpec.IsDueDate())
            {
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if (e.ChangeType == AttributeChange.ValueUpdated && e.newValue.AttributeSpec.IsDueDate())
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));
            }
            // Comletion Date attribute changed
            else if (e.ChangeType == AttributeChange.Added && e.newValue.AttributeSpec.IsCompletionDate())
            {
                TaskCompleteIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if(e.ChangeType == AttributeChange.Removed && e.oldValue.AttributeSpec.IsCompletionDate())
            {
                TaskCompleteIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
        }
                
        public void OnDeletingTree(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }

        private void SetDueDate(MapNode node, DateTime dateTime)
        {
            node.SetDueDate(dateTime);
            node.RemoveCompletionDate();
        }
                       
        private void CompleteTask(MapNode node)
        {
            if (node.DueDateExists())
            {
                node.SetTargetDate(node.GetDueDate());
                node.RemoveDueDate();
                
            }

            node.SetCompletionDate(DateTime.Now);
        }

        private void RemoveTask(MapNode node)
        {
            node.RemoveDueDate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tv"></param>
        private void MoveDown(TaskView tv)
        {
            TaskView nextTV = taskList.GetNextTaskViewInGroup(tv);
            if(nextTV != null) //1- Move Down within a group
            {
                //1.1 Calculate due date 1 hour after next
                DateTime nextDueDate = nextTV.MapNode.GetDueDate();
                DateTime updateDate = nextDueDate.AddHours(1);
                
                //1.2 Check if it falls between 'next' and 'next to next'
                TaskView nextToNext = taskList.GetNextTaskViewInGroup(nextTV);
                if(nextToNext != null)
                {
                    DateTime nextToNextDueDate = nextToNext.MapNode.GetDueDate();
                    if(updateDate > nextToNextDueDate)
                    {
                        updateDate = updateDate.AddTicks((nextToNextDueDate - nextDueDate).Ticks / 2);
                    }
                }

                //1.3 Check if calculated due date stays within the group
                //if()
                    
                //1.4 Update due date
                tv.MapNode.SetDueDate(updateDate);
            }
        }

        private void MoveUp(TaskView tv)
        {
            throw new NotImplementedException();
        }
    }
}
