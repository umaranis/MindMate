using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks.Reminder
{
    public class ReminderCtrl
    {
        private TaskPlugin taskPlugin;

        private TaskReminder reminder;
        public TaskReminder TaskReminder { get { return reminder; } }

        private List<TaskReminderDialog> openDialogs;

        public ReminderCtrl(TaskPlugin taskPlugin)
        {
            this.taskPlugin = taskPlugin;
            openDialogs = new List<TaskReminderDialog>();
            reminder = new TaskReminder(taskPlugin.PendingTasks, taskPlugin.TaskListView);
            reminder.TaskDue += Reminder_TaskDue;
            taskPlugin.PluginManager.ScheduleTask(reminder);
            taskPlugin.PendingTasks.TaskChanged += PendingTasks_TaskChanged;
            taskPlugin.PendingTasks.TaskSelectionChanged += PendingTasks_TaskSelectionChanged;
            taskPlugin.PendingTasks.TaskTextChanged += PendingTasks_TaskTextChanged;       
        }

        private void Reminder_TaskDue(MindMate.Model.MapNode node, ReminderType reminderType)
        {
            if(reminderType == ReminderType.Final)
            {
                TaskReminderDialog dialog = GetOpenDialog(node);
                if (dialog != null)
                    dialog.Close();
            }

            TaskReminderDialog dlg = new TaskReminderDialog(node, reminderType);
            dlg.Left = Screen.PrimaryScreen.Bounds.Width - 50 - dlg.Width;
            dlg.Top = Screen.PrimaryScreen.Bounds.Height - 100 - dlg.Height;
            dlg.TaskViewEvent += taskPlugin.OnTaskViewEvent;
            openDialogs.Add(dlg);
            dlg.FormClosing += Dlg_FormClosing;
            dlg.Show();  
        }

        private void Dlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            openDialogs.Remove((TaskReminderDialog)sender);
            ((TaskReminderDialog)sender).FormClosing -= Dlg_FormClosing;
        }

        private TaskReminderDialog GetOpenDialog(MapNode node)
        {
            return openDialogs.Find(d => d.TaskView.MapNode == node);
        }

        private void PendingTasks_TaskChanged(MindMate.Model.MapNode node, Model.PendingTaskEventArgs args)
        {
            taskPlugin.PluginManager.RescheduleTask(reminder, DateTime.Now);

            TaskReminderDialog dialog = GetOpenDialog(node);
            if (dialog == null) return;
            dialog.Close();
        }

        private void PendingTasks_TaskSelectionChanged(MapNode node, TaskSelectionEventArgs e)
        {
            TaskReminderDialog dialog = GetOpenDialog(node);
            if (dialog == null) return;

            if (e.ChangeType == TaskSelectionChange.Selected)
                dialog.TaskView.Selected = true;
            else
                dialog.TaskView.Selected = false;
        }

        private void PendingTasks_TaskTextChanged(MapNode node, TaskTextEventArgs e)
        {
            TaskReminderDialog dialog = GetOpenDialog(node);
            if (dialog == null) return;

            if (e.ChangeType == TaskTextChange.TextChange)
                dialog.TaskView.TaskTitle = node.Text;
            else if (e.ChangeType == TaskTextChange.AncestorTextChange)
                dialog.TaskView.RefreshTaskPath();
        }
    }
}
