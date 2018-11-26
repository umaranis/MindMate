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
        /// Search within current node and it's descendents only
        /// </summary>
        public bool SearchCurrentNode { get; set; }
        public List<string> Icons { get; set; } = new List<string>();
    }
}
