using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    public class LineWidthChange : IChange
    {
        MapNode node;
        int oldValue;

        public LineWidthChange(MapNode node, int oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Line Width Change";
            }
        }

        public void Undo()
        {
            node.LineWidth = oldValue;
        }
    }
}
