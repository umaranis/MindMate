/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindMate.Model;
using MindMate.Serialization;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Status Bar Controlller
    /// </summary>
    public abstract class StatusBarCtrl
    {
        protected object statusBar;

        public StatusBarCtrl(object statusBar, PersistenceManager persistenceManager)
        {
            this.statusBar = statusBar;

            persistenceManager.CurrentTreeChanged += PersistenceManager_CurrentTreeChanged;
        }

        private void PersistenceManager_CurrentTreeChanged(PersistenceManager manager, PersistentTree oldTree, PersistentTree newTree)
        {
            if (oldTree != null)
            {
                Unregister(oldTree);
            }
            if (newTree != null)
            {
                Register(newTree);
            }
        }

        private void Register(MapTree tree)
        {
            tree.SelectedNodes.NodeSelected += MapView_nodeSelected;
            tree.SelectedNodes.NodeDeselected += MapView_nodeDeselected;

            tree.NodePropertyChanged += MapNode_NodePropertyChanged;
            tree.TreeStructureChanged += MapNode_TreeStructureChanged;
            tree.IconChanged += MapNode_IconChanged;
        }

        private void Unregister(MapTree tree)
        {
            tree.SelectedNodes.NodeSelected -= MapView_nodeSelected;
            tree.SelectedNodes.NodeDeselected -= MapView_nodeDeselected;

            tree.NodePropertyChanged -= MapNode_NodePropertyChanged;
            tree.TreeStructureChanged -= MapNode_TreeStructureChanged;
            tree.IconChanged -= MapNode_IconChanged;
        }


        void MapNode_IconChanged(MapNode node, IconChangedEventArgs arg)
        {
            UpdateStatusBarForNode(node.Tree.SelectedNodes);
        }

        void MapNode_TreeStructureChanged(MapNode node, TreeStructureChangedEventArgs arg)
        {
            UpdateStatusBarForNode(node.Tree.SelectedNodes);
        }

        void MapNode_NodePropertyChanged(MapNode node, NodePropertyChangedEventArgs arg)
        {
            UpdateStatusBarForNode(node.Tree.SelectedNodes);
        }

        
        void MapView_nodeSelected(Model.MapNode node, SelectedNodes selectedNodes)
        {
            UpdateStatusBarForNode(selectedNodes);
        }

        void MapView_nodeDeselected(MapNode node, SelectedNodes selectedNodes) 
        {
            UpdateStatusBarForNode(selectedNodes);
        }

        abstract public void UpdateStatusBarForNode(SelectedNodes nodes);


        abstract public void SetStatusUpdate(string error);


                
    }


}