/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is license under MIT license (see LICENSE.txt)    
 */

using System;
using System.Linq;
using System.Windows.Forms;
using MindMate.MetaModel;
using MindMate.View;
using MindMate.Controller;

namespace MindMate
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
            Application.Run(new MainCtrl().LaunchMindMate());
            traceLog.Close();
        }

    }
}
