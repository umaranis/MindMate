using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls.Interacting
{
    public interface ITraverser
    {
        void TraverseUp(MapTree tree, bool expandSelection);
        void TraverseDown(MapTree tree, bool expandSelection);
        void TraverseRight(MapTree tree);
        void TraverseLeft(MapTree tree);
    }
}
