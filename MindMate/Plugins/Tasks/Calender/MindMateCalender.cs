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

namespace MindMate.Plugins.Tasks.Calender
{
    public partial class MindMateCalendar : Form
    {
        TaskPlugin taskPlugin;

        List<CalendarItem> _items = new List<CalendarItem>();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (MapNode node in taskPlugin.AllTasks)
            {
                CalendarItem cal = new CalendarItem(calendar1, node.GetStartDate(), node.GetEndDate(), node.Text);
                cal.Tag = node;
                _items.Add(cal);
            }

            PlaceItems();
        }

        private void calendar1_LoadItems(object sender, CalendarLoadEventArgs e)
        {
            PlaceItems();
        }

        private void PlaceItems()
        {
            foreach (CalendarItem item in _items)
            {
                if (calendar1.ViewIntersects(item))
                {
                    calendar1.Items.Add(item);
                }
            }
        }
        private void DemoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void calendar1_DayHeaderClick(object sender, CalendarDayEventArgs e)
        {
            calendar1.SetViewRange(e.CalendarDay.Date, e.CalendarDay.Date);
        }

        private void monthView1_SelectionChanged(object sender, EventArgs e)
        {
            calendar1.SetViewRange(monthView1.SelectionStart, monthView1.SelectionEnd);
        }

        private void DeleteCalenderItem(CalendarItem item)
        {
            _items.Remove(item);
            ((MapNode)item.Tag).RemoveTask();
        }

        #region Calendar Item Events

        private void calendar1_ItemCreated(object sender, CalendarItemCancelEventArgs e)
        {
            _items.Add(e.Item);
        }

        private void calendar1_ItemDatesChanged(object sender, CalendarItemEventArgs e)
        {
            MapNode node = (MapNode)e.Item.Tag;

            if (node.GetStartDate() != e.Item.StartDate)
                node.SetStartDate(e.Item.StartDate);

            if (!node.GetEndDate().Equals(e.Item.EndDate))
                node.SetEndDate(e.Item.EndDate);
        }

        private void calendar1_ItemTextEdited(object sender, CalendarItemCancelEventArgs e)
        {
            ((MapNode)e.Item.Tag).Text = e.Item.Text;
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
            DeleteCalenderItem(e.Item);
        }


        #endregion Calendar Events

        #region Context Menu

        private CalendarContextMenu calendarContextMenu;
        public new CalendarContextMenu ContextMenuStrip
        {
            get
            {
                return calendarContextMenu;
            }
            private set
            {
                calendarContextMenu = value;
                ((Control)this).ContextMenuStrip = ContextMenuStrip;
            }
        }

        CalendarItem contextItem = null;

        private void BuildContextMenu()
        {
            ContextMenuStrip = new CalendarContextMenu();            

            ContextMenuStrip.Opening += contextMenuStrip1_Opening;

            ContextMenuStrip.MenuHourTImescale.Click += new System.EventHandler(this.hourToolStripMenuItem_Click);
            ContextMenuStrip.Menu30MinsTimeScale.Click += new System.EventHandler(this.minutesToolStripMenuItem_Click);
            ContextMenuStrip.Menu15MinsTimescale.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            ContextMenuStrip.Menu10MinsTimescale.Click += new System.EventHandler(this.minutesToolStripMenuItem1_Click);
            ContextMenuStrip.Menu6MinsTimescale.Click += new System.EventHandler(this.minutesToolStripMenuItem2_Click);
            ContextMenuStrip.Menu5MinsTimeScale.Click += new System.EventHandler(this.minutesToolStripMenuItem3_Click);

            ContextMenuStrip.MenuEditText.Click += new System.EventHandler(this.editItemToolStripMenuItem_Click);
            ContextMenuStrip.MenuRemoveTask.Click += MenuRemoveTask_Click;
            ContextMenuStrip.MenuEditDueDate.Click += MenuEditDueDate_Click;

        }

        private void MenuEditDueDate_Click(object sender, EventArgs e)
        {
            
        }

        private void MenuRemoveTask_Click(object sender, EventArgs e)
        {
            DeleteCalenderItem(contextItem);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextItem = calendar1.ItemAt(ContextMenuStrip.Bounds.Location);
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