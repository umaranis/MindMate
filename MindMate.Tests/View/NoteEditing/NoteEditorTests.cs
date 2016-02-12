using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.NoteEditing
{
    [TestClass()]
    public class NoteEditorTests
    {
        [TestMethod()]
        public void NoteEditor_Creation()
        {
            var sut = new NoteEditor();
            Assert.IsNotNull(sut);
        }

        [TestMethod()]
        public void NoteEditor_Creation_HTMLIsNull()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                result = sut.HTML == null;
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void NoteEditor_Creation_IsDirtyFalse()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                result = !sut.Dirty;
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Setting HTML property programmatically doesn't make Dirty true. Only changes made through interface will make NoteEditor dirty.
        /// </summary>
        [TestMethod()]
        public void NoteEditor_SetHTML_IsDirtyFalse()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                result = !sut.Dirty;
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Clear()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                sut.HTML = "Sample Test";
                sut.Clear();
                result = sut.HTML == null;
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void CanExecuteCommand()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                result = sut.CanExecuteCommand(NoteEditorCommand.Bold);
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void QueryCommandState()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                result = !sut.QueryCommandState(NoteEditorCommand.Bold);
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void ExecuteCommand()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                sut.ExecuteCommand(NoteEditorCommand.Bold);
                result = sut.QueryCommandState(NoteEditorCommand.Bold);
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void InsertHyperlink()
        {
            var sut = new NoteEditor();
            bool result = true;
            var form = CreateForm();
            form.Controls.Add(sut);
            form.Shown += (sender, args) =>
            {
                sut.HTML = "Website";
                sut.ExecuteCommand(NoteEditorCommand.SelectAll);
                sut.InsertHyperlink("umaranis.com");
                result = sut.HTML != null && sut.HTML.Contains("umar");
                form.Close();
            };
            form.ShowDialog();

            Assert.IsTrue(result);
        }

        //[TestMethod()]
        //public void InsertImage()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Search()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Cut()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Paste()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void Copy()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void ClearUndoStack()
        //{
        //    Assert.Fail();
        //}

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
            form.Size = new Size(320, 320);
            return form;
        }
    }
}