/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;
using System.Resources;
using MindMate.Controller;

namespace MindMate.Serialization
{
    /// <summary>
    /// Icons are first search in resource file , then in icons folder.
    /// </summary>
    public class IconImageLoader
    {
        //#region Singleton

        //static IconHandler()
        //{
        //    instance = new IconHandler();
        //}

        //private IconHandler()
        //{ }

        //private static IconHandler instance;

        //public static IconHandler Instance
        //{
        //    get { return instance; }
        //}

        //#endregion

        public const string ICON_PATH = "images\\icons";

        /// <summary> 
        /// Loads the bitmaps from application resouce file, if not found there then looks into the icons folder on file system.
        /// Should be used from ModelIcon class only for loading icon bitmap.
        /// </summary>
        /// <param name="icon">Name of icon i.e. name of icon image file without extension</param>
        /// <returns></returns>
        public static Bitmap GetIcon(string icon)
        {
#if ! DEBUG 
            string path = Application.StartupPath + "\\" + ICON_PATH + "\\";
#else
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + ICON_PATH + "\\";
#endif

            Bitmap bitmap;// = bitmaps.get[icon];
            
            object obj = MindMate.Properties.Resources.ResourceManager.GetObject(icon.Replace('-','_'));
            
            if (obj != null)
            {
                bitmap = (Bitmap)obj;
            }
            else
            {
                bitmap = new Bitmap(path + icon + ".png");                    
            }            
            
            return bitmap;
        }       

    }
}
