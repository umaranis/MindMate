using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View
{    
    public interface ISearchControl
    {
        event EventHandler SearchTermChanged;
        event EventHandler SearchResultSelected;        
        event EventHandler SearchResultAllSelected;

        SearchTerm CreateSearchTerm();

        void InvokeInUIThread(Action action);
        void ClearResults();

        void AddResult(MapNode node);

        IEnumerable<MapNode> Results { get; }

        MapNode SelectedResultMapNode { get; } 

    }
}