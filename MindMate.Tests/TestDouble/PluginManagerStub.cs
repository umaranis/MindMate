using System;
using MindMate.Model;
using TaskScheduler;

namespace MindMate.Tests.TestDouble
{
    class PluginManagerStub : MindMate.Plugins.IPluginManager
    {
        public TaskScheduler.TaskScheduler TaskSchedular { get; set; }

        public void FocusMapEditor()
        {
            
        }       

        public void ScheduleTask(TaskScheduler.ITask task)
        {
            TaskSchedular?.AddTask(task);
        }

        public void RescheduleTask(ITask task, DateTime startTime)
        {
            TaskSchedular?.UpdateTask(task, startTime);
        }

        public MapTree ActiveTree { get; }

        public SelectedNodes ActiveNodes { get; }
    }
}
