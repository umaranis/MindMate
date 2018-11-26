using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using MindMate.Model;
using MindMate.View.Dialogs;
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
            Exception exception = null;
            Task task = Task.Run(() =>
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
                r.NoteText = "This is a note text.";                

                var taskScheduler = new TaskScheduler.TaskScheduler();
                taskScheduler.Start();
                var control = new SearchControl();
                sut = new SearchController(control, () => t, act => taskScheduler.AddTask(act, DateTime.Now));
                var form = new Form();
                form.Controls.Add(control);
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 5;
                form.Shown += (o, e) => timer.Start();                
                timer.Tick += (o, e) =>
                {
                    try
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
                            case 10:
                                Assert.IsFalse(r.Selected);
                                control.lstResults.SelectedIndex = 0;
                                Assert.IsTrue(r.Selected);
                                break;
                            case 11:
                                control.ckbCase.Checked = true;
                                control.txtSearch.Text = "R";
                                control.btnSearch.PerformClick();
                                break;
                            case 12:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(0, control.lstResults.Items.Count);
                                break;
                            case 13:
                                control.txtSearch.Text = "note text";
                                control.btnSearch.PerformClick();
                                break;
                            case 14:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(1, control.lstResults.Items.Count);
                                break;
                            case 15:
                                control.ckbExcludeNote.Checked = true;
                                control.txtSearch.Text = "note text";
                                control.btnSearch.PerformClick();
                                break;
                            case 16:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(0, control.lstResults.Items.Count);
                                break;
                            case 17:
                                IconSelectorExt.Instance.Shown += (obj, evn) => ((Form)obj).DialogResult = DialogResult.OK;

                                IconSelectorExt.Instance.SelectedIcon = "button_ok";
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(1, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = "desktop_new";
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(2, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = IconSelectorExt.REMOVE_ICON_NAME;
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(1, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = IconSelectorExt.REMOVE_ALL_ICON_NAME;
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(0, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = IconSelectorExt.REMOVE_ICON_NAME;
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(0, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.Shown -= (obj, evn) => ((Form)obj).DialogResult = DialogResult.OK;
                                break;
                            default:
                                form.Close();
                                break;
                        }
                        eventNum++;
                    }
                    catch(Exception exp)
                    {
                        exception = exp;
                        timer.Stop();
                        form.Close();
                        
                    }

                };
                form.ShowDialog();
                timer.Stop();
                taskScheduler.Stop();                
            });


            task.Wait();
            if (exception != null)
                throw new Exception("Check Inner exception", exception);

        }        
    }
}
