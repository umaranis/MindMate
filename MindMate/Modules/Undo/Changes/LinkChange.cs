using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class LinkChange : IChange
    {
        MapNode node;
        string oldValue;

        public LinkChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Link Changed";
            }
        }

        public void Undo()
        {
            node.Link = oldValue;
        }
    }
}
