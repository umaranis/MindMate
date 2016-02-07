/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Keyboard event handler for MapView.
    /// It receives the events from MapView and translate them to the right controller action.     
    /// </summary>
    public class MapViewKeyEventHandler
    {
        private MapCtrl mapCtrl;

        public MapViewKeyEventHandler(MapCtrl mapCtrl)
        {
            this.mapCtrl = mapCtrl;
        }

        public void canvasKeyDown(object sender, KeyEventArgs args)
        {
            args.Handled = true;
            args.SuppressKeyPress = !args.Alt; // don't suppres if Alt key is pressed to enable browsing main menu 
            
            if (args.Control && args.Shift)
                HandleCtrlShiftPlusKey(sender, args);
            else if(args.Control)
                HandleCtrlPlusKey(sender, args);
            else if (args.Shift)
                HandleShiftPlusKey(sender, args);
            else if (args.Alt)
                HandleAltPlusKey(sender, args);
            else
                HandleKey(sender, args);

        }

        private void HandleKey(object sender, KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.Left:
                    mapCtrl.SelectNodeLeftOrUnfold();
                    break;
                case Keys.Right:
                    mapCtrl.SelectNodeRightOrUnfold();
                    break;
                case Keys.Up:
                    mapCtrl.SelectNodeAbove();
                    break;                
                case Keys.Down:
                    mapCtrl.SelectNodeBelow();
                    break;
                case Keys.Space:
                    mapCtrl.ToggleFolded();
                    break;
                case Keys.Enter:
                    mapCtrl.AppendNodeAndEdit();
                    break;                
                case Keys.PageUp:
                    mapCtrl.SelectTopSibling();
                    break;
                case Keys.PageDown:
                    mapCtrl.SelectBottomSibling();
                    break;
                case Keys.End:
                    mapCtrl.BeginCurrentNodeEdit(TextCursorPosition.End);
                    break;
                case Keys.Home:
                    mapCtrl.BeginCurrentNodeEdit(TextCursorPosition.Start);
                    break;                
                case Keys.Insert:
                case Keys.Tab:
                    mapCtrl.AppendChildNodeAndEdit();
                    break;
                case Keys.Delete:
                    mapCtrl.DeleteSelectedNodes();
                    break;
                case Keys.F2:
                    mapCtrl.BeginCurrentNodeEdit(TextCursorPosition.Undefined);
                    break;
            }

        }

        private void HandleCtrlPlusKey(object sender, KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.Up:
                    mapCtrl.MoveNodeUp();
                    break;
                case Keys.Down:
                    mapCtrl.MoveNodeDown();
                    break;
                case Keys.K:
                    mapCtrl.AddHyperlinkUsingTextbox();
                    break;                
            }


        }

        private void HandleShiftPlusKey(object sender, KeyEventArgs args)
        {
            
            switch (args.KeyCode)
            {
                case Keys.Up:
                    mapCtrl.SelectNodeAbove(true);
                    break;
                case Keys.Down:
                    mapCtrl.SelectNodeBelow(true);
                    break;
                case Keys.PageUp:
                    mapCtrl.SelectAllSiblingsAbove();
                    break;
                case Keys.PageDown:
                    mapCtrl.SelectAllSiblingsBelow();
                    break;
                case Keys.Enter:
                    mapCtrl.AppendSiblingAboveAndEdit();
                    break;
            }
        }

        private void HandleAltPlusKey(object sender, KeyEventArgs args)
        {

            switch (args.KeyCode)
            {
                case Keys.Enter:
                    mapCtrl.MultiLineNodeEdit();
                    break;
            }
        }

        private void HandleCtrlShiftPlusKey(object sender, KeyEventArgs args)
        {
            switch(args.KeyCode)
            {
                case Keys.K:
                    mapCtrl.AddHyperlinkUsingFileDialog();
                    break;
            }
        }
    }
}
