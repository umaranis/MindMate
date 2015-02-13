using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins
{
    /// <summary>
    /// Plugin interface to add menu items to Map Node context menu.
    /// </summary>
    public interface IPluginMapNodeContextMenu
    {
        /// <summary>
        /// Plugin to create and provide a list of menu items for MapNode context menu. Will be called only once by PluginManager.
        /// </summary>
        MenuItem[] CreateContextMenuItemsForNode();


        void OnContextMenuOpening(SelectedNodes nodes);
    }
}
