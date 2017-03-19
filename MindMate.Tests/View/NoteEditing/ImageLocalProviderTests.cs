using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
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
    public class ImageLocalProviderTests
    {
        [TestMethod()]
        public void ImageLocalProvider()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\New Format with Images.mm");            

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                new ImageLocalProvider(p);
                form.Shown += (sender, args) =>
                {
                    editor.HTML = tree.RootNode.FirstChild.NoteText;                    
                    form.Close();
                };
                form.ShowDialog();

            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();            
        }

        /// <summary>
        /// Uses FakeItEasy, doesn't help code coverage
        /// </summary>
        [TestMethod()]
        public void ImageLocalProvider_GetUrlDataCalled()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\New Format with Images.mm");

            ImageLocalProvider sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                var form = CreateForm();
                form.Controls.Add(editor);
                sut = A.Fake<ImageLocalProvider>(x => x.WithArgumentsForConstructor(() => new ImageLocalProvider(p)));
                form.Shown += (sender, args) =>
                {
                    editor.HTML = tree.RootNode.FirstChild.NoteText;
                    form.Close();
                };
                form.ShowDialog();

            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            string contentType;
            A.CallTo(() => sut.GetUrlData("mm://33046437-1659-4d39-91dd-5a420e7c4852.png/", out contentType)).MustHaveHappened();
        }

        [TestMethod]        
        public void ImageLocalProvider_NonexistantImageOnNewTree()
        {
            var p = new PersistenceManager();
            var tree = p.NewTree();

            ImageLocalProvider sut = null;
            sut = new ImageLocalProvider(p);
            string contentType;
            sut.GetUrlData("mm://does-not-exists.png", out contentType);
            Assert.AreEqual("text/html", contentType);
        }

        [TestMethod()]        
        public void ImageLocalProvider_NonexistantImageOnSavedTree()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\New Format with Images.mm");

            ImageLocalProvider sut = null;
            sut = new ImageLocalProvider(p);
            string contentType;
            byte[] result = sut.GetUrlData("mm://does-not-exists.png", out contentType);
            CollectionAssert.AreEqual(Encoding.UTF8.GetBytes(@"<b>Page not found!</b>"), result);            
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