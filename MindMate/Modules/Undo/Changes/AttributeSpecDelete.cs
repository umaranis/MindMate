using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecDelete : IChange
    {
        MapTree.AttributeSpec spec;

        public AttributeSpecDelete(MapTree.AttributeSpec spec)
        {
            this.spec = spec;
        }

        public string Description
        {
            get { return "Attribute Spec Delete"; }
        }

        public void Undo()
        {
            SortedSet<string> values = null;
            if (spec.Values != null) { values = new SortedSet<string>(spec.Values); }

            new MapTree.AttributeSpec(spec.Tree, spec.Name, spec.Visible, spec.DataType, spec.ListType, values, spec.Type);
            
        }
    }
}
