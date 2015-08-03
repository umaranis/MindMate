using MindMate.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    public class BaseTaskList : IComparer<MapNode>, IEnumerable<MapNode>/*, IList<MapNode>*/
    {
        protected List<MapNode> tasks;

        private Func<MapNode, DateTime> GetDate;

        public BaseTaskList(Func<MapNode, DateTime> GetDate)
        {
            tasks = new List<MapNode>();
            this.GetDate = GetDate;
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

        //public bool IsReadOnly
        //{
        //    get
        //    {
        //        return ((IList<MapNode>)tasks).IsReadOnly;
        //    }
        //}

        protected void Add(MapNode item)
        {
            int index = tasks.BinarySearch(item, this);
            if (index > -1)
                tasks.Insert(index + 1, item);
            else
                tasks.Insert(~index, item);
        }

        public int Compare(MapNode x, MapNode y)
        {
            return DateTime.Compare(GetDate(x), GetDate(y));
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

        /// <summary>
        /// Returns the index of first task which is greater than given DateTime.
        /// Returns -1 if not found.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="includeEqualto"></param>
        /// <returns></returns>
        public int IndexOfGreaterThan(DateTime value, bool includeEqualto = false)
        {
            int lo = 0;
            int hi = 0 + tasks.Count - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = DateTime.Compare(GetDate(tasks[i]), value);

                if (order == 0)
                {
                    if (includeEqualto)
                        return IndexOfFirstEqualItem(i);
                    else
                        return IndexOfNextGreaterItem(i);
                }
                if (order < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            if (lo < Count)
                return lo;
            else
                return -1;
        }          
        
        /// <summary>
        /// Skips items with the same value to find the next item with greater value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int IndexOfNextGreaterItem(int index)
        {
            do
            {
                index++;
                if (index < tasks.Count && GetDate(tasks[index-1]) < GetDate(tasks[index]))
                    return index;
            } while (index < tasks.Count);

            return -1;
        }   

        /// <summary>
        /// Returns the first preceding item index that has same value as the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int IndexOfFirstEqualItem(int index)
        {
            while(index > 0)
            {
                if (GetDate(tasks[index]).Equals(GetDate(tasks[index - 1])))
                    index--;
                else
                    break;
            }
            return index;
        }

        protected void Insert(int index, MapNode item)
        {
            ((IList<MapNode>)tasks).Insert(index, item);
        }

        protected bool Remove(MapNode item)
        {
            return ((IList<MapNode>)tasks).Remove(item);
        }

        protected void RemoveAt(int index)
        {
            ((IList<MapNode>)tasks).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }
    }
}
