using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeAdd : IChange
    {
        readonly MapNode node;
        MapNode.Attribute attribute;

        public AttributeAdd(MapNode node, MapNode.Attribute attribute)
        {
            this.node = node;
            this.attribute = attribute;
        }

        public string Description => "Add Attribute";

        public void Undo()
        {
            node.DeleteAttribute(attribute);
        }
    }
}
