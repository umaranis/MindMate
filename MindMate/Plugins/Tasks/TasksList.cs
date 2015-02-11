using MindMate.Model;
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

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;

        public void Add(MindMate.Model.MapNode node, DateTime dateTime)
        {
            if(DateHelper.IsToday(dateTime))
            {
                AddTask(this.collapsiblePanelToday, this.tableLayoutToday, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }              
            else if(DateHelper.IsTomorrow(dateTime))
            {
                AddTask(this.collapsiblePanelTomorrow, this.tableLayoutTomorrow, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }
            else if(DateHelper.DateInThisWeek(dateTime))
            {
                AddTask(this.collapsiblePanelThisWeek, this.tableLayoutThisWeek, node, dateTime,
                    DateHelper.GetWeekDayString(dateTime));
            }
            else if(DateHelper.DateInThisMonth(dateTime))
            {
                AddTask(this.collapsiblePanelThisMonth, this.tableLayoutThisMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if (DateHelper.DateInNextMonth(dateTime))
            {
                AddTask(this.collapsiblePanelNextMonth, this.tableLayoutNextMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }

            AdjustMainPanelHeight();
        }

        
        private void AddTask(MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanel, TableLayoutPanel tableLayout,
            MindMate.Model.MapNode node, DateTime dateTime, string dueOnText)
        {
            TaskView tv = new TaskView(node, dateTime, dueOnText, OnTaskViewEvent);

            if (tableLayout.RowCount == 0) collapsiblePanel.Visible = true;
            
            InsertTaskView(tableLayout, tv);
            
            tableLayout.Height = tableLayout.RowCount * tv.Height + (tableLayout.Margin.Bottom * tableLayout.RowCount * 2);
            collapsiblePanel.Height = tableLayout.Height + tableLayout.Top;
        }

        private void OnTaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            TaskViewEvent(tv, e);
        }

        public void RemoveTask(TaskView tv)
        {
            TableLayoutPanel panel = (TableLayoutPanel)tv.Parent;
            CollapsiblePanel collapsiblePanel = (CollapsiblePanel)panel.Parent;

            int rowNum = panel.GetCellPosition(tv).Row;
            panel.Controls.Remove(tv);

            panel.RowCount -= 1;

            for(int i = rowNum; i < panel.RowCount; i++)
            {
                panel.SetRow(panel.GetControlFromPosition(0, i + 1), i); // move one row up
            }

            panel.Height = panel.RowCount * tv.Height + (panel.Margin.Bottom * panel.RowCount * 2);
            collapsiblePanel.Height = panel.Height + panel.Top;
            if (panel.RowCount == 0) collapsiblePanel.Visible = false;

            AdjustMainPanelHeight();
            
        }

        private void AdjustMainPanelHeight()
        {
            Control lastTaskGroup = GetLastTaskGroup();
            if (lastTaskGroup != null)
                this.tablePanelMain.Height = lastTaskGroup.Location.Y + lastTaskGroup.Size.Height + lastTaskGroup.Margin.Bottom;
            else
                this.tablePanelMain.Height = 20;
        }

        
        private void InsertTaskView(TableLayoutPanel tableLayout, TaskView taskView)
        {
            if (tableLayout.RowCount == 0)
            {
                tableLayout.Controls.Add(taskView, 0, 0); // add if list is empty
                tableLayout.RowCount += 1;
                return;
            }
            else
            {
                for (int i = tableLayout.RowCount - 1; i >= 0; i--)
                {
                    TaskView tv = (TaskView)tableLayout.GetControlFromPosition(0, i);
                    if (tv.DueDate > taskView.DueDate)
                    {
                        tableLayout.SetRow(tv, i + 1);
                    }
                    else
                    {
                        tableLayout.Controls.Add(taskView, 0, i + 1); // add in the middle or end
                        tableLayout.RowCount += 1;
                        return;
                    }
                }

                tableLayout.Controls.Add(taskView, 0, 0); // add at the top after all controls are moved down using loop
                tableLayout.RowCount += 1;
                return;
            }
            
        }

        /// <summary>
        /// returns null if TaskView is not found
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate"></param>
        /// <returns></returns>
        public TaskView FindTaskView(MapNode node, DateTime dueDate)
        {
            if (DateHelper.IsToday(dueDate))
            {
                return FindTaskViewInGroup(tableLayoutToday, node, dueDate);
            }
            else if (DateHelper.IsTomorrow(dueDate))
            {
                return FindTaskViewInGroup(tableLayoutTomorrow, node, dueDate);
            }
            else if (DateHelper.DateInThisWeek(dueDate))
            {
                return FindTaskViewInGroup(tableLayoutThisWeek, node, dueDate);
            }
            else if (DateHelper.DateInThisMonth(dueDate))
            {
                return FindTaskViewInGroup(tableLayoutThisMonth, node, dueDate);
            }
            else if (DateHelper.DateInNextMonth(dueDate))
            {
                return FindTaskViewInGroup(tableLayoutNextMonth, node, dueDate);
            }

            return null;            
        }

        private TaskView FindTaskViewInGroup(TableLayoutPanel table, MapNode node, DateTime dueDate)
        {
            for(int i = 0; i < table.RowCount; i++)
            {
                TaskView tv = (TaskView)table.GetControlFromPosition(0, i);
                if(tv.MapNode == node)
                {
                    return tv;
                }
                
                if(tv.DueDate > dueDate)
                {
                    break;
                }
            }

            return null;
        }

        private Control GetLastTaskGroup()
        {
            for(int i = this.tablePanelMain.RowCount - 1; i >= 0; i--)
            {
                Control ctrl = this.tablePanelMain.GetControlFromPosition(0, i);

                // check for ctrl is the last visible Task Group
                if (ctrl != null // control is null in case when it is never made visible  
                    &&
                    (ctrl.Visible || // visible is false when it is the first control ever made visible in TaskList
                    ((TableLayoutPanel)ctrl.Controls[1]).RowCount > 0) // finds if there is any rows inside
                    )
                {
                    return ctrl;
                }
            }

            return null;
        }
        
    }
}
