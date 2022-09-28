using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// ImageLocalPath has three parts: Protocol, File Name and Extension
    /// Example: mm://abc.png/
    /// </summary>
    public class ImageLocalPath
    {
        public const string ProtocolPrefix = "mm";
        public const string Protocol = "mm://";

        public string Extension { get; }
        public string FileNameWithoutExt { get; }
        /// <summary>
        /// Name of file with extension
        /// </summary>
        public string FileName { get { return FileNameWithoutExt + "." + Extension; } }
        /// <summary>
        /// Url with mm protocol, file name and extension
        /// </summary>
        public string Url { get { return Protocol + FileName; } }

        private ImageLocalPath(string fileNameWithoutExt, string extension)
        {
            FileNameWithoutExt = fileNameWithoutExt;
            Extension = extension;
        }

        public static ImageLocalPath ConvertTo(string url)
        {
            //Path.GetFileName and Path GetExtension don't work because url can have slash at the end.
            int lastSlash = url.LastIndexOf("/");
            if (lastSlash == url.Length - 1)
            {
                url = url.Substring(0, url.Length - 1);
                lastSlash = url.LastIndexOf("/");
            }
            string fileName = url.Substring(lastSlash + 1);
            int lastDotIndex = fileName.LastIndexOf(".");

            return new ImageLocalPath(
                fileName.Substring(0, lastDotIndex), //file name
                fileName.Substring(lastDotIndex + 1)); //extension
        }

        /// <summary>
        /// Generates a new unique path
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ImageLocalPath CreateNewLocalPath(string extension)
        {
            return new ImageLocalPath(LargeObjectHelper.GenerateNewKey<BytesLob>(), extension);
        }        
    }
}
