using MindMate.Modules.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate
{
    public static class ProgramMainHelper
    {
        /// <summary>
        /// Returns the first string from the array if it exists and is a valid file name
        /// Otherwise return null
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetFileFromAppArguments(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string fileName = args[0];
                //Check file exists
                if (File.Exists(fileName))
                {
                    return fileName;
                }

            }
            return null;
        }

        public static void SetFileToOpenFromAppArguments(string[] args, Form form)
        {
            form.Tag = GetFileFromAppArguments(args);
        }

        public static string GetFileToOpenFromAppArguments(object form)
        {
            return ((form as Form)?.Tag) as string;
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Write(e);
        }

        public static void EnableLogListeners()
        {
            Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(MetaModel.MetaModel.GetFileDirectory() + "MindMate_Trace.log"));
            Trace.AutoFlush = true;

            //Debug.Listeners.Add(new TextWriterTraceListener("SystemLog.txt"));
            //Debug.AutoFlush = true;
        }

        public static void CloseLogListeners()
        {
            System.Diagnostics.Trace.Close();
            //System.Diagnostics.Debug.Close();
        }
    }
}
