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
        private readonly IMainCtrl mainCtrl;

        public MindMate.View.MapControls.MapView MapView;

        public ContextMenuCtrl ContextMenuCtrl { get; private set; }
        
        private MapTree tree { get { return MapView.Tree; } }

        public MapCtrl(MapView mapView, IMainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            MapViewMouseEventHandler map = new MapViewMouseEventHandler(this);

            MapView = mapView;

            MapView.Canvas.NodeClick += map.MapNodeClick;
            MapView.Canvas.NodeRightClick += map.NodeRightClick;
            MapView.Canvas.CanvasClick += map.CanvasClick;
            MapView.Canvas.NodeMouseOver += map.MapNodeMouseOver;
            MapView.Canvas.NodeMouseEnter += map.NodeMouseEnter;
            MapView.Canvas.NodeMouseExit += map.NodeMouseExit;
            MapView.Canvas.KeyDown += new MapViewKeyEventHandler(this).canvasKeyDown;
            MapView.Canvas.DragDropHandler.NodeDragStart += map.NodeDragStart;
            MapView.Canvas.DragDropHandler.NodeDragDrop += map.NodeDragDrop;
                        
            MapView.NodeTextEditor.Enabled = true;

            MapView.Canvas.BackColor = MetaModel.MetaModel.Instance.MapEditorBackColor;

            new ContextMenuAttacher(mainCtrl.NodeContextMenu, MapView);
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
                    }
                }
                else
                {
                    MindMate.View.Dialogs.LinkManualEdit frm = new MindMate.View.Dialogs.LinkManualEdit();
                    frm.LinkText = node.Link;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        node.Link = frm.LinkText == ""? null : frm.LinkText;                        
                    }
                }
            }
        }
        
        #region Node Editing

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
            }

        }

        public void EndNodeEdit(bool updateNode = true)
        {
            MapView.NodeTextEditor.EndNodeEdit(updateNode, true);
        }

        public void UpdateNodeText(MapNode node, string newText)
        {
            MapView.NodeTextEditor.UpdateNodeText(node, newText);
        }



        #endregion Node Editing

        #region Adding New Node

        public void AppendNodeAndEdit()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                if (MapView.SelectedNodes.First.Pos == NodePosition.Root)
                {
                    AppendChildNodeAndEdit();
                }
                else
                {
                    AppendSiblingNodeAndEdit();
                }
            }
        }

        public void AppendChildNodeAndEdit()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                MapNode newNode = this.AppendChildNode(node); 
                if (newNode != null)
                {
                    this.BeginNodeEdit(newNode, TextCursorPosition.Undefined);
                }
            }
        }

        public MapNode AppendChildNode(MapNode parent)
        {
            MapNode newNode = new MapNode(parent, "");
            if (newNode != null)
            {
                if (parent.Folded)
                {
                    this.ToggleFolded(parent);
                }

                this.MapView.SelectedNodes.Add(newNode, false);
            }
            return newNode;
        }

        /// <summary>
        /// Adds a multi-line child node
        /// </summary>
        public void AppendMultiLineNodeAndEdit()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                View.Dialogs.MultiLineNodeEdit frm = new View.Dialogs.MultiLineNodeEdit();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MapNode newNode = this.AppendChildNode(MapView.SelectedNodes.First);
                    newNode.Text = frm.txt.Text;
                }
            }
        }

        public void AppendSiblingNodeAndEdit()
        {        
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = MapView.SelectedNodes.First;

                MapNode newNode = AppendSiblingNode(node);
                if (newNode != null)
                {
                    this.BeginNodeEdit(newNode, TextCursorPosition.Undefined);
                }
            }
        }

        private MapNode AppendSiblingNode(MapNode node)
        {
            if (node.Pos == NodePosition.Root)
            {
                return null;
            }
            MapNode newNode = new MapNode(node.Parent, "", NodePosition.Undefined, null, node);
            
            this.MapView.SelectedNodes.Add(newNode, false);

            return newNode;
        }

        public void AppendSiblingAboveAndEdit()
        {
            if(MapView.SelectedNodes.Count == 1)
            {
                MapNode node = MapView.SelectedNodes.First;

                MapNode newNode = AppendSiblingAbove(node);
                if(newNode != null)
                {
                    BeginNodeEdit(newNode, TextCursorPosition.Undefined);
                }
            }
        }

        private MapNode AppendSiblingAbove(MapNode node)
        {
            if (node.Pos == NodePosition.Root)
            {
                return null;
            }
            MapNode newNode = new MapNode(node.Parent, "", NodePosition.Undefined, null, node, false);

            MapView.SelectedNodes.Add(newNode, false);

            return newNode;
        }

        public void InsertParentAndEdit()
        {
            if(MapView.SelectedNodes.Count == 1 && MapView.SelectedNodes.First.Pos != NodePosition.Root)
            {
                MapView.SuspendLayout();
                tree.ChangeManager.StartBatch("Add Parent Node"); 

                MapNode childNode = MapView.SelectedNodes.First;
                MapNode currentParent = childNode.Parent;
                MapNode newParent = new MapNode(currentParent, "", NodePosition.Undefined, null, childNode);
                childNode.Detach();
                childNode.AttachTo(newParent);                

                tree.ChangeManager.EndBatch();
                MapView.ResumeLayout(true, childNode.Pos);

                newParent.Selected = true;
                this.BeginCurrentNodeEdit(TextCursorPosition.Undefined);
            }
        }

        #endregion Adding New Node

        public void DeleteSelectedNodes()
        {
            bool isDeleted = false;

            if (this.MapView.SelectedNodes.Last == null || this.MapView.SelectedNodes.Contains(tree.RootNode)) return;

            if (!mainCtrl.SeekDeleteConfirmation("Do you really want to delete selected node(s)?")) return;

            MapView.SuspendLayout();
            var selNode = tree.GetClosestUnselectedNode(MapView.SelectedNodes.Last);
            if (tree.SelectedNodes.Count > 1) { tree.ChangeManager.StartBatch("Delete Nodes"); }

            for (var i = this.MapView.SelectedNodes.Count - 1; i >= 0; i--)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                node.DeleteNode();
                
                isDeleted = true;                
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
            
            if (isDeleted == true)
            {
                MapView.ResumeLayout(true);
                this.MapView.SelectedNodes.Add(selNode, false);
            }
            else
            {
                MapView.ResumeLayout();
            }
        }
               

        public void MoveNodeUp()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                node.MoveUp();
            }
        }

        public void MoveNodeDown()
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                node.MoveDown();                
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
            }
        }

        public void SelectTopSibling()
        {
            if (MapView.SelectedNodes.Count > 0)
            {
                this.MapView.SelectedNodes.Add(MapView.SelectedNodes.Last.GetFirstSib(), false);
            }
        }

        public void SelectBottomSibling()
        {
            if(MapView.SelectedNodes.Count > 0)
            {
                this.MapView.SelectedNodes.Add(MapView.SelectedNodes.Last.GetLastSib(), false);
            }
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
                    this.MapView.SelectedNodes.Add(node.Previous, expandSelection); //select or expand selection
                }
                else if(expandSelection)
                {
                    MapView.SelectedNodes.Remove(node);//reduce selection
                }
                else
                {
                    MapView.SelectedNodes.Add(node.Previous);//clear selection and select previous
                }
            }
            else if (node.Parent.Previous != null && node.Parent.Previous.LastChild  != null && !node.Parent.Previous.Folded)
            {
                if (!MapView.SelectedNodes.Contains(node.Parent.Previous.LastChild))
                {
                    this.MapView.SelectedNodes.Add(node.Parent.Previous.LastChild, expandSelection);
                }
                else if(expandSelection)
                {
                    MapView.SelectedNodes.Remove(node);
                }
                else
                {
                    this.MapView.SelectedNodes.Add(node.Parent.Previous.LastChild);
                }
            }
            else if(!expandSelection)
            {
                MapView.SelectedNodes.Add(node);
            }
            
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
                else
                {
                    MapView.SelectedNodes.Add(node.Next);
                }
            }
            else if (node.Parent.Next != null && node.Parent.Next.FirstChild != null &&!node.Parent.Next.Folded)
            {
                if (!MapView.SelectedNodes.Contains(node.Parent.Next.FirstChild))
                {
                    this.MapView.SelectedNodes.Add(node.Parent.Next.FirstChild, expandSelection);
                }
                else if (expandSelection)
                {
                    MapView.SelectedNodes.Remove(node);
                }
                else
                {
                    this.MapView.SelectedNodes.Add(node.Parent.Next.FirstChild);
                }
            }
            else if (!expandSelection)
            {
                MapView.SelectedNodes.Add(node);
            }
            
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
                    this.ToggleFolded(node);
                    return;
                }

                MapNode tmpNode = MapView.GetNodeView(node).GetLastSelectedChild(NodePosition.Right);
                if (tmpNode == null)
                {
                    return;
                }

                this.MapView.SelectedNodes.Add(tmpNode, false);
            }
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
                    this.ToggleFolded(node);
                    return;
                }

                MapNode tmpNode = MapView.GetNodeView(node).GetLastSelectedChild(NodePosition.Left);
                if (tmpNode == null)
                {
                    return;
                }

                this.MapView.SelectedNodes.Add(tmpNode, false);
            }
        }

        
        public void ToggleFolded()
        {
            if(MapView.SelectedNodes.Count == 1)
            {
                ToggleFolded(MapView.SelectedNodes.First);
            }
        }

        /// <summary>
        /// Toggle Folded property
        /// </summary>
        /// <param name="node"></param>
        public void ToggleFolded(MapNode node)
        {
            if (node.Pos != NodePosition.Root)
            {
                node.Folded = !node.Folded;                
            }
        }

        public void RemoveLastIcon()
        {
            var nSelCnt = this.MapView.SelectedNodes.Count;

            for (var i = 0; i < nSelCnt; i++)
            {

                MapNode node = this.MapView.SelectedNodes[i];

                node.Icons.RemoveLast();                              
            }

        }

        public void RemoveAllIcon()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Remove Icons"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                if (node == null)
                {
                    continue;
                }

                node.Icons.Clear();               

            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }

        }

        public void AppendIcon(string iconSrc)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Add Icons"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                if (node == null)
                {
                    continue;
                }

                node.Icons.Add(iconSrc);                                
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }

        }

        //public void appendIconFromIconSelector()
        //{

        //    if (IconSelector.Instance.ShowDialog() == DialogResult.OK)
        //    {
        //        switch (IconSelector.Instance.SelectedIcon)
        //        {
        //            case IconSelector.REMOVE_ICON_NAME:
        //                removeLastIcon();
        //                break;
        //            case IconSelector.REMOVE_ALL_ICON_NAME:
        //                removeAllIcon();
        //                break;
        //            default:
        //                appendIcon(IconSelector.Instance.SelectedIcon);
        //                break;
        //        }

        //    }

        //}

        public void AppendIconFromIconSelectorExt()
        {

            if (IconSelectorExt.Instance.ShowDialog() == DialogResult.OK)
            {
                switch (IconSelectorExt.Instance.SelectedIcon)
                {
                    case IconSelectorExt.REMOVE_ICON_NAME:
                        RemoveLastIcon();
                        break;
                    case IconSelectorExt.REMOVE_ALL_ICON_NAME:
                        RemoveAllIcon();
                        break;
                    default:
                        AppendIcon(IconSelectorExt.Instance.SelectedIcon);
                        break;
                }

            }

        }

        public void FollowLink(MapNode node)
        {
            try
            {
                node.NodeView.FollowLink();
            }
            catch (Exception e)
            {
                mainCtrl.ShowStatusNotification(e.Message);
            }   
        }               
        
        public void MakeSelectedNodeShapeBubble()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Node Shape Bubble"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Bubble;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void MakeSelectedNodeShapeBox()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Node Shape Box"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Box;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void MakeSelectedNodeShapeFork()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Node Shape Fork"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Fork;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void MakeSelectedNodeShapeBullet()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Node Shape Bullet"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Bullet;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ToggleSelectedNodeItalic()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Italic"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Italic = !node.Italic;                             
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ToggleSelectedNodeBold()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Bold"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Bold = !node.Bold;               
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ToggleSelectedNodeStrikeout()
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Strikeout"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Strikeout = !node.Strikeout;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void SetFontFamily(string family)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Font"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.FontName = family;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void SetFontSize(float size)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Font Size"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.FontSize = size;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ChangeLineWidth(int width)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Line Width Change"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.LineWidth != width)
                {
                    node.LineWidth = width;
                }                
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ChangeLinePattern(System.Drawing.Drawing2D.DashStyle pattern)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Line Pattern Change"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.LinePattern != pattern)
                {
                    node.LinePattern = pattern;
                }
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
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
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Line Color Change"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.LineColor != color)
                {
                    node.LineColor = color;
                }
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        /// <summary>
        /// Change Text Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeTextColorByPicker()
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
            ChangeTextColor(color);
        }

        public void ChangeTextColor(Color color)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Text Color Change"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.Color != color)
                {
                    node.Color = color;
                }
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        //TODO: No option in ribbon to launch Color Picker dialog for BackColor
        /// <summary>
        /// Change Background Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeBackColorByPicker()
        {
            System.Drawing.Color color = new Color();

            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.BackColor;

            //get new color specified by user
            color = mainCtrl.ShowColorPicker(color);
            if (color.IsEmpty) return;

            //set new color
            ChangeBackColor(color);
        }

        public void ChangeBackColor(Color color)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Background Color Change"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.BackColor != color)
                {
                    node.BackColor = color;
                }
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        /// <summary>
        /// Change Font for selected nodes using Font Dialog
        /// </summary>
        public void ChangeFont()
        {
            if (MapView.SelectedNodes.IsEmpty) return;

            System.Drawing.Font font = this.MapView.SelectedNodes.First != null ?
                this.MapView.SelectedNodes.First.NodeView.Font : null;
            font = mainCtrl.ShowFontDialog(font);
            if (font == null) return;

            NodePosition sideToRefresh = MapView.SelectedNodes.First.Pos;
            MapView.SuspendLayout();
            int selectCnt = this.MapView.SelectedNodes.Count;
            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Font Change"); }
            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.NodeView.Font != font)
                {
                    //update model
                    if(node.FontName != font.Name) node.FontName = font.Name;
                    if(node.FontSize != font.Size) node.FontSize = font.Size;
                    if(node.Bold != font.Bold) node.Bold = font.Bold;
                    if(node.Italic != font.Italic) node.Italic = font.Italic;
                    if(node.Strikeout != font.Strikeout) node.Strikeout = font.Strikeout;
                    
                    //update view
                    node.NodeView.RefreshFont();

                    if (sideToRefresh != node.Pos)
                        sideToRefresh = NodePosition.Undefined;
                }                
            }
            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
            MapView.ResumeLayout(true, sideToRefresh);
        }


        public void Copy()
        {
            if (MapView.NodeTextEditor.IsTextEditing)
                MapView.NodeTextEditor.CopyToClipboard();
            else
                ClipboardManager.Copy(tree.SelectedNodes);
        }

        public void Paste(bool asText = false)
        {
            if(MapView.NodeTextEditor.IsTextEditing)
            {
                MapView.NodeTextEditor.PasteFromClipboard();
            }
            else if (tree.SelectedNodes.Count == 1)
            {
                if (ClipboardManager.CanPaste)
                {
                    MapView.SuspendLayout();
                    tree.ChangeManager.StartBatch("Paste");
                    MapNode pasteLocation = tree.SelectedNodes[0];
                    ClipboardManager.Paste(pasteLocation, asText);                    
                    tree.ChangeManager.EndBatch();
                    MapView.ResumeLayout(true, pasteLocation.Pos);
                }
            }
            else // if multiple nodes are selected
            {
                MessageBox.Show("Paste operation cannot be performed if more than one nodes are selected.", "Can't Paste");
            }
            
        }

        public void Cut()
        {
            if (MapView.NodeTextEditor.IsTextEditing)
            {
                MapView.NodeTextEditor.CutToClipboard();
            }
            else
            {
                if (this.MapView.SelectedNodes.Last == null || this.MapView.SelectedNodes.Contains(tree.RootNode)) return;

                // confirm with user if he wants to overwrite existing cut nodes in clipboard
                if (ClipboardManager.HasCutNode &&
                    MessageBox.Show("You will lose data cut to clipboard earlier. Are you sure you want to overwrite?",
                    "Overwrite Clipboard Data!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;
                                                
                MapView.SuspendLayout();
                var selNode = this.MapView.Tree.GetClosestUnselectedNode(MapView.SelectedNodes.Last);

                if (MapView.SelectedNodes.Count > 1) { tree.ChangeManager.StartBatch("Cut Nodes"); }

                ClipboardManager.Cut(tree.SelectedNodes);

                if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }

                MapView.ResumeLayout(true);
                this.MapView.SelectedNodes.Add(selNode, false);
            }
        }

        /// <summary>
        /// Drag and Drop node(s) to a new location
        /// </summary>
        /// <param name="location"></param>
        public void MoveNodes(DropLocation location)
        {
            Debug.Assert(!location.IsEmpty);
            if (this.MapView.SelectedNodes.Last == null || MapView.SelectedNodes.Contains(tree.RootNode)) return;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            bool[] exclude = MapView.SelectedNodes.ExcludeNodesAlreadyPartOfHierarchy();

            MapView.SuspendLayout();
            
            tree.ChangeManager.StartBatch("Drag/Drop Node" + (nodes.Length > 0? "s" : ""));
            for(int i = 0; i < nodes.Length; i++)
            {
                MapNode n = nodes[i];
                if (!exclude[i])
                {
                    n.Detach();
                    Debug.Assert(n.Detached, "Detached property is false for node just detached.");
                    n.AttachTo(location.Parent, location.Sibling, location.InsertAfterSibling);
                    MapView.SelectedNodes.Add(n, true);
                }
            }

            if (location.Parent.Folded) location.Parent.Folded = false;

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }

            MapView.ResumeLayout(true);            
        }         

        public void SetMapViewBackColor(Color color)
        {
            MapView.Canvas.BackColor = color;
        }

        public void CopyFormat(bool multiApply = false)
        {
            if (MapView.SelectedNodes.Count == 1)
            {
                MapView.FormatPainter.Copy(MapView.SelectedNodes.First, multiApply);
            }
        }

        public void EnableFormatMultiApply()
        {
            MapView.FormatPainter.EnableMultiApply();
        }

        public void PasteFormat()
        {
            MapView.SuspendLayout();

            tree.ChangeManager.StartBatch("Copy/Paste Format");

            MapView.FormatPainter.Paste(MapView.SelectedNodes);

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }

            MapView.ResumeLayout(true, MapView.SelectedNodes.Count == 1? MapView.SelectedNodes.First.Pos : NodePosition.Undefined);

        }

        public void ClearFormatPainter()
        {
            MapView.FormatPainter.Clear();
        }

        public void SelectAllNodes()
        {
            var location = MapView.Canvas.Location;

            MapView.Tree.SelectAllNodes();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        /// <summary>
        /// Select all nodes at the given level i.e. depth from root
        /// </summary>
        /// <param name="level"></param>
        /// <param name="expandSelection"></param>
        public void SelectLevel(int level, bool expandSelection = false)
        {
            var location = MapView.Canvas.Location;

            if (!expandSelection) { MapView.SelectedNodes.Clear(); }

            MapView.Tree.RootNode.RollDownAggregate(
                (n, v) =>
                {
                    if (level == v)
                    {
                        MapView.SelectedNodes.Add(n, true);
                    }
                    return v + 1;
                },
                0,
                (n, v) => n.Folded
                );

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectCurrentLevel()
        {
            var location = MapView.Canvas.Location;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var n in nodes)
            {
                int level = n.GetNodeDepth();
                SelectLevel(level, true);
            }

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectSiblings()
        {
            var location = MapView.Canvas.Location;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEachSibling(n => MapView.SelectedNodes.Add(n, true));
            }

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectAncestors()
        {
            var location = MapView.Canvas.Location;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEachAncestor(n => MapView.SelectedNodes.Add(n, true));
            }

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectChildren(bool expandSelection)
        {
            var location = MapView.Canvas.Location;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEach(n => MapView.SelectedNodes.Add(n, true), n => n == node);
                if (!expandSelection) { node.Selected = false; }
            }

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectDescendents()
        {
            var location = MapView.Canvas.Location;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEach(n => MapView.SelectedNodes.Add(n, true));
            }

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectDescendents(int depth)
        {
            var location = MapView.Canvas.Location;

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.RollDownAggregate(
                    (n, v) => {
                        MapView.SelectedNodes.Add(n, true);
                        return v + 1;
                    },
                    0,
                    (n, v) => v > depth || n.Folded
                    );
            }

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }
    }
}
