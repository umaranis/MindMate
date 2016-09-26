using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.View
{
    [TestClass()]
    public class TablePropertyFormTests
    {
        [TestMethod()]
        public void TablePropertyForm()
        {
            var sut = new TablePropertyForm();
            sut.TableProperties = new HtmlTableProperty(true);            
            sut.Show();
            sut.Close();
            Assert.IsTrue(string.IsNullOrEmpty(sut.TableProperties.CaptionText));
        }       
    }
}