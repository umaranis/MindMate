using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class MoveUp : IChange
    {
        private MapNode node;

        public MoveUp(MapNode node)
        {
            this.node = node;
        }

        public string Description
        {
            get
            {
                return "Node Move Up";
            }
        }

        public void Undo()
        {
            node.MoveDown();
        }
    }
}
