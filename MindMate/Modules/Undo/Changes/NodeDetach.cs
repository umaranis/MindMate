using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class NodeDetach : IChange
    {
        MapNode node;
        MapNode parent;
        MapNode siblingAbove;

        public NodeDetach(MapNode node)
        {
            this.node = node;
            this.parent = node.Parent;
            this.siblingAbove = node.Previous;
        }

        public string Description
        {
            get
            {
                return "Node detached";
            }
        }

        public void Undo()
        {
            if (siblingAbove != null)
                node.AttachTo(parent, siblingAbove, true, node.Pos);
            else // insert as the first child
                node.AttachTo(parent, parent.FirstChild, false, node.Pos);
        }
    }
}
