using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.Model
{
    public class SearchTerm
    {
        public string Text { get; set; }
        public StringComparison StringComparison { get; set; }
        public bool ExcludeNote { get; set; }
        /// <summary>
        /// Search within selected nodes and their descendents only
        /// </summary>
        public bool SearchSelectedHierarchy { get; set; }
        public List<string> Icons { get; } = new List<string>();
        public bool MatchAllIcons { get; set; } = true;

        /// <summary>
        /// Search even if a single character is given
        /// </summary>
        public bool Force { get; set;  } = false;

        public bool IsEmpty => String.IsNullOrWhiteSpace(Text) && Icons.Count <= 0;

        public bool MatchNode(MapNode n)
        {
            if (Icons.Count != 0)
            {
                if (MatchAllIcons)
                {
                    if (!Icons.All(icon => n.Icons.Contains(icon))) return false;
                }
                else
                {
                    if (!Icons.Any(icon => n.Icons.Contains(icon))) return false;
                }
            }

            return
                n.Text.IndexOf(Text, StringComparison) >= 0
                || (n.HasNoteText && !ExcludeNote && n.NoteText.IndexOf(Text, StringComparison) >= 0)
                ;
        }
    }
}
