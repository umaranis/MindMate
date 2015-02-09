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

        #region Attributes

        public struct Attribute 
        { 
            public MapTree.AttributeSpec AttributeSpec; 
            public string value;

            public bool IsEmpty() { return AttributeSpec == null; }
        }

        private List<Attribute> attributeList;
        public IEnumerable<Attribute> Attributes { get { return attributeList; } }

        public int AttributeCount { get { return attributeList == null ? 0 : attributeList.Count; } }

        public Attribute GetAttribute(int index) { return attributeList[index]; }

        public bool GetAttribute(string attributeName, out Attribute attribute)
        {
            if (attributeList != null)
            {
                foreach (Attribute att in attributeList)
                {
                    if (att.AttributeSpec.Name == attributeName)
                    {
                        attribute = att;
                        return true; 
                    }
                }
            }

            attribute = new Attribute() { AttributeSpec = null, value = null };
            return false;
        }

        public bool GetAttribute(MapTree.AttributeSpec attSpec, out Attribute attribute)
        {
            if(attributeList != null)
            {
                foreach (Attribute att in attributeList)
                {
                    if (att.AttributeSpec == attSpec)
                    {
                        attribute = att;
                        return true;
                    }
                }
            }

            attribute = new Attribute() { AttributeSpec = null, value = null };
            return false;
        }

        public bool ContainsAttribute(MapTree.AttributeSpec attSpec)
        {
            if (attributeList != null)
            {
                foreach (Attribute att in attributeList)
                {
                    if (att.AttributeSpec == attSpec)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ContainsAttribute(string attributeName)
        {
            if (attributeList != null)
            {
                foreach (Attribute att in attributeList)
                {
                    if (att.AttributeSpec.Name == attributeName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddAttribute(Attribute attribute)
        {
            EnsureAttributeListCreated();

            attributeList.Add(attribute);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Added, newValue = attribute });
        }

        public void UpdateAttribute(int index, string newValue)
        {
            Attribute oldAtt = attributeList[index];
            Attribute newAtt = new Attribute() { AttributeSpec = oldAtt.AttributeSpec, value = newValue };
            attributeList[index] = newAtt;

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.ValueUpdated, newValue = newAtt, oldValue = oldAtt });
        }

        /// <summary>
        /// Attribute is added if doesn't already exist. Otherwise it is updated.
        /// </summary>
        /// <param name="attribute"></param>
        public void AddUpdateAttribute(Attribute attribute)
        {
            if (attributeList != null)
            {
                for (int i = 0; i < attributeList.Count; i++)
                {
                    Attribute att = attributeList[i];
                    if (att.AttributeSpec == attribute.AttributeSpec)
                    {
                        UpdateAttribute(i, attribute.value);
                        return;
                    }
                }
            }
            AddAttribute(attribute);
        }

        

        private void EnsureAttributeListCreated()
        {
            if (attributeList == null)
                attributeList = new List<Attribute>();
        }

        public void InsertAttribute(int index, Attribute attribute)
        {
            attributeList.Insert(index, attribute);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Added, newValue = attribute });
        }

        public void DeleteAttribute(Attribute attribute)
        {
            if (attributeList.Remove(attribute))
            {
                Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Removed, oldValue = attribute });
            }
        }

        public void DeleteAttribute(string attributeName)
        {
            for(int i = 0; i < attributeList.Count; i++)
            {
                Attribute attribute = attributeList[i];
                if (attribute.AttributeSpec.Name == attributeName)
                {
                    attributeList.RemoveAt(i);

                    Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Removed, oldValue = attribute });
                }
            }
            
        }

        
                

        #endregion Attributes

        #endregion

        #region Non-serializable
        public MapTree Tree { get; private set; }
        public MapNode Parent { get; private set; }
        public MapNode Previous { get; private set; }
        public MapNode Next { get; private set; }
        public MapNode FirstChild { get; set; }
        public MapNode LastChild { get; set; }
        
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
        /// <param name="detached">Detached node doesn't have a parent but it is not set as the MapTree.RootNode</param>
        public MapNode(MapTree tree, string text, string id = null, bool detached = false) 
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
            if(!detached) tree.RootNode = this;

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
                        
            // attaching to tree
            AttachTo(parent, appendAfter, true, pos);

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
            Tree.FireEvent(this, pos == NodePosition.Left? TreeStructureChange.MovedLeft : TreeStructureChange.MovedRight);
        }

        public void AttachTo(MapNode parent, MapNode adjacentToSib = null, bool insertAfterSib = true, NodePosition pos = NodePosition.Undefined)
        {
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

            // link with parent
            if (this.Previous == null) parent.FirstChild = this;
            if (this.Next == null) parent.LastChild = this;

            if (this.HasChildren)
                ForEach(n => n.pos = this.pos);

            parent.modified = DateTime.Now;
            Tree.FireEvent(this, TreeStructureChange.Attached);
                    

        }

        public void Detach()
        {
            if (Parent != null)
            {
                Tree.FireEvent(this, TreeStructureChange.Detaching);

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

                this.Parent = null;
                this.Previous = null;
                this.Next = null;               
                
            }
        }

        public bool Detached
        {
            get { return this.Parent == null && this != this.Tree.RootNode; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">node to be tested for being ancestor of 'this' node</param>
        /// <returns></returns>
        public bool isDescendent(MapNode node)
        {
            if(this.Parent != null)
            {
                if (this.Parent == node)
                    return true;
                else
                {
                    return this.Parent.isDescendent(node);
                }
            }    
            else
            {
                return false;
            }
        }
        

        public void DeleteNode()
        {
            if (this.Parent == null)    return;

            Tree.FireEvent(this, TreeStructureChange.Deleting);

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

        /// <summary>
        /// Copy node properties on to the node passed as parameter
        /// </summary>
        /// <param name="node"></param>
        public void CopyNodePropertiesTo(MapNode node)
        {
            node.backColor = this.backColor;
            node.bold = this.bold;
            node.color = this.color;
            
            if(this.attributeList != null)
            {
                node.EnsureAttributeListCreated();
                foreach (Attribute att in attributeList)
                {
                    node.attributeList.Add(att);
                }
            }
            else if(node.attributeList != null)
            {
                node.attributeList.Clear();
            }

            node.folded = this.folded;
            node.fontName = this.fontName;
            node.fontSize = this.fontSize;

            node.Icons.Clear();
            foreach(string icon in this.Icons)
            {
                node.Icons.Add(icon);
            }
            
            // node.Id, node.Created, node.Modified, node.Pos -- shouldn't be copied
            
            node.italic = this.italic;
            node.lineColor = this.lineColor;
            node.linePattern = this.linePattern;
            node.lineWidth = this.lineWidth;
            node.link = this.link;
            node.richContentText = this.richContentText;
            node.richContentType = this.richContentType;
            node.shape = this.shape;
            node.text = this.text;            
        }

        /// <summary>
        /// Copy this node with descendents and attach it to the location (parameter)
        /// </summary>
        /// <param name="location"></param>
        /// <param name="includeDescendents"></param>
        public void CloneTo(MapNode location, bool includeDescendents = true)
        {
            var newNode = new MapNode(location, null);
            this.CopyNodePropertiesTo(newNode);

            if (includeDescendents)
            {
                foreach (MapNode childNode in this.ChildNodes)
                {
                    childNode.CloneTo(newNode);
                }
            }
        }

        public MapNode Clone(bool includeDescendents = true)
        {
            var newNode = new MapNode(Tree, null, null, true);
            this.CopyNodePropertiesTo(newNode);

            if (includeDescendents)
            {
                foreach (MapNode childNode in this.ChildNodes)
                {
                    childNode.CloneTo(newNode);
                }
            }

            return newNode;
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
