using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.Serialization;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindMate.Tests.View
{
    [TestClass()]
    public class HtmlPreProcessorTests
    {
        [TestMethod()]
        public void ImageTagProcessor_BBC()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, tree.RootNode.FirstChild.NoteText);

            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(sut.ProcessedHtml.Contains("srcOrig"));
            Assert.AreEqual(79, sut.ImageSourceChanges.Count());

            int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount > sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_Wikipedia()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, tree.RootNode.FirstChild.Next.NoteText);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(sut.ProcessedHtml.Contains("srcOrig"));
            Assert.AreEqual(12, sut.ImageSourceChanges.Count());

            int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount == sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_Yahoo()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, tree.RootNode.LastChild.NoteText);
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(sut.ProcessedHtml.Contains("srcOrig"));
            Assert.AreEqual(35, sut.ImageSourceChanges.Count());

            int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount == sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SingleQuotes()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' src='http://test.com/image1.jpg'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_DoubleQuotes()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' src=\"http://test.com/image1.jpg\">");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_Newline()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' \n src='http://test.com/image1.jpg'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_NewlineInSrc()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' src='http://test.com/image1.jpg\n'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SpacesInAttribute()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' \n src = 'http://test.com/image1.jpg'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_NewlineInAttribute()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' \n src =\n 'http://test.com/image1.jpg'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SrcFirstAttribute()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img src='http://test.com/image1.jpg' alt='abc'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SrcInCaps()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<IMG SRC='http://test.com/image1.jpg' alt='abc'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_ClosingBracketOnNewline()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img src='http://test.com/image1.jpg' alt='abc' \n>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_TwoNewLines()
        {
            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, "<img alt='abc' \n src =\n 'http://test.com/image1.jpg'>");
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod]
        public void ScriptPreProcessor_BBC()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

            HtmlPreProcessor sut = null;
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                var editor = new NoteEditor();
                sut = new HtmlPreProcessor(editor, tree.RootNode.FirstChild.NoteText);

            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
            t.Join();

            Assert.IsTrue(sut.ProcessedHtml.Contains("script"));

            int scriptCount = Regex.Matches(sut.ProcessedHtml, "<script", RegexOptions.IgnoreCase).Count;
            Assert.AreEqual(0, scriptCount);
        }
    }
}