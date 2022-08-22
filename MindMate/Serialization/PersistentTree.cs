using MindMate.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MindMate.Serialization
{
    /// <summary>
    /// Extends MapTree class to make it persistent to a file. Also enables lazy loading of Large Objects (like images)
    /// </summary>
    public class PersistentTree : MapTree, IDisposable
    {
        /// <summary>
        /// Call <see cref="PersistentTree.Initialize"/> before using the object
        /// </summary>
        internal PersistentTree(PersistenceManager persistenceManager)
        {
            manager = persistenceManager;
        }

        /// <summary>
        /// Initialize a new Tree
        /// </summary>
        internal void Initialize()
        {
            new MapNode(this, "New Map");
            this.TurnOnChangeManager();
            RegisterForMapChangedNotification();
            this.SelectedNodes.Add(this.RootNode);
        }

        /// <summary>
        /// Deserialze an existing Tree. Throws exception if file not found.
        /// </summary>
        /// <param name="fileName"></param>
        internal void Initialize(string fileName)
        {
            FileName = fileName;
            try
            {
                new MapZipSerializer().DeserializeMap(this, FileName);
            }
            catch(InvalidDataException)
            {
                string xmlString = System.IO.File.ReadAllText(FileName);
                new MindMapSerializer().Deserialize(xmlString, this);                                
            }
            this.TurnOnChangeManager();
            RegisterForMapChangedNotification();
            this.SelectedNodes.Add(this.RootNode);
        }

        private readonly PersistenceManager manager;

        public string FileName { get; private set; }

        public bool IsNewMap
        {
            get
            {
                return FileName == null;
            }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                if (isDirty == value) return;
                isDirty = !isDirty;
                DirtyChanged?.Invoke(this);
            }
                    
        }

        /// <summary>
        /// Save new Tree or to a new file
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            if(this.FileName != null && this.FileName != fileName) // Save As - load all large objects
            {
                LoadAllLargeObjects();
            }

            FileName = fileName;
            Save(true);
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        public void Save(bool overwrite = false)
        {
            Debug.Assert(FileName != null, "Persistent Tree: File name is null.");

            var serializer = new MapZipSerializer();
            try
            {
                serializer.SerializeMap(this, FileName, overwrite);
            }
            catch(InvalidDataException) //in case of converting from old xml version, save without overwrite=true will not work
            {
                serializer.SerializeMap(this, FileName, true);
            }

            newLobs.Clear();
            deletedLobs.Clear();
            IsDirty = false;

            manager._InvokeTreeSaved(this);
        }

        #region IsDirty

        private void RegisterForMapChangedNotification()
        {
            this.NodePropertyChanged += Tree_NodePropertyChanged;
            this.TreeStructureChanged += Tree_TreeStructureChanged;
            this.IconChanged += Tree_IconChanged;
            this.AttributeChanged += Tree_AttributeChanged;
            this.TreeFormatChanged += PersistentTree_TreeFormatChanged;
        }

        private void Tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            TreeChanged();
        }

        private void Tree_IconChanged(MapNode node, IconChangedEventArgs e)
        {
            TreeChanged();
        }

        private void Tree_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs e)
        {
            TreeChanged();
        }

        private void Tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            TreeChanged();
        }

        private void PersistentTree_TreeFormatChanged(MapTree arg1, TreeDefaultFormatChangedEventArgs arg2)
        {
            TreeChanged();
        }

        private void TreeChanged()
        {
            IsDirty = true;
        }

        private void UnregisterForMapChangedNotification()
        {
            this.NodePropertyChanged -= Tree_NodePropertyChanged;
            this.TreeStructureChanged -= Tree_TreeStructureChanged;
            this.IconChanged -= Tree_IconChanged;
            this.AttributeChanged -= Tree_AttributeChanged;
        }

        public delegate void DirtyChangedDelegate(PersistentTree tree);
        public event DirtyChangedDelegate DirtyChanged;

        #endregion IsDirty

        #region Lazy loaded Large Object Cache

        private List<string> newLobs = new List<string>();      //not saved yet
        private List<string> deletedLobs = new List<string>();  //to be deleted

        public IEnumerable<KeyValuePair<string, ILargeObject>> NewLargeObjects 
            => lobStore.Where(a => newLobs.Contains(a.Key));
        public IEnumerable<string> DeletedLargeObjects
            => deletedLobs;

                
        public override T GetLargeObject<T>(string key)
        {
            if (base.TryGetLargeObject<T>(key, out T obj))
            {
                return obj;
            }
            else
            {
                obj = new MapZipSerializer().DeserializeLargeObject<T>(FileName, key);
                base.SetLargeObject(key, obj);                
                return obj;
            }
        }

        public override bool TryGetLargeObject<T>(string key, out T largeObject)
        {
            if (base.TryGetLargeObject<T>(key, out largeObject))
            {
                return true;
            }
            else
            {
                largeObject = new MapZipSerializer().DeserializeLargeObject<T>(FileName, key);
                if (largeObject != null)
                {
                    base.SetLargeObject(key, largeObject);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override void SetLargeObject(string key, ILargeObject largeObject)
        {
            base.SetLargeObject(key, largeObject);
            newLobs.Add(key);
        }

        public override bool RemoveLargeObject(string key)
        {
            bool result = base.RemoveLargeObject(key);
            if (result) deletedLobs.Add(key);
            return result;
        }

        #endregion Lazy loaded Large Object Cache

        /// <summary>
        /// Large Objects are loaded on demand by PersistentTree, by default. This method forces to preload all of them.
        /// This is useful when making a copy of the map (like in Save As)
        /// </summary>
        public override void LoadAllLargeObjects()
        {
            if (FileName == null) return; //this map is not saved yet, no large objects to load

            var ser = new MapZipSerializer();
            List<string> keys = ser.DeserializeAllLargeObjectKeys(FileName);
            foreach (var key in keys)
            {
                if(!lobStore.ContainsKey(key))
                {
                    ILargeObject largeObject = LargeObjectHelper.CreateFromKey(key);
                    if(ser.DeserializeLargeObject(FileName, key, largeObject))
                    {
                        base.SetLargeObject(key, largeObject);
                    }
                }
            }
        }

        #region IDisposable Support

        public void Dispose()
        {
            UnregisterForMapChangedNotification();        
        }
        #endregion

    }
}
