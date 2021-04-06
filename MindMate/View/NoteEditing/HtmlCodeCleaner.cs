using MindMate.Modules.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MindMate.View.NoteEditing
{
    public class HtmlCodeCleaner
    {
        public static void Clean(NoteEditor editor)
        {
            var doc = editor.Document;

                       
            foreach (var e in doc.All.Cast<System.Windows.Forms.HtmlElement>())
            {
                CleanStyleAttribute(e);
                ClearControls(e);
            }            
        }

        private static void CleanStyleAttribute(System.Windows.Forms.HtmlElement e)
        {
            try
            {
                var newStyle = new StringBuilder();
                if (e.Style != null)
                {
                    var matches = Regex.Matches(e.Style, @"(?<attr>[^:\s]*)\s*:\s*(?<val>(?:[^;&]*(?<html>&)?[^;&]*(?(html);(?<-html>)))+)(?:;|$)");

                    foreach (var m in matches.Cast<Match>())
                    {
                        if (e.TagName.Equals("DIV"))
                        {
                            if (
                                new[] { "width", "max-width", "min-width", "height" }.Contains(m.Groups[1].Value.ToLower())
                                |
                                m.Groups[1].Value.ToLower().StartsWith("padding")
                                |
                                m.Groups[1].Value.ToLower().StartsWith("margin")
                                )
                            {
                                continue;
                            }
                        }                        

                        if (m.Groups[1].Value.Equals("position", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        newStyle.Append(m.Value);
                    }

                    e.Style = newStyle.ToString();                    
                }
            }
            catch (Exception exp)
            {
                Log.Write($"[HtmlCodeCleaner] Error in cleaning style property: {exp.Message}");
            }
        }

        private static void ClearControls(System.Windows.Forms.HtmlElement e)
        {
            if(new[] { "BUTTON", "INPUT" }.Contains(e.TagName))
            {
                e.OuterHtml = "";
            }
        }


    }
}
