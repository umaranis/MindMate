using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindMate.Model;
using MindMate.View.Search;

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
        private SearchControl SearchControl;
        private int CurrentSearchID { get; set; }
        private Func<MapTree> GetCurrentMapTree;
        private Action<Action> ScheduleParallelTask;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchControl"></param>
        /// <param name="getCurrentMapTree">delegate which returns the current MapTree</param>
        /// <param name="scheduleParallelTask">This is function is used to schedule tasks to run in a separate thread. The function ensures that the tasks are run in order and never parallel.</param>
        public SearchController(SearchControl searchControl, Func<MapTree> getCurrentMapTree, Action<Action> scheduleParallelTask)
        {
            GetCurrentMapTree = getCurrentMapTree;
            ScheduleParallelTask = scheduleParallelTask;
            this.SearchControl = searchControl;

            SearchControl.txtSearch.TextChanged += TxtSearch_TextChanged;
            SearchControl.lstResults.SelectedIndexChanged += LstResults_SelectedIndexChanged;
            SearchControl.btnSearch.Click += BtnSearch_Click;
            SearchControl.btnClear.Click += BtnClear_Click;
            SearchControl.btnSelect.Click += BtnSelect_Click;
        }        

        private void Search()
        {
            SearchTerm searchTerm = SearchControl.CreateSearchTerm();
            if (searchTerm.IsEmpty) return;
            int instanceID = ++CurrentSearchID;
            MapTree tree = GetCurrentMapTree();            
            ScheduleParallelTask(() =>
            {
                Action actClear = () => SearchControl.lstResults.Items.Clear();
                SearchControl.Invoke(actClear);                
                foreach (var n in GetNodesToSearch(tree, searchTerm.SearchSelectedHierarchy))
                {
                    if (instanceID != CurrentSearchID) return;  //this is to cancel the search if searchTerm has changed
                    if (searchTerm.MatchNode(n)) 
                    {
                        Action actAdd = () => SearchControl.lstResults.Items.Add(n);
                        SearchControl.Invoke(actAdd);
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if(SearchControl.txtSearch.Text.Length > 1)
                Search();
        }        

        private void LstResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SearchControl.lstResults.SelectedItem is MapNode node && !node.Detached)
                node.Selected = true;
            else
                SearchControl.lstResults.Items.Remove(SearchControl.lstResults.SelectedItem);

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            SearchControl.lstResults.Items.Clear();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            GetCurrentMapTree().SelectedNodes.Clear();
            foreach (var n in SearchControl.lstResults.Items)
            {
                if (n is MapNode node && !node.Detached)
                    node.AddToSelection();
            }
        }
    }
}
