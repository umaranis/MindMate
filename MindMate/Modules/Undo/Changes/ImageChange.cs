using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ImageChange : IChange
    {
        readonly MapNode node;
        readonly string oldValue;

        public ImageChange(MapNode node, string oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Image Changed";
            }
        }

        public void Undo()
        {
            node.Image = oldValue;
        }
    }
}
