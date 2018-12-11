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
using System.Linq;

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
                var c121 = new MapNode(c12, "c121");
                var c13 = new MapNode(c1, "c13");
                var c131 = new MapNode(c13, "C131");
                var c2 = new MapNode(r, "c2");
                var c3 = new MapNode(r, "c3", NodePosition.Left);
                var c31 = new MapNode(c3, "c31");
                var c32 = new MapNode(c3, "c32");
                r.NoteText = "This is a note text.";
                c11.Icons.Add("button_ok");
                c11.Icons.Add("desktop_new");
                c3.Icons.Add("button_ok");
                c131.Icons.Add("desktop_new");

                var taskScheduler = new TaskScheduler.TaskScheduler();
                taskScheduler.Start();
                var control = new SearchControl();
                sut = new SearchController(control, () => t, act => taskScheduler.AddTask(act, DateTime.Now));
                var form = new Form();
                form.Controls.Add(control);
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 5;
                form.Shown += (o, e) => timer.Start();
                var shown = new EventHandler((obj, evn) => ((Form)obj).DialogResult = DialogResult.OK); //for IconSelectorExt dialog
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
                                Assert.AreEqual(6, control.lstResults.Items.Count);
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
                                Assert.AreEqual(6, control.lstResults.Items.Count);
                                control.btnSelect.PerformClick();
                                Assert.AreEqual(6, t.SelectedNodes.Count);                                
                                control.btnClear.PerformClick();
                                Assert.AreEqual(0, control.lstResults.Items.Count);
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
                                control.ckbSelectedNode.Checked = true;
                                c1.Selected = true;
                                control.txtSearch.Text = "c3";                                
                                break;
                            case 18:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(0, control.lstResults.Items.Count);
                                break;
                            case 19:
                                control.ckbSelectedNode.Checked = true;
                                c2.AddToSelection();
                                foreach (var n in c2.Descendents) n.AddToSelection();
                                control.txtSearch.Text = "c3";
                                break;
                            case 20:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(0, control.lstResults.Items.Count);
                                break;
                            case 21:
                                control.txtSearch.Text = "";
                                control.btnSearch.PerformClick();
                                break;
                            case 22:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(0, control.lstResults.Items.Count);
                                break;
                            case 23:
                                IconSelectorExt.Instance.Shown += shown;

                                IconSelectorExt.Instance.SelectedIcon = "button_ok";
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(1, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = "desktop_new";
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(2, control.CreateSearchTerm().Icons.Count);

                                control.ckbSelectedNode.Checked = false;
                                control.btnSearch.PerformClick();
                                break;
                            case 24:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(1, control.lstResults.Items.Count);
                                break;
                            case 25:
                                control.ckbAnyIcon.Checked = true;
                                control.btnSearch.PerformClick();
                                break;
                            case 26:
                                while (taskScheduler.TaskCount != 0) return;
                                Assert.AreEqual(3, control.lstResults.Items.Count);

                                IconSelectorExt.Instance.SelectedIcon = IconSelectorExt.REMOVE_ICON_NAME;
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(1, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = IconSelectorExt.REMOVE_ALL_ICON_NAME;
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(0, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.SelectedIcon = IconSelectorExt.REMOVE_ICON_NAME;
                                control.btnAddIcon.PerformClick();
                                Assert.AreEqual(0, control.CreateSearchTerm().Icons.Count);

                                IconSelectorExt.Instance.Shown -= shown;
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
