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
        public const string MindMateProtocolPrefix = "mm";
        private PersistenceManager persistence;

        public ImageLocalProvider(PersistenceManager persistence) : base(MindMateProtocolPrefix, "/")
        {
            this.persistence = persistence;
            EmbeddedProtocolFactory.Register(MindMateProtocolPrefix, () => this);
        }

        public override byte[] GetUrlData(string url, out string contentType)
        {
            return GetTestData(url, out contentType);
        }

        private byte[] GetTestData(string url, out string contentType)
        {
            int lastSlash = url.LastIndexOf("/");
            if (lastSlash == url.Length - 1)
            {
                url = url.Substring(0, url.Length - 1);
                lastSlash = url.LastIndexOf("/");
            }
            var fileName = url.Substring(lastSlash + 1);
            var fileExt = url.Substring(url.LastIndexOf(".") + 1);

            var data = persistence.CurrentTree.GetByteArray(fileName);

            if (data == null)
            {
                contentType = "text/html";
                return Encoding.UTF8.GetBytes(@"<b>Page not found!</b>");
            }
                                                    
            switch (fileExt)
            {//TODO: Support other types like svg (check GraphQL logo)
                case "htm":
                    contentType = "text/html";
                    break;
                case "jpg":
                    contentType = "image/jpeg";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            return data;
            
        }
    }
}
