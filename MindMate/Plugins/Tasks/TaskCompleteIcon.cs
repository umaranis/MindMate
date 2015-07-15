using MindMate.MetaModel;
using MindMate.Model;
using MindMate.Plugins.Tasks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class TaskCompleteIcon : ISystemIcon
    {
        public TaskCompleteIcon(CompletedTaskList taskList)
        {
            taskList.TaskChanged += (n, e) =>
            {
                if (StatusChange == null) return;

                switch(e.TaskChange)
                {
                    case CompletedTaskChange.TaskCompleted:
                        StatusChange(n, this, SystemIconStatusChange.Show);
                        break;
                    case CompletedTaskChange.TaskRemoved:
                        StatusChange(n, this, SystemIconStatusChange.Hide);
                        break;
                }
            };
        }
        
        public string Name { get { return "TaskCompleted"; } }

        public bool ShowIcon(MapNode node)
        {
            return node.IsTaskComplete();
        }

        public event Action<MapNode, ISystemIcon, SystemIconStatusChange> StatusChange;

        public System.Drawing.Bitmap Bitmap
        {
            get { return TaskRes.tick; }
        }
        
    }
}
