using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Plugins.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Tests.TestDouble;

namespace MindMate.Tests.Plugins.Tasks
{
    [TestClass()]
    public class TaskPluginTests
    {
        //[TestMethod()]
        //public void Initialize()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void OnApplicationReady()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CreateMainMenuItems()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CreateSideBarWindows()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void OnCreatingTree()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void OnDeletingTree()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ShowDueDatePicker()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ShowDueDatePicker1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void AddSubTask()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateUsingPicker()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateToday()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateToday1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateTomorrow()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateTomorrow1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateNextWeek()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateNextWeek1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateNextMonth()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateNextMonth1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateNextQuarter()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SetDueDateNextQuarter1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CompleteTask()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RemoveTask()
        //{
        //    Assert.Fail();
        //}

        [TestMethod]
        public void AddTaskMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Add Task ...", m[0].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void DueTodayMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Due Today", m[1].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void DueTomorrowMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Tomorrow", m[2].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void DueNextWeekMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Next Week", m[3].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void DueNextMonthMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Next Month", m[4].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void DueNextQuarterMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Next Quarter", m[5].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void CompleteTaskMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Complete Task", m[6].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void RemoveTaskMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("Remove Task", m[7].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void ViewCalendarMenuItem()
        {
            var t = new TaskPlugin();
            var m = t.CreateMainMenuItems()[0].DropDownItems;
            Assert.AreEqual("View Calendar", m[8].Text); //this order of menu item is required by Ribbon event handler
        }

        [TestMethod]
        public void OpenCalendar()
        {
            var t = new TaskPlugin();
            t.Initialize(new PluginManagerStub());
            t.OpenCalender();
        }
    }
}