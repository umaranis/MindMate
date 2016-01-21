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
        public event Action<PersistenceManager, PersistentTree> TreeSaved;

        public event Action<PersistenceManager, PersistentTree> NewTreeCreating;
        public event Action<PersistenceManager, PersistentTree> TreeOpening;
        public event Action<PersistenceManager, PersistentTree> TreeClosing;

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

        public PersistentTree Find(System.Predicate<PersistentTree> match)
        {
            return fileList.Find(match);
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
                if (CurrentTreeChanged != null) CurrentTreeChanged(this, temp, currentTree);
            }
        }

        public PersistentTree NewTree()
        {
            PersistentTree tree = new PersistentTree(this);

            if (NewTreeCreating != null) NewTreeCreating(this, tree);

            tree.Initialize();
            fileList.Add(tree);

            if (NewTreeCreated != null) NewTreeCreated(this, tree);
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
            PersistentTree tree = new PersistentTree(this);

            if (TreeOpening != null) TreeOpening(this, tree);

            tree.Initialize(fileName);
            fileList.Add(tree);

            if (TreeOpened != null) TreeOpened(this, tree);
            CurrentTree = tree;

            return tree;
        }

        public void CloseCurerntTree()
        {
            if(CurrentTree != null)
            {
                Close(CurrentTree);
            }
        }

        public void Close(PersistentTree tree)
        {
            if (tree == CurrentTree)
            {
                CurrentTree = null;
            }

            if (TreeClosing != null) TreeClosing(this, tree);

            fileList.Remove(tree);

            if (TreeClosed != null) TreeClosed(this, tree);
        }

        internal void _InvokeTreeSaved(PersistentTree tree)
        {
            if (TreeSaved != null) TreeSaved(this, tree);
        }
    }
}
