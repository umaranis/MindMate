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
            void Instance_Shown(object sender, EventArgs e)
            {
                SetForegroundWindow(IconSelectorExt.Instance.Handle);
                SendKeys.SendWait("{Esc}");
            }
            IconSelectorExt.Instance.Shown += Instance_Shown;                

            IconSelectorExt.Instance.ShowDialog();

            Assert.IsNull(IconSelectorExt.Instance.SelectedIcon, $"SelectedIcon is not null. It's value is {IconSelectorExt.Instance.SelectedIcon}");            
            IconSelectorExt.Instance.Shown -= Instance_Shown;
            
        }

        

        [TestMethod()]
        public void IconSelectorExt_SelectIconWithShortCut()
        {
            var icon = MetaModel.MetaModel.Instance.IconsList.Find(i => i.Shortcut != null);
            void Instance_Shown(object sender, EventArgs e)
            {
                SetForegroundWindow(IconSelectorExt.Instance.Handle);
                SendKeys.SendWait("{" + icon.Shortcut + "}");
            }
            IconSelectorExt.Instance.Shown += Instance_Shown;

            IconSelectorExt.Instance.ShowDialog();

            Assert.AreEqual(icon.Name, IconSelectorExt.Instance.SelectedIcon);

            IconSelectorExt.Instance.Shown -= Instance_Shown;
        }

        [TestMethod()]
        public void IconSelectorExt_CloseWithEnter()
        {
            void Instance_Shown(object sender, EventArgs e)
            {
                SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);
                SendKeys.SendWait("{Enter}");
            };
            IconSelectorExt.Instance.Shown += Instance_Shown;

            IconSelectorExt.Instance.ShowDialog();

            Assert.IsNotNull(MindMate.View.Dialogs.IconSelectorExt.Instance.SelectedIcon);
            IconSelectorExt.Instance.Shown -= Instance_Shown;

        }

        [TestMethod()]
        public void IconSelectorExt_RemoveAll()
        {
            void Instance_Shown(object sender, EventArgs e)
            {
                SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);
                SendKeys.SendWait("{Del}");
            }
            IconSelectorExt.Instance.Shown += Instance_Shown;
            IconSelectorExt.Instance.ShowDialog();            

            Assert.AreEqual(IconSelectorExt.REMOVE_ALL_ICON_NAME, IconSelectorExt.Instance.SelectedIcon);
            IconSelectorExt.Instance.Shown -= Instance_Shown;
        }

        

        [TestMethod()]
        public void IconSelectorExt_RemoveLast()
        {
            void Instance_Shown(object sender, EventArgs e)
            {
                SetForegroundWindow(MindMate.View.Dialogs.IconSelectorExt.Instance.Handle);
                SendKeys.SendWait("{Backspace}");
            }
            IconSelectorExt.Instance.Shown += Instance_Shown;
            IconSelectorExt.Instance.ShowDialog();

            Assert.AreEqual(IconSelectorExt.REMOVE_ICON_NAME, IconSelectorExt.Instance.SelectedIcon);
            IconSelectorExt.Instance.Shown -= Instance_Shown;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}