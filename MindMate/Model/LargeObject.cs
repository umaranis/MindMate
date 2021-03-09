using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Model
{
    /// <summary>
    /// Examples of large objects are images, attachments etc. which are contained within a map.
    /// </summary>
    public interface ILargeObject
    {
        void SaveToStream(Stream stream);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size">size of the stream in bytes</param>
        void LoadFromStream(Stream stream, int size);
    }

    /// <summary>
    /// Used for images within the note editor
    /// </summary>
    public class BytesLob : ILargeObject
    {
        public byte[] Bytes { get; set; }

        public BytesLob() { }
        public BytesLob(byte[] bytes) { Bytes = bytes; }

        public void LoadFromStream(Stream stream, int size)
        {
            Bytes = new BinaryReader(stream).ReadBytes(size);
        }

        public void SaveToStream(Stream stream)
        {
            new BinaryWriter(stream).Write(Bytes);
        }       
    }

    /// <summary>
    /// Used for images within in map
    /// </summary>
    public class ImageLob : ILargeObject
    {
        public Image Image { get; set; }

        public ImageLob() { }
        public ImageLob(Image image) { Image = image; }

        public void LoadFromStream(Stream stream, int size)
        {
            Image = Image.FromStream(stream);
        }

        public void SaveToStream(Stream stream)
        {
            Image.Save(stream, Image.RawFormat.Equals(ImageFormat.MemoryBmp)? ImageFormat.Png : Image.RawFormat);
        }
    }

}
