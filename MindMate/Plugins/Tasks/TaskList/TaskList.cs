using MindMate.Model;
using MindMate.Plugins.Tasks.SideBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public class TaskList : MindMate.Plugins.Tasks.SideBar.SideBar
    {
        private MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroupOverdue;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroupToday;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroupTomorrow;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroupThisWeek;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroupThisMonth;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroupNextMonth;
        

        public TaskList()
        {
            this.taskGroupOverdue = this.ControlGroups.Add("Overdue", System.Drawing.Color.Red);
            this.taskGroupToday = this.ControlGroups.Add("Today", System.Drawing.Color.Black);
            this.taskGroupTomorrow = this.ControlGroups.Add("Tomorrow", System.Drawing.Color.Black);
            this.taskGroupThisWeek = this.ControlGroups.Add("This Week", System.Drawing.Color.Black);
            this.taskGroupThisMonth = this.ControlGroups.Add("This Month", System.Drawing.Color.Black);
            this.taskGroupNextMonth = this.ControlGroups.Add("Next Month", System.Drawing.Color.Black);
        }

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;

        public void Add(MindMate.Model.MapNode node, DateTime dateTime)
        {
            if (DateHelper.IsOverdue(dateTime))
            {
                AddTask(this.taskGroupOverdue, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if (DateHelper.IsToday(dateTime))
            {
                AddTask(this.taskGroupToday, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }
            else if (DateHelper.IsTomorrow(dateTime))
            {
                AddTask(this.taskGroupTomorrow, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }
            else if (DateHelper.DateInThisWeek(dateTime))
            {
                AddTask(this.taskGroupThisWeek, node, dateTime,
                    DateHelper.GetWeekDayString(dateTime));
            }
            else if (DateHelper.DateInThisMonth(dateTime))
            {
                AddTask(this.taskGroupThisMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if (DateHelper.DateInNextMonth(dateTime))
            {
                AddTask(this.taskGroupNextMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }

        }

        private void AddTask(MindMate.Plugins.Tasks.SideBar.ControlGroup taskGroup,
            MindMate.Model.MapNode node, DateTime dateTime, string dueOnText)
        {
            TaskView tv = new TaskView(node, dateTime, dueOnText, OnTaskViewEvent);
                        
            InsertTaskView(taskGroup, tv);
        }

        private void OnTaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            TaskViewEvent(tv, e);
        }

        public void RemoveTask(TaskView tv)
        {
            var taskGroup = GetTaskGroup(tv);

            taskGroup.Remove(tv);          

        }

        public ControlGroup GetTaskGroup(TaskView control)
        {
            return (ControlGroup)control.Parent.Parent;
        }

        
        private void InsertTaskView(ControlGroup taskGroup, TaskView taskView)
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

        /// <summary>
        /// returns null if TaskView is not found
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate"></param>
        /// <returns>returns null if not found</returns>
        public TaskView FindTaskView(MapNode node, DateTime dueDate)
        {
            if (DateHelper.IsOverdue(dueDate))
            {
                return FindTaskViewInGroup(taskGroupOverdue, node, dueDate);
            }
            else if (DateHelper.IsToday(dueDate))
            {
                return FindTaskViewInGroup(taskGroupToday, node, dueDate);
            }
            else if (DateHelper.IsTomorrow(dueDate))
            {
                return FindTaskViewInGroup(taskGroupTomorrow, node, dueDate);
            }
            else if (DateHelper.DateInThisWeek(dueDate))
            {
                return FindTaskViewInGroup(taskGroupThisWeek, node, dueDate);
            }
            else if (DateHelper.DateInThisMonth(dueDate))
            {
                return FindTaskViewInGroup(taskGroupThisMonth, node, dueDate);
            }
            else if (DateHelper.DateInNextMonth(dueDate))
            {
                return FindTaskViewInGroup(taskGroupNextMonth, node, dueDate);
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

        public void MoveDown(TaskView tv)
        {
            TaskView nextTV = (TaskView)GetNextControlInGroup(tv);
            if (nextTV != null) //1- Move Down within a group
            {
                //1.1 Calculate due date 1 hour after next
                DateTime nextDueDate = nextTV.MapNode.GetDueDate();
                DateTime updateDate = nextDueDate.AddHours(1);

                //1.2 Check if it falls between 'next' and 'next to next'
                TaskView nextToNext = (TaskView)GetNextControlInGroup(nextTV);
                if (nextToNext != null)
                {
                    DateTime nextToNextDueDate = nextToNext.MapNode.GetDueDate();
                    if (updateDate > nextToNextDueDate)
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
    }
}
