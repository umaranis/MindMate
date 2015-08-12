using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ShapeChange : IChange
    {
        MapNode node;
        NodeShape oldValue;

        public ShapeChange(MapNode node, NodeShape oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Node Shape Change";
            }
        }

        public void Undo()
        {
            node.Shape = oldValue;
        }
    }
}
