/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2016 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Windows.Forms;
using MindMate.Controller;
using MindMate.Plugins;
using MindMate.View;
using MindMate.View.EditorTabs;
using MindMate.WinFormsUI;
using MindMate.WinFormsUI.Controller;
using MindMate.WinFormsUI.NoteEditing;

namespace MindMate.WinXP
{
    public partial class MainForm : MainFormBase
    {

        public MainForm()
        {
            Toolbar = new Toolbar();
            MainMenu = new MainMenu();
            Toolbar.MainMenu = MainMenu;
            this.Controls.Add(Toolbar);
            this.Controls.Add(MainMenu);
            
            SetupSideBar();
            EditorTabs = new EditorTabs();
            splitContainer1.Panel1.Controls.Add(EditorTabs);
        }          

        public MainMenu MainMenu { get; private set; }
        internal Toolbar Toolbar { get; private set; }
        public MainMenuCtrl MainMenuCtrl { get; set; }        

        public override void InsertMenuItems(MainMenuItem[] menuItems)
        {
            MainMenuCtrl.InsertMenuItems(menuItems);
        }

        public override void RefreshRecentFilesMenuItems()
        {
            MainMenuCtrl.RefreshRecentFilesMenuItems();
        }
    }
}
