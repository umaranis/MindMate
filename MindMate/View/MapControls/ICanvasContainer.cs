using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMate.View.MapControls
{
    public interface ICanvasContainer
    {
        void ScrollToPoint(int x, int y);
    }
}
