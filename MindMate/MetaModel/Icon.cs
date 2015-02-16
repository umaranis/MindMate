using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MindMate.MetaModel
{
    public interface IIcon
    {
        Bitmap Bitmap { get; }

        string Name { get; }
    }
}
