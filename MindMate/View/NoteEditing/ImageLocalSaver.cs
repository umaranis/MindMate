using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    public class ImageLocalSaver
    {
        private NoteEditor editor;
        private PersistenceManager persistence;

        public ImageLocalSaver(NoteEditor editor, PersistenceManager pManager)
        {
            this.editor = editor;
            this.persistence = pManager;
            editor.ExternalContentAdded += Editor_ExternalContentAdded;
        }

        private void Editor_ExternalContentAdded(object obj)
        {
            ProcessImages(editor.Document, persistence.CurrentTree);
        }

        /// <summary>
        /// Changes all image source to mm protocol, downloads and saves the locally
        /// </summary>
        public static void ProcessImages(HtmlDocument document, PersistentTree tree)
        {
            using (var web = new WebClient())
            {
                var elemCol = document.GetElementsByTagName("img");

                for (int i = 0; i < elemCol.Count; i++)
                {
                    var elem = elemCol[i];
                    var originalSrc = elem.GetAttribute("src");
                    if (originalSrc.Length > 4 && originalSrc.Substring(0, 4).Equals("http", StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] data = null;
                        string extension;
                        try
                        {
                            data = web.DownloadData(originalSrc);
                            extension = MimeTypes.GetExtension(web.ResponseHeaders["Content-Type"]);
                        }
                        catch (WebException) //resource not found (404) and no connection
                        {
                            extension = "png";
                        }

                        var newInternalSrc = ImageLocalPath.CreateNewLocalPath(extension);                            
                        tree.SetLargeObject(newInternalSrc.FileName, new BytesLob(data));
                        
                        elem.SetAttribute("srcOrig", originalSrc);
                        elem.SetAttribute("src", newInternalSrc.Url);
                    }
                }                
            }
        }       
    }
}
