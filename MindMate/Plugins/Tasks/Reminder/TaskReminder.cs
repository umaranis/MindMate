using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Reminder
{
    /// <summary>
    /// Fires Task Due event whenever a task is due or about to be due
    /// </summary>
    public class TaskReminder : TaskScheduler.ITask
    {
        public const int ADVANCED_REMINDER_MINUTES = 15;
        
        private PendingTaskList taskList;

        public delegate void TaskDueDelegate(MapNode node, ReminderType reminderType);

        public event TaskDueDelegate TaskDue;

        private System.Windows.Forms.Control syncControl;

        private DateTime lastAdvancedReminder = DateTime.Today;
        private DateTime lastReminder = DateTime.Today;

        public TaskReminder(PendingTaskList taskList, System.Windows.Forms.Control syncControl)
        {
            this.taskList = taskList;
            StartTime = DateTime.Now.AddSeconds(5);
            TaskId = "TaskReminder";
            this.syncControl = syncControl;
        }

        public void IssueReminder()
        {
            TimeSpan tolerance = TimeSpan.FromSeconds(1);

            int i = taskList.IndexOfGreaterThan(lastAdvancedReminder);
            if (i >= 0)
            {
                while (i < taskList.Count)
                {
                    MapNode node = taskList[i];
                    DateTime dueDate = node.GetDueDate();
                    if (dueDate - DateTime.Now < tolerance)
                    {
                        // task due
                        lastAdvancedReminder = dueDate;
                    }
                    else if (dueDate - DateTime.Now < TimeSpan.FromMinutes(ADVANCED_REMINDER_MINUTES))
                    {
                        // task due soon
                        lastAdvancedReminder = dueDate;
                        if (TaskDue != null) TaskDue(node, ReminderType.First);
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
            }

            i = taskList.IndexOfGreaterThan(lastReminder);
            if (i >= 0)
            {
                while (i < taskList.Count)
                {
                    MapNode node = taskList[i];
                    DateTime dueDate = node.GetDueDate();
                    if (dueDate - DateTime.Now < tolerance)
                    {
                        // task due
                        lastReminder = dueDate;
                        if (TaskDue != null) TaskDue(node, ReminderType.Final);
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
            }
        }

        //public void IssueReminder()
        //{
        //    TimeSpan tolerance = TimeSpan.FromSeconds(1);

        //    for (int i = 0; i < taskList.Count; i++)
        //    {
        //        MapNode node = taskList[i];
        //        DateTime dueDate = node.GetDueDate();
        //        switch (node.GetReminderIssued())
        //        {
        //            case ReminderType.Final:
        //                continue;
        //            case ReminderType.None:
        //                if (dueDate - DateTime.Now < tolerance)
        //                {
        //                    // task due
        //                    node.SetReminderIssued(ReminderType.Final);
        //                    if (TaskDue != null) TaskDue(node, ReminderType.Final);                        
        //                }
        //                else if (dueDate - DateTime.Now < TimeSpan.FromSeconds(ADVANCED_REMINDER_MINUTES))
        //                {
        //                    // task due soon
        //                    node.SetReminderIssued(ReminderType.First);
        //                    if (TaskDue != null) TaskDue(node, ReminderType.First);                            
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //                break;
        //            case ReminderType.First:
        //                if (dueDate - DateTime.Now < tolerance)
        //                {
        //                    // task due
        //                    node.SetReminderIssued(ReminderType.Final);
        //                    if (TaskDue != null) TaskDue(node, ReminderType.Final);
        //                }
        //                break;
        //        }

        //    }
        //}

        #region ITask interface
        public DateTime StartTime { get; set; }

        public string TaskId { get; set; }
        
        //public DateTime GetNextRunTime(DateTime lastExecutionTime)
        //{
        //    DateTime nextRunTime = DateTime.Now.AddDays(1);
        //    for (int i = 0; i < taskList.Count; i++)
        //    {
        //        MapNode node = taskList[i];
        //        DateTime dueDate = node.GetDueDate();
        //        switch(node.GetReminderIssued())
        //        {
        //            case ReminderType.Final:
        //                continue;
        //            case ReminderType.First:
        //                if(dueDate < nextRunTime)
        //                    nextRunTime = dueDate;
        //                break;
        //            case ReminderType.None:
        //                DateTime tempNext = dueDate.Subtract(TimeSpan.FromMinutes(ADVANCED_REMINDER_MINUTES));
        //                if (tempNext < nextRunTime) nextRunTime = tempNext;
        //                return nextRunTime; 
        //        }
        //    }
        //    return nextRunTime;
        //}

        public DateTime GetNextRunTime(DateTime lastExecutionTime)
        {
            DateTime nextRunTime = DateTime.Now.AddDays(1);

            int i = taskList.IndexOfGreaterThan(lastReminder);
            if (i >= 0) nextRunTime = taskList[i].GetDueDate();

            i = taskList.IndexOfGreaterThan(lastAdvancedReminder);
            if(i >= 0)
            {
                DateTime tempNext = taskList[i].GetDueDate().Subtract(TimeSpan.FromMinutes(ADVANCED_REMINDER_MINUTES));
                if (tempNext < nextRunTime) nextRunTime = tempNext;
            }

            return nextRunTime;
        }

        public void Run()
        {
            syncControl.Invoke((Action)IssueReminder);
        }
        #endregion ITask interface
    }
}
