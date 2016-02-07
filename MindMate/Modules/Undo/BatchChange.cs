using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo
{
    class BatchChange : IChange, IDisposable
    {
        private ChangeManager changeManager;

        public BatchChange(string changeDescription, ChangeManager changeManager)
        {
            this.changeManager = changeManager;
            Description = changeDescription;
        }

        private List<IChange> changes = new List<IChange>();

        public IList<IChange> Changes
        {
            get
            {
                return changes;
            }
        }

        public string Description
        {
            get; set;
        }

        public void Undo()
        {
            changeManager.StartBatch(Description);
            for(int i = changes.Count - 1; i >= 0; i--)
            {
                changes[i].Undo();
            }            
            changeManager.EndBatch();
        }

        public void Dispose()
        {
            changeManager.EndBatch();
        }
    }
}
