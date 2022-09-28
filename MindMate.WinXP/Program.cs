using MindMate.Controller;
using MindMate.Modules.Logging;
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
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ProgramMainHelper.EnableLogListeners();
            //MyWebMind.Debug.IconListCreator.GenerateIconXML();
            MainCtrl mainCtrl = new MainCtrl();
			MainForm form = new MainForm();
            ProgramMainHelper.SetFileToOpenFromAppArguments(args, form);
            mainCtrl.InitMindMate(form, new DialogManager());

            // specific to WinXP
            MainMenuCtrl mainMenuCtrl = new MainMenuCtrl(form.MainMenu, mainCtrl);
            form.MainMenuCtrl = mainMenuCtrl;

            // specific to WinXP

            Application.ThreadException += ProgramMainHelper.Application_ThreadException; 
            Application.Run(form);
            ProgramMainHelper.CloseLogListeners();
        }        
    }
}
