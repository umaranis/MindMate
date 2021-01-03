using MindMate.Model;
using System.Drawing;

namespace MindMate.Modules.Undo.Changes
{
    internal class MapCanvasBackColorChange : IChange
    {
        private MapTree tree;
        private Color oldValue;

        public MapCanvasBackColorChange(MapTree tree, Color oldValue)
        {
            this.tree = tree;
            this.oldValue = oldValue;
        }

        public string Description => "Canvas Back Color Change";

        public void Undo()
        {
            tree.CanvasBackColor = oldValue;
        }
    }
}