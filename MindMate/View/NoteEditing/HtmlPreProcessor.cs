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

        public HtmlPreProcessor(WebBrowser browser, string html)
        {
            var tempContainer = browser.Document.CreateElement("DIV");
            tempContainer.InnerHtml = html;
            ReplaceImageTags(tempContainer);
            RemoveScripts(tempContainer);

            ProcessedHtml = tempContainer.InnerHtml;
        }

        public string ProcessedHtml
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
        
        private void ReplaceImageTags(HtmlElement tempContainer)
        {
            var elemCol = tempContainer.GetElementsByTagName("img");

            for(int i=0; i < elemCol.Count; i++)
            {
                var elem = elemCol[i];
                var src = elem.GetAttribute("src");
                if (src.Length > 4 && src.Substring(0, 4).Equals("http", StringComparison.OrdinalIgnoreCase))
                {
                    ImageSourceChange imgSrcChg = new ImageSourceChange();
                    imgSrcChg.OriginalSrc = src;
                    imgSrcChg.NewInternalSrc = NoteImageProtocol + Guid.NewGuid().ToString() + NoteImageExtension;
                    elem.SetAttribute("srcOrig", imgSrcChg.OriginalSrc);
                    elem.SetAttribute("src", imgSrcChg.NewInternalSrc);
                    imageSourceChanges.Add(imgSrcChg);
                }
            }            
        }

        private void RemoveScripts(HtmlElement tempContainer)
        {
            Dictionary<string, int> hashtable = new Dictionary<string, int>();
            var elemCol = tempContainer.All;//.GetElementsByTagName("script");

            for (int i = 0; i < elemCol.Count; i++)
            {
                var elem = elemCol[i];
                //elem.OuterHtml = "";   
                try
                {
                    int t = hashtable[elem.TagName];
                    hashtable[elem.TagName] = ++t;
                }
                catch(Exception)
                {
                    hashtable[elem.TagName] = 0;
                }
                if(elem.TagName.Equals("script", StringComparison.OrdinalIgnoreCase))
                {
                    elem.OuterHtml = null;
                }             
            }
        }

        public struct ImageSourceChange
        {
            public string OriginalSrc;
            public string NewInternalSrc;
        }
    }
}
