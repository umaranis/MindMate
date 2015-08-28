using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeUpdate : IChange
    {
        MapNode node;
        MapNode.Attribute attribute;

        public AttributeUpdate(MapNode node, MapNode.Attribute oldAttribute)
        {
            this.node = node;
            this.attribute = oldAttribute;
        }

        public string Description
        {
            get { return "Update Attribute"; }
        }

        public void Undo()
        {
            node.UpdateAttribute(attribute.AttributeSpec, attribute.ValueString);
        }
    }
}
