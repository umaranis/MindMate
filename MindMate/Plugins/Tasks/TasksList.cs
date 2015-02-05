using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public partial class TasksList : UserControl
    {
        public TasksList()
        {
            InitializeComponent();
        }        

        public void Add(MindMate.Model.MapNode node, DateTime dateTime)
        {
            if(dateTime.Date == DateTime.Today)
            {
                AddTask(this.collapsiblePanelToday, this.tableLayoutToday, node, dateTime);
            }              
            else if(dateTime.Date == (DateTime.Today.AddDays(1).Date))
            {
                AddTask(this.collapsiblePanelTomorrow, this.tableLayoutTomorrow, node, dateTime);
            }
            else if(DateInThisWeek(dateTime))
            {
                AddTask(this.collapsiblePanelThisWeek, this.tableLayoutThisWeek, node, dateTime);
            }
            else if(DateInThisMonth(dateTime))
            {
                AddTask(this.collapsiblePanelThisMonth, this.tableLayoutThisMonth, node, dateTime);
            }
            else if(DateInNextMonth(dateTime))
            {
                AddTask(this.collapsiblePanelNextMonth, this.tableLayoutNextMonth, node, dateTime);
            }

            Control lastTaskGroup = GetLastTaskGroup();
            this.tablePanelMain.Height = lastTaskGroup.Location.Y + lastTaskGroup.Size.Height + lastTaskGroup.Margin.Bottom;
        }

        
        private void AddTask(MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanel, TableLayoutPanel tableLayout,
            MindMate.Model.MapNode node, DateTime dateTime)
        {
            if (tableLayout.RowCount == 0) collapsiblePanel.Visible = true;
            TaskView tv = new TaskView(node, dateTime, "");
            tv.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            tableLayout.Controls.Add(tv, 0, tableLayout.RowCount);
            tableLayout.RowCount += 1;
            tableLayout.Height = tableLayout.RowCount * tv.Height + (tableLayout.Margin.Bottom * tableLayout.RowCount * 2);
            collapsiblePanel.Height = tableLayout.Height + tableLayout.Top;
        }

        
        private int GetTaskListSize()
        {
            int size = 0;

            for(int i = 0; i < tablePanelMain.RowCount; i++)
            {

            }

            return size;
        }

        private bool DateInThisWeek(DateTime dateTime)
        {
            DateTime beginning, end;
            GetWeek(DateTime.Now, System.Globalization.CultureInfo.CurrentCulture, out beginning, out end);
            return dateTime.Date <= end.Date;
        }

        private void GetWeek(DateTime now, System.Globalization.CultureInfo cultureInfo, out DateTime begining, out DateTime end)
        {
            if (now == null)
                throw new ArgumentNullException("now");
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            int offset = firstDayOfWeek - now.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                begining = weekStart;
                end = endOfWeek;
            }
            else
            {
                begining = now.AddDays(-6);
                end = now;
            }
        }

        private bool DateInThisMonth(DateTime dateTime)
        {
            return (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month);
        }

        private bool DateInNextMonth(DateTime dateTime)
        {
            DateTime currentNextMonth = DateTime.Now.AddMonths(1);
            return dateTime.Month == currentNextMonth.Month && dateTime.Year == currentNextMonth.Year;
        }

        private Control GetLastTaskGroup()
        {
            for(int i = this.tablePanelMain.RowCount - 1; i >= 0; i--)
            {
                Control ctrl = this.tablePanelMain.GetControlFromPosition(0, i);
                if (ctrl != null)
                    return ctrl;
            }

            return null;
        }
        
    }
}
