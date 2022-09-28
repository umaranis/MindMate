using MindMate.Model;
using MindMate.Modules.Logging;
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
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

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
                        }
                        catch (Exception exp) 
                        {
                            Log.Write("[ImageLocalSaver] Cannot download the image: " + exp.Message);
                        }
                        if (data != null)
                        {
                            try
                            {
                                extension = MimeTypes.GetExtension(web.ResponseHeaders["Content-Type"]);
                            }
                            catch (WebException exp) //resource not found (404) and no connection
                            {
                                Log.Write("[ImageLocalSaver] Cannot get the content type of image: " + exp.Message);
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
}
