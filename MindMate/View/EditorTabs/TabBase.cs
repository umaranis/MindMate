using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.EditorTabs
{
    public class TabBase : TabPage
    {
        public TabBase(Control control)
        {
            Controls.Add(control);
        }

        public virtual void Close()
        {
            ((TabControl)this.Parent).TabPages.Remove(this);
        }

        /// <summary>
        /// Child Control of the tab. All tabs should have one control only.
        /// </summary>
        public Control Control { get { return Controls[0]; } }
    }
}
