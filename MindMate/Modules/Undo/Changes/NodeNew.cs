using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class NodeNew : IChange
    {
        private MapNode node;

        public NodeNew(MapNode node)
        {
            this.node = node;
        }

        public string Description
        {
            get
            {
                return "New Node";
            }
        }

        public void Undo()
        {
            if (node.Selected = true && node.Tree.SelectedNodes.Count == 1)
            {
                MapNode nextSelection = node.Tree.GetClosestUnselectedNode(node);
                node.Tree.SelectedNodes.Add(nextSelection);
            }
            node.DeleteNode();
        }
    }
}
