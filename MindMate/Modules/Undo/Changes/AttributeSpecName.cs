using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecName : IChange
    {
        MapTree.AttributeSpec spec;
        string oldValue;

        public AttributeSpecName(MapTree.AttributeSpec spec, string oldValue)
        {
            this.spec = spec;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get { return "Attribute Spec Name"; }
        }

        public void Undo()
        {
            spec.Name = oldValue;
        }
    }
}
