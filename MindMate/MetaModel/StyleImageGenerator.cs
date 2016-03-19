using System.Drawing;
using MindMate.Model;
using MindMate.View.MapControls;
using MindMate.View.MapControls.Drawing;

namespace MindMate.MetaModel
{
    public class StyleImageGenerator : View.MapControls.Drawing.IView
    {
        private const int Width = 80;
        private const int Height = 30;
        private readonly MapNode mapNode;

        public StyleImageGenerator(MapNode node)
        {
            mapNode = node;
        }

        public Bitmap GenerateImage()
        {
            CreateNodeView();

            var image = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                MapPainter.DrawNode(mapNode, false, this, g);
                MapPainter.DrawNodeShape(mapNode, this, g);
            }
            return image;
        }

        private void CreateNodeView()
        {
            if (mapNode.NodeView == null)
            {
                mapNode.NodeView = new NodeView(mapNode);
                if (mapNode.NodeView.Width < Width || mapNode.NodeView.Height < Height)
                {
                    float left = (Width - mapNode.NodeView.Width) / 2;
                    float top = (Height - mapNode.NodeView.Height) / 2;
                    mapNode.NodeView.RefreshPosition(left, top);
                }
            }
        }

        public NodeView GetNodeView(MapNode node)
        {
            return mapNode.NodeView;
        }

        public MapTree Tree {
            get { return mapNode.Tree; }
        }

        public MapNode HighlightedNode
        {
            get { return null; }
        }
    }
}
