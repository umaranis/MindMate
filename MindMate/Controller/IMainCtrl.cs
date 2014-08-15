/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is license under MIT license (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Controller
{
    /// <summary>
    /// Isolates MainCtrl from MapCtrl
    /// </summary>
    public interface IMainCtrl
    {
        void AddMainPanel(View.MapControls.MapViewPanel mapViewPanel);

        System.Drawing.Color ShowColorPicker(System.Drawing.Color currentColor);

        System.Drawing.Font ShowFontDialog(System.Drawing.Font currentFont);

        bool SeekDeleteConfirmation(string msg);

        void ShowStatusNotification(string msg);
    }
}
