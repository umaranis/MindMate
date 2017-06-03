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
        void ShowStatusNotification(string msg);        
        
        NodeContextMenu NodeContextMenu { get; }
    }
}
