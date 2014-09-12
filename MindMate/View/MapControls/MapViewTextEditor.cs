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

        private const int TEXTBOX_DEFAULT_WIDTH = 150;
        public bool IsTextEditing = false;

        private TextBox editBox;
        private Control canvas;


        /// <summary>
        /// Called when node text is edited in the view.
        /// MapViewTextEditor doesn't update model. It is the responsibility of provided callback function to update model.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newText"></param>
        public delegate void TextUpdateCallBack(MapNode node, string newText);
        /// <summary>
        /// If this is null, it means that text editing is disabled.
        /// </summary>
        private TextUpdateCallBack textUpdateCallBack;

        public MapViewTextEditor(Control canvas, Font font)
        {
            this.canvas = canvas;
            editBox = new TextBox();
            editBox.Font = font;
            editBox.Visible = false;
            canvas.Controls.Add(editBox);

            editBox.LostFocus += new EventHandler(editBoxLostFocus);
            editBox.KeyDown += new KeyEventHandler(editBoxKeyDown);
        }

       
        public bool Enabled
        {
            get
            {
                return textUpdateCallBack != null;
            }
        }

        public void Enable(TextUpdateCallBack callback)
        {
            this.textUpdateCallBack = callback;
        }

        public void Disable()
        {
            this.textUpdateCallBack = null;
        }     

        

        public void BeginNodeEdit(NodeView nView, TextCursorPosition org)
        {
            if (textUpdateCallBack == null) return;

            MapNode node = nView.Node;

            this.editBox.Location = new Point(
                (int)nView.RecText.X,
                (int)nView.RecText.Y - 3
                );

            if (!node.HasChildren && node.Text == "")
            {
                this.editBox.Size = new Size(TEXTBOX_DEFAULT_WIDTH, (int)nView.RecText.Height);
                if (node.Pos == NodePosition.Left)
                {
                    this.editBox.Location = new Point(this.editBox.Location.X + (int)nView.RecText.Width - TEXTBOX_DEFAULT_WIDTH,
                        this.editBox.Location.Y);
                }
            }
            else
            {
                this.editBox.Size = new Size((int)nView.RecText.Width, (int)nView.RecText.Height);
            }

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


            if (updateNode)
            {
                textUpdateCallBack(node, this.editBox.Text);
                
            }
            this.IsTextEditing = false;

            this.editBox.Tag = null; // ensures that Lost Focus event doesn't call stopNodeEdit again as control is made unvisible
            this.editBox.Visible = false;
            if (focusMap) canvas.Focus();
            //this.Canvas.Invalidate();
        }

        private void editBoxLostFocus(object sender, EventArgs e)
        {
            if (((TextBox)sender).Tag != null) // this ensures that stopNodeEdit is not called again when Textbox loses focus during processing of stopNodeEdit. Textbox loses focus when control is made unvisible. stopNodeEdit clears tag from Textbox first.
                this.EndNodeEdit(true,
                    false // no need to focus on map, focus is already lost
                    );
        }


        private void editBoxKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
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

    }
    
}
