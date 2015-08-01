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

namespace MindMate.Controller
{
    /// <summary>
    /// Controller for Main Menu.
    /// Manages state of Menu Items and passes the events to other controllers (event handlers don't contain any business logic).
    /// </summary>
    class MainMenuCtrl
    {
        public MainForm form;
        public MapCtrl mapCtrl;
        private MainCtrl mainCtrl; 

        public MainMenuCtrl(MainForm f, MapCtrl c, MainCtrl mainCtrl)
        {
            form = f;
            mapCtrl = c;
            this.mainCtrl = mainCtrl;

            form.mSelectIcon.Click += mSelectIcon_Click;

            form.newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            form.openToolStripMenuItem.Click += this.openToolStripMenuItem_Click;
            form.saveToolStripMenuItem.Click += this.saveToolStripMenuItem_Click;
            form.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            form.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            form.asBMPToolStripMenuItem.Click += asBMPToolStripMenuItem_Click;
            form.exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;

            form.mUndo.Click += mUndo_Click;
            form.mRedo.Click += mRedo_Click;
            form.mCut.Click += mCut_Click;
            form.mCopy.Click += mCopy_Click;
            form.mPaste.Click += mPaste_Click;
            form.mDelete.Click += mDelete_Click;
            form.mEditMenu.DropDownOpening += mEditMenu_DropDownOpening;
            form.mEditMenu.DropDownClosed += mEditMenu_DropDownClosed;
            
            form.mBold.Click += mBold_Click;
            form.mItalic.Click += mItalic_Click;

            form.mFont.Click += mFont_Click;
            form.mTextColor.Click += mTextColor_Click;
            form.mBackColor.Click += mBackColor_Click;

            form.mShapeFork.Click += mStyleFork_Click;
            form.mShapeBubble.Click += mStyleBubble_Click;
            form.mShapeBox.Click += mShapeBox_Click;
            form.mShapeBullet.Click += mShapeBullet_Click;
            form.mShape.DropDownOpening += mShape_DropDownOpening;

            form.mLineThick1.Click += mLineThick1_Click;
            form.mLineThick2.Click += mLineThick2_Click;
            form.mLineThick4.Click += mLineThick4_Click;
            form.mLineStyle.DropDownOpening += mLineStyle_DropDownOpening;
            form.mLineSolid.Click += mLineSolid_Click;
            form.mLineDashed.Click += mLineDashed_Click;
            form.mLineDotted.Click += mLineDotted_Click;
            form.mLineMixed.Click += mLineMixed_Click;

            form.mLineColor.Click += mLineColor_Click;

            form.mAbout.Click += mAbout_Click;

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
            form.mUndo.Enabled = mainCtrl.ChangeManager.CanUndo;
            form.mRedo.Enabled = mainCtrl.ChangeManager.CanRedo;
        }


        private void mEditMenu_DropDownClosed(object sender, EventArgs e)
        {
            form.mUndo.Enabled = true;
            form.mRedo.Enabled = true;
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
                form.mLineThick1.Enabled = true;
                form.mLineThick2.Enabled = true;
                form.mLineThick4.Enabled = true;

                form.mLineSolid.Enabled = true;
                form.mLineDashed.Enabled = true;
                form.mLineDotted.Enabled = true;
                form.mLineMixed.Enabled = true;

                switch(mapCtrl.MapView.SelectedNodes.First.LineWidth)
                {
                    case 1:
                        form.mLineThick1.Checked = true;
                        form.mLineThick2.Checked = false;
                        form.mLineThick4.Checked = false;
                        break;
                    case 2:
                        form.mLineThick1.Checked = false;
                        form.mLineThick2.Checked = true;
                        form.mLineThick4.Checked = false;
                        break;
                    case 4:
                        form.mLineThick1.Checked = false;
                        form.mLineThick2.Checked = false;
                        form.mLineThick4.Checked = true;
                        break;
                    default:
                        form.mLineThick1.Checked = false;
                        form.mLineThick2.Checked = false;
                        form.mLineThick4.Checked = false;
                        break;
                }

                switch (mapCtrl.MapView.SelectedNodes.First.LinePattern)
                {
                    case System.Drawing.Drawing2D.DashStyle.Solid:
                        form.mLineSolid.Checked = true;
                        form.mLineDashed.Checked = false;
                        form.mLineDotted.Checked = false;
                        form.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.Dash:
                        form.mLineSolid.Checked = false;
                        form.mLineDashed.Checked = true;
                        form.mLineDotted.Checked = false;
                        form.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.Dot:
                        form.mLineSolid.Checked = false;
                        form.mLineDashed.Checked = false;
                        form.mLineDotted.Checked = true;
                        form.mLineMixed.Checked = false;
                        break;
                    case System.Drawing.Drawing2D.DashStyle.DashDotDot:
                        form.mLineSolid.Checked = false;
                        form.mLineDashed.Checked = false;
                        form.mLineDotted.Checked = false;
                        form.mLineMixed.Checked = true;
                        break;
                    default:
                        form.mLineSolid.Checked = false;
                        form.mLineDashed.Checked = false;
                        form.mLineDotted.Checked = false;
                        form.mLineMixed.Checked = false;
                        break;
                }
            }
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.Tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
            {
                form.mLineThick1.Enabled = false;
                form.mLineThick2.Enabled = false;
                form.mLineThick4.Enabled = false;
                form.mLineThick1.Checked = false;
                form.mLineThick2.Checked = false;
                form.mLineThick4.Checked = false;

                form.mLineSolid.Enabled = false;
                form.mLineDashed.Enabled = false;
                form.mLineDotted.Enabled = false;
                form.mLineMixed.Enabled = false;
                form.mLineSolid.Checked = false;
                form.mLineDashed.Checked = false;
                form.mLineDotted.Checked = false;
                form.mLineMixed.Checked = false;
            }
            else //multiple nodes selected
            {
                form.mLineThick1.Enabled = true;
                form.mLineThick2.Enabled = true;
                form.mLineThick4.Enabled = true;
                form.mLineThick1.Checked = false;
                form.mLineThick2.Checked = false;
                form.mLineThick4.Checked = false;

                form.mLineSolid.Enabled = true;
                form.mLineDashed.Enabled = true;
                form.mLineDotted.Enabled = true;
                form.mLineMixed.Enabled = true;
                form.mLineSolid.Checked = false;
                form.mLineDashed.Checked = false;
                form.mLineDotted.Checked = false;
                form.mLineMixed.Checked = false;
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
                form.mShapeBubble.Enabled = true;
                form.mShapeBox.Enabled = true;
                form.mShapeFork.Enabled = true;

                switch (mapCtrl.MapView.SelectedNodes.First.Shape)
                {
                    case Model.NodeShape.Box:
                        form.mShapeBox.Checked = true;
                        form.mShapeBubble.Checked = false;
                        form.mShapeFork.Checked = false;
                        break;
                    case Model.NodeShape.Bubble:
                        form.mShapeBubble.Checked = true;
                        form.mShapeBox.Checked = false;
                        form.mShapeFork.Checked = false;
                        break;
                    case Model.NodeShape.Fork:
                        form.mShapeBubble.Checked = false;
                        form.mShapeBox.Checked = false;
                        form.mShapeFork.Checked = true;
                        break;
                    default:
                        form.mShapeBubble.Checked = false;
                        form.mShapeBox.Checked = false;
                        form.mShapeFork.Checked = false;
                        break;
                }
            }
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.Tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
            {
                form.mShapeBubble.Enabled = false;
                form.mShapeBox.Enabled = false;
                form.mShapeFork.Enabled = false;
                form.mShapeBubble.Checked = false;
                form.mShapeBox.Checked = false;
                form.mShapeFork.Checked = false;
            }
            else //multiple nodes selected
            {
                form.mShapeBubble.Enabled = true;
                form.mShapeBox.Enabled = true;
                form.mShapeFork.Enabled = true;
                form.mShapeBubble.Checked = false;
                form.mShapeBox.Checked = false;
                form.mShapeFork.Checked = false;
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
            int index = form.mFileMenu.DropDownItems.IndexOf(form.mSepRecentFilesStart);
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
                form.mFileMenu.DropDownItems.Insert(index + i + 1, menuItem);
            }
        }

        public void RefreshRecentFilesMenuItems()
        {
            int index = form.mFileMenu.DropDownItems.IndexOf(form.mSepRecentFilesStart);
            int indexEnd = form.mFileMenu.DropDownItems.IndexOf(form.mSepRecentFilesEnd);
            
            for (int i = 0; i < MetaModel.MetaModel.Instance.RecentFiles.Count; i++)
            {
                ToolStripMenuItem menuItem;

                if (index + i + 1 >= indexEnd)
                {
                    menuItem = new ToolStripMenuItem();
                    menuItem.Click += RecentFiles_Click;
                    form.mFileMenu.DropDownItems.Insert(index + i + 1, menuItem);
                }
                else
                {
                    menuItem = (ToolStripMenuItem)form.mFileMenu.DropDownItems[index + i + 1];
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
            MenuStrip mainMenu = this.form.menuStrip;

            foreach (Plugins.MainMenuItem menu in menuItems)
            {
                switch(menu.MainMenuLocation)
                {
                    case Plugins.MainMenuLocation.Separate:
                        mainMenu.Items.Insert(mainMenu.Items.Count - 2, menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Tools:
                        form.mTools.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Edit:
                        form.mEditMenu.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.File:
                        form.mFileMenu.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Format:
                        form.mFormat.DropDownItems.Add(menu.UnderlyingMenuItem);
                        break;
                    case Plugins.MainMenuLocation.Help:
                        form.mHelp.DropDownItems.Add(menu.UnderlyingMenuItem);
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
