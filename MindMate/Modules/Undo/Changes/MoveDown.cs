using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class MoveDown : IChange
    {
        private MapNode node;

        public MoveDown(MapNode node)
        {
            this.node = node;
        }

        public string Description
        {
            get
            {
                return "Node Move Down";
            }
        }

        public void Undo()
        {
            node.MoveUp();
        }
    }
}
