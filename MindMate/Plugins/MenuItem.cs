using MindMate.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MindMate.Plugins
{
    public class MenuItem
    {
        private ToolStripMenuItem underlyingMenuItem;
        public ToolStripMenuItem UnderlyingMenuItem
        {
            get => underlyingMenuItem;

            private set
            {
                underlyingMenuItem = value;
                underlyingMenuItem.Tag = this;
            }
        }

        public Action<MenuItem, SelectedNodes> Click { get; set; }


        #region Constructors


        public MenuItem()
        {
            UnderlyingMenuItem = new ToolStripMenuItem();
        }


        public MenuItem(Image image)
        {
            UnderlyingMenuItem = new ToolStripMenuItem(image);
        }


        public MenuItem(string text)
        {
            UnderlyingMenuItem = new ToolStripMenuItem(text);
        }


        public MenuItem(string text, Image image)
        {
            UnderlyingMenuItem = new ToolStripMenuItem(text, image);
        }


        public MenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick)
        {
            UnderlyingMenuItem = new ToolStripMenuItem(text, image);
            this.Click += onClick;
        }


        public MenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick, Keys shortcutKeys)
        {
            this.UnderlyingMenuItem = new ToolStripMenuItem(text, image);
            this.Click = onClick;
            this.UnderlyingMenuItem.ShortcutKeys = shortcutKeys;
        }


        public MenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick, string name)
        {
            this.UnderlyingMenuItem = new ToolStripMenuItem(text, image);
            this.Click = onClick;
            this.UnderlyingMenuItem.Name = name;
        }

        #endregion Constructors


        public void AddDropDownItem(MenuItem item)
        {
            UnderlyingMenuItem.DropDownItems.Add(item.UnderlyingMenuItem);
        }

        public void AddDropDownItemSeparator()
        {
            UnderlyingMenuItem.DropDownItems.Add(new ToolStripSeparator());
        }

        public bool Enabled
        {
            get => UnderlyingMenuItem.Enabled;
            set => UnderlyingMenuItem.Enabled = value;
        }

        public bool Visible
        {
            get => UnderlyingMenuItem.Visible;
            set => UnderlyingMenuItem.Visible = value;
        }
    }
}
