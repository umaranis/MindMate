using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.Model;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskView : UserControl
    {
        public TaskView(MapNode node, DateTime dueDate, string dueOnText,
            Action<TaskView, TaskViewEvent> onTaskViewEvent)
        {
            InitializeComponent();

            this.ContextMenuStrip = new TaskViewContextMenu();

            OnTaskViewEvent = onTaskViewEvent;

            SetQuickActionsVisiblity(false);

            MapNode = node;

            TaskTitle = node.Text;

            //TODO: What will happen if node is moved or any of the ancestors is moved
            lblTaskPath.Text = "";
            MapNode parentNode = node.Parent;
            while (parentNode != null)
            {
                if (lblTaskPath.Text != "") lblTaskPath.Text += " <- ";
                lblTaskPath.Text += parentNode.Text;
                parentNode = parentNode.Parent;
            }

            DueDate = dueDate;

            TaskDueOnText = dueOnText;

            if (node.Selected)
                Selected = true;
        }

        public void SetQuickActionsVisiblity(bool visible)
        {
            btnRemove.Visible = visible;
            btnComplete.Visible = visible;
            btnDown.Visible = visible;
            btnUp.Visible = visible;
        }

        public string TaskTitle
        {
            get
            {
                return this.lblNodeName.Text; 
            }
            set
            {
                this.lblNodeName.Text = value;
            }
        }

        public string TaskPath
        {
            get
            {
                return this.lblTaskPath.Text;
            }
            set
            {
                this.lblTaskPath.Text = value;
            }
        }

        public string TaskDueOnText
        {
            get
            {
                return this.lblDueOn.Text;
            }
            set
            {
                this.lblDueOn.Text = value;
            }
        }

        public MapNode MapNode
        {
            get
            {
                return (MapNode)this.Tag;
            }
            set
            {
                this.Tag = value;
            }
        }

        public DateTime DueDate { get; set; }

        private Action<TaskView, TaskViewEvent> OnTaskViewEvent;

        protected override void OnMouseEnter(EventArgs e)
        {
            SetQuickActionsVisiblity(true);            
            //BackColor = Color.AliceBlue;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TaskView_Paint);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            SetQuickActionsVisiblity(false);
            //BackColor = SystemColors.ControlLight;
            this.Paint -= new System.Windows.Forms.PaintEventHandler(this.TaskView_Paint);
            Invalidate();
        }

        
        private void TaskView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);            
        }

        private void TaskView_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.X > btnRemove.Left && e.Y > btnRemove.Top
                && e.X < btnRemove.Left + btnRemove.Width && e.Y < btnRemove.Top + btnRemove.Height)
            {
                OnTaskViewEvent(this, TaskViewEvent.Remove);
            }
            else if (e.X > btnComplete.Left && e.Y > btnComplete.Top
                && e.X < btnComplete.Left + btnComplete.Width && e.Y < btnComplete.Top + btnComplete.Height)
            {
                OnTaskViewEvent(this, TaskViewEvent.Complete);
            }
            else if (e.X > btnDown.Left && e.Y > btnDown.Top
            && e.X < btnDown.Left + btnDown.Width && e.Y < btnDown.Top + btnDown.Height)
            {
                OnTaskViewEvent(this, TaskViewEvent.MoveDown);
            }
            else if (e.X > btnUp.Left && e.Y > btnUp.Top
            && e.X < btnUp.Left + btnUp.Width && e.Y < btnUp.Top + btnUp.Height)
            {
                OnTaskViewEvent(this, TaskViewEvent.MoveUp);
            }
            else
            {
                MapNode.Selected = true;
            }
        }

        public enum TaskViewEvent { Remove, Edit, Complete, MoveUp, MoveDown }


        public bool Selected { 
            get 
            {
                return BackColor == Color.AliceBlue;
            }
            set 
            { 
                if(value)
                    BackColor = Color.AliceBlue;
                else
                    BackColor = SystemColors.ControlLight; 
            }
        }
    }    
}
