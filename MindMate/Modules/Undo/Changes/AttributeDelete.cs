using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeDelete : IChange
    {
        MapNode node;
        MapNode.Attribute attribute;

        public AttributeDelete(MapNode node, MapNode.Attribute attribute)
        {
            this.node = node;
            this.attribute = attribute;
        }

        public string Description
        {
            get { return "Delete Attribute"; }
        }

        public void Undo()
        {
            node.AddAttribute(attribute);
        }
    }
}
