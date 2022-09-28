using MindMate.Model;
using MindMate.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Controller
{
    public class DefaultFormatSettingsCtrl
    {
        public void UpdateSettingsFromMapTree(MapTree tree, DefaultFormatSettings settings)
        {
            var n = tree.DefaultFormat;
            settings.Prop_Font = n.Font;
            settings.Prop_TextColor = n.Color;
            settings.Prop_BackColor = n.BackColor;
            settings.Prop_NodeShape = n.Shape;
            settings.Prop_LineColor = n.LineColor;
            settings.Prop_LinePattern = n.LinePattern;
            settings.Prop_LineWidth = n.LineWidth;

            settings.Prop_MapBackColor = tree.CanvasBackColor;
            settings.Prop_NoteEditorBackColor = tree.NoteBackColor;
            settings.Prop_NoteEditorTextColor = tree.NoteForeColor;
            settings.Prop_SelectedOutlineColor = tree.SelectedOutlineColor;
            settings.Prop_DropHintColor = tree.DropHintColor;
        }

        public void UpdateMapTreeFromSettings(MapTree tree, DefaultFormatSettings settings)
        {
            var n = tree.DefaultFormat;
            if(settings.Prop_Font != n.Font || settings.Prop_TextColor != n.Color || settings.Prop_BackColor != n.BackColor 
                || settings.Prop_NodeShape != n.Shape || settings.Prop_LineColor != n.LineColor 
                || settings.Prop_LinePattern != n.LinePattern || settings.Prop_LineWidth != n.LineWidth)
            {
                tree.DefaultFormat = new NodeFormat(settings.Prop_BackColor, false, settings.Prop_TextColor,
                    settings.Prop_Font.Name, settings.Prop_Font.Size,
                    false, false, settings.Prop_LineColor, settings.Prop_LinePattern,
                    settings.Prop_LineWidth, settings.Prop_NodeShape);
            }

            if(settings.Prop_MapBackColor != tree.CanvasBackColor)
            {
                tree.CanvasBackColor = settings.Prop_MapBackColor;
            }
            if(settings.Prop_NoteEditorBackColor != tree.NoteBackColor)
            {
                tree.NoteBackColor = settings.Prop_NoteEditorBackColor;
            }
            if(settings.Prop_NoteEditorTextColor != tree.NoteForeColor)
            {
                tree.NoteForeColor = settings.Prop_NoteEditorTextColor;
            }
            if(settings.Prop_SelectedOutlineColor != tree.SelectedOutlineColor)
            {
                tree.SelectedOutlineColor = settings.Prop_SelectedOutlineColor;
            }
            if(settings.Prop_DropHintColor != tree.DropHintColor)
            {
                tree.DropHintColor = settings.Prop_DropHintColor;
            }
        }
    }
}
