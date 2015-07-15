using MindMate.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskPlugin : IPluginSystemIcon
    {
        private ISystemIcon[] systemIcons;

        public ISystemIcon[] CreateSystemIcons()
        {
            systemIcons = new ISystemIcon[] {
                new TaskDueIcon(pendingTasks),
                new TaskCompleteIcon(completedTasks)
            };

            return systemIcons;
        }

        private TaskDueIcon TaskDueIcon { get { return (TaskDueIcon)systemIcons[0]; } }

        private TaskCompleteIcon TaskCompleteIcon { get { return (TaskCompleteIcon)systemIcons[1]; } }
    }
}
