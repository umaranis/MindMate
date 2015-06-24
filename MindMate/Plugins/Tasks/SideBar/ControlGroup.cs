using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks.SideBar
{
    public class ControlGroup : CollapsiblePanel, IList<Control>
    {
        public ControlGroup()
        {
            this.Size = new System.Drawing.Size(264, 151);

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

        public new event ControlEventHandler ControlAdded
        {
            add
            {
                Table.ControlAdded += value;
            }
            remove
            {
                Table.ControlAdded -= value;
            }
        }

        public new event ControlEventHandler ControlRemoved
        {
            add
            {
                Table.ControlRemoved += value;
            }
            remove
            {
                Table.ControlRemoved -= value;
            }
        }

        private void AdjustHeightOnAdd(Control item)
        {
            Table.Height = Table.Height + item.Height + Table.Margin.Top + Table.Margin.Bottom + 2;
            this.Height = Table.Height + Table.Top;
        }

        private void AdjustHeightOnRemove(Control item)
        {
            Table.Height = Table.Height - item.Height - Table.Margin.Bottom - Table.Margin.Top - 2;
            this.Height = Table.Height + Table.Top;
        }


        #region IList<T> interface

        public void Add(Control item)
        {
            if (Table.RowCount == 0) this.Visible = true;

            AdjustHeightOnAdd(item);

            Table.RowCount += 1;

            Table.Controls.Add(item, 0, Table.RowCount - 1);                     
        }

        public void Insert(int index, Control item)
        {
            if(index > Table.RowCount)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                for (int i = Table.RowCount - 1; i >= index; i--)
                {
                    Control tv = Table.GetControlFromPosition(0, i);
                    Table.SetRow(tv, i + 1);                    
                }

                AdjustHeightOnAdd(item);

                Table.RowCount += 1;
                Table.Controls.Add(item, 0, index); 
                
                
            }
        }

        public Control this[int index]
        {
            get
            {
                return (Control)Table.GetControlFromPosition(0, index);
            }
            set
            {
                Insert(index, value);
            }
        }

        public IEnumerator<Control> GetEnumerator()
        {
            return CreateEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CreateEnumerable().GetEnumerator();
        }

        private IEnumerable<Control> CreateEnumerable()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return (Control)Table.GetControlFromPosition(0, i);
            }
        }
           
        // This method for IList<Control> is already available from Control class
        //public bool Contains(Control item)
        //{
        //    return Table.Controls.Contains(item);
        //}

        public int IndexOf(Control item)
        {
            return Table.GetRow(item);
        }

        public void CopyTo(Control[] array, int arrayIndex)
        {
            foreach(Control t in this)
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

        public bool Remove(Control item)
        {
            int rowNum = Table.GetRow(item);

            return Remove(item, rowNum);
        }

        public void RemoveAt(int index)
        {
            Control item = (Control)Table.GetControlFromPosition(0, index);

            Remove(item, index);
        }

        private bool Remove(Control item, int rowNum)
        {
            if (rowNum > -1)
            {
                AdjustHeightOnRemove(item);

                Table.RowCount -= 1;
                Table.Controls.Remove(item);                

                for (int i = rowNum; i < Table.RowCount; i++)
                {
                    Table.SetRow(Table.GetControlFromPosition(0, i + 1), i); // move one row up
                }
                              

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
                
        #endregion IList<Control> interface


        
    }
}
