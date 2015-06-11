using MindMate.Model;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Model
{
    static class ClipboardManager
    {
        private static List<MapNode> internalClipboard = new List<MapNode>();

        public const string MindMateTextFormat = "MindMateText";

        /// <summary>
        /// Clipboard contains cut node (detached node)
        /// </summary>
        public static bool HasCutNode
        {
            get
            {
                return internalClipboard.Count > 0 && internalClipboard[0].Detached;
            }
        }

        public static bool CanPaste
        {
            get
            {
                return Clipboard.ContainsData(MindMateTextFormat) || 
                       Clipboard.ContainsText() ||
                       Clipboard.ContainsFileDropList();
            }
        }

        public static void Copy(SelectedNodes nodes)
        {
            if (nodes.Count > 0)
            {
                internalClipboard.Clear();

                StringBuilder str = new StringBuilder();
                MapTextSerializer serializer = new MapTextSerializer();
                if (nodes.Count > 1)
                {
                    bool[] exclude = ExcludeNodesAlreadyPartOfHierarchy(nodes);
                    for (int i = 0; i < nodes.Count; i++ )
                    {
                        if (!exclude[i])
                        {
                            internalClipboard.Add(nodes[i]);
                            serializer.Serialize(nodes[i], str);
                        }
                    }

                }
                else if (nodes.Count == 1)
                {
                    internalClipboard.Add(nodes[0]);
                    serializer.Serialize(nodes[0], str);
                }

                var cbData = new MindMateTextDataObject();
                cbData.SetData(str.ToString());
                Clipboard.SetDataObject(cbData);
                //Clipboard.SetText(str.ToString(), TextDataFormat.Text);
            }
        }

        public static void Paste(MapNode pasteLocation)
        {
            if(Clipboard.ContainsData(MindMateTextFormat))
            {
                foreach(MapNode node in internalClipboard)
                {
                    if (node.Detached)                                  // cut & paste (detached nodes are attached)
                    {
                        node.AttachTo(pasteLocation);
                    }
                    else                                                // copy & paste (nodes are copied)   
                    {
                        node.Clone().AttachTo(pasteLocation);
                    }
                }
            }
            else if(Clipboard.ContainsText())
            {
                string link = GetBrowserSourceLink();
                MapTextSerializer serializer = new MapTextSerializer();

                serializer.Deserialize(Clipboard.GetText(TextDataFormat.Text), pasteLocation, 
                    (parent, text) =>
                    {
                        string tempLink = link;
                        if (text.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                            || text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                            tempLink = text;
                        return new MapNode(parent, text) { Link = tempLink };
                    });

            }
            else if(Clipboard.ContainsFileDropList())
            {
                PasteFileDropList(pasteLocation);
            }
        }

        private static void PasteFileDropList(MapNode pasteLocation)
        {
            var fileList = Clipboard.GetFileDropList();
            foreach(string file in fileList)
            {
                new MapNode(pasteLocation, file) { Link = file };
            }
        }

        private static string GetBrowserSourceLink()
        {
            if (Clipboard.ContainsText(TextDataFormat.Html))
            {
                string text = Clipboard.GetText(TextDataFormat.Html);
                int i = text.IndexOf("SourceURL:");                
                if(i > 0)
                {
                    i += 10;
                    int j = text.IndexOf((char)Keys.Enter, i);
                    text = text.Substring(i, j - i);
                    return text;                    
                }
            }

            return null;            
        }
        

        private static bool[] ExcludeNodesAlreadyPartOfHierarchy(SelectedNodes nodes)
        {
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

        class MindMateTextDataObject : IDataObject
        {
            private string text;
            
            public object GetData(Type format)
            {
                throw new NotImplementedException();
            }

            public object GetData(string format)
            {
                if(format == DataFormats.Text || format == MindMateTextFormat)
                {
                    return text;
                }

                return null;
            }

            public object GetData(string format, bool autoConvert)
            {
                if (format == DataFormats.Text || format == MindMateTextFormat)
                {
                    return text;
                }

                return null;
            }

            public bool GetDataPresent(Type format)
            {
                throw new NotImplementedException();
            }

            public bool GetDataPresent(string format)
            {
                return format == DataFormats.Text || format == MindMateTextFormat;
            }

            public bool GetDataPresent(string format, bool autoConvert)
            {
                return format == DataFormats.Text || format == MindMateTextFormat;
            }

            public string[] GetFormats()
            {
                return new string [] { DataFormats.Text, MindMateTextFormat};
            }

            public string[] GetFormats(bool autoConvert)
            {
                return new string[] { DataFormats.Text, MindMateTextFormat };
            }

            public void SetData(object data)
            {
                text = (string)data;
            }

            public void SetData(Type format, object data)
            {
                throw new NotImplementedException();
            }

            public void SetData(string format, object data)
            {
                throw new NotImplementedException();
            }

            public void SetData(string format, bool autoConvert, object data)
            {
                throw new NotImplementedException();
            }
        }
                
    }
}
