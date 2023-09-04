using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecAdd : IChange
    {
        readonly MapTree.AttributeSpec spec;

        public AttributeSpecAdd(MapTree.AttributeSpec spec)
        {
            this.spec = spec;
        }

        public string Description => "Attribute Spec Added";

        public void Undo()
        {
            spec.Delete();
        }
    }
}
