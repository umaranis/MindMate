/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;

namespace MindMate.Model
{
    public sealed class NodePropertyChangedEventArgs : EventArgs
    {
        public NodeProperties ChangedProperty { get; set; }
        public object OldValue { get; set; }
    }

    public sealed class TreeStructureChangedEventArgs : EventArgs
    {
        public TreeStructureChange ChangeType { get; set; }

    }

    public sealed class IconChangedEventArgs : EventArgs
    {
        public IconChange ChangeType { get; set; }
        public string Icon { get; set; }
    }

    public sealed class AttributeChangeEventArgs : EventArgs
    {
        public AttributeChange ChangeType { get; set; }
        public MapTree.AttributeSpec AttributeSpec { get; set; }
        public string oldValue { get; set; }
    }

    public sealed class AttributeChangingEventArgs : EventArgs
    {
        public AttributeChange ChangeType { get; set; }
        public MapTree.AttributeSpec AttributeSpec { get; set; }
        public string NewValue { get; set; }
    }

    public sealed class TreeDefaultFormatChangedEventArgs : EventArgs
    {
        public TreeFormatChange ChangeType { get; set; }
        public object OldValue { get; set; }
    }
}
