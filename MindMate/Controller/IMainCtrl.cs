/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.View.MapControls;
using System.Windows.Forms;
using MindMate.View.Dialogs;

namespace MindMate.Controller
{
    //TODO: DialogManager should replace this class
    /// <summary>
    /// Isolates MainCtrl from MapCtrl
    /// </summary>
    public interface IMainCtrl
    {
        System.Drawing.Color ShowColorPicker(System.Drawing.Color currentColor);

        System.Drawing.Font ShowFontDialog(System.Drawing.Font currentFont);        

        void ShowStatusNotification(string msg);

        void ShowMessageBox(string title, string msg, MessageBoxIcon icon);

        /// <summary>
        /// Uses InputBox dialog to ask question from the user
        /// </summary>
        /// <param name="question"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        string ShowInputBox(string question, string caption = null);

        NodeContextMenu NodeContextMenu { get; }
    }
}
