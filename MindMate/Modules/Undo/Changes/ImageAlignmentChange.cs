using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ImageAlignmentChange : IChange
    {
        readonly MapNode node;
        readonly ImageAlignment oldValue;

        public ImageAlignmentChange(MapNode node, ImageAlignment oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Image Alignment Changed";
            }
        }

        public void Undo()
        {
            node.ImageAlignment = oldValue;
        }
    }
}
