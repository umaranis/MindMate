using MindMate.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void Deserialize(string text, MapNode location, string link = null)
        {
            string line;
            int indent = 0, previousIndent = 0;
            MapNode node, previousNode = null;
            StringReader str = new StringReader(text);

            while((line = str.ReadLine()) != null)
            {
                indent = GetIndent(line);
                
                if(previousNode == null)
                {
                    node = CreateNode(location, line.Trim(), link);                    
                }                
                else
                {
                    if(indent <= previousIndent)
                    {
                        int backIndent = previousIndent - indent + 1;
                        while(backIndent != 0)
                        {
                            previousNode = previousNode.Parent;
                            if (previousNode == location) break;
                            backIndent--;
                        }
                        
                    }

                    node = CreateNode(previousNode, line.Trim(), link);
                }
                previousNode = node;
                previousIndent = indent;        
            }
        }

        private int GetIndent(string line)
        {
            int indent = 0;
            while (indent < line.Length && line[indent] == '\t')
            {
                indent++;
            }
            return indent;
        }

        private MapNode CreateNode(MapNode parent, string text, string link = null)
        {
            return new MapNode(parent, text) { Link = link };
        }
    }
}
