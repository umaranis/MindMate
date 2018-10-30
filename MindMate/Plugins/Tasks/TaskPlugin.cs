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
    public partial class TaskPlugin : IPlugin, IPluginMainMenu
    {

        private PendingTaskList pendingTasks;
        public PendingTaskList PendingTasks { get { return pendingTasks; } }

        private CompletedTaskList completedTasks;

        /// <summary>
        /// List of all tasks (completed + pending)
        /// </summary>
        public TaskList AllTasks { get; private set; }

        private DateTimePicker dateTimePicker;

        private TaskListView taskListView;
        public TaskListView TaskListView { get { return taskListView; } }

        public IPluginManager PluginManager { get; private set; }


        public void Initialize(IPluginManager pluginMgr)
        {
            this.PluginManager = pluginMgr;

            pendingTasks = new PendingTaskList();
            completedTasks = new CompletedTaskList();
            AllTasks = new TaskList(pendingTasks, completedTasks);
            pendingTasks.TaskChanged += PendingTasks_TaskChanged;
            pendingTasks.TaskTextChanged += PendingTasks_TaskTextChanged;
            pendingTasks.TaskSelectionChanged += PendingTasks_TaskSelectionChanged;

            dateTimePicker = new DateTimePicker();
            taskListView = new TaskListView();
            taskListView.TaskViewEvent += OnTaskViewEvent;

            pluginMgr.ScheduleTask(new TaskScheduler.RecurringTask(
                () =>
                {
                    taskListView.Invoke((Action)RefreshTaskListView);
                },
                DateTime.Today.AddDays(1),
                TimeSpan.FromDays(1),
                "TaskListRefreshNewDay"
                )
            );                        
        }

        public void OnApplicationReady()
        {
            new Reminder.ReminderCtrl(this);
        }
                                               
        public MainMenuItem[] CreateMainMenuItems()
        {
            var mTasks = new MainMenuItem("Tasks");
            mTasks.MainMenuLocation = MainMenuLocation.Separate;

            var mAddTask = new MainMenuItem("Add Task ...");
            mAddTask.Click += (sender, args) => SetDueDateUsingPicker();
            mAddTask.AddSeparator = true;
            mTasks.AddDropDownItem(mAddTask);

            var mAddTaskDueToday = new MainMenuItem("Due Today");
            mAddTaskDueToday.Click += (sender, args) => SetDueDateToday();
            mTasks.AddDropDownItem(mAddTaskDueToday);

            var mAddTaskDueTomorrow = new MainMenuItem("Tomorrow");
            mAddTaskDueTomorrow.Click += (sender, args) => SetDueDateTomorrow();
            mTasks.AddDropDownItem(mAddTaskDueTomorrow);

            var mAddTaskDueNextWeek = new MainMenuItem("Next Week");
            mAddTaskDueNextWeek.Click += (sender, args) => SetDueDateNextWeek();
            mTasks.AddDropDownItem(mAddTaskDueNextWeek);

            var mAddTaskDueNextMonth = new MainMenuItem("Next Month");
            mAddTaskDueNextMonth.Click += (sender, args) => SetDueDateNextMonth();
            mTasks.AddDropDownItem(mAddTaskDueNextMonth);

            var mAddTaskDueNextQuarter = new MainMenuItem("Next Quarter");
            mAddTaskDueNextQuarter.Click += (sender, args) => SetDueDateNextQuarter();
            mAddTaskDueNextQuarter.AddSeparator = true;
            mTasks.AddDropDownItem(mAddTaskDueNextQuarter);

            var mCompleteTask = new MainMenuItem("Complete Task");
            mCompleteTask.Click += (sender, args) => CompleteTask();
            mCompleteTask.AddSeparator = true;
            mTasks.AddDropDownItem(mCompleteTask);

            var mRemoveTask = new MainMenuItem("Remove Task");
            mRemoveTask.Click += (sender, args) => RemoveTask();
            mRemoveTask.AddSeparator = true;
            mTasks.AddDropDownItem(mRemoveTask);

            var mCalendar = new MainMenuItem("View Calendar");
            mCalendar.Click = OnCalendarMenuClick;
            mTasks.AddDropDownItem(mCalendar);

            return new MainMenuItem[] { mTasks };            
        }

        private void OnCalendarMenuClick(object senrder, EventArgs e)
        {
            OpenCalender();
        }

        public Calender.MindMateCalendar OpenCalender()
        {
            Calender.MindMateCalendar frmCalendar = new Calender.MindMateCalendar(this);
            frmCalendar.Show();
            return frmCalendar;
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
            AllTasks.RegisterMap(tree);

            tree.AttributeChanged += Task.OnAttributeChanged;
        }

        public void OnDeletingTree(MapTree tree)
        {
            pendingTasks.UnregisterMap(tree);
            completedTasks.UnregisterMap(tree);
            AllTasks.UnregisterMap(tree);

            tree.AttributeChanged += Task.OnAttributeChanged;
        }

        /// <summary>
        /// Returns DateTime.MinValue if nothing is selected
        /// </summary>
        /// <returns>Returns DateTime.MinValue if nothing is selected</returns>
        public DateTime ShowDueDatePicker()
        {
            return ShowDueDatePicker(DateHelper.GetDefaultDueDate());
        }

        /// <summary>
        /// Returns DateTime.MinValue if nothing is selected
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns>Returns DateTime.MinValue if nothing is selected</returns>
        public DateTime ShowDueDatePicker(DateTime defaultValue)
        {
            dateTimePicker.Value = defaultValue;
            if (dateTimePicker.ShowDialog() == DialogResult.OK)
                return dateTimePicker.Value;
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Sets the date component of DueDate. Time component is set if it is empty, otherwise left unchanged.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate"></param>
        private void SetDueDateKeepTimePart(MapNode node, DateTime dueDate)
        {
            if (node.DueDateExists())
                dueDate = dueDate.Date.Add(node.GetDueDate().TimeOfDay);
            node.AddTask(dueDate);
        }

        /// <summary>
        /// Create a child MapNode and adds task to it
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public bool AddSubTask(string text, DateTime startDate, DateTime endDate)
        {
            if(PluginManager.ActiveNodes.Count == 1)
            {
                MapNode node = new MapNode(PluginManager.ActiveNodes.First, text);
                node.SetStartDate(startDate);
                node.SetEndDate(endDate);
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Command

        /// <summary>
        /// Should only update the model, all interested views should be updated through the event generated by the model.
        /// </summary>
        /// <param name="node"></param>
        private void SetDueDateUsingPicker(MapNode node)
        {
            // initialize date time picker
            if (node.DueDateExists())
            {
                dateTimePicker.Value = node.GetDueDate();
            }
            else
            {
                dateTimePicker.Value = DateHelper.GetDefaultDueDate();
            }

            // show and set due dates
            if (dateTimePicker.ShowDialog() == DialogResult.OK)
            {
                node.AddTask(dateTimePicker.Value);
            }
        }

        /// <summary>
        /// Set Due Date for selected nodes of the active MapTree using DateTimePicker
        /// Should only update the model, all interested views should be updated through the event generated by the model.
        /// </summary>
        public void SetDueDateUsingPicker()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Add Task Due Date"))
            {

                DateTime value;

                // initialize date time picker
                MapNode temp = PluginManager.ActiveNodes.First();
                if (temp != null && temp.DueDateExists())
                {
                    value = ShowDueDatePicker(temp.GetDueDate());
                }
                else
                {
                    value = ShowDueDatePicker();
                }

                // show and set due dates
                if (value != DateTime.MinValue)
                {
                    foreach (MapNode node in PluginManager.ActiveNodes)
                    {
                        node.AddTask(value);
                    }
                }
            }
        }

        public void SetDueDateToday(MapNode node)
        {
            SetDueDateKeepTimePart(node, DateHelper.GetDefaultDueDateToday());
        }

        /// <summary>
        /// Set Due Date as Today for selected nodes of the active MapTree.
        /// </summary>
        public void SetDueDateToday()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Add Task Due Today"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    SetDueDateToday(node);
                }
            }
        }

        public void SetDueDateTomorrow(MapNode node)
        {
            SetDueDateKeepTimePart(node, DateHelper.GetDefaultDueDateTomorrow());
        }

        /// <summary>
        /// Set Due Date as Tomorrow for selected nodes of the active MapTree.
        /// </summary>
        public void SetDueDateTomorrow()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Add Task Due Tomorrow"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    SetDueDateTomorrow(node);
                }
            }
        }

        public void SetDueDateNextWeek(MapNode node)
        {
            SetDueDateKeepTimePart(node, DateHelper.GetDefaultDueDateNextWeek());
        }

        /// <summary>
        /// Set Due Date as NextWeek for selected nodes of the active MapTree.
        /// </summary>
        public void SetDueDateNextWeek()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Add Task Due Next Week"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    SetDueDateNextWeek(node);
                }
            }
        }

        public void SetDueDateNextMonth(MapNode node)
        {
            SetDueDateKeepTimePart(node, DateHelper.GetDefaultDueDateNextMonth());
        }

        /// <summary>
        /// Set Due Date as NextMonth for selected nodes of the active MapTree.
        /// </summary>
        public void SetDueDateNextMonth()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Add Task Due Next Month"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    SetDueDateNextMonth(node);
                }
            }
        }

        public void SetDueDateNextQuarter(MapNode node)
        {
            SetDueDateKeepTimePart(node, DateHelper.GetDefaultDueDateNextQuarter());
        }

        /// <summary>
        /// Set Due Date as NextQuarter for selected nodes of the active MapTree.
        /// </summary>
        public void SetDueDateNextQuarter()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Add task due next quarter"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    SetDueDateNextQuarter(node);
                }
            }
        }

        /// <summary>
        /// Mark Task as Complete for selected nodes of the active MapTree.
        /// </summary>
        public void CompleteTask()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Mark task(s) as complete"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    node.CompleteTask();
                }
            }
        }

        /// <summary>
        /// Remove Task for selected nodes of the active MapTree.
        /// </summary>
        public void RemoveTask()
        {
            if (PluginManager.ActiveNodes.IsEmpty) return;

            //using (PluginManager.ActiveTree.ChangeManager.StartBatch("Remove task(s)"))
            {
                foreach (var node in PluginManager.ActiveNodes)
                {
                    node.RemoveTask();
                }
            }
        }

        #endregion
    }
}
