using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class TextChange : IChange
    {
        MapNode node;
        string oldText;

        public TextChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldText = oldValue;
        }

        public string Description
        {
            get
            {
                return "Text Change";
            }
        }

        public void Undo()
        {
            node.Text = oldText;
        }
    }
}
