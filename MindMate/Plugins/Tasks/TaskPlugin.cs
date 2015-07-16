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
        private TaskListView taskListView;
        private IPluginManager pluginManager;

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
            taskListView.TaskViewEvent += taskList_TaskViewEvent;

            pluginMgr.ScheduleTask(new TaskSchedular.Task()
            {
                StartTime = DateTime.Today.AddDays(1),
                TaskAction = () =>
                {
                    taskListView.Invoke((Action)taskListView.RefreshTaskList);
                },
                Recurrance = TimeSpan.FromDays(1)
            });
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
    }
}
