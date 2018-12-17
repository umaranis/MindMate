using System.Drawing;
using MindMate.Model;

namespace MindMate.View.MapControls.Drawing
{
    public interface IPainter
    {
        void DrawNode(MapNode node, bool bDrawChildren, IView iView, Graphics g);
        void DrawNodeDropHint(DropLocation location, Graphics g);
        void DrawTree(IView iView, Graphics g);
        void DrawTreeNodes(IView iView, Graphics g);
        void DrawNodeShape(MapNode node, IView iView, Graphics g);
        void DrawNodeShapeAndLinker(MapNode node, IView iView, Graphics g, bool drawChildren = true);
    }
}