using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.View.Ribbon
{
    public enum RibbonMarkupCommands : uint
    {
        cmdApplicationMenu = 1000,
        cmdButtonNew = 1001,
        cmdButtonOpen = 1002,
        cmdButtonSave = 1003,
        cmdButtonExit = 1004,
        cmdTabHome = 2000,
        cmdNewNode = 2010,
        cmdNewChildNode = 2011,
        cmdNewLongNode = 2012,
        cmdNewNodeAbove = 2013,
        cmdNewNodeBelow = 2014,
        cmdNewNodeParent = 2015,
        cmdEdit = 2030,
        cmdEditText = 2031,
        cmdEditLong = 2032,
        cmdDeleteNode = 2033,
    }
}
