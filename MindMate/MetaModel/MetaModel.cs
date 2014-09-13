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
    [ProtoBuf.ProtoContract]
    public class MetaModel
    {

        
        #region Singleton

        public MetaModel() 
        {            
            
        }

        
        /// <summary>
        /// Static constructor is not getting called sometimes before the Instance variable is used. To get around the problem, Initialize method is invoked during initial setup of the application.
        /// </summary>
        //static MetaModel()
        //{
        //    if(instance == null)
        //        instance = MetaModel.Load();
        //}

        /// <summary>
        /// Static constructor is not getting called sometimes before the Instance variable is used. To get around the problem, Initialize method is invoked during initial setup of the application.
        /// </summary>
        public static void Initialize()
        {
            if (instance == null)
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

        private List<ModelIcon> iconsList;

        //TODO: Why two data structures are maintained (iconsList and iconsHashMap)
        [ProtoBuf.ProtoMember(1)]
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
                //XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));
                //model = (MetaModel)formatter.Deserialize(new StreamReader(MetaModel.GetFilePath()));


                using (var file = File.OpenRead(MetaModel.GetFilePath()))
                {
                    model = ProtoBuf.Serializer.Deserialize<MetaModel>(file);
                }
            }
            catch (FileNotFoundException)
            {
                model = LoadDefaultMetaModel();                
            }
            catch (DirectoryNotFoundException)
            {
                model = LoadDefaultMetaModel();                
            }

            FillIconsCache(model);
            
            return model;            

        }

        private static MetaModel LoadDefaultMetaModel()
        {
#if ! DEBUG
            string path = Application.ExecutablePath;
            path = path.Substring(0, path.LastIndexOf("\\") + 1) + FILE_NAME;
#else
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + FILE_NAME;
#endif
            
            //XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));
            //MetaModel  model = (MetaModel)formatter.Deserialize(new StreamReader(path));

            MetaModel model;
            using (var file = File.OpenRead(path))
            {
                model = ProtoBuf.Serializer.Deserialize<MetaModel>(file);
            }

            System.IO.Directory.CreateDirectory(GetFileDirectory());
            System.IO.File.Copy(path, MetaModel.GetFilePath(), true);

            return model;
        }

        
        public void Save()
        {
            //XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));            
            //formatter.Serialize(new StreamWriter(Application.UserAppDataPath + "/" + 
            //     "Settings.xml"),this);   

            using (var file = File.Create(MetaModel.GetFilePath()))
            {
                ProtoBuf.Serializer.Serialize<MetaModel>(file, this);
            }

        }

        private Dictionary<string, ModelIcon> iconsCache = new Dictionary<string, ModelIcon>();

        //[@XmlIgnoreAttribute]
        //public Dictionary<string, ModelIcon> IconsHashMap
        //{
        //    get { return iconsCache; }
        //    //set { icons = value; }
        //}

        public ModelIcon GetIcon(string name)
        {
            ModelIcon icon;

            if(!iconsCache.TryGetValue(name, out icon))
            {
                icon = iconsList.Find(a => a.Name == name);
                if(icon == null)
                {
                    System.Diagnostics.Trace.TraceError("Icon (" + name + ") not found. Warning icon shown instead. [" + DateTime.Now.ToString() + "]");
                    icon = new ModelIcon("messagebox_warning", "messagebox_warning", "");
                }
                iconsCache[name] = icon;
            }

            return icon;
        }

        private static void FillIconsCache(MetaModel model)
        {
            foreach (ModelIcon icon in model.iconsList)
            {
                model.iconsCache[icon.Name] = icon;
            }

        }

        const string FILE_NAME = "Settings.bin";

        private static string GetFilePath()
        {
            return GetFileDirectory() + "\\" + FILE_NAME;            
        }

        private static string GetFileDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                "\\" + MindMate.Controller.MainCtrl.APPLICATION_NAME;
        }

    }
}
