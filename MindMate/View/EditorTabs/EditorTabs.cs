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
            HideHeader();  
            SelectedIndexChanged += EditorTabs_SelectedIndexChanged;
        }

        private void HideHeader()
        {
            Appearance = TabAppearance.FlatButtons;
            ItemSize = new System.Drawing.Size(0, 1);
            SizeMode = TabSizeMode.Fixed; 
        }

        private void ShowHeader()
        {
            Appearance = TabAppearance.Normal;
            ItemSize = new System.Drawing.Size(0, 0);
            SizeMode = TabSizeMode.Normal;
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
            MapView mapView = new MapView(tree);
            Tab tab = new Tab(mapView, tree);
            tab.UpdateTitle();
            OpenTabInternal(tab);
            mapView.CenterOnForm();

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

            OpenTabInternal(tab);

            return tab;
        }

        private void OpenTabInternal(TabBase tab)
        {
            TabPages.Add(tab);
            tab.TextChanged += Tab_TextChanged;
            tab.Control.GotFocus += Control_GotFocus;
            if (TabCount == 1) { UpdateAppTitle(); }
            if (TabCount == 2) { ShowHeader(); }
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
            tab.Control.GotFocus -= Control_GotFocus;
            tab.Close();

            if(TabCount == 1) { HideHeader(); }
        }

        /// <summary>
        /// Focus the control on Selected Tab
        /// </summary>
        public new void Focus()
        {
            SelectedTab.Control.Focus();
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            ControlGotFocus?.Invoke(sender, e);
        }

        /// <summary>
        /// Occurs when any control within EditorTab get focus
        /// </summary>
        public event Action<object, EventArgs> ControlGotFocus;

        public Tab FindTab(PersistentTree tree)
        {
            if (tree == null) return null;

            Tab tab = SelectedTab as Tab;

            if(tab != null && tab.Tree == tree)
            {
                return tab;
            }

            foreach(TabPage page in TabPages)
            {
                tab = page as Tab;
                if (tab.MapView.Tree == tree)
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

        public void UpdateAppTitle()
        {
            Debug.Assert(TopLevelControl != null, "TopLevelControl != null");

            PersistentTree tree = (SelectedTab as Tab)?.Tree;
            if(tree != null)
            {
                TopLevelControl.Text = tree.RootNode.Text + " - " + Controller.MainCtrl.APPLICATION_NAME + " - " + tree.FileName;
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
            //Ideally selected tab should never be null, but it happens internally in TabControl during ShowHeader
            SelectedTab?.Control.Focus();
            //base.OnGotFocus(e);
        }

    }    

}
