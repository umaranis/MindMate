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
            MapTree.AttributeSpec aspec = TaskPlugin.GetDueDateAttributeSpec(node.Tree);
            if (aspec != null)
                return node.ContainsAttribute(aspec);
            else
                return false;
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
