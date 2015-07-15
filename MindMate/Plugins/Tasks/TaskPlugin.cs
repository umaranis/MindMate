using MindMate.MetaModel;
using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskPlugin : IPlugin
    {

        private PendingTaskList pendingTasks;
        private CompletedTaskList completedTasks;
        private DateTimePicker dateTimePicker; 
        private TaskListView taskList;
        private IPluginManager pluginManager;

        public void Initialize(IPluginManager pluginMgr)
        {
            this.pluginManager = pluginMgr;

            pendingTasks = new PendingTaskList();
            completedTasks = new CompletedTaskList();
            pendingTasks.TaskChanged += PendingTasks_TaskChanged;

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
                    tv.MapNode.RemoveTask();
                    break;
                case TaskView.TaskViewEvent.Complete:
                    tv.MapNode.CompleteTask();
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
                    tv.MapNode.SetDueDate(DateHelper.GetDefaultDueDateToday());
                    break;
                case TaskView.TaskViewEvent.Tomorrow:
                    tv.MapNode.SetDueDate(DateHelper.GetDefaultDueDateTomorrow());
                    break;
                case TaskView.TaskViewEvent.NextWeek:
                    tv.MapNode.SetDueDate(DateHelper.GetDefaultDueDateNextWeek());
                    break;
                case TaskView.TaskViewEvent.NextMonth:
                    tv.MapNode.SetDueDate(DateHelper.GetDefaultDueDateNextMonth());
                    break;
                case TaskView.TaskViewEvent.NextQuarter:
                    tv.MapNode.SetDueDate(DateHelper.GetDefaultDueDateNextQuarter());
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
            pendingTasks.RegisterMap(tree);
            completedTasks.RegisterMap(tree);

            tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged += tree_NodePropertyChanged;
            tree.TreeStructureChanged += tree_TreeStructureChanged;

            tree.AttributeChanged += Task.OnAttributeChanged;
        }

        public void OnDeletingTree(MapTree tree)
        {
            pendingTasks.UnregisterMap(tree);
            completedTasks.UnregisterMap(tree);

            tree.SelectedNodes.NodeSelected -= SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected -= SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged -= tree_NodePropertyChanged;
            tree.TreeStructureChanged -= tree_TreeStructureChanged;

            tree.AttributeChanged += Task.OnAttributeChanged;

            taskList.Clear(tree);
        }

        private void PendingTasks_TaskChanged(MapNode node, PendingTaskEventArgs e)
        {
            switch(e.TaskChange)
            {
                case PendingTaskChange.TaskAdded:
                    taskList.Add(node);
                    break;
                case PendingTaskChange.TaskCompleted:
                case PendingTaskChange.TaskRemoved:
                    taskList.RemoveTask(node, e.OldDueDate);
                    break;
                case PendingTaskChange.DueDateUpdated:
                    taskList.RefreshTaskDueDate(node, e.OldDueDate);
                    break;
            }
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
