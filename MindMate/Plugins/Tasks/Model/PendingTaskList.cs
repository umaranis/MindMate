using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MindMate.Plugins.Tasks.Model
{
    /// <summary>
    /// List of Pending Tasks with Due Date in sorted order.
    /// List keeps itself updated by listening with MapTree events.
    /// Collection  doesn't change any MapNode attributes.
    /// If due date of a task is changed, it should be removed and re-added to maintain sort order.
    /// </summary>
    public class PendingTaskList : BaseTaskList
    {
        
        public PendingTaskList() : base(n => n.GetDueDate())
        {
            pendingTaskArgs = new PendingTaskEventArgs();
        }
        public void RegisterMap(MapTree tree)
        {
            tree.AttributeChanged += Tree_AttributeChanged;

            tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged += Tree_NodePropertyChanged;
            tree.TreeStructureChanged += Tree_TreeStructureChanged;
        }

        public void UnregisterMap(MapTree tree)
        {
            tree.AttributeChanged -= Tree_AttributeChanged;

            tree.SelectedNodes.NodeSelected -= SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected -= SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged -= Tree_NodePropertyChanged;
            tree.TreeStructureChanged -= Tree_TreeStructureChanged;

            Clear(tree);
        }

        private void Clear(MapTree tree)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                MapNode node = this[i];
                if (node.Tree == tree)
                {
                    RemoveAt(i);
                    TaskChanged(node, new PendingTaskEventArgs() { TaskChange = PendingTaskChange.TaskRemoved, OldDueDate = node.GetDueDate(), OldTaskStatus = node.GetTaskStatus() });
                }
            }
        }
                
        private PendingTaskEventArgs GetEventArgs(MapNode node, PendingTaskChange change, AttributeChangeEventArgs e)
        {
            pendingTaskArgs.TaskChange = change;
            pendingTaskArgs.OldTaskStatus = (e.AttributeSpec.IsTaskStatus() && e.oldValue != null) ?
                (TaskStatus)Enum.Parse(typeof(TaskStatus), e.oldValue) : node.GetTaskStatus();
            if (e.AttributeSpec.IsDueDate())
            {
                pendingTaskArgs.OldDueDate = e.oldValue == null ? DateTime.MinValue : DateHelper.ToDateTime(e.oldValue);
            }
            else
            {
                pendingTaskArgs.OldDueDate = node.DueDateExists() ? node.GetDueDate() : DateTime.MinValue;
            }

            return pendingTaskArgs;
        }

        private void Tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            //// Task List Change
            // task added
            if (e.ChangeType == AttributeChange.Added && e.AttributeSpec.IsDueDate() && !node.IsTaskComplete()) 
            {
                Add(node);
                TaskChanged(node, GetEventArgs(node, PendingTaskChange.TaskAdded, e));                
            }
            // task removed
            else if (e.ChangeType == AttributeChange.Removed && e.AttributeSpec.IsDueDate() && !node.IsTaskComplete()) 
            {
                if (Remove(node))
                {
                    TaskChanged(node, GetEventArgs(node, PendingTaskChange.TaskRemoved, e));
                }                
            }
            // task completed
            else if (e.ChangeType == AttributeChange.Added && e.AttributeSpec.IsCompletionDate() && node.DueDateExists()) 
            {
                if (Remove(node))
                {
                    TaskChanged(node, GetEventArgs(node, PendingTaskChange.TaskCompleted, e));
                }              
            }
            // task reopened
            else if (e.ChangeType == AttributeChange.Removed && e.AttributeSpec.IsCompletionDate() && node.DueDateExists()) 
            {
                Add(node);
                TaskChanged(node, GetEventArgs(node, PendingTaskChange.TaskReopened, e));
            }
            // task due date updated
            else if(e.ChangeType == AttributeChange.ValueUpdated && e.AttributeSpec.IsDueDate() && !node.IsTaskComplete())
            {
                Remove(node);
                Add(node);
                TaskChanged(node, GetEventArgs(node, PendingTaskChange.DueDateUpdated, e));
            }

            //// Task Property Change (which doesn't affect list)
            else if(PendingTaskPropertyChanged != null && e.AttributeSpec.IsStartDate() && node.IsTaskPending())
            {
                var args = new PendingTaskPropertyEventArgs() { PropertyChanged = PendingTaskProperty.StartDate };
                if (!string.IsNullOrEmpty(e.oldValue)) args.OldValue = DateHelper.ToDateTime(e.oldValue);
                PendingTaskPropertyChanged(node, args);
            }

        }        
        
        private void Tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            if (TaskTextChanged == null) return;

            if (e.ChangedProperty == NodeProperties.Text)
            {
                // update task title
                if (node.DueDateExists())
                {
                    TaskTextChanged(node, new TaskTextEventArgs() { ChangeType = TaskTextChange.TextChange, OldText = (string)e.OldValue });
                }

                // update task parent
                if(node.HasChildren)
                {
                    foreach(MapNode n in this)
                    {
                        if (n.IsDescendent(node))
                            TaskTextChanged(n, new TaskTextEventArgs() { ChangeType = TaskTextChange.AncestorTextChange, ChangedAncestor = node, OldText = (string)e.OldValue });
                    }
                }
            }
        }

        private void Tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs e)
        {
            if (e.ChangeType == TreeStructureChange.Deleted || e.ChangeType == TreeStructureChange.Detached)
            {
                node.ForEach(n =>
                    {
                        if (n.IsTaskPending())
                        {
                            Remove(n);
                            var evtTask = new PendingTaskEventArgs();
                            evtTask.TaskChange = PendingTaskChange.TaskRemoved;
                            if (n.DueDateExists()) evtTask.OldDueDate = n.GetDueDate();
                            evtTask.OldTaskStatus = n.GetTaskStatus();
                            TaskChanged(n, evtTask);
                        }
                    });
            }
            else if (e.ChangeType == TreeStructureChange.Attached)
            {
                node.ForEach((n) =>
                {
                    if (n.IsTaskPending())
                    {
                        Add(n);
                        var evtTask = new PendingTaskEventArgs();
                        evtTask.TaskChange = PendingTaskChange.TaskAdded;
                        if (n.DueDateExists()) evtTask.OldDueDate = n.GetDueDate();
                        evtTask.OldTaskStatus = n.GetTaskStatus();
                        TaskChanged(n, evtTask);
                    }
                });
            }
        }

        private void SelectedNodes_NodeSelected(MapNode node, SelectedNodes selectedNodes)
        {
            if (TaskSelectionChanged == null) return;

            if (node.IsTaskPending())
            {
                TaskSelectionChanged(node, new TaskSelectionEventArgs() { ChangeType = TaskSelectionChange.Selected });
            }
        }

        private void SelectedNodes_NodeDeselected(MapNode node, SelectedNodes selectedNodes)
        {
            if (TaskSelectionChanged == null) return;

            if (node.IsTaskPending())
            {
                TaskSelectionChanged(node, new TaskSelectionEventArgs() { ChangeType = TaskSelectionChange.Deselected });
            }
        }

        /// <summary>
        /// Subscribing to this event can be performance intensive if many tasks are there. 
        /// For every MapNode text change, it traverses through the task list to check if the changed MapNode is ancestor of any task.
        /// If no one subscribes, than no overhead is incurred.
        /// </summary>
        public event TaskTextChangedDelegate TaskTextChanged;

        public event TaskSelectionChangedDelegate TaskSelectionChanged;

        private PendingTaskEventArgs pendingTaskArgs;
        public event PendingTaskChangedDelegate TaskChanged = delegate { };

        event PendingTaskPropertyChanged PendingTaskPropertyChanged;

    }
}
