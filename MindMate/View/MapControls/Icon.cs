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
    public class Icon
    {
        public Icon(ModelIcon icon)
        {
            modelIcon = icon;
        }

        public Icon(string icon)
        {
            modelIcon = MetaModel.MetaModel.Instance.IconsHashMap[icon];
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
                return modelIcon.Bitmap.Size; 
            }            
        }

        ModelIcon modelIcon;

        public ModelIcon ModelIcon
        {
            get 
            { 
                return modelIcon; 
            }            
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(modelIcon.Bitmap, location.X, location.Y, Size.Width, Size.Height);
        }
    }
}
