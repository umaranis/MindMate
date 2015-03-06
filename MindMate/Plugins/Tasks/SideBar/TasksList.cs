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

namespace MindMate.Plugins.Tasks.SideBar
{
    public partial class TasksList : UserControl
    {
        public TasksList()
        {
            MyInitializeComponent();
        }

        public event Action<TaskView, TaskView.TaskViewEvent> TaskViewEvent;

        public void Add(MindMate.Model.MapNode node, DateTime dateTime)
        {
            if(DateHelper.IsOverdue(dateTime))
            {
                AddTask(this.collapsiblePanelOverdue, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if(DateHelper.IsToday(dateTime))
            {
                AddTask(this.collapsiblePanelToday, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }              
            else if(DateHelper.IsTomorrow(dateTime))
            {
                AddTask(this.collapsiblePanelTomorrow, node, dateTime,
                    DateHelper.GetTimePartString(dateTime));
            }
            else if(DateHelper.DateInThisWeek(dateTime))
            {
                AddTask(this.collapsiblePanelThisWeek, node, dateTime,
                    DateHelper.GetWeekDayString(dateTime));
            }
            else if(DateHelper.DateInThisMonth(dateTime))
            {
                AddTask(this.collapsiblePanelThisMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }
            else if (DateHelper.DateInNextMonth(dateTime))
            {
                AddTask(this.collapsiblePanelNextMonth, node, dateTime,
                    DateHelper.GetDayOfMonthString(dateTime));
            }

            AdjustMainPanelHeight();
        }

        
        private void AddTask(MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> taskGroup, 
            MindMate.Model.MapNode node, DateTime dateTime, string dueOnText)
        {
            TaskView tv = new TaskView(node, dateTime, dueOnText, OnTaskViewEvent);

            if (taskGroup.Table.RowCount == 0)
            {
                taskGroup.Visible = true;
                lblNoTasks.Visible = false;
            }
            
            InsertTaskView(taskGroup, tv); 
        }

        private void OnTaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            TaskViewEvent(tv, e);
        }

        public void RemoveTask(TaskView tv)
        {
            TableLayoutPanel panel = (TableLayoutPanel)tv.Parent;
            var collapsiblePanel = (TaskGroup<TaskView>)panel.Parent;

            collapsiblePanel.Remove(tv);

            if (panel.RowCount == 0)
            {
                if (GetLastControl() == null) lblNoTasks.Visible = true;
            }

            AdjustMainPanelHeight();
            
        }

        private void AdjustMainPanelHeight()
        {
            Control lastTaskGroup = GetLastControl();
            if (lastTaskGroup != null)
                this.tablePanelMain.Height = lastTaskGroup.Location.Y + lastTaskGroup.Size.Height + lastTaskGroup.Margin.Bottom;
            else
                this.tablePanelMain.Height = 20;
        }

        
        private void InsertTaskView(TaskGroup<TaskView> taskGroup, TaskView taskView)
        {
            if (taskGroup.Count == 0)
            {
                taskGroup.Add(taskView); // add if list is empty
            }
            else
            {
                for (int i = taskGroup.Count - 1; i >= 0; i--)
                {
                    TaskView tv = (TaskView)taskGroup.Table.GetControlFromPosition(0, i);
                    if (tv.DueDate > taskView.DueDate)
                    {
                        continue;
                    }
                    else
                    {
                        taskGroup.Insert(i + 1, taskView); // add in the middle or end
                        return;
                    }
                }

                taskGroup.Insert(0, taskView); // add at the top after all controls are moved down using loop
            }
            
        }

        /// <summary>
        /// returns null if TaskView is not found
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dueDate"></param>
        /// <returns>returns null if not found</returns>
        public TaskView FindTaskView(MapNode node, DateTime dueDate)
        {
            if(DateHelper.IsOverdue(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelOverdue.Table, node, dueDate);
            }
            else if (DateHelper.IsToday(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelToday.Table, node, dueDate);
            }
            else if (DateHelper.IsTomorrow(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelTomorrow.Table, node, dueDate);
            }
            else if (DateHelper.DateInThisWeek(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelThisWeek.Table, node, dueDate);
            }
            else if (DateHelper.DateInThisMonth(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelThisMonth.Table, node, dueDate);
            }
            else if (DateHelper.DateInNextMonth(dueDate))
            {
                return FindTaskViewInGroup(collapsiblePanelNextMonth.Table, node, dueDate);
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

        public TaskView GetTaskView(int index)
        {
            int currentIndex = index;
            for(int i = 0; i < TaskGroupCount; i++)
            {
                CollapsiblePanel panel = GetTaskGroup(i);
                if (panel != null)
                {
                    TableLayoutPanel table = (TableLayoutPanel)panel.Controls[1];
                    TaskView tv = (TaskView)table.GetControlFromPosition(0, currentIndex);
                    if (tv != null)
                    {
                        return tv;
                    }
                    else
                    {
                        currentIndex -= table.RowCount;
                    }
                }
            }

            return null;
        }

        public CollapsiblePanel GetTaskGroup(int index)
        {
            return (CollapsiblePanel)tablePanelMain.GetControlFromPosition(0, index);
        }

        public int TaskGroupCount
        {
            get { return tablePanelMain.RowCount; }
        }

        public TaskView GetNextTaskViewInGroup(TaskView tv)
        {
            TableLayoutPanel table = (TableLayoutPanel)tv.Parent;
            var cell = table.GetPositionFromControl(tv);
            return (TaskView)table.GetControlFromPosition(cell.Column, cell.Row + 1);
        }

        public TaskView GetPreviousTaskViewInGroup(TaskView tv)
        {
            TableLayoutPanel table = (TableLayoutPanel)tv.Parent;
            var cell = table.GetPositionFromControl(tv);
            return (TaskView)table.GetControlFromPosition(cell.Column, cell.Row - 1);
        }

        public CollapsiblePanel GetNextTaskGroup(CollapsiblePanel taskGroup)
        {
            var cell = tablePanelMain.GetPositionFromControl(taskGroup);
            return (CollapsiblePanel)tablePanelMain.GetControlFromPosition(cell.Column, cell.Row + 1);
        }

        public CollapsiblePanel GetPreviousTaskGroup(CollapsiblePanel taskGroup)
        {
            var cell = tablePanelMain.GetPositionFromControl(taskGroup);
            return (CollapsiblePanel)tablePanelMain.GetControlFromPosition(cell.Column, cell.Row - 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>return null if no Task Group is visible</returns>
        private Control GetLastControl()
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
