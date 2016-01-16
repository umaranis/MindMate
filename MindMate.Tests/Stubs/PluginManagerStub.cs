using System;
using MindMate.Model;
using TaskSchedular;

namespace MindMate.Tests.Stubs
{
    class PluginManagerStub : MindMate.Plugins.IPluginManager
    {
        public TaskSchedular.TaskSchedular TaskSchedular { get; set; }

        public void FocusMapEditor()
        {
            
        }       

        public void ScheduleTask(TaskSchedular.ITask task)
        {
            TaskSchedular?.AddTask(task);
        }

        public void RescheduleTask(ITask task, DateTime startTime)
        {
            TaskSchedular?.UpdateTask(task, startTime);
        }

        public SelectedNodes ActiveNodes { get; }
    }
}
