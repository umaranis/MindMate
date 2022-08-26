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
using MindMate.View;
using MindMate.View.MapControls;

namespace MindMate.Controller
{
    /// <summary>
    /// Status Bar Controlller
    /// </summary>
    public class StatusBarCtrl
    {
        protected IStatusBar statusBar;

        public StatusBarCtrl(IStatusBar statusBar, PersistenceManager persistenceManager)
        {
            this.statusBar = statusBar;

            persistenceManager.CurrentTreeChanged += PersistenceManager_CurrentTreeChanged;
        }

        #region Hook up with Tree change events

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

        #endregion Hook up with Tree change events

        public void UpdateStatusBarForNode(SelectedNodes nodes)
        {
            if (nodes.Count != 1)
            {
                statusBar.UpdateText(0, nodes.Count + " nodes selected");
                statusBar.UpdateText(1, "");
                statusBar.UpdateText(2, "");
            }
            else
            {
                MapNode node = nodes.First;
                if (node.Link != null && node.GetLinkType() == NodeLinkType.MindMapNode)
                {
                    statusBar.UpdateText(0,
                        node.Tree.RootNode.Find( //TODO: Rather than traversing through the tree, a more efficient approach should be used
                            n => node.Link.Substring(1) == n.Id
                        ).Text + " (Internal Link)");
                }
                else
                {
                    if (node.Link != null)
                        statusBar.UpdateText(0, node.Link);
                    else
                        statusBar.UpdateText(0, " "); //don't set it to null or "", somehow it triggers mouse move event on MapViewPanel
                }

                statusBar.UpdateText(1, "Modified: " + node.Modified.ToString());
                statusBar.UpdateText(2, "Created: " + node.Created.ToString());
            }
        }


        public void SetStatusUpdate(string error)
        {
            statusBar.UpdateText(0, error);
        }


                
    }


}