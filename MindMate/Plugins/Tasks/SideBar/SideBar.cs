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
    public partial class SideBar : UserControl
    {

        public SideBar()
        {
            controlGroups = new ControlGroupCollection(this);

            MyInitializeComponent();
        }

        private ControlGroupCollection controlGroups; 
        public ControlGroupCollection ControlGroups 
        { 
            get
            {
                return controlGroups;
            }
        }

        private void AdjustMainPanelHeight()
        {
            Control lastTaskGroup = GetLastControl();
            if (lastTaskGroup != null)
                this.tablePanelMain.Height = lastTaskGroup.Location.Y + lastTaskGroup.Size.Height + lastTaskGroup.Margin.Bottom;
            else
                this.tablePanelMain.Height = 20;
        }

        internal void OnControlAdded(object sender, ControlEventArgs e)
        {
            AdjustMainPanelHeight();

            lblNoTasks.Visible = false;            
        }

        internal void OnControlRemoved(object sender, ControlEventArgs e)
        {
            AdjustMainPanelHeight();

            if (GetLastControl() == null) lblNoTasks.Visible = true;
        }

        public Control GetControl(int index)
        {
            int currentIndex = index;
            for(int i = 0; i < ControlGroups.Count; i++)
            {
                CollapsiblePanel panel = ControlGroups[i];
                if (panel != null)
                {
                    TableLayoutPanel table = (TableLayoutPanel)panel.Controls[1];
                    Control ctrl = table.GetControlFromPosition(0, currentIndex);
                    if (ctrl != null)
                    {
                        return ctrl;
                    }
                    else
                    {
                        currentIndex -= table.RowCount;
                    }
                }
            }

            return null;
        }

        public ControlGroup GetControlGroup(Control control)
        {
            return (ControlGroup)control.Parent.Parent;
        }

        public Control GetNextControlInGroup(Control tv)
        {
            TableLayoutPanel table = (TableLayoutPanel)tv.Parent;
            var cell = table.GetPositionFromControl(tv);
            return table.GetControlFromPosition(cell.Column, cell.Row + 1);
        }

        public Control GetPreviousControlInGroup(Control tv)
        {
            TableLayoutPanel table = (TableLayoutPanel)tv.Parent;
            var cell = table.GetPositionFromControl(tv);
            if (cell.Row > 0)
                return table.GetControlFromPosition(cell.Column, cell.Row - 1);
            else
                return null;
        }

        public ControlGroup GetNextControlGroup(ControlGroup taskGroup)
        {
            int row = tablePanelMain.GetRow(taskGroup) + 1;

            if (row < tablePanelMain.RowCount)
                return ControlGroups[row];
            else
                return null;
        }

        public ControlGroup GetPreviousControlGroup(ControlGroup taskGroup)
        {
            int row = tablePanelMain.GetRow(taskGroup) - 1;

            if (row > -1)
                return ControlGroups[row];
            else
                return null;
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
