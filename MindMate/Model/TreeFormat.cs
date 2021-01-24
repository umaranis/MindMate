using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Model
{
    /// <summary>
    /// Format related settings for MapTree (look at MapTree.NodeFormat for node formatting)
    /// </summary>
    public class TreeFormat
    {


        public Color CanvasBackColor { get; set; } = DefaultCanvasBackColor;
        public Color NoteEditorBackColor { get; set; } = DefaultNoteEditorBackColor;
        public Color NoteEditorForeColor { get; set; } = DefaultNoteEditorForeColor;

        private Color nodeHighlightColor = DefaultSelectedNodeOutlineColor;        
        /// <summary>
        /// Color used for hover on (highlight) and selected node effects
        /// </summary>
        public Color NodeHighlightColor
        {
            get => nodeHighlightColor;
            set
            {
                nodeHighlightColor = value;

                NodeHighlightPen = new Pen(nodeHighlightColor);
                NodeHighlightPen.DashStyle = DashStyle.Dash;

                SelectedNodeOutlinePen = new Pen(nodeHighlightColor);
            }
        }

        private Color dropHintColor = DefaultDropTargetHintColor;
        /// <summary>
        /// Color used to highlight the drop target during drag and drop
        /// </summary>
        public Color DropHintColor 
        { 
            get => dropHintColor;
            set
            {
                dropHintColor = value;
                DropHintPen = new Pen(dropHintColor);
                DropHintPen.DashStyle = DashStyle.Dash;
            }
        }

        /// <summary>
        /// Pen for selected node effect
        /// </summary> 
        public Pen SelectedNodeOutlinePen { get; private set; } = DefaultSelectedNodeOutlinePen;
        /// <summary>
        /// Pen for hover over node effect
        /// </summary>
        public Pen NodeHighlightPen { get; private set; } = DefaultNodeHighlightPen;
        /// <summary>
        /// Pen used to highlight the drop target during drag and drop
        /// </summary>
        public Pen DropHintPen { get; private set; } = DefaultDropHintPen;

        #region Defaults
        static TreeFormat()
        {
            DefaultNodeHighlightPen = new Pen(DefaultSelectedNodeOutlineColor);
            DefaultNodeHighlightPen.DashStyle = DashStyle.Dash;

            DefaultDropHintPen = new Pen(DefaultDropTargetHintColor);
            DefaultDropHintPen.DashStyle = DashStyle.Dash;
        }

        public static Color DefaultCanvasBackColor => MetaModel.MetaModel.Instance?.MapEditorBackColor ?? Color.White;
        public static Color DefaultNoteEditorBackColor => MetaModel.MetaModel.Instance?.NoteEditorBackColor ?? Color.White;
        public static Color DefaultNoteEditorForeColor => Color.Black;
        /// <summary>
        /// Highlight Color used on node hover and on selection of nodes
        /// </summary>
        public static Color DefaultSelectedNodeOutlineColor => Color.MediumBlue;
        public static Color DefaultDropTargetHintColor => Color.Red;
        public static Pen DefaultSelectedNodeOutlinePen => Pens.MediumBlue;
        public static Pen DefaultNodeHighlightPen { get; private set; }
        public static Pen DefaultDropHintPen { get; private set; }
        #endregion Defaults
    }
}
