using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using MindMate.Modules.Undo.Changes;

namespace MindMate.Modules.Undo
{
    class ChangeFactory
    {
        internal IChange CreateChange(MapNode node, NodePropertyChangedEventArgs e)
        {
            switch (e.ChangedProperty)
            {
                case NodeProperties.Text:
                    return new TextChange(node, e.OldValue != null ? (string)e.OldValue : null);
                case NodeProperties.Bold:
                    return new BoldChange(node, (bool)e.OldValue);
                case NodeProperties.Italic:
                    return new ItalicChange(node, (bool)e.OldValue);
                case NodeProperties.Folded:
                    return new FoldChange(node, (bool)e.OldValue);
                    break;
                default:
                    return null;
            }
        }

        internal IChange CreateChange(MapNode node, TreeStructureChangedEventArgs e)
        {
            switch(e.ChangeType)
            {
                case TreeStructureChange.Deleted:
                case TreeStructureChange.Detached:
                    return new NodeDelete(node);
                case TreeStructureChange.Attached:
                case TreeStructureChange.New:
                    return new NodeAttach(node);
                case TreeStructureChange.MovedLeft:
                case TreeStructureChange.MovedDown:
                    return new MoveDown(node);
                case TreeStructureChange.MovedRight:
                case TreeStructureChange.MovedUp:
                    return new MoveUp(node);
                default:
                    return null;
            }
        }
    }
}
