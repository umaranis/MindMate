using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.WinFormsUI.Dialogs
{
    public class DefaultFormatSettingsCtrl
    {
        public void UpdateSettingsFromMapTree(MapTree tree, DefaultFormatSettings setttingsForm)
        {
            var n = tree.DefaultFormat;
            setttingsForm.Prop_Font = n.Font;
            setttingsForm.Prop_TextColor = n.Color;
            setttingsForm.Prop_BackColor = n.BackColor;
            setttingsForm.Prop_NodeShape = n.Shape;
            setttingsForm.Prop_LineColor = n.LineColor;
            setttingsForm.Prop_LinePattern = n.LinePattern;
            setttingsForm.Prop_LineWidth = n.LineWidth;

            setttingsForm.Prop_MapBackColor = tree.CanvasBackColor;
            setttingsForm.Prop_NoteEditorBackColor = tree.NoteBackColor;
            setttingsForm.Prop_NoteEditorTextColor = tree.NoteForeColor;
            setttingsForm.Prop_SelectedOutlineColor = tree.SelectedOutlineColor;
            setttingsForm.Prop_DropHintColor = tree.DropHintColor;
        }

        public void UpdateMapTreeFromSettings(MapTree tree, DefaultFormatSettings settingsForm)
        {
            var n = tree.DefaultFormat;
            if(settingsForm.Prop_Font != n.Font || settingsForm.Prop_TextColor != n.Color || settingsForm.Prop_BackColor != n.BackColor 
                || settingsForm.Prop_NodeShape != n.Shape || settingsForm.Prop_LineColor != n.LineColor 
                || settingsForm.Prop_LinePattern != n.LinePattern || settingsForm.Prop_LineWidth != n.LineWidth)
            {
                tree.DefaultFormat = new NodeFormat(settingsForm.Prop_BackColor, false, settingsForm.Prop_TextColor,
                    settingsForm.Prop_Font.Name, settingsForm.Prop_Font.Size,
                    false, false, settingsForm.Prop_LineColor, settingsForm.Prop_LinePattern,
                    settingsForm.Prop_LineWidth, settingsForm.Prop_NodeShape);
            }

            if(settingsForm.Prop_MapBackColor != tree.CanvasBackColor)
            {
                tree.CanvasBackColor = settingsForm.Prop_MapBackColor;
            }
            if(settingsForm.Prop_NoteEditorBackColor != tree.NoteBackColor)
            {
                tree.NoteBackColor = settingsForm.Prop_NoteEditorBackColor;
            }
            if(settingsForm.Prop_NoteEditorTextColor != tree.NoteForeColor)
            {
                tree.NoteForeColor = settingsForm.Prop_NoteEditorTextColor;
            }
            if(settingsForm.Prop_SelectedOutlineColor != tree.SelectedOutlineColor)
            {
                tree.SelectedOutlineColor = settingsForm.Prop_SelectedOutlineColor;
            }
            if(settingsForm.Prop_DropHintColor != tree.DropHintColor)
            {
                tree.DropHintColor = settingsForm.Prop_DropHintColor;
            }
        }
    }
}
