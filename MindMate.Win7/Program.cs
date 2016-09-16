using MindMate.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.Win7
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
            MainForm form = new MainForm(mainCtrl);            
            mainCtrl.InitMindMate(form);
            var ribbonHandler = new View.Ribbon.Ribbon(form.Ribbon, mainCtrl, form);
            form.RibbonCtrl = ribbonHandler;
            form.Shown += (sender, args) => ribbonHandler.OnRibbonLoaded();
            //Application.ThreadException += Application_ThreadException; //TODO: implement this it replace the standard exception dialog
            Application.Run(form);
            CloseLogListeners();
        }

        //private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        //{
        //}

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
