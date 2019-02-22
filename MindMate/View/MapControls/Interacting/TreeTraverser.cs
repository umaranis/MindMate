using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls.Interacting
{
    public class TreeTraverser : BaseTraverser
    {

        public static MapNode GetNodeBelow(MapNode node)
        {
            if (node.HasChildren && !node.Folded)
            {
                return node.FirstChild;
            }
            else if (node.Next != null)
            {
                return node.Next;
            }
            else 
            {
                var tmp = node.Parent;
                while(tmp != null)
                {
                    if(tmp.Next != null)
                    {
                        return tmp.Next;
                    }
                    tmp = tmp.Parent;
                }
            }

            return null;
        }

        public static MapNode GetNodeAbove(MapNode node)
        {
            var tmp = node.Previous;
            while (tmp != null) //once inside the loop, the only way out is through return statement
            {
                if (tmp.HasChildren && !tmp.Folded)
                {
                    tmp = tmp.LastChild;
                }
                else
                {
                    return tmp;
                }
            }

            return node.Parent;   
        }

        private void TraverseUpDownTo(MapTree tree, MapNode node, MapNode next, bool expandSelection)
        {
            if (next != null)
            {
                if (!tree.SelectedNodes.Contains(next))
                {
                    tree.SelectedNodes.Add(next, expandSelection);
                }
                else if (expandSelection)
                {
                    tree.SelectedNodes.Remove(node);
                }
                else
                {
                    tree.SelectedNodes.Add(next);
                }
            }
        }

        public override void TraverseDown(MapTree tree, bool expandSelection)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null) return;
            var next = GetNodeBelow(node);

            TraverseUpDownTo(tree, node, next, expandSelection);
        }
                
        public override void TraverseUp(MapTree tree, bool expandSelection)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null) return;
            var next = GetNodeAbove(node);

            TraverseUpDownTo(tree, node, next, expandSelection);
        }

        public override void TraverseLeft(MapTree tree)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null) return;

            tree.SelectedNodes.Add(node.Parent, false);
        }

        public override void TraverseRight(MapTree tree)
        {
            MapNode node = tree.SelectedNodes.Last;
            if (node == null) return;

            if (node.Folded && node.Pos != NodePosition.Root)
            {
                node.Folded = false;
                return;
            }

            MapNode tmpNode = node.FirstChild;
            if (tmpNode == null)
            {
                return;
            }

            tree.SelectedNodes.Add(tmpNode, false);
            
        }
    }
}
