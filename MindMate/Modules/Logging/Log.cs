using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Logging
{
    public static class Log
    {
        public static void Write(Exception e)
        {
            Trace.WriteLine(DateTime.Now + ": " + e.Message);
            Trace.WriteLine(e.StackTrace);
        }

        public static void Write(string message)
        {
            Trace.WriteLine(DateTime.Now + ": " + message);
        }
    }
}
