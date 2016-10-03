using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// It is used for pasting html into the NoteEditor.
    /// - Changes img src to point to new image protocol
    /// - Removes script blocks
    /// </summary>
    public class HtmlPreProcessor
    {
        public const string NoteImageProtocol = "mm:";
        public const string NoteImageExtension = ".png";

        public HtmlPreProcessor(string html)
        {
            Changed = false;
            ReplaceImageTags(html);
        }

        public string ProcessedHtml
        {
            get;
            private set;
        }  
        
        public bool Changed
        {
            get;
            private set;
        }      

        private List<ImageSourceChange> imageSourceChanges = new List<ImageSourceChange>();
        public IEnumerable<ImageSourceChange> ImageSourceChanges
        {
            get
            {
                return imageSourceChanges;
            }            
        }

        private void ReplaceImageTags(string html)
        {
            var parser = new HtmlAgilityPack.HtmlDocument();
            parser.LoadHtml(html);
            HtmlAgilityPack.HtmlNodeCollection nodeCol = parser.DocumentNode.SelectNodes("//img");
            if (nodeCol != null)
            {
                foreach (var node in nodeCol)
                {
                    var src = node.Attributes["src"];
                    if (src.Value.StartsWith("http"))
                    {
                        ImageSourceChange imgSrcChg = new ImageSourceChange();
                        imgSrcChg.OriginalSrc = src.Value;
                        imgSrcChg.NewInternalSrc = NoteImageProtocol + Guid.NewGuid().ToString() + NoteImageExtension;
                        node.Attributes.Add("srcOrig", imgSrcChg.OriginalSrc);
                        src.Value = imgSrcChg.NewInternalSrc;
                        imageSourceChanges.Add(imgSrcChg);
                    }
                }

                Changed = true;
                ProcessedHtml = parser.DocumentNode.InnerHtml;
            }
            
        }

        public struct ImageSourceChange
        {
            public string OriginalSrc;
            public string NewInternalSrc;
        }
    }
}
