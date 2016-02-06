using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Plugins.Tasks.Model
{
    public static class Task
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
        /// Get the date on which task is to be due. 
        /// Throws exception if there is no Due Date attribute.
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
            return node.DueDateExists() && !node.CompletionDateExists();
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

        public static void AddTask(this MapNode node, DateTime dateTime)
        {
            if (!node.IsTaskPending())
            {
                node.Tree.ChangeManager.StartBatch("Add Task");

                DueDateAttribute.SetDueDate(node, dateTime);
                TaskStatusAttribute.SetTaskStatus(node, TaskStatus.Open);
                CompletionDateAttribute.RemoveCompletionDate(node);

                node.Tree.ChangeManager.EndBatch();

                //node.AttributeBatchUpdate(new MapNode.Attribute[]
                //    {
                //        new MapNode.Attribute(DueDateAttribute.GetOrCreateAttributeSpec(node.Tree), DateHelper.ToString(dateTime)),
                //        new MapNode.Attribute(TaskStatusAttribute.GetOrCreateAttributeSpec(node.Tree), TaskStatus.Open.ToString())
                //    },
                //    new MapTree.AttributeSpec[]
                //    {
                //        CompletionDateAttribute.GetOrCreateAttributeSpec(node.Tree)
                //    });
            }
            else
            {
                node.Tree.ChangeManager.StartBatch("Update Task Due Date");

                if (node.StartDateExists())
                {
                    TimeSpan duration = node.GetDueDate() - node.GetStartDate();
                    node.SetStartDate(dateTime.Subtract(duration));
                }
                DueDateAttribute.SetDueDate(node, dateTime);

                node.Tree.ChangeManager.EndBatch();
            }

        }

        public static void RemoveTask(this MapNode node)
        {
            node.Tree.ChangeManager.StartBatch("Remove Task");

            DueDateAttribute.RemoveDueDate(node);
            CompletionDateAttribute.RemoveCompletionDate(node);
            TaskStatusAttribute.RemoveTaskStatus(node);
            node.RemoveStartDate();

            node.Tree.ChangeManager.EndBatch();
        }

        public static void CompleteTask(this MapNode node)
        {
            node.Tree.ChangeManager.StartBatch("Complete Task");

            CompletionDateAttribute.SetCompletionDate(node, DateTime.Now);
            TaskStatusAttribute.SetTaskStatus(node, TaskStatus.Complete);

            node.Tree.ChangeManager.EndBatch();
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

        #region Calender Dates

        public const string START_DATE_ATTRIBUTE = "Start Date";

        public static bool IsStartDate(this MapTree.AttributeSpec aSpec)
        {
            return aSpec.Name.Equals(START_DATE_ATTRIBUTE);
        }

        public static bool StartDateExists(this MapNode node)
        {
            return node.ContainsAttribute(START_DATE_ATTRIBUTE);
        }

        /// <summary>
        /// Returns Start Date of the Task.
        /// If Start Date is null, returns time 30 minutes prior to the Task Due Date or Completion Date
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DateTime GetStartDate(this MapNode node)
        {
            MapNode.Attribute attribute;
            if (node.GetAttribute(START_DATE_ATTRIBUTE, out attribute))
                return DateHelper.ToDateTime(attribute.ValueString);
            else
            {
                return node.GetEndDate() - TimeSpan.FromMinutes(30);
            }
        }

        public static void SetStartDate(this MapNode node, DateTime value)
        {
            MapTree.AttributeSpec aSpec = node.Tree.GetAttributeSpec(START_DATE_ATTRIBUTE);
            if (aSpec == null)
                aSpec = new MapTree.AttributeSpec(node.Tree, START_DATE_ATTRIBUTE, false, MapTree.AttributeDataType.DateTime, MapTree.AttributeListOption.NoList, null, MapTree.AttributeType.System);
            node.AddUpdateAttribute(new MapNode.Attribute(aSpec, DateHelper.ToString(value)));
        }

        public static void RemoveStartDate(this MapNode node)
        {
            node.DeleteAttribute(START_DATE_ATTRIBUTE);
        }

        /// <summary>
        /// End Date of the Task i.e. Due Date.  If due date doesn't exits, then completion date is returned 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static DateTime GetEndDate(this MapNode node)
        {
            if (node.DueDateExists())
                return node.GetDueDate();
            else if (node.CompletionDateExists())
                return node.GetCompletionDate();
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// Sets the DueDate or Completion Date depending on status (pending or complete)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        public static void SetEndDate(this MapNode node, DateTime value)
        {
            if (node.IsTaskComplete())
                CompletionDateAttribute.SetCompletionDate(node, value);
            else
                DueDateAttribute.SetDueDate(node, value);
        }

        #endregion
    }
}
