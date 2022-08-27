using MindMate.Controller;
using MindMate.Model;
using MindMate.View.MapControls;
using System.Drawing;
using System.Windows.Forms;

namespace MindMate.View
{
    public interface IDialogManager
    {
        StatusBarCtrl StatusBarCtrl { get; set; }

        string GetImageFile();
        bool SeekDeleteConfirmation(string msg);
        Color ShowColorPicker(Color currentColor);
        void ShowDefaultFormatSettingsDialog(MapTree tree);
        Font ShowFontDialog(Font currentFont);
        string ShowInputBox(string question, string caption = null);
        void ShowMessageBox(string title, string msg, MessageBoxIcon icon);
        void ShowStatusNotification(string msg);

        void ShowAboutBox();

        bool ShowLinkManualEditDialog(ref string link);

        bool ShowMultiLineEditDialog(ref string text, TextCursorPosition pos = TextCursorPosition.Undefined);

        bool ShowIconSelector(out string selectedIcon);
    }
}