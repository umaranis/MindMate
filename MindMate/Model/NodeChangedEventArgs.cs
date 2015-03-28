/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Model
{
    public class NodePropertyChangedEventArgs : EventArgs
    {
        public NodeProperties ChangedProperty { get; set; }
        public object OldValue { get; set; }
    }

    public class TreeStructureChangedEventArgs : EventArgs
    {
        public TreeStructureChange ChangeType { get; set; }
    }

    public class TreeStructureChangeDescendentEventArgs : EventArgs
    {
        public TreeStructureChangeDescendent ChangeType { get; set; }
        public MapNode ChangedAncestor { get; set; }
    }

    public class IconChangedEventArgs : EventArgs
    {
        public IconChange ChangeType { get; set; }
        public string Icon { get; set; }
    }

    public class AttributeChangeEventArgs : EventArgs
    {
        public AttributeChange ChangeType { get; set; }
        public MapNode.Attribute oldValue { get; set; }
        public MapNode.Attribute newValue { get; set; }
    }
}
