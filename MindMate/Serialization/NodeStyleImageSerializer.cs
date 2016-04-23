using MindMate.MetaModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MindMate.Serialization
{
    public class NodeStyleImageSerializer
    {
        private const string NodeStyleDir = "NodeStyles";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="nodeStyleTitle"></param>
        /// <exception cref="ExternalException">Throws exception image file path is incorrect.</exception>
        /// <exception cref="ArgumentException">This exception is thrown when file path contains invalid characters.</exception>
        public void SerializeImage(Bitmap image, string nodeStyleTitle)
        {
            Directory.CreateDirectory(Dir.UserSettingsDirectory + "\\" + NodeStyleDir);
            image.Save(GetFileName(nodeStyleTitle));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeStyleTitle"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">This exception is thrown if the image file if not found.</exception>
        public Bitmap DeserializeImage(string nodeStyleTitle)
        {
            try
            {
                return new Bitmap(GetFileName(nodeStyleTitle));
            }
            catch
            {
                return null;
            }
        }

        public void DeleteImage(string nodeStyleTitle)
        {
            try
            {
                File.Delete(GetFileName(nodeStyleTitle));
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        private string GetFileName(string nodeStyleTitle)
        {
            return Dir.UserSettingsDirectory + NodeStyleDir + "\\" + nodeStyleTitle + ".png";
        }
    }
}
