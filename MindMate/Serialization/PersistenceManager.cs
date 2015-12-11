using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Serialization
{
    /// <summary>
    /// Manages the list of loaded Trees and their persistance.
    /// </summary>
    public class PersistenceManager : IEnumerable<PersistentTree>
    {
        public event Action<PersistenceManager, PersistentTree> NewTreeCreated;
        public event Action<PersistenceManager, PersistentTree> TreeOpened;
        public event Action<PersistenceManager, PersistentTree> TreeClosed;
        public delegate void CurrentTreeChangedDelete(PersistenceManager manager, PersistentTree oldTree, PersistentTree newTree);
        public event CurrentTreeChangedDelete CurrentTreeChanged;

        public PersistenceManager()
        {
            fileList = new List<PersistentTree>();
        }

        public bool IsDirty
        {
            get
            {
                return !fileList.TrueForAll(t => !t.IsDirty);
            }
        }

        #region Tree List

        private readonly List<PersistentTree> fileList;
        public int FileCount { get { return fileList.Count; } }

        public IEnumerator<PersistentTree> GetEnumerator()
        {
            return fileList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return fileList.GetEnumerator();
        }

        public PersistentTree this[int i]
        {
            get
            {
                return fileList[i];
            }
        }

        #endregion Tree List

        private PersistentTree currentTree;
        public PersistentTree CurrentTree
        {
            get
            {
                return currentTree;
            }
            set
            {
                if (currentTree == value) return;
                PersistentTree temp = currentTree;
                currentTree = value;
                CurrentTreeChanged?.Invoke(this, temp, currentTree);
            }
        }

        public PersistentTree NewTree()
        {
            PersistentTree tree = new PersistentTree();
            fileList.Add(tree);

            NewTreeCreated?.Invoke(this, tree);
            CurrentTree = tree;

            return tree;
        }

        /// <summary>
        /// Throws exception if file not found
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public PersistentTree OpenTree(string fileName)
        {
            PersistentTree tree = new PersistentTree(fileName);
            fileList.Add(tree);

            TreeOpened?.Invoke(this, tree);
            CurrentTree = tree;

            return tree;
        }

        //TODO: Implement close map menu command
        public void CloseCurerntTree()
        {
            if(CurrentTree != null)
            {
                Close(CurrentTree);
            }
        }

        public void Close(PersistentTree tree)
        {
            if(tree == CurrentTree)
            {
                CurrentTree = null;
            }
            fileList.Remove(tree);

            TreeClosed?.Invoke(this, tree);
            CurrentTree = tree;
        }

    }
}
