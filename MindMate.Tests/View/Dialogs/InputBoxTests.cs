using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View.Dialogs
{
    [TestClass()]
    public class InputBoxTests
    {
        [TestMethod()]
        public void InputBox()
        {
            var sut = new InputBox("Question?", "Input");
            sut.Show();
            sut.Close();
            Assert.IsTrue(string.IsNullOrEmpty(sut.Answer));
        }

        [TestMethod()]
        public void InputBox_DefaultValue()
        {
            var sut = new InputBox("Question?", "Input");
            sut.Answer = "Test";
            sut.Show();
            sut.Hide();
            Assert.AreEqual("Test", sut.Answer);
        }
    }
}