using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Calendar;
using System.Xml.Serialization;
using System.IO;
using MindMate.Plugins.Tasks.Model;
using MindMate.Plugins.Tasks;
using MindMate.Model;
using System.Linq;

namespace MindMate.Plugins.Tasks.Calender
{
    public partial class MindMateCalendar : Form
    {
        TaskPlugin taskPlugin;

        /// <summary>
        /// Flag is used to avoid refreshing the Calendar from task list when it is editing data internal
        /// </summary>
        public bool SuspendRefreshFromTaskList { get; private set; }

        public MindMateCalendar(TaskPlugin taskPlugin)
        {

            InitializeComponent();

            this.taskPlugin = taskPlugin;

            //Monthview colors
            monthView1.MonthTitleColor = monthView1.MonthTitleColorInactive = CalendarColorTable.FromHex("#C2DAFC");
            monthView1.ArrowsColor = CalendarColorTable.FromHex("#77A1D3");
            monthView1.DaySelectedBackgroundColor = CalendarColorTable.FromHex("#F4CC52");
            monthView1.DaySelectedTextColor = monthView1.ForeColor;

            BuildContextMenu();

        }

        private void Form_Load(object sender, EventArgs e)
        {
            InitializeCalenderFromTaskList();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectCalenderFromTaskList();
        }

        #region Refresh Calendar from Task List

        /// <summary>
        /// Called when the form is loaded
        /// </summary>
        private void InitializeCalenderFromTaskList()
        {
            PlaceItems();

            taskPlugin.AllTasks.TaskChanged += AllTasks_TaskChanged;
            taskPlugin.AllTasks.TaskTextChanged += AllTasks_TaskTextChanged;
        }

        /// <summary>
        /// Called whent the form is closed
        /// </summary>
        private void DisconnectCalenderFromTaskList()
        {
            taskPlugin.AllTasks.TaskChanged -= AllTasks_TaskChanged;
            taskPlugin.AllTasks.TaskTextChanged -= AllTasks_TaskTextChanged;
        }

        private void AllTasks_TaskTextChanged(MapNode node, TaskTextEventArgs e)
        {
            RefreshCalenderFromTaskList();
        }

        private void AllTasks_TaskChanged(MapNode node, TaskList.TaskChangeEventArgs args)
        {
            RefreshCalenderFromTaskList();
        }

        /// <summary>
        /// Clear calendar items and reload them from Task List
        /// </summary>
        private void RefreshCalenderFromTaskList()
        {
            if (!SuspendRefreshFromTaskList)
            {
                calendar1.Items.Clear();
                PlaceItems();
            }
        }

        /// <summary>
        /// Load task list to Calendar's Items collection
        /// </summary>
        private void PlaceItems()
        {
            foreach (MapNode node in taskPlugin.AllTasks.GetTasksBetween(calendar1.ViewStart, calendar1.ViewEnd))
            {
                CalendarItem item = new CalendarItem(calendar1, node.GetStartDate(), node.GetEndDate(), node.Text);
                item.Tag = node;
                if (node.IsTaskComplete())
                {
                    MarkComplete(item);
                }

                calendar1.Items.Add(item);
            }
        }

        private void MarkComplete(CalendarItem item)
        {
            item.Image = TaskRes.tick;
            item.ShowTime = false;
        }

        #endregion Refresh Calendar from Task List

        #region Calendar Events

        private void calendar1_LoadItems(object sender, CalendarLoadEventArgs e)
        {
            PlaceItems();
        }

        private void calendar1_DayHeaderClick(object sender, CalendarDayEventArgs e)
        {
            calendar1.SetViewRange(e.CalendarDay.Date, e.CalendarDay.Date);
        }

        private void monthView1_SelectionChanged(object sender, EventArgs e)
        {
            calendar1.SetViewRange(monthView1.SelectionStart, monthView1.SelectionEnd);
        }

        #endregion Calendar Events

        #region Calendar Item Events

        private void calendar1_ItemCreated(object sender, CalendarItemCancelEventArgs e)
        {
            SuspendRefreshFromTaskList = true;
            bool success = taskPlugin.AddSubTask(e.Item.Text, e.Item.StartDate, e.Item.EndDate);
            if (!success)
            {
                e.Cancel = true;
            }
            SuspendRefreshFromTaskList = false;
        }

        private void calendar1_ItemDatesChanged(object sender, CalendarItemEventArgs e)
        {
            SuspendRefreshFromTaskList = true;

            MapNode node = (MapNode)e.Item.Tag;

            if (node.GetStartDate() != e.Item.StartDate)
                node.SetStartDate(e.Item.StartDate);

            if (!node.GetEndDate().Equals(e.Item.EndDate))
                node.SetEndDate(e.Item.EndDate);

            SuspendRefreshFromTaskList = false;
        }

        private void calendar1_ItemTextEdited(object sender, CalendarItemCancelEventArgs e)
        {
            SuspendRefreshFromTaskList = true;
            ((MapNode)e.Item.Tag).Text = e.Item.Text;
            SuspendRefreshFromTaskList = false;
        }

        private void calendar1_ItemClick(object sender, CalendarItemEventArgs e)
        {
            //MessageBox.Show(e.Item.Text);
        }

        private void calendar1_ItemDoubleClick(object sender, CalendarItemEventArgs e)
        {
            //MessageBox.Show("Double click: " + e.Item.Text);
        }

        private void calendar1_ItemDeleted(object sender, CalendarItemEventArgs e)
        {
            SuspendRefreshFromTaskList = true;
            ((MapNode)e.Item.Tag).RemoveTask();
            SuspendRefreshFromTaskList = false;
        }


        #endregion Calendar Events

        #region Context Menu

        //private CalendarContextMenu calendarContextMenu;
        //public new CalendarContextMenu ContextMenuStrip
        //{
        //    get
        //    {
        //        return calendarContextMenu;
        //    }
        //    private set
        //    {
        //        calendarContextMenu = value;
        //        ((Control)this).ContextMenuStrip = ContextMenuStrip;
        //    }
        //}

        CalendarItem contextItem = null;

        private void BuildContextMenu()
        {
            CalendarContextMenu menu = new CalendarContextMenu();

            menu.Opening += contextMenuStrip1_Opening;

            menu.MenuHourTImescale.Click += new System.EventHandler(this.hourToolStripMenuItem_Click);
            menu.Menu30MinsTimeScale.Click += new System.EventHandler(this.minutesToolStripMenuItem_Click);
            menu.Menu15MinsTimescale.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            menu.Menu10MinsTimescale.Click += new System.EventHandler(this.minutesToolStripMenuItem1_Click);
            menu.Menu6MinsTimescale.Click += new System.EventHandler(this.minutesToolStripMenuItem2_Click);
            menu.Menu5MinsTimeScale.Click += new System.EventHandler(this.minutesToolStripMenuItem3_Click);
            
            menu.MenuEditText.Click += new System.EventHandler(this.editItemToolStripMenuItem_Click);
            menu.MenuRemoveTask.Click += MenuRemoveTask_Click;
            menu.MenuCompleteTask.Click += MenuCompleteTask_Click;
            menu.MenuEditDueDate.Click += MenuEditDueDate_Click;
            menu.MenuDueDateToday.Click += MenuDueDateToday_Click;
            menu.MenuDueDateTomorrow.Click += MenuDueDateTomorrow_Click;
            menu.MenuDueDateNextWeek.Click += MenuDueDateNextWeek_Click;
            menu.MenuDueDateNextMonth.Click += MenuDueDateNextMonth_Click;
            menu.MenuDueDateNextQuarter.Click += MenuDueDateNextQuarter_Click;

            ContextMenuStrip = menu;

        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextItem = calendar1.ItemAt(calendar1.PointToClient(ContextMenuStrip.Bounds.Location));

            
            if (contextItem != null)
            {
                SetTimeScaleMenuVisibility(false);
                SetCalendarItemMenuVisibility(true);                
            }
            else
            {
                SetTimeScaleMenuVisibility(true);
                SetCalendarItemMenuVisibility(false);
            }
        }

        private void SetCalendarItemMenuVisibility(bool value)
        {
            CalendarContextMenu menu = (CalendarContextMenu)ContextMenuStrip;
            menu.MenuRemoveTask.Available = value;
            menu.MenuEditText.Available = value;
            menu.MenuEditDueDate.Available = value;
            menu.MenuQuickDates.Available = value;
            //menu.MenuDueDateTomorrow.Available = value;
            //menu.MenuDueDateToday.Available = value;
            //menu.MenuDueDateNextWeek.Available = value;
            //menu.MenuDueDateNextQuarter.Available = value;
            //menu.MenuDueDateNextMonth.Available = value;
            menu.MenuCompleteTask.Available = value;
        }

        private void SetTimeScaleMenuVisibility(bool value)
        {
            CalendarContextMenu menu = (CalendarContextMenu)ContextMenuStrip;
            menu.MenuTimescale.Available = value;
        }

        private void MenuDueDateNextQuarter_Click(object sender, EventArgs e)
        {
            SetDateForSelected(n => taskPlugin.SetDueDateNextQuarter(n));
        }

        private void MenuDueDateNextMonth_Click(object sender, EventArgs e)
        {
            SetDateForSelected(n => taskPlugin.SetDueDateNextMonth(n));
        }

        private void MenuDueDateNextWeek_Click(object sender, EventArgs e)
        {
            SetDateForSelected(n => taskPlugin.SetDueDateNextWeek(n));
        }

        private void MenuDueDateTomorrow_Click(object sender, EventArgs e)
        {
            SetDateForSelected(n => taskPlugin.SetDueDateTomorrow(n));
        }

        private void MenuDueDateToday_Click(object sender, EventArgs e)
        {
            SetDateForSelected(n => taskPlugin.SetDueDateToday(n));
        }

        private void MenuEditDueDate_Click(object sender, EventArgs e)
        {
            MapNode firstNode = ((MapNode)calendar1.GetSelectedItems().First().Tag);
            DateTime dueDate = taskPlugin.ShowDueDatePicker(firstNode.GetEndDate());

            if (dueDate != DateTime.MinValue)
            {
                SetDateForSelected(n => n.AddTask(dueDate));
            }           
        }

        /// <summary>
        /// Executes the action for all selected MapNode(s) and updates their CalenderItem(s)
        /// </summary>
        /// <param name="SetDate">action for all selected MapNode(s)</param>
        private void SetDateForSelected(Action<MapNode> SetDate)
        {
            SuspendRefreshFromTaskList = true;

            foreach (CalendarItem item in calendar1.GetSelectedItems())
            {
                MapNode node = (MapNode)item.Tag;
                SetDate(node);
                item.StartDate = node.GetStartDate();
                item.EndDate = node.GetEndDate();
                if(!node.IsTaskComplete()) { item.Image = null; }
            }

            calendar1.Renderer.PerformItemsLayout();
            calendar1.Invalidate();

            SuspendRefreshFromTaskList = false;
        }

        private void MenuCompleteTask_Click(object sender, EventArgs e)
        {
            SuspendRefreshFromTaskList = true;

            foreach (CalendarItem item in calendar1.GetSelectedItems())
            {
                MapNode node = (MapNode)item.Tag;
                node.CompleteTask();
                MarkComplete(item);
            }

            calendar1.Renderer.PerformItemsLayout();
            calendar1.Invalidate();

            SuspendRefreshFromTaskList = false;
        }

        private void MenuRemoveTask_Click(object sender, EventArgs e)
        {
            SuspendRefreshFromTaskList = true;

            foreach (CalendarItem item in calendar1.GetSelectedItems())
            {
                calendar1.Items.Remove(item);
                ((MapNode)item.Tag).RemoveTask();
            }

            SuspendRefreshFromTaskList = false;
        }

        private void minutesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.SixMinutes;
        }

        private void hourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.SixtyMinutes;
        }

        private void minutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.ThirtyMinutes;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.FifteenMinutes;
        }

        private void minutesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.TenMinutes;
        }

        private void minutesToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            calendar1.TimeScale = CalendarTimeScale.FiveMinutes;
        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendar1.ActivateEditMode();
        }


        #endregion Context Menu
    }
}