using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.MetaModel
{
    public class NodeStyle
    {
        /// <summary>
        /// Preset styles for MapNode
        /// </summary>
        public NodeStyle()
        {
        }

        /// <summary>
        /// Create a new style using the refNode (internal copy of given refNode is created)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="styleSource">Changing refNode afterwards will not affect the created style.</param>
        /// <param name="custom"></param>
        public NodeStyle(string title, MapNode styleSource, bool custom = true)
        {
            Title = title;
            Custom = custom;

            RefNode = MapNode.CreateIsolatedNode(NodePosition.Right);
            RefNode.Text = "Sample";
            
            styleSource.CopyFormatTo(RefNode);

            Image = new StyleImageGenerator(RefNode).GenerateImage();
        }

        public string Title { get; set; }

        public Bitmap Image { get; set; }

        public MapNode RefNode { get; set; }

        public bool Custom { get; set; }

        public void ApplyTo(MapNode node)
        {
            RefNode.CopyFormatTo(node);
        }

        public void ApplyTo(IEnumerable<MapNode> nodes)
        {
            foreach (var n in nodes)
            {
                RefNode.CopyFormatTo(n);
            }
        }
    }
}
