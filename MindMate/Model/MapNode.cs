/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.View.MapControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using MindMate.Serialization;
using System.Drawing.Imaging;

namespace MindMate.Model
{
    public partial class MapNode
    {

        #region Node Attributes

        #region Serializable
        /// <summary>
        /// It is used for hyperlinking nodes, it is null generally.
        /// </summary>
        [Serialized(Order = 3)]
        public string Id { get; private set; }
        public bool HasId { get { return Id != null; } }


        private NodePosition pos;
        [Serialized(Order = 2)]
        public NodePosition Pos
        {
            get
            {
                return pos;
            }
        }
        public bool HasPos
        {
            get { return Parent != null && Pos != Parent.Pos; }
        }

        private string text;
        [Serialized(Order = 1)]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                object oldValue = text;
                text = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Text, oldValue);
            }
        }
        public bool HasText
        {
            get { return text != null; }
        }

        private bool folded;
        [Serialized(Order = 4)]
        public bool Folded
        {
            get
            {
                return folded;
            }
            set
            {
                if (folded == value) return;

                //if folding, ensure no selected descendent nodes
                if (value == true && (Tree.SelectedNodes.Count > 1 || Tree.SelectedNodes.First != this)) 
                {
                    //find selected nodes descendent of 'this' and deselect them
                    var toUnselect = Tree.SelectedNodes.Where(sNode => sNode.IsDescendent(this)).ToList(); 
                    toUnselect.ForEach(sNode => sNode.Selected = false); 
                }

                folded = value;
                Tree.FireEvent(this, NodeProperties.Folded, !folded);
            }
        }
        public bool HasFolded
        {
            get { return folded; }
        }

        private string link;
        [Serialized(Order = 5)]
        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                object oldValue = link;
                link = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Link, oldValue);
            }
        }
        public bool HasLink
        {
            get { return link != null; }
        }

        [Serialized(Order = 6)]
        public DateTime Created { get; set; }

        private DateTime modified;
        [Serialized(Order = 7)]
        public DateTime Modified
        {
            get
            {
                return modified;
            }
            set
            {
                modified = value;
            }
        }

        private bool bold;
        [Serialized(Order = 8)]
        public bool Bold
        {
            get
            {
                return bold;
            }
            set
            {
                if (bold == value) return;
                bold = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Bold, !bold);
            }
        }
        public bool HasBold
        {
            get { return bold; }
        }

        private bool italic;
        [Serialized(Order = 9)]
        public bool Italic
        {
            get
            {
                return italic;
            }
            set
            {
                if (italic == value) return;
                italic = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Italic, !italic);
            }
        }
        public bool HasItalic
        {
            get { return italic; }
        }

        private bool strikeout;
        [Serialized(Order = 10)]
        public bool Strikeout
        {
            get { return strikeout; }
            set
            {
                if (strikeout == value) return;
                strikeout = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Strikeout, !strikeout);
            }
        }
        public bool HasStrikeout
        {
            get { return strikeout; }
        }

        private string fontName;
        [Serialized(Order = 11)]
        public string FontName {
            get
            {
                return fontName;
            }
            set
            {
                object oldValue = fontName;
                fontName = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.FontName, oldValue);
            }
        }
        public bool HasFontName
        {
            get { return fontName != null; }
        }

        private float fontSize;
        /// <summary>
        /// 0 is the default value, meaning Font Size is not defined (default size should be used)
        /// </summary>
        [Serialized(Order = 12)]
        public float FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                object oldValue = fontSize;
                fontSize = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.FontSize, oldValue);
            }
        }
        public bool HasFontSize
        {
            get { return fontSize != 0; }
        }

        private Color backColor;
        /// <summary>
        /// Default value is Color.Empty
        /// </summary>
        [Serialized(Order = 13)]
        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                object oldValue = backColor;
                backColor = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.BackColor, oldValue);
            }
        }
        public bool HasBackColor
        {
            get { return !backColor.IsEmpty; } 
        }

        private Color color;
        /// <summary>
        /// Default value is Color.Empty
        /// </summary>
        [Serialized(Order = 14)]
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                object oldValue = color;
                color = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Color, oldValue);
            }
        }
        public bool HasColor
        {
            get { return !color.IsEmpty; }
        }

        private NodeShape shape;
        [Serialized(Order = 15)]
        public NodeShape Shape
        {
            get
            {
                return shape;
            }
            set
            {
                object oldValue = shape;
                shape = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Shape, oldValue);
            }
        }
        public bool HasShape
        {
            get { return shape != NodeShape.None; }
        }

        private int lineWidth;
        /// <summary>
        /// 0 stands for default line width (as parent)
        /// </summary>
        [Serialized(Order = 16)]
        public int LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                object oldValue = lineWidth;
                lineWidth = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.LineWidth, oldValue);
            }
        }
        public bool HasLineWidth
        {
            get { return lineWidth != 0; }
        }

        private System.Drawing.Drawing2D.DashStyle linePattern = System.Drawing.Drawing2D.DashStyle.Custom;
        /// <summary>
        /// Custom stands for default (as parent)
        /// </summary>
        [Serialized(Order = 17)]
        public System.Drawing.Drawing2D.DashStyle LinePattern
        {
            get
            {
                return linePattern;
            }
            set
            {
                object oldValue = linePattern;
                linePattern = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.LinePattern, oldValue);
            }
        }
        public bool HasLinePattern
        {
            get { return linePattern != DashStyle.Custom; }
        }

        private Color lineColor;
        [Serialized(Order = 18)]
        public Color LineColor
        {
            get
            {
                return lineColor;
            }
            set
            {
                object oldValue = lineColor;
                lineColor = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.LineColor, oldValue);
            }
        }
        public bool HasLineColor
        {
            get { return !lineColor.IsEmpty; }
        }

        private string noteText;
        [Serialized(Order = 19)]
        public string NoteText
        {
            get
            {
                return noteText;
            }
            set
            {
                object oldValue = noteText;
                noteText = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.NoteText, oldValue);
            }
        }
        public bool HasNoteText
        {
            get { return noteText != null; }
        }

        [Serialized(Order = 20)]
        public string Image
        {
            get { return props?.Image; }
            set
            {
                InitializeProps();
                string oldValue = props.Image;
                props.Image = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Image, oldValue); 
            }
        }
        public bool HasImage
        {
            get { return props?.Image != null; }
        }
                
        /// <summary>
        /// Alignment of text in relation to image.
        /// This property is only applicable if node has an image.
        /// </summary>
        [Serialized(Order = 21)]
        public ImageAlignment ImageAlignment
        {
            get { return props == null? ImageAlignment.Default : props.ImageAlignment; }
            set
            {
                InitializeProps();
                ImageAlignment oldValue = props.ImageAlignment;
                props.ImageAlignment = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.ImageAlignment, oldValue);                
            }
        }
        public bool HasImageAlignment
        {
            get { return props != null && props.ImageAlignment != ImageAlignment.Default; }
        }
                
		[Serialized(Order =22)]
        public Size ImageSize
        {
            get { return props == null ? Size.Empty : props.ImageSize; }
            set
            {
                InitializeProps();
                Size oldValue = props.ImageSize;
                props.ImageSize = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.ImageSize, oldValue);
            }
        }
        public bool HasImageSize
        {
            get { return props != null && !props.ImageSize.IsEmpty; }
        }

        private string label;
        [Serialized(Order = 23)]
        public string Label
        {
            get { return label; }
            set
            {
                string oldValue = Label;
                label = value;
                modified = DateTime.Now;
                Tree.FireEvent(this,NodeProperties.Label, oldValue);
            }
        }
        public bool HasLabel
        {
            get { return label != null; }
        }

        [Serialized(Order = 24)]
        public IconList Icons { get; private set; }
        public bool HasIcons
        {
            get { return Icons != null && Icons.Count > 0; }
        }

        private AdvancedProperties props;
        private void InitializeProps()
        {
            if (props == null)
            {
                props = new AdvancedProperties();
            }
        }

        #endregion

        #region Non-serializable
        public MapTree Tree { get; private set; }
        public MapNode Parent { get; private set; }
        public MapNode Previous { get; private set; }
        public MapNode Next { get; private set; }
        public MapNode FirstChild { get; set; }
        public MapNode LastChild { get; set; }
        public MapNode LastSelectedChild { get; set; }

        #endregion

        #endregion

        #region Supporting Properties (without attributes)

        public bool HasChildren { get { return FirstChild != null; } }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Creates root node
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="text"></param>
        /// <param name="id"></param>
        public MapNode(MapTree tree, string text, string id = null)
        {
            System.Diagnostics.Debug.Assert(text != null, "text parameter should not be null.");

            this.Id = id;
            this.text = text;
            this.Created = DateTime.Now;
            this.modified = DateTime.Now;
            this.Icons = new IconList(this);

            // setting NodePosition
            this.pos = NodePosition.Root;

            // attaching to tree
            this.Tree = tree;
            tree.RootNode = this;

            Tree.FireEvent(this, TreeStructureChange.New);
        }

        /// <summary>
        /// Creates a child node. 
        /// </summary>
        /// <param name="parent">Should not be null</param>
        /// <param name="text"></param>
        /// <param name="pos">If undefined, determined from parent node. In case of root node, balances the tree.</param>
        /// <param name="id">could be null</param>  
        /// <param name="adjacentToSib">Appended at the end if null.</param>
        public MapNode(MapNode parent, string text, NodePosition pos = NodePosition.Undefined,
            string id = null, MapNode adjacentToSib = null, bool insertAfterSib = true)
        {
            System.Diagnostics.Debug.Assert(parent != null, "parent parameter should not be null. Use other constructor for root node.");
            System.Diagnostics.Debug.Assert(text != null, "text parameter should not be null.");

            this.Id = id;
            this.text = text;
            this.Created = DateTime.Now;
            this.modified = DateTime.Now;
            this.Icons = new IconList(this);

            // attaching to tree
            AttachTo(parent, adjacentToSib, insertAfterSib, pos, false);

            Tree.FireEvent(this, TreeStructureChange.New);
        }

        private NodePosition GetNodePositionToBalance()
        {

            var leftNodeCnt = 0;
            var rightNodeCnt = 0;

            foreach (var node in ChildNodes)
            {
                if (node.Pos == NodePosition.Left)
                    leftNodeCnt++;
                else
                    rightNodeCnt++;
            }

            return leftNodeCnt < rightNodeCnt ? NodePosition.Left : NodePosition.Right;
        }

        public void AttachTo(MapNode parent, MapNode adjacentToSib = null, bool insertAfterSib = true,
                    NodePosition pos = NodePosition.Undefined, bool raiseAttachEvent = true)
        {
            //Debug.Assert(!(adjacentToSib == null && insertAfterSib == false)); //AttachTo handles this case

            this.Parent = parent;
            this.Tree = parent.Tree;

            // setting NodePosition
            if (pos != NodePosition.Undefined) this.pos = pos;
            else if (parent == null) this.pos = NodePosition.Root;
            else if (adjacentToSib != null) this.pos = adjacentToSib.Pos;
            else if (parent.Pos == NodePosition.Root) this.pos = parent.GetNodePositionToBalance();
            else this.pos = parent.Pos;

            // get the last sib if appendAfter is not given
            if (adjacentToSib == null) adjacentToSib = parent.GetLastChild(this.Pos);

            // if last child is not available on the given pos, then try on the other side
            if (adjacentToSib == null)
            {
                if (this.Pos == NodePosition.Left)
                {
                    adjacentToSib = parent.LastChild;
                    insertAfterSib = true;
                }
                else
                {
                    adjacentToSib = parent.FirstChild;
                    insertAfterSib = false;
                }
            }

            // link with siblings
            if (adjacentToSib != null && insertAfterSib == true)
            {
                this.Previous = adjacentToSib;
                this.Next = adjacentToSib.Next;
                adjacentToSib.Next = this;
                if (this.Next != null) this.Next.Previous = this;
            }
            else if(adjacentToSib != null && insertAfterSib == false)
            {
                this.Previous = adjacentToSib.Previous;
                this.Next = adjacentToSib;
                if(this.Previous != null) this.Previous.Next = this;
                adjacentToSib.Previous = this;
            }
            else
            {
                this.Previous = null;
                this.Next = null;
            }

            // link with parent
            if (this.Previous == null) parent.FirstChild = this;
            if (this.Next == null) parent.LastChild = this;

            if (this.HasChildren)
            {
                ForEach(n =>
                {
                    n.pos = this.pos;
                    n.Tree = this.Tree;
                });
            }

            parent.modified = DateTime.Now;
            if(raiseAttachEvent)    Tree.FireEvent(this, TreeStructureChange.Attached);


        }
        #endregion


        #region Detached Node

        /// <summary>
        /// Post Conditions: Detached == true && Selected == false
        /// </summary>
        public void Detach()
        {
            if (Parent != null)
            {
                Selected = false;

                if (Parent.FirstChild == this) Parent.FirstChild = this.Next;
                if (Parent.LastChild == this) Parent.LastChild = this.Previous;

                if (this.Previous != null)
                {
                    this.Previous.Next = this.Next;
                }

                if (this.Next != null)
                {
                    this.Next.Previous = this.Previous;
                }

                Parent.modified = DateTime.Now;

                Tree.FireEvent(this, TreeStructureChange.Detached);

            }
        }

        /// <summary>
        /// No operation should be performed on detached node or its decendents except restoring them.
        /// Descendents of a detached node are still return Detached as false.
        /// </summary>
        public bool Detached
        {
            get
            {
                return (Previous != null && Previous.Next != this) ||
                        (Next != null && Next.Previous != this) ||
                        (Parent != null && !Parent.ChildNodes.Contains(this)) ||
                        (Parent == null && Pos != NodePosition.Root);
            }
        }

        /// <summary>
        /// Create a detached node. Detached node represents deleted/cut/copied nodes. They should not be modified in any way, should only be restored.
        /// 
        /// Modifying them will generate events which is not expected.
        /// </summary>
        /// <returns></returns>
        public MapNode CloneAsDetached()
        {
            var newNode = new MapNode(Tree);
            newNode.pos = this.Pos;
            this.CopyNodePropertiesTo(newNode);
            
            foreach (MapNode childNode in this.ChildNodes)
            {
                childNode.CloneAsDetached(newNode);
            }

            return newNode;
        }

        /// <summary>
        /// Copy this node with descendents and attach it to the location (parameter)
        /// </summary>
        /// <param name="location"></param>
        private void CloneAsDetached(MapNode location)
        {
            var node = new MapNode(location.Tree);

            // attaching to tree
            node.AttachTo(location, null, true, this.pos, false);
            this.CopyNodePropertiesTo(node);

            foreach (MapNode childNode in this.ChildNodes)
            {
                childNode.CloneAsDetached(node);
            }
        }

        /// <summary>
        /// Create a detached node
        /// </summary>
        /// <param name="tree"></param>
        private MapNode(MapTree tree)
        {
            this.Created = DateTime.Now;
            this.modified = DateTime.Now;
            this.Icons = new IconList(this);

            this.Tree = tree;
        }
                
        /// <summary>
        /// Copy all current node properties on to the node passed as parameter. No property change notifications are triggered.
        /// </summary>
        /// <param name="node"></param>
        private void CopyNodePropertiesTo(MapNode node)
        {
            // node.Id, node.Created, node.Modified, node.Pos -- shouldn't be copied
            node.text = this.text;
            node.label = this.label;
            node.folded = this.folded;
                       
            node.link = this.link;

            node.noteText = this.noteText;

            if (this.props != null)
                node.props = this.props.Clone();
            else
                node.props = null;

            this.CopyAttributesTo(node);
            this.CopyIconsTo(node);
            this.CopyFormattingTo(node);
        }

        /// <summary>
        /// No property change notifications are triggered
        /// </summary>
        /// <param name="node"></param>
        private void CopyFormattingTo(MapNode node)
        {
            node.backColor = this.backColor;
            node.bold = this.bold;
            node.color = this.color;
            node.fontName = this.fontName;
            node.fontSize = this.fontSize;
            node.italic = this.italic;
            node.strikeout = this.strikeout;
            node.lineColor = this.lineColor;
            node.linePattern = this.linePattern;
            node.lineWidth = this.lineWidth;
            node.shape = this.shape;
        }

        /// <summary>
        /// No property change notiifcations are triggered
        /// </summary>
        /// <param name="node"></param>
        private void CopyAttributesTo(MapNode node)
        {
            if (this.attributeList != null)
            {
                node.EnsureAttributeListCreated();
                foreach (Attribute att in attributeList)
                {
                    node.attributeList.Add(att);
                }
            }
            else if (node.attributeList != null)
            {
                node.attributeList.Clear();
            }
        }

        /// <summary>
        /// No property change notifications are generated
        /// </summary>
        /// <param name="node"></param>
        private void CopyIconsTo(MapNode node)
        {
            node.Icons.Clear();
            foreach (string icon in this.Icons)
            {
                node.Icons.Add(icon);
            }
        }

        #endregion Detached Node

        #region Isolated Node

        /// <summary>
        /// Create an isolated node
        /// </summary>
        private MapNode(NodePosition position)
        {
            this.Created = DateTime.Now;
            this.modified = DateTime.Now;
            this.Icons = new IconList(this);

            this.pos = position;

            this.Tree = MapTree.Default;
        }

        /// <summary>
        /// An isolated node is not connected to any tree. 
        /// The Tree property of such nodes is not null, rather it is MapTree.Default.
        /// </summary>
        /// <returns></returns>
        public static MapNode CreateIsolatedNode(NodePosition position)
        {
            return new MapNode(position);
        }

        #endregion Isolatead Node

        public MapNode GetFirstSib()
        {
            return this.Parent?.FirstChild;
        }

        public MapNode GetLastSib()
        {
            return this.Parent?.LastChild;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">Left or Right. Root and Undefined are not supported.</param>
        /// <returns></returns>
        public MapNode GetFirstChild(NodePosition pos)
        {
            System.Diagnostics.Debug.Assert(pos != NodePosition.Undefined, "Undefined NodePosition is not supported.");
            System.Diagnostics.Debug.Assert(pos != NodePosition.Root, "Root NodePosition is not supported.");

            if (pos == NodePosition.Right)
            {
                return this.FirstChild;
            }
            else
            {
                MapNode cNode = FirstChild;
                while (cNode != null)
                {
                    if (pos == cNode.Pos) return cNode;
                    cNode = cNode.Next;
                }
                return null;
            }
        }

        public MapNode GetLastChild(NodePosition pos)
        {
            System.Diagnostics.Debug.Assert(pos != NodePosition.Undefined, "Undefined NodePosition is not supported.");
            System.Diagnostics.Debug.Assert(pos != NodePosition.Root, "Root NodePosition is not supported.");

            if(pos == NodePosition.Left)
            {
                return LastChild;
            }
            else
            {
                MapNode cNode = LastChild;
                while (cNode != null)
                {
                    if (pos == cNode.Pos) return cNode;
                    cNode = cNode.Previous;
                }
                return null;
            }
        }

        public IEnumerable<MapNode> ChildNodes
        {
            get
            {
                MapNode cNode = this.FirstChild;

                if (cNode != null)
                {
                    do
                    {
                        yield return cNode;
                        cNode = cNode.Next;
                    }
                    while (cNode != null);
                }
            }
        }

        public IEnumerable<MapNode> ChildRightNodes
        {
            get
            {
                MapNode cNode = this.FirstChild;

                if (cNode != null && cNode.Pos == NodePosition.Right)
                {
                    do
                    {
                        yield return cNode;
                        cNode = cNode.Next;
                    }
                    while (cNode != null && cNode.Pos == NodePosition.Right);
                }
            }
        }

        public IEnumerable<MapNode> ChildLeftNodes
        {
            get
            {
                MapNode cNode = this.GetFirstChild(NodePosition.Left);

                if (cNode != null)
                {
                    do
                    {
                        yield return cNode;
                        cNode = cNode.Next;
                    }
                    while (cNode != null);
                }
            }
        }


        private void ChangePos(NodePosition pos)
        {
            ForEach(n => n.pos = pos);
            Tree.FireEvent(this, pos == NodePosition.Left? TreeStructureChange.MovedLeft : TreeStructureChange.MovedRight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">node to be tested for being ancestor of 'this' node</param>
        /// <returns></returns>
        public bool IsDescendent(MapNode node)
        {
            if(this.Parent != null)
            {
                if (this.Parent == node)
                    return true;
                else
                {
                    return this.Parent.IsDescendent(node);
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the location of sibling (above this node, below this node or not sibling)
        /// </summary>
        /// <param name="sibling"></param>
        public SiblingLocaton GetSiblingLocation(MapNode sibling)
        {
            if(sibling.Parent == this.Parent)
            {
                MapNode temp = this.Parent.FirstChild;
                do
                {
                    if(temp == sibling)
                    {
                        return SiblingLocaton.Above;
                    }
                    else if(temp == this)
                    {
                        return SiblingLocaton.Below;
                    }
                    else
                    {
                        temp = temp.Next;
                    }
                } while (true);
            }
            else
            {
                return SiblingLocaton.NotSibling;
            }
        }

        public enum SiblingLocaton { NotSibling, Above, Below }


        public void DeleteNode()
        {
            if (this.Parent == null)    return;

            Selected = false;

            if (Parent.FirstChild == this) Parent.FirstChild = this.Next;
            if (Parent.LastChild == this) Parent.LastChild = this.Previous;

            if (this.Previous != null)
            {
                this.Previous.Next = this.Next;
            }
            if (this.Next != null)
            {
                this.Next.Previous = this.Previous;
            }

            if (Parent.LastSelectedChild == this) { Parent.LastSelectedChild = null; }

            Parent.modified = DateTime.Now;

            Tree.FireEvent(this, TreeStructureChange.Deleted);
        }


        public bool MoveUp()
        {
            if (this.Pos == NodePosition.Root) // return if root
                return false;

            if(this.Previous == null) // if previous node is null, move from left to right else do nothing
            {
                if (this.Pos == NodePosition.Left && this.Parent.pos == NodePosition.Root)
                {
                    this.ChangePos(NodePosition.Right);
                    return true;
                }
                else
                    return false;
            }

            if (this.Previous.Pos != this.Pos) // move from left to right side
            {
                this.ChangePos(this.Previous.Pos);
                return true;
            }

            // move up
            MapNode previousNode = this.Previous;

            previousNode.Next = this.Next;
            if(this.Next != null) this.Next.Previous = previousNode;

            this.Previous = previousNode.Previous;
            if (previousNode.Previous != null)  previousNode.Previous.Next = this;

            this.Next = previousNode;
            previousNode.Previous = this;

            if (this.Previous == null) Parent.FirstChild = this;
            if (Parent.LastChild == this) Parent.LastChild = this.Next;

            Parent.modified = DateTime.Now;
            Tree.FireEvent(this, TreeStructureChange.MovedUp);

            return true;
        }

        public bool MoveDown()
        {
            if (this.Pos == NodePosition.Root) // return if root
                return false;

            if(this.Next == null) // if next node is null, move from right to left, else do nothing
            {
                if (this.Pos == NodePosition.Right && this.Parent.pos == NodePosition.Root)
                {
                    this.ChangePos(NodePosition.Left);
                    return true;
                }
                else
                    return false;
            }

            if(this.Next.Pos != this.Pos) // move from right to left
            {
                this.ChangePos(this.Next.Pos);
                return true;
            }


            // move down
            MapNode nextNode = this.Next;

            nextNode.Previous = this.Previous;
            if (this.Previous != null) this.Previous.Next = nextNode;

            this.Next = nextNode.Next;
            if (nextNode.Next != null) nextNode.Next.Previous = this;

            this.Previous = nextNode;
            nextNode.Next = this;

            if (this == Parent.FirstChild) Parent.FirstChild = this.Previous;
            if (this.Next == null) Parent.LastChild = this;

            Parent.modified = DateTime.Now;
            Tree.FireEvent(this, TreeStructureChange.MovedDown);

            return true;
        }

        /// <summary>
        /// Finds the first node that matches the provided condition.
        /// Node and it's descendents are searched.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>MapNode that matches the condition or null if no such node found</returns>"></param>
        public MapNode Find(Func<MapNode, bool> condition)
        {
            if (condition(this)) return this;

            foreach (MapNode n in this.ChildNodes)
            {
                MapNode result = n.Find(condition);
                if (result != null)
                    return result;
            }

            return null;
        }

        /// <summary>
        /// Finds node and its descendents which match the condition 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<MapNode> FindAll(Func<MapNode, bool> condition)
        {
            var list = new List<MapNode>();
            FindAll(condition, list);
            return list;
        }

        private void FindAll(Func<MapNode, bool> condition, List<MapNode> list)
        {
            if (condition(this))
            {
                list.Add(this);
            }

            foreach (MapNode n in this.ChildNodes)
            {
                n.FindAll(condition, list);
            }
        }

        /// <summary>
        /// Performs the given action for node and it's descendents
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<MapNode> action)
        {
            action(this);

            foreach(MapNode cNode in this.ChildNodes)
            {
                cNode.ForEach(action);
            }
        }

        /// <summary>
        /// Performs the given action for node and it's descendents
        /// </summary>
        /// <param name="action">action to be performed on each MapNode</param>
        /// <param name="includeDescendents">Should action be performed on descendents of node. Provide a way to skip branches. For example, this can be used to include only unfolded branches.</param>
        public void ForEach(Action<MapNode> action, Func<MapNode, bool> includeDescendents)
        {
            action(this);

            if (includeDescendents(this))
            {
                foreach (MapNode cNode in this.ChildNodes)
                {
                    cNode.ForEach(action, includeDescendents);
                } 
            }
        }

        /// <summary>
        /// Performs the given action for node's ancestors (excluding the node itself)
        /// </summary>
        /// <param name="action"></param>
        public void ForEachAncestor(Action<MapNode> action)
        {
            MapNode parent = this.Parent;
            while(parent != null)
            {
                action(parent);
                parent = parent.Parent;
            }
        }

        /// <summary>
        /// Executes the given action for all siblings excluding the node itself.
        /// </summary>
        /// <param name="action"></param>
        public void ForEachSibling(Action<MapNode> action)
        {
            //1- apply to siblings above
            MapNode node = this.Previous;
            while (node != null)
            {
                action(node);
                node = node.Previous;
            }

            //2- apply to siblings below
            node = this.Next;
            while (node != null)
            {
                action(node);
                node = node.Next;
            }
        }

        /// <summary>
        /// Returns the sibling above or the closest same level(depth) node above
        /// </summary>
        /// <returns></returns>
        public MapNode GetClosestSameLevelNodeAbove()
        {
            MapNode node = this;
            if (node.Previous != null)
            {
                return node.Previous; //found (sibling)
            }
            else if (node.Parent?.Previous != null)
            {
                var pPrevious = node.Parent.Previous;
                while (pPrevious != null)
                {
                    if (pPrevious.LastChild != null)
                    {
                        return pPrevious.LastChild; //found (same level but different parent)
                    }
                    else
                    {
                        pPrevious = pPrevious.Previous;
                    }
                }

                return null; //not found
            }
            else
            {
                return null; //not found
            }
        }

        /// <summary>
        /// Returns the sibling below or the closest same level(depth) node below
        /// </summary>
        /// <returns></returns>
        public MapNode GetClosestSameLevelNodeBelow()
        {
            MapNode node = this;
            if (node.Next != null)
            {
                return node.Next;
            }
            else if (node.Parent?.Next != null)
            {
                var pNext = node.Parent.Next;
                while (pNext != null)
                {
                    if (pNext.FirstChild != null)
                    {
                        return pNext.FirstChild;
                    }
                    else
                    {
                        pNext = pNext.Next;
                    }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Applies the given action to all nodes at the same depth from root. Action to NOT applied to 'this' node.
        /// </summary>
        /// <param name="action"></param>
        public void ForEachSameLevelNode(Action<MapNode> action)
        {
            MapNode node = this;
            do
            {
                node = node.GetClosestSameLevelNodeAbove();

                if (node != null)
                {
                    action(node);
                }
                else
                {
                    break;
                }
            }
            while (true);

            node = this;
            do
            {
                node = node.GetClosestSameLevelNodeBelow();

                if (node != null)
                {
                    action(node);
                }
                else
                {
                    break;
                }
            }
            while (true);
        }

        /// <summary>
        /// Aggregates a value from leaves up to the given node. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="calculate">function to calculate the value(T) for a node. For example, number of icons.</param>
        /// <param name="aggregate">function to aggregate value(T). For example, sum, max etc.</param>
        /// <param name="rollupCallback">function called with the rolled up value(T) for each node.</param>
        /// <param name="skipDescendents">traversing descendents are skipped if this condition is true</param>
        /// <returns></returns>
        public T RollUpAggregate<T>(Func<MapNode, T> calculate, Func<T, T, T> aggregate, Action<MapNode, T> rollupCallback, Func<MapNode, bool> skipDescendents)
        {
            T value = default(T);

            if (!skipDescendents(this))
            {
                foreach (var cNode in this.ChildNodes)
                {
                    value = aggregate(
                        cNode.RollUpAggregate<T>(calculate, aggregate, rollupCallback, skipDescendents),
                        value);
                }
            }

            value = aggregate(calculate(this), value);
            rollupCallback(this, value);

            return value;
        }

        /// <summary>
        /// Aggregates a value from leaves up to the given node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="calculate">function to calculate the value(T) for a node. For example, number of icons.</param>
        /// <param name="aggregate">function to aggregate value(T). For example, sum, max etc.</param>
        /// <returns></returns>
        public T RollUpAggregate<T>(Func<MapNode, T> calculate, Func<T, T, T> aggregate)
        {
            T value = default(T);

            foreach (var cNode in this.ChildNodes)
            {
                value = aggregate(
                    cNode.RollUpAggregate<T>(calculate, aggregate),
                    value);
            }

            value = aggregate(calculate(this), value);

            return value;
        }

        /// <summary>
        /// Aggregates a value(T) from given node towards leaves. 
        /// For instance, it can be used to calculate depth from center for each node (seed should be -1 for that).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aggregate"></param>
        /// <param name="seed"></param>
        public void RollDownAggregate<T>(Func<MapNode, T, T> aggregate, T seed)
        {
            T value = aggregate(this, seed);

            foreach (var cNode in this.ChildNodes)
            {
                cNode.RollDownAggregate<T>(aggregate, value);
            }
        }

        /// <summary>
        /// Aggregates a value(T) from given node towards leaves. It can be used to calculate depth from center for each node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aggregate"></param>
        /// <param name="seed"></param>
        /// <param name="skipDescendents"></param>
        public void RollDownAggregate<T>(Func<MapNode, T, T> aggregate, T seed, Func<MapNode, T, bool> skipDescendents)
        {
            T value = aggregate(this, seed);

            if (!skipDescendents(this, value))
            {
                foreach (var cNode in this.ChildNodes)
                {
                    cNode.RollDownAggregate<T>(aggregate, value, skipDescendents);
                }
            }
        }

        public IEnumerable<MapNode> Descendents
        {
            get
            {
                var current = FirstChild;
                if (FirstChild == null) yield break;
                do
                {
                    yield return current;
                    if (current.FirstChild != null)
                        current = current.FirstChild;
                    else if (current.Next != null)
                        current = current.Next;
                    else {
                        var temp = current;
                        current = null;
                        do {
                            if(temp.Parent == this) {
                                temp = null;
                            }
                            else if (temp.Parent?.Next != null) {
                                current = temp.Parent.Next;
                                temp = null;
                            }
                            else
                                temp = temp.Parent;
                        } while (temp != null);
                    }                    
                }
                while (current != null);
            }
        }

        public NodeLinkType GetLinkType()
        {
            NodeLinkType linkType; int i;

            if (link == null)
            {
                linkType = NodeLinkType.Empty;
            }
            else if (link.StartsWith("#"))
            {
                linkType = NodeLinkType.MindMapNode;
            }
            else if (link.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || link.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                linkType = NodeLinkType.InternetLink;
            }
            else if (link.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
            {
                linkType = NodeLinkType.EmailLink;
            }
            else if (link.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                linkType = NodeLinkType.Executable;
            }
            else if (link.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
            {
                linkType = NodeLinkType.ImageFile;
            }
            else if (link.EndsWith(".mov", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".mpg", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".mpeg", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".wmv", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".flv", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".avi", StringComparison.OrdinalIgnoreCase)
                || link.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            {
                linkType = NodeLinkType.VideoFile;
            }
            else if((i = link.LastIndexOf('\\')) >= 0 && link.IndexOf('.', i) < 0)
            {
                linkType = NodeLinkType.Folder;
            }
            else
            {
                linkType = NodeLinkType.File;
            }

            return linkType;
        }

        public int GetNodeDepth()
        {
            int depth = 0;
            MapNode node = this;
            while(node.Parent != null)
            {
                depth++;
                node = node.Parent;
            }
            return depth;
        }

        public int GetDescendentsCount()
        {
            int count = -1;
            ForEach(n => count++);
            return count;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Text) &&
                   string.IsNullOrEmpty(Label) &&
                   Icons.Count == 0 &&
                   AttributeCount == 0 &&
                   NoteText == null &&
                   Image == null &&
                   !HasChildren;
        }

        public void CopyFormatTo(MapNode node)
        {
            node.BackColor = this.BackColor;
            node.Bold = this.Bold;
            node.Color = this.Color;
            node.FontName = this.FontName;
            node.FontSize = this.FontSize;
            node.Italic = this.Italic;
            node.Strikeout = this.Strikeout;
            node.LineColor = this.LineColor;
            node.LinePattern = this.LinePattern;
            node.LineWidth = this.LineWidth;
            node.Shape = this.Shape;
        }

        public void ClearFormatting()
        {
            BackColor = Color.Empty;
            Bold = false;
            Color = Color.Empty;
            FontName = null;
            FontSize = 0;
            Italic = false;
            Strikeout = false;
            LineColor = Color.Empty;
            LinePattern = DashStyle.Custom;
            LineWidth = 0;
            Shape = NodeShape.None;
        }

        public bool HasFormatting()
        {
            return HasBackColor || HasBold || HasColor || HasFontName || HasFontSize || HasItalic || HasStrikeout || HasLineColor || HasLinePattern || HasLineWidth || HasShape;
        }

        /// <summary>
        /// Unfold the node and all its descendents recursively
        /// <returns>true if any node was unfolded</returns>
        /// </summary>
        public bool UnfoldDescendents()
        {
            bool change = false;
            ForEach(n =>
            {
                if (n.Folded)
                {
                    n.Folded = false;
                    change = true;
                }

            });

            return change;
        }

        /// <summary>
        /// Fold the node and all its descendents recursively
        /// <returns>true if any node was folded</returns>
        /// </summary>
        public bool FoldDescendents()
        {
            bool change = false;
            ForEach(n =>
            {
                if (HasChildren && n.Pos != NodePosition.Root)
                {
                    n.Folded = true;
                    change = true;
                }
            });
            return change;
        }

        public void ToggleDescendentsFolding()
        {
            if (!UnfoldDescendents())
            {
                FoldDescendents();
            }
        }

        public void SortChildren(Func<MapNode, MapNode, int> compare)
        {
            if (!HasChildren) return;

            var sortedUpto = FirstChild;
            var current = sortedUpto.Next;

            while (current != null)
            {
                if (compare(sortedUpto, current) > 0)
                {
                    do
                    {
                        current.MoveUp();
                    } while (current.Previous != null && compare(current, current.Previous) < 0);
                }
                else
                {
                    sortedUpto = current;
                }
                current = sortedUpto.Next;
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="reduceSize">reduces the image size to default maximum</param>
        public void InsertImage(Image image, bool reduceSize)
        {
            //figure out image extension            
            string ext = ImageHelper.GetExtension(image);
            

            //add image to node
            var imageLOB = new ImageLob(image);
            var imageKey = LargeObjectHelper.GenerateNewKey<ImageLob>() + "." + ext;
            Tree.SetLargeObject(imageKey, imageLOB);
            Image = imageKey;    
            
            if(reduceSize)
            {
                ImageSize = ImageHelper.CalculateDefaultSize(image.Size);
            }
        }

        public Image GetImage()
        {
            if(HasImage)
            {
                return Tree.GetLargeObject<ImageLob>(Image).Image;
            }
            else
            {
                return null;
            }
        }

        public void RemoveImage()
        {
            //TODO: Remove image LOB from Tree (this should take care of undo and clipboard functionality)
            Image = null;
            ImageSize = Size.Empty;
        }

        public void SetImagePosition(ImagePosition position)
        {
            if (ImageAlignment == ImageAlignment.Default) ImageAlignment = ImageAlignment.AboveStart;
            ImageAlignment = (ImageAlignment)((int)ImageAlignment & 0b00011 | (int)position);
        }

        public void SetImageAlignment(ImageAlign align)
        {
            if (ImageAlignment == ImageAlignment.Default) ImageAlignment = ImageAlignment.AboveStart;
            ImageAlignment = (ImageAlignment)((int)ImageAlignment & 0b11100 | (int)align);
        }

        #region Select

        public bool Selected
        {
            get { return Tree.SelectedNodes.Contains(this); }
            set
            {
                if (value)
                {
                    Tree.SelectedNodes.Add(this);
                }
                else
                {
                    Tree.SelectedNodes.Remove(this);
                }
            }
        }

        /// <summary>
        /// Adds this node to the list of currently selected nodes
        /// </summary>
        public void AddToSelection()
        {
            Tree.SelectedNodes.Add(this, true);
        }

        #endregion

        public override string ToString()
        {
            return Text;
        }

        #region Node View Property
        private NodeView nodeView = null;

        public NodeView NodeView
        {
            get { return nodeView; }
            set { nodeView = value; }
        }

        #endregion

    }
}
