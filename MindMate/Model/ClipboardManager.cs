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

        public const string MindMateClipboardFormat = "MindMateContent";

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
                return Clipboard.ContainsData(MindMateClipboardFormat) || 
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
                foreach (var n in nodes.ExcludeNodesAlreadyPartOfHierarchy())
                {
                    internalClipboard.Add(n.CloneAsDetached());
                }                
                
                var cbData = new MindMateClipboardDataObject(internalClipboard);
                Clipboard.SetDataObject(cbData);

                hasCutNode = false;

                StatusChanged?.Invoke();
            }
        }

        public static void Cut(SelectedNodes nodes)
        {
            if(nodes.Count > 0)
            {
                internalClipboard.Clear();
                foreach (var n in nodes.ExcludeNodesAlreadyPartOfHierarchy())
                {
                   internalClipboard.Add(n);
                }
                internalClipboard.ForEach(n => n.Detach());
                

                var cbData = new MindMateClipboardDataObject(internalClipboard);
                Clipboard.SetDataObject(cbData);

                hasCutNode = true;
                StatusChanged?.Invoke();
            }
        }

        public static void Paste(MapNode pasteLocation, bool asText = false, bool pasteFileAsImage = false)
        {
            if(Clipboard.ContainsData(MindMateClipboardFormat))
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

                serializer.Deserialize(Clipboard.GetText(), pasteLocation, 
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
            StatusChanged?.Invoke();
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

        public class MindMateClipboardDataObject : IDataObject
        {

            private List<MapNode> mapNodes;
            public MindMateClipboardDataObject(List<MapNode> mapNodes)
            {
                this.mapNodes = mapNodes;
            }

            private string[] formats;

            private string text;
            
            public object GetData(Type format)
            {
                throw new NotImplementedException();
            }

            public object GetData(string format)
            {
                return GetData(format, true);
            }

            public object GetData(string format, bool autoConvert)
            {
                string[] formats = GetFormats();
                if (!formats.Contains(format)) return null;

                if (format == DataFormats.Text || format == MindMateClipboardFormat)
                {
                    if (text == null)
                    {
                        StringBuilder str = new StringBuilder();
                        MapTextSerializer serializer = new MapTextSerializer();
                        mapNodes.ForEach(n => serializer.Serialize(n, str));
                        text = str.ToString();
                    }
                    return text;
                }
                else if (format == DataFormats.Bitmap)
                {
                    return mapNodes.First(n => n.HasImage).GetImage();
                }

                return null;
            }

            public bool GetDataPresent(Type format)
            {
                throw new NotImplementedException();
            }

            public bool GetDataPresent(string format)
            {
                return GetDataPresent(format, true);
            }

            public bool GetDataPresent(string format, bool autoConvert)
            {
                return GetFormats().Contains(format);
            }

            public string[] GetFormats()
            {
                return GetFormats(true);
            }

            public string[] GetFormats(bool autoConvert)
            {
                if(formats == null)
                {
                    bool hasImage = mapNodes.Any(n => n.HasImage);
                    if (hasImage)
                    {
                        formats = new string[] { DataFormats.Text, MindMateClipboardFormat, DataFormats.Bitmap };
                    }
                    else
                    {
                        formats = new string[] { DataFormats.Text, MindMateClipboardFormat };
                    }
                }
                return formats;
            }

            public void SetData(object data)
            {
                throw new NotImplementedException();
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
