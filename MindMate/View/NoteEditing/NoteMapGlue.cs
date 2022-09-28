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
using MindMate.Serialization;
using MindMate.View.NoteEditing;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// Note Window Controlller
    /// </summary>
    public class NoteMapGlue
    {
        
        private readonly NoteEditor editor;
        public NoteEditor Editor
        {
            get
            {
                return editor;
            }
        }


        public MapNode CurrentMapNpde { get; private set; }

        /// <summary>
        /// Ignore <see cref="MapTree.NodePropertyChanged"/> event when <see cref="MapNode.NoteText"/> property is changed by <see cref="NoteMapGlue"/> itself.
        /// </summary>
        private bool ignoreModelChange;        

        public NoteMapGlue(NoteEditor editor, PersistenceManager manager)
        {
            this.editor = editor;
            editor.BackColor = manager.CurrentTree?.NoteBackColor ?? MetaModel.MetaModel.Instance.NoteEditorBackColor;
            editor.ForeColor = manager.CurrentTree?.NoteForeColor ?? TreeFormat.DefaultNoteEditorForeColor;
            new NoteEditorContextMenu(editor);
                   
            manager.CurrentTreeChanged += Manager_CurrentTreeChanged;            
            if (manager.CurrentTree != null) { Register(manager.CurrentTree); }

            if (editor.DocumentReady)
            {
                editor.Document.Body.LostFocus += editor_LostFocus; // setup editor lost focus event
            }
            else // same as above block in case the document is not ready yet
            {
                editor.Ready += (obj) => {
                    editor.Document.Body.LostFocus += editor_LostFocus;
                };
            }
        }

        #region Setting & Updating back Content

        /// <summary>
        /// Based on SelectedNodes status, set NoteEditor content. If multiple selection, NoteEditor is cleared.
        /// </summary>
        /// <param name="selectedNodes"></param>
        private void SetEditorContent(SelectedNodes selectedNodes)
        {
            if (selectedNodes.Count == 1)
            {
                CurrentMapNpde = selectedNodes.First;
                editor.Enabled = true;
                if (selectedNodes.First.HasNoteText)
                {
                    editor.HTML = this.CurrentMapNpde.NoteText;
                }
                else
                {
                    editor.Clear();
                }
                editor.ClearUndoStack();

            }
            else if (selectedNodes.Count > 1)
            {
                CurrentMapNpde = null;
                editor.Enabled = false;
                editor.Clear();
                editor.ClearUndoStack();
            }
            else if (!editor.Empty)
            {
                CurrentMapNpde = null;
                editor.Enabled = true;
                editor.Clear();
                editor.ClearUndoStack();
            }
        }

        /// <summary>
        /// Updates the MapNode from the Note Editor if the editor is dirty.
        /// Uses change manager to enable undo.
        /// </summary>
        public void UpdateNodeFromEditor()
        {
            if (editor.Dirty && CurrentMapNpde != null)
            {
                CurrentMapNpde.Tree.ChangeManager.StartBatch("Change Note");
                ignoreModelChange = true;
                if (!editor.Empty)
                {
                    CurrentMapNpde.NoteText = editor.HTML;
                }
                else
                {
                    CurrentMapNpde.NoteText = null;
                }
                editor.Dirty = false;
                ignoreModelChange = false;
                CurrentMapNpde.Tree.ChangeManager.EndBatch();
            }
        }

        #endregion

        #region Registering for Change Events

        private void Manager_CurrentTreeChanged(PersistenceManager manager, PersistentTree oldTree, PersistentTree newTree)
        {
            if (oldTree != null)
            {
                UpdateNodeFromEditor();
                Unregister(oldTree);
            }
            if (newTree != null)
            {
                Register(newTree);
                editor.BackColor = newTree.NoteBackColor;
                editor.ForeColor = newTree.NoteForeColor;
            }            
        }

        /// <summary>
        /// Only one MapTree should be registered at a time
        /// </summary>
        /// <param name="tree"></param>
        private void Register(MapTree tree)
        {
            if (tree != null)
            {
                
                if (editor.DocumentReady)
                {
                    SetEditorContent(tree.SelectedNodes); // setup the NoteEditor for already selected node
                }
                else // same as above block in case the document is not ready yet
                {
                    editor.Ready += (obj) => SetEditorContent(tree.SelectedNodes); 
                }

                // events for nodes selected in future
                tree.SelectedNodes.NodeSelected += Tree_NodeSelected;
                tree.SelectedNodes.NodeDeselected += Tree_NodeDeselected;
                // event for Node's Rich Content change (required where Note content is changed outside of Note window)
                tree.NodePropertyChanged += Tree_NodePropertyChanged;
                tree.TreeFormatChanged += Tree_TreeFormatChanged;
                
            }            
        }        

        private void Unregister(MapTree tree)
        {
            if (tree != null)
            {
                tree.SelectedNodes.NodeSelected -= Tree_NodeSelected;
                tree.SelectedNodes.NodeDeselected -= Tree_NodeDeselected;
                tree.NodePropertyChanged -= Tree_NodePropertyChanged;
                tree.TreeFormatChanged -= Tree_TreeFormatChanged;
            }
        }

        #endregion

        #region Change Events

        void Tree_NodeSelected(Model.MapNode node, SelectedNodes selectedNodes)
        {
            SetEditorContent(selectedNodes);
        }

        void Tree_NodeDeselected(MapNode node, SelectedNodes selectedNodes) 
        {
            UpdateNodeFromEditor();

            // in case deselection has resulted in 'selection of one node'
            SetEditorContent(selectedNodes);
        }

        /// <summary>
        /// Event for Node's Rich Content change (required where Note content is changed outside of Note window)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="e"></param>
        private void Tree_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs e)
        {
            if (ignoreModelChange) return;

            if (node.Selected && e.ChangedProperty == NodeProperties.NoteText)
            {
                SetEditorContent(node.Tree.SelectedNodes);
            }
        }

        private void Tree_TreeFormatChanged(MapTree tree, TreeDefaultFormatChangedEventArgs e)
        {
            switch(e.ChangeType)
            {
                case TreeFormatChange.NoteEditorBackColor:
                    editor.BackColor = tree.NoteBackColor;
                    break;
                case TreeFormatChange.NoteEditorForeColor:
                    editor.ForeColor = tree.NoteForeColor;
                    break;
            }
        }

        void editor_LostFocus(object sender, EventArgs e)
        {
            UpdateNodeFromEditor();
        }

        #endregion Change Events

    }
}