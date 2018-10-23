using System;
using MindMate.Model;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using MindMate.Plugins.Tasks; //TODO: Move DateHelper class out of the plugin into core
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MindMate.Serialization
{
    public partial class MapYamlSerializer
    {

        public void Serialize(MapNode node, Emitter emitter)
        {
            emitter.Emit(new MappingStart());

            SerializeScalarProperties(node, emitter);

            emitter.Emit(new MappingEnd());
        }

        /// <summary>
        /// Deserializes an isolated node
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public MapNode Deserialize(Parser r)
        {
            r.Expect<MappingStart>();

            MapNode n = DeserializeIsolatedNode(r);
            DeserializeScalarProperties(n, r);

            r.Expect<MappingEnd>();

            return n;
        }

        private MapNode DeserializeIsolatedNode(Parser r)
        {
            string text = null; NodePosition pos = NodePosition.Undefined; string id = null;

            string prop = r.Peek<Scalar>().Value;

            if (prop != null && prop.Equals(Text))
            {
                r.Expect<Scalar>();
                text = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>().Value;
            }

            if (prop != null && prop.Equals(Pos))
            {
                r.Expect<Scalar>();
                pos = (NodePosition)Enum.Parse(typeof(NodePosition), r.Expect<Scalar>().Value);
                //prop = r.Peek<Scalar>().Value;
            }

            // Isolated nodes donot have Id 
            //if (prop != null && prop.Equals(Id))
            //{
            //    r.Expect<Scalar>();
            //    id = r.Expect<Scalar>().Value;
            //    //prop = r.Peek<Scalar>().Value;
            //}

            MapNode node = MapNode.CreateIsolatedNode(pos);
            node.Text = text;
            return node;
        }

        private void DeserializeScalarProperties(MapNode node, Parser r)
        {
            Scalar prop = r.Peek<Scalar>();

            if (prop != null && prop.Value.Equals(Folded))
            {
                r.Expect<Scalar>();
                node.Folded = Boolean.Parse(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Link))
            {
                r.Expect<Scalar>();
                node.Link = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Created))
            {
                r.Expect<Scalar>();
                node.Created = DateHelper.ToDateTime(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Modified))
            {
                r.Expect<Scalar>();
                node.Modified = DateHelper.ToDateTime(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Bold))
            {
                r.Expect<Scalar>();
                node.Bold = Boolean.Parse(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Italic))
            {
                r.Expect<Scalar>();
                node.Italic = Boolean.Parse(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Strikeout))
            {
                r.Expect<Scalar>();
                node.Strikeout = Boolean.Parse(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(FontName))
            {
                r.Expect<Scalar>();
                node.FontName = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(FontSize))
            {
                r.Expect<Scalar>();
                node.FontSize = Single.Parse(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(BackColor))
            {
                r.Expect<Scalar>();
                node.BackColor = (Color)new ColorConverter().ConvertFromString(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Color))
            {
                r.Expect<Scalar>();
                node.Color = (Color)new ColorConverter().ConvertFromString(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Shape))
            {
                r.Expect<Scalar>();
                node.Shape = (NodeShape)Enum.Parse(typeof(NodeShape), r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(LineWidth))
            {
                r.Expect<Scalar>();
                node.LineWidth = Int32.Parse(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(LinePattern))
            {
                r.Expect<Scalar>();
                node.LinePattern = (DashStyle)Enum.Parse(typeof(DashStyle), r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(LineColor))
            {
                r.Expect<Scalar>();
                node.LineColor = (Color)new ColorConverter().ConvertFromString(r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(NoteText))
            {
                r.Expect<Scalar>();
                node.NoteText = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

            //if (prop != null && prop.Value.Equals(Image))
            //{
            //    r.Expect<Scalar>();
            //    node.Image = r.Expect<Scalar>().Value;
            //    prop = r.Peek<Scalar>();
            //}

            if (prop != null && prop.Value.Equals(ImageAlignment))
            {
                r.Expect<Scalar>();
                node.ImageAlignment = (ImageAlignment)Enum.Parse(typeof(ImageAlignment), r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(ImageHeight))
            {
                r.Expect<Scalar>();
                node.ImageSize = new Size(node.ImageSize.Width, int.Parse(r.Expect<Scalar>().Value));
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(ImageWidth))
            {
                r.Expect<Scalar>();
                node.ImageSize = new Size(int.Parse(r.Expect<Scalar>().Value), node.ImageSize.Height);
                prop = r.Peek<Scalar>();
            }

            if (prop != null && prop.Value.Equals(Label))
            {
                r.Expect<Scalar>();
                node.Label = r.Expect<Scalar>().Value;
                //prop = r.Peek<Scalar>();
            }

        }
    }
}
