using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;

namespace MindMate.View.MapControls.Interacting
{
    public abstract class BaseTraverser : ITraverser
    {
        /// <summary>
        /// Select the node below the currently selected node.
        /// If expandSelection is true, then adds the node below to the list of currently selected nodes.
        /// If expandSelection is true and node below is already selected, then 'deselects' the current node.
        /// </summary>
        public virtual void TraverseDown(MapTree tree, bool expandSelection)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null || node.Parent == null) return;

            if (node.Next != null)
            {
                if (!tree.SelectedNodes.Contains(node.Next))
                {
                    tree.SelectedNodes.Add(node.Next, expandSelection);
                }
                else if (expandSelection)
                {
                    tree.SelectedNodes.Remove(node);
                }
                else
                {
                    tree.SelectedNodes.Add(node.Next);
                }
            }
            else if (node.Parent.Next != null && node.Parent.Next.FirstChild != null && !node.Parent.Next.Folded)
            {
                if (!tree.SelectedNodes.Contains(node.Parent.Next.FirstChild))
                {
                    tree.SelectedNodes.Add(node.Parent.Next.FirstChild, expandSelection);
                }
                else if (expandSelection)
                {
                    tree.SelectedNodes.Remove(node);
                }
                else
                {
                    tree.SelectedNodes.Add(node.Parent.Next.FirstChild);
                }
            }
            else if (!expandSelection)
            {
                tree.SelectedNodes.Add(node);
            }
        }

        /// <summary>
        /// Select the node above the currently selected node.
        /// If expandSelection is true, then adds the node above to the list of currently selected nodes.
        /// If expandSelection is true and node above is already selected, then 'deselects' the current node.
        /// </summary>
        /// <param name="expandSelection"></param>
        public virtual void TraverseUp(MapTree tree, bool expandSelection)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null || node.Parent == null) return;

            if (node.Previous != null)
            {
                if (!tree.SelectedNodes.Contains(node.Previous))
                {
                    tree.SelectedNodes.Add(node.Previous, expandSelection); //select or expand selection
                }
                else if (expandSelection)
                {
                    tree.SelectedNodes.Remove(node);//reduce selection
                }
                else
                {
                    tree.SelectedNodes.Add(node.Previous);//clear selection and select previous
                }
            }
            else if (node.Parent.Previous != null && node.Parent.Previous.LastChild != null && !node.Parent.Previous.Folded)
            {
                if (!tree.SelectedNodes.Contains(node.Parent.Previous.LastChild))
                {
                    tree.SelectedNodes.Add(node.Parent.Previous.LastChild, expandSelection);
                }
                else if (expandSelection)
                {
                    tree.SelectedNodes.Remove(node);
                }
                else
                {
                    tree.SelectedNodes.Add(node.Parent.Previous.LastChild);
                }
            }
            else if (!expandSelection)
            {
                tree.SelectedNodes.Add(node);
            }
        }

        public virtual void TraverseLeft(MapTree tree)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null) return;

            if (node.Pos == NodePosition.Right)
            {
                tree.SelectedNodes.Add(node.Parent, false);
            }
            else
            {
                if (node.Folded && node.Pos != NodePosition.Root)
                {
                    node.Folded = false;
                    return;
                }

                MapNode tmpNode = NodeView.GetNodeView(node).GetLastSelectedChild(NodePosition.Left);
                if (tmpNode == null)
                {
                    return;
                }

                tree.SelectedNodes.Add(tmpNode, false);
            }
        }

        /// <summary>
        /// Select node on right or unfold
        /// </summary>
        /// <param name="tree"></param>
        /// 
        public virtual void TraverseRight(MapTree tree)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null) return;

            if (node.Pos == NodePosition.Left)
            {
                tree.SelectedNodes.Add(node.Parent, false);
            }
            else
            {
                if (node.Folded && node.Pos != NodePosition.Root)
                {
                    node.Folded = false;
                    return;
                }

                MapNode tmpNode = NodeView.GetNodeView(node).GetLastSelectedChild(NodePosition.Right);
                if (tmpNode == null)
                {
                    return;
                }

                tree.SelectedNodes.Add(tmpNode, false);
            }
        }
    }
}
