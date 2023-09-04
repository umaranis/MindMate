using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class IconAdd : IChange
    {
        readonly MapNode node;
        readonly string icon;

        public IconAdd(MapNode node, string icon)
        {
            this.node = node;
            this.icon = icon;
        }

        public string Description => "Icon Added";

        public void Undo()
        {
            node.Icons.Remove(icon);
        }
    }
}
