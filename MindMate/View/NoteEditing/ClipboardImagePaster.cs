using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.NoteEditing.MsHtmlWrap;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// Pastes image from Clipboard to NoteEditor
    /// </summary>
    public class ImagePaster
    {

#region Insert from Clipboard
		private readonly PersistenceManager pManager;

        public ImagePaster(NoteEditor editor, PersistenceManager pManager)
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
                tree.SetLargeObject(imagePath.FileName, new BytesLob(ms.ToArray()));

                var htmlImage = new HtmlImageCreator(editor);
                htmlImage.InsertImage(imagePath.Url, "");

                return true;                            
            }
            else if (Clipboard.ContainsFileDropList())
            {
                var fileList = Clipboard.GetFileDropList();
                var imageList = FilterImageFiles(fileList);
                if (imageList.Any())
                {
                    imageList.ForEach(i =>
                    {
						InsertFormFile(editor, i, tree);
                    });
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;

            }
        }

        private static List<string> FilterImageFiles(StringCollection fileList)
        {
            string[] imageExtensions = { "png", "jpg", "jpeg", "gif", "bmp" };
            var imageList = new List<string>();
            foreach (var fileName in fileList)
            {
                if (imageExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
                {
                    imageList.Add(fileName);
                }
            }
            return imageList;
        }

		#endregion

#region Insert from File

		public static void InsertFormFile(NoteEditor editor, string fileName, MapTree tree)
		{
			var localPath = ImageLocalPath.CreateNewLocalPath(Path.GetExtension(fileName).Substring(1));
			tree.SetLargeObject(localPath.FileName, new BytesLob(File.ReadAllBytes(fileName)));

			var htmlImage = new HtmlImageCreator(editor);
			htmlImage.InsertImage(localPath.Url, "");
		}

#endregion
	}
}