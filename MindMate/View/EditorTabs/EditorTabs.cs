using MindMate.Controller;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.EditorTabs
{
    /// <summary>
    /// Main Tabs Window for MapView and other view controls.
    /// Passive View: Controller manages events and updating the view from model.
    /// </summary>
    public sealed class EditorTabs : TabControl
    {
        public EditorTabs()
        {
            Dock = DockStyle.Fill;
            //HideHeader();  //TODO: Hide header if only one window
            SelectedIndexChanged += EditorTabs_SelectedIndexChanged;

        }
        private void HideHeader()
        {
            Appearance = TabAppearance.FlatButtons;
            ItemSize = new System.Drawing.Size(0, 1);
            SizeMode = TabSizeMode.Fixed; 
        }

        public new TabBase SelectedTab
        {
            get { return (TabBase)base.SelectedTab; }
            set { base.SelectedTab = value; }
        }

        /// <summary>
        /// Open MapView tab
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public Tab OpenTab(PersistentTree tree)
        {
            MapView mapView = new MapView(tree.Tree);
            Tab tab = new Tab(mapView, tree);
            TabPages.Add(tab);
            mapView.CenterOnForm();
            tab.UpdateTitle();

            if(TabCount == 1) { UpdateAppTitle(); }
            tab.TextChanged += Tab_TextChanged;

            return tab;
        }

        /// <summary>
        /// Open custom tab
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public TabPage OpenTab(Control control, string text)
        {
            TabBase tab = new TabBase(control);
            tab.Text = text;
            tab.TextChanged += Tab_TextChanged;
            return tab;
        }
                
        public void CloseTab(PersistentTree tree)
        {
            Tab tab = FindTab(tree);
            if(tab != null)
            {
                CloseTab(tab);
            }
        }     
        
        public void CloseTab(TabBase tab)
        {
            tab.TextChanged -= Tab_TextChanged;
            tab.Close();
        }       

        /// <summary>
        /// Focus the control on Selected Tab
        /// </summary>
        public new void Focus()
        {
            SelectedTab.Control.Focus();
        }

        public Tab FindTab(PersistentTree tree)
        {
            Tab tab = SelectedTab as Tab;

            if(tab != null && tab.Tree == tree)
            {
                return tab;
            }

            foreach(TabPage page in TabPages)
            {
                tab = page as Tab;
                if (tab.MapView.Tree == tree.Tree)
                {
                    return tab;
                }
            }

            return null;
        }

        private void Tab_TextChanged(object sender, EventArgs e)
        {
            UpdateAppTitle();
        }

        private void EditorTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAppTitle();
        }

        private void UpdateAppTitle()
        {
            Debug.Assert(TopLevelControl != null, "TopLevelControl != null");

            PersistentTree tree = (SelectedTab as Tab)?.Tree;
            if(tree != null)
            {
                TopLevelControl.Text = tree.Tree.RootNode.Text + " - " + Controller.MainCtrl.APPLICATION_NAME + " - " + tree.FileName;
                if (tree.IsDirty)
                {
                    TopLevelControl.Text += "*";
                }
            }
            else
            {
                TopLevelControl.Text = Controller.MainCtrl.APPLICATION_NAME + " - " + SelectedTab.Text;
            }         

            
        }

        protected override void OnGotFocus(EventArgs e)
        {
            SelectedTab.Control.Focus();
            //base.OnGotFocus(e);
        }

    }    

}
