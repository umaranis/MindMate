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
        private MindMate.Plugins.Tasks.SideBar.ControlGroup collapsiblePanelThisWeek;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup collapsiblePanelTomorrow;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup collapsiblePanelOverdue;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup collapsiblePanelToday;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup collapsiblePanelNextMonth;
        private MindMate.Plugins.Tasks.SideBar.ControlGroup collapsiblePanelThisMonth;

        public TaskList()
        {
            this.collapsiblePanelOverdue = this.ControlGroups.Add("Overdue", System.Drawing.Color.Red);
            this.collapsiblePanelToday = this.ControlGroups.Add("Today", System.Drawing.Color.Black);
            this.collapsiblePanelTomorrow = this.ControlGroups.Add("Tomorrow", System.Drawing.Color.Black);
            this.collapsiblePanelThisWeek = this.ControlGroups.Add("This Week", System.Drawing.Color.Black);
            this.collapsiblePanelThisMonth = this.ControlGroups.Add("This Month", System.Drawing.Color.Black);
            this.collapsiblePanelNextMonth = this.ControlGroups.Add("Next Month", System.Drawing.Color.Black);
        }

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;

        public void Add(MindMate.Model.MapNode node, DateTime dateTime)
        {
            if (DateHelper.IsOverdue(dateTime))
            {
                AddTask(this.collapsiblePanelOverdue, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if (DateHelper.IsToday(dateTime))
            {
                AddTask(this.collapsiblePanelToday, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }
            else if (DateHelper.IsTomorrow(dateTime))
            {
                AddTask(this.collapsiblePanelTomorrow, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }
            else if (DateHelper.DateInThisWeek(dateTime))
            {
                AddTask(this.collapsiblePanelThisWeek, node, dateTime,
                    DateHelper.GetWeekDayString(dateTime));
            }
            else if (DateHelper.DateInThisMonth(dateTime))
            {
                AddTask(this.collapsiblePanelThisMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if (DateHelper.DateInNextMonth(dateTime))
            {
                AddTask(this.collapsiblePanelNextMonth, node, dateTime,
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
                return FindTaskViewInGroup(collapsiblePanelOverdue, node, dueDate);
            }
            else if (DateHelper.IsToday(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelToday, node, dueDate);
            }
            else if (DateHelper.IsTomorrow(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelTomorrow, node, dueDate);
            }
            else if (DateHelper.DateInThisWeek(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelThisWeek, node, dueDate);
            }
            else if (DateHelper.DateInThisMonth(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelThisMonth, node, dueDate);
            }
            else if (DateHelper.DateInNextMonth(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelNextMonth, node, dueDate);
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
    }
}
