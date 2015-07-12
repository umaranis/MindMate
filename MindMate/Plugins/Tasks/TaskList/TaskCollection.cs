using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MindMate.Plugins.Tasks.Model;

namespace MindMate.Plugins.Tasks
{
    /// <summary>
    /// List of Tasks in sorted order.
    /// If due date of a task is changed, it should be removed and re-added to maintain sort order.
    /// </summary>
    public class TaskCollection : IComparer<MapNode>, IEnumerable<MapNode>/*, IList<MapNode>*/
    {
        private List<MapNode> tasks;

        public TaskCollection()
        {
            tasks = new List<MapNode>();
        }

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

        public bool IsReadOnly
        {
            get
            {
                return ((IList<MapNode>)tasks).IsReadOnly;
            }
        }

        public void Add(MapNode item)
        {
            int index = tasks.BinarySearch(item, this);
            if (index > -1)
                tasks.Insert(index + 1, item);
            else
                tasks.Insert(~index, item);
        }

        public void Clear(MapTree tree)
        {
            tasks.RemoveAll(node => node.Tree == tree);
        }

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
            return ((IList<MapNode>)tasks).IndexOf(item);
        }

        private void Insert(int index, MapNode item)
        {
            ((IList<MapNode>)tasks).Insert(index, item);
        }

        public bool Remove(MapNode item)
        {
            return ((IList<MapNode>)tasks).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<MapNode>)tasks).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }
        
    }
}
