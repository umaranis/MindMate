/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using MindMate.View.MapControls;
using MindMate.View;
using System.Drawing;
using MindMate.View.NoteEditing;

namespace MindMate.Controller
{
    /// <summary>
    /// Note Window Controlller
    /// </summary>
    public class NoteCtrl
    {
        
        private NoteEditor editor;

        /// <summary>
        /// Ignore <see cref="MapTree.NodePropertyChanged"/> event when <see cref="MapNode.RichContentText"/> property is changed by <see cref="NoteCtrl"/> itself.
        /// </summary>
        private bool ignoreModelChange;
        
        public NoteCtrl(NoteEditor editor)
        {
            this.editor = editor;
            this.editor.BackColor = MetaModel.MetaModel.Instance.NoteEditorBackColor; //System.Drawing.Color.LightYellow;            
        }

        private MapTree tree;
        public MapTree MapTree
        {
            get { return tree; }
            set
            {
                Unregister();
                tree = value;
                Register();
            }
        }


        /// <summary>
        /// Only one MapTree should be registered at a time
        /// </summary>
        /// <param name="tree"></param>
        private void Register()
        {
            if (tree != null)
            {
                
                if (editor.DocumentReady)
                {
                    MapView_nodeSelected(tree.SelectedNodes.First, tree.SelectedNodes); // setup the NoteEditor for already selected node
                    editor.Document.Body.LostFocus += editor_LostFocus; // setup editor lost focus event
                }
                else // same as above block in case the document is not ready yet
                {
                    editor.Ready += (obj) => { 
                        if (tree.SelectedNodes.Count == 1) MapView_nodeSelected(tree.SelectedNodes.First, tree.SelectedNodes); 
                        editor.Document.Body.LostFocus += editor_LostFocus;
                    };
                }

                // events for nodes selected in future
                tree.SelectedNodes.NodeSelected += MapView_nodeSelected;
                tree.SelectedNodes.NodeDeselected += MapView_nodeDeselected;
                // event for Node's Rich Content change (required where Note content is changed outside of Note window)
                tree.NodePropertyChanged += Tree_NodePropertyChanged;
                
            }            
        }
        

        void editor_LostFocus(object sender, EventArgs e)
        {
            UpdateNodeFromEditor();
        }
        

        private void Unregister()
        {
            if (tree != null)
            {
                tree.SelectedNodes.NodeSelected -= MapView_nodeSelected;
                tree.SelectedNodes.NodeDeselected -= MapView_nodeDeselected;
                tree.NodePropertyChanged -= Tree_NodePropertyChanged;
            }
        }

        void MapView_nodeSelected(Model.MapNode node, SelectedNodes selectedNodes)
        {
            if (selectedNodes.First.RichContentType == NodeRichContentType.NOTE &&
                selectedNodes.Count == 1)
            {
                editor.Enabled = true;
                editor.HTML = selectedNodes.First.RichContentText;
                editor.ClearUndoStack();

            }
            else if(selectedNodes.Count > 1)
            {
                editor.Enabled = false;
                editor.Clear();
                editor.ClearUndoStack();
            }
            else if (!editor.Empty) 
            {
                editor.Enabled = true;
                editor.Clear();
                editor.ClearUndoStack();
            }
        }

        void MapView_nodeDeselected(MapNode node, SelectedNodes selectedNodes) 
        {
            UpdateNodeFromEditor(node);

            if (selectedNodes.Count == 1)
            {
                editor.Enabled = true;
                MapView_nodeSelected(selectedNodes.First, selectedNodes);
            }
        }

        private void UpdateNodeFromEditor(MapNode node)
        {
            if (editor.Dirty)
            {
                ignoreModelChange = true;
                if (!editor.Empty)
                {
                    node.RichContentType = NodeRichContentType.NOTE;
                    node.RichContentText = editor.HTML;
                }
                else
                {
                    node.RichContentType = NodeRichContentType.NONE;
                    node.RichContentText = null;
                }
                editor.Dirty = false;
                ignoreModelChange = false;
            }
        }

        public void UpdateNodeFromEditor()
        {
            if (tree.SelectedNodes.Count == 1)
                UpdateNodeFromEditor(tree.SelectedNodes.First);
        }

        private void Tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            if (ignoreModelChange) return;

            if (node.Selected)
            {
                MapView_nodeSelected(node, node.Tree.SelectedNodes);
            }
        }

        public void SetNoteEditorBackColor(Color color)
        {
            editor.BackColor = color;
        }

    }
}