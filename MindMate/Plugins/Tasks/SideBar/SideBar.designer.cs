namespace MindMate.Plugins.Tasks.SideBar
{
    partial class SideBar
    {
        
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void MyInitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.tablePanelMain = new System.Windows.Forms.TableLayoutPanel();
            
            this.lblNoTasks = new System.Windows.Forms.Label();

            this.tablePanelMain.SuspendLayout();
            this.panelMain.SuspendLayout();
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
            this.tablePanelMain.Location = new System.Drawing.Point(3, 3);
            this.tablePanelMain.Name = "tablePanelMain";
            this.tablePanelMain.RowCount = 0;
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanelMain.Size = new System.Drawing.Size(269, 600);
            this.tablePanelMain.TabIndex = 1;

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
        
        private System.Windows.Forms.Label lblNoTasks;
    }
}