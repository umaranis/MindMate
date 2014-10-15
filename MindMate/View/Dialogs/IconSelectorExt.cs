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

        private void tbnShowTitle_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(listView.Items[0].Text))
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    listView.Items[i].Text = null;
                }
                tbnShowTitle.Text = "Show Title";
                tbnShowTitle.ToolTipText = "Show Icon Title";
            }
            else
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    listView.Items[i].Text = MetaModel.MetaModel.Instance.IconsList[listView.Items[i].ImageIndex].Title;
                }
                tbnShowTitle.Text = "Hide Title";
                tbnShowTitle.ToolTipText = "Hide Icon Title";
            }
        }

        private void tbnCustomize_Click(object sender, EventArgs e)
        {
            toolStrip1.Hide();
            toolStrip2.Show();
        }

        private void tbnMoveUp_Click(object sender, EventArgs e)
        {
            ListViewItem item1 = listView.SelectedItems[0];

            if(listView.SelectedItems.Count == 1 && item1.Index != 0)
            {
                listView.BeginUpdate();

                ListViewItem item2 = listView.Items[item1.Index - 1];
                SwapListItem(item1, item2);
                item1.Selected = false; 
                item2.Selected = true;
                item2.Focused = true;
                item1.EnsureVisible();
                
                listView.EndUpdate();

                //Action[] a = new Action[10];
                //a[0] = () => MetaModel.MetaModel.Instance.IconsList.Insert(item2.Index, MetaModel.MetaModel.Instance.IconsList[item1.Index]);
            }
        }

        private void tnMoveDown_Click(object sender, EventArgs e)
        {
            ListViewItem item1 = listView.SelectedItems[0];

            if (listView.SelectedItems.Count == 1 && item1.Index != listView.Items.Count - 1)
            {
                listView.BeginUpdate();

                ListViewItem item2 = listView.Items[item1.Index + 1];
                SwapListItem(item1, item2);
                item1.Selected = false;
                item2.Selected = true;
                item2.Focused = true;
                item1.EnsureVisible();

                listView.EndUpdate();
                
            }
        }

        private void SwapListItem(ListViewItem item1, ListViewItem item2)
        {
            var title = item1.Text;
            var shortcut = item1.SubItems[1].Text;
            var imageIndex = item1.ImageIndex;

            item1.Text = item2.Text;
            item1.SubItems[1].Text = item2.SubItems[1].Text;
            item1.ImageIndex = item2.ImageIndex;

            item2.Text = title;
            item2.SubItems[1].Text = shortcut;
            item2.ImageIndex = imageIndex;
            
        }

        private void tbnCancel_Click(object sender, EventArgs e)
        {
            toolStrip2.Hide();
            toolStrip1.Show();            
        }

        

        
    }
}