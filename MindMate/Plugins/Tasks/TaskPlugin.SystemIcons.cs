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
                new TaskDueIcon()
            };

            return systemIcons;
        }

        private TaskDueIcon TaskDueIcon { get { return (TaskDueIcon)systemIcons[0]; } }
    }
}
