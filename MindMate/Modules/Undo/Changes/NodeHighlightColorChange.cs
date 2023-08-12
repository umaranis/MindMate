using MindMate.Model;
using System.Drawing;

namespace MindMate.Modules.Undo.Changes
{
    internal class NodeHighlightColorChange : IChange
    {
        private readonly MapTree tree;
        private readonly Color oldValue;

        public NodeHighlightColorChange(MapTree tree, Color oldValue)
        {
            this.tree = tree;
            this.oldValue = oldValue;
        }

        public string Description => "Node Highlight Color Change";

        public void Undo()
        {
            tree.SelectedOutlineColor = oldValue;
        }
    }
}