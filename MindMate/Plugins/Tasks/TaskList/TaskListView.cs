using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using MindMate.Plugins.Tasks.SideBar;
using System;
using System.Collections.Generic;

namespace MindMate.Plugins.Tasks
{
    public class TaskListView : MindMate.Plugins.Tasks.SideBar.SideBar
    {
        /// <summary>
        /// Today's date as per TaskListView. This is used to ensure that TaskListView is upto date.
        /// </summary>
        private DateTime todayDate = DateTime.Today;
        
        public TaskListView()
        {
            ControlGroup taskGroupOverdue;
            ControlGroup taskGroupToday;
            ControlGroup taskGroupTomorrow;
            ControlGroup taskGroupThisWeek;
            ControlGroup taskGroupThisMonth;
            ControlGroup taskGroupNextMonth;

            taskGroupOverdue = this.ControlGroups.Add("Overdue", System.Drawing.Color.Red);
            taskGroupToday = this.ControlGroups.Add("Today", System.Drawing.Color.Black);
            taskGroupTomorrow = this.ControlGroups.Add("Tomorrow", System.Drawing.Color.Black);
            taskGroupThisWeek = this.ControlGroups.Add("This Week", System.Drawing.Color.Black);
            taskGroupThisMonth = this.ControlGroups.Add("This Month", System.Drawing.Color.Black);
            taskGroupNextMonth = this.ControlGroups.Add("Next Month", System.Drawing.Color.Black);
            
            taskGroupOverdue.Tag = new TaskGroupOverdue();
            taskGroupToday.Tag = new TaskGroupToday();
            taskGroupTomorrow.Tag = new TaskGroupTomorrow(); 
            taskGroupThisWeek.Tag = new TaskGroupThisWeek();
            taskGroupThisMonth.Tag = new TaskGroupThisMonth();
            taskGroupNextMonth.Tag = new TaskGroupNextMonth();
        }

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;

        public void Add(MapNode node)
        {
            DateTime dateTime = node.GetDueDate();
            ControlGroup ctrlGroup = GetApplicableGroup(dateTime);

            if (ctrlGroup != null)
            {
                ITaskGroup taskGroup = (ITaskGroup)ctrlGroup.Tag;
                TaskView tv = new TaskView(node, taskGroup.ShortDueDateString(dateTime), OnTaskViewEvent);
                AddToGroup(ctrlGroup, tv);
            }       
        }

        public void Add(TaskView tv)
        {
            ControlGroup ctrlGroup = GetApplicableGroup(tv.DueDate);

            if (ctrlGroup != null)
            {
                ITaskGroup taskGroup = (ITaskGroup)ctrlGroup.Tag;
                tv.TaskDueOnText = taskGroup.ShortDueDateString(tv.DueDate);
                tv.RefreshTaskPath();
                tv.TaskTitle = tv.MapNode.Text;
                AddToGroup(ctrlGroup, tv);
            }            
        }

        public ControlGroup GetApplicableGroup(DateTime dueDate)
        {
            for (int i = 0; i < this.ControlGroups.Count; i++)
            {
                ControlGroup ctrlGroup = this.ControlGroups[i];
                ITaskGroup taskGroup = (ITaskGroup)ctrlGroup.Tag;
                if (taskGroup.CanContain(dueDate))
                {
                    return ctrlGroup;
                }
            }

            // not found
            return null;
        }

        private void AddToGroup(ControlGroup taskGroup, TaskView taskView)
        {
            if (taskGroup.Count == 0)
            {
                taskGroup.Add(taskView); // add if list is empty
            }
            else
            {
                for (int i = taskGroup.Count - 1; i >= 0; i--)
                {
                    TaskView tv = (TaskView)taskGroup[i];
                    if (tv.DueDate > taskView.DueDate)
                    {
                        continue;
                    }
                    else
                    {
                        taskGroup.Insert(i + 1, taskView); // add in the middle or end
                        return;
                    }
                }

                taskGroup.Insert(0, taskView); // add at the top after all controls are moved down using loop
            }

        }

        private void RemoveTask(TaskView tv)
        {
            var taskGroup = GetControlGroup(tv);

            taskGroup.Remove(tv);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate">old DueDate in case there is a change not reflected in TaskList</param>
        public void RemoveTask(MapNode node, DateTime dueDate)
        {
            TaskView tv = this.FindTaskView(node, dueDate);
            if (tv != null)
            {
                RemoveTask(tv);
            }            
        }

        public ITaskGroup GetTaskGroup(TaskView tv)
        {
            return (ITaskGroup)GetControlGroup(tv).Tag;
        }

        public ITaskGroup GetTaskGroup(ControlGroup cg)
        {
            return (ITaskGroup)cg.Tag;
        }

        /// <summary>
        /// returns null if TaskView is not found
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate">DueDate is available from MapNode (first parameter) also. But it is important to pass it separately as TaskList might be out of order, in such a case old value can be provided in dueDate parameter</param>
        /// <returns>returns null if not found</returns>
        public TaskView FindTaskView(MapNode node, DateTime dueDate)
        {
            if (!todayDate.Equals(DateTime.Today))
            {
                RefreshTaskList();
            }

            for (int i = 0; i < this.ControlGroups.Count; i++)
            {
                ControlGroup ctrlGroup = this.ControlGroups[i];
                ITaskGroup taskGroup = (ITaskGroup)ctrlGroup.Tag;
                if (taskGroup.CanContain(dueDate))
                {
                    return FindTaskViewInGroup(ctrlGroup, node, dueDate);
                }
            }

            return null;
        }

        private TaskView FindTaskViewInGroup(ControlGroup taskGroup, MapNode node, DateTime dueDate)
        {
            for (int i = 0; i < taskGroup.Count; i++)
            {
                TaskView tv = (TaskView)taskGroup[i];
                if (tv.MapNode == node)
                {
                    return tv;
                }

                if (tv.DueDate > dueDate)
                {
                    break;
                }
            }

            return null;
        }

        private void OnTaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            TaskViewEvent(tv, e);
        }

        public void MoveDown(TaskView tv)
        {
            DateTime updateDate = tv.DueDate.AddDays(5); //new date for TaskView  (setting default value, used if nothing else works)

            TaskView nextTV = (TaskView)GetNextControl(tv, true);
            if (nextTV != null) //1- Move Down within a group
            {
                //1.1 Calculate due date 1 hour after next
                DateTime nextDueDate = nextTV.MapNode.GetDueDate();
                updateDate = nextDueDate.AddHours(1);

                //1.2 Check if it falls between 'next' and 'next to next'
                TaskView nextToNext = (TaskView)GetNextControl(nextTV, true);
                if (nextToNext != null)
                {
                    DateTime nextToNextDueDate = nextToNext.MapNode.GetDueDate();
                    if (updateDate > nextToNextDueDate)
                    {
                        updateDate = updateDate.AddTicks((nextToNextDueDate - nextDueDate).Ticks / 2);
                    }
                }

                //1.3 Check if calculated due date stays within the group
                if(!GetTaskGroup(tv).CanContain(updateDate))
                {
                    updateDate = GetTaskGroup(tv).EndTime;                   
                }

            }
            else //2- Move Down to next group
            {
                ControlGroup cg = GetNextControlGroup(GetControlGroup(tv));

                //2.1 Check if we are not in the last group
                if (cg != null)
                {
                    //2.1.1 set the default first due date of the group
                    updateDate = GetTaskGroup(cg).StartTime.AddHours(7);

                    //2.1.2 Check if due date is before the first existing item
                    if (cg.Count > 0 && ((TaskView)cg[0]).DueDate <= updateDate)
                        updateDate = GetTaskGroup(cg).StartTime;

                    //2.1.3 Check if due date is before the orignal date
                    if (updateDate <= tv.DueDate)
                    {
                        if (GetTaskGroup(cg).EndTime.Date <= tv.DueDate)
                            updateDate = tv.DueDate.AddDays(1);
                        else
                            updateDate = GetTaskGroup(cg).EndTime.Date;
                    }

                }
            }

            //3- Update due date
            tv.MapNode.UpdateDueDate(updateDate);
        }

        public void MoveUp(TaskView tv)
        {
            DateTime updateDate = tv.DueDate.Subtract(TimeSpan.FromDays(1)); //new date for TaskView  (setting default value, used if nothing else works)

            TaskView prevTV = (TaskView)GetPreviousControl(tv, true);
            if (prevTV != null) //1- Move Down within a group
            {
                //1.1 Calculate due date 1 hour before previous
                DateTime previousDueDate = prevTV.MapNode.GetDueDate();
                updateDate = previousDueDate.Subtract(TimeSpan.FromHours(1));

                //1.2 Check if it falls between 'previous' and 'previous to previous'
                TaskView previousToPrevious = (TaskView)GetPreviousControl(prevTV, true);
                if (previousToPrevious != null)
                {
                    DateTime previousToPreviousDueDate = previousToPrevious.MapNode.GetDueDate();
                    if (updateDate < previousToPreviousDueDate)
                    {
                        updateDate = updateDate.Subtract(TimeSpan.FromTicks((previousToPreviousDueDate - previousDueDate).Ticks / 2));
                    }
                }

                //1.3 Check if calculated due date stays within the group
                if (!GetTaskGroup(tv).CanContain(updateDate))
                {
                    updateDate = GetTaskGroup(tv).StartTime;
                }

            }
            else //2- Move up to previous group
            {
                ControlGroup cg = GetPreviousControlGroup(GetControlGroup(tv));

                //2.1 Check if we are not in the first group
                if (cg != null)
                {
                    //2.1.1 set the default first due date of the group
                    updateDate = GetTaskGroup(cg).EndTime.Subtract(new TimeSpan(16,59,59));

                    //2.1.2 Check if due date is after the last existing item
                    if (cg.Count > 0 && ((TaskView)cg[cg.Count - 1]).DueDate >= updateDate)
                        updateDate = GetTaskGroup(cg).EndTime;

                    //2.1.3 Check if due date is after the orignal date
                    if (updateDate > tv.DueDate)
                        updateDate = GetTaskGroup(cg).StartTime.Date;

                }
            }

            //3- Update due date
            tv.MapNode.UpdateDueDate(updateDate);
        }

        public void SelectNode(MapNode node)
        {
            TaskView tv = FindTaskView(node, node.GetDueDate());
            if (tv != null) tv.Selected = true;
        }

        public void DeselectNode(MapNode node)
        {
            TaskView tv = FindTaskView(node, node.GetDueDate());
            if (tv != null) tv.Selected = false;
        }

        /// <summary>
        /// Refreshes TaskList if Due Date is changed for a MapNode
        /// </summary>
        /// <param name="node"></param>
        public void RefreshTaskDueDate(MapNode node, DateTime oldDueDate)
        {
            RemoveTask(node, oldDueDate);
            Add(node);            
        }

        public void RefreshTaskText(MapNode node)
        {
            TaskView tv = FindTaskView(node, node.GetDueDate());
            if (tv != null) tv.TaskTitle = node.Text;
        }

        public void RefreshTaskPath(MapNode node)
        {
            TaskView tv = FindTaskView(node, node.GetDueDate());
            if (tv != null) tv.RefreshTaskPath();
        }
        
        /// <summary>
        /// Called as the day changes to refresh task list
        /// </summary>
        public void RefreshTaskList()
        {
            TaskView tv = (TaskView)GetFirstControl();
            while(tv != null)
            {
                RemoveTask(tv);
                Add(tv);
                tv = (TaskView)GetNextControl(tv);
            }          
            
            todayDate = DateTime.Today;  
        }        

        public void Clear(MapTree tree)
        {
            TaskView tv = (TaskView)GetFirstControl();
            while (tv != null)
            {
                TaskView nextTV = (TaskView)GetNextControl(tv);

                if (tv.MapNode.Tree == tree) RemoveTask(tv);

                tv = nextTV;                
            }
        }
    }
}
