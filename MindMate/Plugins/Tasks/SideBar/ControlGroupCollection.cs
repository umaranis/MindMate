using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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

            public ControlGroup Add(string headerText, System.Drawing.Color headerTextColor)
            {
                ControlGroup newControlGroup = CreateControlGroup();
                newControlGroup.HeaderText = headerText;
                newControlGroup.HeaderTextColor = headerTextColor;

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

                sideBar.tablePanelMain.Controls.Add(newControlGroup, 0, sideBar.tablePanelMain.RowCount);
                sideBar.tablePanelMain.RowCount++;

                return newControlGroup;
            }

            #region IList<ControlGroup>

            public int IndexOf(ControlGroup item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, ControlGroup item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            public ControlGroup this[int index]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Add(ControlGroup item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(ControlGroup item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(ControlGroup[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(ControlGroup item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<ControlGroup> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            #endregion IList<ControlGroup>
        }
    }
}
