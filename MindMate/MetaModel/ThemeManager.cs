using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.MetaModel
{
    public class ThemeManager
    {
        public IEnumerable<string> Themes
        {
            get
            {
                yield return "Default (Light)";
                yield return "Default (Dark)";
                yield return "Pitch Black";
                yield return "Fun (Light)";
                yield return "Fun (Dark)";
                yield return "Solarized Blue";
                yield return "Solarized Orange";
                yield return "Solarized Dark";
                yield return "Red";
                yield return "Dim";
                yield return "Twine";

            }
        }

        public void ApplyTheme(string theme, MapTree tree)
        {
            switch(theme)
            {
                case "Default (Light)":
                    tree.DefaultFormat = NodeFormat.CreateDefaultFormat();
                    tree.CanvasBackColor = TreeFormat.DefaultCanvasBackColor;
                    tree.NoteBackColor = TreeFormat.DefaultNoteEditorBackColor;
                    tree.NoteForeColor = TreeFormat.DefaultNoteEditorForeColor;
                    tree.SelectedOutlineColor = TreeFormat.DefaultSelectedNodeOutlineColor;
                    tree.DropHintColor = TreeFormat.DefaultDropTargetHintColor;
                    break;
                case "Default (Dark)":
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, Color.White, NodeFormat.DefaultFontName,
                        NodeFormat.DefaultFontSize, false, false, NodeFormat.DefaultLineColor, NodeFormat.DefaultLinePattern,
                        NodeFormat.DefaultLineWidth, NodeFormat.DefaultNodeShape);
                    var backColor = Color.FromArgb(40, 44, 52);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = Color.White;
                    tree.SelectedOutlineColor = Color.Red;
                    tree.DropHintColor = Color.FromArgb(255, 128, 64);
                    break;
                case "Pitch Black":
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, Color.White, NodeFormat.DefaultFontName,
                        NodeFormat.DefaultFontSize, false, false, NodeFormat.DefaultLineColor, NodeFormat.DefaultLinePattern,
                        NodeFormat.DefaultLineWidth, NodeFormat.DefaultNodeShape);
                    tree.CanvasBackColor = Color.Black;
                    tree.NoteBackColor = Color.Black;
                    tree.NoteForeColor = Color.White;
                    tree.SelectedOutlineColor = Color.Red;
                    tree.DropHintColor = Color.FromArgb(255, 128, 64);
                    break;
                case "Fun (Light)":
                    tree.DefaultFormat = new NodeFormat(Color.FromArgb(253, 246, 227), false, Color.Black, "Comic Sans MS",
                        11, false, false, Color.Green, NodeFormat.DefaultLinePattern,
                        2, NodeShape.Bullet);
                    backColor = Color.WhiteSmoke;
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = Color.Black;
                    tree.SelectedOutlineColor = Color.Green;
                    tree.DropHintColor = Color.Red;
                    break;
                case "Fun (Dark)":
                    tree.DefaultFormat = new NodeFormat(Color.FromArgb(123, 109, 141), false, Color.White, "Comic Sans MS",
                        11, false, false, Color.FromArgb(147, 79, 28), NodeFormat.DefaultLinePattern,
                        4, NodeShape.Bullet);
                    backColor = Color.FromArgb(64, 57, 73);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = Color.White;
                    tree.SelectedOutlineColor = Color.Yellow;
                    tree.DropHintColor = Color.Lime;
                    break;                                    
                case "Solarized Blue":
                    var textColor = Color.FromArgb(60, 151, 174);
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, textColor, NodeFormat.DefaultFontName,
                        10, false, false, Color.FromArgb(255, 128, 64), NodeFormat.DefaultLinePattern,
                        1, NodeShape.Fork);
                    backColor = Color.FromArgb(253, 246, 227);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = textColor;
                    tree.SelectedOutlineColor = Color.FromArgb(64,0,0);
                    tree.DropHintColor = Color.Black;
                    break;
                case "Solarized Orange":
                    textColor = Color.FromArgb(255, 128, 64);
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, textColor, NodeFormat.DefaultFontName,
                        10, false, false, Color.FromArgb(60, 151, 174), NodeFormat.DefaultLinePattern,
                        1, NodeShape.Bullet);
                    backColor = Color.FromArgb(253, 246, 227);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = textColor;
                    tree.SelectedOutlineColor = Color.FromArgb(64, 0, 0);
                    tree.DropHintColor = Color.Black;
                    break;
                case "Solarized Dark":
                    textColor = Color.FromArgb(253, 246, 227);
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, textColor, NodeFormat.DefaultFontName,
                        10, false, false, Color.FromArgb(60, 151, 174), NodeFormat.DefaultLinePattern,
                        1, NodeShape.Fork);
                    backColor = Color.FromArgb(0,43,54);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = textColor;
                    tree.SelectedOutlineColor = Color.FromArgb(255, 128, 64);
                    tree.DropHintColor = Color.Red;
                    break;
                case "Red":
                    tree.DefaultFormat = new NodeFormat(Color.FromArgb(77, 0, 0), false, Color.White, NodeFormat.DefaultFontName,
                        NodeFormat.DefaultFontSize, false, false, Color.FromArgb(97,0,0), NodeFormat.DefaultLinePattern,
                        NodeFormat.DefaultLineWidth, NodeFormat.DefaultNodeShape);
                    backColor = Color.FromArgb(57, 0, 0);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = Color.White;
                    tree.SelectedOutlineColor = Color.Red;
                    tree.DropHintColor = Color.FromArgb(0, 128, 255);
                    break;
                case "Dim":
                    textColor = Color.FromArgb(203, 105, 89);
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, textColor, NodeFormat.DefaultFontName,
                        NodeFormat.DefaultFontSize, false, false, Color.FromArgb(103, 159, 115), NodeFormat.DefaultLinePattern,
                        NodeFormat.DefaultLineWidth, NodeFormat.DefaultNodeShape);
                    backColor = Color.FromArgb(41, 40, 40);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = textColor;
                    tree.SelectedOutlineColor = Color.Red;
                    tree.DropHintColor = Color.FromArgb(255, 128, 64);
                    break;
                case "Twine":
                    textColor = Color.FromArgb(210, 141, 37);
                    tree.DefaultFormat = new NodeFormat(Color.Empty, false, textColor, NodeFormat.DefaultFontName,
                        NodeFormat.DefaultFontSize, false, false, Color.FromArgb(189, 153, 86), NodeFormat.DefaultLinePattern,
                        NodeFormat.DefaultLineWidth, NodeFormat.DefaultNodeShape);
                    backColor = Color.FromArgb(34, 26, 15);
                    tree.CanvasBackColor = backColor;
                    tree.NoteBackColor = backColor;
                    tree.NoteForeColor = textColor;
                    tree.SelectedOutlineColor = Color.FromArgb(198,53,52);
                    tree.DropHintColor = Color.FromArgb(255, 128, 64);
                    break;
            }
        }
    }
}
