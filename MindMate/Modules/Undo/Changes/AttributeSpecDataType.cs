﻿using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class AttributeSpecDataType : IChange
    {
        readonly MapTree.AttributeSpec spec;
        readonly MapTree.AttributeDataType oldValue;

        public AttributeSpecDataType(MapTree.AttributeSpec spec, MapTree.AttributeDataType oldValue)
        {
            this.spec = spec;
            this.oldValue = oldValue;
        }

        public string Description => "Attribute Spec DataType";

        public void Undo()
        {
            spec.DataType = oldValue;
        }
    }
}
