using MindMate.Model;
using MindMate.Modules.Logging;
using MindMate.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Model
{
    public static class ClipboardManager
    {
        private static readonly List<MapNode> internalClipboard = new List<MapNode>();

        public const string MindMateTextFormat = "MindMateText";

        public static event Action StatusChanged;

        private static bool hasCutNode;
        /// <summary>
        /// Clipboard contains cut node (detached node)
        /// </summary>
        public static bool HasCutNode
        {
            get
            {
                return internalClipboard.Count > 0 && hasCutNode;
            }
        }

        public static bool CanPaste
        {
            get
            {
                return Clipboard.ContainsData(MindMateTextFormat) || 
                       Clipboard.ContainsText() ||
                       Clipboard.ContainsFileDropList() ||
                       Clipboard.ContainsImage();
            }
        }

        public static void Copy(SelectedNodes nodes)
        {
            if (nodes.Count > 0)
            {
                internalClipboard.Clear();

                StringBuilder str = new StringBuilder();
                MapTextSerializer serializer = new MapTextSerializer();
                
                bool[] exclude = nodes.ExcludeNodesAlreadyPartOfHierarchy();
                for (int i = 0; i < nodes.Count; i++ )
                {
                    if (!exclude[i])
                    {
                        internalClipboard.Add(nodes[i].CloneAsDetached());
                        serializer.Serialize(nodes[i], str);
                    }
                }                
                
                var cbData = new MindMateTextDataObject();
                cbData.SetData(str.ToString());
                Clipboard.SetDataObject(cbData);
                //Clipboard.SetText(str.ToString(), TextDataFormat.Text);

                hasCutNode = false;

                if(StatusChanged != null) { StatusChanged(); }
            }
        }

        public static void Cut(SelectedNodes nodes)
        {
            if(nodes.Count > 0)
            {
                internalClipboard.Clear();

                StringBuilder str = new StringBuilder();
                MapTextSerializer serializer = new MapTextSerializer();

                bool[] exclude = nodes.ExcludeNodesAlreadyPartOfHierarchy();
                for (int i = 0; i < nodes.Count; i++)
                {
                    if(!exclude[i])
                    {
                        internalClipboard.Add(nodes[i]);
                    }
                }
                internalClipboard.ForEach(n =>
                    {
                        serializer.Serialize(n, str);
                        n.Detach();
                    });
                

                var cbData = new MindMateTextDataObject();
                cbData.SetData(str.ToString());
                Clipboard.SetDataObject(cbData);

                hasCutNode = true;
                if (StatusChanged != null) { StatusChanged(); }
            }
        }

        public static void Paste(MapNode pasteLocation, bool asText = false, bool pasteFileAsImage = false)
        {
            if(Clipboard.ContainsData(MindMateTextFormat))
            {
                foreach(MapNode node in internalClipboard)
                {
                    if(asText)
                    {
                        new MapNode(pasteLocation, node.Text);
                    }
                    else
                    {
                        node.CloneAsDetached().AttachTo(pasteLocation);
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
                        if (asText)
                        {
                            return new MapNode(parent, text);
                        }
                        else
                        {
                            string tempLink = link; // add link to source website in case text itself is not a URL (if text is URL, link to it)
                            if (text.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                                || text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                                tempLink = text;
                            return new MapNode(parent, text) { Link = tempLink };
                        }
                    });

            }
            else if (Clipboard.ContainsImage())
            {
                PasteImage(pasteLocation);
            }
            else if(Clipboard.ContainsFileDropList())
            {
                if (pasteFileAsImage)
                    PasteFileListAsImage(pasteLocation);
                else
                    PasteFileDropList(pasteLocation, asText);
            }
            
            hasCutNode = false;
            if (StatusChanged != null) { StatusChanged(); }

            if (pasteLocation.Folded) { pasteLocation.Folded = false; }
        }

        private static void PasteFileListAsImage(MapNode pasteLocation)
        {
            var fileList = Clipboard.GetFileDropList();
            Image image;
            if (fileList.Count == 1 && !pasteLocation.HasImage && ImageHelper.GetImageFromFile(fileList[0], out image))
            {
                pasteLocation.InsertImage(image, true);                
            }
            else
            {
                foreach (string file in fileList)
                {
                    if (ImageHelper.GetImageFromFile(file, out image))
                    {
                        MapNode n = new MapNode(pasteLocation, String.Empty);
                        n.InsertImage(image, true);
                    }
                    else
                    {
                        MapNode n = new MapNode(pasteLocation, file);
                        n.Link = file;
                    }
                }
            }
        }

        private static void PasteFileDropList(MapNode pasteLocation, bool asText = false)
        {
            var fileList = Clipboard.GetFileDropList();
            foreach(string file in fileList)
            {
                MapNode n = new MapNode(pasteLocation, file);
                if (!asText) { n.Link = file; }
            }
        }

        private static void PasteImage(MapNode pasteLocation)
        {
            new MapNode(pasteLocation, "").InsertImage(Clipboard.GetImage(), true);            
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
                    int j = text.IndexOf('\r', i);
                    if (j == -1) j = text.IndexOf('\n', i);
                    if (j > 0)
                    {
                        text = text.Substring(i, j - i);
                        return text;
                    }
                }
            }

            return null;            
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
