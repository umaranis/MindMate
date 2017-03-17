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
            FileName = fileName;
            Save(true);
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        public void Save(bool overwrite = false)
        {
            Debug.Assert(FileName != null, "Persistent Tree: File name is null.");

            //if overwrite then save all largeObjects, otherwise only new ones
            IEnumerable<KeyValuePair<string, byte[]>> largeObjectsToSave = overwrite ? lobCache : lobCache.Where(k => newLobs.Contains(k.Key));

            var serializer = new MapZipSerializer();
            try
            {
                serializer.SerializeMap(this, largeObjectsToSave, FileName, overwrite);
            }
            catch(InvalidDataException) //in case of converting from old xml version, save without overwrite=true will not work
            {
                serializer.SerializeMap(this, largeObjectsToSave, FileName, true);
            }

            newLobs.Clear();
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

        private Dictionary<string, byte[]> lobCache = new Dictionary<string, byte[]>();
        private List<string> newLobs = new List<string>(); //not saved yet
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>null if not found</returns>
        public byte[] GetByteArray(string key)
        {
            byte[] obj;
            if (lobCache.TryGetValue(key, out obj))
            {
                return lobCache[key] as byte[];
            }
            else
            {
                obj = new MapZipSerializer().DeserializeLargeObject(FileName, key);
                lobCache[key] = obj;                
            }

            return obj;
        }

        public void SetByteArray(string key, byte[] data)
        {
            lobCache[key] = data;
            newLobs.Add(key);
        }

        #endregion Lazy loaded Large Object Cache

        #region IDisposable Support

        public void Dispose()
        {
            UnregisterForMapChangedNotification();        
        }
        #endregion

    }
}
