using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.View.Ribbon
{
    public enum RibbonMarkupCommands : uint
    {
        ApplicationMenu = 1000,
        ButtonNew = 1001,
        ButtonOpen = 1002,
        ButtonSave = 1003,
        ButtonExit = 1004,
        TabHome = 2000,
        NewNode = 2010,
        NewChildNode = 2011,
        NewLongNode = 2012,
        NewNodeAbove = 2013,
        NewNodeBelow = 2014,
        NewNodeParent = 2015,
        GrpEdit = 2030,
        EditText = 2031,
        EditLong = 2032,
        DeleteNode = 2033,
        GrpClipboard = 2050,
        Paste = 2051,
        PasteAsText = 2052,
        Cut = 2053,
        Copy = 2054,
        FormatPainter = 2055,
        GrpFont = 2070,
        RichFont = 2071
    }
}
