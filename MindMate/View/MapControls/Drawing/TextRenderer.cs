/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

//#define TextRenderer
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.View.MapControls.Drawing
{

#if TextRenderer
    public static class TextRenderer
    {
        public static Size MeasureText(string text, Font font)
        {
            return System.Windows.Forms.TextRenderer.MeasureText(text, font, 
                new Size(NodeView.MAXIMUM_TEXT_WIDTH, 5000),
                System.Windows.Forms.TextFormatFlags.WordBreak);
        }

        public static void DrawText(Graphics g, string text, Font font, RectangleF rect, Color textColor)
        {
            System.Windows.Forms.TextRenderer.DrawText(g, text, font, Rectangle.Round(rect), textColor, Color.Transparent, System.Windows.Forms.TextFormatFlags.WordBreak);
            //System.Windows.Forms.TextRenderer.DrawText(g, text, font, new Point(rect.X, rect.Y), textColor);
        }
    }
#endif

#if !TextRenderer
    public static class TextRenderer
    {
        /// <summary>
        /// Only used for measuring text, not for drawing
        /// </summary>
        public static Graphics g;
        static TextRenderer()
        {
            Bitmap bmp = new Bitmap(1, 1);
            g = Graphics.FromImage(bmp);
        }
        public static SizeF MeasureText(string text, Font font)
        {
            return Size.Round(g.MeasureString(text, font, NodeView.MAXIMUM_TEXT_WIDTH));            
        }

        public static void DrawText(Graphics g, string text, Font font, RectangleF rect, Color textColor)
        {
            using(Brush brush = new SolidBrush(textColor))
            {
                g.DrawString(text, font, brush, rect);
            }
        }
    }
#endif
}
