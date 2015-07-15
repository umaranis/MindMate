using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    public class TaskTextEventArgs
    {
        public TaskTextChange ChangeType { get; set; }
        public string OldText { get; set; }
        public MapNode ChangedAncestor { get; set; }
    }

    public enum TaskTextChange { TextChange, AncestorTextChange }

    public delegate void TaskTextChangedDelegate(MapNode node, TaskTextEventArgs e);

    public class TaskSelectionEventArgs
    {
        public TaskSelectionChange ChangeType { get; set; }
    }

    public enum TaskSelectionChange { Selected, Deselected }

    public delegate void TaskSelectionChangedDelegate(MapNode node, TaskSelectionEventArgs e);
}
