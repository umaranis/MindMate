using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class LinePatternChange : IChange
    {
        MapNode node;
        System.Drawing.Drawing2D.DashStyle oldValue;

        public LinePatternChange(MapNode node, System.Drawing.Drawing2D.DashStyle oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Line Pattern Change";
            }
        }

        public void Undo()
        {
            node.LinePattern = oldValue;
        }
    }
}
