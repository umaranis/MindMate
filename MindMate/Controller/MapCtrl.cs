/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using MindMate.Serialization;
using MindMate.MetaModel;
using MindMate.Properties;
using System.IO;
using System.Diagnostics;
using MindMate.View.MapControls;
using MindMate.View;
using MindMate.Model;
using MindMate.View.Dialogs;


namespace MindMate.Controller
{
    /// <summary>
    /// Map Controller.
    /// Should not handle view events directly, rather through routers [MapViewEventHandler (Key & Mouse)]
    /// Controls MapView. Should not have knowledge about other UI controls like NoteEditor, StatusBar etc.
    /// </summary>
    public class MapCtrl
    {
        private IMainCtrl mainCtrl;

        public MapViewMouseEventHandler map;
        
        public MindMate.View.MapControls.MapView MapView;
        
        public string MindMateFile;
        
        private MapTree tree = null;

        public MapCtrl(MapTree tree, IMainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            map = new MapViewMouseEventHandler(this);
            this.tree = tree;
                       
            MapView = new MindMate.View.MapControls.MapView(tree);
            MapView.Canvas.NodeClick += map.MapNodeClick;
            MapView.Canvas.CanvasClick += map.CanvasClick;
            MapView.Canvas.NodeMouseOver += map.MapNodeMouseOver;
            MapView.Canvas.NodeMouseExit += map.NodeMouseExit;
            MapView.Canvas.KeyDown += new MapViewKeyEventHandler(this).canvasKeyDown;
            
            MapView.NodeTextEditor.Enable(this.UpdateNodeText);

            mainCtrl.AddMainPanel(this.MapView.Canvas);

            //center Canvas
            const int w = 4096;
            const int h = 4096;

            this.MapView.Canvas.Width = w;
            this.MapView.Canvas.Height = h;

            this.MapView.Canvas.Left = (int)((this.MapView.Canvas.Parent.Width - w) / 2);
            this.MapView.Canvas.Top = 0 - (h / 2) + 300;
                        
            MapView.RefreshNodePositions();           
            

        }

        public void ChangeTree(MapTree tree)
        {
            this.tree = tree;
            this.MapView.tree = tree;
            MapView.RefreshNodePositions();
            MapView.SelectedNodes.Add(tree.RootNode, false);
            this.MapView.Canvas.Invalidate();
        }              


        public void EditHyperlink(bool useFileDialog)
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = MapView.SelectedNodes.First;
                if (useFileDialog)
                {
                    OpenFileDialog file = new OpenFileDialog();
                    file.FileName = node.Link;
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        node.Link = file.FileName;
                        node.NodeView.RefreshLink();
                        if (node == tree.RootNode)
                        {
                            node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                            MapView.RefreshChildNodePositions(node, NodePosition.Undefined);
                        }
                        else
                        {
                            MapView.RefreshChildNodePositions(node.Parent, NodePosition.Undefined);
                        }

                        MapView.Canvas.Invalidate();
                    }
                }
                else
                {
                    MindMate.View.Dialogs.LinkManualEdit frm = new MindMate.View.Dialogs.LinkManualEdit();
                    frm.LinkText = node.Link;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        node.Link = frm.LinkText == ""? null : frm.LinkText;
                        //node.NodeView.RefreshNodeView();
                        node.NodeView.RefreshLink();
                        if (node == tree.RootNode)
                        {
                            node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                            MapView.RefreshChildNodePositions(node, NodePosition.Undefined);
                        }
                        else
                        {
                            MapView.RefreshChildNodePositions(node.Parent, NodePosition.Undefined);
                        }
                        MapView.Canvas.Invalidate();
                    }
                }
            }
        }

                           

        #region [Methods] Node Editing

        public void BeginCurrentNodeEdit(TextCursorPosition org)
        {
            if (this.MapView.SelectedNodes.Count != 1) return;

            MapNode node = this.MapView.SelectedNodes.First;            

            this.BeginNodeEdit(node, org);
        }
        
        public void BeginNodeEdit(MapNode node, TextCursorPosition org)
        {
            if (node.NodeView.IsMultiline)
                MultiLineNodeEdit(node, org);
            else
                MapView.NodeTextEditor.BeginNodeEdit(node.NodeView, org);
        }

        public void MultiLineNodeEdit()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = MapView.SelectedNodes.First;

                MultiLineNodeEdit(node, TextCursorPosition.End);
            }
        }

        public void MultiLineNodeEdit(MapNode node, TextCursorPosition org)
        {

            MindMate.View.Dialogs.MultiLineNodeEdit frm = new MindMate.View.Dialogs.MultiLineNodeEdit();
            frm.txt.Text = node.Text;
             if (org == TextCursorPosition.End || org == TextCursorPosition.Undefined)
            {
                frm.txt.SelectionStart = node.Text.Length;                
            }
            else if (org == TextCursorPosition.Start)
            {
                frm.txt.SelectionStart = 0;                
            }
            frm.txt.SelectionLength = 0;
            frm.txt.Focus();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                node.Text = frm.txt.Text;
                node.NodeView.RefreshText();
                if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                MapView.RefreshChildNodePositions(tree.RootNode, node.Pos);
                MapView.Canvas.Invalidate();
            }

        }

        public void UpdateNodeText(MapNode node, string newText)
        {
            node.Text = newText;
            //node.NodeView.RefreshNodeView();
            node.NodeView.RefreshText();
            if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
            MapView.RefreshChildNodePositions(tree.RootNode, node.Pos);
            MapView.Canvas.Invalidate();


        }

        

        #endregion
               
        public void appendNodeAndEdit()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                if (MapView.SelectedNodes.First.Pos == NodePosition.Root)
                {
                    appendChildNodeAndEdit();
                }
                else
                {
                    appendSiblingNodeAndEdit();
                }
            }
        }

        public void appendChildNodeAndEdit()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                MapNode tmpNode = this.appendChildNode(node); 
                if (tmpNode != null)
                {
                    this.BeginNodeEdit(tmpNode, TextCursorPosition.Undefined);
                }
            }
        }

        public MapNode appendChildNode(MapNode parent)
        {
            MapNode tmpNode = new MapNode(parent, "");
            if (tmpNode != null)
            {
                if (parent.Folded)
                {
                    this.ToggleNode(parent);
                }

                this.MapView.SelectedNodes.Add(tmpNode, false);
                MapView.RefreshChildNodePositions(tree.RootNode, tmpNode.Pos);                
                this.MapView.Canvas.Invalidate();
            }
            return tmpNode;
        }

        public void appendSiblingNodeAndEdit()
        {        
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                MapNode tmpNode = this.appendSiblingNode(node);
                if (tmpNode != null)
                {
                    this.BeginNodeEdit(tmpNode, TextCursorPosition.Undefined);
                }
            }
        }

        public MapNode appendSiblingNode(MapNode node)
        {
            if (node.Pos == NodePosition.Root)
            {
                return null;
            }
            MapNode tmpNode = new MapNode(node.Parent, "", NodePosition.Undefined, null, node);
            
            this.MapView.RefreshChildNodePositions(tree.RootNode, tmpNode.Pos);
            
            this.MapView.SelectedNodes.Add(tmpNode, false);

            this.MapView.Canvas.Invalidate();

            return tmpNode;
        }

                      

        public void DeleteSelectedNodes()
        {
            bool isDeleted = false;

            if (this.MapView.SelectedNodes.Last == null || this.MapView.SelectedNodes.Contains(tree.RootNode)) return;

            if (!mainCtrl.SeekDeleteConfirmation("Do you really want to delete selected node(s)?")) return;

            var selNode = this.MapView.getNearestUnSelectedNode(MapView.SelectedNodes.Last);

            for (var i = this.MapView.SelectedNodes.Count - 1; i >= 0; i--)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                this.MapView.SelectedNodes.Remove(node);

                if (node == null)
                {
                    continue;
                }

                node.DeleteNode();
                
                isDeleted = true;
                
            }


            if (isDeleted == true)
            {
                MapView.RefreshChildNodePositions(tree.RootNode, NodePosition.Undefined);
                this.MapView.SelectedNodes.Add(selNode, false);
                MapView.Canvas.Invalidate();
            }
        }
               

        public void MoveNodeUp()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                if (node.MoveUp())
                {
                    this.MapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);
                    MapView.Canvas.Invalidate();
                }
            }
        }

        public void MoveNodeDown()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                if (node.MoveDown())
                {
                    this.MapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);
                    MapView.Canvas.Invalidate();
                }
            }
        }

        public void SelectAllSiblingsAbove()
        {
            MapNode node = MapView.SelectedNodes.Last;
            if (node != null)
            {
                while (node.Previous != null)
                {
                    MapView.SelectedNodes.Add(node.Previous, true);
                    node = node.Previous;
                }
                MapView.Canvas.Invalidate();
            }            
        }

        public void SelectAllSiblingsBelow()
        {
            MapNode node = MapView.SelectedNodes.Last;
            if(node != null)
            {
                while (node.Next != null)
                {
                    MapView.SelectedNodes.Add(node.Next, true);
                    node = node.Next;
                }
                MapView.Canvas.Invalidate();
            }
        }

        public void SelectTopSibling()
        {
            if (MapView.SelectedNodes.Count > 0)
            {
                this.MapView.SelectedNodes.Add(MapView.SelectedNodes.Last.GetFirstSib(), false);
            }
            MapView.Canvas.Invalidate();
        }

        public void SelectBottomSibling()
        {
            if(MapView.SelectedNodes.Count > 0)
            {
                this.MapView.SelectedNodes.Add(MapView.SelectedNodes.Last.GetLastSib(), false);
            }
            MapView.Canvas.Invalidate();
        }

        /// <summary>
        /// Select the node above the currently selected node.
        /// If expandSelection is true, then adds the node above to the list of currently selected nodes.
        /// If expandSelection is true and node above is already selected, then 'deselects' the current node.
        /// </summary>
        /// <param name="expandSelection"></param>
        public void SelectNodeAbove(bool expandSelection = false)
        {
            MapNode node = MapView.SelectedNodes.Last;
            if (node == null || node.Parent == null) return;
            
            if (node.Previous != null)
            {
                if (!MapView.SelectedNodes.Contains(node.Previous))
                {
                    this.MapView.SelectedNodes.Add(node.Previous, expandSelection);
                }
                else if(expandSelection)
                {
                    MapView.SelectedNodes.Remove(node);
                }
            }
            else
            {
                if (node.Parent.Previous != null && node.Parent.Previous.LastChild  != null && !node.Parent.Previous.Folded)
                {
                    if (!MapView.SelectedNodes.Contains(node.Parent.Previous.LastChild))
                    {
                        this.MapView.SelectedNodes.Add(node.Parent.Previous.LastChild, expandSelection);
                    }
                    else if(expandSelection)
                    {
                        MapView.SelectedNodes.Remove(node);
                    }
                }
            }
            MapView.Canvas.Invalidate();            
        }

        /// <summary>
        /// Select the node below the currently selected node.
        /// If expandSelection is true, then adds the node below to the list of currently selected nodes.
        /// If expandSelection is true and node below is already selected, then 'deselects' the current node.
        /// </summary>
        public void SelectNodeBelow(bool expandSelection = false)
        {
            MapNode node = MapView.SelectedNodes.Last;
            if (node == null || node.Parent == null) return;

            if (node.Next != null)
            {
                if (!MapView.SelectedNodes.Contains(node.Next))
                {
                    this.MapView.SelectedNodes.Add(node.Next, expandSelection);
                }
                else if(expandSelection)
                {
                    MapView.SelectedNodes.Remove(node);
                }
            }
            else
            {
                if (node.Parent.Next != null && node.Parent.Next.FirstChild != null &&!node.Parent.Next.Folded)
                {
                    if (!MapView.SelectedNodes.Contains(node.Parent.Next.FirstChild))
                    {
                        this.MapView.SelectedNodes.Add(node.Parent.Next.FirstChild, expandSelection);
                    }
                    else if(expandSelection)
                    {
                        MapView.SelectedNodes.Remove(node);
                    }
                }
            }
            MapView.Canvas.Invalidate();
        }

        
        public void SelectNodeRightOrUnfold()
        {
            MapNode node = MapView.SelectedNodes.Last;
            if (node == null) return;

            if (node.Pos == NodePosition.Left)
            {
                this.MapView.SelectedNodes.Add(node.Parent, false);
            }
            else
            {
                if (node.Folded)
                {
                    this.ToggleNode(node);
                    return;
                }

                MapNode tmpNode = MapView.GetNodeView(node).GetLastSelectedChild(NodePosition.Right);
                if (tmpNode == null)
                {
                    return;
                }

                this.MapView.SelectedNodes.Add(tmpNode, false);
            }
            MapView.Canvas.Invalidate();
        }

        public void SelectNodeLeftOrUnfold()
        {
            MapNode node = MapView.SelectedNodes.Last;
            if (node == null) return;

            if (node.Pos == NodePosition.Right)
            {
                this.MapView.SelectedNodes.Add(node.Parent, false);
            }
            else
            {
                if (node.Folded)
                {
                    this.ToggleNode(node);
                    return;
                }

                MapNode tmpNode = MapView.GetNodeView(node).GetLastSelectedChild(NodePosition.Left);
                if (tmpNode == null)
                {
                    return;
                }

                this.MapView.SelectedNodes.Add(tmpNode, false);
            }
            MapView.Canvas.Invalidate();
        }

        
        public void ToggleNode()
        {
            if(MapView.SelectedNodes.Count == 1)
            {
                ToggleNode(MapView.SelectedNodes.First);
            }
        }

        /// <summary>
        /// Toggle Folded property
        /// </summary>
        /// <param name="node"></param>
        public void ToggleNode(MapNode node)
        {
            if (node.Pos != NodePosition.Root)
            {
                node.Folded = !node.Folded;
                MapView.RefreshChildNodePositions(tree.RootNode, node.Pos);
                MapView.Canvas.Invalidate(); 
            }
        }

        public void removeLastIcon()
        {
            var nSelCnt = this.MapView.SelectedNodes.Count;

            for (var i = 0; i < nSelCnt; i++)
            {

                MapNode node = this.MapView.SelectedNodes[i];

                
                node.Icons.RemoveLast();
                //node.NodeView.RefreshNodeView();
                node.NodeView.RefreshIcons();
                if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                MapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);
            }

            MapView.Canvas.Invalidate();

        }

        public void removeAllIcon()
        {
            int nSelCnt = this.MapView.SelectedNodes.Count;

            for (int i = 0; i < nSelCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                if (node == null)
                {
                    continue;
                }

                node.Icons.Clear();
                node.NodeView.RefreshIcons();
                if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                MapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);

            }

            MapView.Canvas.Invalidate();

        }

        public void appendIcon(string iconSrc)
        {
            int nSelCnt = this.MapView.SelectedNodes.Count;

            for (int i = 0; i < nSelCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                if (node == null)
                {
                    continue;
                }

                node.Icons.Add(iconSrc);
                //node.NodeView.RefreshNodeView();
                node.NodeView.RefreshIcons();
                if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                MapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);
            }

            MapView.Canvas.Invalidate();
        }

        public void appendIconFromIconSelector()
        {

            if (IconSelector.Instance.ShowDialog() == DialogResult.OK)
            {
                switch (IconSelector.Instance.SelectedIcon)
                {
                    case IconSelector.REMOVE_ICON_NAME:
                        removeLastIcon();
                        break;
                    case IconSelector.REMOVE_ALL_ICON_NAME:
                        removeAllIcon();
                        break;
                    default:
                        appendIcon(IconSelector.Instance.SelectedIcon);
                        break;
                }

            }

        }

        public void followLink(MapNode node)
        {
            switch (node.NodeView.Link.LinkType)
            {
                case NodeLinkType.Executable:
                case NodeLinkType.ExternalFile:
                case NodeLinkType.InternetLink:
                    try
                    {
                        Process.Start(node.Link);
                    }
                    catch (Exception e)
                    {
                        mainCtrl.ShowStatusNotification(e.Message);
                    }
                    break;
                case NodeLinkType.MindMapNode:

                    break;
            }
        }               
        
        public void MakeSelectedNodeShapeBubble()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Bubble;
            }
            this.MapView.Canvas.Invalidate();
        }

        public void MakeSelectedNodeShapeBox()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Box;
            }
            this.MapView.Canvas.Invalidate();
        }

        public void MakeSelectedNodeShapeFork()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Fork;
            }
            this.MapView.Canvas.Invalidate();
        }

        public void MakeSelectedNodeShapeBullet()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Bullet;
            }
            this.MapView.Canvas.Invalidate();
        }

        public void MakeSelectedNodeItalic()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Italic = !node.Italic;
                //node.NodeView.RefreshNodeView();
                node.NodeView.RefreshFont();
                if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                MapView.RefreshChildNodePositions(tree.RootNode, node.Pos);                
            }
            this.MapView.Canvas.Invalidate();
        }

        public void MakeSelectedNodeBold()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Bold = !node.Bold;
                //node.NodeView.RefreshNodeView();
                node.NodeView.RefreshFont();
                if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                MapView.RefreshChildNodePositions(tree.RootNode, node.Pos);
            }
            this.MapView.Canvas.Invalidate();
        }

        public void ChangeLineWidth(int width)
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.LineWidth != width)
                {
                    node.LineWidth = width;
                }                
            }
            this.MapView.Canvas.Invalidate();
        }

        public void ChangeLinePattern(System.Drawing.Drawing2D.DashStyle pattern)
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.LinePattern != pattern)
                {
                    node.LinePattern = pattern;
                }
            }
            this.MapView.Canvas.Invalidate();
        }

        /// <summary>
        /// Change Line Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeLineColor()
        {
            System.Drawing.Color color = new Color();
            
            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.LineColor;

            if (color.IsEmpty) color = NodeView.DefaultLineColor;
            
            //get new color specified by user
            color = mainCtrl.ShowColorPicker(color);
            if (color.IsEmpty) return;
            
            //set new color
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.LineColor != color)
                {
                    node.LineColor = color;
                }
            }
            this.MapView.Canvas.Invalidate();
        }

        /// <summary>
        /// Change Text Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeTextColor()
        {
            System.Drawing.Color color = new Color();

            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.Color;

            if (color.IsEmpty) color = NodeView.DefaultTextColor;

            //get new color specified by user
            color = mainCtrl.ShowColorPicker(color);
            if (color.IsEmpty) return;

            //set new color
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.Color != color)
                {
                    node.Color = color;
                }
            }
            this.MapView.Canvas.Invalidate();
        }

        /// <summary>
        /// Change Background Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeBackgroundColor()
        {
            System.Drawing.Color color = new Color();

            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.BackColor;

            //get new color specified by user
            color = mainCtrl.ShowColorPicker(color);
            if (color.IsEmpty) return;

            //set new color
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.BackColor != color)
                {
                    node.BackColor = color;
                }
            }
            this.MapView.Canvas.Invalidate();
        }

        /// <summary>
        /// Change Font for selected nodes using Font Dialog
        /// </summary>
        public void ChangeFont()
        {
            System.Drawing.Font font = this.MapView.SelectedNodes.First != null ?
                this.MapView.SelectedNodes.First.NodeView.Font : null;
            font = mainCtrl.ShowFontDialog(font);
            if (font == null) return;

            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.NodeView.Font != font)
                {
                    //update model
                    node.FontName = font.Name;
                    node.FontSize = font.Size;
                    node.Bold = font.Bold;
                    node.Italic = font.Italic;
                    
                    //update view
                    node.NodeView.RefreshFont();
                    if (node == tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
                    MapView.RefreshChildNodePositions(tree.RootNode, node.Pos);
                }                
            }
            this.MapView.Canvas.Invalidate();            
        }

        
    }
}
