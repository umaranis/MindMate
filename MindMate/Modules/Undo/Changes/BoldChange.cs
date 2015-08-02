using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class BoldChange : IChange
    {
        private MapNode node;
        private bool oldValue;

        public BoldChange(MapNode node, bool oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Toggle Bold";
            }
        }

        public void Undo()
        {
            node.Bold = oldValue;
        }
    }
}
