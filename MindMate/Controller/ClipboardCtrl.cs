using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Controller
{
    static class ClipboardCtrl
    {
        public static void Copy(MapTree tree)
        {
            if (tree.SelectedNodes.Count > 0)
            {
                StringBuilder str = new StringBuilder();
                MapTextSerializer serializer = new MapTextSerializer();
                if (tree.SelectedNodes.Count > 1)
                {
                    bool[] exclude = ExcludeNodesAlreadyPartOfHierarchy(tree);
                    for (int i = 0; i < tree.SelectedNodes.Count; i++ )
                    {
                        if(!exclude[i])
                            serializer.Serialize(tree.SelectedNodes[i], str);
                    }

                }
                else if (tree.SelectedNodes.Count == 1)
                {
                    serializer.Serialize(tree.SelectedNodes[0], str);
                }

                Clipboard.SetText(str.ToString(), TextDataFormat.Text);
            }
        }

        private static bool[] ExcludeNodesAlreadyPartOfHierarchy(MapTree tree)
        {
            int[] depth = new int[tree.SelectedNodes.Count];
            bool[] exclude = new bool[tree.SelectedNodes.Count]; // default value is false

            for (int i = 0; i < tree.SelectedNodes.Count; i++)
            {
                depth[i] = tree.SelectedNodes[i].GetNodeDepth();
            }

            for (int i = 0; i < tree.SelectedNodes.Count; i++)
            {
                if (exclude[i]) continue;

                MapNode node1 = tree.SelectedNodes[i];

                for (int j = i + 1; j < tree.SelectedNodes.Count; j++)
                {
                    MapNode node2 = tree.SelectedNodes[j];

                    if (depth[i] == depth[j] || exclude[j])
                    {
                        continue;
                    }
                    else if (depth[i] < depth[j] && node2.isDescendent(node1))
                    {
                        exclude[j] = true;
                    }
                    else if (node1.isDescendent(node2))
                    {
                        exclude[i] = true;
                    }
                }
            }

            return exclude;
        }
    }
}
