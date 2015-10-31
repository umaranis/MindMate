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

namespace MindMate.WinXP
{
    /// <summary>
    /// Controller for Main Menu.
    /// Manages state of Menu Items and passes the events to other controllers (event handlers don't contain any business logic).
    /// </summary>
    public class MainMenuCtrl
    {
        public MainForm form;
        public MapCtrl mapCtrl { get { return mainCtrl.mapCtrl; } }
        private MainCtrl mainCtrl; 

        public MainMenuCtrl(MainForm f, MainCtrl mainCtrl)
        {
            form = f;
            this.mainCtrl = mainCtrl;

            form.MainMenu.mSelectIcon.Click += mSelectIcon_Click;

            form.MainMenu.newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            form.MainMenu.openToolStripMenuItem.Click += this.openToolStripMenuItem_Click;
            form.MainMenu.saveToolStripMenuItem.Click += this.saveToolStripMenuItem_Click;
            form.MainMenu.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            form.MainMenu.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            form.MainMenu.asBMPToolStripMenuItem.Click += asBMPToolStripMenuItem_Click;
            form.MainMenu.exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;

            form.MainMenu.mUndo.Click += mUndo_Click;
            form.MainMenu.mRedo.Click += mRedo_Click;
            form.MainMenu.mCut.Click += mCut_Click;
            form.MainMenu.mCopy.Click += mCopy_Click;
            form.MainMenu.mPaste.Click += mPaste_Click;
            form.MainMenu.mDelete.Click += mDelete_Click;
            form.MainMenu.mEditMenu.DropDownOpening += mEditMenu_DropDownOpening;
            form.MainMenu.mEditMenu.DropDownClosed += mEditMenu_DropDownClosed;
            
            form.MainMenu.mBold.Click += mBold_Click;
            form.MainMenu.mItalic.Click += mItalic_Click;

            form.MainMenu.mFont.Click += mFont_Click;
            form.MainMenu.mTextColor.Click += mTextColor_Click;
            form.MainMenu.mBackColor.Click += mBackColor_Click;

            form.MainMenu.mShapeFork.Click += mStyleFork_Click;
            form.MainMenu.mShapeBubble.Click += mStyleBubble_Click;
            form.MainMenu.mShapeBox.Click += mShapeBox_Click;
            form.MainMenu.mShapeBullet.Click += mShapeBullet_Click;
            form.MainMenu.mShape.DropDownOpening += mShape_DropDownOpening;

            form.MainMenu.mLineThick1.Click += mLineThick1_Click;
            form.MainMenu.mLineThick2.Click += mLineThick2_Click;
            form.MainMenu.mLineThick4.Click += mLineThick4_Click;
            form.MainMenu.mLineStyle.DropDownOpening += mLineStyle_DropDownOpening;
            form.MainMenu.mLineSolid.Click += mLineSolid_Click;
            form.MainMenu.mLineDashed.Click += mLineDashed_Click;
            form.MainMenu.mLineDotted.Click += mLineDotted_Click;
            form.MainMenu.mLineMixed.Click += mLineMixed_Click;

            form.MainMenu.mLineColor.Click += mLineColor_Click;

            form.MainMenu.mAbout.Click += mAbout_Click;

            CreateRecentFilesMenuItems();
        }
        void mAbout_Click(object sender, EventArgs e)
        {
            mainCtrl.ShowAboutBox();
        }

        void mBackColor_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeBackgroundColor();
        }

        void mTextColor_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeTextColor();
        }

        void asBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.ExportAsBMP();
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
            form.MainMenu.mUndo.Enabled = mainCtrl.ChangeManager.CanUndo;
            form.MainMenu.mRedo.Enabled = mainCtrl.ChangeManager.CanRedo;
        }


        private void mEditMenu_DropDownClosed(object sender, EventArgs e)
        {
            form.MainMenu.mUndo.Enabled = true;
            form.MainMenu.mRedo.Enabled = true;
        }

        void mFont_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeFont();
        }

        void mLineColor_Click(object sender, EventArgs e)
        {
            mapCtrl.ChangeLineColor();                      
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
                form.MainMenu.mLineThick1.Enabled = true;
                form.MainMenu.mLineThick2.Enabled = true;
                form.MainMenu.mLineThick4.Enabled = true;

                form.MainMenu.mLineSolid.Enabled = true;
                form.MainMenu.mLineDashed.Enabled = true;
                form.MainMenu.mLineDotted.Enabled = true;
                form.MainMenu.mLineMixed.Enabled = true;

                switch(mapCtrl.MapView.SelectedNodes.First.LineWidth)
                {
                    case 1:
                        form.MainMenu.mLineThick1.Checked = true;
                        form.MainMenu.mLineThick2.Checked = false;
                        form.MainMenu.mLineThick4.Checked = false;
                        break;
                    case 2:
                        form.MainMenu.mLineThick1.Checked = false;
                        form.MainMenu.mLineThick2.Checked = true;
                        form.MainMenu.mLineThick4.Checked = false;
                        break;
                    case 4:
                        form.MainMenu.mLineThick1.Checked = false;
                        form.MainMenu.mLineThick2.Checked = false;
                        form.MainMenu.mLineThick4.Checked = true;
                        break;
                    default:
                        form.MainMenu.mLineThick1.Checked = false;
                        form.MainMenu.mLineThick2.Checked = false;
                        form.MainMenu.mLineThick4.Checked = false;
                        break;
                }

                switch (mapCtrl.MapView.SelectedNodes.First.LinePattern)
                {
                    case System.Drawing.Drawing2D.DashStyle.Solid:
                        form.MainMenu.mLineSolid.Checked = true;
                        form.MainMenu.mLineDashed.Checked = false;
                        form.MainMenu.mLineDotted.Checked = false;
                        form.MainMenu.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.Dash:
                        form.MainMenu.mLineSolid.Checked = false;
                        form.MainMenu.mLineDashed.Checked = true;
                        form.MainMenu.mLineDotted.Checked = false;
                        form.MainMenu.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.Dot:
                        form.MainMenu.mLineSolid.Checked = false;
                        form.MainMenu.mLineDashed.Checked = false;
                        form.MainMenu.mLineDotted.Checked = true;
                        form.MainMenu.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.DashDotDot:
                        form.MainMenu.mLineSolid.Checked = false;
                        form.MainMenu.mLineDashed.Checked = false;
                        form.MainMenu.mLineDotted.Checked = false;
                        form.MainMenu.mLineMixed.Checked = true;
                        break;
                    default:
                        form.MainMenu.mLineSolid.Checked = false;
                        form.MainMenu.mLineDashed.Checked = false;
                        form.MainMenu.mLineDotted.Checked = false;
                        form.MainMenu.mLineMixed.Checked = false;
                        break;
                }
            }
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.Tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
            {
                form.MainMenu.mLineThick1.Enabled = false;
                form.MainMenu.mLineThick2.Enabled = false;
                form.MainMenu.mLineThick4.Enabled = false;
                form.MainMenu.mLineThick1.Checked = false;
                form.MainMenu.mLineThick2.Checked = false;
                form.MainMenu.mLineThick4.Checked = false;

                form.MainMenu.mLineSolid.Enabled = false;
                form.MainMenu.mLineDashed.Enabled = false;
                form.MainMenu.mLineDotted.Enabled = false;
                form.MainMenu.mLineMixed.Enabled = false;
                form.MainMenu.mLineSolid.Checked = false;
                form.MainMenu.mLineDashed.Checked = false;
                form.MainMenu.mLineDotted.Checked = false;
                form.MainMenu.mLineMixed.Checked = false;
            }
            else //multiple nodes selected
            {
                form.MainMenu.mLineThick1.Enabled = true;
                form.MainMenu.mLineThick2.Enabled = true;
                form.MainMenu.mLineThick4.Enabled = true;
                form.MainMenu.mLineThick1.Checked = false;
                form.MainMenu.mLineThick2.Checked = false;
                form.MainMenu.mLineThick4.Checked = false;

                form.MainMenu.mLineSolid.Enabled = true;
                form.MainMenu.mLineDashed.Enabled = true;
                form.MainMenu.mLineDotted.Enabled = true;
                form.MainMenu.mLineMixed.Enabled = true;
                form.MainMenu.mLineSolid.Checked = false;
                form.MainMenu.mLineDashed.Checked = false;
                form.MainMenu.mLineDotted.Checked = false;
                form.MainMenu.mLineMixed.Checked = false;
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
                form.MainMenu.mShapeBubble.Enabled = true;
                form.MainMenu.mShapeBox.Enabled = true;
                form.MainMenu.mShapeFork.Enabled = true;

                switch (mapCtrl.MapView.SelectedNodes.First.Shape)
                {
                    case Model.NodeShape.Box:
                        form.MainMenu.mShapeBox.Checked = true;
                        form.MainMenu.mShapeBubble.Checked = false;
                        form.MainMenu.mShapeFork.Checked = false;
                        break;
                    case Model.NodeShape.Bubble:
                        form.MainMenu.mShapeBubble.Checked = true;
                        form.MainMenu.mShapeBox.Checked = false;
                        form.MainMenu.mShapeFork.Checked = false;
                        break;
                    case Model.NodeShape.Fork:
                        form.MainMenu.mShapeBubble.Checked = false;
                        form.MainMenu.mShapeBox.Checked = false;
                        form.MainMenu.mShapeFork.Checked = true;
                        break;
                    default:
                        form.MainMenu.mShapeBubble.Checked = false;
                        form.MainMenu.mShapeBox.Checked = false;
                        form.MainMenu.mShapeFork.Checked = false;
                        break;
                }
            }
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.Tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
            {
                form.MainMenu.mShapeBubble.Enabled = false;
                form.MainMenu.mShapeBox.Enabled = false;
                form.MainMenu.mShapeFork.Enabled = false;
                form.MainMenu.mShapeBubble.Checked = false;
                form.MainMenu.mShapeBox.Checked = false;
                form.MainMenu.mShapeFork.Checked = false;
            }
            else //multiple nodes selected
            {
                form.MainMenu.mShapeBubble.Enabled = true;
                form.MainMenu.mShapeBox.Enabled = true;
                form.MainMenu.mShapeFork.Enabled = true;
                form.MainMenu.mShapeBubble.Checked = false;
                form.MainMenu.mShapeBox.Checked = false;
                form.MainMenu.mShapeFork.Checked = false;
            }

        }

        void mShapeBox_Click(object sender, EventArgs e)
        {
            mapCtrl.MakeSelectedNodeShapeBox();            
        }

        void mStyleBubble_Click(object sender, EventArgs e)
        {
            mapCtrl.MakeSelectedNodeShapeBubble();
        }

        void mStyleFork_Click(object sender, EventArgs e)
        {
            mapCtrl.MakeSelectedNodeShapeFork();
        }

        void mShapeBullet_Click(object sender, EventArgs e)
        {
            mapCtrl.MakeSelectedNodeShapeBullet();
        }

        void mItalic_Click(object sender, EventArgs e)
        {
            mapCtrl.MakeSelectedNodeItalic();
        }

        void mBold_Click(object sender, EventArgs e)
        {
            mapCtrl.MakeSelectedNodeBold();
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
            mainCtrl.SaveMap();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainCtrl.SaveAsMap();
        }


        #region Recent Files Menu Items

        private void CreateRecentFilesMenuItems()
        {
            int index = form.MainMenu.mFileMenu.DropDownItems.IndexOf(form.MainMenu.mSepRecentFilesStart);
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
                form.MainMenu.mFileMenu.DropDownItems.Insert(index + i + 1, menuItem);
            }
        }

        public void RefreshRecentFilesMenuItems()
        {
            int index = form.MainMenu.mFileMenu.DropDownItems.IndexOf(form.MainMenu.mSepRecentFilesStart);
            int indexEnd = form.MainMenu.mFileMenu.DropDownItems.IndexOf(form.MainMenu.mSepRecentFilesEnd);
            
            for (int i = 0; i < MetaModel.MetaModel.Instance.RecentFiles.Count; i++)
            {
                ToolStripMenuItem menuItem;

                if (index + i + 1 >= indexEnd)
                {
                    menuItem = new ToolStripMenuItem();
                    menuItem.Click += RecentFiles_Click;
                    form.MainMenu.mFileMenu.DropDownItems.Insert(index + i + 1, menuItem);
                }
                else
                {
                    menuItem = (ToolStripMenuItem)form.MainMenu.mFileMenu.DropDownItems[index + i + 1];
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
            string filePath = menuItem.ToolTipText != null ? menuItem.ToolTipText : menuItem.Text;

            mainCtrl.OpenMap(filePath);
        }

        #endregion

        #region Plugin Menu Items

        public void InsertMenuItems(Plugins.MainMenuItem[] menuItems)
        {
            MenuStrip mainMenu = this.form.MainMenu;

            foreach (Plugins.MainMenuItem menu in menuItems)
            {
                switch(menu.MainMenuLocation)
                {
                    case Plugins.MainMenuLocation.Separate:
                        mainMenu.Items.Insert(mainMenu.Items.Count - 2, menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Tools:
                        form.MainMenu.mTools.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Edit:
                        form.MainMenu.mEditMenu.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.File:
                        form.MainMenu.mFileMenu.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Format:
                        form.MainMenu.mFormat.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Help:
                        form.MainMenu.mHelp.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                }
                menu.UnderlyingMenuItem.Click += PluginMenuItem_Click;
                SetClickHandlerForSubMenu(menu);
            }
        }

        private void SetClickHandlerForSubMenu(Plugins.MenuItem menu)
        {
            foreach (ToolStripDropDownItem subMenuItem in menu.UnderlyingMenuItem.DropDownItems)
            {
                subMenuItem.Click += PluginMenuItem_Click;
                SetClickHandlerForSubMenu((Plugins.MenuItem)(subMenuItem.Tag));
            }
        }

        void PluginMenuItem_Click(object sender, EventArgs e)
        {
            Plugins.MenuItem menuItem = ((Plugins.MenuItem)((ToolStripMenuItem)sender).Tag);
            if (menuItem.Click != null)
                menuItem.Click(menuItem, this.mapCtrl.MapView.Tree.SelectedNodes);
        }

        #endregion Plugin Menu Items

    }
}
