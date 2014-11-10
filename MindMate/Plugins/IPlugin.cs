using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins
{
    public interface IPlugin
    {
        void Initialize(PluginManager pluginMgr);

        /// <summary>
        /// Plugin to create and provide a list of menu items for MapNode context menu. Will be called only once by PluginManager.
        /// </summary>
        MenuItem[] CreateContextMenuItemsForNode();

        /// <summary>
        /// Plugin to create and provide a list of menu items for main menu. Will be called only once by PluginManager.
        /// </summary>
        void CreateMainMenuItems(out MenuItem[] menuItems, out MainMenuLocation location);

        /// <summary>
        /// Register for Node, SelectedNodes and Tree events
        /// </summary>
        /// <param name="tree"></param>
        void RegisterTreeEvents(MapTree tree);

        /// <summary>
        /// Plugin to unregister for events which were registers in RegisterTreeEvents method
        /// </summary>
        /// <param name="tree"></param>
        void UnregisterTreeEvents(MapTree tree);
        
    }

    public enum MainMenuLocation { File, Edit, Format, Tools, Help, Separate}
}
