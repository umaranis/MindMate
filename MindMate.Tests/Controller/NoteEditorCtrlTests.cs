using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Controller;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.Controller
{
    [TestClass()]
    public class NoteEditorCtrlTests
    {
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
            form.Size = new Size(320, 320);
            return form;
        }

        [TestMethod()]
        public void NoteEditorCtrl()
        {
            bool result = false;

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
                    var c1 = new MapNode(ptree1.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteEditorCtrl(noteEditor, persistence, null);

                    result = sut != null;

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
                    var c1 = new MapNode(ptree1.RootNode, "c1");
                    c1.Selected = true;

                    var sut = new NoteEditorCtrl(noteEditor, persistence, null);
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

        [TestMethod]
        public void ShowHtmlSourceDialog()
        {
            string result = null;

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
                    var c1 = new MapNode(ptree1.RootNode, "c1");
                    c1.NoteText = "This is a note.";
                    c1.Selected = true;

                    var sut = new NoteEditorCtrl(noteEditor, persistence, null);
                    Debugging.FormDebugHooks.Instance.ProvideShownEventHook((o, e) => {
                        var f = (HtmlSourceDialog)o;
                        foreach(Control c in f.Controls)
                        {
                            if(c.Name == "txtSource")
                            {
                                c.Text = "updated";
                                break;
                            }
                        }
                        f.DialogResult = DialogResult.OK;
                    });

                    sut.ShowHtmlSourceDialog();                    

                    result = noteEditor.HTML;

                    form.Close();
                    Debugging.FormDebugHooks.Instance.ClearHook();
                };

                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual("updated", result);
        }

        [TestMethod()]
        public void CleanHtmlCode()
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
                    var c1 = new MapNode(ptree1.RootNode, "c1");
                    c1.NoteText = "<div style='width:30px'>Testing</div>";
                    c1.Selected = true;

                    var sut = new NoteEditorCtrl(noteEditor, persistence, null);
                    sut.CleanHtmlCode();

                    noteEditor.Dirty = true; //marking as dirty manually. Automatically, it will not happen till the next event loop.

                    ptree1.RootNode.Selected = true; //deselection of c1 triggers the update of NoteText

                    result = !c1.NoteText.Contains("30");

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