using MindMate.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View
{


    
    
    public interface ISideBarControl
    {

        /// <summary>
        /// Invoked when SideTabControl gets focus if it doesn't need it.
        /// Some of the tabs don't need keyboard focus.
        /// </summary>
        event EventHandler GotExtraFocus;

        ISearchControl SearchControl { get; }

        void SelectTab(string tabTitle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control">should be a control (not Tab object). The control displayed using DockStyle.Fill</param>
        void AddTab(object control);

        INoteEditor NoteEditor { get; }
    }
}
