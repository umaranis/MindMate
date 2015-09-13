using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecVisibility : IChange
    {
        MapTree.AttributeSpec spec;
        bool oldValue;

        public AttributeSpecVisibility(MapTree.AttributeSpec spec, bool oldValue)
        {
            this.spec = spec;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get { return "Attribute Spec Visibility"; }
        }

        public void Undo()
        {
            spec.Visible = oldValue;
        }
    }
}
