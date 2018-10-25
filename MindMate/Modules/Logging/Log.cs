using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MindMate.Modules.Logging
{
    public static class Log
    {
        public static void Write(Exception e)
        {
            Trace.WriteLine(DateTime.Now + ": " + e.ToString());
        }

        public static void Write(string message, Exception e)
        {
            Trace.WriteLine(PrintTime() +  message);
            Trace.WriteLine(e.ToString());
        }

        public static void Write(ThreadExceptionEventArgs e)
        {
            Trace.WriteLine(PrintTime() + e.ToString());
        }

        public static void Write(string message)
        {
            Trace.WriteLine(PrintTime() + message);
        }

        private static string PrintTime()
        {
            return "[" + DateTime.Now + "] ";
        }
    }
}
