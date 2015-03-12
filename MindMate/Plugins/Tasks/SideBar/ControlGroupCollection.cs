using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks.SideBar
{
    public partial class SideBar
    {
        public class ControlGroupCollection : IList<ControlGroup>
        {
            private SideBar sideBar;

            public ControlGroupCollection(SideBar sideBar)
            {
                this.sideBar = sideBar;
            }            

            private TableLayoutPanel Table
            {
                get { return sideBar.tablePanelMain; }
            }

            public ControlGroup Add(string headerText, System.Drawing.Color headerTextColor)
            {
                ControlGroup newControlGroup = CreateControlGroup();
                newControlGroup.HeaderText = headerText;
                newControlGroup.HeaderTextColor = headerTextColor;

                Add(newControlGroup);

                return newControlGroup;
            }

            private ControlGroup CreateControlGroup()
            {
                ControlGroup newControlGroup = new ControlGroup();

                newControlGroup.SuspendLayout();

                newControlGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((
                    System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
                newControlGroup.AnimationInterval = 20;
                newControlGroup.BackColor = System.Drawing.Color.Transparent;
                newControlGroup.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
                newControlGroup.HeaderImage = null;
                //newControlGroup.HeaderText = "Overdue";
                //newControlGroup.HeaderTextColor = System.Drawing.Color.Red;
                newControlGroup.Location = new System.Drawing.Point(3, 3);
                //newControlGroup.Name = "collapsiblePanelOverdue";
                newControlGroup.RoundedCorners = false;
                newControlGroup.ShowHeaderSeparator = false;
                //newControlGroup.Size = new System.Drawing.Size(264, 151);
                newControlGroup.TabIndex = 2;
                newControlGroup.UseAnimation = true;
                newControlGroup.Visible = false;

                newControlGroup.ResumeLayout();                

                return newControlGroup;
            }

            #region IList<ControlGroup>

            public void Add(ControlGroup item)
            {
                Table.Controls.Add(item, 0, Table.RowCount);
                Table.RowCount += 1;

                item.ControlAdded += sideBar.OnControlAdded;
                item.ControlRemoved += sideBar.OnControlRemoved;
            }

            public void Insert(int index, ControlGroup item)
            {
                if (index > Table.RowCount)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    for (int i = Table.RowCount - 1; i >= index; i--)
                    {
                        Control ctrl = Table.GetControlFromPosition(0, i);
                        Table.SetRow(ctrl, i + 1);
                    }

                    Table.Controls.Add(item, 0, index);
                    Table.RowCount += 1;

                    item.ControlAdded += sideBar.OnControlAdded;
                    item.ControlRemoved += sideBar.OnControlRemoved;
                }
            }

            public ControlGroup this[int index]
            {
                get
                {
                    ControlGroup ctrlGroup = (ControlGroup)Table.GetControlFromPosition(0, index);

                    if(ctrlGroup == null)
                    {
                        ctrlGroup = (ControlGroup)Table.Controls[index];

                        if(Table.GetRow(ctrlGroup) != index)
                        {
                            foreach(Control c in Table.Controls)
                            {
                                if(Table.GetRow(c) == index)
                                {
                                    ctrlGroup = (ControlGroup)c;
                                    break;
                                }
                            }
                        }
                    }

                    return ctrlGroup;
                }
                set
                {
                    Insert(index, value);
                }
            }

            public IEnumerator<ControlGroup> GetEnumerator()
            {
                return CreateEnumerable().GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return CreateEnumerable().GetEnumerator();
            }

            private IEnumerable<ControlGroup> CreateEnumerable()
            {
                for (int i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }

            public bool Contains(ControlGroup item)
            {
                return Table.Controls.Contains(item);
            }

            public int IndexOf(ControlGroup item)
            {
                return Table.GetRow(item);
            }

            public void CopyTo(ControlGroup[] array, int arrayIndex)
            {
                foreach (ControlGroup t in this)
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

            public bool Remove(ControlGroup item)
            {
                int rowNum = Table.GetRow(item);

                return Remove(item, rowNum);
            }

            public void RemoveAt(int index)
            {
                ControlGroup item = (ControlGroup)Table.GetControlFromPosition(0, index);

                Remove(item, index);
            }

            private bool Remove(ControlGroup item, int rowNum)
            {
                if (rowNum > -1)
                {
                    Table.Controls.Remove(item);

                    item.ControlAdded -= sideBar.OnControlAdded;
                    item.ControlRemoved -= sideBar.OnControlRemoved;

                    Table.RowCount -= 1;

                    for (int i = rowNum; i < Table.RowCount; i++)
                    {
                        Table.SetRow(Table.GetControlFromPosition(0, i + 1), i); // move one row up
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            public void Clear()
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    RemoveAt(i);
                }
            }                    

            #endregion IList<ControlGroup>
        }
        
    }
}
