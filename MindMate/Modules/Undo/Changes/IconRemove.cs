using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class IconRemove : IChange
    {
        MapNode node;
        string icon;

        public IconRemove(MapNode node, string icon)
        {
            this.node = node;
            this.icon = icon;
        }

        public string Description
        {
            get
            {
                return "Icon Removed";
            }
        }

        public void Undo()
        {
            node.Icons.Add(icon);
        }
    }
}
