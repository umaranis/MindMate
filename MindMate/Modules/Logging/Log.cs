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
            Debug.WriteLine(DateTime.Now + ":" + e.Message);
            Debug.WriteLine(e.StackTrace);
        }
    }
}
