namespace MindMate.Plugins.Tasks.SideBar
{
    partial class TasksList
    {
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void MyInitializeComponent()
        {
            this.tablePanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.collapsiblePanelThisWeek = new MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView>();
            this.collapsiblePanelTomorrow = new MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView>();
            this.collapsiblePanelOverdue = new MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView>();
            this.collapsiblePanelToday = new MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView>();
            this.panelMain = new System.Windows.Forms.Panel();
            this.collapsiblePanelThisMonth = new MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView>();
            this.collapsiblePanelNextMonth = new MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView>();
            this.lblNoTasks = new System.Windows.Forms.Label();
            this.tablePanelMain.SuspendLayout();
            this.collapsiblePanelThisWeek.SuspendLayout();
            this.collapsiblePanelTomorrow.SuspendLayout();
            this.collapsiblePanelToday.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.collapsiblePanelThisMonth.SuspendLayout();
            this.collapsiblePanelNextMonth.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.AutoScroll = true;
            this.panelMain.BackColor = System.Drawing.SystemColors.Control;
            this.panelMain.Controls.Add(this.tablePanelMain);
            this.panelMain.Controls.Add(lblNoTasks);
            this.lblNoTasks.BringToFront();
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(274, 416);
            this.panelMain.TabIndex = 0;
            // 
            // tablePanelMain
            // 
            this.tablePanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablePanelMain.BackColor = System.Drawing.SystemColors.Control;
            this.tablePanelMain.ColumnCount = 1;
            this.tablePanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePanelMain.Controls.Add(this.collapsiblePanelOverdue, 0, 0);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelToday, 0, 1);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelTomorrow, 0, 2);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelThisWeek, 0, 3);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelThisMonth, 0, 4);  
            this.tablePanelMain.Controls.Add(this.collapsiblePanelNextMonth, 0, 5);              
            this.tablePanelMain.Location = new System.Drawing.Point(3, 3);
            this.tablePanelMain.Name = "tablePanelMain";
            this.tablePanelMain.RowCount = 6;
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.Size = new System.Drawing.Size(269, 600);
            this.tablePanelMain.TabIndex = 1;
            // 
            // collapsiblePanelOverdue
            // 
            this.collapsiblePanelOverdue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelOverdue.AnimationInterval = 20;
            this.collapsiblePanelOverdue.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelOverdue.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelOverdue.HeaderImage = null;
            this.collapsiblePanelOverdue.HeaderText = "Overdue";
            this.collapsiblePanelOverdue.HeaderTextColor = System.Drawing.Color.Red;
            this.collapsiblePanelOverdue.Location = new System.Drawing.Point(3, 3);
            this.collapsiblePanelOverdue.Name = "collapsiblePanelOverdue";
            this.collapsiblePanelOverdue.RoundedCorners = false;
            this.collapsiblePanelOverdue.ShowHeaderSeparator = false;
            this.collapsiblePanelOverdue.Size = new System.Drawing.Size(264, 151);
            this.collapsiblePanelOverdue.TabIndex = 2;
            this.collapsiblePanelOverdue.UseAnimation = true;
            this.collapsiblePanelOverdue.Visible = false;
            // 
            // collapsiblePanelToday
            // 
            this.collapsiblePanelToday.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelToday.AnimationInterval = 20;
            this.collapsiblePanelToday.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelToday.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelToday.HeaderImage = null;
            this.collapsiblePanelToday.HeaderText = "Today";
            this.collapsiblePanelToday.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelToday.Location = new System.Drawing.Point(3, 3);
            this.collapsiblePanelToday.Name = "collapsiblePanelToday";
            this.collapsiblePanelToday.RoundedCorners = false;
            this.collapsiblePanelToday.ShowHeaderSeparator = false;
            this.collapsiblePanelToday.Size = new System.Drawing.Size(264, 151);
            this.collapsiblePanelToday.TabIndex = 4;
            this.collapsiblePanelToday.UseAnimation = true;
            this.collapsiblePanelToday.Visible = false;
            // 
            // collapsiblePanelTomorrow
            // 
            this.collapsiblePanelTomorrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelTomorrow.AnimationInterval = 20;
            this.collapsiblePanelTomorrow.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelTomorrow.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelTomorrow.HeaderImage = null;
            this.collapsiblePanelTomorrow.HeaderText = "Tomorrow";
            this.collapsiblePanelTomorrow.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelTomorrow.Location = new System.Drawing.Point(3, 160);
            this.collapsiblePanelTomorrow.Name = "collapsiblePanelTomorrow";
            this.collapsiblePanelTomorrow.RoundedCorners = false;
            this.collapsiblePanelTomorrow.ShowHeaderSeparator = false;
            this.collapsiblePanelTomorrow.Size = new System.Drawing.Size(264, 150);
            this.collapsiblePanelTomorrow.TabIndex = 6;
            this.collapsiblePanelTomorrow.UseAnimation = true;
            this.collapsiblePanelTomorrow.Visible = false;
            // 
            // collapsiblePanelThisWeek
            // 
            this.collapsiblePanelThisWeek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelThisWeek.AnimationInterval = 20;
            this.collapsiblePanelThisWeek.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelThisWeek.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelThisWeek.HeaderImage = null;
            this.collapsiblePanelThisWeek.HeaderText = "This Week";
            this.collapsiblePanelThisWeek.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelThisWeek.Location = new System.Drawing.Point(3, 316);
            this.collapsiblePanelThisWeek.Name = "collapsiblePanelThisWeek";
            this.collapsiblePanelThisWeek.RoundedCorners = false;
            this.collapsiblePanelThisWeek.ShowHeaderSeparator = false;
            this.collapsiblePanelThisWeek.Size = new System.Drawing.Size(263, 150);
            this.collapsiblePanelThisWeek.TabIndex = 8;
            this.collapsiblePanelThisWeek.UseAnimation = true;
            this.collapsiblePanelThisWeek.Visible = false;
            // 
            // collapsiblePanelThisMonth
            // 
            this.collapsiblePanelThisMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelThisMonth.AnimationInterval = 20;
            this.collapsiblePanelThisMonth.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelThisMonth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelThisMonth.HeaderImage = null;
            this.collapsiblePanelThisMonth.HeaderText = "This Month";
            this.collapsiblePanelThisMonth.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelThisMonth.Location = new System.Drawing.Point(3, 472);
            this.collapsiblePanelThisMonth.Name = "collapsiblePanelThisMonth";
            this.collapsiblePanelThisMonth.RoundedCorners = false;
            this.collapsiblePanelThisMonth.ShowHeaderSeparator = false;
            this.collapsiblePanelThisMonth.Size = new System.Drawing.Size(263, 140);
            this.collapsiblePanelThisMonth.TabIndex = 10;
            this.collapsiblePanelThisMonth.UseAnimation = true;
            this.collapsiblePanelThisMonth.Visible = false;
            // 
            // collapsiblePanelNextMonth
            // 
            this.collapsiblePanelNextMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelNextMonth.AnimationInterval = 20;
            this.collapsiblePanelNextMonth.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelNextMonth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelNextMonth.HeaderImage = null;
            this.collapsiblePanelNextMonth.HeaderText = "Next Month";
            this.collapsiblePanelNextMonth.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelNextMonth.Location = new System.Drawing.Point(3, 618);
            this.collapsiblePanelNextMonth.Name = "collapsiblePanelNextMonth";
            this.collapsiblePanelNextMonth.RoundedCorners = false;
            this.collapsiblePanelNextMonth.ShowHeaderSeparator = false;
            this.collapsiblePanelNextMonth.Size = new System.Drawing.Size(263, 151);
            this.collapsiblePanelNextMonth.TabIndex = 12;
            this.collapsiblePanelNextMonth.UseAnimation = true;
            this.collapsiblePanelNextMonth.Visible = false;
            //
            // lblNoTasks
            //
            CreateNoTaskLabel();
            // 
            // TasksList
            // 
            this.Controls.Add(this.panelMain);
            this.Name = "TasksList";
            this.Size = new System.Drawing.Size(276, 418);
            this.tablePanelMain.ResumeLayout(false);
            this.collapsiblePanelThisWeek.ResumeLayout(false);
            this.collapsiblePanelTomorrow.ResumeLayout(false);
            this.collapsiblePanelToday.ResumeLayout(false);            
            this.collapsiblePanelThisMonth.ResumeLayout(false);
            this.collapsiblePanelNextMonth.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void CreateNoTaskLabel()
        {
            lblNoTasks.Text = "There is no pending task till end of next month.";
            lblNoTasks.ForeColor = System.Drawing.Color.Black;
            lblNoTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            lblNoTasks.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            lblNoTasks.Padding = new System.Windows.Forms.Padding(25);            
        }

        private System.Windows.Forms.TableLayoutPanel tablePanelMain;
        private System.Windows.Forms.Panel panelMain;
        private MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> collapsiblePanelThisWeek;
        private MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> collapsiblePanelTomorrow;
        private MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> collapsiblePanelOverdue;
        private MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> collapsiblePanelToday;
        private MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> collapsiblePanelNextMonth;
        private MindMate.Plugins.Tasks.SideBar.TaskGroup<TaskView> collapsiblePanelThisMonth;
        private System.Windows.Forms.Label lblNoTasks;
    }
}