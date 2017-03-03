using MindMate.Serialization;
using MindMate.View.NoteEditing.MsHtmlWrap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// Pastes image from Clipboard to NoteEditor
    /// </summary>
    public class ClipboardImagePaster
    {
        private PersistenceManager pManager;

        public ClipboardImagePaster(NoteEditor editor, PersistenceManager pManager)
        {
            editor.Pasting += Editor_Pasting;
            this.pManager = pManager;
        }

        private void Editor_Pasting(object arg1, PastingEventArgs arg2)
        {
            arg2.Handled = PasteFromClipboard((NoteEditor)arg1, pManager.CurrentTree);
        }

        public static bool PasteFromClipboard(NoteEditor editor, PersistentTree tree)
        {
            if(Clipboard.ContainsImage())
            {
                Image image = Clipboard.GetImage();

                var imagePath = ImageLocalPath.CreateNewLocalPath("png");

                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);           
                tree.SetByteArray(imagePath.FileName, ms.ToArray());

                var htmlImage = new HtmlImageCreator(editor);
                htmlImage.InsertImage(imagePath.Url, "");

                return true;                            
            }
            else
            {
                return false;
            }
        }
    }
}