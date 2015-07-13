using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    static class Task
    {
        /// <summary>
        /// Checks if this attribute spec exists on the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool DueDateExists(this MapNode node)
        {
            MapTree.AttributeSpec aspec = DueDateAttribute.GetAttributeSpec(node.Tree);
            if (aspec != null)
                return node.ContainsAttribute(aspec);
            else
                return false;
        }

        /// <summary>
        /// Throws exception if there is no Due Date attribute
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DateTime GetDueDate(this MapNode node)
        {
            MapNode.Attribute att;
            DueDateAttribute.GetAttribute(node, out att);
            return DateHelper.ToDateTime(att.value);
        }

        public static void SetDueDate(this MapNode node, DateTime dateTime)
        {
            DueDateAttribute.SetDueDate(node, dateTime);
            CompletionDateAttribute.RemoveCompletionDate(node);
        }

        public static bool UpdateDueDate(this MapNode node, DateTime dueDate)
        {
            return node.UpdateAttribute(DueDateAttribute.ATTRIBUTE_NAME, DateHelper.ToString(dueDate));
        }

        public static void RemoveTask(this MapNode node)
        {
            DueDateAttribute.RemoveDueDate(node);
        }

        public static void CompleteTask(this MapNode node)
        {
            if (node.DueDateExists())
            {
                TargetDateAttribute.SetTargetDate(node, node.GetDueDate());
                DueDateAttribute.RemoveDueDate(node);

            }

            CompletionDateAttribute.SetCompletionDate(node, DateTime.Now);
        }

        /// <summary>
        /// Checks if this attribute spec exists on the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool CompletionDateExists(this MapNode node)
        {
            MapTree.AttributeSpec aspec = CompletionDateAttribute.GetAttributeSpec(node.Tree);
            if (aspec != null)
                return node.ContainsAttribute(aspec);
            else
                return false;
        }

        /// <summary>
        /// Throws exception if there is no Completion Date attribute
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DateTime GetCompletionDate(this MapNode node)
        {
            MapNode.Attribute att;
            CompletionDateAttribute.GetAttribute(node, out att);
            return DateHelper.ToDateTime(att.value);
        }

        /// <summary>
        /// Checks if this attribute spec exists on the given node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool TargetDateExists(this MapNode node)
        {
            MapTree.AttributeSpec aspec = TargetDateAttribute.GetAttributeSpec(node.Tree);
            if (aspec != null)
                return node.ContainsAttribute(aspec);
            else
                return false;
        }

        /// <summary>
        /// Throws exception if there is no Target Date attribute
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DateTime GetTargetDate(this MapNode node)
        {
            MapNode.Attribute att;
            TargetDateAttribute.GetAttribute(node, out att);
            return DateHelper.ToDateTime(att.value);
        }


    }
}
