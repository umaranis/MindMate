using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ItalicChange : IChange
    {
        private readonly MapNode node;
        private readonly bool oldValue;

        public ItalicChange(MapNode node, bool oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description => "Toggle Italic";

        public void Undo()
        {
            node.Italic = oldValue;
        }
    }
}
