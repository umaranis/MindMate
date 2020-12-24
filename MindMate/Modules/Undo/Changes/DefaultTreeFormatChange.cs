using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Modules.Undo.Changes
{
    class DefaultTreeFormatChange : IChange
    {
        MapTree tree;
        NodeFormat oldValue;

        public DefaultTreeFormatChange(MapTree tree, NodeFormat oldValue)
        {
            this.tree = tree;
            this.oldValue = oldValue;
        }

        public string Description => "Default Node Format Change";

        public void Undo()
        {
            tree.DefaultFormat = oldValue;
        }
    }
}
