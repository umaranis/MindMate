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

        const int w = 4096;
        const int h = 4096;

        public MapCtrl(MapTree tree, IMainCtrl mainCtrl)
        {
            this.mainCtrl = mainCtrl;

            map = new MapViewMouseEventHandler(this);
            this.tree = tree;
                       
            MapView = new MapView(tree);
            MapView.Canvas.NodeClick += map.MapNodeClick;
            MapView.Canvas.NodeRightClick += map.NodeRightClick;
            MapView.Canvas.CanvasClick += map.CanvasClick;
            MapView.Canvas.NodeMouseOver += map.MapNodeMouseOver;
            MapView.Canvas.NodeMouseEnter += map.NodeMouseEnter;
            MapView.Canvas.NodeMouseExit += map.NodeMouseExit;
            MapView.Canvas.KeyDown += new MapViewKeyEventHandler(this).canvasKeyDown;
            
            MapView.NodeTextEditor.Enabled = true;

            MapView.Canvas.BackColor = MetaModel.MetaModel.Instance.MapEditorBackColor;

            //mainCtrl.AddMainPanel(this.MapView.Canvas);

            this.MapView.Canvas.Width = w;
            this.MapView.Canvas.Height = h;
                        
            MapView.RefreshNodePositions();           
            
        }        

        public void ChangeTree(MapTree tree)
        {
            this.tree = tree;
            this.MapView.ChangeTree(tree);
            MapView.RefreshNodePositions();
            MapView.SelectedNodes.Add(tree.RootNode, false);
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
                    this.ToggleNode(parent);
                }

                this.MapView.SelectedNodes.Add(newNode, false);
            }
            return newNode;
        }

        public void AppendSiblingNodeAndEdit()
        {        
            if (MapView.SelectedNodes.Count == 1)
            {
                MapNode node = this.MapView.SelectedNodes.First;

                MapNode tmpNode = this.AppendSiblingNode(node);
                if (tmpNode != null)
                {
                    this.BeginNodeEdit(tmpNode, TextCursorPosition.Undefined);
                }
            }
        }

        public MapNode AppendSiblingNode(MapNode node)
        {
            if (node.Pos == NodePosition.Root)
            {
                return null;
            }
            MapNode newNode = new MapNode(node.Parent, "", NodePosition.Undefined, null, node);
            
            this.MapView.SelectedNodes.Add(newNode, false);

            return newNode;
        }

        #endregion Adding New Node

        public void DeleteSelectedNodes()
        {
            bool isDeleted = false;

            if (this.MapView.SelectedNodes.Last == null || this.MapView.SelectedNodes.Contains(tree.RootNode)) return;

            if (!mainCtrl.SeekDeleteConfirmation("Do you really want to delete selected node(s)?")) return;

            MapView.SuspendLayout();
            var selNode = tree.GetClosestUnselectedNode(MapView.SelectedNodes.Last);

            for (var i = this.MapView.SelectedNodes.Count - 1; i >= 0; i--)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                node.DeleteNode();
                
                isDeleted = true;                
            }

            MapView.ResumeLayout();
            if (isDeleted == true)
            {
                MapView.RefreshChildNodePositions(tree.RootNode, NodePosition.Undefined);
                this.MapView.SelectedNodes.Add(selNode, false);
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
            int nSelCnt = this.MapView.SelectedNodes.Count;

            for (int i = 0; i < nSelCnt; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];

                if (node == null)
                {
                    continue;
                }

                node.Icons.Clear();               

            }

        }

        public void AppendIcon(string iconSrc)
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
            }

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
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Bubble;
            }
        }

        public void MakeSelectedNodeShapeBox()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Box;
            }
        }

        public void MakeSelectedNodeShapeFork()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Fork;
            }
        }

        public void MakeSelectedNodeShapeBullet()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Shape = NodeShape.Bullet;
            }
        }

        public void MakeSelectedNodeItalic()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Italic = !node.Italic;                             
            }
        }

        public void MakeSelectedNodeBold()
        {
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                node.Bold = !node.Bold;               
            }
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
            for (int i = 0; i < this.MapView.SelectedNodes.Count; i++)
            {
                MapNode node = this.MapView.SelectedNodes[i];
                if (node.NodeView.Font != font)
                {
                    //update model
                    if(node.FontName != font.Name) node.FontName = font.Name;
                    if(node.FontSize != font.Size) node.FontSize = font.Size;
                    if(node.Bold != font.Bold) node.Bold = font.Bold;
                    if(node.Italic != font.Italic) node.Italic = font.Italic;
                    
                    //update view
                    node.NodeView.RefreshFont();

                    if (sideToRefresh != node.Pos)
                        sideToRefresh = NodePosition.Undefined;
                }                
            }
            MapView.ResumeLayout();
            MapView.RefreshChildNodePositions(tree.RootNode, sideToRefresh);
        }


        public void Copy()
        {
            if (MapView.NodeTextEditor.IsTextEditing)
                MapView.NodeTextEditor.CopyToClipboard();
            else
                ClipboardManager.Copy(tree.SelectedNodes);
        }

        public void Paste()
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
                    MapNode pasteLocation = tree.SelectedNodes[0];
                    ClipboardManager.Paste(pasteLocation);
                    if (pasteLocation.Folded) pasteLocation.Folded = false;
                    MapView.ResumeLayout();
                    MapView.RefreshChildNodePositions(tree.RootNode, pasteLocation.Pos);
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

                // 1) copy to system clipboard as text
                ClipboardManager.Copy(tree.SelectedNodes);

                // 2) detach nodes from tree
                MapView.SuspendLayout();
                var selNode = this.MapView.Tree.GetClosestUnselectedNode(MapView.SelectedNodes.Last);

                for (var i = this.MapView.SelectedNodes.Count - 1; i >= 0; i--)
                {
                    MapNode node = this.MapView.SelectedNodes[i];

                    node.Detach();
                    Debug.Assert(node.Detached, "Detached property is false for node just detached.");
                }

                MapView.ResumeLayout();
                MapView.RefreshChildNodePositions(tree.RootNode, NodePosition.Undefined);
                this.MapView.SelectedNodes.Add(selNode, false);
            }
        }

        public void SetMapViewBackColor(Color color)
        {
            MapView.Canvas.BackColor = color;
        }
    }
}
