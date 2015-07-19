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
    /// Collection is only aware about DueDate and TaskStatus attributes and it doesn't change any MapNode attributes.
    /// If due date of a task is changed, it should be removed and re-added to maintain sort order.
    /// </summary>
    public class PendingTaskList : IComparer<MapNode>, IEnumerable<MapNode>/*, IList<MapNode>*/
    {
        private List<MapNode> tasks;

        public PendingTaskList()
        {
            tasks = new List<MapNode>();
            pendingTaskArgs = new PendingTaskEventArgs();
        }
        public void RegisterMap(MapTree tree)
        {
            tree.AttributeChanged += Tree_AttributeChanged;
            tree.AttributeChanging += Tree_AttributeChanging;

            tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;

            tree.NodePropertyChanged += Tree_NodePropertyChanged;
            tree.TreeStructureChanged += Tree_TreeStructureChanged;
        }
        public void UnregisterMap(MapTree tree)
        {
            tree.AttributeChanged -= Tree_AttributeChanged;
            tree.AttributeChanging -= Tree_AttributeChanging;

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

        private void Tree_AttributeChanging(MapNode node, AttributeChangingEventArgs e)
        {
            if (e.AttributeSpec.IsTaskStatus() || e.AttributeSpec.IsDueDate())
            {
                pendingTaskArgs.OldTaskStatus = node.GetTaskStatus();
                if (node.DueDateExists())
                    pendingTaskArgs.OldDueDate = node.GetDueDate();
                else
                    pendingTaskArgs.OldDueDate = DateTime.MinValue;
            }
        }

        private void Tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            // task added
            if (e.ChangeType == AttributeChange.Added && e.AttributeSpec.IsDueDate() && node.GetTaskStatus() != TaskStatus.Complete) 
            {
                Add(node);
                pendingTaskArgs.TaskChange = PendingTaskChange.TaskAdded;
                TaskChanged(node, pendingTaskArgs);                
            }
            // task removed
            else if (e.ChangeType == AttributeChange.Removed && e.AttributeSpec.IsDueDate() && pendingTaskArgs.OldTaskStatus != TaskStatus.Complete) 
            {
                if (Remove(node))
                {
                    pendingTaskArgs.TaskChange = PendingTaskChange.TaskRemoved;
                    TaskChanged(node, pendingTaskArgs);
                }                
            }
            // task completed
            else if (e.AttributeSpec.IsTaskStatus() && node.GetTaskStatus() == TaskStatus.Complete && !pendingTaskArgs.IsOldDueDateEmpty) 
            {
                if (Remove(node))
                {
                    pendingTaskArgs.TaskChange = PendingTaskChange.TaskCompleted;
                    TaskChanged(node, pendingTaskArgs);
                }              
            }
            // task reopened
            else if (e.AttributeSpec.IsTaskStatus() && node.GetTaskStatus() != TaskStatus.Complete && pendingTaskArgs.OldTaskStatus == TaskStatus.Complete && !pendingTaskArgs.IsOldDueDateEmpty && node.DueDateExists()) 
            {
                Add(node);
                pendingTaskArgs.TaskChange = PendingTaskChange.TaskAdded;
                TaskChanged(node, pendingTaskArgs);
            }
            // task due date updated
            else if(e.ChangeType == AttributeChange.ValueUpdated && e.AttributeSpec.IsDueDate() && pendingTaskArgs.OldTaskStatus != TaskStatus.Complete && node.GetTaskStatus() != TaskStatus.Complete)
            {
                Remove(node);
                Add(node);
                pendingTaskArgs.TaskChange = PendingTaskChange.DueDateUpdated;
                TaskChanged(node, pendingTaskArgs);
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
                        if (n.isDescendent(node))
                            TaskTextChanged(n, new TaskTextEventArgs() { ChangeType = TaskTextChange.TextChange, ChangedAncestor = node, OldText = (string)e.OldValue });
                    }
                }
            }
        }

        private void Tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs e)
        {
            if (e.ChangeType == TreeStructureChange.Deleting || e.ChangeType == TreeStructureChange.Detaching)
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
                    if (n.GetTaskStatus() == TaskStatus.Open && n.DueDateExists())
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

            if (node.GetTaskStatus() == TaskStatus.Open && node.DueDateExists())
            {
                TaskSelectionChanged(node, new TaskSelectionEventArgs() { ChangeType = TaskSelectionChange.Selected });
            }
        }

        private void SelectedNodes_NodeDeselected(MapNode node, SelectedNodes selectedNodes)
        {
            if (TaskSelectionChanged == null) return;

            if (node.GetTaskStatus() == TaskStatus.Open && node.DueDateExists())
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
        

        #region IList<MapNode>

        public MapNode this[int index]
        {
            get
            {
                return ((IList<MapNode>)tasks)[index];
            }
            //set
            //{
            //    ((IList<MapNode>)tasks)[index] = value;
            //}
        }

        public int Count
        {
            get
            {
                return ((IList<MapNode>)tasks).Count;
            }
        }

        //public bool IsReadOnly
        //{
        //    get
        //    {
        //        return ((IList<MapNode>)tasks).IsReadOnly;
        //    }
        //}

        private void Add(MapNode item)
        {
            int index = tasks.BinarySearch(item, this);
            if (index > -1)
                tasks.Insert(index + 1, item);
            else
                tasks.Insert(~index, item);
        }
        /// <summary>
        /// Compares two tasks in terms of their DueDate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(MapNode x, MapNode y)
        {
            return DateTime.Compare(x.GetDueDate(), y.GetDueDate());            
        }

        public bool Contains(MapNode item)
        {
            return ((IList<MapNode>)tasks).Contains(item);
        }

        public void CopyTo(MapNode[] array, int arrayIndex)
        {
            ((IList<MapNode>)tasks).CopyTo(array, arrayIndex);
        }

        public IEnumerator<MapNode> GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        public int IndexOf(MapNode item)
        {
            return tasks.BinarySearch(item, this);
        }

        public int IndexOfGreaterThan(DateTime value, bool includeEqualto = false)
        {
            int lo = 0;
            int hi = 0 + tasks.Count - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = DateTime.Compare(tasks[i].GetDueDate(), value);

                if (order == 0) return i + (includeEqualto? 0 : 1);
                if (order < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return lo;
        }

        private void Insert(int index, MapNode item)
        {
            ((IList<MapNode>)tasks).Insert(index, item);
        }

        private bool Remove(MapNode item)
        {
            return ((IList<MapNode>)tasks).Remove(item);
        }

        private void RemoveAt(int index)
        {
            ((IList<MapNode>)tasks).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        #endregion #region IList<MapNode>

    }
}
