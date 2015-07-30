using MindMate.MetaModel;
using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class TaskDueIcon : ISystemIcon
    {
        public TaskDueIcon(PendingTaskList taskList)
        {
            taskList.TaskChanged += (n, e) =>
            {
                if (StatusChange == null) return;

                switch(e.TaskChange)
                {
                    case PendingTaskChange.TaskAdded:
                    case PendingTaskChange.TaskReopened:
                        StatusChange(n, this, SystemIconStatusChange.Show);
                        break;
                    case PendingTaskChange.TaskCompleted:
                        StatusChange(n, this, SystemIconStatusChange.Hide);
                        break;
                    case PendingTaskChange.TaskRemoved:
                        StatusChange(n, this, SystemIconStatusChange.Hide);
                        break;
                }
            };
        }

        public string Name { get { return "TaskPending"; } }

        public bool ShowIcon(MapNode node)
        {
            return node.DueDateExists() && node.IsTaskPending();          
        }

        public event Action<MapNode, ISystemIcon, SystemIconStatusChange> StatusChange;

        public System.Drawing.Bitmap Bitmap
        {
            get { return TaskRes.date; }
        }
        
    }
}
