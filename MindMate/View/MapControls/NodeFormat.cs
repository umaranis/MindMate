using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// The properties should never be null except BackColor. 
    /// Null doesn't make sense because NodeFormat is a computed style (in contrast to MapNode where style related properties can be null)
    /// </summary>
    public class NodeFormat
    {
        /// <summary>
        /// Sets all properties to their default values
        /// </summary>
        private NodeFormat()
        {
            BackColor = Color.Empty;
            Bold = false;
            Color = DefaultColor;
            FontName = DefaultFontName;
            FontSize = DefaultFontSize;
            Italic = false;
            Strikeout = false;
            LineColor = DefaultLineColor;
            LinePattern = DefaultLinePattern;
            LineWidth = DefaultLineWidth;
            Shape = DefaultNodeShape;

            Font = DefaultFont;
            LinePen = Pens.Gray;
        }


        private NodeFormat(Color backColor, bool bold, Color color, string fontName, float fontSize, bool italic, bool strikeout, Color lineColor, DashStyle linePattern, int lineWidth, NodeShape shape)
        {
            BackColor = backColor;
            Bold = bold;
            Color = color;
            FontName = fontName;
            FontSize = fontSize;
            Italic = italic;
            Strikeout = strikeout;
            LineColor = lineColor;
            LinePattern = linePattern;
            LineWidth = lineWidth;
            Shape = shape;

            Font = CreateFont();
            LinePen = new Pen(LineColor, LineWidth);
            LinePen.DashCap = DashCap.Round;
            LinePen.DashStyle = LinePattern;
        }

        private static NodeFormat defaultObject;
        public static NodeFormat CreateDefaultFormat() => defaultObject;

        static NodeFormat()
        {
            DefaultFont = new Font(DefaultFontFamily, DefaultFontSize);
            defaultObject = new NodeFormat();
        }

        public static NodeFormat CreateNodeFormat(MapNode node)
        {
            if (!node.HasFormatting())
            {
                return node.Tree.DefaultFormat;
            }
            else
            {
                return new NodeFormat(
                    node.BackColor,
                    node.Bold,
                    node.HasColor ? node.Color : DefaultColor,
                    node.HasFontName ? node.FontName : DefaultFontName,
                    node.HasFontSize ? node.FontSize : DefaultFontSize,
                    node.Italic,
                    node.Strikeout,
                    node.HasLineColor ? node.LineColor : DefaultLineColor,
                    node.HasLinePattern ? node.LinePattern : DefaultLinePattern,
                    node.HasLineWidth ? node.LineWidth : DefaultLineWidth,
                    node.HasShape ? node.Shape : DefaultNodeShape
                    );
            }
        }

        #region core properties
        /// <summary>
        /// Can be empty
        /// </summary>
        public Color BackColor { get; }
        public bool Bold { get; }
        /// <summary>
        /// Should never be empty
        /// </summary>
        public Color Color { get; }
        public string FontName { get; }
        /// <summary>
        /// Should never be empty
        /// </summary>
        public float FontSize { get; }
        public bool Italic { get; }
        public bool Strikeout { get; }
        /// <summary>
        /// Should never be empty
        /// </summary>
        public Color LineColor { get; }
        /// <summary>
        /// Should never be null
        /// </summary>
        public DashStyle LinePattern { get; }
        /// <summary>
        /// Should never be null
        /// </summary>
        public int LineWidth { get; }
        /// <summary>
        /// Should never be none
        /// </summary>
        public NodeShape Shape { get; }
        #endregion core properties


        #region calculated properties
        public Font Font { get; }
        public Pen LinePen { get; }
        #endregion calculated properties


        #region default values
        public static FontFamily DefaultFontFamily => FontFamily.GenericSansSerif;
        public static float DefaultFontSize => 10;
        public static string DefaultFontName => "Microsoft Sans Serif";
        /// <summary>
        /// Default Text Color
        /// </summary>
        public static Color DefaultColor => Color.Black;
        public static Color DefaultLineColor => Color.Gray;
        public static DashStyle DefaultLinePattern => DashStyle.Solid;
        public static int DefaultLineWidth => 1;
        public static NodeShape DefaultNodeShape => NodeShape.Fork;
        public static Font DefaultFont { get; private set; }
        #endregion default values


        private Font CreateFont()
        {
            FontStyle style = FontStyle.Regular;
            if (Bold) style |= FontStyle.Bold;
            if (Italic) style |= FontStyle.Italic;
            if (Strikeout) style |= FontStyle.Strikeout;

            FontFamily ff;
            try
            {
                ff = FontName != null ? new FontFamily(FontName) : DefaultFontFamily;
            }
            catch (ArgumentException)
            {
                ff = DefaultFontFamily;
                System.Diagnostics.Trace.TraceError("Specified Font Name (" + FontName + ") not found. Using default font instead.");
            }

            return new Font(ff, FontSize != 0 ? FontSize : DefaultFontSize, style);
        }
    }
}