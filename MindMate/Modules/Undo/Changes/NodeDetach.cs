using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class NodeDetach : IChange
    {
        readonly MapNode node;
        readonly MapNode parent;
        readonly MapNode siblingAbove;
        readonly NodePosition position;

        public NodeDetach(MapNode node)
        {
            this.node = node;
            this.parent = node.Parent;
            this.siblingAbove = node.Previous;
            this.position = node.Pos;
        }

        public string Description => "Node detached";

        public void Undo()
        {
            if (siblingAbove != null) //attach after sibling
                node.AttachTo(parent, siblingAbove, true, position);
            else if (parent.FirstChild != null) //insert as the first child
                node.AttachTo(parent, parent.FirstChild, false, position);
            else //attach as only child
                node.AttachTo(parent, null, true, position);
        }
    }
}
