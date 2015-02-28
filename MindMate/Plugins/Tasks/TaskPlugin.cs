using MindMate.MetaModel;
using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public partial class TaskPlugin : IPlugin
    {
        public static NodeAttribute DueDateAttribute = new NodeAttribute("Due Date");
        /// <summary>
        /// used to store Due Date after task is completed
        /// </summary>
        public static NodeAttribute TargetDateAttribute = new NodeAttribute("Target Date");  
        public static NodeAttribute CompletionDateAttrbute = new NodeAttribute("Completion Date");

        private DateTimePicker dateTimePicker; 
        private TasksList taskList;                

        public void Initialize(IPluginManager pluginMgr)
        {
            dateTimePicker = new DateTimePicker();
            taskList = new TasksList();
            taskList.TaskViewEvent += taskList_TaskViewEvent;
        }

        void taskList_TaskViewEvent(TaskView tv, TaskView.TaskViewEvent e)
        {
            switch(e)
            { 
                case TaskView.TaskViewEvent.Remove:
                    RemoveTask(tv.MapNode);
                    break;
                case TaskView.TaskViewEvent.Complete:
                    CompleteTask(tv.MapNode);
                    break;
            }
        }
                        
                                
        public void CreateMainMenuItems(out MenuItem[] menuItems, out MainMenuLocation position)
        {
            throw new NotImplementedException();
        }

        public Control[] CreateSideBarWindows()
        {
            taskList.Text = "Tasks";
            return new Control [] { taskList };
        }

        public void OnCreatingTree(Model.MapTree tree)
        {
            tree.AttributeChanged += tree_AttributeChanged;

            tree.SelectedNodes.NodeSelected += SelectedNodes_NodeSelected;
            tree.SelectedNodes.NodeDeselected += SelectedNodes_NodeDeselected;
        }

        void SelectedNodes_NodeSelected(MapNode node, SelectedNodes selectedNodes)
        {
            MapNode.Attribute att;
            if(DueDateAttribute.GetAttribute(node, out att))
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(att.value));
                if(tv != null) tv.Selected = true;
            }
        }

        void SelectedNodes_NodeDeselected(MapNode node, SelectedNodes selectedNodes)
        {
            MapNode.Attribute att;
            if (DueDateAttribute.GetAttribute(node, out att))
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(att.value));
                if(tv != null) tv.Selected = false;
            }
        }

        private void tree_AttributeChanged(MapNode node, AttributeChangeEventArgs e)
        {
            // Due Date attribute changed
            if (e.ChangeType == AttributeChange.Removed && DueDateAttribute.SameSpec(e.oldValue.AttributeSpec)) 
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
            else if (e.ChangeType == AttributeChange.Added && DueDateAttribute.SameSpec(e.newValue.AttributeSpec))
            {
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));

                TaskDueIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if (e.ChangeType == AttributeChange.ValueUpdated && DueDateAttribute.SameSpec(e.newValue.AttributeSpec))
            {
                TaskView tv = taskList.FindTaskView(node, DateHelper.ToDateTime(e.oldValue.value));
                if (tv != null) taskList.RemoveTask(tv);
                taskList.Add(node, DateHelper.ToDateTime(e.newValue.value));
            }
            // Comletion Date attribute changed
            else if (e.ChangeType == AttributeChange.Added && CompletionDateAttrbute.SameSpec(e.newValue.AttributeSpec))
            {
                TaskCompleteIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Show);
            }
            else if(e.ChangeType == AttributeChange.Removed && CompletionDateAttrbute.SameSpec(e.oldValue.AttributeSpec))
            {
                TaskCompleteIcon.FireStatusChangeEvent(node, SystemIconStatusChange.Hide);
            }
        }
                
        public void OnDeletingTree(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }
                       
        private void CompleteTask(MapNode node)
        {
            MapNode.Attribute att;
            if (DueDateAttribute.GetAttribute(node, out att))
            {
                DueDateAttribute.Delete(node);
                TargetDateAttribute.SetValue(node, att.value);
            }

            CompletionDateAttrbute.SetValue(node, att.value);
        }

        private void RemoveTask(MapNode node)
        {
            DueDateAttribute.Delete(node);
        }

        private void SetDueDate(MapNode node, DateTime dateTime)
        {
            DueDateAttribute.SetValue(node, DateHelper.ToString(dateTime));
            CompletionDateAttrbute.Delete(node);
        }
    }
}
