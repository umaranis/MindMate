﻿/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System.Drawing;

namespace MindMate.View.MapControls
{
    public class NoteIcon
    {
        public NoteIcon()
        {
            bitmap = MindMate.Properties.Resources.sticky_note_pin;
            
        }

        PointF location;

        public PointF Location
        {
            get => location;
            set => location = value;
        }


        public Size Size => bitmap.Size;

        public bool Contains(Point p)
        {
            return p.X >= Location.X && p.Y >= Location.Y && p.X <= Location.X + Size.Width && p.Y <= Location.Y + Size.Height;
        }

        readonly Bitmap bitmap;

        public Bitmap Bitmap => bitmap;

        public void Draw(Graphics g)
        {
            g.DrawImage(bitmap, location.X, location.Y, Size.Width, Size.Height);
        }
    }
}
