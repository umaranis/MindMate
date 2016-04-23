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
    /// <summary>
    /// FreeMind has following three types of RichContent
    /// </summary>
    public enum NodeRichContentType { NONE, NODE, NOTE }

    public enum NodeShape { None, Fork, Bubble, Box, Bullet }

    public enum NodePosition : int { Root = 0, Left = 1, Right = 2, Undefined = -1 };

    public enum NodeProperties
    {
        Text,
        Folded,
        Bold,
        Italic,
        Strikeout,
        FontName,
        FontSize,
        Link,
        BackColor,
        Color,
        Shape,
        LineWidth,
        LinePattern,
        LineColor,
        NoteText,
        Image,
        ImageAlignment,
        Label
    }

    public enum IconChange
    {
        Added,
        Removed
    }

    public enum AttributeChange
    {
        Added,
        Removed,
        ValueUpdated
    }

    public enum TreeStructureChange
    {
        /// <summary>
        /// New Node is created. 
        /// Not invoked if 
        ///     - existing node is attached
        ///     - detached node is created
        /// </summary>
        New,       
        /// <summary>
        /// Applicable only to childs of root node
        /// </summary>
        MovedLeft,  
        /// <summary>
        /// Applicable only to childs of root node
        /// </summary>
        MovedRight, 
        /// <summary>
        /// Invoked after existing node is attached
        /// </summary>
        Attached,
        /// <summary>
        /// After a node is detached
        /// </summary>
        Detached,
        /// <summary>
        /// After a node is deleted
        /// </summary>
        Deleted,
        MovedUp,
        MovedDown
    }


    public enum NodeLinkType
    {
        File,
        ImageFile,
        VideoFile, //audio included in this
        Folder,
        MindMapNode,
        Executable,
        InternetLink,
        EmailLink,
        Empty
    }

    public enum ImageAlignment
    {
        BelowCenter,
        BelowLeft,
        BelowRight,
        AboveCenter,
        AboveLeft,
        AboveRight
    } 
    
}
