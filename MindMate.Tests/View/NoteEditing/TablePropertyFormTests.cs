using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.NoteEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        [TestMethod()]
        public void UpdateTable_DefaultValue()
        {
            var sut = new TablePropertyForm();
            sut.TableProperties = new HtmlTableProperty(true);
            Assert.IsFalse(sut.UpdateTable);
        }

        [TestMethod()]
        public void UpdateTable()
        {
            var sut = new TablePropertyForm();
            sut.TableProperties = new HtmlTableProperty(true);
            sut.UpdateTable = true;
            Assert.IsTrue(sut.UpdateTable);
        }
    }
}