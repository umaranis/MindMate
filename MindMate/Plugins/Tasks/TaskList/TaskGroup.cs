using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public class TaskGroup<T> : CollapsiblePanel
    {
        public TaskGroup()
        {
            TableLayoutPanel table = new TableLayoutPanel();

            this.Controls.Add(table);

            table.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            table.ColumnCount = 1;
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            table.Location = new System.Drawing.Point(0, 30);
            //table.Name = "tableLayout"  + this.HeaderText.Replace(' ','_');
            table.RowCount = 0;
            table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            table.Size = new System.Drawing.Size(264, 120);
            //table.TabIndex = 3;  
        }

        public TableLayoutPanel Table
        {
            get { return (TableLayoutPanel)this.Controls[1]; }
        }
                
    }
}
