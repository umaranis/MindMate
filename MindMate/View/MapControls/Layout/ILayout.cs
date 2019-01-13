using MindMate.Model;
using MindMate.View.MapControls.Drawing;
using MindMate.View.MapControls.Interacting;
using System.Drawing;
using System.Windows.Forms;

namespace MindMate.View.MapControls.Layout
{
    /// <summary>
    /// Responsible for setting position of nodes. 
    /// This class should NOT do any drawing. The actual drawing is done by Painter class.
    /// </summary>
    public interface ILayout
    {
        void RefreshNodePositions();
        
        /// <summary>
        /// Returns true if successfully refreshes all node positions. If canvas is not big enough, the operation is aborted and 'false' is returned.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="sideToRefresh"></param>
        /// <returns></returns>
        void RefreshChildNodePositions(MapNode parent, NodePosition sideToRefresh);

        /// <summary>
        /// Get height of the node including child nodes
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pos"></param>        
        /// <returns></returns>
        float GetNodeHeight(MapNode node, NodePosition pos);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns>returns null if no MapNode found</returns>
        MapNode GetMapNodeFromPoint(System.Drawing.Point point);

        /// <summary>
        /// Returns Painter responsible for drawing the nodes and other elements.        
        /// </summary>
        IPainter Painter { get; }
        ITraverser Traverser { get; }

        void OnParentResize(Panel parent);
        
    }
}
