/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.View.MapControls
{
    /// <summary>
    /// This an extension of MapView which enables editing of node through TextBox
    /// </summary>
    public class MapViewTextEditor
    {
        /// <summary>
        /// Size of EditBox for new node
        /// </summary>
        private const int TEXTBOX_DEFAULT_WIDTH = 100;
        /// <summary>
        /// Mininum Size of EditBox. Comes into play Node Text size is smaller than 50 units.
        /// </summary>
        private const int TEXTBOX_MIN_WIDTH = 50;
        /// <summary>
        /// Extra width of EditBox on top of node text size
        /// </summary>
        private const int TEXTBOX_PADDING = 10;

        public bool IsTextEditing { get; private set; }

        private TextBox editBox;
        private MapView mapView;
        
        public MapViewTextEditor(MapView mapView, Font font)
        {
            this.mapView = mapView;
            editBox = new TextBox();
            editBox.Font = font;
            editBox.Visible = false;
            mapView.Canvas.Controls.Add(editBox);

            editBox.PreviewKeyDown += editBox_PreviewKeyDown;
            editBox.LostFocus += new EventHandler(editBoxLostFocus);
            editBox.KeyDown += new KeyEventHandler(editBoxKeyDown);
            //increases the EditBox size as text is entered. Commented out as performance is affected with very large maps (also doesn't work nicely with left side nodes).
            //editBox.TextChanged += EditBox_TextChanged;                                          
        }

        private void EditBox_TextChanged(object sender, EventArgs e)
        {
            if (IsTextEditing)
            {
                SizeF s = Drawing.TextRenderer.MeasureText(editBox.Text, editBox.Font);
                editBox.Width = (int)s.Width + TEXTBOX_PADDING;
                IncreaseNodeTextSize(((MapNode)(editBox.Tag)).NodeView, (int)s.Width);
            }
        }

        void editBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 9)
                e.IsInputKey = true;
        }


        private void editBoxKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 9: //TAB key pressed
                    this.EndNodeEdit(true, true);
                    SendKeys.Send("{TAB}");
                    break;
                case 13: //Enter key pressed
                case 27: //Esc key pressed
                    this.EndNodeEdit(e.KeyValue == 13 ? true : false,
                        true // focus on map
                        );
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public ContextMenuStrip ContextMenu
        {
            get
            {
                return editBox.ContextMenuStrip;
            }
            set
            {
                editBox.ContextMenuStrip = value;
            }
        }

        public void BeginNodeEdit(NodeView nView, TextCursorPosition org)
        {
            if (!enabled) return;

            MapNode node = nView.Node;

            if (!node.HasChildren && nView.RecText.Width < TEXTBOX_DEFAULT_WIDTH)
            {
                IncreaseNodeTextSize(nView, TEXTBOX_DEFAULT_WIDTH);                
            }
            else if(nView.RecText.Width < TEXTBOX_MIN_WIDTH)
            {
                IncreaseNodeTextSize(nView, TEXTBOX_MIN_WIDTH);
            }
            else
            {
                IncreaseNodeTextSize(nView, (int)(nView.RecText.Width + TEXTBOX_PADDING));
            }

            mapView.AdjustLocationToShowNodeView(nView);

            this.editBox.Location = new Point(
                (int)nView.RecText.X,
                (int)nView.RecText.Y - 3
                );

            this.editBox.MinimumSize = new Size((int)nView.RecText.Width, (int)nView.RecText.Height);
            this.editBox.Size = this.editBox.MinimumSize;
                                                
            this.editBox.Text = node.Text;
            this.editBox.Tag = node;
            this.editBox.Visible = true;
            this.editBox.BringToFront();
            if (org == TextCursorPosition.End || org == TextCursorPosition.Undefined)
            {
                this.editBox.SelectionStart = node.Text.Length;
            }
            else if (org == TextCursorPosition.Start)
            {
                this.editBox.SelectionStart = 0;
            }
            this.editBox.SelectionLength = 0;

            this.editBox.Focus();

            this.IsTextEditing = true;

        }


        /// <summary>
        /// This method is only for ending inline textbox editing (usually used for short nodes i.e. single line)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="updateNode"></param>
        public void EndNodeEdit(bool updateNode, bool focusMap)
        {
            MapNode node = (MapNode)this.editBox.Tag;
            if (node == null) return;


            if (updateNode && editBox.CanUndo)
            {
                UpdateNodeText(node, this.editBox.Text);                
                mapView.AdjustLocationToShowNodeView(node.NodeView);    
            }
            else if(!updateNode && node.IsEmpty())
            {
                node.Tree.GetClosestUnselectedNode(node).Selected = true;
                node.DeleteNode();
            }
            else
            {
                ResetNodeTextSize(node.NodeView); //restore and clear any changes like increase in NodeView size
            }

            this.IsTextEditing = false;

            this.editBox.Text = null;
            this.editBox.Tag = null; // ensures that Lost Focus event doesn't call stopNodeEdit again as control is made unvisible
            this.editBox.Visible = false;
            
            if (focusMap) mapView.Canvas.Focus();
        }

        public void UpdateNodeText(MapNode node, string newText)
        {
            node.Text = newText;
        }

        private void editBoxLostFocus(object sender, EventArgs e)
        {
            if (((TextBox)sender).Tag != null) // this ensures that stopNodeEdit is not called again when Textbox loses focus during processing of stopNodeEdit. Textbox loses focus when control is made unvisible. stopNodeEdit clears tag from Textbox first.
                this.EndNodeEdit(true,
                    false // no need to focus on map, focus is already lost
                    );
        }

        private void IncreaseNodeTextSize(NodeView nView, int width)
        {
            MapNode node = nView.Node;

            nView.RefreshText(new SizeF(width, nView.RecText.Height));
            
            if (node == node.Tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
            mapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);

            mapView.Canvas.Invalidate();
        }

        private void ResetNodeTextSize(NodeView nView)
        {
            MapNode node = nView.Node;

            nView.RefreshText();

            if (node == node.Tree.RootNode) node.NodeView.RefreshPosition(node.NodeView.Left, node.NodeView.Top);
            mapView.RefreshChildNodePositions(node.Parent != null ? node.Parent : node, NodePosition.Undefined);

            mapView.Canvas.Invalidate();
        }

        #region EditBox Commands

        /// <summary>
        /// Copy selection to clipboard
        /// </summary>
        public void CopyToClipboard()
        {
            this.editBox.Copy();
        }

        /// <summary>
        /// Cut selection to clipboard
        /// </summary>
        public void CutToClipboard()
        {
            this.editBox.Cut();
        }

        public void PasteFromClipboard()
        {
            this.editBox.Paste();
        }

        internal void Undo()
        {
            this.editBox.Undo();
        }

        #endregion EditBox Commands
    }

}
