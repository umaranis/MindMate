using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecType : IChange
    {
        MapTree.AttributeSpec spec;
        MapTree.AttributeType oldValue;

        public AttributeSpecType(MapTree.AttributeSpec spec, MapTree.AttributeType oldValue)
        {
            this.spec = spec;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get { return "Attribute Spec Type"; }
        }

        public void Undo()
        {
            spec.Type = oldValue;
        }
    }
}
