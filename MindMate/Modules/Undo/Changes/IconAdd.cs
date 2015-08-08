using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class IconAdd : IChange
    {
        MapNode node;
        string icon;

        public IconAdd(MapNode node, string icon)
        {
            this.node = node;
            this.icon = icon;
        }

        public string Description
        {
            get
            {
                return "Icon Added";
            }
        }

        public void Undo()
        {
            node.Icons.Remove(icon);
        }
    }
}
