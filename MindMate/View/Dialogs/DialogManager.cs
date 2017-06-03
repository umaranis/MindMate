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
        public string ShowInputBox(string question, string caption = null)
        {
            var inputBox = new InputBox(question, caption);
            if (inputBox.ShowDialog() == DialogResult.OK)
            {
                return inputBox.Answer;
            }

            return null;
        }
    }
}
