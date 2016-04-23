using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MindMate.Serialization
{
    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializedAttribute : Attribute
    {
        public int Order { get; set; }
    }
}
