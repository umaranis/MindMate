using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class StrikeoutChange : IChange
    {
        private readonly MapNode node;
        private readonly bool oldValue;

        public StrikeoutChange(MapNode node, bool oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Toggle Strikeout";
            }
        }

        public void Undo()
        {
            node.Strikeout = oldValue;
        }
    }
}
