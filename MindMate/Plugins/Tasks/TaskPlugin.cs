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
        private TaskListView taskList;
        private IPluginManager pluginManager;

        public void Initialize(IPluginManager pluginMgr)
        {
            this.pluginManager = pluginMgr;
            dateTimePicker = new DateTimePicker();
            taskList = new TaskListView();
            taskList.TaskViewEvent += taskList_TaskViewEvent;

            pluginMgr.ScheduleTask(new TaskSchedular.Task()
            {
                StartTime = DateTime.Today.AddDays(1),
                TaskAction = () =>
                {
                    taskList.Invoke((Action)taskList.RefreshTaskList);
                },
                Recurrance = TimeSpan.FromDays(1)
            });
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
                case TaskView.TaskViewEvent.Defer:
                    MoveDown(tv);
                    break;
                case TaskView.TaskViewEvent.Expedite:
                    MoveUp(tv);
                    break;
                case TaskView.TaskViewEvent.Edit:
                    SetDueDate(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.Today:
                    SetDueDate(tv.MapNode, DateHelper.GetDefaultDueDateToday());
                    break;
                case TaskView.TaskViewEvent.Tomorrow:
                    SetDueDate(tv.MapNode, DateHelper.GetDefaultDueDateTomorrow());
                    break;
                case TaskView.TaskViewEvent.NextWeek:
                    SetDueDate(tv.MapNode, DateHelper.GetDefaultDueDateNextWeek());
                    break;
                case TaskView.TaskViewEvent.NextMonth:
                    SetDueDate(tv.MapNode, DateHelper.GetDefaultDueDateNextMonth());
                    break;
                case TaskView.TaskViewEvent.NextQuarter:
                    SetDueDate(tv.MapNode, DateHelper.GetDefaultDueDateNextQuarter());
                    break;

            }

            pluginManager.FocusMapEditor();            
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

        public void OnCreatingTree(MapTree tree)
        {
            tree.AttributeChanged += tree_AttributeChanged;

            tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged += tree_NodePropertyChanged;
            tree.TreeStructureChanged += tree_TreeStructureChanged;
        }

        public void OnDeletingTree(MapTree tree)
        {
            tree.AttributeChanged -= tree_AttributeChanged;

            tree.SelectedNodes.NodeSelected -= SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected -= SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged -= tree_NodePropertyChanged;
            tree.TreeStructureChanged -= tree_TreeStructureChanged;

            taskList.Clear(tree);
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
                taskList.RemoveTask(node, DateHelper.ToDateTime(e.oldValue.value));

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
            else if (e.ChangeType == AttributeChange.Added && e.newValue.AttributeSpec.IsDueDate())
            {
                taskList.Add(node);

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if (e.ChangeType == AttributeChange.ValueUpdated && e.newValue.AttributeSpec.IsDueDate())
            {
                taskList.RefreshTaskDueDate(node, DateHelper.ToDateTime(e.oldValue.value));
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

        void tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            if(e.ChangedProperty == NodeProperties.Text)
            {
                // update task title
                if (node.DueDateExists())
                {
                    TaskView tv = taskList.FindTaskView(node, node.GetDueDate());
                    if (tv != null) tv.TaskTitle = node.Text;
                }

                // update task parent
                taskList.RefreshTaskList(node, tv => tv.RefreshTaskPath());
            }            
        }

        void tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs e)
        {
            if(e.ChangeType == TreeStructureChange.Deleting)
            {
                taskList.RefreshTaskList(node, tv => taskList.RemoveTask(tv.MapNode, tv.MapNode.GetDueDate()));
            }
            else if(e.ChangeType == TreeStructureChange.Detaching)
            {
                taskList.RefreshTaskList(node, tv => taskList.RemoveTask(tv.MapNode, tv.MapNode.GetDueDate()));
            }
            else if(e.ChangeType == TreeStructureChange.Attached)
            {
                node.ForEach((n) =>
                    {
                        if (n.DueDateExists())
                            taskList.Add(n);
                    });
            }
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
            taskList.MoveDown(tv);
        }

        private void MoveUp(TaskView tv)
        {
            taskList.MoveUp(tv);
        }
    }
}
