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

        void ScheduleTask(TaskSchedular.ITask task);    
    }
}
