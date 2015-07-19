using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Reminder
{
    public enum ReminderType
    {
        /// <summary>
        /// Reminder not issued
        /// </summary>
        None,
        /// <summary>
        /// First reminder issued before the task is due
        /// </summary>
        First,
        /// <summary>
        /// Final reminder issued as task is due
        /// </summary>
        Final
    }
}
