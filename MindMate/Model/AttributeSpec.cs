using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Model
{

    public partial class MapTree { 
    public class AttributeSpec
    {
        public AttributeSpec(MapTree tree, string name, bool visible = true, AttributeDataType dataType = AttributeDataType.Alphanumeric, 
            AttributeListOption listType = AttributeListOption.OptionalList, SortedSet<string> values = null, AttributeType type = AttributeType.UserDefined)
        {
            this.Tree = tree;
            this.name = name;
            this.visible = visible;
            this.dataType = dataType;
            this.listType = listType;
            this.values = values;
            this.Type = type;

            if (values == null && ListType != AttributeListOption.NoList) this.values = new SortedSet<string>();

            if(this.type != AttributeType.Calculated)
                Tree.attributeSpecs.Add(this.Name, this);
            else
                Tree.attributeSpecs.Add(":" + this.Name, this); //attributes apart from calculated should not start from ':' as it is reserved for calculated attributes

            Tree.FireEvent(this, new AttributeSpecEventArgs() {  Change = AttributeSpecChange.Added  } );

        }

        public MapTree Tree { get; private set; }

        private string name;
        /// <summary>
        /// Name is unique across all attributes except Calculated type. Calculated type attribute has same name as the attribute it aggregates on.
        /// </summary>
        public string Name 
        {
            get { return name; }
            set 
            {
                object oldValue = name;
                name = value;
                Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.NameChanged, OldValue = oldValue });
            }
        }

        private bool visible;
        /// <summary>
        /// Show attribute in the Map
        /// </summary>
        public bool Visible 
        {
            get { return visible; }
            set
            {
                visible = value;
                Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.VisibilityChanged });
            }
        }

        private AttributeType type;
        public AttributeType Type 
        {
            get { return type; }
            set
            {
                object oldValue = type;
                type = value;
                Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.TypeChanged, OldValue = oldValue });
            }
        }

        private AttributeDataType dataType;
        public AttributeDataType DataType 
        {
            get { return dataType; }
            set
            {
                object oldValue = dataType;
                dataType = value;
                Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.DataTypedChanged, OldValue = oldValue });
            }
        }

        private AttributeListOption listType;
        /// <summary>
        /// Always NoList for DateTime and Date
        /// </summary>
        public AttributeListOption ListType 
        {
            get { return listType; }
            set
            {
                object oldValue = listType;
                listType = value;
                Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.ListTypeChanged, OldValue = oldValue });
            }
        }


        
        #region Attribute List of Values **********************

        /// <summary>
        /// List of values are maintained if AttributeListOption is RestrictedList or OptionalList
        /// </summary>
        private SortedSet<string> values;

        /// <summary>
        /// List of values are maintained if AttributeListOption is RestrictedList or OptionalList
        /// </summary>
        public IEnumerable<string> Values { get { return values; } }

        public int GetValuesCount() {  return values != null? 0 : values.Count; }

        public void AddValue(string value) 
        { 
            values.Add(value);
            Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.ListValueAdded, OldValue = value });
        }

        public bool RemoveValue(string value) 
        {
            bool success = values.Remove(value);
            if (success) Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.ListValueRemoved, OldValue = value }); 
            return success; 
        }

        #endregion Attribute List of Values **********************



        public void Delete()
        {
            Tree.attributeSpecs.Remove(Name);
            Tree.FireEvent(this, new AttributeSpecEventArgs() { Change = AttributeSpecChange.Removed });
        }


        public override string ToString()
        {
            return name;
        }
    }

    public enum AttributeDataType { Alphanumeric, Numeric, DateTime, Date }

    public enum AttributeListOption { RestrictedList, OptionalList, NoList }

    public enum AttributeSpecChange { 
        Added, Removed, 
        NameChanged, VisibilityChanged, DataTypedChanged, TypeChanged, 
        ListTypeChanged, ListValueAdded, ListValueRemoved 
    }

    public enum AttributeType { 
        /// <summary>
        /// Attributes defined by the user
        /// </summary>
        UserDefined, 
        /// <summary>
        /// System defined attributes which are visible to the user
        /// </summary>
        System, 
        /// <summary>
        /// System defined attributes which are hidden from the user
        /// </summary>
        Internal, 
        /// <summary>
        /// Calculated attributes
        /// </summary>
        Calculated 
    }

    public class AttributeSpecEventArgs : EventArgs
    {
        public AttributeSpecChange Change { get; set; }

        public object OldValue { get; set; }
    }

       
    }
}
