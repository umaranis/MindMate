using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class FoldChange : IChange
    {
        MapNode node;
        bool oldValue;

        public FoldChange(MapNode node, bool oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                if (oldValue)
                    return "Node Unfolded";
                else
                    return "Node folded";
            }
        }

        public void Undo()
        {
            node.Folded = oldValue;
        }
    }
}
