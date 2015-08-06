using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MindMate.Plugins.Tasks.Model
{
    /// <summary>
    /// A wrapper around completed and pending tasks collection
    /// </summary>
    public class TaskList : IEnumerable<MapNode> /*, IList<MapNode> */
    {

        PendingTaskList pendingTasks;
        CompletedTaskList completedTasks;

        public TaskList(PendingTaskList plist, CompletedTaskList clist)
        {
            pendingTasks = plist;
            completedTasks = clist;
        }

        #region IList interface

        public MapNode this[int index]
        {
            get
            {
                if (index < pendingTasks.Count)
                    return pendingTasks[index];
                else
                    return completedTasks[index];
            }

            //set
            //{
            //    throw new NotImplementedException();
            //}
        }

        public int Count
        {
            get
            {
                return pendingTasks.Count + completedTasks.Count;
            }
        }

        //public bool IsReadOnly
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public void Add(MapNode item)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Clear()
        //{
        //    throw new NotImplementedException();
        //}

        public bool Contains(MapNode item)
        {
            return pendingTasks.Contains(item) || completedTasks.Contains(item);
        }

        //public void CopyTo(MapNode[] array, int arrayIndex)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerator<MapNode> GetEnumerator()
        {
            foreach (MapNode node in pendingTasks)
            {
                yield return node;
            }
            foreach (MapNode node in completedTasks)
            {
                yield return node;
            }
        }

        public int IndexOf(MapNode item)
        {
            int index = pendingTasks.IndexOf(item);
            if (index < 0)
                index = completedTasks.IndexOf(item);

            return index;
        }

        //public void Insert(int index, MapNode item)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool Remove(MapNode item)
        //{
        //    item.RemoveTask();
        //    return true;
        //}

        //public void RemoveAt(int index)
        //{
        //    throw new NotImplementedException();
        //}

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (MapNode node in pendingTasks)
            {
                yield return node;
            }
            foreach (MapNode node in completedTasks)
            {
                yield return node;
            }
        }
        #endregion IList<MapNode> interface

        public IEnumerable<MapNode> GetTasksBetween(DateTime startDate, DateTime endDate)
        {
            int i = pendingTasks.IndexOfGreaterThan(startDate, true);

            if(i >= 0)
            {
                for(; i < pendingTasks.Count; i++)
                {
                    MapNode node = pendingTasks[i];
                    if (DateHelper.DateIntersects(startDate, endDate, node.GetStartDate(), node.GetEndDate()))
                        yield return node;
                }
            }

            i = completedTasks.IndexOfGreaterThan(startDate, true);

            if (i >= 0)
            {
                for (; i < completedTasks.Count; i++)
                {
                    MapNode node = completedTasks[i];
                    if (DateHelper.DateIntersects(startDate, endDate, node.GetStartDate(), node.GetEndDate()))
                        yield return node;
                }
            }
        }

        #region Events

        public class TaskChangeEventArgs {  }

        public delegate void TaskChangedDelegate(MapNode node, TaskChangeEventArgs args);

        public event TaskChangedDelegate TaskChanged;
        
        internal void RegisterMap(MapTree tree)
        {
            tree.AttributeChanged += Tree_AttributeChanged;
        }        

        internal void UnregisterMap(MapTree tree)
        {
            tree.AttributeChanged -= Tree_AttributeChanged;
        }

        private void Tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            if(TaskChanged != null &&
                (
                e.AttributeSpec.IsCompletionDate() ||
                e.AttributeSpec.IsDueDate() ||
                e.AttributeSpec.IsStartDate()
                ))
            {
                TaskChanged(node, new TaskChangeEventArgs());
            }
        }

        public event TaskTextChangedDelegate TaskTextChanged
        {
            add
            {
                pendingTasks.TaskTextChanged += value;
                completedTasks.TaskTextChanged += value;
            }
            remove
            {
                pendingTasks.TaskTextChanged -= value;
                completedTasks.TaskTextChanged -= value;
            }
        }

        public event TaskSelectionChangedDelegate TaskSelectionChanged
        {
            add
            {
                pendingTasks.TaskSelectionChanged += value;
                completedTasks.TaskSelectionChanged += value;
            }
            remove
            {
                pendingTasks.TaskSelectionChanged -= value;
                completedTasks.TaskSelectionChanged -= value;
            }
        }

        #endregion Events
    }
}
