using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Tests.View.Dialogs
{
    [TestClass()]
    public class AboutBoxTests
    {
        [TestMethod()]
        public void AboutBox()
        {
            var dialog = new AboutBox();
            dialog.Show();
            dialog.Close();
            Assert.IsNotNull(dialog);
        }
    }
}