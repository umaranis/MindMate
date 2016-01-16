﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Model;
using MindMate.Plugins.Tasks.Model;

namespace MindMate.Tests.IntegrationTest
{
    [TestClass]
    public class TaskPlugin
    {
        [TestMethod]
        public void AddTask_RetrieveIt()
        {
            // setup
            var pManager = new Stubs.PluginManagerStub();

            MindMate.Plugins.Tasks.TaskPlugin taskPlugin = new MindMate.Plugins.Tasks.TaskPlugin();
            taskPlugin.Initialize(pManager);
            taskPlugin.OnApplicationReady();

            MapTree tree = new MapTree();
            tree.RootNode = new MapNode(tree, "Center");
            tree.TurnOnChangeManager();
            taskPlugin.OnCreatingTree(tree);

            tree.RootNode.AddTask(DateTime.Now);

            // test
            Assert.IsNotNull(taskPlugin.TaskListView.FindTaskView(tree.RootNode, tree.RootNode.GetDueDate()));
        }
    }
}