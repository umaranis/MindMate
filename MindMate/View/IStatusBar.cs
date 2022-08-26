using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMate.View
{
    public interface IStatusBar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">starts from 0</param>
        /// <param name="text"></param>
        void UpdateText(int position, string text);
    }
}
