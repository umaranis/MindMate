using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecValueRemove : IChange
    {
        MapTree.AttributeSpec spec;
        string value;

        public AttributeSpecValueRemove(MapTree.AttributeSpec spec, string value)
        {
            this.spec = spec;
            this.value = value;
        }

        public string Description
        {
            get { return "Attribute Spec Value Remove"; }
        }

        public void Undo()
        {
            spec.AddValue(value);
        }
    }
}
