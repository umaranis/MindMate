/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MindMate.Model
{
    public partial class MapNode { 
    public class IconList : Collection<string>
    {
        private MapNode node;

        public IconList(MapNode node)
        {
            this.node = node;
        }

        protected sealed override void InsertItem(int index, string item)
        {
            base.InsertItem(index, item);
            node.modified = DateTime.Now;
            node.Tree.FireEvent(node, IconChange.Added, item);
        }

        protected sealed override void ClearItems()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.RemoveItem(i);
            }                
        }

        protected sealed override void RemoveItem(int index)
        {
            string oldValue = this[index];
            base.RemoveItem(index);
            node.modified = DateTime.Now;
            node.Tree.FireEvent(node, IconChange.Removed, oldValue);
        }

        protected sealed override void SetItem(int index, string item)
        {
            base.SetItem(index, item);
            node.modified = DateTime.Now;
            node.Tree.FireEvent(node, IconChange.Added, item);
        }

                
        public bool RemoveLast()
        {
            if (this.Count > 0)
            {
                this.RemoveAt(this.Count - 1);
                return true;                
            }
            else
            {
                return false;
            }
        }
    }

    }
}
