/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2016 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;
using MindMate.Plugins;
using MindMate.Controller;
using MindMate.View;
using MindMate.View.EditorTabs;
using MindMate.WinFormsUI;

namespace MindMate.Win7
{
    public partial class MainForm : MainFormBase
    {
        private readonly MainCtrl mainCtrl;
        
        public MainForm(MainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;
            Ribbon = new RibbonLib.Ribbon {ResourceName = "MindMate.Win7.View.Ribbon.RibbonMarkup.ribbon"};
            this.Controls.Add(Ribbon);
            
            SetupSideBar();
            EditorTabs = new EditorTabs();
            splitContainer1.Panel1.Controls.Add(EditorTabs);


#if (Win7)
            //this is required for Windows 7 & 8, otherwise sidebar is not laid out properly
            Load += (sender, args) => Ribbon.Minimized = true;
            Shown += (sender, args) => Ribbon.Minimized = false;
#endif            
        }
        
        

        public RibbonLib.Ribbon Ribbon { get; private set; }
        public View.Ribbon.Ribbon RibbonCtrl { get; set; }
        

        

        public override void InsertMenuItems(MainMenuItem[] menuItems)
        {
            RibbonCtrl.SetupPluginCommands(menuItems);
        }

        public override void RefreshRecentFilesMenuItems()
        {
            RibbonCtrl.RefreshRecentItemsList();
        }

        //this method is specific to Win7 implementation (not required in WinXP)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Control) == Keys.Control)
            {
                switch (keyData)
                {
                    case (Keys.Control | Keys.N):
                        mainCtrl.NewMap();
                        return true;
                    case (Keys.Control | Keys.O):
                        mainCtrl.OpenMap();
                        return true;
                    case (Keys.Control | Keys.S):
                        mainCtrl.SaveCurrentMap();
                        return true;
                    case (Keys.Control | Keys.Shift | Keys.S):
                        mainCtrl.SaveAll();
                        return true;
                    case (Keys.Control | Keys.Z):
                        mainCtrl.Undo();
                        return true;
                    case (Keys.Control | Keys.Y):
                        mainCtrl.Redo();
                        return true;
                    case (Keys.Control | Keys.C):
                        mainCtrl.Copy();
                        return true;
                    case (Keys.Control | Keys.V):
                        mainCtrl.Paste();
                        return true;
                    case (Keys.Control | Keys.X):
                        mainCtrl.Cut();
                        return true;
                    case (Keys.Control | Keys.B):
                        mainCtrl.CurrentMapCtrl.ToggleBold();
                        return true;
                    case (Keys.Control | Keys.I):
                        mainCtrl.CurrentMapCtrl.ToggleItalic();
                        return true;
                    case (Keys.Control | Keys.D):
                        mainCtrl.CurrentMapCtrl.ChangeFont();
                        return true;
                }
            }
            else if ((keyData & Keys.Alt) == Keys.Alt)
            {
                switch (keyData)
                {
                    case (Keys.Alt | Keys.I):
                        mainCtrl.CurrentMapCtrl.AppendIconFromIconSelectorExt();
                        return true;
                }
            }            

            return base.ProcessCmdKey(ref msg, keyData);
        }        
    }
}
