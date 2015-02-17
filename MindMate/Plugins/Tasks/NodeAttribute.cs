using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks
{
    public class NodeAttribute
    {
        private string attributeName;

        public NodeAttribute(string attributeName)
        {
            this.attributeName = attributeName;
        }

        /// <summary>
        /// Checks if this attribute spec exists on the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Exists(MapNode node)
        {
            MapTree.AttributeSpec aspec = GetAttributeSpec(node.Tree);
            if (aspec != null)
                return node.ContainsAttribute(aspec);
            else
                return false;
        }

        /// <summary>
        /// Delete this attribute from the given node
        /// </summary>
        /// <param name="node"></param>
        public void Delete(MapNode node)
        {
            MapTree.AttributeSpec aspec = GetAttributeSpec(node.Tree);
            if (aspec != null)
            {
                node.DeleteAttribute(aspec);
            }            
        }

        public bool GetAttribute(MapNode node, out MapNode.Attribute attribute)
        {
            MapTree.AttributeSpec aspec = GetAttributeSpec(node.Tree);
            if (aspec != null)
            {
                if (node.GetAttribute(aspec, out attribute))
                {
                    return true;
                }                
            }

            attribute = MapNode.Attribute.Empty;
            return false;
        }

        public void SetValue(MapNode node, string value)
        {
            MapTree.AttributeSpec aspec = GetOrCreateAttributeSpec(node.Tree);
            node.AddUpdateAttribute(new MapNode.Attribute(aspec, value));
        }

        public bool SameSpec(MapTree.AttributeSpec aspec)
        {
            return aspec.Name == attributeName && aspec.Type == MapTree.AttributeType.System;
        }

        private MapTree.AttributeSpec CreateAttributeSpec(MapTree tree)
        {
            return new MapTree.AttributeSpec(
                tree, attributeName, true, MapTree.AttributeDataType.DateTime,
                MapTree.AttributeListOption.NoList, null, MapTree.AttributeType.System);
        }

        private MapTree.AttributeSpec GetAttributeSpec(MapTree tree)
        {
            MapTree.AttributeSpec aspec = tree.GetAttributeSpec(attributeName);
            return (aspec != null && aspec.Type == MapTree.AttributeType.System) ? aspec : null;
        }

        private MapTree.AttributeSpec GetOrCreateAttributeSpec(MapTree tree)
        {
            MapTree.AttributeSpec aspec = GetAttributeSpec(tree);
            if (aspec == null) aspec = CreateAttributeSpec(tree);

            return aspec;
        }
    }
}
