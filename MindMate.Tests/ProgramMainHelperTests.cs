using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.Model
{
    [TestClass()]
    public class ProgramMainHelperTests
    {
        [TestMethod()]
        public void GetFileFromAppArguments_Folders_Null()
        {
            var r = MindMate.ProgramMainHelper.GetFileFromAppArguments(new string[] { "C:\\windows" });
            Assert.AreEqual(null, r);
        }

        [TestMethod()]
        public void GetFileFromAppArguments_File()
        {
            string file = "C:\\windows\\notepad.exe";
            var r = MindMate.ProgramMainHelper.GetFileFromAppArguments(new string[] { file });
            Assert.AreEqual(file, r);
        }

        [TestMethod()]
        public void GetFileFromAppArguments_FirstFile()
        {
            string file1 = "C:\\windows\\notepad.exe";
            string file2 = "C:\\windows\\write.exe";
            var r = ProgramMainHelper.GetFileFromAppArguments(new string[] { file1, file2 });
            Assert.AreEqual(file1, r);
        }

        [TestMethod()]
        public void GetFileFromAppArguments_NullTest()
        {
            var r = ProgramMainHelper.GetFileFromAppArguments(null);
            Assert.AreEqual(null, r);
        }

        [TestMethod()]
        public void GetFileFromAppArguments_ZeroLength()
        {
            var r = ProgramMainHelper.GetFileFromAppArguments(new string[] { });
            Assert.AreEqual(null, r);
        }

        [TestMethod()]
        public void SetGetFileToOpenFromAppArguments()
        {
            string file = "C:\\windows\\notepad.exe";
            Form f = new Form();
            ProgramMainHelper.SetFileToOpenFromAppArguments(new string[] { file }, f);
            Assert.AreEqual(file, ProgramMainHelper.GetFileToOpenFromAppArguments(f));
            f.Dispose();
        }

        [TestMethod()]
        public void Logging_NoException()
        {
            ProgramMainHelper.EnableLogListeners();
            ProgramMainHelper.Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(new Exception("Test exception")));
            ProgramMainHelper.CloseLogListeners();
        }
    }
}