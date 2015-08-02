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
            PlaceItems();
        }

        private void calendar1_LoadItems(object sender, CalendarLoadEventArgs e)
        {
            PlaceItems();
        }

        private void PlaceItems()
        {
            foreach (MapNode node in taskPlugin.AllTasks.GetTasksBetween(calendar1.ViewStart, calendar1.ViewEnd))
            {
                CalendarItem item = new CalendarItem(calendar1, node.GetStartDate(), node.GetEndDate(), node.Text);
                item.Tag = node;
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
                
        #region Calendar Item Events

        private void calendar1_ItemCreated(object sender, CalendarItemCancelEventArgs e)
        {
            //_items.Add(e.Item);
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
            ((MapNode)e.Item.Tag).RemoveTask(); 
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
            menu.MenuEditDueDate.Click += MenuEditDueDate_Click;

            ContextMenuStrip = menu;

        }

        private void MenuEditDueDate_Click(object sender, EventArgs e)
        {
            
        }

        private void MenuRemoveTask_Click(object sender, EventArgs e)
        {
            foreach (CalendarItem item in calendar1.GetSelectedItems())
            {
                calendar1.Items.Remove(item);
                ((MapNode)item.Tag).RemoveTask();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextItem = calendar1.ItemAt(calendar1.PointToClient(ContextMenuStrip.Bounds.Location));
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