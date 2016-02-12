using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class NoteTextChange : IChange
    {
        MapNode node;
        string oldValue;

        public NoteTextChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Rich Content Text Change";
            }
        }

        public void Undo()
        {
            node.NoteText = oldValue;
        }
    }
}
