using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecType : IChange
    {
        readonly MapTree.AttributeSpec spec;
        readonly MapTree.AttributeType oldValue;

        public AttributeSpecType(MapTree.AttributeSpec spec, MapTree.AttributeType oldValue)
        {
            this.spec = spec;
            this.oldValue = oldValue;
        }

        public string Description => "Attribute Spec Type";

        public void Undo()
        {
            spec.Type = oldValue;
        }
    }
}
