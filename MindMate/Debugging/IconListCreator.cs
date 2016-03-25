/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MindMate.Controller;
using MindMate.Serialization;

namespace MindMate.Debugging
{
    
    public static class IconListCreator
    {

        #region Generate Icon Settings file - first time use - commented now

        /// <summary>
        /// Generate icon settings file from the folder of icons. 
        /// Not used inside the application. Used first time to generate the xml file.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void GenerateIconXML()
        {

            string path = Application.StartupPath + "\\" + IconImageLoader.ICON_PATH + "\\";

            string[] files = Directory.GetFiles(path);
            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xml.Append("<MetaModel xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            xml.Append("<IconsList>");
            for (int i = 0; i < files.Length; i++)
            {
                int j = files[i].LastIndexOf('\\');
                string fileName = files[i].Substring(j + 1,
                    files[i].Length - j - 5);
                
                //this is some windows thumbnail hidden file
                if (fileName == "Thumb")
                    continue;

                xml.Append("<ModelIcon>");
                xml.Append("<Name>");
                xml.Append(fileName);
                xml.Append("</Name>");
                xml.Append("<Title>");
                xml.Append(fileName);
                xml.Append("</Title>");
                xml.Append("<Shortcut></Shortcut>");
                xml.Append("</ModelIcon>");
            }
            xml.Append("</IconsList>");
            xml.Append("</MetaModel>");

            StreamWriter file = new StreamWriter(path + "\\Settings.xml");
            file.Write(xml.ToString());
            file.Close();
        }

        #endregion


        #region Generate Meta Model Icons object from the folder of icons - commented now - first time use only

        /// <summary>
        /// Generate icon meta model from the folder of icons. 
        /// Not used inside the application. Used first time to generate the MetaModel object.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        public static void GenerateMetaModelIconFromFolderFiles()
        {

            string path = Application.StartupPath + "\\" + IconImageLoader.ICON_PATH + "\\";

            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                int j = files[i].LastIndexOf('\\');
                string fileName = files[i].Substring(j + 1,
                    files[i].Length - j - 5);

                if (fileName == "Thumb")
                    continue;

                MetaModel.MetaModel.Instance.IconsList.Add(new MetaModel.ModelIcon(fileName, fileName, ""));

            }

            MetaModel.MetaModel.Instance.Save();

        }

        #endregion


    }
}
