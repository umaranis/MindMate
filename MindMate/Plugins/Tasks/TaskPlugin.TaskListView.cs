using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskPlugin
    {
        private void PendingTasks_TaskChanged(MapNode node, PendingTaskEventArgs e)
        {
            switch (e.TaskChange)
            {
                case PendingTaskChange.TaskAdded:
                case PendingTaskChange.TaskReopened:
                    taskListView.Add(node);
                    break;
                case PendingTaskChange.TaskCompleted:
                case PendingTaskChange.TaskRemoved:
                    taskListView.RemoveTask(node, e.OldDueDate);
                    break;
                case PendingTaskChange.DueDateUpdated:
                    taskListView.RefreshTaskDueDate(node, e.OldDueDate);
                    break;
            }
        }

        private void PendingTasks_TaskTextChanged(MapNode node, TaskTextEventArgs e)
        {
            switch (e.ChangeType)
            {
                case TaskTextChange.TextChange:
                    taskListView.RefreshTaskText(node);
                    break;
                case TaskTextChange.AncestorTextChange:
                    taskListView.RefreshTaskPath(node);
                    break;
            }
        }

        private void PendingTasks_TaskSelectionChanged(MapNode node, TaskSelectionEventArgs e)
        {
            switch (e.ChangeType)
            {
                case TaskSelectionChange.Selected:
                    taskListView.SelectNode(node);
                    break;
                case TaskSelectionChange.Deselected:
                    taskListView.DeselectNode(node);
                    break;
            }
        }

        /// <summary>
        /// Used to refresh task list view as date changes
        /// </summary>
        private void RefreshTaskListView()
        {
            taskListView.RefreshTaskList();

            //refresh for rest of the tasks which are not displayed
            TaskView lastTV = (TaskView)taskListView.GetLastControl();
            if (lastTV != null)
            {
                int startIndex = pendingTasks.IndexOf(lastTV.MapNode) + 1;
                MapNode node = (startIndex < pendingTasks.Count) ? pendingTasks[startIndex] : null;
                while (node != null)
                {
                    DateTime dueDate = node.GetDueDate();
                    if (taskListView.GetApplicableGroup(dueDate) != null)
                    {
                        taskListView.Add(node);
                        startIndex++;
                        node = (startIndex < pendingTasks.Count) ? pendingTasks[startIndex] : null;
                    }
                    else
                    {
                        node = null;
                    }
                }
            }

        }

        public void OnTaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            switch (e)
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
                    SetDueDateUsingPicker(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.Today:
                    SetDueDateToday(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.Tomorrow:
                    SetDueDateTomorrow(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.NextWeek:
                    SetDueDateNextWeek(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.NextMonth:
                    SetDueDateNextMonth(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.NextQuarter:
                    SetDueDateNextQuarter(tv.MapNode);
                    break;

            }

            PluginManager.FocusMapEditor();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tv"></param>
        private void MoveDown(TaskView tv)
        {
            taskListView.MoveDown(tv);
        }

        private void MoveUp(TaskView tv)
        {
            taskListView.MoveUp(tv);
        }
    }
}
