using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ItalicChange : IChange
    {
        private MapNode node;
        private bool oldValue;

        public ItalicChange(MapNode node, bool oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Toggle Italic";
            }
        }

        public void Undo()
        {
            node.Italic = oldValue;
        }
    }
}
