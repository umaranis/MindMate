using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class FontSizeChange : IChange
    {
        MapNode node;
        float OldValue;

        public FontSizeChange(MapNode node, float oldValue)
        {
            this.node = node;
            this.OldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Font Size Change";
            }
        }

        public void Undo()
        {
            node.FontSize = OldValue;
        }
    }
}
