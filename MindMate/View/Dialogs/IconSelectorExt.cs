/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MindMate.MetaModel;

namespace MindMate.View.Dialogs
{
    public partial class IconSelectorExt : Form
    {
        public static IconSelectorExt Instance = new IconSelectorExt();

        public const string REMOVE_ICON_NAME = "remove";
        public const string REMOVE_ALL_ICON_NAME = "removeAll";

        public string SelectedIcon = "";

        private IconSelectorExt()
        {
            InitializeComponent();

            Debugging.Utility.StartTimeCounter("Loading icons");

            ImageList imageList = new ImageList();
            for (int i = 0; i < MetaModel.MetaModel.Instance.IconsList.Count; i++)
            {
                ModelIcon icon = MetaModel.MetaModel.Instance.IconsList[i];
                imageList.Images.Add(icon.Bitmap);
                listView.Items.Add(icon.Title, i);
            }
            listView.SmallImageList = imageList;
            listView.LargeImageList = imageList;

            listView.ItemActivate += listView_ItemActivate;
                                    
            Debugging.Utility.EndTimeCounter("Loading icons");
        }

        void listView_ItemActivate(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                CloseForm(MetaModel.MetaModel.Instance.IconsList[listView.SelectedItems[0].ImageIndex].Name);
        }        


        void b_LostFocus(object sender, EventArgs e)
        {
            ((Button)sender).FlatStyle = FlatStyle.Standard; 
        }

        void b_GotFocus(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = toolTip1.GetToolTip((Control)sender);
            ((Button)sender).FlatStyle = FlatStyle.Flat; 
        }

        void b_MouseHover(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        void b_Click(object sender, EventArgs e)
        {
            CloseForm(((Button)sender).Name);
        }

        
        private Button CreateRemoveIconButton()
        {
            Button bRemove = new Button();
            bRemove.Name = REMOVE_ICON_NAME;
            bRemove.FlatStyle = FlatStyle.Standard;
            bRemove.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            bRemove.Image = MindMate.Properties.Resources.remove;
            bRemove.Size = new Size(27, 27);
            toolTip1.SetToolTip(bRemove, "Remove Last Icon, Backspace");
            bRemove.Margin = new Padding(0);
            //bRemove.Tag = "";
            bRemove.MouseHover += new EventHandler(b_MouseHover);
            bRemove.Click += new EventHandler(b_Click);
            bRemove.GotFocus += b_GotFocus;
            bRemove.LostFocus += b_LostFocus;

            return bRemove;
           
        }

        private Button CreateRemoveAllIconButton()
        {
            Button bRemoveAll = new Button();
            bRemoveAll.Name = REMOVE_ALL_ICON_NAME;
            bRemoveAll.FlatStyle = FlatStyle.Standard;
            bRemoveAll.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            bRemoveAll.Image = MindMate.Properties.Resources.edittrash;
            bRemoveAll.Size = new Size(27, 27);
            toolTip1.SetToolTip(bRemoveAll, "Remove All Icons, Delete");
            bRemoveAll.Margin = new Padding(0);
            //bRemoveAll.Tag = "";
            bRemoveAll.MouseHover += new EventHandler(b_MouseHover);
            bRemoveAll.Click += new EventHandler(b_Click);
            bRemoveAll.GotFocus += b_GotFocus;
            bRemoveAll.LostFocus += b_LostFocus;

            return bRemoveAll;

        }

                
        private void IconSelector_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Back: //Backspace key pressed
                    CloseForm(REMOVE_ICON_NAME);                    
                    break;
                case Keys.Delete: //Delete key pressed    
                    CloseForm(REMOVE_ALL_ICON_NAME);
                    break;           
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;                
                default: //Other keys                    
                    foreach (ModelIcon icon in MetaModel.MetaModel.Instance.IconsList)
                    {
                        if (icon.Shortcut == ((char)e.KeyValue).ToString().ToUpper())
                        {
                            CloseForm(icon.Name);
                        }
                    }
                    break;
            }
            //e.SuppressKeyPress = true;
            //e.Handled = true;
        }   
     
        private void CloseForm(string selectedIcon)
        {
            this.SelectedIcon = selectedIcon;
            this.DialogResult = DialogResult.OK;
        }

        private void IconSelectorExt_Activated(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                listView.FocusedItem = listView.SelectedItems[0]; 
        }

        private void tbnRemoveLast_Click(object sender, EventArgs e)
        {
            CloseForm(IconSelectorExt.REMOVE_ICON_NAME);
        }

        private void tbnRemoveAll_Click(object sender, EventArgs e)
        {
            CloseForm(IconSelectorExt.REMOVE_ALL_ICON_NAME);
        }

        
    }
}