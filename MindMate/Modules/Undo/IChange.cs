using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Modules.Undo
{
    public interface IChange
    {
        string Description { get; }
        void Undo();
    }
}
