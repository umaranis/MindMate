using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class LabelChange : IChange
    {
        readonly MapNode node;
        readonly string oldText;

        public LabelChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldText = oldValue;
        }

        public string Description
        {
            get
            {
                return "Label Change";
            }
        }

        public void Undo()
        {
            node.Label = oldText;
        }
    }
}
