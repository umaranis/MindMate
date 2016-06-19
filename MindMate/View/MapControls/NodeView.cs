/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MindMate.Model;
using MindMate.Controller;
using MindMate.MetaModel;

namespace MindMate.View.MapControls
{
    public enum NodePortion { Body, Head };

    public class NodeView
    {       

        public const int MAXIMUM_TEXT_WIDTH = 500;
        public const int LEFT_PADDING = 4;
        public const int RIGHT_PADDING = 3;
        public const int INTER_CONTROL_PADDING = 3;
        public const int TOP_PADDING = 2;
        public const int BOTTOM_PADDING = 1;

        public static Font DefaultFont = new Font(FontFamily.GenericSansSerif, 10);
        public static Color DefaultLineColor = Color.Gray;
        public static Color DefaultTextColor = Color.Black;

        private MapNode node;
        public MapNode Node {get{ return node; }}

        private Link link;
        public Link Link
        {
            get { return link; }            
        }

        private NoteIcon noteIcon;
        /// <summary>
        /// Gets NoteIcon
        /// </summary>
        public NoteIcon NoteIcon {  get { return noteIcon; }    }

        private RectangleF recText;
        /// <summary>
        /// Gets the rectangle for the node text
        /// </summary> 
        public RectangleF RecText {     get { return recText; } }

        private List<IconView> recIcons = new List<IconView>(); 
        public List<IconView> RecIcons  {   get { return recIcons; }    }

        private Font font;
        public Font Font {  get { return font; }    }


        public Color TextColor  {   
            get 
            {
                if (node.Color.IsEmpty)
                    return DefaultTextColor;
                else
                    return node.Color;
            }   
        }
        

        public Color BackColor
        {
            get
            {
                return node.BackColor;
            }
        }
        

        public NodeView(MapNode node, PointF location)
        {
            this.node = node;
            this.left = location.X;
            this.top = location.Y;

            CreateNodeViewContent();
        }

        public NodeView(MapNode node)
            : this(node, new PointF(0f, 0f))
        {

        }

        public bool Selected
        {
            get 
            {
                return node.Tree.SelectedNodes.Contains(node);
            }
        }

        private float left;

        public float Left
        {
            get { return left; }            
        }

        private float top;

        public float Top
        {
            get { return top; }            
        }

        private float height;

        /// <summary>
        /// Height of the node (without considering child nodes)
        /// </summary>
        public float Height
        {
            get { return height; }            
        }

        private float width;

        /// <summary>
        /// Width of the node (without considering child nodes)
        /// </summary>
        public float Width
        {
            get { return width; }            
        }

        public float Bottom
        {
            get { return top + height; }
        }

        public float Right
        {
            get { return left + width; }
        }

        public bool IsMultiline
        {
            get
            {
                return font.Height*2 <= recText.Height ? true : false;
            }
        }

        /// <summary>
        /// Returns last selected child node or the first child
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public MapNode GetLastSelectedChild(NodePosition pos)
        {
            MapNode resultNode = node.LastSelectedChild;

            if (resultNode == null || resultNode.Pos != pos)
                resultNode = this.node.GetFirstChild(pos);

            return resultNode;
        }


        
        /// <summary>
        /// Clears and re-creates the node view (node view contents and sizes). 
        /// Node position should be updated separately using TreeView.RefreshChildNodePositions or NodeView.RefreshPosition (used for parent node).
        /// </summary>
        /// <param name="location"></param>
        //public void RefreshNodeView()
        //{
        //    //TOOD: Rather than using this method directly update the state of NodeView. Search for all occurances and replace code.
        //    recIcons.Clear();
        //    width = 0;
        //    height = 0;
        //    link = null;

        //    CreateNodeViewContent();
        //}

        public void RefreshFont()
        {
            if(font != null && font != DefaultFont) font.Dispose();

            //step 1: refresh font from MapNode
            FontStyle style = FontStyle.Regular;
            if (node.Bold)  style |= FontStyle.Bold;
            if (node.Italic)    style |= FontStyle.Italic;
            if (node.Strikeout)  style |= FontStyle.Strikeout;
            
            if (style == FontStyle.Regular && node.FontName == null && node.FontSize == 0) //set the default font
            {
                font = DefaultFont;
            }
            else 
            {
                
                FontFamily ff;
                try
                {
                    ff = node.FontName != null ? new FontFamily(node.FontName) : DefaultFont.FontFamily;
                }
                catch (ArgumentException)
                {
                    ff = DefaultFont.FontFamily;
                    System.Diagnostics.Trace.TraceError("Specified Font Name (" + node.FontName + ") not found. Using default font instead.");
                }
                
                font = new Font(
                    ff, node.FontSize != 0 ? node.FontSize : DefaultFont.Size,
                    style);               
                
            }
            
            //step 2: recalculate text rec size        
            SizeF s = Drawing.TextRenderer.MeasureText(node.Text, font);

            if (s.Height == 0) //if empty text than set minimum size
                s = new SizeF(10, font.Height);

            recText.Width = s.Width; 
            recText.Height = s.Height;

            //step 3: recalculate NodeView size
            this.RefreshNodeViewSize();
        }

        public void RefreshLink()
        {
            if (node.Link == null)
                this.link = null;
            else
            {
                if (this.link == null)
                    this.link = new Link(node.Link, node.GetLinkType());
                else
                    this.link.ChangeLink(node.Link, node.GetLinkType());
            }

            this.RefreshNodeViewSize();            
        }

        //public void RefreshIcons()
        //{
        //    this.recIcons.Clear();
        //    for (int i = 0; i < node.Icons.Count; i++)
        //    {
        //        IconView icon = new IconView(node.Icons[i]);
        //        this.recIcons.Add(icon);                
        //    }

        //    this.RefreshNodeViewSize();
        //}

        public void AddIcon(string icon)
        {
            this.recIcons.Add(new IconView(icon));

            this.RefreshNodeViewSize();
        }

        public void AddIcon(ISystemIcon iconSpec)
        {
            recIcons.Insert(0, new IconView(iconSpec));

            this.RefreshNodeViewSize();
        }

        public void RemoveIcon(string icon)
        {
            for(int i = 0; i < recIcons.Count; i++)
            {
                if(recIcons[i].IconSpec.Name == icon)
                {
                    recIcons.RemoveAt(i);
                    RefreshNodeViewSize();
                    return;
                }
            }
        }

        public void RemoveIcon(ISystemIcon iconSpec)
        {
            for (int i = 0; i < recIcons.Count; i++)
            {
                if (recIcons[i].IconSpec == iconSpec)
                {
                    recIcons.RemoveAt(i);
                    RefreshNodeViewSize();
                    return;
                }
            }
        }

        public void RefreshText()
        {
            //SizeF s = MapView.graphics.MeasureString(node.Text, font, NodeView.MAXIMUM_TEXT_WIDTH);
            SizeF s = Drawing.TextRenderer.MeasureText(node.Text, font);

            RefreshText(s);
        }

        public void RefreshText(SizeF textSize)
        {
            if (textSize.Height == 0) //if empty text than set minimum size
                textSize = new SizeF(10, font.Height);

            recText.Width = textSize.Width;
            recText.Height = textSize.Height;

            this.RefreshNodeViewSize();
        }



        public void RefreshNoteIcon()
        {
            if(node.HasNoteText && this.noteIcon == null)
            {
                this.noteIcon = new NoteIcon();
                this.RefreshNodeViewSize();
            }
            else if(!node.HasNoteText && this.noteIcon != null)
            {
                this.noteIcon = null;
                this.RefreshNodeViewSize();
            }
        }


        private void CreateNodeViewContent()
        {
            if (node.Link != null)
            {
                this.link = new Link(node.Link, node.GetLinkType());
                width += (link.Size.Width + INTER_CONTROL_PADDING);
            }

            if (node.HasNoteText)
            {
                this.noteIcon = new NoteIcon();
                width += (noteIcon.Size.Width + INTER_CONTROL_PADDING);
            }

            for (int i = 0; i < MetaModel.MetaModel.Instance.SystemIconList.Count; i++)
            {
                var systemIcon = MetaModel.MetaModel.Instance.SystemIconList[i];
                if(systemIcon.ShowIcon(node))
                {
                    recIcons.Add(new IconView(systemIcon));
                }
            }

            for (int i = 0; i < node.Icons.Count; i++)
            {
                IconView icon = new IconView(node.Icons[i]);
                this.recIcons.Add(icon);
                width += (icon.Size.Width + INTER_CONTROL_PADDING);
            }

            RefreshFont();


        }

        private void RefreshNodeViewSize()
        {
            width = 0; //height = 0;

            if (this.link != null)  width += (link.Size.Width + INTER_CONTROL_PADDING);
            
            if (this.noteIcon != null)  width += (noteIcon.Size.Width + INTER_CONTROL_PADDING);

            for (int i = 0; i < this.recIcons.Count; i++)   width += (this.recIcons[i].Size.Width + INTER_CONTROL_PADDING);

            width += (NodeView.LEFT_PADDING + recText.Width + NodeView.RIGHT_PADDING);

            height = TOP_PADDING + recText.Height + BOTTOM_PADDING;
        }

        /// <summary>
        /// Updates the position of node view and its contents
        /// </summary>
        /// <param name="location"></param>
        public void RefreshPosition(float x, float y)
        {
            left = x;
            top = y;

            float drawCursorLeft = left + NodeView.LEFT_PADDING;
            if (node.Pos != NodePosition.Left) //right or root node
            {
                if (link != null)
                {
                    link.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += link.Size.Width;
                    drawCursorLeft += INTER_CONTROL_PADDING;
                }
                if (noteIcon != null)
                {
                    noteIcon.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += noteIcon.Size.Width;
                    drawCursorLeft += INTER_CONTROL_PADDING;
                }
                for (int i = 0; i < recIcons.Count; i++)
                {
                    recIcons[i].Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += recIcons[i].Size.Width;
                    drawCursorLeft += INTER_CONTROL_PADDING;
                }
                recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);

                
            }
            else // left node
            {

                recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                drawCursorLeft += recText.Width;
                for (int i = recIcons.Count - 1; i >= 0; i--)
                {
                    drawCursorLeft += INTER_CONTROL_PADDING;
                    recIcons[i].Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += recIcons[i].Size.Width;
                }
                if (noteIcon != null)
                {
                    drawCursorLeft += INTER_CONTROL_PADDING;
                    noteIcon.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += noteIcon.Size.Width;
                }
                if (link != null)
                {
                    drawCursorLeft += INTER_CONTROL_PADDING;
                    link.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += link.Size.Width;
                }
            }
                    
            
        }

        public NodePortion GetNodeClickPortion(Point point)
        {
            if (node.Pos == NodePosition.Left)
            {
                if (point.X > node.NodeView.Left + (node.NodeView.Width * 6 / 10))
                    return NodePortion.Head;
                else
                    return NodePortion.Body;
            }
            else
            {
                if (point.X > node.NodeView.Left + (node.NodeView.Width * 4 / 10))
                    return NodePortion.Body;
                else
                    return NodePortion.Head;
            }
        }

        public void FollowLink()
        {
            if (Link.LinkType == NodeLinkType.MindMapNode)
            {
                //handle
            }
            else if (Link.LinkType != NodeLinkType.Empty)
            {
                System.Diagnostics.Process.Start(node.Link);                
            }
        }

    }
}
