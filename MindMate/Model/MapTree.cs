/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MindMate.Modules.Undo;

namespace MindMate.Model
{

    public partial class MapTree
    {

        public MapTree()
        {
            selectedNodes = new SelectedNodes();
        }

        private MapNode rootNode;
        public MapNode RootNode
        {
            get
            {
                return rootNode;
            }
            set
            {
                rootNode = value;
            }
        }

        #region Selected Nodes

        private readonly SelectedNodes selectedNodes;
        public SelectedNodes SelectedNodes
        {
            get
            {
                return selectedNodes;
            }
        }

        /// <summary>
        /// Select all visible nodes
        /// </summary>
        public void SelectAllNodes()
        {
            RootNode.ForEach(
                n => SelectedNodes.Add(n, true), //action
                n => n.Folded == false           //condition for traversing descendents   
                );
        }

        /// <summary>
        /// Select all nodes at the given level (depth) from center
        /// </summary>
        /// <param name="level"></param>
        /// <param name="expandSelection"></param>
        /// <param name="expandNodes">Unfold nodes to select given level if true, otherwise only visible nodes of the given level are selected</param>
        public void SelectLevel(int level, bool expandSelection, bool expandNodes)
        {
            if (!expandSelection) { SelectedNodes.Clear(); }

            RootNode.RollDownAggregate(
                (n, v) =>
                {
                    if (level == v)
                    {
                        SelectedNodes.Add(n, true);
                        n.ForEachAncestor(a => a.Folded = false);
                    }
                    return v + 1;
                },
                0,
                (n, v) => n.Folded && !expandNodes
                );
        }

        #endregion

        /// <summary>
        /// Indicates that MapTree is currently being deserialized and MapTree data is not completely loaded
        /// </summary>
        public bool Deserializing { get; set; }

        #region AttributeSpec

        private Dictionary<string, AttributeSpec> attributeSpecs = new Dictionary<string, AttributeSpec>();

        public IEnumerable<AttributeSpec> AttributeSpecs
        {
            get { return attributeSpecs.Values; }
        }

        /// <summary>
        /// Returns null if AttributeSpec doesn't exist
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public AttributeSpec GetAttributeSpec(string attributeName)
        {
            AttributeSpec attSpec = null;
            if (attributeSpecs.TryGetValue(attributeName, out attSpec))
                return attSpec;
            else
                return null;
        }

        public int AttributeSpecCount { get { return attributeSpecs.Count; } }

        public event Action<AttributeSpec, AttributeSpecEventArgs> AttributeSpecChangeEvent = delegate { };

        private void FireEvent(AttributeSpec obj, AttributeSpecEventArgs args)
        {
            AttributeSpecChangeEvent(obj, args);
        }


        #endregion AttributeSpec

        public MapNode GetClosestUnselectedNode(MapNode node)
        {
            if (node == null)
            {
                return this.RootNode;
            }

            var parentNode = node.Parent;
            var prevNode = node.Previous;
            var nextNode = node.Next;

            while (parentNode != null && parentNode.Pos != NodePosition.Root)
            {
                if (!this.SelectedNodes.Contains(parentNode))
                {
                    parentNode = parentNode.Parent;
                    continue;
                }
                return this.GetClosestUnselectedNode(parentNode);
            }

            while (nextNode != null)
            {
                if (this.SelectedNodes.Contains(nextNode))
                {
                    nextNode = nextNode.Next;
                    continue;
                }
                return nextNode;
            }

            while (prevNode != null)
            {
                if (this.SelectedNodes.Contains(prevNode))
                {
                    prevNode = prevNode.Previous;
                    continue;
                }
                return prevNode;
            }

            return node.Parent;
        }

        /// <summary>
        /// Expand / Collapse node so that all branches are expanded to the given level
        /// </summary>
        /// <param name="level"></param>
        public void UnfoldMapToLevel(int level)
        {
            if (level < 1) return;

            RootNode.RollDownAggregate(
                (n, v) =>
                {
                    v++;
                    n.Folded = v == level;
                    return v;
                },
                -1,
                (n, v) => v > level
                );
        }

        public void RebalanceTree()
        {
            int rightCount = RootNode.ChildRightNodes.Count();
            int leftCount = RootNode.ChildLeftNodes.Count();

            int diff = (int)Math.Truncate((rightCount - leftCount) / 2d);

            if (diff > 0)
            {
                while (diff > 0)
                {
                    RootNode.GetLastChild(NodePosition.Right)?.MoveDown();
                    diff--;
                }
            }
            else
            {
                while (diff < 0)
                {
                    RootNode.GetFirstChild(NodePosition.Left)?.MoveUp();
                    diff++;
                }
            }
        }

        #region "Node Change Events"

        public event Action<MapNode, NodePropertyChangedEventArgs> NodePropertyChanged = delegate { };
        public event Action<MapNode, TreeStructureChangedEventArgs> TreeStructureChanged = delegate { };
        public event Action<MapNode, IconChangedEventArgs> IconChanged = delegate { };
        public event Action<MapNode, AttributeChangeEventArgs> AttributeChanged = delegate { };
        public event Action<MapTree, TreeDefaultFormatChangedEventArgs> TreeFormatChanged = delegate { };
        /// <summary>
        /// Event is fired before changes are applied. See AttributeChanged event for notification after the changes.
        /// </summary>
        public event Action<MapNode, AttributeChangingEventArgs> AttributeChanging = delegate { };

        internal void FireEvent(MapNode node, NodeProperties property, object oldValue)
        {
            var args = new NodePropertyChangedEventArgs()
            {
                ChangedProperty = property,
                OldValue = oldValue
            };

            NodePropertyChanged(node, args);
        }

        internal void FireEvent(MapNode node, TreeStructureChange change)
        {
            var args = new TreeStructureChangedEventArgs()
            {
                ChangeType = change
            };

            TreeStructureChanged(node, args);
        }

        internal void FireEvent(MapNode node, IconChange change, string icon)
        {
            var args = new IconChangedEventArgs()
            {
                ChangeType = change,
                Icon = icon
            };

            IconChanged(node, args);
        }

        internal void FireEvent(MapNode node, AttributeChangeEventArgs args)
        {
            AttributeChanged(node, args);
        }

        internal void FireEvent(MapNode node, AttributeChangingEventArgs args)
        {
            AttributeChanging(node, args);
        }

        private void FireEvent(TreeDefaultFormatChangedEventArgs args)
        {
            TreeFormatChanged(this, args);
        }

        #endregion

        #region Change Manager

        public ChangeManager ChangeManager
        {
            get;
            private set;
        }

        public bool ChangeManagerOn
        {
            get { return ChangeManager != null; }
        }

        /// <summary>
        /// Change Manager is off by default (helps in deserialization).
        /// It is mandatory to turn on ChangeManager (MapCtrl and others assume that it is on).
        /// </summary>
        public void TurnOnChangeManager()
        {
            if (!ChangeManagerOn)
            {
                ChangeManager = new ChangeManager();
                ChangeManager.RegisterMap(this);
            }
        }

        public void TurnOffChangeManager()
        {
            ChangeManager.Unregister(this);
            ChangeManager = null;
        }

        #endregion Change Manager

        #region Large Objects

        protected Dictionary<string, ILargeObject> lobStore = new Dictionary<string, ILargeObject>();

        /// <summary>
        /// For lazy loading tree like <see cref="MindMate.Serialization.PersistentTree"/>, the enumeration will only return large object already loaded.
        /// If required, use <see cref="Serialization.PersistentTree.LoadAllLargeObjects"/> to load all first.
        /// </summary>
        public IEnumerable<KeyValuePair<string, ILargeObject>> LargeObjectsDictionary => lobStore;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">if key not found</exception>
        public virtual T GetLargeObject<T>(string key) where T : class, ILargeObject, new()
        {
            return (T)lobStore[key];
        }

        public virtual bool TryGetLargeObject<T>(string key, out T largeObject) where T : class, ILargeObject, new()
        {
            bool result = lobStore.TryGetValue(key, out ILargeObject value);
            largeObject = (T)value;
            return result;
        }

        public virtual void SetLargeObject(string key, ILargeObject largeObject) 
        {
            lobStore[key] = largeObject;
        }

        public virtual bool RemoveLargeObject(string key)
        {
            return lobStore.Remove(key);
        }

        #endregion LargeObjects

        /// <summary>
        /// Iterate over all nodes in the tree
        /// </summary>
        public IEnumerable<MapNode> MapNodes
        {
            get
            {
                return (new[] { RootNode }).Concat(RootNode.Descendents);
            }
        }

        #region Default Node Formatting / Theme

        private NodeFormat nodeFormat = NodeFormat.CreateDefaultFormat();
        /// <summary>
        /// Null if the format is not defined
        /// </summary>
        public NodeFormat DefaultFormat
        {
            get => nodeFormat;
            set
            {
                var oldValue = nodeFormat;
                nodeFormat = value;

                FireEvent(new TreeDefaultFormatChangedEventArgs()
                {
                    ChangeType = TreeFormatChange.NodeFormat,
                    OldValue = oldValue
                });
            }
        }

        private TreeFormat treeFormat = new TreeFormat();

        /// <summary>
        /// Background color of Map Canvas
        /// </summary>
        public Color CanvasBackColor
        {
            get => treeFormat.CanvasBackColor;
            set
            {
                var oldValue = treeFormat.CanvasBackColor;
                treeFormat.CanvasBackColor = value;
                FireEvent(new TreeDefaultFormatChangedEventArgs()
                {
                    ChangeType = TreeFormatChange.MapCanvasBackColor,
                    OldValue = oldValue
                });
            }
        }
        public bool HasCanvasBackColor => CanvasBackColor != TreeFormat.DefaultCanvasBackColor;

        /// <summary>
        /// Background color of note editor window
        /// </summary>
        public Color NoteBackColor
        {
            get => treeFormat.NoteEditorBackColor;
            set
            {
                var oldValue = treeFormat.NoteEditorBackColor;
                treeFormat.NoteEditorBackColor = value;
                FireEvent(new TreeDefaultFormatChangedEventArgs()
                {
                    ChangeType = TreeFormatChange.NoteEditorBackColor,
                    OldValue = oldValue
                });
            }
        }
        public bool HasNoteBackColor => NoteBackColor != TreeFormat.DefaultNoteEditorBackColor;

        /// <summary>
        /// Text color of note editor window
        /// </summary>
        public Color NoteForeColor
        {
            get => treeFormat.NoteEditorForeColor;
            set
            {
                var oldValue = treeFormat.NoteEditorForeColor;
                treeFormat.NoteEditorForeColor = value;
                FireEvent(new TreeDefaultFormatChangedEventArgs()
                {
                    ChangeType = TreeFormatChange.NoteEditorForeColor,
                    OldValue = oldValue
                });
            }
        }
        public bool HasNoteForeColor => NoteForeColor != TreeFormat.DefaultNoteEditorForeColor;

        /// <summary>
        /// Color used for outlining the selected node and on hover
        /// </summary>
        public Color SelectedOutlineColor 
        { 
            get => treeFormat.NodeHighlightColor;
            set
            {
                var oldValue = treeFormat.NodeHighlightColor;
                treeFormat.NodeHighlightColor = value;
                FireEvent(new TreeDefaultFormatChangedEventArgs()
                {
                    ChangeType = TreeFormatChange.NodeHighlightColor,
                    OldValue = oldValue
                });
            }
        }
        public bool HasSelectedOutlineColor => SelectedOutlineColor != TreeFormat.DefaultSelectedNodeOutlineColor;

        /// <summary>
        /// Color used to highlight the drag and drop target
        /// </summary>
        public Color DropHintColor
        {
            get => treeFormat.DropHintColor;
            set {
                var oldValue = treeFormat.DropHintColor;
                treeFormat.DropHintColor = value;
                FireEvent(new TreeDefaultFormatChangedEventArgs()
                {
                    ChangeType = TreeFormatChange.NodeDropHintColor,
                    OldValue = oldValue
                });
            }
        }
        public bool HasDropHintColor => DropHintColor != TreeFormat.DefaultDropTargetHintColor;

        public Pen SelectedNodeOutlinePen => this.treeFormat.SelectedNodeOutlinePen;
        public Pen NodeHighlightPen => this.treeFormat.NodeHighlightPen;
        public Pen DropHintPen => this.treeFormat.DropHintPen;

        #endregion Default Node Formatting / Theme

        /// <summary>
        /// This method is to ensure that all large objects are loaded.
        /// Not applicable to MapTree but some dervied classes of MapTree may lazy load large objects (for example, PersistentTree). 
        /// </summary>
        public virtual void LoadAllLargeObjects()
        {

        }


        /// <summary>
        /// Used for creating isolated MapNode(s)
        /// </summary>
        public static MapTree Default = new MapTree();        
    }
}
