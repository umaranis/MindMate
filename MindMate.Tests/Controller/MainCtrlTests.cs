using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using FakeItEasy;
using MindMate.View;
using MindMate.WinXP;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace MindMate.Tests.Controller
{
    [TestClass()]
    public class MainCtrlTests
    {
        [TestMethod()]
        public void InitMindMate()
        {
            var sut = new MainCtrl();
            sut.InitMindMate(A.Fake<IMainForm>());
            Assert.IsNotNull(sut.PersistenceManager);
        }

        [TestMethod()]
        public void ReturnFocusToMapView()
        {
            var focus = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var sut = new MainCtrl();
                var form = new MainForm();
                sut.InitMindMate(form);
                MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, sut);
                form.MainMenuCtrl = mainMenuCtrl;
                form.Shown += (sender, args) =>
                {
                    sut.ReturnFocusToMapView();
                    sut.Bold(true);
                    focus = sut.CurrentMapCtrl.MapView.Tree.RootNode.Bold;
                    sut.ClearSelectionFormatting();
                    sut.Copy();
                    sut.Cut();
                    sut.SetBackColor(Color.White);
                    sut.SetFontFamily("Arial");
                    sut.SetFontSize(15);
                    sut.SetForeColor(Color.Blue);
                    sut.SetMapViewBackColor(Color.White);                    
                    sut.Strikethrough(true);
                    sut.Subscript();
                    sut.Superscript();
                    sut.Underline(true);                    
                };
                Timer timer = new Timer { Interval = 50 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
                timer.Tick += delegate
                {
                    if (timer.Tag == null)
                    {
                        timer.Tag = "First Event Fired";                        
                    }
                    else if (timer.Tag.Equals("First Event Fired"))
                    {
                        timer.Tag = "Second Event Fired";
                    }
                    else
                    {
                        foreach(var f in sut.PersistenceManager)
                        {
                            f.IsDirty = false; //to avoid save warning dialog
                        }
                        form.Close();
                    }
                };               

                timer.Start();
                form.ShowDialog();
                timer.Stop();                
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(focus);
        }

        [TestMethod()]
        public void ShowApplicationOptions()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExportAsBmp()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExportAsPng()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExportAsJpg()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShowColorPicker()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShowFontDialog()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SeekDeleteConfirmation()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShowStatusNotification()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShowMessageBox()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShowInputBox()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ShowAboutBox()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetMapViewBackColor()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ScheduleTask()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RescheduleTask()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Copy()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Cut()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Paste()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Undo()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Redo()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Bold()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Italic()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Underline()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Strikethrough()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Subscript()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void Superscript()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetFontFamily()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetFontSize()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetForeColor()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetBackColor()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ClearSelectionFormatting()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartNoteEditing()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ViewNoteTab()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ViewTaskListTab()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NewMap()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void OpenMap()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveCurrentMap()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveMap()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveCurrentMapAs()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveAll()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SaveAsMap()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CloseCurrentMap()
        {
            Assert.Fail();
        }
    }
}