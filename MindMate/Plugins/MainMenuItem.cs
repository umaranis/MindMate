using System;
using System.Collections.Generic;

namespace MindMate.Plugins
{
    public class MainMenuItem
    {
        public MainMenuItem(string text)
        {
            DropDownItems = new List<MainMenuItem>();
            Text = text;
        }

        public string Text { get; set; }

        public MainMenuLocation MainMenuLocation { get; set; }

        public EventHandler Click { get; set; }

        /// <summary>
        /// If true, a separator is placed after this Menu Item
        /// </summary>
        public bool AddSeparator { get; set; }
        
        public List<MainMenuItem> DropDownItems { get; private set; }

        internal void AddDropDownItem(MainMenuItem item)
        {
            DropDownItems.Add(item);
        }
    }
}
