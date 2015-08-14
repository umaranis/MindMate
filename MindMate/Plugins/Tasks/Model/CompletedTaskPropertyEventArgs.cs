using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    /// <summary>
    /// Arguments for event on change of Task properties other than Text, CompletionDate or Selected.
    /// <seealso cref="TaskTextEventArgs"/>, <seealso cref="CompletedTaskEventArgs"/>, <seealso cref="TaskSelectionEventArgs"/>
    /// </summary>
    class CompletedTaskPropertyEventArgs
    {
        public CompletedTaskProperty PropertyChanged { get; set; }

        public object OldValue { get; set; }
    }

    enum CompletedTaskProperty { StartDate, DueDate }

    delegate void CompletedTaskPropertyChanged(MapNode node, CompletedTaskPropertyEventArgs args);
}
