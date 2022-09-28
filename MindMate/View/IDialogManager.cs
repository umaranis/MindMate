using MindMate.Controller;
using MindMate.View.Dialogs;
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
        DialogResult ShowDefaultFormatSettingsDialog(DefaultFormatSettings form);
        Font ShowFontDialog(Font currentFont);
        string ShowInputBox(string question, string caption = null);
        void ShowMessageBox(string title, string msg, MessageBoxIcon icon);
        void ShowStatusNotification(string msg);

        void ShowAboutBox();
    }
}