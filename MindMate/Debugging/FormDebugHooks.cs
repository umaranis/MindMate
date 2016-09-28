using MindMate.Modules.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Debugging
{
    public class FormDebugHooks
    {
        private FormDebugHooks() {  }

        static FormDebugHooks()
        {
            Instance = new FormDebugHooks();
        }

        public static FormDebugHooks Instance { get; }

        private Action<object, EventArgs> shownEventHook;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onShown"></param>
        [Conditional("DEBUG")]
        public void ProvideShownEventHook(Action<object, EventArgs> onShown)
        {
            shownEventHook = onShown;
        }

        [Conditional("DEBUG")]
        public void ClearHook()
        {
            shownEventHook = null;
        }

        [Conditional("DEBUG")]
        public void ApplyHook(Form f)
        {
            if(shownEventHook != null)
                f.Shown += new EventHandler(shownEventHook);
        }        
    }
}
