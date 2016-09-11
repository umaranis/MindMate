using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.Dialogs;
using MindMate.View.NoteEditing;

namespace MindMate.Tests.Controller
{
    /// <summary>
    /// Test methods in this are executed in a separate thread to avoid following exception:
    ///     MindMate.Tests.View.NoteEditing.NoteEditorTests.CanExecuteCommand threw exception: 
    ///     System.Threading.ThreadStateException: ActiveX control '8856f961-340a-11d0-a96b-00c04fd705a2' cannot be instantiated because the current thread is not in a single-threaded apartment.    
    /// This exception occurs in some machines also (not sure what causes it).
    /// </summary>
    [TestClass()]
    public class NoteCtrlTests
    {
        [TestMethod()]
        public void NoteCtrl()
        {
            NoteCtrl sut = null;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var nodeEditor = new NoteEditor();
                sut = new NoteCtrl(nodeEditor, persistence);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsNotNull(sut);
        }

        [TestMethod()]
        private Form CreateForm()
        {
            Form form = new Form();
            form.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.ClientSize = new System.Drawing.Size(415, 304);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            form.KeyPreview = true;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.Name = "TestForm";
            form.ShowInTaskbar = false;
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            form.Text = "Test Form";
            form.Size = new Size(320,320);
            return form;
        }

        [TestMethod()]
        public void NoteCtrl_AssignNote_EditorUpdated()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();
                             
                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var tree = persistence.NewTree();

                    var sut = new NoteCtrl(noteEditor, persistence);

                    tree.Tree.RootNode.NoteText = "ABC";

                    result = noteEditor.HTML != null && noteEditor.HTML.Contains("ABC");

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NoteCtrl_AssignNoteToUnselected()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var tree = persistence.NewTree();
                    var c1 = new MapNode(tree.Tree.RootNode, "c1");

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    result = noteEditor.HTML == null;

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NoteCtrl_AssignNoteToUnselected_ClearNoteEditor()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var tree = persistence.NewTree();
                    var c1 = new MapNode(tree.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    c1.Parent.Selected = true;

                    result = noteEditor.HTML == null;

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NoteCtrl_MultiSelection_ClearNoteEditor()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var tree = persistence.NewTree();
                    var c1 = new MapNode(tree.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    tree.Tree.SelectedNodes.Add(c1.Parent);

                    result = noteEditor.HTML == null;

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NoteCtrl_ChangeCurrentMapTree()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var ptree1 = persistence.NewTree();
                    var c1 = new MapNode(ptree1.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    var pTree2 = persistence.NewTree();

                    result = noteEditor.HTML == null;

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void UpdateNodeFromEditor_WithoutCalling_MapNotUpdated()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var ptree1 = persistence.NewTree();
                    var c1 = new MapNode(ptree1.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    noteEditor.HTML = "EFG";
                
                    result = c1.NoteText != null && c1.NoteText.Contains("ABC");

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void UpdateNodeFromEditor()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var ptree1 = persistence.NewTree();
                    var c1 = new MapNode(ptree1.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    noteEditor.HTML = "EFG";
                    noteEditor.Dirty = true; 
                    sut.UpdateNodeFromEditor();

                    result = c1.NoteText != null && c1.NoteText.Contains("EFG");

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void UpdateNodeFromEditor_WithSettingDirty()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var ptree1 = persistence.NewTree();
                    var c1 = new MapNode(ptree1.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);

                    c1.NoteText = "ABC";

                    noteEditor.HTML = "EFG";
                    sut.UpdateNodeFromEditor();

                    result = c1.NoteText != null && c1.NoteText.Contains("ABC");

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void SetNoteEditorBackColor()
        {
            bool result = true;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                MetaModel.MetaModel.Initialize();
                var persistence = new PersistenceManager();
                var noteEditor = new NoteEditor();

                var form = CreateForm();
                form.Controls.Add(noteEditor);
                form.Shown += (sender, args) =>
                {
                    var ptree1 = persistence.NewTree();
                    var c1 = new MapNode(ptree1.Tree.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteCtrl(noteEditor, persistence);
                    sut.SetNoteEditorBackColor(Color.Azure);
                
                    result = noteEditor.BackColor.Equals(Color.Azure);

                    form.Close();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(result);
        }
    }
}