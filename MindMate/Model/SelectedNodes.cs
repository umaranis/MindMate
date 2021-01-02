/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MindMate.Model
{
    /// <summary>
    /// Maintain a list of currently selected nodes.
    /// A selected node should always be visible (not hidden due to folding).
    /// </summary>
    public class SelectedNodes : IEnumerable<MapNode>
    {
        // list of currently selected nodes
        private readonly System.Collections.Generic.List<MapNode> selectedNodes;

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

        public bool IsEmpty { get { return Count == 0; } }

        public bool IsMultiple { get { return Count > 1; } }

        public MapNode this [int index]
        {
            get
            {
                return selectedNodes[index];
            }
        }        

        /// <summary>
        /// Nothing happens if the given node is the selected already.
        /// </summary>
        /// <param name="node">if node is null, nothing happens</param>
        /// <param name="expandSelection">multi-select</param>
        public void Add(MapNode node, bool expandSelection = false)
        {
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
                    node.ForEachAncestor(n =>
                        {
                            if (n.Folded) n.Folded = false;
                        });
                    node.Parent.LastSelectedChild = node;
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

        /// <summary>
        /// Deselects the node.
        /// Nothing happens if node is already deselected.
        /// </summary>
        /// <param name="node"></param>
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

        public IEnumerator<MapNode> GetEnumerator()
        {
            return selectedNodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return selectedNodes.GetEnumerator();
        }

        public IEnumerable<MapNode> ExcludeNodesAlreadyPartOfHierarchy()
        {
            SelectedNodes nodes = this;

            int[] depth = new int[nodes.Count];
            bool[] exclude = new bool[nodes.Count]; // default value is false

            for (int i = 0; i < nodes.Count; i++)
            {
                depth[i] = nodes[i].GetNodeDepth();
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                if (exclude[i]) continue;

                MapNode node1 = nodes[i];

                for (int j = i + 1; j < nodes.Count; j++)
                {
                    MapNode node2 = nodes[j];

                    if (depth[i] == depth[j] || exclude[j])
                    {
                        continue;
                    }
                    else if (depth[i] < depth[j] && node2.IsDescendent(node1))
                    {
                        exclude[j] = true;
                    }
                    else if (node1.IsDescendent(node2))
                    {
                        exclude[i] = true;
                    }
                }

                if (!exclude[i]) yield return node1;
            }

        }
    }
}
