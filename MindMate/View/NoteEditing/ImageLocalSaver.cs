using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
            using (var web = new WebClient())
            {
                new HtmlImageProcessor(editor, e =>
                {
                    persistence.CurrentTree.SetByteArray(Path.GetFileName(e.NewInternalSrc), web.DownloadData(e.OriginalSrc));                     
                    //TODO: web resource not found
                    //TODO: no connection
                });
            }
        }

        
    }
}
