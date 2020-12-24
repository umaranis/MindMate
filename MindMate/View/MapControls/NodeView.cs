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
    public enum SubControlType { None, Text, Link, Note, Icon, Image };

    /// <summary>
    /// Three functionalities of NodeView:
    /// 1- Creating NodeView controls, setting their sizes and size of NodeView 
    ///         (Basically done through CreateNodeViewContent. RefreshNodeViewSize is used for calculating NodeView size.)
    /// 2- Refreshing controls and their sizes and size of NodeView
    ///         (Refresh functions are provided for various controls and RefreshNodeViewSize for overall size of NodeView.)
    /// 3- Setting the positions of NodeView and its Controls
    ///         (Done through RefreshPosition.)
    /// </summary>
    public class NodeView
    {       

        public const int MAXIMUM_TEXT_WIDTH = 500;
        public const int LEFT_PADDING = 4;
        public const int RIGHT_PADDING = 3;
        public const int INTER_CONTROL_PADDING = 3;
        public const int TOP_PADDING = 2;
        public const int BOTTOM_PADDING = 2;
        public const int ICON_SIZE = 16;

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

        private NodeFormat nodeFomat;
        public NodeFormat NodeFormat => nodeFomat;
        
        public ImageView ImageView { get; private set; }
        

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
                return NodeFormat.Font.Height*2 <= recText.Height;
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

        public void RefreshFontAndFormat()
        {
            CreateNodeFormat();
            CreateTextRec();

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
            SizeF s = Drawing.TextRenderer.MeasureText(node.Text, NodeFormat.Font);

            RefreshText(s);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSize">Rather than calculating the Text size, given text size is used. 
        /// This is useful when text is being edited in the view.
        /// </param>
        public void RefreshText(SizeF textSize)
        {
           // if (textSize.Height == 0) //if empty text than set minimum size
           //     textSize = new SizeF(10, font.Height);

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

        public void RefreshImageView()
        {
            if (!node.HasImage)
                ImageView = null;
            else if (ImageView == null)
                CreateImageView();

            RefreshNodeViewSize();
        }

        private void CreateImageView()
        {
            if (node.HasImage)
            {
                ImageView = new ImageView(node);
            }
        }

        private void CreateNodeFormat()
        {
            nodeFomat = NodeFormat.CreateNodeFormat(node);
        }

        private void CreateTextRec()
        {
            //step 2: recalculate text rec size        
            SizeF s = Drawing.TextRenderer.MeasureText(node.Text, nodeFomat.Font);

            //if (s.Height == 0) //if empty text than set minimum size
            //    s = new SizeF(10, font.Height);

            recText.Width = s.Width;
            recText.Height = s.Height;
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

            CreateImageView();

            CreateNodeFormat();
            CreateTextRec();            

            RefreshNodeViewSize();

        }

        private void RefreshNodeViewSize()
        {
            //width------------------
            width = 0; 

            if (this.link != null)  width += (link.Size.Width + INTER_CONTROL_PADDING);
            
            if (this.noteIcon != null)  width += (noteIcon.Size.Width + INTER_CONTROL_PADDING);

            for (int i = 0; i < this.recIcons.Count; i++)   width += (this.recIcons[i].Size.Width + INTER_CONTROL_PADDING);

            if (!Node.HasImage)
            {
                width += (NodeView.LEFT_PADDING + recText.Width + NodeView.RIGHT_PADDING);
            }
            else
            {
                switch (Node.ImageAlignment)
                {
                    case ImageAlignment.Default:
                    case ImageAlignment.AboveStart:
                    case ImageAlignment.BelowStart:
                    case ImageAlignment.AboveCenter:
                    case ImageAlignment.BelowCenter:
                    case ImageAlignment.AboveEnd:
                    case ImageAlignment.BelowEnd:                        
                        if (recText.Width > ImageView.Size.Width)
                            width += (NodeView.LEFT_PADDING + recText.Width + NodeView.RIGHT_PADDING);
                        else
                            width += (NodeView.LEFT_PADDING + ImageView.Size.Width + NodeView.RIGHT_PADDING);
                        break;
                    case ImageAlignment.AfterTop:
                    case ImageAlignment.AfterCenter:
                    case ImageAlignment.AfterBottom:
                    case ImageAlignment.BeforeTop:
                    case ImageAlignment.BeforeCenter:
                    case ImageAlignment.BeforeBottom:
                        width += (NodeView.LEFT_PADDING + recText.Width + NodeView.INTER_CONTROL_PADDING +
                            ImageView.Size.Width + NodeView.RIGHT_PADDING);
                        break;
                }
            }
            //width------------------


            //height-----------------
            height = TOP_PADDING;
            if (node.HasImage)
            {
                switch(node.ImageAlignment)
                {
                    case ImageAlignment.Default:
                    case ImageAlignment.AboveStart:
                    case ImageAlignment.BelowStart:
                    case ImageAlignment.AboveCenter:
                    case ImageAlignment.BelowCenter:
                    case ImageAlignment.AboveEnd:
                    case ImageAlignment.BelowEnd:
                        if(ImageView.Size.Height + RecText.Size.Height < ICON_SIZE && (recIcons.Any() || link != null || noteIcon != null))
                        {
                            height += ICON_SIZE;
                        }
                        else
                        {
                            height += ImageView.Size.Height + INTER_CONTROL_PADDING + RecText.Height;
                        }
                        break;
                    case ImageAlignment.AfterTop:
                    case ImageAlignment.AfterCenter:
                    case ImageAlignment.AfterBottom:
                    case ImageAlignment.BeforeTop:
                    case ImageAlignment.BeforeCenter:
                    case ImageAlignment.BeforeBottom:
                        height += Math.Max(ImageView.Size.Height, Math.Max(RecText.Height, ICON_SIZE));
                        break;
                }
            }
            else
            {
                height += Math.Max(recText.Height, ICON_SIZE);                
            }
            height += BOTTOM_PADDING;
            //height-----------------
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
            float drawIconCursorTop = top + ((height - TOP_PADDING - BOTTOM_PADDING) / 2) - (ICON_SIZE / 2) + TOP_PADDING;
            if (node.Pos != NodePosition.Left) //right or root node
            {
                if (link != null)
                {
                    link.Location = new PointF(drawCursorLeft, drawIconCursorTop);
                    drawCursorLeft += link.Size.Width;
                    drawCursorLeft += INTER_CONTROL_PADDING;
                }
                if (noteIcon != null)
                {
                    noteIcon.Location = new PointF(drawCursorLeft, drawIconCursorTop);
                    drawCursorLeft += noteIcon.Size.Width;
                    drawCursorLeft += INTER_CONTROL_PADDING;
                }
                for (int i = 0; i < recIcons.Count; i++)
                {
                    recIcons[i].Location = new PointF(drawCursorLeft, drawIconCursorTop);
                    drawCursorLeft += recIcons[i].Size.Width;
                    drawCursorLeft += INTER_CONTROL_PADDING;
                }
                if (node.HasImage)
                {
                    switch (node.ImageAlignment)
                    {
                        case ImageAlignment.Default:
                        case ImageAlignment.AboveStart:
                            ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                            recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
                            break;
                        case ImageAlignment.BelowStart:
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
							break;
						case ImageAlignment.AboveCenter:
							if (ImageView.Size.Width > RecText.Width)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								recText.Location = new PointF(drawCursorLeft + ImageView.Size.Width / 2 - RecText.Width / 2, 
									top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft + RecText.Width / 2 - ImageView.Size.Width / 2, top + TOP_PADDING);
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
							}
							break;
						case ImageAlignment.BelowCenter:
							if (ImageView.Size.Width > RecText.Width)
							{
								recText.Location = new PointF(drawCursorLeft + ImageView.Size.Width / 2 - RecText.Width / 2, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);								
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft + RecText.Width / 2 - ImageView.Size.Width / 2,
									top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
							}
							break;
						case ImageAlignment.AboveEnd:
							if (ImageView.Size.Width > RecText.Width)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								recText.Location = new PointF(drawCursorLeft + ImageView.Size.Width - RecText.Width,
									top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft + RecText.Width - ImageView.Size.Width, top + TOP_PADDING);
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
							}
							break;
						case ImageAlignment.BelowEnd:
							if (ImageView.Size.Width > RecText.Width)
							{
								recText.Location = new PointF(drawCursorLeft + ImageView.Size.Width - RecText.Width, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft + RecText.Width - ImageView.Size.Width,
									top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
							}
							break;
						case ImageAlignment.AfterTop:
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							break;
						case ImageAlignment.AfterCenter:
							if (ImageView.Size.Height > RecText.Height)
							{
								recText.Location = new PointF(drawCursorLeft, 
									top + TOP_PADDING + ImageView.Size.Height / 2 - RecText.Height / 2);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft, 
									top + TOP_PADDING + RecText.Height / 2 - ImageView.Size.Height / 2);

							}
							break;
						case ImageAlignment.AfterBottom:
							if (ImageView.Size.Height > RecText.Height)
							{
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height - RecText.Height);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height - ImageView.Size.Height);

							}
							break;
						case ImageAlignment.BeforeTop:
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING); 
							break;
						case ImageAlignment.BeforeCenter:
							if (ImageView.Size.Height > RecText.Height)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height / 2 - RecText.Height / 2);							
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height / 2 - ImageView.Size.Height / 2);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);					
							}
							break;
						case ImageAlignment.BeforeBottom:
							if (ImageView.Size.Height > RecText.Height)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height - RecText.Height);
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height - ImageView.Size.Height);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							}
							break;
					}
                }
                else
                {
                    recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                }

                
            }
            else // left node
            {
                if (node.HasImage)
                {
                    switch (node.ImageAlignment)
                    {
                        case ImageAlignment.Default:
                        case ImageAlignment.AboveStart:
                            if (ImageView.Size.Width > RecText.Width)
                            {
                                ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                                recText.Location = new PointF(drawCursorLeft + (ImageView.Size.Width - RecText.Size.Width), top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
                                drawCursorLeft += ImageView.Size.Width;
                            }
                            else
                            {
                                ImageView.Location = new PointF(drawCursorLeft + (RecText.Size.Width - ImageView.Size.Width), top + INTER_CONTROL_PADDING);
                                recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += RecText.Width;
                            }                            
                            break;
						case ImageAlignment.BelowStart:
							if (ImageView.Size.Width > RecText.Width)
							{
								recText.Location = new PointF(drawCursorLeft + (ImageView.Size.Width - RecText.Size.Width), top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += ImageView.Size.Width;
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft + (RecText.Size.Width - ImageView.Size.Width), top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += RecText.Size.Width;
							}
							break;
						case ImageAlignment.AboveCenter:
							if (ImageView.Size.Width > RecText.Width)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								recText.Location = new PointF(drawCursorLeft + ImageView.Size.Width / 2 - RecText.Width / 2,
									top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += ImageView.Size.Width;
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft + RecText.Width / 2 - ImageView.Size.Width / 2, top + TOP_PADDING);
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += RecText.Width;						
							}
							break;
						case ImageAlignment.BelowCenter:
							if (ImageView.Size.Width > RecText.Width)
							{
								recText.Location = new PointF(drawCursorLeft + ImageView.Size.Width / 2 - RecText.Width / 2, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += ImageView.Size.Width;
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								ImageView.Location = new PointF(drawCursorLeft + RecText.Width / 2 - ImageView.Size.Width / 2,
									top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
								drawCursorLeft += RecText.Width;
							}
							break;
						case ImageAlignment.AboveEnd:
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING + ImageView.Size.Height + INTER_CONTROL_PADDING);
							drawCursorLeft += (ImageView.Size.Width > RecText.Width)? ImageView.Size.Width : RecText.Width;
							break;
						case ImageAlignment.BelowEnd:
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING + RecText.Height + INTER_CONTROL_PADDING);
							drawCursorLeft += (ImageView.Size.Width > RecText.Width) ? ImageView.Size.Width : RecText.Width;
							break;
						case ImageAlignment.AfterTop:
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							drawCursorLeft += RecText.Width;
							break;
						case ImageAlignment.AfterCenter:
							if (ImageView.Size.Height > RecText.Height)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height / 2 - RecText.Height / 2);
								drawCursorLeft += RecText.Width;
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height / 2 - ImageView.Size.Height / 2);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += RecText.Width;
							}
							break;
						case ImageAlignment.AfterBottom:
							if (ImageView.Size.Height > RecText.Height)
							{
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height - RecText.Height);
								drawCursorLeft += recText.Width;
							}
							else
							{
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height - ImageView.Size.Height);
								drawCursorLeft += ImageView.Size.Width + INTER_CONTROL_PADDING;
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += recText.Width;
							}
							break;
						case ImageAlignment.BeforeTop:
							recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
							ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
							drawCursorLeft += ImageView.Size.Width;
							break;
						case ImageAlignment.BeforeCenter:
							if (ImageView.Size.Height > RecText.Height)
							{
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height / 2 - RecText.Height / 2);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += ImageView.Size.Width;
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height / 2 - ImageView.Size.Height / 2);
								drawCursorLeft += ImageView.Size.Width;

							}
							break;
						case ImageAlignment.BeforeBottom:
							if (ImageView.Size.Height > RecText.Height)
							{
								recText.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + ImageView.Size.Height - RecText.Height);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += ImageView.Size.Width;
							}
							else
							{
								recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
								drawCursorLeft += RecText.Width + INTER_CONTROL_PADDING;
								ImageView.Location = new PointF(drawCursorLeft,
									top + TOP_PADDING + RecText.Height - ImageView.Size.Height);
								drawCursorLeft += ImageView.Size.Width;

							}
							break;
					}
                }
                else
                {
                    recText.Location = new PointF(drawCursorLeft, top + TOP_PADDING);
                    drawCursorLeft += recText.Width;
                }
                
                for (int i = recIcons.Count - 1; i >= 0; i--)
                {
                    drawCursorLeft += INTER_CONTROL_PADDING;
                    recIcons[i].Location = new PointF(drawCursorLeft, drawIconCursorTop);
                    drawCursorLeft += recIcons[i].Size.Width;
                }
                if (noteIcon != null)
                {
                    drawCursorLeft += INTER_CONTROL_PADDING;
                    noteIcon.Location = new PointF(drawCursorLeft, drawIconCursorTop);
                    drawCursorLeft += noteIcon.Size.Width;
                }
                if (link != null)
                {
                    drawCursorLeft += INTER_CONTROL_PADDING;
                    link.Location = new PointF(drawCursorLeft, drawIconCursorTop);
                    drawCursorLeft += link.Size.Width;
                }
            }
                    
            
        }

        public bool IsPointInsideNode(Point p)
        {
            //Debug.WriteLine("IsPointInsideNode for " + node.Pos.ToString() + node.Text);
            return
                p.X > Left && p.X < Right &&
                p.Y > Top && p.Y < Bottom;
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

        public KeyValuePair<SubControlType, object> GetSubControl(Point point)
        {
            if(RecText.Contains(point))
            {
                return new KeyValuePair<SubControlType, object>(SubControlType.Text, node.Text);
            }
            if (node.HasImage && ImageView.Contains(point))
            {
                return new KeyValuePair<SubControlType, object>(SubControlType.Image, node.Image);
            }
            if(node.HasLink && Link.Contains(point))
            {
                return new KeyValuePair<SubControlType, object>(SubControlType.Link, node.Link);
            }
            if(node.HasNoteText && NoteIcon.Contains(point))
            {
                return new KeyValuePair<SubControlType, object>(SubControlType.Note, node.NoteText);
            }
            foreach(var i in RecIcons)
            {
                if (i.Contains(point)) return new KeyValuePair<SubControlType, object>(SubControlType.Icon, i.Name);
            }
            return new KeyValuePair<SubControlType, object>(SubControlType.None, null);
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
