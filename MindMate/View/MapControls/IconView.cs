/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MindMate.MetaModel;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// Can represent both System Icon and User Icon
    /// </summary>
    public class IconView
    {
        
        public IconView(string icon)
        {
            iconSpec = MetaModel.MetaModel.Instance.GetIcon(icon);
        }

        public IconView(IIcon iIcon)
        {
            iconSpec = iIcon;
        }

        PointF location;

        public PointF Location
        {
            get { return location; }
            set { location = value; }
        }

               
        public Size Size
        {
            get 
            { 
                return iconSpec.Bitmap.Size; 
            }            
        }

        public string Name
        {
            get
            {
                return iconSpec.Name;
            }
        }

        IIcon iconSpec;

        public IIcon IconSpec
        {
            get 
            { 
                return iconSpec; 
            }            
        }

        public void Draw(Graphics g)
        {
            g.DrawImageUnscaled(iconSpec.Bitmap, (int)location.X, (int)location.Y);
        }
    }
}
