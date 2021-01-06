/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MindMate.MetaModel;
using MindMate.Model;
using MindMate.Modules.Logging;
using MindMate.Plugins.Tasks.Model;
using MindMate.Serialization;
using MindMate.View.Dialogs;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Map Controller.
    /// Should not handle view events directly, rather through routers [MapViewEventHandler (Key & Mouse)]
    /// Controls MapView. Should not have knowledge about other UI controls like NoteEditor, StatusBar etc.
    /// </summary>
    public class MapCtrl
    {
        private readonly DialogManager dialogs;

        public MapView MapView;

        public ContextMenuCtrl ContextMenuCtrl { get; private set; }
        
        private MapTree tree { get { return MapView.Tree; } }

        public MapCtrl(MapView mapView, DialogManager dialogs, NodeContextMenu nodeContextMenu)
        {
            this.dialogs = dialogs;

            MapViewMouseEventHandler map = new MapViewMouseEventHandler(this);

            MapView = mapView;

            MapView.Canvas.NodeClick += map.MapNodeClick;
            MapView.Canvas.NodeRightClick += map.NodeRightClick;
            MapView.Canvas.CanvasClick += map.CanvasClick;
            MapView.Canvas.NodeMouseHover += map.MapNodeMouseHover;
            MapView.Canvas.NodeMouseMove += map.MapNodeMouseMove;
            MapView.Canvas.NodeMouseEnter += map.NodeMouseEnter;
            MapView.Canvas.NodeMouseExit += map.NodeMouseExit;
            MapView.Canvas.KeyDown += new MapViewKeyEventHandler(this).canvasKeyDown;
            MapView.Canvas.DragDropHandler.NodeDragStart += map.NodeDragStart;
            MapView.Canvas.DragDropHandler.NodeDragDrop += map.NodeDragDrop;
                        
            MapView.NodeTextEditor.Enabled = true;

            new ContextMenuAttacher(nodeContextMenu, MapView);
        }        

        public void AddHyperlinkUsingTextbox()
        {
            if (tree.SelectedNodes.Count > 0)
            {
                MapNode node = MapView.SelectedNodes.First;

                LinkManualEdit frm = new LinkManualEdit();
                frm.LinkText = node.Link;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AddHyperlink(frm.LinkText == "" ? null : frm.LinkText);
                }
            }
        }

        public void AddHyperlinkUsingFileDialog()
        {
            if (tree.SelectedNodes.Count > 0)
            {
                MapNode node = MapView.SelectedNodes.First;

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = Path.GetDirectoryName(node.Link);
                dialog.FileName = node.Link;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AddHyperlink(dialog.FileName);
                }
            }
        }

        public void AddHyperlinkUsingFolderDialog()
        {
            if (tree.SelectedNodes.Count > 0)
            {
                MapNode node = MapView.SelectedNodes.First;

                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = node.Link;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AddHyperlink(dialog.SelectedPath);
                }
            }
        }

        /// <summary>
        /// Add / Update given hyperlink to the selected nodes.
        /// It is a ChangeManager Batch operation.
        /// </summary>
        /// <param name="link"></param>
        public void AddHyperlink(string link)
        {
            using (tree.ChangeManager.StartBatch("Add hyperlink"))
            {
                foreach (var node in tree.SelectedNodes)
                {
                    node.Link = link;
                }
            }
        }

        /// <summary>
        /// Remove hyperlink from selected nodes.
        /// It is a ChangeManager Batch operation.
        /// </summary>
        public void RemoveHyperlink()
        {
            using (tree.ChangeManager.StartBatch("Remove hyperlink"))
            {
                foreach (var node in tree.SelectedNodes)
                {
                    node.Link = null;
                }
            }
        }
        
        #region Node Editing

        public void BeginCurrentNodeEdit(TextCursorPosition org = TextCursorPosition.Undefined)
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

            MultiLineNodeEdit frm = new MultiLineNodeEdit();
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
                tree.ChangeManager.StartBatch("Add Child Node");

                MapNode node = this.MapView.SelectedNodes.First;
                MapNode newNode = this.AppendChildNode(node); 
                if (newNode != null)
                {
                    this.BeginNodeEdit(newNode, TextCursorPosition.Undefined);
                }

                tree.ChangeManager.EndBatch();
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
                tree.ChangeManager.StartBatch("Add Child Node");

                MultiLineNodeEdit frm = new MultiLineNodeEdit();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MapNode newNode = this.AppendChildNode(MapView.SelectedNodes.First);
                    newNode.Text = frm.txt.Text;
                }

                tree.ChangeManager.EndBatch();
            }
        }

        public void AppendSiblingNodeAndEdit()
        {        
            if (MapView.SelectedNodes.Count == 1)
            {
                tree.ChangeManager.StartBatch("Add Sibling Node");

                MapNode node = MapView.SelectedNodes.First;
                MapNode newNode = AppendSiblingNode(node);
                if (newNode != null)
                {
                    this.BeginNodeEdit(newNode, TextCursorPosition.Undefined);
                }

                tree.ChangeManager.EndBatch();
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
                tree.ChangeManager.StartBatch("Add Sibling Above");

                MapNode node = MapView.SelectedNodes.First;
                MapNode newNode = AppendSiblingAbove(node);
                if(newNode != null)
                {
                    BeginNodeEdit(newNode, TextCursorPosition.Undefined);
                }

                tree.ChangeManager.EndBatch();
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

            if (!dialogs.SeekDeleteConfirmation("Do you really want to delete selected node(s)?")) return;

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
            EndNodeEdit(); //if node is being edited, end it
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                node.MoveUp();
            }
        }

        public void MoveNodeDown()
        {
            EndNodeEdit(); //if node is being edited, end it
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

        public void SelectRootNode()
        {
            EndNodeEdit(); //if node is being edited, end it
            tree.RootNode.Selected = true;
        }

        public void SelectTopSibling()
        {
            EndNodeEdit(); //if node is being edited, end it
            if (MapView.SelectedNodes.Count > 0)
            {
                this.MapView.SelectedNodes.Add(MapView.SelectedNodes.Last.GetFirstSib(), false);
            }
        }

        public void SelectBottomSibling()
        {
            EndNodeEdit(); //if node is being edited, end it
            if (MapView.SelectedNodes.Count > 0)
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
            tree.ChangeManager.StartBatch("Toggle Expand / Collapse");

            foreach(var node in tree.SelectedNodes)
            {
                ToggleFolded(node);
            }

            tree.ChangeManager.EndBatch();
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

                node?.Icons.Clear();               

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

                node?.Icons.Add(iconSrc);                                
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
                dialogs.ShowStatusNotification(e.Message);
            }   
        }               
        
        public void ChangeNodeShapeBubble()
        {
            ChangeNodeShape(NodeShape.Bubble);
        }

        public void ChangeNodeShapeBox()
        {
            ChangeNodeShape(NodeShape.Box);
        }

        public void ChangeNodeShapeFork()
        {
            ChangeNodeShape(NodeShape.Fork);
        }

        public void ChangeNodeShapeBullet()
        {
            ChangeNodeShape(NodeShape.Bullet);
        }

        public void ChangeNodeShape(NodeShape shape)
        {
            using (tree.ChangeManager.StartBatch("Node Shape " + shape.ToString()))
            {
                for (int i = 0; i < MapView.SelectedNodes.Count; i++)
                {
                    MapNode node = this.MapView.SelectedNodes[i];
                    node.Shape = shape;
                }
            }
        }

        public void ChangeNodeShape(string shape)
        {
            NodeShape nodeShape = (NodeShape)Enum.Parse(typeof (NodeShape), shape);
            ChangeNodeShape(nodeShape);
        }

        public void ClearNodeShape()
        {
            using (tree.ChangeManager.StartBatch("Clear Node Shape"))
            {
                for (int i = 0; i < MapView.SelectedNodes.Count; i++)
                {
                    MapNode node = this.MapView.SelectedNodes[i];
                    node.Shape = NodeShape.None;
                }
            }
        }

        public void ToggleItalic()
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

        public void ChangeItalic(bool value)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Italic"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Italic = value;
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ToggleBold()
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

        public void ChangeBold(bool value)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Bold"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Bold = value;               
            }

            if (tree.ChangeManager.IsBatchOpen) { tree.ChangeManager.EndBatch(); }
        }

        public void ToggleStrikeout()
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

        public void ChangeStrikeout(bool value)
        {
            int selectCnt = this.MapView.SelectedNodes.Count;

            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Strikeout"); }

            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Strikeout = value;
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
            using (tree.ChangeManager.StartBatch("Line Width " + width))
            {
                for (int i = 0; i < MapView.SelectedNodes.Count; i++)
                {
                    MapNode node = this.MapView.SelectedNodes[i];
                    if (node.LineWidth != width)
                    {
                        node.LineWidth = width;
                    }
                }
            }
        }

        public void ChangeLinePattern(DashStyle pattern)
        {
            using (tree.ChangeManager.StartBatch("Line Pattern " + pattern))
            {
                for (int i = 0; i < MapView.SelectedNodes.Count; i++)
                {
                    MapNode node = this.MapView.SelectedNodes[i];
                    if (node.LinePattern != pattern)
                    {
                        node.LinePattern = pattern;
                    }
                }
            }
        }

        public void ChangeLineColor(Color color)
        {
            using (tree.ChangeManager.StartBatch("Line Color Change"))
            {
                for (int i = 0; i < MapView.SelectedNodes.Count; i++)
                {
                    MapNode node = this.MapView.SelectedNodes[i];
                    if (node.LineColor != color)
                    {
                        node.LineColor = color;
                    }
                }
            }
        }

        /// <summary>
        /// Change Line Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeLineColorUsingPicker()
        {
            Color color = new Color();
            
            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.LineColor;

            if (color.IsEmpty) color = NodeFormat.DefaultLineColor;
            
            //get new color specified by user
            color = dialogs.ShowColorPicker(color);
            if (color.IsEmpty) return;

            //set new color
            ChangeLineColor(color);
        }

        /// <summary>
        /// Change Text Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeTextColorByPicker()
        {
            Color color = new Color();

            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.Color;

            if (color.IsEmpty) color = NodeFormat.DefaultColor;

            //get new color specified by user
            color = dialogs.ShowColorPicker(color);
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
        
        /// <summary>
        /// Change Background Color for selected nodes using Color Picker Dialog
        /// </summary>
        public void ChangeBackColorByPicker()
        {
            Color color = new Color();

            //get current color
            if (this.MapView.SelectedNodes.Count == 1)
                color = this.MapView.SelectedNodes.First.BackColor;

            //get new color specified by user
            color = dialogs.ShowColorPicker(color);
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

            Font font = this.MapView.SelectedNodes.First != null ?
                this.MapView.SelectedNodes.First.NodeView.NodeFormat.Font : null;
            font = dialogs.ShowFontDialog(font);
            if (font == null) return;

            NodePosition sideToRefresh = MapView.SelectedNodes.First.Pos;
            MapView.SuspendLayout();
            int selectCnt = this.MapView.SelectedNodes.Count;
            if (selectCnt > 1) { tree.ChangeManager.StartBatch("Font Change"); }
            for (int i = 0; i < selectCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.NodeView.NodeFormat.Font != font)
                {
                    //update model
                    if(node.FontName != font.Name) node.FontName = font.Name;
                    if(node.FontSize != font.Size) node.FontSize = font.Size;
                    if(node.Bold != font.Bold) node.Bold = font.Bold;
                    if(node.Italic != font.Italic) node.Italic = font.Italic;
                    if(node.Strikeout != font.Strikeout) node.Strikeout = font.Strikeout;
                    
                    //update view
                    node.NodeView.RefreshFontAndFormat();

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

        public void Paste(bool asText = false, bool fileAsImage = false)
        {
            try
            {
                if (MapView.NodeTextEditor.IsTextEditing)
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
                        ClipboardManager.Paste(pasteLocation, asText, fileAsImage);
                        tree.ChangeManager.EndBatch();
                        MapView.ResumeLayout(true, pasteLocation.Pos);
                    }
                }
                else // if multiple nodes are selected
                {
                    MessageBox.Show("Paste operation cannot be performed if more than one nodes are selected.", "Can't Paste");
                }
            }
            catch (Exception e)
            {
                Log.Write("Error in Paste operation", e);
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

            var nodes = MapView.SelectedNodes.ExcludeNodesAlreadyPartOfHierarchy().ToList();

            MapView.SuspendLayout();
            
            tree.ChangeManager.StartBatch("Drag/Drop Node" + (nodes.Count > 0? "s" : ""));
            foreach(var n in nodes)
            {
                n.Detach();
                Debug.Assert(n.Detached, "Detached property is false for node just detached.");
                n.AttachTo(location.Parent, location.Sibling, location.InsertAfterSibling);
                MapView.SelectedNodes.Add(n, true);
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

        public void SelectAllNodes(bool unfoldNodes)
        {
            var location = MapView.Canvas.Location;

            tree.ChangeManager.StartBatch("Expand Nodes for Select All");

            MapView.Tree.RootNode.ForEach(
                n => MapView.Tree.SelectedNodes.Add(n, true), //action
                n => !n.Folded || unfoldNodes          //condition for traversing descendents   
                );

            tree.ChangeManager.EndBatch();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        /// <summary>
        /// Select all nodes at the given level i.e. depth from root
        /// </summary>
        /// <param name="level"></param>
        /// <param name="expandSelection"></param>
        /// <param name="expandNodes"></param>
        public void SelectLevel(int level, bool expandSelection, bool expandNodes)
        {
            var location = MapView.Canvas.Location;

            tree.ChangeManager.StartBatch("Expand Nodes for Select Level");

            tree.SelectLevel(level, expandSelection, expandNodes);

            tree.ChangeManager.EndBatch();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectCurrentLevel(bool expandNodes)
        {
            var location = MapView.Canvas.Location;

            tree.ChangeManager.StartBatch("Expand Nodes for Select Level");

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var n in nodes)
            {
                int level = n.GetNodeDepth();
                tree.SelectLevel(level, true, expandNodes);
            }

            tree.ChangeManager.EndBatch();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectSiblings()
        {
            var location = MapView.Canvas.Location;

            tree.ChangeManager.StartBatch("Expand Nodes for Select Siblings");

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEachSibling(n => MapView.SelectedNodes.Add(n, true));
            }

            tree.ChangeManager.EndBatch();

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

            tree.ChangeManager.StartBatch("Expand Nodes for Select Children");

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEach(n => MapView.SelectedNodes.Add(n, true), n => n == node);
                if (!expandSelection) { node.Selected = false; }
            }

            tree.ChangeManager.EndBatch();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectDescendents(bool expandNodes)
        {
            var location = MapView.Canvas.Location;

            tree.ChangeManager.StartBatch("Expand Nodes for Select Descendents");

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.ForEach(
                    n => MapView.SelectedNodes.Add(n, true),
                    n => !n.Folded || expandNodes);
            }

            tree.ChangeManager.EndBatch();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void SelectDescendents(int depth, bool expandNodes)
        {
            var location = MapView.Canvas.Location;

            tree.ChangeManager.StartBatch("Expand Nodes for Select Descendents");

            MapNode[] nodes = MapView.SelectedNodes.ToArray();
            foreach (var node in nodes)
            {
                node.RollDownAggregate(
                    (n, v) => {
                        MapView.SelectedNodes.Add(n, true);
                        return v + 1;
                    },
                    0,
                    (n, v) => v > depth || (n.Folded && !expandNodes)
                    );
            }

            tree.ChangeManager.EndBatch();

            //mapview will bring last selected node into view, to avoid this in current case, Canvas location is saved and restored
            MapView.Canvas.Location = location;
        }

        public void FoldAll()
        {
            tree.ChangeManager.StartBatch("Collapse All Nodes");

            tree.RootNode.FoldDescendents();

            tree.ChangeManager.EndBatch();
        }

        public void UnfoldAll()
        {
            tree.ChangeManager.StartBatch("Expand All Nodes");

            tree.RootNode.UnfoldDescendents();

            tree.ChangeManager.EndBatch();
        }

        public void ToggleBranchFolding()
        {
            tree.ChangeManager.StartBatch("Toggle Branch Expand/Collapse");

            foreach (var node in tree.SelectedNodes)
            {
                node.ToggleDescendentsFolding();
            }

            tree.ChangeManager.EndBatch();
        }

        public void UnfoldMapToCurrentLevel()
        {
            if (tree.SelectedNodes.Count > 0)
            {
                tree.ChangeManager.StartBatch("Expand/Collapse to current level");

                tree.UnfoldMapToLevel(tree.SelectedNodes.First.GetNodeDepth());

                tree.ChangeManager.EndBatch();
            }
        }

        public void UnfoldMapToLevel(int level)
        {
            tree.ChangeManager.StartBatch("Expand/Collapse to level " + level);

            tree.UnfoldMapToLevel(level);

            tree.ChangeManager.EndBatch();
        }

        public void SortAlphabeticallyAsc()
        {
            tree.ChangeManager.StartBatch("Sort Alphabetically Asc");

            foreach (var node in tree.SelectedNodes)
            {
                node.SortChildren((n1, n2) => string.CompareOrdinal(n1.Text, n2.Text));

                if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
            }

            tree.ChangeManager.EndBatch();
        }

        /// <summary>
        /// Sort children of the selected nodes in ascending alphabetical order
        /// </summary>
        public void SortAlphabeticallyDesc()
        {
            tree.ChangeManager.StartBatch("Sort Alphabetically Desc");

            foreach (var node in tree.SelectedNodes)
            {
                node.SortChildren((n1, n2) => string.CompareOrdinal(n2.Text, n1.Text));

                if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
            }

            tree.ChangeManager.EndBatch();
        }

        public void SortByTaskAsc()
        {
            tree.ChangeManager.StartBatch("Sort by Task Asc");

            foreach (var node in tree.SelectedNodes)
            {
                node.SortChildren((n1, n2) => DateTime.Compare(
                    GetTaskDateForSort(n1),
                    GetTaskDateForSort(n2)
                    ));

                if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
            }

            tree.ChangeManager.EndBatch();
        }        

        public void SortByTaskDesc()
        {
            tree.ChangeManager.StartBatch("Sort by Task Desc");

            foreach (var node in tree.SelectedNodes)
            {
                node.SortChildren((n1, n2) => DateTime.Compare(
                    GetTaskDateForSort(n2),
                    GetTaskDateForSort(n1)
                    ));

                if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
            }

            tree.ChangeManager.EndBatch();
        }

        private DateTime GetTaskDateForSort(MapNode node)
        {
            return node.GetEndDate();
        }

        public void SortByDescendentsCountAsc()
        {
            tree.ChangeManager.StartBatch("Sort by Descendents Count Asc");

            foreach (var node in tree.SelectedNodes)
            {
                node.SortChildren((n1, n2) => n1.GetDescendentsCount() - n2.GetDescendentsCount());

                if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
            }

            tree.ChangeManager.EndBatch();
        }

        public void SortByDescendentsCountDesc()
        {
            tree.ChangeManager.StartBatch("Sort by Descendents Count Desc");

            foreach (var node in tree.SelectedNodes)
            {
                node.SortChildren((n1, n2) => n2.GetDescendentsCount() - n1.GetDescendentsCount());

                if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
            }

            tree.ChangeManager.EndBatch();
        }

        public void SortByCreateDateAsc()
        {
            RunCommand("Sort by Creation Date Asc", true, () =>
            {
                foreach (var node in tree.SelectedNodes)
                {
                    node.SortChildren((n1, n2) => DateTime.Compare(n1.Created, n2.Created));

                    if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
                }
            });
        }

        public void SortByCreateDateDesc()
        {
            RunCommand("Sort by Creation Date Desc", true, () =>
            {
                foreach (var node in tree.SelectedNodes)
                {
                    node.SortChildren((n1, n2) => DateTime.Compare(n2.Created, n1.Created));

                    if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
                }
            });
        }

        public void SortByModifiedDateAsc()
        {
            RunCommand("Sort by Modified Date Asc", true, () =>
            {
                foreach (var node in tree.SelectedNodes)
                {
                    node.SortChildren((n1, n2) => DateTime.Compare(n1.Modified, n2.Modified));

                    if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
                }
            });
        }

        public void SortByModifiedDateDesc()
        {
            RunCommand("Sort by Modified Date Desc", true, () =>
            {
                foreach (var node in tree.SelectedNodes)
                {
                    node.SortChildren((n1, n2) => DateTime.Compare(n2.Modified, n1.Modified));

                    if (node.Pos == NodePosition.Root) { tree.RebalanceTree(); }
                }
            });            
        }

        /// <summary>
        /// Create NodeStyle from selected node and adds it to the MetaModel
        /// </summary>
        /// <returns>returns null if more than one nodes are selected</returns>
        public NodeStyle CreateNodeStyle()
        {
            try
            {
                if (tree.SelectedNodes.Count == 1)
                {
                    var n = tree.SelectedNodes.First;
                    var styleName = dialogs.ShowInputBox("Enter the style name:");
                    var s = new NodeStyle(styleName, n, true);
                    new NodeStyleImageSerializer().SerializeImage(s.Image, s.Title);
                    MetaModel.MetaModel.Instance.NodeStyles.Add(s);
                    MetaModel.MetaModel.Instance.Save();
                    return s;
                }
            }
            catch (Exception e)
            {
                dialogs.ShowMessageBox("Error", e.Message, MessageBoxIcon.Error);
                Log.Write("Error in CreateNoteStyle.", e);
            }

            return null;
        }

        public void ApplyNodeStyle(NodeStyle style)
        {
            RunCommand("Apply Node Style", true, () => style.ApplyTo(tree.SelectedNodes) );
        }

        public void ClearFormatting()
        {
            RunCommand("Clear Formatting", node => node.ClearFormatting());
        }

        public void InsertImage()
        {
            string fileName = dialogs.GetImageFile();
            if (fileName != null)
            {
                RunCommand("Insert Image", node => {
                    if (ImageHelper.GetImageFromFile(fileName, out Image image))
                    {
                        node.InsertImage(image, true);
                    }
                });               
            }
        }

        public void RemoveImage()
        {
            RunCommand("Remove Image", n => n.RemoveImage());
        }

        public void IncreaseImageSize()
        {
            RunCommand("Increase Image Size",
                node => node.ImageSize = new Size((int)(node.ImageSize.Width * 1.1), (int)(node.ImageSize.Height * 1.1)),
                node => node.HasImage
                );            
        }

        public void DecreaseImageSize()
        {
            RunCommand("Decrease Image Size",
                node => node.ImageSize = new Size((int)(node.ImageSize.Width * 0.9), (int)(node.ImageSize.Height * 0.9)),
                node => node.HasImage
                );            
        }

        public void ImageAlignStart()
        {
            RunCommand("Image Align Start", n => n.SetImageAlignment(ImageAlign.Start));            
        }

        public void ImageAlignCenter()
        {
            RunCommand("Image Align Center", n => n.SetImageAlignment(ImageAlign.Center));            
        }

        public void ImageAlignEnd()
        {
            RunCommand("Image Align End", n => n.SetImageAlignment(ImageAlign.End));            
        }

        public void ImagePosAbove()
        {
            RunCommand("Image Position Above", n => n.SetImagePosition(ImagePosition.Above));
        }

        public void ImagePosBelow()
        {
            RunCommand("Image Position Below", n => n.SetImagePosition(ImagePosition.Below));
        }

        public void ImagePosBefore()
        {
            RunCommand("Image Position Before", n => n.SetImagePosition(ImagePosition.Before));
        }

        public void ImagePosAfter()
        {
            RunCommand("Image Position After", n => n.SetImagePosition(ImagePosition.After));
        }

        public void SetSelectedNodeFormatAsDefault()
        {
            RunCommand("Set Selected Node Format As Default", false, () =>
            {
                var format = tree.SelectedNodes.First?.NodeView?.NodeFormat;
                if (format != null)
                {
                    tree.DefaultFormat = format;
                }

            });
        }

        public void SetDefaultFormatDialog()
        {
            RunCommand("Set Default Format Dialog", true, () => {

                var form = new DefaultFormatSettings(this.dialogs);
                var controller = new DefaultFormatSettingsCtrl();
                controller.UpdateSettingsFromMapTree(this.tree, form);
                if (this.dialogs.ShowDefaultFormatSettingsDialog(form) == DialogResult.OK)
                {
                    controller.UpdateMapTreeFromSettings(this.tree, form);
                }

            });            
        }

        public void ApplyTheme(string theme)
        {
            RunCommand("Apply Theme", true, () => MetaModel.MetaModel.Instance.Themes.ApplyTheme(theme, tree) );
        }

        /// <summary>
        /// Runs the given command on all selected nodes
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        private void RunCommand(string command, Action<MapNode> action)
        {
            //TODO: utilize this method in MapCtrl wherever possible
            RunCommand(command, tree.SelectedNodes.IsMultiple, () =>
            {
                foreach (var node in tree.SelectedNodes)
                {
                    action(node);
                }
            });
        }

        /// <summary>
        /// Runs the given command on selected nodes that pass the condition
        /// </summary>
        /// <param name="command"></param>
        /// <param name="action"></param>
        /// <param name="condition"></param>
        private void RunCommand(string command, Action<MapNode> action, Func<MapNode, bool> condition)
        {
            //TODO: utilize this method in MapCtrl wherever possible
            RunCommand(command, tree.SelectedNodes.IsMultiple, () =>
            {
                foreach (var node in tree.SelectedNodes.Where(condition))
                {
                    action(node);
                }
            });
        }

        //TODO: utilize this method in MapCtrl wherever possible
        private void RunCommand(string command, bool useBatch, Action action)
        {
            try
            {
                if(useBatch)
                {
                    using(tree.ChangeManager.StartBatch(command))
                    {
                        action();
                    }
                }
                else
                {
                    action();
                }
            }
            catch (Exception exp)
            {
                Log.Write($"Unexpected exception while executing command '{command}': {exp.Message}");
                dialogs.ShowMessageBox("Error", exp.Message, MessageBoxIcon.Error);
            }
        }

    }
}
