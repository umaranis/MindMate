/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is license under MIT license (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.View.MapControls;

namespace MindMate.View.MapControls
{
    public class NodeMouseEventArgs : MouseEventArgs
    {
        public NodeMouseEventArgs(MouseEventArgs e)  : 
            base(e.Button, e.Clicks, e.Y, e.Y, e.Delta)
        {
            
        }

        public NodePortion NodePortion { get; set; }
    }
}
