using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class FontNameChange : IChange
    {
        MapNode node;
        string oldValue;

        public FontNameChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Font Name Change";
            }
        }

        public void Undo()
        {
            node.FontName = oldValue;
        }
    }
}
