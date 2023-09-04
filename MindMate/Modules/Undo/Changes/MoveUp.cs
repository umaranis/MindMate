using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class MoveUp : IChange
    {
        private readonly MapNode node;

        public MoveUp(MapNode node)
        {
            this.node = node;
        }

        public string Description => "Node Move Up";

        public void Undo()
        {
            node.MoveDown();
        }
    }
}
