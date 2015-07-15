using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    public static class TaskStatusAttribute
    {
        public const string ATTRIBUTE_NAME = "Task Status";

        /// <summary>
        /// Checks if this attribute spec exists on the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool TaskStatusExists(MapNode node)
        {
            MapTree.AttributeSpec aspec = TaskStatusAttribute.GetAttributeSpec(node.Tree);
            if (aspec != null)
                return node.ContainsAttribute(aspec);
            else
                return false;
        }

        /// <summary>
        /// Delete this attribute from the given node if attribute exists
        /// </summary>
        /// <param name="node"></param>
        public static void RemoveTaskStatus(MapNode node)
        {
            MapTree.AttributeSpec aspec = GetAttributeSpec(node.Tree);
            if (aspec != null)
            {
                node.DeleteAttribute(aspec);
            }            
        }

        public static bool GetAttribute(MapNode node, out MapNode.Attribute attribute)
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

        public static void SetTaskStatus(MapNode node, TaskStatus value)
        {
            MapTree.AttributeSpec aspec = GetOrCreateAttributeSpec(node.Tree);
            node.AddUpdateAttribute(new MapNode.Attribute(aspec, value.ToString()));
        }

        public static bool IsTaskStatus(this MapTree.AttributeSpec aspec)
        {
            return aspec.Name == ATTRIBUTE_NAME;
        }

        private static MapTree.AttributeSpec CreateAttributeSpec(this MapTree tree)
        {
            return new MapTree.AttributeSpec(
                tree, ATTRIBUTE_NAME, true, MapTree.AttributeDataType.DateTime,
                MapTree.AttributeListOption.NoList, null, MapTree.AttributeType.System);
        }

        public static MapTree.AttributeSpec GetAttributeSpec(MapTree tree)
        {
            MapTree.AttributeSpec aspec = tree.GetAttributeSpec(ATTRIBUTE_NAME);
            return aspec;
        }

        public static MapTree.AttributeSpec GetOrCreateAttributeSpec(MapTree tree)
        {
            MapTree.AttributeSpec aspec = GetAttributeSpec(tree);
            if (aspec == null) aspec = tree.CreateAttributeSpec();

            return aspec;
        }
    }

    public enum TaskStatus
    {
        /// <summary>
        /// Not a Task (no status attribute).
        /// It is default enum value.
        /// </summary>
        None = 0,
        /// <summary>
        /// Task is open
        /// </summary>
        Open,
        /// <summary>
        /// Task is completed
        /// </summary>
        Complete
    }
}
