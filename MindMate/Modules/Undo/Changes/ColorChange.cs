using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ColorChange : IChange
    {
        readonly MapNode node;
        readonly Color OldColor;

        public ColorChange(MapNode node, Color oldValue)
        {
            this.node = node;
            this.OldColor = oldValue;
        }

        public string Description
        {
            get
            {
                return "Text Color Change";
            }
        }

        public void Undo()
        {
            node.Color = OldColor;
        }
    }
}
