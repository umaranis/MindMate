using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class LineColorChange : IChange
    {
        MapNode node;
        System.Drawing.Color oldValue;

        public LineColorChange(MapNode node, System.Drawing.Color oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Line Color Change";
            }
        }

        public void Undo()
        {
            node.LineColor = oldValue;
        }
    }
}
