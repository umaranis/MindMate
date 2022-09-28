using MindMate.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.View.Dialogs
{
    public partial class DefaultFormatSettings : Form
    {
        private readonly DialogManager dialogManager;

        public DefaultFormatSettings(DialogManager dialogManager)
        {
            InitializeComponent();

            this.dialogManager = dialogManager;
        }

        private string ConvertFontToString(Font f)
        {
            return f.FontFamily.Name + ", " + f.Size + "pt" +
                    (f.Bold ? ", bold" : "") + (f.Italic ? ", italic" : "") + (f.Strikeout ? ", strikeout" : "");
        }

        public Font Prop_Font
        {
            get => (Font)valFont.Tag;
            set
            {
                valFont.Tag = value;
                valFont.Text = ConvertFontToString(value);
            }
        }

        public Color Prop_TextColor
        {
            get => valTextColor.BackColor;
            set => valTextColor.BackColor = value;
        }

        public Color Prop_BackColor
        {
            get => valNodeBackColor.BackColor == SystemColors.Control ? Color.Empty : valNodeBackColor.BackColor;
            set => valNodeBackColor.BackColor = value;
        }

        public NodeShape Prop_NodeShape
        {
            get => (NodeShape)Enum.Parse(NodeShape.Fork.GetType(), valNodeShape.SelectedItem.ToString());
            set => valNodeShape.SelectedItem = value.ToString();
        }

        public Color Prop_LineColor
        {
            get => valLineColor.BackColor;
            set => valLineColor.BackColor = value;
        }

        public DashStyle Prop_LinePattern
        {
            get
            {
                var str = valLinePattern.SelectedItem.ToString();
                if(str.Equals("Mixed"))
                {
                    return DashStyle.DashDotDot;
                }
                else if(str.Equals("Dotted"))
                {
                    return DashStyle.Dot;
                }
                else if(str.Equals("Dashed"))
                {
                    return DashStyle.Dash;
                }
                else if(str.Equals("Solid"))
                {
                    return DashStyle.Solid;
                }
                else
                {
                    return DashStyle.Solid;
                }
                
            }
            set
            {
                string val;
                switch (value) {
                    case DashStyle.Dash:
                        val = "Dashed";
                        break;
                    case DashStyle.Dot:
                        val = "Dotted";
                        break;
                    case DashStyle.DashDotDot:
                        val = "Mixed";
                        break;
                    default:
                        val = "Solid";
                        break;
                }
                valLinePattern.SelectedItem = val;
            }
        }

        public int Prop_LineWidth
        {
            get => int.Parse(valLineWidth.SelectedItem.ToString());
            set => valLineWidth.SelectedItem = value.ToString();
        }

        public Color Prop_MapBackColor
        {
            get => valCanvasBackColor.BackColor;
            set => valCanvasBackColor.BackColor = value;
        }

        public Color Prop_NoteEditorBackColor
        {
            get => valNoteBackColor.BackColor;
            set => valNoteBackColor.BackColor = value;
        }

        public Color Prop_NoteEditorTextColor
        {
            get => valNoteTextColor.BackColor;
            set => valNoteTextColor.BackColor = value;
        }

        public Color Prop_SelectedOutlineColor
        {
            get => valSelectedOutlineColor.BackColor;
            set => valSelectedOutlineColor.BackColor = value;
        }
        
        public Color Prop_DropHintColor
        {
            get => valDropTargetHintColor.BackColor;
            set => valDropTargetHintColor.BackColor = value;
        }

        private void btnSetFont_Click(object sender, EventArgs e)
        {
            var f = dialogManager.ShowFontDialog(Prop_Font);
            if (f != null) { Prop_Font = f; }
        }

        private void btnResetFont_Click(object sender, EventArgs e)
        {
            Prop_Font = NodeFormat.DefaultFont;
        }

        private void btnSetTextColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_TextColor);
            if (!c.IsEmpty) { Prop_TextColor = c; }
        }

        private void btnResetTextColor_Click(object sender, EventArgs e)
        {
            Prop_TextColor = NodeFormat.DefaultColor;
        }

        private void btnSetNodeBackColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_BackColor);
            if (!c.IsEmpty) { Prop_BackColor = c; }
        }

        private void btnResetNodeBackColor_Click(object sender, EventArgs e)
        {
            Prop_BackColor = NodeFormat.DefaultBackColor;
        }

        private void btnResetNodeShape_Click(object sender, EventArgs e)
        {
            Prop_NodeShape = NodeFormat.DefaultNodeShape;
        }

        private void btnSetLineColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_LineColor);
            if (!c.IsEmpty) { Prop_LineColor = c; }
        }

        private void btnResetLineColor_Click(object sender, EventArgs e)
        {
            Prop_LineColor = NodeFormat.DefaultLineColor;
        }

        private void btnResetLinePattern_Click(object sender, EventArgs e)
        {
            Prop_LinePattern = NodeFormat.DefaultLinePattern;
        }

        private void btnResetLineWidth_Click(object sender, EventArgs e)
        {
            Prop_LineWidth = NodeFormat.DefaultLineWidth;
        }

        private void btnSetMapBackColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_MapBackColor);
            if (!c.IsEmpty) { Prop_MapBackColor = c; }
        }

        private void btnResetMapBackColor_Click(object sender, EventArgs e)
        {
            Prop_MapBackColor = TreeFormat.DefaultCanvasBackColor;
        }

        private void btnSetNoteBackColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_NoteEditorBackColor);
            if(!c.IsEmpty) { Prop_NoteEditorBackColor = c; }
        }

        private void btnResetNoteBackColor_Click(object sender, EventArgs e)
        {
            Prop_NoteEditorBackColor = TreeFormat.DefaultNoteEditorBackColor;
        }

        private void btnSetSelectedOutlineColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_SelectedOutlineColor);
            if(!c.IsEmpty) { Prop_SelectedOutlineColor = c; }
        }

        private void btnResetSelectedOutlineColor_Click(object sender, EventArgs e)
        {
            Prop_SelectedOutlineColor = TreeFormat.DefaultSelectedNodeOutlineColor;
        }

        private void btnSetDropTargetHintColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_DropHintColor);
            if(!c.IsEmpty) { Prop_DropHintColor = c; }
        }

        private void btnResetDropTargetHintColor_Click(object sender, EventArgs e)
        {
            Prop_DropHintColor = TreeFormat.DefaultDropTargetHintColor;
        }

        private void btnSetNoteTextColor_Click(object sender, EventArgs e)
        {
            var c = dialogManager.ShowColorPicker(Prop_NoteEditorTextColor);
            if (!c.IsEmpty) { Prop_NoteEditorTextColor = c; }
        }

        private void btnResetNoteTextColor_Click(object sender, EventArgs e)
        {
            Prop_NoteEditorTextColor = TreeFormat.DefaultNoteEditorForeColor;
        }
    }
}
