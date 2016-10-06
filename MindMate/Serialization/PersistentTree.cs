using MindMate.Model;
using System;
using System.Collections;
using System.Collections.Generic;
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
            string xmlString = System.IO.File.ReadAllText(FileName);
            new MindMapSerializer().Deserialize(xmlString, Tree);
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
            Save();
        }

        /// <summary>
        /// Save Changes
        /// </summary>
        public void Save()
        {
            Debug.Assert(FileName != null, "Persistent Tree: File name is null.");

            var serializer = new MindMapSerializer();
            using (var fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fileStream, Tree);
            }

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

        private Hashtable lobCache = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>null if not found</returns>
        public byte[] GetByteArray(string key)
        {
            return lobCache[key] as byte[];
        }

        public void SetByteArray(string key, byte[] data)
        {
            lobCache[key] = data;
        }

        #endregion Lazy loaded Large Object Cache

    }
}
