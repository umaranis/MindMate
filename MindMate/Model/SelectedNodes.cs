/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Model
{
    /// <summary>
    /// Maintain a list of currently selected nodes
    /// </summary>
    public class SelectedNodes
    {
        // list of currently selected nodes
        private System.Collections.Generic.List<MapNode> selectedNodes;

        public delegate void NodeSelectedDelegate(MapNode node, SelectedNodes selectedNodes);
        public event NodeSelectedDelegate NodeSelected = delegate { };

        public delegate void NodeDeselectedDelegate(MapNode node, SelectedNodes selectedNodes);
        public event NodeDeselectedDelegate NodeDeselected = delegate { };


        public SelectedNodes()
        {
            selectedNodes = new List<MapNode>();
        }
        
        public MapNode First
        {
            get
            {
                if (selectedNodes.Count > 0)
                    return selectedNodes[0];
                else
                    return null;
            }
        }

        public MapNode Last
        {
            get
            {
                if (selectedNodes.Count > 0)
                    return selectedNodes[selectedNodes.Count - 1];
                else
                    return null;
            }
        }

        public int Count
        {
            get
            {
                return selectedNodes.Count;
            }
        }

        public MapNode this [int index]
        {
            get
            {
                return selectedNodes[index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">if node is null, nothing happens</param>
        /// <param name="expandSelection"></param>
        public void Add(MapNode node, bool expandSelection = false)
        {
            System.Diagnostics.Debug.Assert(node != null, "node parameter is null.");
            if (node == null) return;

            if(selectedNodes.Contains(node)) // node already selected
            {
                if(!expandSelection) // deselect other nodes, if expandSelection is false
                {
                    for(int i = selectedNodes.Count - 1; i >= 0; i--)
                    {
                        if (selectedNodes[i] != node)
                        {
                            RemoveAt(i);
                        }
                    }
                }
            }
            else // node not already selected
            {
                if (!expandSelection)
                {
                    this.Clear();
                }

                this.selectedNodes.Add(node);

                if (node.Parent != null)
                {
                    node.Parent.NodeView.LastSelectedChild = node;
                }

                NodeSelected(node, this);

            }
            
        }

        public void Clear()
        {
            for (int i = selectedNodes.Count - 1; i >= 0; i--)
            {
                this.RemoveAt(i);
            }
        }

        public void Remove(MapNode node)
        {
            bool success = selectedNodes.Remove(node);

            if (success)
            {
                NodeDeselected(node, this);
            }

        }

        public void RemoveAt(int index)
        {
            MapNode node = selectedNodes[index];
            selectedNodes.RemoveAt(index);

            NodeDeselected(node, this);
        }

        public bool Contains(MapNode node)
        {
            return selectedNodes.Contains(node);            
        }
        

    }
}
