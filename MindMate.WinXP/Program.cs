using MindMate.Controller;
using MindMate.View.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.WinXP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EnableLogListeners();
            //MyWebMind.Debug.IconListCreator.GenerateIconXML();
            MainCtrl mainCtrl = new MainCtrl();
			MainForm form = new MainForm();
            mainCtrl.InitMindMate(form, new DialogManager());
            MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, mainCtrl);
            form.MainMenuCtrl = mainMenuCtrl;
            Application.ThreadException += Application_ThreadException; 
            Application.Run(form);
            CloseLogListeners();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Trace.WriteLine(DateTime.Now.ToString() + ":" + e.Exception.Message);
            Trace.WriteLine(e.Exception.StackTrace);
        }

        private static void EnableLogListeners()
        {
            Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener("MindMate_Trace.log"));
            Trace.AutoFlush = true;
            
            //Debug.Listeners.Add(new TextWriterTraceListener("SystemLog.txt"));
            //Debug.AutoFlush = true;
        }

        private static void CloseLogListeners()
        {
            System.Diagnostics.Trace.Close();
            //System.Diagnostics.Debug.Close();
        }
    }
}
