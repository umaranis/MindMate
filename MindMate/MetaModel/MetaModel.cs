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
using MindMate.Serialization;

namespace MindMate.MetaModel
{
    
    /// <summary>
    /// MetaModel is the data about icon names, shortcuts and other meta-data/meta-map information.
    /// </summary>
    //[ProtoBuf.ProtoContract]
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

        private readonly List<ModelIcon> iconsList = new List<ModelIcon>();

        //[ProtoBuf.ProtoMember(1)]
        public List<ModelIcon> IconsList
        {
            get { return iconsList;  }
        }

        private readonly List<ISystemIcon> systemIconList = new List<ISystemIcon>();
        public List<ISystemIcon> SystemIconList
        {
            get { return systemIconList; }
        }

        //[ProtoBuf.ProtoMember(2)]
        public string LastOpenedFile { get; set; }

        public const int RecentFilesCount = 12;

        private readonly CustomFontDialog.RecentlyUsedList<string> recentFiles = new CustomFontDialog.RecentlyUsedList<string>(RecentFilesCount);
        //[ProtoBuf.ProtoMember(3)]
        public CustomFontDialog.RecentlyUsedList<string> RecentFiles { 
            get
            {
                return recentFiles;
            }
        }

        public System.Drawing.Color MapEditorBackColor { get; set; }

        //[ProtoBuf.ProtoMember(4, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        //private int MapEditorBackColorSerialized
        //{
        //    get { return MapEditorBackColor.ToArgb(); }
        //    set { MapEditorBackColor = System.Drawing.Color.FromArgb(value); }
        //}

        public System.Drawing.Color NoteEditorBackColor { get; set; }

        //[ProtoBuf.ProtoMember(5, DataFormat = ProtoBuf.DataFormat.FixedSize)]
        //private int NoteEditorBackColorSerialized
        //{
        //    get { return NoteEditorBackColor.ToArgb(); }
        //    set { NoteEditorBackColor = System.Drawing.Color.FromArgb(value); }
        //}

        private readonly List<NodeStyle> nodeStyles = new List<NodeStyle>();

        public List<NodeStyle> NodeStyles
        {
            get { return nodeStyles; }
        }
        
        private static MetaModel Load()
        {
            MetaModel model;

            try
            {
                //xml
                //XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));
                //model = (MetaModel)formatter.Deserialize(new StreamReader(MetaModel.GetFilePath()));

                //binary
                //using (var file = File.OpenRead(MetaModel.GetFilePath()))
                //{
                //    model = ProtoBuf.Serializer.Deserialize<MetaModel>(file);                    
                //}

                //yaml
                using (var file = new StreamReader(GetFilePath()))
                {
                    model = new MetaModel();
                    new MetaModelYamlSerializer().Deserialize(model, file);
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
            model.RecentFiles.Reverse();

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
            //binary
            //using (var file = File.OpenRead(path))
            //{
            //    model = ProtoBuf.Serializer.Deserialize<MetaModel>(file);
            //}

            //yaml
            using (var file = new StreamReader(path))
            {
                model = new MetaModel();
                new MetaModelYamlSerializer().Deserialize(model, file);
            }

            System.IO.Directory.CreateDirectory(GetFileDirectory());
            System.IO.File.Copy(path, MetaModel.GetFilePath(), true);

            return model;
        }

        
        public void Save()
        {
            //xml
            //XmlSerializer formatter = new XmlSerializer(typeof(MetaModel));            
            //formatter.Serialize(new StreamWriter(Application.UserAppDataPath + "/" + 
            //     "Settings.xml"),this);   

            //binary
            //using (var file = File.Create(MetaModel.GetFilePath()))
            //{
            //    ProtoBuf.Serializer.Serialize<MetaModel>(file, this);
            //}

            //yaml
            using (var file = new StreamWriter(GetFilePath()))
            {
                var s = new MetaModelYamlSerializer();
                s.Serialize(this, file);
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

        const string FILE_NAME = "Settings.Yaml";

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
