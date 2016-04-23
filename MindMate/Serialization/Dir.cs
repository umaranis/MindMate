using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MindMate.Serialization
{
    public class Dir
    {
        public static string UserSettingsDirectory
        {
            get
            {
#if !DEBUG
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                    "\\" + MindMate.Controller.MainCtrl.APPLICATION_NAME + "\\";
#else
                return Path.GetTempPath() + MindMate.Controller.MainCtrl.APPLICATION_NAME + "\\";
#endif
            }
        }
    }
}
