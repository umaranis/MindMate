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
        /// <summary>
        /// This is the first method called on a plugin.
        /// </summary>
        /// <param name="pluginMgr"></param>
        void Initialize(IPluginManager pluginMgr);

        /// <summary>
        /// Called when application launching is completed and application is ready (main form loaded with MapTree)
        /// </summary>
        void OnApplicationReady();
                
        /// <summary>
        /// Plugin to create and provide a list of controls to be displayed in the side bar.
        /// 
        /// Name of Side Bar will be set from Control.Text property.
        /// </summary>
        /// <returns>List of controls. Each control will be shown in a separate tab in Side Bar</returns>
        Control[] CreateSideBarWindows();

        /// <summary>
        /// Register for Node, SelectedNodes and Tree events.
        /// Method is called before any nodes are added to the Tree.
        /// </summary>
        /// <param name="tree"></param>
        void OnCreatingTree(MapTree tree);

        /// <summary>
        /// Plugin to unregister for events which were registers in RegisterTreeEvents method. 
        /// </summary>
        /// <param name="tree"></param>
        void OnDeletingTree(MapTree tree);
        
    }

    public enum MainMenuLocation { File, Edit, Format, Tools, Help, Separate}
}
