using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using MindMate.View;

namespace MindMate.Controller
{
    /// <summary>
    /// Perform a search for matching map nodes using the search term.
    /// Search is performed in a separate thread.
    /// Ongoing search is cancelled if a new search is initiated.
    /// Using single-threaded Task Scheduler to ensure searches run in order and never in parallel.
    /// </summary>
    public class SearchController
    {
        private ISearchControl SearchControl;
        private int CurrentSearchID { get; set; }
        private Func<MapTree> GetCurrentMapTree;
        private Action<Action> ScheduleParallelTask;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchControl"></param>
        /// <param name="getCurrentMapTree">delegate which returns the current MapTree</param>
        /// <param name="scheduleParallelTask">This is function is used to schedule tasks to run in a separate thread. The function ensures that the tasks are run in order and never parallel.</param>
        public SearchController(ISearchControl searchControl, Func<MapTree> getCurrentMapTree, Action<Action> scheduleParallelTask)
        {
            GetCurrentMapTree = getCurrentMapTree;
            ScheduleParallelTask = scheduleParallelTask;
            this.SearchControl = searchControl;

            SearchControl.SearchResultSelected += (s, o) => SearchControl.SelectedResultMapNode.Selected = true;
            SearchControl.SearchTermChanged += (s, o) => Search();
            SearchControl.SearchResultAllSelected += SearchControl_SearchResultAllSelected;
        }        

        private void Search()
        {
            SearchTerm searchTerm = SearchControl.CreateSearchTerm();
            if (searchTerm.IsEmpty)
            {
                SearchControl.ClearResults();
                return;
            }
            int instanceID = ++CurrentSearchID;
            MapTree tree = GetCurrentMapTree();            
            ScheduleParallelTask(() =>
            {
                SearchControl.InvokeInUIThread(SearchControl.ClearResults);                
                foreach (var n in GetNodesToSearch(tree, searchTerm.SearchSelectedHierarchy))
                {
                    if (instanceID != CurrentSearchID) return;  //this is to cancel the search if searchTerm has changed
                    if (searchTerm.MatchNode(n)) 
                    {
                        SearchControl.InvokeInUIThread(() => SearchControl.AddResult(n));
                    }
                }                
            });
        }        

        private IEnumerable<MapNode> GetNodesToSearch(MapTree tree, bool selectedNodeHierarchy)
        {
            if (!selectedNodeHierarchy)
            {
                return tree.MapNodes;
            }
            else
            {
                return tree.SelectedNodes.ExcludeNodesAlreadyPartOfHierarchy().SelectMany(n => (n.Descendents.Concat(new[] { n })));
            }
        }        

        private void SearchControl_SearchResultAllSelected(object sender, EventArgs e)
        {
            GetCurrentMapTree().SelectedNodes.Clear();
            foreach (var n in SearchControl.Results)
            {
                if (n is MapNode node && !node.Detached)
                    node.AddToSelection();
            }
        }        
    }
}
