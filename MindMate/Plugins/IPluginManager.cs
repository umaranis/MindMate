using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;

namespace MindMate.Plugins
{
    public interface IPluginManager
    {
        void FocusMapEditor();

        void ScheduleTask(TaskScheduler.ITask task);

        void RescheduleTask(TaskScheduler.ITask task, DateTime startTime);

        /// <summary>
        /// Currently active MapTree
        /// </summary>
        MapTree ActiveTree { get; }

        /// <summary>
        /// Selected Nodes of the currently active MapTree
        /// </summary>
        SelectedNodes ActiveNodes { get; }
    }
}
