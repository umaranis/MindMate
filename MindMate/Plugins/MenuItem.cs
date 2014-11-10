using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins
{
    public class MenuItem : ToolStripMenuItem
    {
        public new Action<MenuItem, SelectedNodes> Click { get; set; }

        public Action<MenuItem, SelectedNodes> Opening { get; set; }

        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class.
        public MenuItem() : base() {}
        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class that displays the specified System.Drawing.Image.
        //
        // Parameters:
        //   image:
        //     The System.Drawing.Image to display on the control.
        public MenuItem(Image image) : base(image) { }
        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class that displays the specified text.
        //
        // Parameters:
        //   text:
        //     The text to display on the menu item.
        public MenuItem(string text) : base(text) { }
        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class that displays the specified text and image.
        //
        // Parameters:
        //   text:
        //     The text to display on the menu item.
        //
        //   image:
        //     The System.Drawing.Image to display on the control.
        public MenuItem(string text, Image image) : base(text, image) { }
        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class that displays the specified text and image and that does the specified
        //     action when the System.Windows.Forms.MenuItem is clicked.
        //
        // Parameters:
        //   text:
        //     The text to display on the menu item.
        //
        //   image:
        //     The System.Drawing.Image to display on the control.
        //
        //   onClick:
        //     An event handler that raises the System.Windows.Forms.Control.Click event
        //     when the control is clicked.
        public MenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick) : base(text, image)
        {
            this.Click += onClick;
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class that displays the specified text and image and that contains the specified
        //     System.Windows.Forms.ToolStripItem collection.
        //
        // Parameters:
        //   text:
        //     The text to display on the menu item.
        //
        //   image:
        //     The System.Drawing.Image to display on the control.
        //
        //   dropDownItems:
        //     The menu items to display when the control is clicked.
        public MenuItem(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems) { }

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class that displays the specified text and image, does the specified action
        //     when the System.Windows.Forms.MenuItem is clicked, and displays
        //     the specified shortcut keys.
        //
        // Parameters:
        //   text:
        //     The text to display on the menu item.
        //
        //   image:
        //     The System.Drawing.Image to display on the control.
        //
        //   onClick:
        //     An event handler that raises the System.Windows.Forms.Control.Click event
        //     when the control is clicked.
        //
        //   shortcutKeys:
        //     One of the values of System.Windows.Forms.Keys that represents the shortcut
        //     key for the System.Windows.Forms.MenuItem.
        public MenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick, Keys shortcutKeys)
            : base(text, image)
        {
            this.Click = onClick;
            this.ShortcutKeys = shortcutKeys;
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Windows.Forms.MenuItem
        //     class with the specified name that displays the specified text and image
        //     that does the specified action when the System.Windows.Forms.MenuItem
        //     is clicked.
        //
        // Parameters:
        //   text:
        //     The text to display on the menu item.
        //
        //   image:
        //     The System.Drawing.Image to display on the control.
        //
        //   onClick:
        //     An event handler that raises the System.Windows.Forms.Control.Click event
        //     when the control is clicked.
        //
        //   name:
        //     The name of the menu item.
        public MenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick, string name) : base(text, image)
        {
            this.Click = onClick;
            this.Name = name;
        }
    }
}
