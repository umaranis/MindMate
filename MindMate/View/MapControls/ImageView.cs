using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls
{
    public class ImageView
    {
        private MapNode node;

        public ImageView(MapNode node)
        {
            Debug.Assert(node.HasImage);
            this.node = node;
        }

        public PointF Location { get; set; }

        public Size Size
        {
            get
            {
                if (node.ImageSize.IsEmpty)
                    return node.GetImage().Size;
                else
                    return node.ImageSize;
            }
        }

        public bool Contains(Point p)
        {
            return p.X >= Location.X && p.Y >= Location.Y && p.X <= Location.X + Size.Width && p.Y <= Location.Y + Size.Height;
        }

        public void Draw(Graphics g)
        {
            var s = Size;
            g.DrawImage(node.GetImage(), Location.X, Location.Y, s.Width, s.Height);
        }
    }
}
