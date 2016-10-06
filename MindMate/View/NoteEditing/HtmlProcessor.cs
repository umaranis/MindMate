using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    /// <summary>
    /// It is used for processing pasted html into the NoteEditor.
    /// - Changes img src to point to new image protocol
    /// - Removes script blocks //TODO: not done yet
    /// </summary>
    public class HtmlProcessor
    {
        public const string NoteImageProtocol = "mm://";
        public const string NoteImageExtension = ".png";

        public HtmlProcessor(NoteEditor editor, Action<ImageSourceChange> imageSourceChanging)
        {
            ReplaceImageTags(editor.Document, imageSourceChanging);
            //RemoveScripts(editor.Document);
        }        
        
        private void ReplaceImageTags(HtmlDocument document, Action<ImageSourceChange> imageSourceChanging)
        {
            var elemCol = document.GetElementsByTagName("img");

            for (int i = 0; i < elemCol.Count; i++)
            {
                var elem = elemCol[i];
                var src = elem.GetAttribute("src");
                if (src.Length > 4 && src.Substring(0, 4).Equals("http", StringComparison.OrdinalIgnoreCase))
                {
                    ImageSourceChange imgSrcChg = new ImageSourceChange();
                    imgSrcChg.OriginalSrc = src;
                    imgSrcChg.NewInternalSrc = NoteImageProtocol + Guid.NewGuid().ToString() + NoteImageExtension;
                    imageSourceChanging(imgSrcChg); //should be done before making changes to Html
                    elem.SetAttribute("srcOrig", imgSrcChg.OriginalSrc);
                    elem.SetAttribute("src", imgSrcChg.NewInternalSrc);                    
                }
            }
        }

        //private void RemoveScripts(HtmlDocument document)
        //{
        //    Dictionary<string, int> hashtable = new Dictionary<string, int>();
        //    var elemCol = document.All;//.GetElementsByTagName("script");

        //    for (int i = 0; i < elemCol.Count; i++)
        //    {
        //        var elem = elemCol[i];
        //        //elem.OuterHtml = "";   
        //        try
        //        {
        //            int t = hashtable[elem.TagName];
        //            hashtable[elem.TagName] = ++t;
        //        }
        //        catch (Exception)
        //        {
        //            hashtable[elem.TagName] = 0;
        //        }
        //        if (elem.TagName.Equals("script", StringComparison.OrdinalIgnoreCase))
        //        {
        //            elem.OuterHtml = null;
        //        }
        //    }
        //}

        public struct ImageSourceChange
        {
            public string OriginalSrc;
            public string NewInternalSrc;
        }
    }
}
