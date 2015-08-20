using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class RichContentTypeChange : IChange
    {
        MapNode node;
        NodeRichContentType oldValue;

        public RichContentTypeChange(MapNode node, NodeRichContentType oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Rich Content Type Change";
            }
        }

        public void Undo()
        {
            node.RichContentType = oldValue;
        }
    }
}
