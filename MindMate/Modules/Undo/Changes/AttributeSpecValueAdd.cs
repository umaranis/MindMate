using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecValueAdd : IChange
    {
        MapTree.AttributeSpec spec;
        string value;

        public AttributeSpecValueAdd(MapTree.AttributeSpec spec, string value)
        {
            this.spec = spec;
            this.value = value;
        }

        public string Description
        {
            get { return "Attribute Spec Value Add"; }
        }

        public void Undo()
        {
            spec.RemoveValue(value);
        }
    }
}
