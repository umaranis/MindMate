/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.View;

namespace MindMate.Controller
{
    /// <summary>
    /// Controller for Main Menu
    /// Manages state of Menu Items and passes the events to other controller
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
            form.openToolStripMenuItem.Click += this.openToolStripMenuItem_Click;
            form.saveToolStripMenuItem.Click += this.saveToolStripMenuItem_Click;
            form.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            form.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            form.asBMPToolStripMenuItem.Click += asBMPToolStripMenuItem_Click;
            
            form.mBold.Click += mBold_Click;
            form.mItalic.Click += mItalic_Click;

            form.mFont.Click += mFont_Click;
            form.mTextColor.Click += mTextColor_Click;
            form.mBackColor.Click += mBackColor_Click;

            form.mShapeFork.Click += mStyleFork_Click;
            form.mShapeBubble.Click += mStyleBubble_Click;
            form.mShapeBox.Click += mShapeBox_Click;
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
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
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
            else if (mapCtrl.MapView.SelectedNodes.Contains(mapCtrl.MapView.tree.RootNode) || mapCtrl.MapView.SelectedNodes.First == null)
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
            mapCtrl.appendIconFromIconSelector();
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

        
    }
}
