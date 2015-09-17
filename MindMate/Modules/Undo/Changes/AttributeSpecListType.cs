using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecListType : IChange
    {
        MapTree.AttributeSpec spec;
        MapTree.AttributeListOption oldValue;

        public AttributeSpecListType(MapTree.AttributeSpec spec, MapTree.AttributeListOption oldValue)
        {
            this.spec = spec;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get { return "Attribute Spec ListOption Change"; }
        }

        public void Undo()
        {
            spec.ListType = oldValue;
        }
    }
}
