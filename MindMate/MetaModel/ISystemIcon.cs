using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.MetaModel
{
    /// <summary>
    /// System icon is an icon displayed on a MapNode by the system based on certain condition. 
    /// It is different from Icons which are selected by the users.
    /// Example: Icon for node with Link or Note.
    /// </summary>
    public interface ISystemIcon : IIcon
    {
        /// <summary>
        /// Whether the icon should be displayed on given node or not.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>true if icon is to be displayed</returns>
        bool ShowIcon(MapNode node);

        /// <summary>
        /// Raised when System Icon status changes for a node. This happens when:
        /// 1- Icon is no more applicable to the node and should be hidden.
        /// 2- Icon is now applicable to the node and should be displayed.
        /// Views should listen to this event to make appropriate changes.
        /// View should verify that MapNode belongs to its Tree before taking any action (if multiple maps are open all will get notification)
        /// </summary>
        event Action<MapNode, ISystemIcon, SystemIconStatusChange> StatusChange;

        
    }

    public enum SystemIconStatusChange { Show, Hide }
}
