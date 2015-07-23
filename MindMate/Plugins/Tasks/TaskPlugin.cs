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
        public PendingTaskList PendingTasks { get { return pendingTasks; } }

        private CompletedTaskList completedTasks;
        private DateTimePicker dateTimePicker;

        private TaskListView taskListView;
        public TaskListView TaskListView { get { return taskListView; } }

        private IPluginManager pluginManager;
        public IPluginManager PluginManager { get { return pluginManager; } }

        

        public void Initialize(IPluginManager pluginMgr)
        {
            this.pluginManager = pluginMgr;

            pendingTasks = new PendingTaskList();
            completedTasks = new CompletedTaskList();
            pendingTasks.TaskChanged += PendingTasks_TaskChanged;
            pendingTasks.TaskTextChanged += PendingTasks_TaskTextChanged;
            pendingTasks.TaskSelectionChanged += PendingTasks_TaskSelectionChanged;

            dateTimePicker = new DateTimePicker();
            taskListView = new TaskListView();
            taskListView.TaskViewEvent += OnTaskViewEvent;

            pluginMgr.ScheduleTask(new TaskSchedular.RecurringTask(
                () =>
                {
                    taskListView.Invoke((Action)RefreshTaskListView);
                },
                DateTime.Today.AddDays(1),
                TimeSpan.FromDays(1)
                )
            );                        
        }

        public void OnApplicationReady()
        {
            new Reminder.ReminderCtrl(this);
        }
                                               
        public void CreateMainMenuItems(out MenuItem[] menuItems, out MainMenuLocation position)
        {
            throw new NotImplementedException();
        }

        public Control[] CreateSideBarWindows()
        {
            taskListView.Text = "Tasks";
            return new Control [] { taskListView };
        }

        public void OnCreatingTree(MapTree tree)
        {
            pendingTasks.RegisterMap(tree);
            completedTasks.RegisterMap(tree);

            tree.AttributeChanged += Task.OnAttributeChanged;
        }

        public void OnDeletingTree(MapTree tree)
        {
            pendingTasks.UnregisterMap(tree);
            completedTasks.UnregisterMap(tree);

            tree.AttributeChanged += Task.OnAttributeChanged;
        }        

        public IEnumerable<MapNode> GetTasks()
        {
            foreach(MapNode node in pendingTasks)
            {
                yield return node;
            }
            foreach(MapNode node in completedTasks)
            {
                yield return node;
            }
        }
    }
}
