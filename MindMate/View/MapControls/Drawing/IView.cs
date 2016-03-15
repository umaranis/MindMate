using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MindMate.Model;

namespace MindMate.View.MapControls.Drawing
{
    public interface IView
    {
        NodeView GetNodeView(MapNode node);

        MapTree Tree { get; }

        MapNode HighlightedNode { get; }
    }
}
