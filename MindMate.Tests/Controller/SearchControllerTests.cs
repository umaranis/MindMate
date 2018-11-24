using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using MindMate.Model;
using MindMate.View.Search;

namespace MindMate.Tests.Controller
{
    [TestClass]
    public class SearchControllerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            SearchController sut = null;
            int eventNum = 1;
            Task.Run(() =>
            {
                var t = new MapTree();
                var r = new MapNode(t, "r");
                var c1 = new MapNode(r, "c1");
                var c11 = new MapNode(c1, "c11");
                var c12 = new MapNode(c1, "c12");
                var c13 = new MapNode(c1, "c13");
                var c2 = new MapNode(r, "c2");
                var c3 = new MapNode(r, "c3", NodePosition.Left);
                var c31 = new MapNode(c3, "c31");
                var c32 = new MapNode(c3, "c32");

                var taskScheduler = new TaskScheduler.TaskScheduler();
                taskScheduler.Start();
                var control = new SearchControl();
                sut = new SearchController(control, () => t, act => taskScheduler.AddTask(act, DateTime.Now));
                var form = new Form();
                form.Controls.Add(control);
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 1;
                form.Shown += (o, e) => timer.Start();                
                timer.Tick += (o, e) =>
                {
                    switch (eventNum)
                    {
                        case 1:
                            Assert.AreEqual(0, control.lstResults.Items.Count);
                            break;
                        case 2:
                            control.txtSearch.Text = "c1";
                            break;
                        case 3:
                            while (taskScheduler.TaskCount != 0) return; //return without moving to next eventNum
                            Assert.AreEqual(4, control.lstResults.Items.Count);
                            break;
                        case 4:
                            control.txtSearch.Text = "c1";
                            control.txtSearch.Text = "c1";
                            control.txtSearch.Text = "c1";
                            control.txtSearch.Text = "c1";
                            control.txtSearch.Text = "c1";
                            break;
                        case 5:
                            while (taskScheduler.TaskCount != 0) return;
                            Assert.AreEqual(4, control.lstResults.Items.Count);
                            break;
                        case 6:
                            control.txtSearch.Text = "rr";                            
                            break;
                        case 7:
                            while (taskScheduler.TaskCount != 0) return;
                            Assert.AreEqual(0, control.lstResults.Items.Count);
                            break;
                        case 8:
                            control.txtSearch.Text = "r";
                            control.btnSearch.PerformClick();
                            break;
                        case 9:
                            while (taskScheduler.TaskCount != 0) return;
                            Assert.AreEqual(1, control.lstResults.Items.Count);
                            break;
                        default:                            
                            form.Close();
                            break;
                    }
                    eventNum++;


                };
                form.ShowDialog();
                timer.Stop();
                taskScheduler.Stop();
            }).Wait();

            
        }        
    }
}
