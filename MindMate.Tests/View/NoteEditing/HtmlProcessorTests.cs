using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View
{
    [TestClass()]
    public class HtmlProcessorTests
    {
        [TestMethod()]
        public void ImageTagProcessor_BBC()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;
            string html = null;
            int imgUpdated = 0;

            HtmlProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();                               
                var form = CreateForm();
                form.Controls.Add(editor);
                form.Shown += (sender, args) =>
                {
                    editor.HTML = tree.RootNode.FirstChild.NoteText;
                    sut = new HtmlProcessor(editor, e => { imgUpdated++; });
                    html = editor.HTML;
                    form.Close();
                };
                form.ShowDialog();

            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(html.Contains("srcOrig"));
            Assert.AreEqual(79, imgUpdated);

            int imgCount = Regex.Matches(html, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount > imgUpdated);
        }

        //TODO: Fix the following tests
        //[TestMethod()]
        //public void ImageTagProcessor_Wikipedia()
        //{
        //    var p = new PersistenceManager();
        //    var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, tree.RootNode.FirstChild.Next.NoteText);
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();

        //    Assert.IsTrue(sut.ProcessedHtml.Contains("srcOrig"));
        //    Assert.AreEqual(12, sut.ImageSourceChanges.Count());

        //    int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
        //    Assert.IsTrue(imgCount == sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_Yahoo()
        //{
        //    var p = new PersistenceManager();
        //    var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, tree.RootNode.LastChild.NoteText);
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();

        //    Assert.IsTrue(sut.ProcessedHtml.Contains("srcOrig"));
        //    Assert.AreEqual(35, sut.ImageSourceChanges.Count());

        //    int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
        //    Assert.IsTrue(imgCount == sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_SingleQuotes()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' src='http://test.com/image1.jpg'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();

        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_DoubleQuotes()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' src=\"http://test.com/image1.jpg\">");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_Newline()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' \n src='http://test.com/image1.jpg'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_NewlineInSrc()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' src='http://test.com/image1.jpg\n'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_SpacesInAttribute()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' \n src = 'http://test.com/image1.jpg'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_NewlineInAttribute()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' \n src =\n 'http://test.com/image1.jpg'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_SrcFirstAttribute()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img src='http://test.com/image1.jpg' alt='abc'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_SrcInCaps()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<IMG SRC='http://test.com/image1.jpg' alt='abc'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_ClosingBracketOnNewline()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img src='http://test.com/image1.jpg' alt='abc' \n>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod()]
        //public void ImageTagProcessor_TwoNewLines()
        //{
        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, "<img alt='abc' \n src =\n 'http://test.com/image1.jpg'>");
        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        //}

        //[TestMethod]
        //public void ScriptPreProcessor_BBC()
        //{
        //    var p = new PersistenceManager();
        //    var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

        //    HtmlProcessor sut = null;
        //    System.Threading.Thread t = new System.Threading.Thread(() =>
        //    {
        //        var editor = new NoteEditor();
        //        sut = new HtmlProcessor(editor, tree.RootNode.FirstChild.NoteText);

        //    });
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();

        //    Assert.IsTrue(sut.ProcessedHtml.Contains("script"));

        //    int scriptCount = Regex.Matches(sut.ProcessedHtml, "<script", RegexOptions.IgnoreCase).Count;
        //    Assert.AreEqual(0, scriptCount);
        //}

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