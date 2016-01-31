using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindMate.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Tests.View
{
    [TestClass()]
    public class IconSelectorExtTests
    {
        [TestMethod()]
        public void IconSelectorExt_CloseWithEscape()
        {
            MindMate.View.Dialogs.IconSelectorExt.Instance.Show();

            SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);

            SendKeys.SendWait("{Esc}");

            Assert.IsNull(MindMate.View.Dialogs.IconSelectorExt.Instance.SelectedIcon);
        }

        [TestMethod()]
        public void IconSelectorExt_SelectIconWithShortCut()
        {
            MindMate.View.Dialogs.IconSelectorExt.Instance.Show();
            var icon = MetaModel.MetaModel.Instance.IconsList.Find(i => i.Shortcut != null);

            SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);

            SendKeys.SendWait("{" + icon.Shortcut + "}");

            Assert.AreEqual(icon.Name, MindMate.View.Dialogs.IconSelectorExt.Instance.SelectedIcon);
        }

        [TestMethod()]
        public void IconSelectorExt_CloseWithEnter()
        {
            MindMate.View.Dialogs.IconSelectorExt.Instance.Show();

            SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);

            SendKeys.SendWait("{Enter}");

            Assert.IsNotNull(MindMate.View.Dialogs.IconSelectorExt.Instance.SelectedIcon);
        }

        [TestMethod()]
        public void IconSelectorExt_RemoveAll()
        {
            MindMate.View.Dialogs.IconSelectorExt.Instance.Show();

            SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);

            SendKeys.SendWait("{Del}");

            Assert.AreEqual(IconSelectorExt.REMOVE_ALL_ICON_NAME, IconSelectorExt.Instance.SelectedIcon);
        }

        [TestMethod()]
        public void IconSelectorExt_RemoveLast()
        {
            MindMate.View.Dialogs.IconSelectorExt.Instance.Show();

            SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);

            SendKeys.SendWait("{Backspace}");

            Assert.AreEqual(IconSelectorExt.REMOVE_ICON_NAME, IconSelectorExt.Instance.SelectedIcon);
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}