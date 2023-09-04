using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class LinkChange : IChange
    {
        readonly MapNode node;
        readonly string oldValue;

        public LinkChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description => "Link Changed";

        public void Undo()
        {
            node.Link = oldValue;
        }
    }
}
