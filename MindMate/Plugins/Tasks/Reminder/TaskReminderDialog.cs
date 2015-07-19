using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks.Reminder
{
    public partial class TaskReminderDialog : Form
    {
        /// <summary>
        /// Popup for task due alert
        /// </summary>
        /// <param name="node"></param>
        /// <param name="reminderType">should never be None. Acceptable values are First and Final.</param>
        public TaskReminderDialog(MapNode node, ReminderType reminderType)
        {
            InitializeComponent();

            TaskView tv = new TaskView(node, DateHelper.GetTimePartString(node.GetDueDate()), onTaskViewEvent, false);
            tv.Width = 300;
            Text = "Task Due " + (reminderType == ReminderType.First ? "in " + (node.GetDueDate() - DateTime.Now).Minutes + " mins" : "now");
            ClientSize = new Size(tv.Width, tv.Height);            
            Controls.Add(tv);

            FormClosing += TaskReminderDialog_FormClosing;
        }

        private void TaskReminderDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            TaskViewEvent = null;
        }

        public TaskView TaskView { get { return (TaskView)Controls[0]; } }
                

        private void onTaskViewEvent(TaskView taskView, TaskView.TaskViewEvent e)
        {
            if (TaskViewEvent != null)
                TaskViewEvent(taskView, e);            
        }

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;
        
    }
}
