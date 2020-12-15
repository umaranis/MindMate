using MindMate.Controller;
using MindMate.Modules.Logging;
using MindMate.View.Dialogs;
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
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProgramMainHelper.EnableLogListeners();            
            //MyWebMind.Debug.IconListCreator.GenerateIconXML();
            MainCtrl mainCtrl = new MainCtrl();
            MainForm form = new MainForm(mainCtrl);
            ProgramMainHelper.SetFileToOpenFromAppArguments(args, form);
            mainCtrl.InitMindMate(form, new DialogManager());

            // specific to win7
            var ribbonHandler = new View.Ribbon.Ribbon(form.Ribbon, mainCtrl, form);
            form.RibbonCtrl = ribbonHandler;
            form.Load += (sender, arguments) => ribbonHandler.OnRibbonLoaded();
            // specific to win7

            Application.ThreadException += ProgramMainHelper.Application_ThreadException;
            Application.Run(form);
            ProgramMainHelper.CloseLogListeners();
        }
    }
}
