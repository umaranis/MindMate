using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeUpdate : IChange
    {
        readonly MapNode node;
        MapNode.Attribute attribute;

        public AttributeUpdate(MapNode node, MapNode.Attribute oldAttribute)
        {
            this.node = node;
            this.attribute = oldAttribute;
        }

        public string Description => "Update Attribute";

        public void Undo()
        {
            node.UpdateAttribute(attribute.AttributeSpec, attribute.ValueString);
        }
    }
}
