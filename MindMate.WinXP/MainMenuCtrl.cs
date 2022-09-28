/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.View;
using System.Windows.Forms;
using MindMate.Controller;
using MindMate.Plugins;

namespace MindMate.WinXP
{
    /// <summary>
    /// Controller for Main Menu.
    /// Manages state of Menu Items and passes the events to other controllers (event handlers don't contain any business logic).
    /// </summary>
    public class MainMenuCtrl
    {
        public MainMenu mainMenu;
        public MapCtrl mapCtrl { get { return mainCtrl.CurrentMapCtrl; } }
        private MainCtrl mainCtrl; 

        public MainMenuCtrl(MainMenu mainMenu, MainCtrl mainCtrl)
        {
            this.mainMenu = mainMenu;
            this.mainCtrl = mainCtrl;

            mainMenu.mSelectIcon.Click += mSelectIcon_Click;

            mainMenu.newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            mainMenu.openToolStripMenuItem.Click += this.openToolStripMenuItem_Click;
            mainMenu.saveToolStripMenuItem.Click += this.saveToolStripMenuItem_Click;
            mainMenu.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            mainMenu.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            mainMenu.asBMPToolStripMenuItem.Click += asBMPToolStripMenuItem_Click;
            mainMenu.exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;

            mainMenu.mUndo.Click += mUndo_Click;
            mainMenu.mRedo.Click += mRedo_Click;
            mainMenu.mCut.Click += mCut_Click;
            mainMenu.mCopy.Click += mCopy_Click;
            mainMenu.mPaste.Click += mPaste_Click;
            mainMenu.mDelete.Click += mDelete_Click;
            mainMenu.mEditMenu.DropDownOpening += mEditMenu_DropDownOpening;
            mainMenu.mEditMenu.DropDownClosed += mEditMenu_DropDownClosed;
            
            mainMenu.mBold.Click += mBold_Click;
            mainMenu.mItalic.Click += mItalic_Click;

            mainMenu.mFont.Click += mFont_Click;
            mainMenu.mTextColor.Click += mTextColor_Click;
            mainMenu.mBackColor.Click += mBackColor_Click;

            mainMenu.mShapeFork.Click += mStyleFork_Click;
            mainMenu.mShapeBubble.Click += mStyleBubble_Click;
            mainMenu.mShapeBox.Click += mShapeBox_Click;
            mainMenu.mShapeBullet.Click += mShapeBullet_Click;
            mainMenu.mShape.DropDownOpening += mShape_DropDownOpening;

            mainMenu.mLineThick1.Click += mLineThick1_Click;
            mainMenu.mLineThick2.Click += mLineThick2_Click;
            mainMenu.mLineThick4.Click += mLineThick4_Click;
            mainMenu.mLineStyle.DropDownOpening += mLineStyle_DropDownOpening;
            mainMenu.mLineSolid.Click += mLineSolid_Click;
            mainMenu.mLineDashed.Click += mLineDashed_Click;
            mainMenu.mLineDotted.Click += mLineDotted_Click;
            mainMenu.mLineMixed.Click += mLineMixed_Click;

            mainMenu.mLineColor.Click += mLineColor_Click;

            mainMenu.mAbout.Click += mAbout_Click;

            CreateRecentFilesMenuItems();
        }
        void mAbout_Click(object sender, EventArgs e)
        {
            mainCtrl.ShowAboutBox();
        }

        void mBackColor_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeBackColorByPicker();
        }

        void mTextColor_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeTextColorByPicker();
        }

        void asBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.ExportAsBmp();
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void mUndo_Click(object sender, EventArgs e)
        {
            mainCtrl.Undo();
        }

        private void mRedo_Click(object sender, EventArgs e)
        {
            mainCtrl.Redo();
        }

        void mCut_Click(object sender, EventArgs e)
        {
            mainCtrl.Cut();   
        }

        void mCopy_Click(object sender, EventArgs e)
        {
            mainCtrl.Copy();
        }

        void mPaste_Click(object sender, EventArgs e)
        {
            mainCtrl.Paste();
        }

        void mDelete_Click(object sender, EventArgs e)
        {
            mapCtrl.DeleteSelectedNodes();
        }

        private void mEditMenu_DropDownOpening(object sender, EventArgs e)
        {
            mainMenu.mUndo.Enabled = mainCtrl.ChangeManager.CanUndo;
            mainMenu.mRedo.Enabled = mainCtrl.ChangeManager.CanRedo;
        }


        private void mEditMenu_DropDownClosed(object sender, EventArgs e)
        {
            mainMenu.mUndo.Enabled = true;
            mainMenu.mRedo.Enabled = true;
        }

        void mFont_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeFont();
        }

        void mLineColor_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLineColorUsingPicker();                      
        }

        void mLineMixed_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLinePattern(System.Drawing.Drawing2D.DashStyle.DashDotDot);
        }

        void mLineDotted_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLinePattern(System.Drawing.Drawing2D.DashStyle.Dot);
        }

        void mLineDashed_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLinePattern(System.Drawing.Drawing2D.DashStyle.Dash);
        }

        void mLineSolid_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLinePattern(System.Drawing.Drawing2D.DashStyle.Solid);
        }

        void mLineStyle_DropDownOpening(object sender, EventArgs e)
        {
            if (mapCtrl.MapView.SelectedNodes.Count == 1)
            {
                mainMenu.mLineThick1.Enabled = true;
                mainMenu.mLineThick2.Enabled = true;
                mainMenu.mLineThick4.Enabled = true;

                mainMenu.mLineSolid.Enabled = true;
                mainMenu.mLineDashed.Enabled = true;
                mainMenu.mLineDotted.Enabled = true;
                mainMenu.mLineMixed.Enabled = true;

                switch(mapCtrl.MapView.SelectedNodes.First.LineWidth)
                {
                    case 1:
                        mainMenu.mLineThick1.Checked = true;
                        mainMenu.mLineThick2.Checked = false;
                        mainMenu.mLineThick4.Checked = false;
                        break;
                    case 2:
                        mainMenu.mLineThick1.Checked = false;
                        mainMenu.mLineThick2.Checked = true;
                        mainMenu.mLineThick4.Checked = false;
                        break;
                    case 4:
                        mainMenu.mLineThick1.Checked = false;
                        mainMenu.mLineThick2.Checked = false;
                        mainMenu.mLineThick4.Checked = true;
                        break;
                    default:
                        mainMenu.mLineThick1.Checked = false;
                        mainMenu.mLineThick2.Checked = false;
                        mainMenu.mLineThick4.Checked = false;
                        break;
                }

                switch (mapCtrl.MapView.SelectedNodes.First.LinePattern)
                {
                    case System.Drawing.Drawing2D.DashStyle.Solid:
                        mainMenu.mLineSolid.Checked = true;
                        mainMenu.mLineDashed.Checked = false;
                        mainMenu.mLineDotted.Checked = false;
                        mainMenu.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.Dash:
                        mainMenu.mLineSolid.Checked = false;
                        mainMenu.mLineDashed.Checked = true;
                        mainMenu.mLineDotted.Checked = false;
                        mainMenu.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.Dot:
                        mainMenu.mLineSolid.Checked = false;
                        mainMenu.mLineDashed.Checked = false;
                        mainMenu.mLineDotted.Checked = true;
                        mainMenu.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.DashDotDot:
                        mainMenu.mLineSolid.Checked = false;
                        mainMenu.mLineDashed.Checked = false;
                        mainMenu.mLineDotted.Checked = false;
                        mainMenu.mLineMixed.Checked = true;
                        break;
                    default:
                        mainMenu.mLineSolid.Checked = false;
                        mainMenu.mLineDashed.Checked = false;
                        mainMenu.mLineDotted.Checked = false;
                        mainMenu.mLineMixed.Checked = false;
                        break;
                }
            }
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.Tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
            {
                mainMenu.mLineThick1.Enabled = false;
                mainMenu.mLineThick2.Enabled = false;
                mainMenu.mLineThick4.Enabled = false;
                mainMenu.mLineThick1.Checked = false;
                mainMenu.mLineThick2.Checked = false;
                mainMenu.mLineThick4.Checked = false;

                mainMenu.mLineSolid.Enabled = false;
                mainMenu.mLineDashed.Enabled = false;
                mainMenu.mLineDotted.Enabled = false;
                mainMenu.mLineMixed.Enabled = false;
                mainMenu.mLineSolid.Checked = false;
                mainMenu.mLineDashed.Checked = false;
                mainMenu.mLineDotted.Checked = false;
                mainMenu.mLineMixed.Checked = false;
            }
            else //multiple nodes selected
            {
                mainMenu.mLineThick1.Enabled = true;
                mainMenu.mLineThick2.Enabled = true;
                mainMenu.mLineThick4.Enabled = true;
                mainMenu.mLineThick1.Checked = false;
                mainMenu.mLineThick2.Checked = false;
                mainMenu.mLineThick4.Checked = false;

                mainMenu.mLineSolid.Enabled = true;
                mainMenu.mLineDashed.Enabled = true;
                mainMenu.mLineDotted.Enabled = true;
                mainMenu.mLineMixed.Enabled = true;
                mainMenu.mLineSolid.Checked = false;
                mainMenu.mLineDashed.Checked = false;
                mainMenu.mLineDotted.Checked = false;
                mainMenu.mLineMixed.Checked = false;
            }
        }

        void mLineThick4_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLineWidth(4);
        }

        void mLineThick2_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLineWidth(2);
        }

        void mLineThick1_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLineWidth(1);
        }

        void mShape_DropDownOpening(object sender, EventArgs e)
        {
            if (mapCtrl.MapView.SelectedNodes.Count == 1)
            {
                mainMenu.mShapeBubble.Enabled = true;
                mainMenu.mShapeBox.Enabled = true;
                mainMenu.mShapeFork.Enabled = true;

                switch (mapCtrl.MapView.SelectedNodes.First.Shape)
                {
                    case Model.NodeShape.Box:
                        mainMenu.mShapeBox.Checked = true;
                        mainMenu.mShapeBubble.Checked = false;
                        mainMenu.mShapeFork.Checked = false;
                        break;
                    case Model.NodeShape.Bubble:
                        mainMenu.mShapeBubble.Checked = true;
                        mainMenu.mShapeBox.Checked = false;
                        mainMenu.mShapeFork.Checked = false;
                        break;
                    case Model.NodeShape.Fork:
                        mainMenu.mShapeBubble.Checked = false;
                        mainMenu.mShapeBox.Checked = false;
                        mainMenu.mShapeFork.Checked = true;
                        break;
                    default:
                        mainMenu.mShapeBubble.Checked = false;
                        mainMenu.mShapeBox.Checked = false;
                        mainMenu.mShapeFork.Checked = false;
                        break;
                }
            }
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.Tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
            {
                mainMenu.mShapeBubble.Enabled = false;
                mainMenu.mShapeBox.Enabled = false;
                mainMenu.mShapeFork.Enabled = false;
                mainMenu.mShapeBubble.Checked = false;
                mainMenu.mShapeBox.Checked = false;
                mainMenu.mShapeFork.Checked = false;
            }
            else //multiple nodes selected
            {
                mainMenu.mShapeBubble.Enabled = true;
                mainMenu.mShapeBox.Enabled = true;
                mainMenu.mShapeFork.Enabled = true;
                mainMenu.mShapeBubble.Checked = false;
                mainMenu.mShapeBox.Checked = false;
                mainMenu.mShapeFork.Checked = false;
            }

        }

        void mShapeBox_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeNodeShapeBox();            
        }

        void mStyleBubble_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeNodeShapeBubble();
        }

        void mStyleFork_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeNodeShapeFork();
        }

        void mShapeBullet_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeNodeShapeBullet();
        }

        void mItalic_Click(object sender, EventArgs e)
        {
            mapCtrl.ToggleItalic();
        }

        void mBold_Click(object sender, EventArgs e)
        {
            mapCtrl.ToggleBold();
        }

        void mSelectIcon_Click(object sender, EventArgs e)
        {
            mapCtrl.AppendIconFromIconSelectorExt();
        }

        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.NewMap();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.OpenMap();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.ShowApplicationOptions();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.SaveCurrentMap();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.SaveCurrentMapAs();
        }


        #region Recent Files Menu Items

        private void CreateRecentFilesMenuItems()
        {
            int index = mainMenu.mFileMenu.DropDownItems.IndexOf(mainMenu.mSepRecentFilesStart);
            for(int i =0; i < MetaModel.MetaModel.Instance.RecentFiles.Count; i++)
            {
                ToolStripMenuItem menuItem;

                // setting file path for menu item
                string filePath = MetaModel.MetaModel.Instance.RecentFiles[i];
                if(filePath.Length <= 50)
                {
                    menuItem = new ToolStripMenuItem(filePath);
                }
                else
                {
                    menuItem = new ToolStripMenuItem(filePath.Substring(0, 5) + 
                        "..." + 
                        filePath.Substring(filePath.Length - 42));
                    menuItem.ToolTipText = MetaModel.MetaModel.Instance.RecentFiles[i];
                }                
                // setting event handler
                menuItem.Click += RecentFiles_Click;
                
                // adding menu item to File Menu
                mainMenu.mFileMenu.DropDownItems.Insert(index + i + 1, menuItem);
            }
        }

        public void RefreshRecentFilesMenuItems()
        {
            int index = mainMenu.mFileMenu.DropDownItems.IndexOf(mainMenu.mSepRecentFilesStart);
            int indexEnd = mainMenu.mFileMenu.DropDownItems.IndexOf(mainMenu.mSepRecentFilesEnd);
            
            for (int i = 0; i < MetaModel.MetaModel.Instance.RecentFiles.Count; i++)
            {
                ToolStripMenuItem menuItem;

                if (index + i + 1 >= indexEnd)
                {
                    menuItem = new ToolStripMenuItem();
                    menuItem.Click += RecentFiles_Click;
                    mainMenu.mFileMenu.DropDownItems.Insert(index + i + 1, menuItem);
                }
                else
                {
                    menuItem = (ToolStripMenuItem)mainMenu.mFileMenu.DropDownItems[index + i + 1];
                }

                // setting file path for menu item
                string filePath = MetaModel.MetaModel.Instance.RecentFiles[i];
                if (filePath.Length <= 50)
                {
                    menuItem.Text = filePath;
                    menuItem.ToolTipText = null;
                }
                else
                {
                    menuItem.Text = filePath.Substring(0, 5) + "..." + filePath.Substring(filePath.Length - 42);
                    menuItem.ToolTipText = MetaModel.MetaModel.Instance.RecentFiles[i];
                }               

                
            }

        }


        private void RecentFiles_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string filePath = menuItem.ToolTipText ?? menuItem.Text;

            mainCtrl.OpenMap(filePath);
        }

        #endregion

        #region Plugin Menu Items

        public void InsertMenuItems(Plugins.MainMenuItem[] menuItems)
        {
            foreach (Plugins.MainMenuItem menu in menuItems)
            {
                var winFormsMenu = ConstructMenuItem(menu);
                switch(menu.MainMenuLocation)
                {
                    case Plugins.MainMenuLocation.Separate:
                        mainMenu.Items.Insert(mainMenu.Items.Count - 2, winFormsMenu);
                        break;
                    case Plugins.MainMenuLocation.Tools:
                        mainMenu.mTools.DropDownItems.Add(winFormsMenu);
                        if (menu.AddSeparator)
                        {
                            mainMenu.mTools.DropDownItems.Add(new ToolStripSeparator());
                        }
                        break;
                    case Plugins.MainMenuLocation.Edit:
                        mainMenu.mEditMenu.DropDownItems.Add(winFormsMenu);
                        if (menu.AddSeparator)
                        {
                            mainMenu.mEditMenu.DropDownItems.Add(new ToolStripSeparator());
                        }
                        break;
                    case Plugins.MainMenuLocation.File:
                        mainMenu.mFileMenu.DropDownItems.Add(winFormsMenu);
                        if (menu.AddSeparator)
                        {
                            mainMenu.mFileMenu.DropDownItems.Add(new ToolStripSeparator());
                        }
                        break;
                    case Plugins.MainMenuLocation.Format:
                        mainMenu.mFormat.DropDownItems.Add(winFormsMenu);
                        if (menu.AddSeparator)
                        {
                            mainMenu.mFormat.DropDownItems.Add(new ToolStripSeparator());
                        }
                        break;
                    case Plugins.MainMenuLocation.Help:
                        mainMenu.mHelp.DropDownItems.Add(winFormsMenu);
                        if (menu.AddSeparator)
                        {
                            mainMenu.mHelp.DropDownItems.Add(new ToolStripSeparator());
                        }
                        break;
                }
                //winFormsMenu.Click += PluginMenuItem_Click;
                //SetClickHandlerForSubMenu(menu);
            }
        }

        private ToolStripMenuItem ConstructMenuItem(MainMenuItem pluginItem)
        {
            var menuItem = new ToolStripMenuItem(pluginItem.Text);
            menuItem.Click += pluginItem.Click;

            foreach (var dropDownItem in pluginItem.DropDownItems)
            {
                menuItem.DropDownItems.Add(ConstructMenuItem(dropDownItem));
                if (dropDownItem.AddSeparator)
                {
                    menuItem.DropDownItems.Add(new ToolStripSeparator());
                }
            }

            return menuItem;
        }

        //private void SetClickHandlerForSubMenu(Plugins.MenuItem menu)
        //{
        //    foreach (ToolStripDropDownItem subMenuItem in menu.UnderlyingMenuItem.DropDownItems)
        //    {
        //        subMenuItem.Click += PluginMenuItem_Click;
        //        SetClickHandlerForSubMenu((Plugins.MenuItem)(subMenuItem.Tag));
        //    }
        //}

        //void PluginMenuItem_Click(object sender, EventArgs e)
        //{
        //    Plugins.MenuItem menuItem = ((Plugins.MenuItem)((ToolStripMenuItem)sender).Tag);
        //    if (menuItem.Click != null)
        //        menuItem.Click(menuItem, this.mapCtrl.MapView.Tree.SelectedNodes);
        //}

        #endregion Plugin Menu Items

    }
}
