 
using MindMate.Model;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using System.Globalization;
using MindMate.Plugins.Tasks; //TODO: Move DateHelper class out of the plugin into core

namespace MindMate.Serialization
{

	public partial class MapYamlSerializer
	{
		public const string Text = "text";
		public const string Pos = "pos";
		public const string Id = "id";
		public const string Folded = "folded";
		public const string Link = "link";
		public const string Created = "created";
		public const string Modified = "modified";
		public const string Bold = "bold";
		public const string Italic = "italic";
		public const string Strikeout = "strikeout";
		public const string FontName = "fontName";
		public const string FontSize = "fontSize";
		public const string BackColor = "backColor";
		public const string Color = "color";
		public const string Shape = "shape";
		public const string LineWidth = "lineWidth";
		public const string LinePattern = "linePattern";
		public const string LineColor = "lineColor";
		public const string NoteText = "noteText";
		public const string Image = "image";
		public const string ImageAlignment = "imageAlignment";
        public const string ImageHeight = "imageHeight";
        public const string ImageWidth = "imageWidth";
		public const string Label = "label";
		public const string Icons = "icons";
		public const string Attributes = "attributes";
		
		private void SerializeScalarProperties(MapNode node, Emitter emitter)
        {
			if (node.HasText)
			{
				emitter.Emit(new Scalar(Text));
				emitter.Emit(new Scalar(node.Text));
			}
			if (node.HasPos)
			{
				emitter.Emit(new Scalar(Pos));
				emitter.Emit(new Scalar(node.Pos.ToString()));
			}
			if (node.HasId)
			{
				emitter.Emit(new Scalar(Id));
				emitter.Emit(new Scalar(node.Id));
			}
			if (node.HasFolded)
			{
				emitter.Emit(new Scalar(Folded));
				emitter.Emit(new Scalar(node.Folded.ToString()));
			}
			if (node.HasLink)
			{
				emitter.Emit(new Scalar(Link));
				emitter.Emit(new Scalar(node.Link));
			}
			emitter.Emit(new Scalar(Created));
			emitter.Emit(new Scalar(node.Created.ToString(CultureInfo.InvariantCulture)));
			emitter.Emit(new Scalar(Modified));
			emitter.Emit(new Scalar(node.Modified.ToString(CultureInfo.InvariantCulture)));
			if (node.HasBold)
			{
				emitter.Emit(new Scalar(Bold));
				emitter.Emit(new Scalar(node.Bold.ToString()));
			}
			if (node.HasItalic)
			{
				emitter.Emit(new Scalar(Italic));
				emitter.Emit(new Scalar(node.Italic.ToString()));
			}
			if (node.HasStrikeout)
			{
				emitter.Emit(new Scalar(Strikeout));
				emitter.Emit(new Scalar(node.Strikeout.ToString()));
			}
			if (node.HasFontName)
			{
				emitter.Emit(new Scalar(FontName));
				emitter.Emit(new Scalar(node.FontName));
			}
			if (node.HasFontSize)
			{
				emitter.Emit(new Scalar(FontSize));
				emitter.Emit(new Scalar(node.FontSize.ToString()));
			}
			if (node.HasBackColor)
			{
				emitter.Emit(new Scalar(BackColor));
				emitter.Emit(new Scalar(Convert.ToString(node.BackColor)));
			}
			if (node.HasColor)
			{
				emitter.Emit(new Scalar(Color));
				emitter.Emit(new Scalar(Convert.ToString(node.Color)));
			}
			if (node.HasShape)
			{
				emitter.Emit(new Scalar(Shape));
				emitter.Emit(new Scalar(node.Shape.ToString()));
			}
			if (node.HasLineWidth)
			{
				emitter.Emit(new Scalar(LineWidth));
				emitter.Emit(new Scalar(node.LineWidth.ToString()));
			}
			if (node.HasLinePattern)
			{
				emitter.Emit(new Scalar(LinePattern));
				emitter.Emit(new Scalar(node.LinePattern.ToString()));
			}
			if (node.HasLineColor)
			{
				emitter.Emit(new Scalar(LineColor));
				emitter.Emit(new Scalar(Convert.ToString(node.LineColor)));
			}
			if (node.HasNoteText)
			{
				emitter.Emit(new Scalar(NoteText));
				emitter.Emit(new Scalar(node.NoteText));
			}
			if (node.HasImage)
			{
				emitter.Emit(new Scalar(Image));
				emitter.Emit(new Scalar(node.Image));
			}
			if (node.HasImageAlignment)
			{
				emitter.Emit(new Scalar(ImageAlignment));
				emitter.Emit(new Scalar(node.ImageAlignment.ToString()));
			}
            if(node.HasImageSize)
            {
                emitter.Emit(new Scalar(ImageHeight));
                emitter.Emit(new Scalar(node.ImageSize.Height.ToString()));
                emitter.Emit(new Scalar(ImageWidth));
                emitter.Emit(new Scalar(node.ImageSize.Width.ToString()));
            }
			if (node.HasLabel)
			{
				emitter.Emit(new Scalar(Label));
				emitter.Emit(new Scalar(node.Label));
			}
		}

		/*private void DeserializeScalarProperties(MapNode node, EventReader r)
		{
			Scalar prop = r.Peek<Scalar>();

			if (prop != null && prop.Value.Equals(Text))
            {
                r.Expect<Scalar>();
				node.Text = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

			if (prop != null && prop.Value.Equals(Pos))
            {
                r.Expect<Scalar>();
				node.Pos = (NodePosition)Enum.Parse(typeof(NodePosition), r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

			if (prop != null && prop.Value.Equals(Id))
            {
                r.Expect<Scalar>();
				node.Id = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

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

			if (prop != null && prop.Value.Equals(Image))
            {
                r.Expect<Scalar>();
				node.Image = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

			if (prop != null && prop.Value.Equals(ImageAlignment))
            {
                r.Expect<Scalar>();
				node.ImageAlignment = (ImageAlignment)Enum.Parse(typeof(ImageAlignment), r.Expect<Scalar>().Value);
                prop = r.Peek<Scalar>();
            }

			if (prop != null && prop.Value.Equals(Label))
            {
                r.Expect<Scalar>();
				node.Label = r.Expect<Scalar>().Value;
                prop = r.Peek<Scalar>();
            }

		}*/
	}
}