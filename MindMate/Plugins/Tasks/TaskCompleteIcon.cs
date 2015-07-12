using MindMate.MetaModel;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class TaskCompleteIcon : ISystemIcon
    {
        public bool ShowIcon(MapNode node)
        {
            return node.CompletionDateExists();
        }

        public event Action<MapNode, ISystemIcon, SystemIconStatusChange> StatusChange;

        public System.Drawing.Bitmap Bitmap
        {
            get { return TaskRes.tick; }
        }

        public string Name
        {
            get { return "TaskCompleted"; }
        }

        internal void FireStatusChangeEvent(MapNode node, SystemIconStatusChange change)
        {
            StatusChange(node, this, change);
        }
    }
}
