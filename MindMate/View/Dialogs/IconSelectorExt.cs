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

            // adding items to ListView
            ImageList imageList = new ImageList();
            for (int i = 0; i < MetaModel.MetaModel.Instance.IconsList.Count; i++)
            {
                ModelIcon icon = MetaModel.MetaModel.Instance.IconsList[i];
                imageList.Images.Add(icon.Bitmap);
                listView.Items.Add(new ListViewItem(new string[] { icon.Title, icon.Shortcut }, i));
            }
            listView.SmallImageList = imageList;
            listView.LargeImageList = imageList;

            // setting columns for Detail View
            var columnHeader1 = new System.Windows.Forms.ColumnHeader();
            var columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader1.Text = "Icon";
            columnHeader2.Text = "Shortcut";
            listView.Columns.Add(columnHeader1);
            listView.Columns.Add(columnHeader2);


            listView.ItemActivate += listView_ItemActivate;
            SetViewButtonEnable(listView.View, false);
                                    
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

        private void tbnViewLargeIcons_Click(object sender, EventArgs e)
        {
            tbnViewLargeIcons.Enabled = false;
            SetViewButtonEnable(listView.View, true);
            listView.View = System.Windows.Forms.View.LargeIcon;            
        }

        private void tbnViewSmallIcons_Click(object sender, EventArgs e)
        {
            tbnViewSmallIcons.Enabled = false;
            SetViewButtonEnable(listView.View, true);
            listView.View = System.Windows.Forms.View.SmallIcon;
        }

        private void tbnViewList_Click(object sender, EventArgs e)
        {
            tbnViewList.Enabled = false;
            SetViewButtonEnable(listView.View, true);
            listView.View = System.Windows.Forms.View.List;
        }

        private void tbnViewTile_Click(object sender, EventArgs e)
        {
            tbnViewTile.Enabled = false;
            SetViewButtonEnable(listView.View, true);
            listView.View = System.Windows.Forms.View.Tile;
        }

        private void tbnViewDetail_Click(object sender, EventArgs e)
        {
            tbnViewDetail.Enabled = false;
            SetViewButtonEnable(listView.View, true);
            listView.View = System.Windows.Forms.View.Details;
        }

        public void SetViewButtonEnable(System.Windows.Forms.View view, bool enabled)
        {
            switch (view)
            {
                case System.Windows.Forms.View.Details:
                    tbnViewDetail.Enabled = enabled;
                    break;
                case System.Windows.Forms.View.LargeIcon:
                    tbnViewLargeIcons.Enabled = enabled;
                    break;
                case System.Windows.Forms.View.List:
                    tbnViewList.Enabled = enabled;
                    break;
                case System.Windows.Forms.View.SmallIcon:
                    tbnViewSmallIcons.Enabled = enabled;
                    break;
                case System.Windows.Forms.View.Tile:
                    tbnViewTile.Enabled = enabled;
                    break;
            }
        }

        
    }
}