/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Isolates MainCtrl from MapCtrl
    /// </summary>
    public interface IMainCtrl
    {
        System.Drawing.Color ShowColorPicker(System.Drawing.Color currentColor);

        System.Drawing.Font ShowFontDialog(System.Drawing.Font currentFont);

        bool SeekDeleteConfirmation(string msg);

        void ShowStatusNotification(string msg);

        NodeContextMenu NodeContextMenu { get; }
    }
}
