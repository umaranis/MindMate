﻿using MindMate.Modules.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MindMate.Model
{
    public static class ImageHelper
    {
        public static string GetExtension(Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Bmp))
                return "bmp";
            else if (image.RawFormat.Equals(ImageFormat.Gif))
                return "gif";
            else if (image.RawFormat.Equals(ImageFormat.Icon))
                return "ico";
            else if (image.RawFormat.Equals(ImageFormat.Jpeg))
                return "jpg";
            else if (image.RawFormat.Equals(ImageFormat.Tiff))
                return "tiff";
            else
                return "png";
        }

        public const int IMAGE_DEFAULT_MAX_HEIGHT = 300;
        public const int IMAGE_DEAFULT_MAX_WIDTH = 400;

        /// <summary>
        /// Calculates the reduced size if the given size is larger than default.
        /// It maintains the aspect ratio.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Size CalculateDefaultSize(Size size)
        {
            if(size.Height > IMAGE_DEFAULT_MAX_HEIGHT)
            {
                size.Width = (int)(size.Width * IMAGE_DEFAULT_MAX_HEIGHT / (float)size.Height);
                size.Height = IMAGE_DEFAULT_MAX_HEIGHT;
            }

            if(size.Width > IMAGE_DEAFULT_MAX_WIDTH)
            {
                size.Height = (int)(size.Height * IMAGE_DEAFULT_MAX_WIDTH / (float)size.Width);
                size.Width = IMAGE_DEAFULT_MAX_WIDTH;
            }

            return size;
        }

        /// <summary>
        /// Return false if not successful in loading image
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="image"></param>
        /// <returns>Return false if not successful in loading image</returns>
        public static bool GetImageFromFile(string fileName, out Image image)
        {
            try
            {
                image = Image.FromFile(fileName);
                return true;
            }
            catch(Exception e)
            {
                Log.Write("Cannot load image from given file.", e);
                image = null;
                return false;
            }
        }
    }
}
