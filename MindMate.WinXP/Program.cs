using MindMate.Controller;
using System;
using System.Collections.Generic;
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
            var traceLog = new System.Diagnostics.TextWriterTraceListener("MindMate_Trace.log");
            System.Diagnostics.Trace.Listeners.Add(traceLog);
            System.Diagnostics.Trace.AutoFlush = true;
            //MyWebMind.Debug.IconListCreator.GenerateIconXML();
            MainCtrl mainCtrl = new MainCtrl();
			MainForm form = new MainForm();
            mainCtrl.InitMindMate(form);
            MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, mainCtrl);
            form.MainMenuCtrl = mainMenuCtrl;
            Application.Run(form);
            traceLog.Close();
        }
    }
}
