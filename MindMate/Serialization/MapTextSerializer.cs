using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MindMate.Serialization
{
    public class MapTextSerializer
    {
        public void Serialize(MapNode mapNode, StringBuilder str, int indent = 0)
        {
            for (int i = 0; i < indent; i++) str.Append("\t");
            str.AppendLine(mapNode.Text);
            
            foreach(MapNode childNode in mapNode.ChildNodes)
            {
                Serialize(childNode, str, indent + 1);
            }
        }
    }
}
