/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using MindMate.View.MapControls;
using System.Windows.Forms;
using MindMate.Serialization;

namespace MindMate.Controller
{
    /// <summary>
    /// Status Bar Controlller for Windows Forms
    /// </summary>
    public class WinFormsStatusBarCtrl : StatusBarCtrl
    {
        public WinFormsStatusBarCtrl(object statusBar, PersistenceManager persistenceManager) : base(statusBar, persistenceManager)
        {
        }
                
        override public void UpdateStatusBarForNode(SelectedNodes nodes)
        {
            var toolStrip = ((ToolStrip)statusBar);

            if (nodes.Count != 1)
            {
                toolStrip.Items[0].Text = nodes.Count + " nodes selected";
                toolStrip.Items[1].Text = "";
                toolStrip.Items[2].Text = "";
            }
            else
            {
                MapNode node = nodes.First;
                if (node.Link != null && node.GetLinkType() == NodeLinkType.MindMapNode)
                {
                    toolStrip.Items[0].Text =
                        node.Tree.RootNode.Find( //TODO: Rather than traversing through the tree, a more efficient approach should be used
                            n => node.Link.Substring(1) == n.Id
                        ).Text + " (Internal Link)";
                }
                else
                {
                    if (node.Link != null)
                        toolStrip.Items[0].Text = node.Link;
                    else
                        toolStrip.Items[0].Text = " "; //don't set it to null or "", somehow it triggers mouse move event on MapViewPanel
                }

                toolStrip.Items[1].Text = "Modified: " + node.Modified.ToString();
                toolStrip.Items[2].Text = "Created: " + node.Created.ToString();
            }

        }


        override public void SetStatusUpdate(string error)
        {
            var toolStrip = ((ToolStrip)statusBar);

            toolStrip.Items[0].Text = error;
        }

        
    }


}