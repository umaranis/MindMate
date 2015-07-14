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

            public Attribute(MapTree.AttributeSpec aspec, string val)
            {
                this.AttributeSpec = aspec;
                this.value = val;
            }

            public static Attribute Empty = new Attribute(null, null);
            public bool IsEmpty() { return AttributeSpec == null; }
            public override string ToString()
            {
                return (AttributeSpec != null ? AttributeSpec.Name : "") + " : " + (value != null ? value : "");
            }
        }

        private List<Attribute> attributeList;

        /// <summary>
        /// returns null if no attributes
        /// </summary>
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

            attribute = Attribute.Empty;
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

            attribute = Attribute.Empty;
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

        /// <summary>
        /// Returns the index of attribute. '-1' if not found.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public int FindAttributeIndex(string attributeName)
        {
            if (attributeList != null)
            {
                for (int i = 0; i < attributeList.Count; i++)
                {
                    if (attributeList[i].AttributeSpec.Name == attributeName)
                    {
                        return i;
                    }
                }
            }

            return -1;

        }

        /// <summary>
        /// Returns the index of attribute. '-1' if not found.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public int FindAttributeIndex(MapTree.AttributeSpec attribute)
        {
            if (attributeList != null)
            {
                for (int i = 0; i < attributeList.Count; i++)
                {
                    if (attributeList[i].AttributeSpec == attribute)
                    {
                        return i;
                    }
                }
            }

            return -1;

        }

        public int[] FindAttributeIndex(Attribute[] attributes)
        {
            int[] indexes = null;
            if (attributes != null)
            {
                indexes = new int[attributes.Length];

                for(int i = 0; i < attributes.Length; i++)
                {
                    indexes[i] = FindAttributeIndex(attributes[i].AttributeSpec);
                }
            }

            return indexes;
        }

        public int[] FindAttributeIndex(MapTree.AttributeSpec[] attributes)
        {
            int[] indexes = null;
            if (attributes != null)
            {
                indexes = new int[attributes.Length];

                for (int i = 0; i < attributes.Length; i++)
                {
                    indexes[i] = FindAttributeIndex(attributes[i]);
                }
            }

            return indexes;
        }

        private void EnsureAttributeListCreated()
        {
            if (attributeList == null)
                attributeList = new List<Attribute>();
        }

        public void AddAttribute(Attribute attribute)
        {
            EnsureAttributeListCreated();

            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec, NewValue = attribute.value });

            attributeList.Add(attribute);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec });
        }

        public void AddAttribute(string attributeName, string value)
        {
            MapTree.AttributeSpec aSpec = Tree.GetAttributeSpec(attributeName);
            if(aSpec == null) aSpec = new MapTree.AttributeSpec(Tree, attributeName);
            MapNode.Attribute attribute = new Attribute(aSpec, value);
            AddAttribute(attribute);
        }

        public void InsertAttribute(int index, Attribute attribute)
        {
            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec, NewValue = attribute.value });

            attributeList.Insert(index, attribute);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec });
        }

        public void UpdateAttribute(int index, string newValue)
        {
            Attribute oldAtt = attributeList[index];
            Attribute newAtt = new Attribute() { AttributeSpec = oldAtt.AttributeSpec, value = newValue };

            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.ValueUpdated, AttributeSpec = newAtt.AttributeSpec, NewValue = newAtt.value });

            attributeList[index] = newAtt;

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.ValueUpdated, AttributeSpec = oldAtt.AttributeSpec, oldValue = oldAtt.value });
        }

        /// <summary>
        /// Returns false if attribute not found
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateAttribute(string attributeName, string value)
        {
            if(attributeList != null)
            {
                for(int i = 0; i < attributeList.Count; i++)
                {
                    if(attributeList[i].AttributeSpec.Name == attributeName)
                    {
                        UpdateAttribute(i, value);
                        return true;
                    }
                }                
            }
            
            return false;
            
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

        public void DeleteAttribute(Attribute attribute)
        {
            DeleteAttribute(attribute.AttributeSpec);
        }

        public bool DeleteAttribute(string attributeName)
        {
            return DeleteAttribute(a => a.AttributeSpec.Name.Equals(attributeName));
        }

        public bool DeleteAttribute(MapTree.AttributeSpec attributeSpec)
        {
            return DeleteAttribute(a => a.AttributeSpec == attributeSpec);
        }

        /// <summary>
        /// Deletes the first occurance of Attribute that fulfils the condiion
        /// </summary>
        /// <param name="condition"></param>
        public bool DeleteAttribute(Func<Attribute, bool> condition)
        {
            for (int i = 0; i < attributeList.Count; i++)
            {
                Attribute attribute = attributeList[i];
                if (condition(attribute))
                {
                    DeleteAttribute(i);
                    return true;
                }
            }
            return false;
        }

        public void DeleteAttribute(int index)
        {
            Attribute attribute = attributeList[index];

            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Removed, AttributeSpec = attribute.AttributeSpec });

            attributeList.RemoveAt(index);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Removed, AttributeSpec = attribute.AttributeSpec, oldValue = attribute.value });
        }

        /// <summary>
        /// Add/Update/Remove multiple attributes as an atomic transaction.
        /// Attribute Change notifications are generated after all attributes are updated. This avoids notification with intermediate / invalid state.
        /// All Attribute Changing notification are generated before applying any change.
        /// </summary>
        /// <param name="addAttributes"></param>
        /// <param name="updateAttributes">non existing attribute are ignored</param>
        /// <param name="deleteAttributes">non existing attribute are ignored</param>
        public void AttributeBatchUpdate(Attribute [] addAttributes, Attribute [] updateAttributes, MapTree.AttributeSpec [] deleteAttributes)
        {
            // 1. initialization
            EnsureAttributeListCreated();

            List<AttributeChangeEventArgs> args;
            int listCount = 0;
            int[] indexForUpdate = null;
            int[] indexForDelete = null;

            if(addAttributes != null)
            {
                listCount += addAttributes.Length;                
            }
            if(updateAttributes != null)
            {
                listCount += updateAttributes.Length;
                indexForUpdate = new int[updateAttributes.Length];
            }
            if(deleteAttributes != null)
            {
                listCount += deleteAttributes.Length;
                indexForDelete = new int[deleteAttributes.Length];
            }

            args = new List<AttributeChangeEventArgs>(listCount);


            // 2. raise attribute changing event
            if (addAttributes != null)
            {
                foreach (Attribute a in addAttributes)
                {
                    Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = a.AttributeSpec, NewValue = a.value });
                }
            }
            if (indexForUpdate != null)
            {
                for (int i = 0; i < updateAttributes.Length; i++)
                {
                    Attribute a = updateAttributes[i];
                    indexForUpdate[i] = FindAttributeIndex(a.AttributeSpec);
                    if (indexForUpdate[i] > -1)
                        Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.ValueUpdated, AttributeSpec = a.AttributeSpec, NewValue = a.value });
                }
            }
            if (indexForDelete != null)
            {
                for (int i = 0; i < deleteAttributes.Length; i++)
                {
                    MapTree.AttributeSpec a = deleteAttributes[i];
                    indexForDelete[i] = FindAttributeIndex(a);
                    if (indexForDelete[i] > -1)
                        Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Removed, AttributeSpec = a });
                }                
            }

            // 3. update attributes, update should be done before add / delete to avoid index changes
            UpdateAttributesForBatch(indexForUpdate, updateAttributes, args);

            // 4. delete attributes, delete should be done before add to avoid index changes
            DeleteAttributesForBatch(indexForDelete, args);

            // 5. add attributes
            AddAttributesForBatch(addAttributes, args);


            // 6. raise attribute changed event
            args.ForEach(e => Tree.FireEvent(this, e));

        }

        /// <summary>
        /// Add/Update/Remove multiple attributes as an atomic transaction.
        /// Attribute Change notifications are generated after all attributes are updated. This avoids notification with intermediate / invalid state.
        /// All Attribute Changing notification are generated before applying any change.
        /// </summary>
        public void AttributeBatchUpdate(Attribute[] addUpdateAttributes, MapTree.AttributeSpec[] deleteAttributes)
        {
            var addAttributes = new List<Attribute>();
            var updateAttributes = new List<Attribute>();
            if(addUpdateAttributes != null)
            {
                int[] indexes = FindAttributeIndex(addUpdateAttributes);
                for(int i = 0; i < indexes.Length; i++)
                {
                    if (indexes[i] > -1)
                        updateAttributes.Add(addUpdateAttributes[i]);
                    else
                        addAttributes.Add(addUpdateAttributes[i]);
                }
            }

            AttributeBatchUpdate(addAttributes.ToArray(), updateAttributes.ToArray(), deleteAttributes);
        }

        private void AddAttributesForBatch(Attribute[] addAttributes, List<AttributeChangeEventArgs> args)
        {
            if (addAttributes != null)
            {
                for (int i = 0; i < addAttributes.Length; i++)
                {
                    Attribute newAtt = addAttributes[i];

                    attributeList.Add(newAtt);

                    args.Add(new AttributeChangeEventArgs()
                    {
                        ChangeType = AttributeChange.Added,
                        AttributeSpec = newAtt.AttributeSpec
                    });
                }
            }            
        }

        private void UpdateAttributesForBatch(int[] indexForUpdate, Attribute[] updateAttributes, List<AttributeChangeEventArgs> args)
        {
            if (indexForUpdate != null)
            {
                for (int i = 0; i < indexForUpdate.Length; i++)
                {
                    if (indexForUpdate[i] > -1)
                    {
                        Attribute oldAtt = attributeList[indexForUpdate[i]];

                        attributeList[indexForUpdate[i]] = updateAttributes[i];

                        args.Add(new AttributeChangeEventArgs()
                        {
                            ChangeType = AttributeChange.ValueUpdated,
                            AttributeSpec = oldAtt.AttributeSpec,
                            oldValue = oldAtt.value,
                        });
                    }
                }
            }            
        }

        private void DeleteAttributesForBatch(int[] indexForDelete, List<AttributeChangeEventArgs> args)
        {
            if (indexForDelete != null)
            {
                Array.Sort(indexForDelete, (a, b) => b.CompareTo(a)); // sorted in desc order so that largest index is deleted first

                for (int i = 0; i < indexForDelete.Length; i++)
                {
                    int attIndex = indexForDelete[i];
                    if (attIndex > -1)
                    {
                        Attribute oldAtt = attributeList[attIndex];                        

                        this.DeleteAttribute(attIndex);

                        args.Add(new AttributeChangeEventArgs()
                        {
                            ChangeType = AttributeChange.Removed,
                            AttributeSpec = oldAtt.AttributeSpec,
                            oldValue = oldAtt.value
                        });                        
                    }
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

        public bool Selected {
            get { return Tree.SelectedNodes.Contains(this); }
            set
            {
                if(value)
                {
                    Tree.SelectedNodes.Add(this);
                }
                else
                {
                    Tree.SelectedNodes.Remove(this);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates root node or detached node
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
            AttachTo(parent, appendAfter, true, pos, false);

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

        public void AttachTo(MapNode parent, MapNode adjacentToSib = null, bool insertAfterSib = true, 
            NodePosition pos = NodePosition.Undefined, bool raiseAttachEvent = true)
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
            if(raiseAttachEvent)    Tree.FireEvent(this, TreeStructureChange.Attached);


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
            else if((i = link.LastIndexOf('\\')) > 0 && link.IndexOf('.', i) < 0)
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
