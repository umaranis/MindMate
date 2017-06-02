using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Model
{
    public partial class MapNode
    {

        public struct Attribute
        {
            public MapTree.AttributeSpec AttributeSpec;
            public object Value;            

            public Attribute(MapTree.AttributeSpec aspec, object val)
            {
                this.AttributeSpec = aspec;
                this.Value = val;
            }

            public static Attribute Empty = new Attribute(null, null);
            public bool IsEmpty() { return AttributeSpec == null; }
            public string ValueString { get { return Value?.ToString(); } }
            public override string ToString()
            {
                return (AttributeSpec != null ? AttributeSpec.Name : "") + " : " + (ValueString ?? "");
            }
        }

        private List<Attribute> attributeList;

        /// <summary>
        /// returns null if no attributes
        /// </summary>
        [Serialized(Order = 25)]
        public IEnumerable<Attribute> Attributes { get { return attributeList; } }

        public bool HasAttributes
        {
            get { return attributeList != null && attributeList.Count > 0; }
        }

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
            if (attributeList != null)
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

                for (int i = 0; i < attributes.Length; i++)
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

            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec, NewValue = attribute.ValueString });

            attributeList.Add(attribute);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec });
        }

        public void AddAttribute(string attributeName, string value)
        {
            MapTree.AttributeSpec aSpec = Tree.GetAttributeSpec(attributeName);
            if (aSpec == null) aSpec = new MapTree.AttributeSpec(Tree, attributeName);
            MapNode.Attribute attribute = new Attribute(aSpec, value);
            AddAttribute(attribute);
        }

        public void InsertAttribute(int index, Attribute attribute)
        {
            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec, NewValue = attribute.ValueString });

            attributeList.Insert(index, attribute);

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = attribute.AttributeSpec });
        }

        public void UpdateAttribute(int index, string newValue)
        {
            Attribute oldAtt = attributeList[index];
            Attribute newAtt = new Attribute() { AttributeSpec = oldAtt.AttributeSpec, Value = newValue };

            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.ValueUpdated, AttributeSpec = newAtt.AttributeSpec, NewValue = newAtt.ValueString });

            attributeList[index] = newAtt;

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.ValueUpdated, AttributeSpec = oldAtt.AttributeSpec, oldValue = oldAtt.ValueString });
        }

        /// <summary>
        /// Returns false if attribute not found
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateAttribute(string attributeName, string value)
        {
            if (attributeList != null)
            {
                for (int i = 0; i < attributeList.Count; i++)
                {
                    if (attributeList[i].AttributeSpec.Name == attributeName)
                    {
                        UpdateAttribute(i, value);
                        return true;
                    }
                }
            }

            return false;

        }

        /// <summary>
        /// Returns false if attribute not found
        /// </summary>
        /// <param name="aSpec">AttributeSpec</param>
        /// <param name="value">New value</param>
        /// <returns></returns>
        public bool UpdateAttribute(MapTree.AttributeSpec aSpec, string value)
        {
            if (attributeList != null)
            {
                for (int i = 0; i < attributeList.Count; i++)
                {
                    if (attributeList[i].AttributeSpec == aSpec)
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
                        UpdateAttribute(i, attribute.ValueString);
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

            Tree.FireEvent(this, new AttributeChangeEventArgs() { ChangeType = AttributeChange.Removed, AttributeSpec = attribute.AttributeSpec, oldValue = attribute.ValueString });
        }

        /// <summary>
        /// Add/Update/Remove multiple attributes as an atomic transaction.
        /// Attribute Change notifications are generated after all attributes are updated. This avoids notification with intermediate / invalid state.
        /// All Attribute Changing notification are generated before applying any change.
        /// </summary>
        /// <param name="addAttributes"></param>
        /// <param name="updateAttributes">non existing attribute are ignored</param>
        /// <param name="deleteAttributes">non existing attribute are ignored</param>
        //public void AttributeBatchUpdate(Attribute[] addAttributes, Attribute[] updateAttributes, MapTree.AttributeSpec[] deleteAttributes)
        //{
        //    // 1. initialization
        //    EnsureAttributeListCreated();

        //    List<AttributeChangeEventArgs> args;
        //    int listCount = 0;
        //    int[] indexForUpdate = null;
        //    int[] indexForDelete = null;

        //    if (addAttributes != null)
        //    {
        //        listCount += addAttributes.Length;
        //    }
        //    if (updateAttributes != null)
        //    {
        //        listCount += updateAttributes.Length;
        //        indexForUpdate = new int[updateAttributes.Length];
        //    }
        //    if (deleteAttributes != null)
        //    {
        //        listCount += deleteAttributes.Length;
        //        indexForDelete = new int[deleteAttributes.Length];
        //    }

        //    args = new List<AttributeChangeEventArgs>(listCount);


        //    // 2. raise attribute changing event
        //    if (addAttributes != null)
        //    {
        //        foreach (Attribute a in addAttributes)
        //        {
        //            Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Added, AttributeSpec = a.AttributeSpec, NewValue = a.ValueString });
        //        }
        //    }
        //    if (indexForUpdate != null)
        //    {
        //        for (int i = 0; i < updateAttributes.Length; i++)
        //        {
        //            Attribute a = updateAttributes[i];
        //            indexForUpdate[i] = FindAttributeIndex(a.AttributeSpec);
        //            if (indexForUpdate[i] > -1)
        //                Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.ValueUpdated, AttributeSpec = a.AttributeSpec, NewValue = a.ValueString });
        //        }
        //    }
        //    if (indexForDelete != null)
        //    {
        //        for (int i = 0; i < deleteAttributes.Length; i++)
        //        {
        //            MapTree.AttributeSpec a = deleteAttributes[i];
        //            indexForDelete[i] = FindAttributeIndex(a);
        //            if (indexForDelete[i] > -1)
        //                Tree.FireEvent(this, new AttributeChangingEventArgs() { ChangeType = AttributeChange.Removed, AttributeSpec = a });
        //        }
        //    }

        //    // 3. update attributes, update should be done before add / delete to avoid index changes
        //    UpdateAttributesForBatch(indexForUpdate, updateAttributes, args);

        //    // 4. delete attributes, delete should be done before add to avoid index changes
        //    DeleteAttributesForBatch(indexForDelete, args);

        //    // 5. add attributes
        //    AddAttributesForBatch(addAttributes, args);


        //    // 6. raise attribute changed event
        //    args.ForEach(e => Tree.FireEvent(this, e));

        //}

        /// <summary>
        /// Add/Update/Remove multiple attributes as an atomic transaction.
        /// Attribute Change notifications are generated after all attributes are updated. This avoids notification with intermediate / invalid state.
        /// All Attribute Changing notification are generated before applying any change.
        /// </summary>
        //public void AttributeBatchUpdate(Attribute[] addUpdateAttributes, MapTree.AttributeSpec[] deleteAttributes)
        //{
        //    var addAttributes = new List<Attribute>();
        //    var updateAttributes = new List<Attribute>();
        //    if (addUpdateAttributes != null)
        //    {
        //        int[] indexes = FindAttributeIndex(addUpdateAttributes);
        //        for (int i = 0; i < indexes.Length; i++)
        //        {
        //            if (indexes[i] > -1)
        //                updateAttributes.Add(addUpdateAttributes[i]);
        //            else
        //                addAttributes.Add(addUpdateAttributes[i]);
        //        }
        //    }

        //    AttributeBatchUpdate(addAttributes.ToArray(), updateAttributes.ToArray(), deleteAttributes);
        //}

        //private void AddAttributesForBatch(Attribute[] addAttributes, List<AttributeChangeEventArgs> args)
        //{
        //    if (addAttributes != null)
        //    {
        //        for (int i = 0; i < addAttributes.Length; i++)
        //        {
        //            Attribute newAtt = addAttributes[i];

        //            attributeList.Add(newAtt);

        //            args.Add(new AttributeChangeEventArgs()
        //            {
        //                ChangeType = AttributeChange.Added,
        //                AttributeSpec = newAtt.AttributeSpec
        //            });
        //        }
        //    }
        //}

        //private void UpdateAttributesForBatch(int[] indexForUpdate, Attribute[] updateAttributes, List<AttributeChangeEventArgs> args)
        //{
        //    if (indexForUpdate != null)
        //    {
        //        for (int i = 0; i < indexForUpdate.Length; i++)
        //        {
        //            if (indexForUpdate[i] > -1)
        //            {
        //                Attribute oldAtt = attributeList[indexForUpdate[i]];

        //                attributeList[indexForUpdate[i]] = updateAttributes[i];

        //                args.Add(new AttributeChangeEventArgs()
        //                {
        //                    ChangeType = AttributeChange.ValueUpdated,
        //                    AttributeSpec = oldAtt.AttributeSpec,
        //                    oldValue = oldAtt.ValueString,
        //                });
        //            }
        //        }
        //    }
        //}

        //private void DeleteAttributesForBatch(int[] indexForDelete, List<AttributeChangeEventArgs> args)
        //{
        //    if (indexForDelete != null)
        //    {
        //        Array.Sort(indexForDelete, (a, b) => b.CompareTo(a)); // sorted in desc order so that largest index is deleted first

        //        for (int i = 0; i < indexForDelete.Length; i++)
        //        {
        //            int attIndex = indexForDelete[i];
        //            if (attIndex > -1)
        //            {
        //                Attribute oldAtt = attributeList[attIndex];

        //                this.attributeList.RemoveAt(attIndex);

        //                args.Add(new AttributeChangeEventArgs()
        //                {
        //                    ChangeType = AttributeChange.Removed,
        //                    AttributeSpec = oldAtt.AttributeSpec,
        //                    oldValue = oldAtt.ValueString
        //                });
        //            }
        //        }
        //    }
        //}
    }
}
