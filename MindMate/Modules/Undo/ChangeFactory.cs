using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using MindMate.Modules.Undo.Changes;
using System.Drawing;

namespace MindMate.Modules.Undo
{
    class ChangeFactory
    {
        internal IChange CreateChange(MapNode node, NodePropertyChangedEventArgs e)
        {
            switch (e.ChangedProperty)
            {
                case NodeProperties.Text:
                    return new TextChange(node, (string)e.OldValue);
                case NodeProperties.Bold:
                    return new BoldChange(node, (bool)e.OldValue);
                case NodeProperties.Italic:
                    return new ItalicChange(node, (bool)e.OldValue);
                case NodeProperties.Folded:
                    return new FoldChange(node, (bool)e.OldValue);
                case NodeProperties.FontName:
                    return new FontNameChange(node, (string)e.OldValue);
                case NodeProperties.FontSize:
                    return new FontSizeChange(node, (float)e.OldValue);
                case NodeProperties.Link:
                    return new LinkChange(node, (string)e.OldValue);
                case NodeProperties.BackColor:
                    return new BackColorChange(node, (Color)e.OldValue);
                case NodeProperties.Color:
                    return new ColorChange(node, (Color)e.OldValue);
                case NodeProperties.Shape:
                    return new ShapeChange(node, (NodeShape)e.OldValue);
                case NodeProperties.LineWidth:
                    return new LineWidthChange(node, (int)e.OldValue);
                case NodeProperties.LinePattern:
                    return new LinePatternChange(node, (System.Drawing.Drawing2D.DashStyle)e.OldValue);
                case NodeProperties.LineColor:
                    return new LineColorChange(node, (Color)e.OldValue);
                case NodeProperties.RichContentType:
                    return new RichContentTypeChange(node, (NodeRichContentType)e.OldValue);
                case NodeProperties.RichContentText:
                    return new RichContextTextChange(node, (string)e.OldValue);
                    
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

        internal IChange CreateChange(MapNode node, IconChangedEventArgs e)
        {
            switch(e.ChangeType)
            {
                case IconChange.Added:
                    return new IconAdd(node, e.Icon);
                case IconChange.Removed:
                    return new IconRemove(node, e.Icon);
                default:
                    return null;
            }
        }

        internal IChange CreateChange(MapNode node, AttributeChangeEventArgs e)
        {
            switch(e.ChangeType)
            {
                case AttributeChange.Added:
                    MapNode.Attribute att;
                    node.GetAttribute(e.AttributeSpec, out att);
                    return new AttributeAdd(node, att);
                case AttributeChange.Removed:
                    return new AttributeDelete(node, new MapNode.Attribute(e.AttributeSpec, e.oldValue));
                case AttributeChange.ValueUpdated:
                    return new AttributeUpdate(node, new MapNode.Attribute(e.AttributeSpec, e.oldValue));
                default:
                    return null;
            }
        }

        internal IChange CreateChange(MapTree.AttributeSpec spec, MapTree.AttributeSpecEventArgs e)
        {
            switch(e.Change)
            {
                case MapTree.AttributeSpecChange.Added:
                    return new AttributeSpecAdd(spec);
                case MapTree.AttributeSpecChange.Removed:
                    return new AttributeSpecDelete(spec);
                case MapTree.AttributeSpecChange.NameChanged:
                    return new AttributeSpecName(spec, (string)e.OldValue);
                case MapTree.AttributeSpecChange.VisibilityChanged:
                    return new AttributeSpecVisibility(spec, (bool)e.OldValue);
                default:
                    return null;
            }
        }        
    }
}
