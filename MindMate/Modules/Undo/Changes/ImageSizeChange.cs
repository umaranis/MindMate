using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo.Changes
{
    class ImageSizeChange : IChange
    {
        readonly MapNode node;
        readonly Size oldValue;

        public ImageSizeChange(MapNode node, Size oldValue)
        {
            this.node = node;
            this.oldValue = oldValue;
        }

        public string Description
        {
            get
            {
                return "Image Size Changed";
            }
        }

        public void Undo()
        {
            node.ImageSize = oldValue;
        }
    }
}
