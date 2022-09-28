using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using FakeItEasy;
using MindMate.View;
using MindMate.WinXP;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using MindMate.View.Dialogs;

namespace MindMate.Tests.Controller
{
    [TestClass()]
    public class MainCtrlTests
    {
        [TestMethod()]
        public void InitMindMate()
        {
            var sut = new MainCtrl();
            sut.InitMindMate(A.Fake<IMainForm>(), A.Fake<DialogManager>());
            Assert.IsNotNull(sut.PersistenceManager);
        }

        [TestMethod()]
        public void MapCtrl_MethodsWithNoUserInteraction()
        {
            var focus = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var sut = new MainCtrl();
                var form = new MainForm();
                sut.InitMindMate(form, A.Fake<DialogManager>());
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
                    sut.Paste();
                    sut.SetBackColor(Color.White);
                    sut.SetFontFamily("Arial");
                    sut.SetFontSize(15);
                    sut.SetForeColor(Color.Blue);
                    sut.SetMapViewBackColor(Color.White);                    
                    sut.Strikethrough(true);
                    sut.Subscript();
                    sut.Superscript();
                    sut.Underline(true);
                    sut.PersistenceManager.NewTree();
                    sut.PersistenceManager.CloseCurerntTree();
                };
                Timer timer = new Timer { Interval = 5 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
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
        public void MapCtrl_OpenXmlFormatSaveZip()
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var sut = new MainCtrl();
                var form = new MainForm();
                sut.InitMindMate(form, A.Fake<DialogManager>());
                MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, sut);
                form.MainMenuCtrl = mainMenuCtrl;
                form.Shown += (sender, args) =>
                {
                    sut.OpenMap(@"Resources\OldFormat_OverWritten_MainCtrl.mm");
                    sut.Italic(true);
                    sut.SaveCurrentMap();
                    
                };
                Timer timer = new Timer { Interval = 5 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
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
                        foreach (var f in sut.PersistenceManager)
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
        }

        [TestMethod()]
        public void InsertImage_InNote()
        {
            var imageAdded = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var sut = new MainCtrl();
                var form = new MainForm();
                DialogManager dialogs = A.Fake<DialogManager>();
                A.CallTo(() => dialogs.GetImageFile()).Returns(@"Resources\TestImage.png");
                sut.InitMindMate(form, dialogs);
                MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, sut);
                form.MainMenuCtrl = mainMenuCtrl;
                form.Shown += (sender, args) =>
                {
                    form.NoteEditor.Focus();
                    sut.InsertImage();
                    imageAdded = form.NoteEditor.HTML.Contains("IMG") || form.NoteEditor.HTML.Contains("img");
                };
                Timer timer = new Timer { Interval = 5 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
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
                        foreach (var f in sut.PersistenceManager)
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

            Assert.IsTrue(imageAdded);
        }

        [TestMethod()]
        public void MainCtrl_InsertImage_InNote_Null()
        {
            var imageAdded = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var sut = new MainCtrl();
                var form = new MainForm();
                DialogManager dialogs = A.Fake<DialogManager>();
                A.CallTo(() => dialogs.GetImageFile()).Returns(null);
                sut.InitMindMate(form, dialogs);
                MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, sut);
                form.MainMenuCtrl = mainMenuCtrl;
                form.Shown += (sender, args) =>
                {
                    form.NoteEditor.Focus();
                    sut.InsertImage();
                    imageAdded = form.NoteEditor.HTML != null && (form.NoteEditor.HTML.Contains("IMG") || form.NoteEditor.HTML.Contains("img"));
                };
                Timer timer = new Timer { Interval = 5 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
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
                        foreach (var f in sut.PersistenceManager)
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

            Assert.IsFalse(imageAdded);
        }

        [TestMethod()]
        public void InsertImage_InMap()
        {
            var imageAdded = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var sut = new MainCtrl();
                var form = new MainForm();
                DialogManager dialogs = A.Fake<DialogManager>();
                A.CallTo(() => dialogs.GetImageFile()).Returns(@"Resources\TestImage.png");
                sut.InitMindMate(form, dialogs);
                MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, sut);
                form.MainMenuCtrl = mainMenuCtrl;
                form.Shown += (sender, args) =>
                {
                    sut.ReturnFocusToMapView();
                    sut.InsertImage();
                    imageAdded = sut.CurrentMapCtrl.MapView.Tree.SelectedNodes.All(n => n.HasImage);
                };
                Timer timer = new Timer { Interval = 5 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
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
                        foreach (var f in sut.PersistenceManager)
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

            Assert.IsTrue(imageAdded);
        }

        [TestMethod()]
        public void MainCtrl_InsertImage_InMap_Null()
        {
            var imageAdded = false;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var sut = new MainCtrl();
                var form = new MainForm();
                DialogManager dialogs = A.Fake<DialogManager>();
                A.CallTo(() => dialogs.GetImageFile()).Returns(null);
                sut.InitMindMate(form, dialogs);
                MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, sut);
                form.MainMenuCtrl = mainMenuCtrl;
                form.Shown += (sender, args) =>
                {
                    sut.ReturnFocusToMapView();
                    sut.InsertImage();
                    imageAdded = sut.CurrentMapCtrl.MapView.Tree.SelectedNodes.All(n => n.HasImage);
                };
                Timer timer = new Timer { Interval = 5 }; //timer is used because the Dirty property is updated in the next event of GUI thread.
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
                        foreach (var f in sut.PersistenceManager)
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

            Assert.IsFalse(imageAdded);
        }

    }
}