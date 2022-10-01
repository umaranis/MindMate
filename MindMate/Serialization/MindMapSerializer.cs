/* Author: Syed Umar Anis (mail@umaranis.com)                    
 * Copyright (c) 2014 Syed Umar Anis                             
 * This software is licensed under MIT (see LICENSE.txt)    
 */

using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
            XmlTextWriter xml = new XmlTextWriter(stream, Encoding.Unicode);
            //xml.WriteStartDocument();
            xml.WriteStartElement("map");
            xml.WriteAttributeString("version", "0.9.0");
            this.SerializeFormat(tree, xml);
            this.SerializeAttributeRegistry(tree, xml);
            this.Serialize(tree.RootNode, xml);
            xml.WriteEndElement();
            //xml.WriteEndDocument();
            xml.Flush();
            //xml.Close();
        }

        public void SerializeAttributeRegistry(MapTree tree, XmlTextWriter xml)
        {
            if (tree.AttributeSpecCount > 0)
            {
                xml.WriteStartElement("attribute_registry");
                foreach(MindMate.Model.MapTree.AttributeSpec att in tree.AttributeSpecs)
                {
                    xml.WriteStartElement("attribute_name");
                    xml.WriteAttributeString("NAME", att.Name);
                    xml.WriteAttributeString("VISIBLE", att.Visible.ToString());
                    xml.WriteAttributeString("RESTRICTED", (att.ListType == MapTree.AttributeListOption.RestrictedList).ToString()); // included in listoption attribute, it is there for compatibility with freemind
                    xml.WriteAttributeString("TYPE", att.Type.ToString()); // additional to freemind
                    xml.WriteAttributeString("DATATYPE", att.DataType.ToString()); // additional to freemind
                    xml.WriteAttributeString("LISTOPTION", att.ListType.ToString()); // additional to freemind

                    if (att.ListType == MapTree.AttributeListOption.RestrictedList)
                    {
                        foreach (string v in att.Values)
                        {
                            xml.WriteStartElement("attribute_value");
                            xml.WriteAttributeString("VALUE", v);
                            xml.WriteEndElement();
                        }
                    }

                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
            }
        }

        public void SerializeFormat(MapTree tree, XmlTextWriter xml)
        {
            if (tree.DefaultFormat.HasBackColor || tree.DefaultFormat.Bold || tree.DefaultFormat.HasColor || tree.DefaultFormat.HasFontName
                || tree.DefaultFormat.HasFontSize || tree.DefaultFormat.HasItalic || tree.DefaultFormat.HasLineColor
                || tree.DefaultFormat.HasLinePattern || tree.DefaultFormat.HasLineWidth || tree.DefaultFormat.HasShape || tree.DefaultFormat.HasStrikeout)
            {
                xml.WriteStartElement("format");

                if (tree.DefaultFormat.HasBackColor) xml.WriteAttributeString("BACKGROUND_COLOR", Convert.ToString(tree.DefaultFormat.BackColor));
                if (tree.DefaultFormat.Bold) xml.WriteAttributeString("BOLD", "true");
                if (tree.DefaultFormat.HasColor) xml.WriteAttributeString("COLOR", Convert.ToString(tree.DefaultFormat.Color));
                if (tree.DefaultFormat.HasFontName) xml.WriteAttributeString("FONTNAME", tree.DefaultFormat.FontName);
                if (tree.DefaultFormat.HasFontSize) xml.WriteAttributeString("FONTSIZE", tree.DefaultFormat.FontSize.ToString());
                if (tree.DefaultFormat.HasItalic) xml.WriteAttributeString("ITALIC", "true");
                if (tree.DefaultFormat.HasLineColor) xml.WriteAttributeString("LINECOLOR", Convert.ToString(tree.DefaultFormat.LineColor));
                if (tree.DefaultFormat.HasLinePattern) xml.WriteAttributeString("PATTERN", Convert.ToString(tree.DefaultFormat.LinePattern));
                if (tree.DefaultFormat.HasLineWidth) xml.WriteAttributeString("LINEWIDTH", tree.DefaultFormat.LineWidth.ToString());
                if (tree.DefaultFormat.HasShape) xml.WriteAttributeString("STYLE", Convert.ToString(tree.DefaultFormat.Shape));
                if (tree.DefaultFormat.HasStrikeout) xml.WriteAttributeString("STRIKEOUT", "true");
                
                if (tree.HasCanvasBackColor) xml.WriteAttributeString("CANVASCOLOR", Convert.ToString(tree.CanvasBackColor));
                if (tree.HasNoteBackColor) xml.WriteAttributeString("NOTEBACKCOLOR", Convert.ToString(tree.NoteBackColor));
                if (tree.HasNoteForeColor) xml.WriteAttributeString("NOTETEXTCOLOR", Convert.ToString(tree.NoteForeColor));
                if (tree.HasSelectedOutlineColor) xml.WriteAttributeString("HIGHLIGHTCOLOR", Convert.ToString(tree.SelectedOutlineColor));
                if (tree.HasDropHintColor) xml.WriteAttributeString("DROPHINTCOLOR", Convert.ToString(tree.DropHintColor));

                xml.WriteEndElement();
            }
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

            if(mapNode.Label != null)
                xml.WriteAttributeString("LABEL", mapNode.Label);

            xml.WriteAttributeString("CREATED", ((long)(mapNode.Created.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToString());
            xml.WriteAttributeString("MODIFIED", ((long)(mapNode.Modified.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)).ToString());

            if (!mapNode.BackColor.IsEmpty)
                xml.WriteAttributeString("BACKGROUND_COLOR", Convert.ToString(mapNode.BackColor));
            if (!mapNode.Color.IsEmpty)
                xml.WriteAttributeString("COLOR", Convert.ToString(mapNode.Color));

            if (!string.IsNullOrEmpty(mapNode.Id))
                xml.WriteAttributeString("ID", mapNode.Id);

            if (mapNode.Link != null)
                xml.WriteAttributeString("LINK", mapNode.Link);

            if (mapNode.Shape != NodeShape.None)
                xml.WriteAttributeString("STYLE", Convert.ToString(mapNode.Shape));

            if (mapNode.HasImage)
                xml.WriteAttributeString("IMAGE", mapNode.Image);

            if (mapNode.HasImageAlignment)
                xml.WriteAttributeString("IMAGEALIGNMENT", mapNode.ImageAlignment.ToString());

            if(mapNode.HasImageSize)
            {
                xml.WriteAttributeString("IMAGEHEIGHT", mapNode.ImageSize.Height.ToString());
                xml.WriteAttributeString("IMAGEWIDTH", mapNode.ImageSize.Width.ToString());
            }


            if (mapNode.Bold || mapNode.Italic || mapNode.Strikeout || mapNode.FontName != null || mapNode.FontSize != 0)
            {
                xml.WriteStartElement("font");
                if (mapNode.Bold) xml.WriteAttributeString("BOLD", mapNode.Bold ? "true" : "false");
                if (mapNode.Italic) xml.WriteAttributeString("ITALIC", mapNode.Italic ? "true" : "false");
                if (mapNode.Strikeout) xml.WriteAttributeString("STRIKEOUT", mapNode.Strikeout ? "true" : "false");
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
                if (!mapNode.LineColor.IsEmpty) xml.WriteAttributeString("COLOR", Convert.ToString(mapNode.LineColor));
                if (mapNode.LineWidth != 0) xml.WriteAttributeString("WIDTH", mapNode.LineWidth.ToString());
                if (mapNode.LinePattern != System.Drawing.Drawing2D.DashStyle.Custom)
                    xml.WriteAttributeString("PATTERN", Enum.GetName(mapNode.LinePattern.GetType(), mapNode.LinePattern).ToLower());
                xml.WriteEndElement();
            }

            if (mapNode.HasNoteText)
            {
                xml.WriteStartElement("richcontent");
                xml.WriteAttributeString("TYPE", "NOTE");
                xml.WriteString(mapNode.NoteText);
                xml.WriteEndElement();
            }

            if (mapNode.Attributes != null)
            {
                foreach (MapNode.Attribute a in mapNode.Attributes)
                {
                    xml.WriteStartElement("attribute");
                    xml.WriteAttributeString("NAME", a.AttributeSpec.Name);
                    xml.WriteAttributeString("VALUE", a.ValueString);
                    xml.WriteEndElement();
                }
            }

            foreach (MapNode cNode in mapNode.ChildNodes)
            {
                this.Serialize(cNode, xml);
            }

            xml.WriteEndElement();
        }

        /// <summary>
        /// </summary>
        /// <param name="XMLString"></param>
        /// <param name="tree"></param>
        /// <returns>Return the Root Node after constructing the hierarchy</returns>
        public void Deserialize(string XMLString, MapTree tree)
        {
            tree.Deserializing = true;

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument(); //TODO: Use XmlTextReader instead to speed up the code
            xmlDoc.LoadXml(XMLString);
            XmlElement x = xmlDoc.DocumentElement;

            for (int i = 0; i < x.ChildNodes.Count; i++)
            {
                XmlNode xnode = x.ChildNodes[i];
                if(xnode.Name == "format")
                {
                    this.DeserializeFormat(xnode, tree);
                }
                else if (xnode.Name == "attribute_registry")
                {
                    this.DeserializeAttributeRegistry(xnode, tree);
                }
                else if (xnode.Name == "node")
                {
                    this.Deserialize(xnode, tree);
                }
            }

            tree.Deserializing = false;
        }

        public void DeserializeAttributeRegistry(XmlNode xmlAttrRegistry, MapTree tree)
        {
            foreach(XmlNode xmlAttribute in xmlAttrRegistry)
            {
                if(xmlAttribute.Name == "attribute_name")
                {
                    var attr = xmlAttribute.Attributes["NAME"];
                    string attrName = attr != null ? attr.Value : null;

                    attr = xmlAttribute.Attributes["VISIBLE"];
                    bool visible = attr != null ? bool.Parse(attr.Value) : true;

                    attr = xmlAttribute.Attributes["TYPE"];
                    MapTree.AttributeType type = (MapTree.AttributeType) (attr != null ? Enum.Parse(typeof(MapTree.AttributeType), attr.Value) : MapTree.AttributeType.UserDefined);

                    attr = xmlAttribute.Attributes["DATATYPE"];
                    MapTree.AttributeDataType dataType = (MapTree.AttributeDataType)
                        (attr != null ? 
                            Enum.Parse(typeof(MapTree.AttributeDataType), attr.Value) : 
                            MapTree.AttributeDataType.Alphanumeric);

                    attr = xmlAttribute.Attributes["LISTOPTION"];
                    MapTree.AttributeListOption listOption = (MapTree.AttributeListOption)
                        (attr != null ?
                        Enum.Parse(typeof(MapTree.AttributeListOption), attr.Value) :
                        MapTree.AttributeListOption.OptionalList);

                    SortedSet<string> attrValues = null;                    
                    if(listOption != MapTree.AttributeListOption.NoList)
                    {
                        attrValues = new SortedSet<string>();

                        foreach(XmlNode xmlValue in xmlAttribute.ChildNodes)
                        {
                            if(xmlValue.Name == "attribute_value")
                            {
                                attr = xmlValue.Attributes["VALUE"];
                                if(attr != null)
                                    attrValues.Add(attr.Value);
                            }
                        }
                    }

                    new MapTree.AttributeSpec(tree, attrName, visible, dataType, listOption, attrValues, type);
                }
            }
        }

        public void DeserializeFormat(XmlNode xmlNode, MapTree tree)
        {
            XmlAttribute att;

            Color backColor = (att = xmlNode.Attributes["BACKGROUND_COLOR"]) != null ? Convert.ToColor(att.Value) : NodeFormat.DefaultBackColor;
            bool bold = (att = xmlNode.Attributes["BOLD"]) != null ? Convert.ToBool(att.Value) : false;
            Color color = (att = xmlNode.Attributes["COLOR"]) != null ? Convert.ToColor(att.Value) : NodeFormat.DefaultColor;
            string fontName = (att = xmlNode.Attributes["FONTNAME"]) != null ? att.Value : NodeFormat.DefaultFontName;
            float fontSize = (att = xmlNode.Attributes["FONTSIZE"]) != null ? float.Parse(att.Value) : NodeFormat.DefaultFontSize;
            bool italic = (att = xmlNode.Attributes["ITALIC"]) != null ? Convert.ToBool(att.Value) : false;
            Color lineColor = (att = xmlNode.Attributes["LINECOLOR"]) != null ? Convert.ToColor(att.Value) : NodeFormat.DefaultLineColor;
            DashStyle linePattern = (att = xmlNode.Attributes["PATTERN"]) != null ? Convert.ToDashStyle(att.Value) : NodeFormat.DefaultLinePattern;
            int lineWidth = (att = xmlNode.Attributes["LINEWIDTH"]) != null ? int.Parse(att.Value) : NodeFormat.DefaultLineWidth;
            NodeShape shape = (att = xmlNode.Attributes["STYPE"]) != null ? Convert.ToNodeStyle(att.Value) : NodeFormat.DefaultNodeShape;
            bool strikeout = (att = xmlNode.Attributes["STRIKEOUT"]) != null ? Convert.ToBool(att.Value) : false;
            
            if ((att = xmlNode.Attributes["CANVASCOLOR"]) != null) { tree.CanvasBackColor = Convert.ToColor(att.Value); }
            if ((att = xmlNode.Attributes["NOTEBACKCOLOR"]) != null) { tree.NoteBackColor = Convert.ToColor(att.Value); }
            if ((att = xmlNode.Attributes["NOTETEXTCOLOR"]) != null) { tree.NoteForeColor = Convert.ToColor(att.Value); }
            if ((att = xmlNode.Attributes["HIGHLIGHTCOLOR"]) != null) { tree.SelectedOutlineColor = Convert.ToColor(att.Value); }
            if ((att = xmlNode.Attributes["DROPHINTCOLOR"]) != null) { tree.DropHintColor = Convert.ToColor(att.Value); }

            var format = new NodeFormat(backColor, bold, color, fontName, fontSize, italic, strikeout, lineColor, linePattern, lineWidth, shape);
            tree.DefaultFormat = format;
        }

        /// <summary>
        /// MapNode with it's descendent are created using 'xmlElement'
        /// </summary>
        /// <param name="xmlElement">Should be 'node' the element</param>
        /// <param name="tree"></param>
        /// <param name="parent">Parent to which deserialized MapNode is attached</param>
        /// <returns></returns>
        public MapNode Deserialize(XmlNode xmlElement, MapTree tree, MapNode parent = null)
        {
            // temp for holding Xml Attributes
            XmlAttribute att;

            string text = (att = xmlElement.Attributes["TEXT"]) != null ? att.Value : "";

            string posStr = (att = xmlElement.Attributes["POSITION"]) != null ? att.Value : null;
            NodePosition pos = Convert.ToNodePosition(posStr);

            string id = (att = xmlElement.Attributes["ID"]) != null ? att.Value : null;

            MapNode node = (parent != null ? 
                new MapNode(parent, text, pos, id) : new MapNode(tree, text, id) );

            if ((att = xmlElement.Attributes["LABEL"]) != null)
            {
                node.Label = att.Value;
            }

            if ((att = xmlElement.Attributes["FOLDED"]) != null)
            {
                var folded = att.Value;
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

            if((att = xmlElement.Attributes["IMAGE"]) != null)
            { 
                node.Image = att.Value;
            }

            if((att = xmlElement.Attributes["IMAGEALIGNMENT"]) != null)
            {
                node.ImageAlignment = (ImageAlignment)Enum.Parse(typeof(ImageAlignment), att.Value, true);
            }

            if ((att = xmlElement.Attributes["IMAGEHEIGHT"]) != null)
            {
                node.ImageSize = new Size(node.ImageSize.Width, int.Parse(att.Value));
            }

            if ((att = xmlElement.Attributes["IMAGEWIDTH"]) != null)
            {
                node.ImageSize = new Size(int.Parse(att.Value), node.ImageSize.Height);
            }

            for (var i = 0; i < xmlElement.ChildNodes.Count; i++)
            {
                XmlNode tmpXNode = xmlElement.ChildNodes[i];
                if (tmpXNode.Name == "node")
                {
                    this.Deserialize(tmpXNode, tree, node);
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

                    if (tmpXNode.Attributes["STRIKEOUT"] != null)
                    {
                        node.Strikeout = (tmpXNode.Attributes["STRIKEOUT"].Value == "true");
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

                        //if (rcType == "NODE")
                        //    node.RichContentType = NodeRichContentType.NODE;
                        //else
                        //    node.RichContentType = NodeRichContentType.NOTE;
                        
                        //all rich content types are loaded as NOTE. If there are more than one on same node (FreeMind allows this), all merged into single note.

                        if (node.NoteText == null)
                        {
                            node.NoteText = tmpXNode.InnerText;
                        }
                        else
                        {
                            node.NoteText = Environment.NewLine + Environment.NewLine + "---" + Environment.NewLine + tmpXNode.InnerText;
                        }
                    }
                }                
                else if(tmpXNode.Name == "attribute")
                {
                    string attibuteName = tmpXNode.Attributes["NAME"].Value;
                    string value = null;

                    XmlAttribute xmlAtt = tmpXNode.Attributes["VALUE"];

                    if (xmlAtt != null)
                    {
                        value = xmlAtt.Value;
                    }

                    node.AddAttribute(attibuteName, value);
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
