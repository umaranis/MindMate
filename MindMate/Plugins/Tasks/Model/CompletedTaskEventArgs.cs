using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    public class CompletedTaskEventArgs : EventArgs
    {
        public CompletedTaskChange TaskChange { get; set; }

        /// <summary>
        /// OldCompletionDate will be same as current if there is no change.
        /// Due to batch updates, OldCompletionDate may be different than current even if TaskChange is not CompletionDateUpdated.
        /// </summary>
        public DateTime OldCompletionDate { get; set; }
        public bool IsOldCompletionDateEmpty { get { return DateTime.MinValue.Equals(OldCompletionDate); } }
        /// <summary>
        /// OldTaskStatus will be same as current if there is no change.
        /// </summary>
        public TaskStatus OldTaskStatus { get; set; }
    }

    public enum CompletedTaskChange
    {
        TaskCompleted,
        CompletionDateUpdated,
        TaskRemoved
    }

    public delegate void CompletedTaskChangedDelegate(MapNode node, CompletedTaskEventArgs args);
}
