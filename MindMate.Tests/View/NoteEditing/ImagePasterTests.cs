using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.NoteEditing
{
    [TestClass()]
    public class ImagePasterTests
    {
        [TestMethod()]
        public void ClipboardImagePaster_HookupEvents()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    Clipboard.SetImage(Bitmap.FromFile(@"Resources\MapCtrl1.png"));
                    editor.Paste();
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();             
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result.ToLower().Contains("img"));
            Assert.AreEqual(1, result.ToLower().Where(a => a == '.').Count());
        }

        [TestMethod()]
        public void PasteFromClipboard()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                var tree = pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    Clipboard.SetImage(Bitmap.FromFile(@"Resources\MapCtrl1.png"));                    
                    ImagePaster.PasteFromClipboard(editor, tree);
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result.ToLower().Contains("img"));
        }

        [TestMethod()]
        public void PasteFromClipboard_WithEmptyClipboard()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                var tree = pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    Clipboard.Clear();
                    ImagePaster.PasteFromClipboard(editor, tree);
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ClipboardImagePaster_FileDropList()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    var fileList = new StringCollection();
                    fileList.Add(Path.GetFullPath(@"Resources\MapCtrl1.png"));
                    Clipboard.SetFileDropList(fileList);
                    editor.Paste();
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();             
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result.ToLower().Contains("img"));
            Assert.AreEqual(1, result.ToLower().Where(a => a == '.').Count());
        }

        [TestMethod()]
        public void ClipboardImagePaster_FileDropList_MultipleFiles()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    var fileList = new StringCollection();
                    fileList.Add(Path.GetFullPath(@"Resources\MapCtrl1.png"));
                    fileList.Add(Path.GetFullPath(@"Resources\MapCtrl2.png"));
                    fileList.Add(Path.GetFullPath(@"Resources\MapCtrl3.png"));
                    Clipboard.SetFileDropList(fileList);
                    editor.Paste();
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result.ToLower().Contains("img"));
            Assert.AreEqual(3, result.ToLower().Where(a => a == '.').Count());
        }

        [TestMethod()]
        public void ClipboardImagePaster_FileDropListWithNoImages()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    var fileList = new StringCollection();
                    fileList.Add(Path.GetFullPath(@"Resources\Sample Map.mm"));
                    Clipboard.SetFileDropList(fileList);
                    editor.Paste();
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ClipboardImagePaster_FileDropListWithMixedFiles()
        {
            string result = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var pManager = new PersistenceManager();
                pManager.NewTree();
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                var sut = new ImagePaster(editor, pManager);
                form.Shown += (sender, args) =>
                {
                    var fileList = new StringCollection();
                    fileList.Add(Path.GetFullPath(@"Resources\Sample Map.mm"));
                    fileList.Add(Path.GetFullPath(@"Resources\MapCtrl1.png"));
                    Clipboard.SetFileDropList(fileList);
                    editor.Paste();
                    result = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.IsTrue(result.ToLower().Contains("img"));
            Assert.AreEqual(1, result.ToLower().Where(a => a == '.').Count());
        }

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
    }
}