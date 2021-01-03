/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace MindMate.Serialization
{
    public static class Convert
    {
        public static NodeShape ToNodeStyle(string style)
        {
            switch (style)
            {
                case "fork":
                    return NodeShape.Fork;
                case "bubble":
                    return NodeShape.Bubble;
                case "box":
                    return NodeShape.Box;
                case "bullet":
                    return NodeShape.Bullet;
                default:
                    return NodeShape.None;

            }
        }

        public static string ToString(NodeShape style)
        {
            switch (style)
            {
                case NodeShape.Box:
                    return "box";
                case NodeShape.Bubble:
                    return "bubble";
                case NodeShape.Fork:
                    return "fork";
                case NodeShape.Bullet:
                    return "bullet";
                default:
                    return "none";
            }
        }

        public static int ToLineWidth(string width)
        {
            switch (width)
            {
                case "thin":
                    return 1;
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "4":
                    return 4;
                case "8":
                    return 4;
                default:
                    return 1;
            }
        }

        public static DashStyle ToDashStyle(string pattern)
        {
            switch (pattern)
            {
                case "dash":
                    return System.Drawing.Drawing2D.DashStyle.Dash;
                case "dashdotdot":
                    return System.Drawing.Drawing2D.DashStyle.DashDotDot;
                case "dot":
                    return System.Drawing.Drawing2D.DashStyle.Dot;
                case "solid":
                    return System.Drawing.Drawing2D.DashStyle.Solid;
                default:
                    return System.Drawing.Drawing2D.DashStyle.Custom;
            }
        }

        public static string ToString(DashStyle linePattern)
        {
            return Enum.GetName(linePattern.GetType(), linePattern).ToLower();
        }

        public static NodePosition ToNodePosition(string pos)
        {
            switch (pos)
            {
                case "right":
                    return NodePosition.Right;
                case "left":
                    return NodePosition.Left;
                case "root":
                    return NodePosition.Root;
                default:
                    return NodePosition.Undefined;
            }
        }

        public static string ToString(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static Color ToColor(string color)
        {
            return (Color)(new ColorConverter().ConvertFromString(color));
        }

        public static bool ToBool(string boolean)
        {
            switch(boolean)
            {
                case "true":
                    return true;
                default:
                    return false;
            }
        }

        public static string ToString(bool boolean)
        {
            switch(boolean)
            {
                case true:
                    return "true";
                default:
                    return "false";
            }
        }
    }
}
