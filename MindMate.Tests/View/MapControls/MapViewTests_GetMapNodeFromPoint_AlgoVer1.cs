//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MindMate.Model;
//using MindMate.View.MapControls;
//using System.Drawing;
//using MindMate.Tests.TestDouble;
//using System.Diagnostics;
//using System.Linq;

//namespace MindMate.Tests.View.MapControls
//{
//    public class MapViewTests_GetMapNodeFromPoint_AlgoVer1
//    {


//        /// <summary>
//        /// The first version (tried and tested)
//        /// </summary>
//        public static MapNode GetMapNodeFromPoint(System.Drawing.Point point, MapNode node)
//        {
//            float xdiff = 0, ydiff = 0;
//            if (node.NodeView != null)
//            {
//                //Debug.WriteLine("node " + node.Pos + node.Text);
//                xdiff = point.X - node.NodeView.Left;
//                ydiff = point.Y - node.NodeView.Top;
//                if (
//                    (xdiff > 0 && xdiff < node.NodeView.Width) &&
//                    (ydiff > 0 && ydiff < node.NodeView.Height))
//                {
//                    return node;
//                }

//                if (!node.Folded && node.HasChildren)
//                {
//                    if (
//                        (node.Pos == NodePosition.Right && xdiff > (node.NodeView.Width + MapView.HOR_MARGIN))
//                        ||
//                        (node.Pos == NodePosition.Left && xdiff < (-MapView.HOR_MARGIN))
//                        )
//                    {
//                        foreach (var cNode in node.ChildNodes)
//                        {
//                            MapNode tnode = GetMapNodeFromPoint(point, cNode);
//                            if (tnode != null)
//                            {
//                                return tnode;
//                            }
//                        }
//                    }
//                    else if (node.Pos == NodePosition.Root)
//                    {
//                        NodePosition posToProcess = NodePosition.Undefined;
//                        if (xdiff > (node.NodeView.Width + MindMate.View.MapControls.MapView.HOR_MARGIN))
//                        {
//                            posToProcess = NodePosition.Right;
//                        }
//                        else if (xdiff < (-MindMate.View.MapControls.MapView.HOR_MARGIN))
//                        {
//                            posToProcess = NodePosition.Left;
//                        }

//                        if (posToProcess != NodePosition.Undefined)
//                        {
//                            foreach (var cNode in node.ChildNodes)
//                            {
//                                if (cNode.Pos == posToProcess)
//                                {
//                                    var tNode = GetMapNodeFromPoint(point, cNode);
//                                    if (tNode != null)
//                                        return tNode;
//                                }
//                            }

//                        }


//                    }

//                }
//            }
//            return null;

//        }
//    }
//}
