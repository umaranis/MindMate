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

            this.taskGroupOverdue.Tag = new TaskGroupOverdue();
            this.taskGroupToday.Tag = new TaskGroupToday();
            this.taskGroupTomorrow.Tag = new TaskGroupTomorrow(); 
            this.taskGroupThisWeek.Tag = new TaskGroupThisWeek();
            this.taskGroupThisMonth.Tag = new TaskGroupThisMonth();
            this.taskGroupNextMonth.Tag = new TaskGroupNextMonth();
        }

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;

        public void Add(MindMate.Model.MapNode node, DateTime dateTime)
        {
            for (int i = 0; i < this.ControlGroups.Count; i++)
            {
                ControlGroup ctrlGroup = this.ControlGroups[i];
                ITaskGroup taskGroup = (ITaskGroup)ctrlGroup.Tag;
                if(taskGroup.CanContain(dateTime))
                {
                    AddTask(ctrlGroup, node, dateTime, taskGroup.ShortDueDateString(dateTime));
                    break;
                }
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
            var taskGroup = GetControlGroup(tv);

            taskGroup.Remove(tv);          

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

        public ITaskGroup GetTaskGroup(TaskView tv)
        {
            return (ITaskGroup)GetControlGroup(tv).Tag;
        }

        /// <summary>
        /// returns null if TaskView is not found
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate"></param>
        /// <returns>returns null if not found</returns>
        public TaskView FindTaskView(MapNode node, DateTime dueDate)
        {
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
                if(!GetTaskGroup(tv).CanContain(updateDate))
                {
                                        
                }

                //1.4 Update due date
                tv.MapNode.SetDueDate(updateDate);
            }
        }
    }
}
