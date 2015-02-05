namespace MindMate.Plugins.Tasks
{
    partial class TasksList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tablePanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.collapsiblePanelThisWeek = new MindMate.Plugins.Tasks.CollapsiblePanel();
            this.tableLayoutThisWeek = new System.Windows.Forms.TableLayoutPanel();
            this.collapsiblePanelTomorrow = new MindMate.Plugins.Tasks.CollapsiblePanel();
            this.tableLayoutTomorrow = new System.Windows.Forms.TableLayoutPanel();
            this.collapsiblePanelToday = new MindMate.Plugins.Tasks.CollapsiblePanel();
            this.tableLayoutToday = new System.Windows.Forms.TableLayoutPanel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.collapsiblePanelThisMonth = new MindMate.Plugins.Tasks.CollapsiblePanel();
            this.tableLayoutThisMonth = new System.Windows.Forms.TableLayoutPanel();
            this.collapsiblePanelNextMonth = new MindMate.Plugins.Tasks.CollapsiblePanel();
            this.tableLayoutNextMonth = new System.Windows.Forms.TableLayoutPanel();
            this.tablePanelMain.SuspendLayout();
            this.collapsiblePanelThisWeek.SuspendLayout();
            this.collapsiblePanelTomorrow.SuspendLayout();
            this.collapsiblePanelToday.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.collapsiblePanelThisMonth.SuspendLayout();
            this.collapsiblePanelNextMonth.SuspendLayout();
            this.tableLayoutNextMonth.SuspendLayout();
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
            this.tablePanelMain.Controls.Add(this.collapsiblePanelToday, 0, 0);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelTomorrow, 0, 1);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelThisWeek, 0, 2);
            this.tablePanelMain.Controls.Add(this.collapsiblePanelThisMonth, 0, 3);  
            this.tablePanelMain.Controls.Add(this.collapsiblePanelNextMonth, 0, 4);              
            this.tablePanelMain.Location = new System.Drawing.Point(3, 3);
            this.tablePanelMain.Name = "tablePanelMain";
            this.tablePanelMain.RowCount = 5;
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.Size = new System.Drawing.Size(269, 600);
            this.tablePanelMain.TabIndex = 1;
            // 
            // collapsiblePanelToday
            // 
            this.collapsiblePanelToday.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelToday.AnimationInterval = 20;
            this.collapsiblePanelToday.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelToday.Controls.Add(this.tableLayoutToday);
            this.collapsiblePanelToday.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelToday.HeaderImage = null;
            this.collapsiblePanelToday.HeaderText = "Today";
            this.collapsiblePanelToday.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelToday.Location = new System.Drawing.Point(3, 3);
            this.collapsiblePanelToday.Name = "collapsiblePanelToday";
            this.collapsiblePanelToday.RoundedCorners = false;
            this.collapsiblePanelToday.ShowHeaderSeparator = false;
            this.collapsiblePanelToday.Size = new System.Drawing.Size(264, 151);
            this.collapsiblePanelToday.TabIndex = 2;
            this.collapsiblePanelToday.UseAnimation = true;
            this.collapsiblePanelToday.Visible = false;
            // 
            // tableLayoutToday
            // 
            this.tableLayoutToday.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutToday.ColumnCount = 1;
            this.tableLayoutToday.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutToday.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutToday.Name = "tableLayoutToday";
            this.tableLayoutToday.RowCount = 0;
            this.tableLayoutToday.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutToday.Size = new System.Drawing.Size(264, 120);
            this.tableLayoutToday.TabIndex = 3;
            // 
            // collapsiblePanelTomorrow
            // 
            this.collapsiblePanelTomorrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelTomorrow.AnimationInterval = 20;
            this.collapsiblePanelTomorrow.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelTomorrow.Controls.Add(this.tableLayoutTomorrow);
            this.collapsiblePanelTomorrow.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelTomorrow.HeaderImage = null;
            this.collapsiblePanelTomorrow.HeaderText = "Tomorrow";
            this.collapsiblePanelTomorrow.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelTomorrow.Location = new System.Drawing.Point(3, 160);
            this.collapsiblePanelTomorrow.Name = "collapsiblePanelTomorrow";
            this.collapsiblePanelTomorrow.RoundedCorners = false;
            this.collapsiblePanelTomorrow.ShowHeaderSeparator = false;
            this.collapsiblePanelTomorrow.Size = new System.Drawing.Size(264, 150);
            this.collapsiblePanelTomorrow.TabIndex = 4;
            this.collapsiblePanelTomorrow.UseAnimation = true;
            this.collapsiblePanelTomorrow.Visible = false;
            // 
            // tableLayoutTomorrow
            // 
            this.tableLayoutTomorrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutTomorrow.ColumnCount = 1;
            this.tableLayoutTomorrow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutTomorrow.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutTomorrow.Name = "tableLayoutTomorrow";
            this.tableLayoutTomorrow.RowCount = 0;
            this.tableLayoutTomorrow.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutTomorrow.Size = new System.Drawing.Size(264, 120);
            this.tableLayoutTomorrow.TabIndex = 5;
            // 
            // collapsiblePanelThisWeek
            // 
            this.collapsiblePanelThisWeek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelThisWeek.AnimationInterval = 20;
            this.collapsiblePanelThisWeek.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelThisWeek.Controls.Add(this.tableLayoutThisWeek);
            this.collapsiblePanelThisWeek.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelThisWeek.HeaderImage = null;
            this.collapsiblePanelThisWeek.HeaderText = "This Week";
            this.collapsiblePanelThisWeek.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelThisWeek.Location = new System.Drawing.Point(3, 316);
            this.collapsiblePanelThisWeek.Name = "collapsiblePanelThisWeek";
            this.collapsiblePanelThisWeek.RoundedCorners = false;
            this.collapsiblePanelThisWeek.ShowHeaderSeparator = false;
            this.collapsiblePanelThisWeek.Size = new System.Drawing.Size(263, 150);
            this.collapsiblePanelThisWeek.TabIndex = 6;
            this.collapsiblePanelThisWeek.UseAnimation = true;
            this.collapsiblePanelThisWeek.Visible = false;
            // 
            // tableLayoutThisWeek
            // 
            this.tableLayoutThisWeek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutThisWeek.ColumnCount = 1;
            this.tableLayoutThisWeek.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutThisWeek.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutThisWeek.Name = "tableLayoutThisWeek";
            this.tableLayoutThisWeek.RowCount = 0;
            this.tableLayoutThisWeek.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutThisWeek.Size = new System.Drawing.Size(264, 120);
            this.tableLayoutThisWeek.TabIndex = 7;            
            // 
            // collapsiblePanelThisMonth
            // 
            this.collapsiblePanelThisMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelThisMonth.AnimationInterval = 20;
            this.collapsiblePanelThisMonth.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelThisMonth.Controls.Add(this.tableLayoutThisMonth);
            this.collapsiblePanelThisMonth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelThisMonth.HeaderImage = null;
            this.collapsiblePanelThisMonth.HeaderText = "This Month";
            this.collapsiblePanelThisMonth.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelThisMonth.Location = new System.Drawing.Point(3, 472);
            this.collapsiblePanelThisMonth.Name = "collapsiblePanelThisMonth";
            this.collapsiblePanelThisMonth.RoundedCorners = false;
            this.collapsiblePanelThisMonth.ShowHeaderSeparator = false;
            this.collapsiblePanelThisMonth.Size = new System.Drawing.Size(263, 140);
            this.collapsiblePanelThisMonth.TabIndex = 8;
            this.collapsiblePanelThisMonth.UseAnimation = true;
            this.collapsiblePanelThisMonth.Visible = false;
            // 
            // tableLayoutThisMonth
            // 
            this.tableLayoutThisMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutThisMonth.ColumnCount = 1;
            this.tableLayoutThisMonth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutThisMonth.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutThisMonth.Name = "tableLayoutThisMonth";
            this.tableLayoutThisMonth.RowCount = 0;
            this.tableLayoutThisMonth.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutThisMonth.Size = new System.Drawing.Size(264, 120);
            this.tableLayoutThisMonth.TabIndex = 9;
            // 
            // collapsiblePanelNextMonth
            // 
            this.collapsiblePanelNextMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.collapsiblePanelNextMonth.AnimationInterval = 20;
            this.collapsiblePanelNextMonth.BackColor = System.Drawing.Color.Transparent;
            this.collapsiblePanelNextMonth.Controls.Add(this.tableLayoutNextMonth);
            this.collapsiblePanelNextMonth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.collapsiblePanelNextMonth.HeaderImage = null;
            this.collapsiblePanelNextMonth.HeaderText = "Next Month";
            this.collapsiblePanelNextMonth.HeaderTextColor = System.Drawing.Color.Black;
            this.collapsiblePanelNextMonth.Location = new System.Drawing.Point(3, 618);
            this.collapsiblePanelNextMonth.Name = "collapsiblePanelNextMonth";
            this.collapsiblePanelNextMonth.RoundedCorners = false;
            this.collapsiblePanelNextMonth.ShowHeaderSeparator = false;
            this.collapsiblePanelNextMonth.Size = new System.Drawing.Size(263, 151);
            this.collapsiblePanelNextMonth.TabIndex = 10;
            this.collapsiblePanelNextMonth.UseAnimation = true;
            this.collapsiblePanelNextMonth.Visible = false;
            // 
            // tableLayoutNextMonth
            // 
            this.tableLayoutNextMonth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutNextMonth.ColumnCount = 1;
            this.tableLayoutNextMonth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutNextMonth.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutNextMonth.Name = "tableLayoutNextMonth";
            this.tableLayoutNextMonth.RowCount = 0;
            this.tableLayoutNextMonth.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutNextMonth.Size = new System.Drawing.Size(264, 120);
            this.tableLayoutNextMonth.TabIndex = 11;
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
            this.tableLayoutNextMonth.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablePanelMain;
        private System.Windows.Forms.Panel panelMain;
        private MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanelThisWeek;
        private System.Windows.Forms.TableLayoutPanel tableLayoutThisWeek;
        private MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanelTomorrow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutTomorrow;
        private MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanelToday;
        private System.Windows.Forms.TableLayoutPanel tableLayoutToday;
        private MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanelNextMonth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutNextMonth;
        private MindMate.Plugins.Tasks.CollapsiblePanel collapsiblePanelThisMonth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutThisMonth;
    }
}