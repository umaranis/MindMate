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

            var sut = new HtmlPreProcessor(tree.RootNode.FirstChild.NoteText);
            Assert.IsTrue(sut.ProcessedHtml.Contains("srcorig"));
            Assert.AreEqual(79, sut.ImageSourceChanges.Count());

            int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount > sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_Wikipedia()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

            var sut = new HtmlPreProcessor(tree.RootNode.FirstChild.Next.NoteText);
            Assert.IsTrue(sut.ProcessedHtml.Contains("srcorig"));
            Assert.AreEqual(12, sut.ImageSourceChanges.Count());

            int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount == sut.ImageSourceChanges.Count());

            var a = new HtmlAgilityPack.HtmlWeb();
            
        }

        [TestMethod()]
        public void ImageTagProcessor_Yahoo()
        {
            var p = new PersistenceManager();
            var tree = p.OpenTree(@"Resources\Websites.mm").Tree;

            var sut = new HtmlPreProcessor(tree.RootNode.LastChild.NoteText);
            Assert.IsTrue(sut.ProcessedHtml.Contains("srcorig"));
            Assert.AreEqual(35, sut.ImageSourceChanges.Count());

            int imgCount = Regex.Matches(sut.ProcessedHtml, "<img", RegexOptions.IgnoreCase).Count;
            Assert.IsTrue(imgCount == sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SingleQuotes()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' src='http://test.com/image1.jpg'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_DoubleQuotes()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' src=\"http://test.com/image1.jpg\">");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_Newline()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' \n src='http://test.com/image1.jpg'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_NewlineInSrc()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' src='http://test.com/image1.jpg\n'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SpacesInAttribute()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' \n src = 'http://test.com/image1.jpg'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_NewlineInAttribute()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' \n src =\n 'http://test.com/image1.jpg'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SrcFirstAttribute()
        {
            var sut = new HtmlPreProcessor("<img src='http://test.com/image1.jpg' alt='abc'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_SrcInCaps()
        {
            var sut = new HtmlPreProcessor("<IMG SRC='http://test.com/image1.jpg' alt='abc'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_ClosingBracketOnNewline()
        {
            var sut = new HtmlPreProcessor("<img src='http://test.com/image1.jpg' alt='abc' \n>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }

        [TestMethod()]
        public void ImageTagProcessor_TwoNewLines()
        {
            var sut = new HtmlPreProcessor("<img alt='abc' \n src =\n 'http://test.com/image1.jpg'>");
            Assert.AreEqual(1, sut.ImageSourceChanges.Count());
        }
    }
}