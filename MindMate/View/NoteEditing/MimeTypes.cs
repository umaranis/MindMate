using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.NoteEditing
{
    static class MimeTypes
    {
        public static string GetMimeType(string extension)
        {
            switch (extension)
            {
                case "jpg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "svg":
                    return "image/svg+xml";
                case "webp":
                    return "image/webp";
                case "bmp":
                    return "image/bmp";
                case "tiff":
                    return "image/tiff";
                case "ico":
                    return "image/x-icon";
                case "fh":
                    return "image/x-freehand";
                case "psd":
                    return "image/vnd.adobe.photoshop";
                default:
                    return null;
            }
        }

        public static string GetExtension(string mimeType)
        {
            switch(mimeType)
            {
                case "image/jpeg":
                    return "jpg";
                case "image/png":
                    return "png";
                case "image/gif":
                    return "gif";
                case "image/svg+xml":
                    return "svg";
                case "image/webp":
                    return "webp";
                case "image/bmp":
                    return "bmp";
                case "image/tiff":
                    return "tiff";
                case "image/x-icon":
                    return "ico";
                case "image/x-freehand":
                    return "fh";
                case "image/vnd.adobe.photoshop":
                    return "psd";
                default:
                    return null;

            }
        }
    }
}
