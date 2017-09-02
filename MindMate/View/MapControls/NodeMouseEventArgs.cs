/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
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
        public NodeMouseEventArgs(NodeView nView, MouseEventArgs e)  : 
            base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            NodePortion = nView.GetNodeClickPortion(e.Location);
            var s = nView.GetSubControl(e.Location);
            SubControl = s.Value;
            SubControlType = s.Key;
        }

        public NodePortion NodePortion { get; }
        public SubControlType SubControlType { get; }
        public object SubControl { get; }
    }
}
