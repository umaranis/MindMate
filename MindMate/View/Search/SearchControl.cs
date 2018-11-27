using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MindMate.Controller;
using MindMate.Model;
using MindMate.View.Dialogs;
using MindMate.MetaModel;

namespace MindMate.View.Search
{
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        private void btnAddIcon_Click(object sender, EventArgs e)
        {
            if(IconSelectorExt.Instance.ShowDialog() == DialogResult.OK)
            {
                switch (IconSelectorExt.Instance.SelectedIcon)
                {
                    case IconSelectorExt.REMOVE_ICON_NAME:
                        if(pnlIcons.Controls.Count > 0)
                            pnlIcons.Controls.RemoveAt(pnlIcons.Controls.Count - 1);
                        break;
                    case IconSelectorExt.REMOVE_ALL_ICON_NAME:
                        pnlIcons.Controls.Clear();
                        break;
                    default:
                        ModelIcon icon = MetaModel.MetaModel.Instance.GetIcon(IconSelectorExt.Instance.SelectedIcon);
                        var picBox = new PictureBox();
                        picBox.SizeMode = PictureBoxSizeMode.AutoSize;
                        picBox.Image = icon.Bitmap;
                        picBox.Tag = icon.Name;
                        pnlIcons.Controls.Add(picBox);
                        break;
                }
            }
        }

        public SearchTerm CreateSearchTerm()
        {
            var term = new SearchTerm();
            term.Text = txtSearch.Text;
            term.ExcludeNote = ckbExcludeNote.Checked;
            term.StringComparison = ckbCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            term.SearchSelectedHierarchy = ckbSelectedNode.Checked;
            term.MatchAllIcons = !ckbAnyIcon.Checked;
            foreach(Control p in pnlIcons.Controls)
            {
                term.Icons.Add(p.Tag.ToString());
            }
            return term;
        }
    }
}
