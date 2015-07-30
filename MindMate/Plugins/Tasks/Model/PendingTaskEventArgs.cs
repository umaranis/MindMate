using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    public class PendingTaskEventArgs : EventArgs
    {
        public PendingTaskChange TaskChange { get; set; }

        /// <summary>
        /// OldDueDate will be same as current if there is no change.
        /// Due to batch updates, OldDueDate may be different than current even if PendingTaskChange is not DueDateUpdated.
        /// </summary>
        public DateTime OldDueDate { get; set; }
        public bool IsOldDueDateEmpty { get { return DateTime.MinValue.Equals(OldDueDate); } }
        /// <summary>
        /// OldTaskStatus will be same as current if there is no change.
        /// </summary>
        public TaskStatus OldTaskStatus { get; set; }        
    }

    public enum PendingTaskChange
    {
        TaskAdded,
        TaskReopened,
        TaskCompleted,
        TaskRemoved,
        DueDateUpdated

    }

    public delegate void PendingTaskChangedDelegate(MapNode node, PendingTaskEventArgs args);
}
