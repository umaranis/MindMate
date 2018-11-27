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
        public static readonly IconSelectorExt Instance = new IconSelectorExt();

        public const string REMOVE_ICON_NAME = "remove";
        public const string REMOVE_ALL_ICON_NAME = "removeAll";

        public string SelectedIcon = null;

        private IconSelectorExt()
        {
            InitializeComponent();

            // adding items to ListView
            ImageList imageList = new ImageList();
            MetaModel.MetaModel.Initialize();
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

            listView.Items[0].Selected = true;


            listView.ItemActivate += listView_ItemActivate;
            SetViewButtonEnable(listView.View, false);

            listView.AfterLabelEdit += listView_AfterLabelEdit;
                                    
        }

        /// <summary>
        /// Event Handler for ListView Item Activation (double click, enter key)
        /// </summary>
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
                    this.SelectedIcon = null;
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

        /// <summary>
        /// Event Handler for form activation
        /// </summary>
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

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.listView_ItemActivate(listView, new EventArgs());
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        #region Customize Icon Selector Dialog *********************************

        public bool IsCustomizing
        {
            get { return toolStrip2.Visible; }
        }

        private void StartCustomizing()
        {
            toolStrip1.Hide();
            toolStrip2.Show();

            listView.LabelEdit = true;
            SetShortcutTextBox();

            this.KeyDown -= this.IconSelector_KeyDown;
            listView.ItemActivate -= this.listView_ItemActivate;

            this.KeyDown += Customization_KeyDown;
            listView.SelectedIndexChanged += listView_SelectedIndexChanged;
        }

              
        private void EndCustomizing()
        {
            toolStrip2.Hide();
            toolStrip1.Show();

            listView.LabelEdit = false;

            this.KeyDown += IconSelector_KeyDown;
            listView.ItemActivate += this.listView_ItemActivate;

            this.KeyDown -= Customization_KeyDown;
            listView.SelectedIndexChanged -= listView_SelectedIndexChanged;

            MetaModel.MetaModel.Instance.Save();

        }

        void Customization_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                EndCustomizing();
            else if (e.KeyCode == Keys.F2)
                BeginIconTextEdit();
        }

        private void BeginIconTextEdit()
        {
            if(listView.SelectedItems.Count == 1)
                listView.SelectedItems[0].BeginEdit();
        }

        private void tbnEditTitle_Click(object sender, EventArgs e)
        {
            BeginIconTextEdit();
        }

        void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null) //e.Label is null if editing ended without any change
            {
                MetaModel.MetaModel.Instance.IconsList[e.Item].Title = e.Label;
            }
        }

        void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetShortcutTextBox();
        }

        private void SetShortcutTextBox()
        {
            if (listView.SelectedItems.Count == 1)
                ttbShortcut.Text = listView.SelectedItems[0].SubItems[1].Text;
        }

        private void ttbShortcut_TextChanged(object sender, EventArgs e)
        {
            if(listView.SelectedItems.Count == 1)
            {
                var selectedItem = listView.SelectedItems[0];
                ttbShortcut.Text = ttbShortcut.Text.ToUpper();
                selectedItem.SubItems[1].Text = ttbShortcut.Text;
                MetaModel.MetaModel.Instance.IconsList[selectedItem.Index].Shortcut = ttbShortcut.Text;
            }
        }

        private void tbnCustomize_Click(object sender, EventArgs e)
        {
            StartCustomizing();
        }

        private void tbnMoveUp_Click(object sender, EventArgs e)
        {
            ListViewItem item1;

            if(listView.SelectedItems.Count == 1 && (item1 = listView.SelectedItems[0]).Index != 0)
            {
                listView.BeginUpdate();

                ListViewItem item2 = listView.Items[item1.Index - 1];
                SwapListItem(item1, item2);
                item1.Selected = false; 
                item2.Selected = true;
                item2.Focused = true;
                item2.EnsureVisible();
                
                listView.EndUpdate();

                //Action[] a = new Action[10];
                //a[0] = () => MetaModel.MetaModel.Instance.IconsList.Insert(item2.Index, MetaModel.MetaModel.Instance.IconsList[item1.Index]);
            }
        }

        private void tnMoveDown_Click(object sender, EventArgs e)
        {
            ListViewItem item1;

            if (listView.SelectedItems.Count == 1 && (item1 = listView.SelectedItems[0]).Index != listView.Items.Count - 1)
            {
                listView.BeginUpdate();

                ListViewItem item2 = listView.Items[item1.Index + 1];
                SwapListItem(item1, item2);
                item1.Selected = false;
                item2.Selected = true;
                item2.Focused = true;
                item2.EnsureVisible();

                listView.EndUpdate();
                
            }
        }

        /// <summary>
        /// Couldn't find a way to move items so contents are swapped instead.
        /// </summary>        
        private void SwapListItem(ListViewItem item1, ListViewItem item2)
        {
            // switch images in ImageList, rather than swapping ImageIndex
            var tempImage = listView.SmallImageList.Images[item1.Index];
            listView.SmallImageList.Images[item1.Index] = listView.SmallImageList.Images[item2.Index];
            listView.SmallImageList.Images[item2.Index] = tempImage;

            var tempTitle = item1.Text;
            var tempShortcut = item1.SubItems[1].Text;

            item1.Text = item2.Text;
            item1.SubItems[1].Text = item2.SubItems[1].Text;

            item2.Text = tempTitle;
            item2.SubItems[1].Text = tempShortcut;

            var tempIcon = MetaModel.MetaModel.Instance.IconsList[item1.Index];
            MetaModel.MetaModel.Instance.IconsList[item1.Index] = MetaModel.MetaModel.Instance.IconsList[item2.Index];
            MetaModel.MetaModel.Instance.IconsList[item2.Index] = tempIcon;           
            
        }

        
        private void tbnBack_Click(object sender, EventArgs e)
        {
            EndCustomizing();            
        }

        
        private void IconSelectorExt_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (IsCustomizing) EndCustomizing();
        }

        #endregion Customize Icon Selector Dialog *********************************


    }
}