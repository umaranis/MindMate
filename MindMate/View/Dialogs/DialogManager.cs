using MindMate.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.View.Dialogs
{
    public class DialogManager
    {
#if DEBUG
        virtual
#endif
        public string GetImageFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif; *.bmp|All files (*.*)|*.*";
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }

#if DEBUG
        virtual
#endif
        public bool SeekDeleteConfirmation(string msg)
        {
            var result = MessageBox.Show(msg, "Delete Confirmation", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            return result == DialogResult.Yes;
        }

        /// <summary>
        /// Uses InputBox dialog to ask question from the user
        /// </summary>
        /// <param name="question"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
#if DEBUG
        virtual
#endif
        public string ShowInputBox(string question, string caption = null)
        {
            var inputBox = new InputBox(question, caption);
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                return inputBox.Answer;
            }

            return null;
        }

#if DEBUG
        virtual
#endif
        public void ShowMessageBox(string title, string msg, MessageBoxIcon icon)
        {
            MessageBox.Show(msg, title, MessageBoxButtons.OK, icon);
        }

        private ColorDialog colorDialog;
#if DEBUG
        virtual
#endif
        public System.Drawing.Color ShowColorPicker(System.Drawing.Color currentColor)
        {
            if (colorDialog == null) colorDialog = new ColorDialog() { FullOpen = true };
            if (!currentColor.IsEmpty) colorDialog.Color = currentColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                return colorDialog.Color;
            }
            else
            {
                return System.Drawing.Color.Empty;
            }
        }
        
        private CustomFontDialog.FontDialog fontDialog;
#if DEBUG
        virtual
#endif
        public System.Drawing.Font ShowFontDialog(System.Drawing.Font currentFont)
        {
            if (fontDialog == null) fontDialog = new CustomFontDialog.FontDialog();
            if (currentFont != null) fontDialog.Font = currentFont;
            //fd.ShowEffects = false;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                return fontDialog.Font;
            }
            else
            {
                return null;
            }
        }

#if DEBUG
        virtual
#endif
        public DialogResult ShowDefaultFormatSettingsDialog(DefaultFormatSettings form)
        {
            return form.ShowDialog();
        }

        public WinFormsStatusBarCtrl StatusBarCtrl { get; set; }
#if DEBUG
        virtual
#endif
        public void ShowStatusNotification(string msg)
        {
            StatusBarCtrl.SetStatusUpdate(msg);
        }

    }
}
