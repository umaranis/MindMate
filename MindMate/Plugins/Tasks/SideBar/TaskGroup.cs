using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks.SideBar
{
    public class TaskGroup<T> : CollapsiblePanel, IList<T> where T:Control
    {
        public TaskGroup()
        {
            TableLayoutPanel table = new TableLayoutPanel();

            this.Controls.Add(table);

            table.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            table.ColumnCount = 1;
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            table.Location = new System.Drawing.Point(0, 30);
            //table.Name = "tableLayout"  + this.HeaderText.Replace(' ','_');
            table.RowCount = 0;
            table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            table.Size = new System.Drawing.Size(264, 0);
            //table.TabIndex = 3;  
        }

        private TableLayoutPanel Table
        {
            get { return (TableLayoutPanel)this.Controls[1]; }
        }

              

        #region IList<T> interface

        public void Add(T item)
        {
            if (Table.RowCount == 0) this.Visible = true;

            Table.Controls.Add(item, 0, Table.RowCount);
            Table.RowCount += 1;

            Table.Height = Table.Height + item.Height + Table.Margin.Top + Table.Margin.Bottom;
            this.Height = Table.Height + Table.Top;            
        }

        public void Insert(int index, T item)
        {
            if(index > Table.RowCount)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                for (int i = Table.RowCount - 1; i >= index; i--)
                {
                    TaskView tv = (TaskView)Table.GetControlFromPosition(0, i);
                    Table.SetRow(tv, i + 1);                    
                }

                Table.Controls.Add(item, 0, index); 
                Table.RowCount += 1;

                Table.Height = Table.Height + item.Height + Table.Margin.Top + Table.Margin.Bottom;
                this.Height = Table.Height + Table.Top;    
            }
        }

        public T this[int index]
        {
            get
            {
                return (T)Table.GetControlFromPosition(0, index);
            }
            set
            {
                Insert(index, value);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return CreateEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CreateEnumerable().GetEnumerator();
        }

        private IEnumerable<T> CreateEnumerable()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return (T)Table.GetControlFromPosition(0, i);
            }
        }
                
        public bool Contains(T item)
        {
            return Table.Controls.Contains(item);
        }

        public int IndexOf(T item)
        {
            return Table.GetRow(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach(T t in this)
            {
                array[arrayIndex] = t;
                arrayIndex++;
            }
        }

        public int Count
        {
            get { return Table.RowCount; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int rowNum = Table.GetRow(item);

            return Remove(item, rowNum);
        }

        public void RemoveAt(int index)
        {
            T item = (T)Table.GetControlFromPosition(0, index);

            Remove(item, index);
        }

        private bool Remove(T item, int rowNum)
        {
            if (rowNum > -1)
            {
                Table.Controls.Remove(item);

                Table.RowCount -= 1;

                for (int i = rowNum; i < Table.RowCount; i++)
                {
                    Table.SetRow(Table.GetControlFromPosition(0, i + 1), i); // move one row up
                }

                Table.Height = Table.Height - item.Height - Table.Margin.Bottom - Table.Margin.Top;
                this.Height = Table.Height + Table.Top;

                if (Table.RowCount == 0) this.Visible = false;

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            for(int i = Count - 1; i >= 0; i--)
            {
                RemoveAt(i);
            }
        }
                
        #endregion IList<T> interface


        
    }
}
