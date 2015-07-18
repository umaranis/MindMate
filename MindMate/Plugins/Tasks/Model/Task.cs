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
        /// Checks if Due Date attribute exists on the given node
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
            return DateHelper.ToDateTime(att.ValueString);
        }
        public static bool UpdateDueDate(this MapNode node, DateTime dueDate)
        {
            return node.UpdateAttribute(DueDateAttribute.ATTRIBUTE_NAME, DateHelper.ToString(dueDate));
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
            return DateHelper.ToDateTime(att.ValueString);
        }

        #region Task Status
        
        public static bool IsTaskPending(this MapNode node)
        {
            return node.DueDateExists() && node.GetTaskStatus() != TaskStatus.Complete;
        }

        public static bool IsTaskComplete(this MapNode node)
        {
            return node.CompletionDateExists();
        }

        /// <summary>
        /// Returns 'None' if TaskStatus is not set
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Returns 'None' if TaskStatus is not set</returns>
        public static TaskStatus GetTaskStatus(this MapNode node)
        {
            MapNode.Attribute att;
            if (TaskStatusAttribute.GetAttribute(node, out att))
                return (TaskStatus)Enum.Parse(typeof(TaskStatus), att.ValueString);
            else
                return TaskStatus.None;
        }

        public static void SetDueDate(this MapNode node, DateTime dateTime)
        {
            if (node.GetTaskStatus() != TaskStatus.Open)
                node.AttributeBatchUpdate(new MapNode.Attribute[]
                    {
                        new MapNode.Attribute(DueDateAttribute.GetOrCreateAttributeSpec(node.Tree), DateHelper.ToString(dateTime)),
                        new MapNode.Attribute(TaskStatusAttribute.GetOrCreateAttributeSpec(node.Tree), TaskStatus.Open.ToString())
                    }, 
                    new MapTree.AttributeSpec[] 
                    {
                        CompletionDateAttribute.GetOrCreateAttributeSpec(node.Tree)
                    });            
            else
                DueDateAttribute.SetDueDate(node, dateTime);

        }

        public static void RemoveTask(this MapNode node)
        {
            DueDateAttribute.RemoveDueDate(node);
            CompletionDateAttribute.RemoveCompletionDate(node);
            TaskStatusAttribute.RemoveTaskStatus(node);
        }

        public static void CompleteTask(this MapNode node)
        {
            TaskStatusAttribute.SetTaskStatus(node, TaskStatus.Complete);
        }
        #endregion

        #region On Attribute Change

        /// <summary>
        /// On change of an attribute makes changes to other related attributes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="e"></param>
        public static void OnAttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            if (node.Tree.Deserializing) return;

            if (e.ChangeType == AttributeChange.Added)
            {
                OnAttributeAdded(node, e);
            }
            else if (e.ChangeType == AttributeChange.Removed)
            {
                OnAttributeRemoved(node, e);
            }
            else if (e.ChangeType == AttributeChange.ValueUpdated)
            {
                OnAttributeValueUpdated(node, e);
            }
        }

        private static void OnAttributeAdded(MapNode node, AttributeChangeEventArgs e)
        {
            if (e.AttributeSpec.IsDueDate())
            {
                if (node.GetTaskStatus() == TaskStatus.None)
                {
                    TaskStatusAttribute.SetTaskStatus(node, TaskStatus.Open);
                }
            }
            else if(e.AttributeSpec.IsTaskStatus())
            {
                if(node.GetTaskStatus() == TaskStatus.Complete && !node.CompletionDateExists())
                {
                    CompletionDateAttribute.SetCompletionDate(node, DateTime.Now);
                }
            }
        }

        private static void OnAttributeValueUpdated(MapNode node, AttributeChangeEventArgs e)
        {
            if(e.AttributeSpec.IsTaskStatus())
            {
                if (node.GetTaskStatus() == TaskStatus.Complete && !node.CompletionDateExists())
                {
                    CompletionDateAttribute.SetCompletionDate(node, DateTime.Now);
                }
            }
        }

        private static void OnAttributeRemoved(MapNode node, AttributeChangeEventArgs e)
        {
            
        }

        #endregion On Attribute Change
    }
}
