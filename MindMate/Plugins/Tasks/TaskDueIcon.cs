using MindMate.MetaModel;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class TaskDueIcon : ISystemIcon
    {
        public string Name { get { return "TaskDueDate"; } }
        public bool ShowIcon(Model.MapNode node)
        {
            return TaskPlugin.DueDateAttribute.Exists(node);            
        }

        public event Action<Model.MapNode, ISystemIcon, SystemIconStatusChange> StatusChange;

        public System.Drawing.Bitmap Bitmap
        {
            get { return TaskRes.date; }
        }

        internal void FireStatusChangeEvent(MapNode node, SystemIconStatusChange change)
        {
            StatusChange(node, this, change);
        }
    }
}
