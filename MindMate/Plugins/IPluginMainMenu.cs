using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins
{
    public interface IPluginMainMenu
    {
        /// <summary>
        /// Plugin to create and provide a list of menu items for main menu. 
        /// This method will be called only once by PluginManager.
        /// </summary>
        MainMenuItem[] CreateMainMenuItems();
        
    }
}
