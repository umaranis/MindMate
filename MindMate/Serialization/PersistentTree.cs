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
    public class PersistentTree
    {
        /// <summary>
        /// Call <see cref="PersistentTree.Initialize"/> before using the object
        /// </summary>
        internal PersistentTree(PersistenceManager persistenceManager)
        {
            manager = persistenceManager;
            Tree = new MapTree();
        }

        /// <summary>
        /// Initialize a new Tree
        /// </summary>
        internal void Initialize()
        {
            new MapNode(Tree, "New Map");
            Tree.TurnOnChangeManager();
            RegisterForMapChangedNotification();
            Tree.SelectedNodes.Add(Tree.RootNode);
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
                new MapZipSerializer().DeserializeMap(Tree, FileName);
            }
            catch(InvalidDataException)
            {
                string xmlString = System.IO.File.ReadAllText(FileName);
                new MindMapSerializer().Deserialize(xmlString, Tree);
            }
            Tree.TurnOnChangeManager();
            RegisterForMapChangedNotification();
            Tree.SelectedNodes.Add(Tree.RootNode);
        }

        private readonly PersistenceManager manager;

        public MapTree Tree { get; private set; }

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

            var serializer = new MapZipSerializer();
            serializer.SerializeMap(Tree, lobCache.Where(k => newLobs.Contains(k.Key)), FileName, overwrite);

            newLobs.Clear();
            IsDirty = false;

            manager._InvokeTreeSaved(this);
        }

        #region IsDirty

        private void RegisterForMapChangedNotification()
        {
            Tree.NodePropertyChanged += Tree_NodePropertyChanged;
            Tree.TreeStructureChanged += Tree_TreeStructureChanged;
            Tree.IconChanged += Tree_IconChanged;
            Tree.AttributeChanged += Tree_AttributeChanged;
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
            Tree.NodePropertyChanged -= Tree_NodePropertyChanged;
            Tree.TreeStructureChanged -= Tree_TreeStructureChanged;
            Tree.IconChanged -= Tree_IconChanged;
            Tree.AttributeChanged -= Tree_AttributeChanged;
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
            try
            {
                return lobCache[key] as byte[];
            }
            catch(KeyNotFoundException)
            {
                byte[] obj = new MapZipSerializer().DeserializeLargeObject(FileName, key);
                lobCache[key] = obj;
                return obj;
            }
        }

        public void SetByteArray(string key, byte[] data)
        {
            lobCache[key] = data;
            newLobs.Add(key);
        }

        #endregion Lazy loaded Large Object Cache

    }
}
