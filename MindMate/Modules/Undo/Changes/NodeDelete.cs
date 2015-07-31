using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class NodeDelete : IChange
    {
        MapNode node;

        public NodeDelete(MapNode node)
        {
            this.node = node;
        }

        public string Description
        {
            get
            {
                return "Node deleted";
            }
        }

        public void Undo()
        {
            if (node.Previous != null)
                node.AttachTo(node.Parent, node.Previous, true, node.Pos);
            else // insert as the first child
                node.AttachTo(node.Parent, node.Parent.FirstChild, false, node.Pos);
        }
    }
}
