using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins
{
    public class MainMenuItem : MenuItem
    {
        public MainMenuLocation MainMenuLocation { get; set; }

        public MainMenuItem() : base()
        {
        }


        public MainMenuItem(Image image) : base(image)
        {
        }


        public MainMenuItem(string text) : base(text)
        {
        }


        public MainMenuItem(string text, Image image) : base(text, image)
        {
        }


        public MainMenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick)
            : base(text, image, onClick)
        {
        }


        public MainMenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick, Keys shortcutKeys)
            : base(text, image, onClick, shortcutKeys)
        {
        }


        public MainMenuItem(string text, Image image, Action<MenuItem, SelectedNodes> onClick, string name)
            : base (text, image, onClick, name)
        {
        }

    }
}
