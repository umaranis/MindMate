/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is license under MIT license (see LICENSE.txt)    
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

    public class IconChangedEventArgs : EventArgs
    {
        public IconChange ChangeType { get; set; }
        public string Icon { get; set; }
    }
}
