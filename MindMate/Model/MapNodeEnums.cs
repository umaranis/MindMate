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
        ImageSize,
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

    public enum TreeFormatChange
    {
        NodeFormat,
        MapCanvasBackColor,
        NoteEditorBackColor,
        NoteEditorForeColor,
        /// <summary>
        /// Color used to highlight the node on hover and on selection
        /// </summary>
        NodeHighlightColor,
        /// <summary>
        /// Drop target hint color during drag and drop
        /// </summary>
        NodeDropHintColor
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

    /// <summary>
    /// Combination of ImageAlign and ImagePosition.
    /// Alignment could be: Start, Center, End
    /// Position could be: Above, Below, Before, After
    /// First 2 digits designate alignment, while the following 3 position
    /// </summary>
    public enum ImageAlignment
    {
        Default = 0,
        AboveStart = 5,     // binary = 001 01 (Position Alignment)
        AboveCenter	= 6,    // 001 10
        AboveEnd = 7,       // 001 11
        BelowStart = 9,     // 010 01                               text is displayed below image aligned at the starting
        BelowCenter = 10,   // 010 10                               text is displayed below image aligned center
        BelowEnd = 11,      // 010 11
        BeforeTop = 13,     // 011 01
        BeforeCenter = 14,  // 011 10
        BeforeBottom = 15,  // 011 11
        AfterTop = 17,      // 100 01
        AfterCenter = 18,   // 100 10
        AfterBottom = 19    // 100 11                               text is displayed after image aligned to the bottom
    }

    public enum ImageAlign
    {
        Start =  0b000_01,
        Center = 0b000_10,
        End =    0b000_11
    };

    public enum ImagePosition
    {
        Above = 0b001_00,
        Below = 0b010_00,
        Before =  0b011_00,
        After = 0b100_00
    };

}
