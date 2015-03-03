using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public class TaskViewContextMenu : ContextMenuStrip
    {
        public TaskViewContextMenu()
        {
            this.Items.Add("Complete Task");
            this.Items.Add("Remove Task");
            this.Items.Add("Move Up");
            this.Items.Add("Move Down");
            this.Items.Add("Edit Due Date ...");
            this.Items.Add("Quick Due Dates");
        }
    }
}
