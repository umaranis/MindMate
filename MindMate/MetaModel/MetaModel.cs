/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace MindMate.MetaModel
{

    /// <summary>
    /// MetaModel is the data about icon names, shortcuts and other meta-data/meta-map information.
    /// </summary>
    public class MetaModel
    {

        #region Singleton

        private MetaModel() 
        {            
            
        }

        static MetaModel()
        {
            instance = MetaModel.Load();
        }

        private static MetaModel instance;

        public static MetaModel Instance
        {
            get 
            {
                return instance; 
            }            
        }

        #endregion

        private Dictionary<string, ModelIcon> iconsHashMap;

        [@XmlIgnoreAttribute]
        public Dictionary<string, ModelIcon> IconsHashMap
        {
            get { return iconsHashMap; }
            //set { icons = value; }
        }

        private List<ModelIcon> iconsList;

        //TODO: Why two data structures are maintained (iconsList and iconsHashMap)
        public List<ModelIcon> IconsList
        {
            get { return iconsList;  }
            set { iconsList = value; }
        }

        private static MetaModel Load()
        {
            MetaModel model;

            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));
                model = (MetaModel)formatter.Deserialize(new StreamReader(Application.UserAppDataPath + "/" +
                     "Settings.xml"));
            }
            catch (FileNotFoundException)
            {
                model = LoadDefaultMetaModel();                
            }
            catch (DirectoryNotFoundException)
            {
                model = LoadDefaultMetaModel();                
            }

            CreateHashMap(model);


            return model;            

        }

        private static MetaModel LoadDefaultMetaModel()
        {
#if ! DEBUG
            string path = Application.ExecutablePath;
            path = path.Substring(0, path.LastIndexOf("\\") + 1) + "Settings.xml";
#else
            string path = System.IO.Directory.GetCurrentDirectory() + "\\Settings.xml";
#endif
            
            XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));
            MetaModel  model = (MetaModel)formatter.Deserialize(new StreamReader(path));

            System.IO.File.Copy(path, Application.UserAppDataPath + "/" + "Settings.xml");

            return model;
        }

        private static void CreateHashMap(MetaModel model)
        {
            model.iconsHashMap = new Dictionary<string, ModelIcon>();

            foreach (ModelIcon icon in model.iconsList)
            {
                model.iconsHashMap.Add(icon.Name, icon);
            }


        }

        public void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));            
            formatter.Serialize(new StreamWriter(Application.UserAppDataPath + "/" + 
                 "Settings.xml"),this);           

        }


        

    }
}
