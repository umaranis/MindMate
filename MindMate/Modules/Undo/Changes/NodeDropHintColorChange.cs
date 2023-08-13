using MindMate.Model;
using System.Drawing;

namespace MindMate.Modules.Undo.Changes
{
    internal class NodeDropHintColorChange : IChange
    {
        private readonly MapTree tree;
        private readonly Color oldValue;

        public NodeDropHintColorChange(MapTree tree, Color oldValue)
        {
            this.tree = tree;
            this.oldValue = oldValue;
        }

        public string Description => "Node Drop Hint Color Change";

        public void Undo()
        {
            tree.DropHintColor = oldValue;
        }
    }
}