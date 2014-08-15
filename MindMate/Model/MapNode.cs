/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.View.MapControls;
using System.Drawing;


namespace MindMate.Model
{
    public partial class MapNode
    {    
                
        #region Node Attributes

        #region Serializable
        /// <summary>
        /// It is used for hyperlinking nodes, it is null generally.
        /// </summary>
        public string Id { get; private set; }

        private NodePosition pos;
        public NodePosition Pos
        {
            get
            {
                return pos;
            }            
        }

        private string text;
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

        private bool folded;
        public bool Folded 
        {
            get
            {
                return folded;
            }
            set
            {
                folded = value;
                Tree.FireEvent(this, NodeProperties.Folded, !folded);
            }
        }

        private bool bold;
        public bool Bold 
        {
            get
            {
                return bold;
            }
            set
            {
                bold = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Bold, !bold);
            }
        }

        private bool italic;
        public bool Italic 
        {
            get
            {
                return italic;
            }
            set
            {
                italic = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.Italic, !italic);
            }
        }

        private string fontName;
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
                Tree.FireEvent(this, NodeProperties.FontName, fontName);
            }
        }

        private float fontSize;
        /// <summary>
        /// 0 is the default value, meaning Font Size is not defined (default size should be used)
        /// </summary>
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

        public IconList Icons { get; private set; }

        private string link;
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

        public DateTime Created { get; set; }
        
        private DateTime modified;
        public DateTime Modified { 
            get
            {
                return modified;
            }
            set 
            {
                modified = value;
            }
        }

        private Color backColor;
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

        private Color color;
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

        private NodeShape shape;
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

        private int lineWidth;
        /// <summary>
        /// 0 stands for default line width (as parent)
        /// </summary>
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

        private System.Drawing.Drawing2D.DashStyle linePattern = System.Drawing.Drawing2D.DashStyle.Custom;
        /// <summary>
        /// Custom stands for default (as parent)
        /// </summary>
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

        private Color lineColor;
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

        private NodeRichContentType richContentType;
        public NodeRichContentType RichContentType
        {
            get
            {
                return richContentType;
            }
            set
            {
                object oldValue = richContentType;
                richContentType = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.RichContentType, oldValue);
            }
        }

        private string richContentText;
        public string RichContentText
        {
            get
            {
                return richContentText;
            }
            set
            {
                object oldValue = richContentText;
                richContentText = value;
                modified = DateTime.Now;
                Tree.FireEvent(this, NodeProperties.RichContentText, oldValue);
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
        
        #endregion

        private Dictionary<string, object> extendedProperties = new Dictionary<string,object>();

        public Dictionary<string, object> ExtendedProperties 
        {
            get
            {
                return extendedProperties;
            }
        }

        #endregion

        #region Supporting Properties (without attributes)

        public bool HasChildren { get { return FirstChild != null; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates root node.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="text"></param>
        /// <param name="id"></param>
        public MapNode(MapTree tree, string text, string id = null) 
        {
            this.Id = id;
            this.text = text;
            this.Created = DateTime.Now;
            this.modified = DateTime.Now;
            this.richContentType = NodeRichContentType.NONE;
            this.Icons = new IconList(this);

            // setting NodePosition
            this.pos = NodePosition.Root;
            
            // attaching to tree
            this.Tree = tree;
            tree.RootNode = this;

            Tree.FireEvent(this, TreeStructureChange.New);           
        }

        /// <summary>
        /// Creates a node. 
        /// </summary>
        /// <param name="parent">Should not be null</param>
        /// <param name="text"></param>
        /// <param name="pos">If undefined, determined from parent node. In case of root node, balances the tree.</param>
        /// <param name="id">could be null</param>  
        /// <param name="appendAfter">Appended at the end if null.</param>
        public MapNode(MapNode parent, string text, NodePosition pos = NodePosition.Undefined, 
            string id = null, MapNode appendAfter = null)
        {
            System.Diagnostics.Debug.Assert(parent != null, "parent parameter should not be null. Use other constructor for root node.");
            
            this.Id = id;
            this.text = text;
            this.Created = DateTime.Now;
            this.modified = DateTime.Now;
            this.richContentType = NodeRichContentType.NONE;
            this.Icons = new IconList(this);

            // setting NodePosition
            if      (pos != NodePosition.Undefined)         this.pos = pos;            
            else if (parent == null)                        this.pos = NodePosition.Root;         
            else if (appendAfter != null)                   this.pos = appendAfter.Pos;
            else if (parent.Pos == NodePosition.Root)       this.pos = parent.GetNodePositionToBalance();            
            else                                            this.pos = parent.Pos;

            // attaching to tree
            AttachTo(parent, appendAfter, true);

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

        #endregion
        
                                
        public MapNode GetFirstSib()
        {
            return this.Parent.FirstChild;

        }

        public MapNode GetLastSib()
        {
            return this.Parent.LastChild;
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
            modified = DateTime.Now;
            Tree.FireEvent(this, pos == NodePosition.Left? TreeStructureChange.MoveLeft : TreeStructureChange.MoveRight);
        }

        public void AttachTo(MapNode parent, MapNode adjacentToSib = null, bool insertAfterSib = true)
        {
            this.Parent = parent;
            this.Tree = parent.Tree;

            
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

            // link with parent
            if (this.Previous == null) parent.FirstChild = this;
            if (this.Next == null) parent.LastChild = this;

            parent.modified = DateTime.Now;
            Tree.FireEvent(this, TreeStructureChange.Attach);
                    

        }

        public void Detach()
        {
            if (Parent != null)
            {
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

                //this.Parent = null;
                //this.Previous = null;
                //this.Next = null;

                Parent.modified = DateTime.Now;
                Tree.FireEvent(this, TreeStructureChange.Detach);
            }
        }

        
        public bool isDescendent(MapNode node)
        {
            return Find(n => node == n) != null;            
        }
        

        public void DeleteNode()
        {
            if (this.Parent == null)    return;

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
            Tree.FireEvent(this, TreeStructureChange.Delete);
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
            Tree.FireEvent(this, TreeStructureChange.MoveUp);

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
            Tree.FireEvent(this, TreeStructureChange.MoveDown);

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
        /// <param name="function"></param>
        public void ForEach(Action<MapNode> action)
        {
            action(this);

            foreach(MapNode cNode in this.ChildNodes)
            {
                cNode.ForEach(action);
            }
        }

        public NodeLinkType GetLinkType()
        {
            NodeLinkType linkType;

            if(link == null)
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
            else if (link.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                linkType = NodeLinkType.Executable;
            }
            else
            {
                linkType = NodeLinkType.ExternalFile;
            }

            return linkType;
        }

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
