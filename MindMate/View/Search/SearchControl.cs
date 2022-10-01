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
        public event EventHandler<SearchTerm> SearchTermChanged;
        public event EventHandler<SearchTerm> SearchResultSelected;
        public event EventHandler<SearchTerm> SearchResultAllSelected;
        
        public SearchControl()
        {
            InitializeComponent();
            
            btnClear.Click += (o, e) => lstResults.Items.Clear();
            
            // trigger events
            lstResults.SelectedIndexChanged += (s, e) => SearchResultSelected?.Invoke(s, CreateSearchTerm());
            btnSearch.Click += (s, e) => SearchTermChanged?.Invoke(s, CreateSearchTerm(true));
            txtSearch.TextChanged += (s, e) => SearchTermChanged?.Invoke(s, CreateSearchTerm());
            btnSelect.Click += (s, e) => SearchResultAllSelected?.Invoke(s, CreateSearchTerm());
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
                        toolTip1.SetToolTip(picBox, icon.Title);
                        picBox.ContextMenuStrip = new ContextMenuStrip();
                        picBox.ContextMenuStrip.Items.Add("Remove", null, (o, evn) => pnlIcons.Controls.Remove(picBox));                         
                        pnlIcons.Controls.Add(picBox);
                        break;
                }
            }
        }

        public SearchTerm CreateSearchTerm(bool force = false)
        {
            var term = new SearchTerm
            {
                Text = txtSearch.Text,
                ExcludeNote = ckbExcludeNote.Checked,
                StringComparison = ckbCase.Checked ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase,
                SearchSelectedHierarchy = ckbSelectedNode.Checked,
                MatchAllIcons = !ckbAnyIcon.Checked,
                Force = force 
            };
            foreach(Control p in pnlIcons.Controls)
            {
                term.Icons.Add(p.Tag.ToString());
            }
            return term;
        }
        
        public void InvokeInUIThread(Action action)
        {
            this.Invoke(action);
        }

        public void ClearResults()
        {
            lstResults.Items.Clear();
        }

        public MapNode SelectedResultMapNode
        {
            get
            {
                if(this.lstResults.SelectedItem is MapNode node && node.Detached)
                {
                    lstResults.Items.Remove(lstResults.SelectedItem);
                }
                return this.lstResults.SelectedItem as MapNode;
            }
        }

        public IEnumerable<MapNode> Results => lstResults.Items.OfType<MapNode>();        

        public void AddResult(MapNode node)
        {
            lstResults.Items.Add(node);
        }
    }
}
