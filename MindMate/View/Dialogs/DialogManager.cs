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
    }
}
