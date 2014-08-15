/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MindMate.Serialization
{
    public class MindMapSerializer
    {

        /// <summary>
        /// XmlTextWriter based
        /// </summary>
        /// <returns></returns>
        public void Serialize(Stream stream, MapTree tree)
        {
            XmlTextWriter xml = new XmlTextWriter(stream, Encoding.ASCII);
            xml.WriteStartDocument();
            xml.WriteStartElement("map");
            xml.WriteAttributeString("version", "0.8.0");
            this.Serialize(tree.RootNode, xml);
            xml.WriteEndElement();
            xml.WriteEndDocument();
            xml.Flush();
            //xml.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="XMLString"></param>
        /// <returns>Return the Root Node after constructing the hierarchy</returns>
        public MapTree Deserialize(string XMLString)
        {

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(XMLString);
            XmlElement x = xmlDoc.DocumentElement;

            for (int i = 0; i < x.ChildNodes.Count; i++)
            {
                XmlNode xnode = x.ChildNodes[i];
                if (xnode.Name == "node")
                {
                    return this.Deserialize(xnode).Tree;                    
                }
            }

            return null;
        }

        /// <summary>
        /// XmlTextWriter based
        /// </summary>
        /// <param name="mapNode"></param>
        /// <returns></returns>
        public void Serialize(MapNode mapNode, XmlTextWriter xml)
        {
            xml.WriteStartElement("node");

            if (mapNode.Parent != null && mapNode.Parent.Pos == NodePosition.Root)
                xml.WriteAttributeString("POSITION", (mapNode.Pos == NodePosition.Left) ? "left" : "right");

            if (mapNode.Pos != NodePosition.Root && mapNode.Folded)
                xml.WriteAttributeString("FOLDED", "true");

            xml.WriteAttributeString("TEXT", mapNode.Text);

            xml.WriteAttributeString("CREATED", ((long)(mapNode.Created.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToString());
            xml.WriteAttributeString("MODIFIED", ((long)(mapNode.Modified.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToString());

            if (!mapNode.BackColor.IsEmpty)
                xml.WriteAttributeString("BACKGROUND_COLOR", "#" + mapNode.BackColor.R.ToString("x2") +
                    mapNode.BackColor.G.ToString("x2") + mapNode.BackColor.B.ToString("x2"));
            if (!mapNode.Color.IsEmpty)
                xml.WriteAttributeString("COLOR", "#" + mapNode.Color.R.ToString("x2") +
                    mapNode.Color.G.ToString("x2") + mapNode.Color.B.ToString("x2"));

            if (!string.IsNullOrEmpty(mapNode.Id))
                xml.WriteAttributeString("ID", mapNode.Id);

            if (mapNode.Link != null)
                xml.WriteAttributeString("LINK", mapNode.Link);

            if (mapNode.Shape != NodeShape.None)
                xml.WriteAttributeString("STYLE", Convert.ToString(mapNode.Shape));


            if (mapNode.Bold || mapNode.Italic || mapNode.FontName != null || mapNode.FontSize != 0)
            {
                xml.WriteStartElement("font");
                if (mapNode.Bold) xml.WriteAttributeString("BOLD", mapNode.Bold ? "true" : "false");
                if (mapNode.Italic) xml.WriteAttributeString("ITALIC", mapNode.Italic ? "true" : "false");
                if (mapNode.FontName != null) xml.WriteAttributeString("NAME", mapNode.FontName);
                if (mapNode.FontSize != 0) xml.WriteAttributeString("SIZE", mapNode.FontSize.ToString());
                xml.WriteEndElement();
            }

            for (var i = 0; i < mapNode.Icons.Count; i++)
            {
                xml.WriteStartElement("icon");
                xml.WriteAttributeString("BUILTIN", mapNode.Icons[i]);
                xml.WriteEndElement();
            }

            if (mapNode.LineWidth != 0 || mapNode.LinePattern != System.Drawing.Drawing2D.DashStyle.Custom ||
                !mapNode.LineColor.IsEmpty)
            {
                xml.WriteStartElement("edge");
                if (!mapNode.LineColor.IsEmpty) xml.WriteAttributeString("COLOR", "#" + mapNode.LineColor.R.ToString("x2") +
                     mapNode.LineColor.G.ToString("x2") + mapNode.LineColor.B.ToString("x2"));
                if (mapNode.LineWidth != 0) xml.WriteAttributeString("WIDTH", mapNode.LineWidth.ToString());
                if (mapNode.LinePattern != System.Drawing.Drawing2D.DashStyle.Custom)
                    xml.WriteAttributeString("PATTERN", Enum.GetName(mapNode.LinePattern.GetType(), mapNode.LinePattern).ToLower());
                xml.WriteEndElement();
            }

            if (mapNode.RichContentType != NodeRichContentType.NONE)
            {
                xml.WriteStartElement("richcontent");
                xml.WriteAttributeString("TYPE",
                    (mapNode.RichContentType == NodeRichContentType.NODE ? "NODE" : "NOTE"));
                xml.WriteRaw(mapNode.RichContentText);
                xml.WriteEndElement();
            }

            foreach (MapNode cNode in mapNode.ChildNodes)
            {
                this.Serialize(cNode, xml);
            }

            xml.WriteEndElement();
        }

        /// <summary>
        /// MapNode with it's descendent are created using 'xmlElement'
        /// </summary>
        /// <param name="xmlElement">Should be 'node' the element</param>
        /// <param name="parent">Parent to which deserialized MapNode is attached</param>
        /// <returns></returns>
        public MapNode Deserialize(XmlNode xmlElement, MapNode parent = null)
        {
            // temp for holding Xml Attributes
            XmlAttribute att;

            string text = (att = xmlElement.Attributes["TEXT"]) != null ? att.Value : "";

            string posStr = (att = xmlElement.Attributes["POSITION"]) != null ? att.Value : null;
            NodePosition pos = Convert.ToNodePosition(posStr);

            string id = (att = xmlElement.Attributes["ID"]) != null ? att.Value : null;

            MapNode node = (parent != null ? 
                new MapNode(parent, text, pos, id) : new MapNode(new MapTree(), text, id) );

            string folded;
            if ((att = xmlElement.Attributes["FOLDED"]) != null)
            {
                folded = att.Value;
                if (folded == "true")
                {
                    node.Folded = true;
                }
            }

            if ((att = xmlElement.Attributes["LINK"]) != null)
            {
                node.Link = att.Value;
            }
            
            if ((att = xmlElement.Attributes["BACKGROUND_COLOR"]) != null)
            {
                node.BackColor = (Color)new ColorConverter().ConvertFromString(
                    att.Value);
            }

            if ((att = xmlElement.Attributes["COLOR"]) != null)
            {
                node.Color = (Color)new ColorConverter().ConvertFromString(
                    att.Value);
            }

            if ((att = xmlElement.Attributes["STYLE"]) != null)
            {
                node.Shape = Convert.ToNodeStyle(att.Value);
            }

            for (var i = 0; i < xmlElement.ChildNodes.Count; i++)
            {
                XmlNode tmpXNode = xmlElement.ChildNodes[i];
                if (tmpXNode.Name == "node")
                {
                    this.Deserialize(tmpXNode, node);
                }
                else if (tmpXNode.Name == "icon")
                {
                    att = tmpXNode.Attributes["BUILTIN"];
                    if (att == null)
                    {
                        continue;
                    }
                    node.Icons.Add(att.Value);

                }
                else if (tmpXNode.Name == "font")
                {
                    if (tmpXNode.Attributes["BOLD"] != null)
                    {
                        string boldStr = tmpXNode.Attributes["BOLD"].Value;
                        if (boldStr == "true")
                        {
                            node.Bold = true;
                        }
                    }

                    if (tmpXNode.Attributes["ITALIC"] != null)
                    {
                        string italic = tmpXNode.Attributes["ITALIC"].Value;
                        if (italic == "true")
                        {
                            node.Italic = true;
                        }
                    }

                    if (tmpXNode.Attributes["NAME"] != null)
                    {
                        node.FontName = tmpXNode.Attributes["NAME"].Value;
                    }

                    if (tmpXNode.Attributes["SIZE"] != null)
                    {
                        node.FontSize = float.Parse(tmpXNode.Attributes["SIZE"].Value);
                    }
                }
                else if (tmpXNode.Name == "edge")
                {
                    if (tmpXNode.Attributes["COLOR"] != null)
                    {
                        node.LineColor = (Color)new ColorConverter().ConvertFromString(tmpXNode.Attributes["COLOR"].Value);
                    }
                    if (tmpXNode.Attributes["STYLE"] != null)
                    {
                    }
                    if (tmpXNode.Attributes["WIDTH"] != null)
                    {
                        node.LineWidth = Convert.ToLineWidth(tmpXNode.Attributes["WIDTH"].Value);
                    }
                    if (tmpXNode.Attributes["PATTERN"] != null)
                    {
                        node.LinePattern = Convert.ToDashStyle(tmpXNode.Attributes["PATTERN"].Value);
                    }
                }
                else if (tmpXNode.Name == "richcontent")
                {
                    if (tmpXNode.Attributes["TYPE"] != null)
                    {
                        string rcType = tmpXNode.Attributes["TYPE"].Value;

                        if (rcType == "NODE")
                            node.RichContentType = NodeRichContentType.NODE;
                        else
                            node.RichContentType = NodeRichContentType.NOTE;

                        node.RichContentText = tmpXNode.InnerXml;
                    }
                }
            }

            if ((att = xmlElement.Attributes["CREATED"]) != null)
            {
                node.Created =
                    (new DateTime(1970, 1, 1)).AddMilliseconds(
                    long.Parse(att.Value));
            }

            // during serialization, modified should be the last attribute to be set
            // changing other attributes will update 'modified'
            // setting it at last ensures that the right value is deserialized 
            if ((att = xmlElement.Attributes["MODIFIED"]) != null)
            {
                node.Modified =
                    (new DateTime(1970, 1, 1)).AddMilliseconds(
                    long.Parse(att.Value));
            }

            return node;
        }
    }
}
