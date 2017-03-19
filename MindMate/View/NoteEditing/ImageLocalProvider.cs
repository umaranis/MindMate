using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.NoteEditing.PluggableProtocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MindMate.View.NoteEditing
{
    [Guid("4C74EFBD-2028-4084-A99C-98CB29A6A69F")]
    public class ImageLocalProvider : EmbeddedProtocol
    {
        private PersistenceManager persistence;

        public ImageLocalProvider(PersistenceManager persistence) : base(ImageLocalPath.ProtocolPrefix, "/")
        {
            this.persistence = persistence;
            EmbeddedProtocolFactory.Register(ImageLocalPath.ProtocolPrefix, () => this);
        }

        public override byte[] GetUrlData(string url, out string contentType)
        {
            ImageLocalPath path = ImageLocalPath.ConvertTo(url);

            byte[] data;

            if (persistence.CurrentTree.TryGetLargeObject(path.FileName, out BytesLob lob))
            {
                data = lob.Bytes;
                contentType = MimeTypes.GetMimeType(path.Extension);
            }
            else
            {
                contentType = "text/html";
                data = Encoding.UTF8.GetBytes(@"<b>Page not found!</b>");
            }

            return data;
        }        
    }
}
