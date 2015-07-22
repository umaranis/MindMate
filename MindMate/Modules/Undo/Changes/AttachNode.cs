using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class NodeAttach : IChange
    {
        private MapNode node;

        public NodeAttach(MapNode node)
        {
            this.node = node;
        }

        public string Description
        {
            get
            {
                return "Restore Node";
            }
        }

        public void Undo()
        {
            node.DeleteNode();
        }
    }
}
